//go:build integration

package http_test

import (
	"bytes"
	"context"
	"encoding/json"
	"fmt"
	"net/http"
	"net/http/httptest"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	httpapi "github.com/Gujiaweiguo/mi/backend/internal/http"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	_ "github.com/go-sql-driver/mysql"
	"go.uber.org/zap"
)

func TestE2ELeaseToInvoiceSmoke(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	router := httpapi.NewRouter(&config.Config{
		App:  config.AppConfig{Name: "mi-backend", Environment: "test"},
		Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600},
	}, db, zap.NewNop())

	token := loginAsAdmin(t, router)
	authHeader := "Bearer " + token

	// === Step 1: Create lease (draft) ===
	createBody := `{
		"lease_no": "CON-E2E-001",
		"department_id": 101,
		"store_id": 101,
		"building_id": 101,
		"customer_id": 101,
		"brand_id": 101,
		"trade_id": 102,
		"management_type_id": 101,
		"tenant_name": "ACME Retail E2E",
		"start_date": "2026-04-01",
		"end_date": "2027-03-31",
		"units": [{"unit_id": 101, "rent_area": 118}],
		"terms": [{
			"term_type": "rent",
			"billing_cycle": "monthly",
			"currency_type_id": 101,
			"amount": 12000,
			"effective_from": "2026-04-01",
			"effective_to": "2027-03-31"
		}]
	}`

	w := httptest.NewRecorder()
	req := httptest.NewRequest(http.MethodPost, "/api/leases", bytes.NewBufferString(createBody))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusCreated {
		t.Fatalf("step1 create lease: expected 201, got %d body=%s", w.Code, w.Body.String())
	}

	var leaseResp struct {
		Lease struct {
			ID                 int64  `json:"id"`
			Status             string `json:"status"`
			WorkflowInstanceID *int64 `json:"workflow_instance_id"`
		} `json:"lease"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &leaseResp)

	if leaseResp.Lease.Status != "draft" {
		t.Fatalf("step1: expected draft, got %s", leaseResp.Lease.Status)
	}
	leaseID := leaseResp.Lease.ID
	t.Logf("step1: created lease id=%d status=draft", leaseID)

	// === Step 2: Submit lease for approval ===
	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodPost, fmt.Sprintf("/api/leases/%d/submit", leaseID),
		bytes.NewBufferString(`{"idempotency_key":"e2e-submit-lease","comment":"E2E test submit"}`))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusOK {
		t.Fatalf("step2 submit lease: expected 200, got %d body=%s", w.Code, w.Body.String())
	}

	mustDecodeJSON(t, w.Body.Bytes(), &leaseResp)

	if leaseResp.Lease.Status != "pending_approval" {
		t.Fatalf("step2: expected pending_approval, got %s", leaseResp.Lease.Status)
	}
	if leaseResp.Lease.WorkflowInstanceID == nil {
		t.Fatal("step2: expected workflow_instance_id to be set")
	}
	workflowInstanceID := *leaseResp.Lease.WorkflowInstanceID
	t.Logf("step2: submitted lease, workflow_instance_id=%d", workflowInstanceID)

	// === Step 3: Approve lease workflow step 1 (manager) ===
	w = approveWorkflow(t, router, authHeader, workflowInstanceID, "e2e-approve-lease-step1", "manager approved")
	var wfResp1 struct {
		Instance struct {
			ID     int64  `json:"id"`
			Status string `json:"status"`
		} `json:"instance"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &wfResp1)
	t.Logf("step3: workflow status after manager approve = %s", wfResp1.Instance.Status)

	// === Step 4: Approve lease workflow step 2 (finance) ===
	w = approveWorkflow(t, router, authHeader, workflowInstanceID, "e2e-approve-lease-step2", "finance approved")
	var wfResp2 struct {
		Instance struct {
			ID     int64  `json:"id"`
			Status string `json:"status"`
		} `json:"instance"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &wfResp2)
	t.Logf("step4: workflow status after finance approve = %s", wfResp2.Instance.Status)

	// Verify lease is now active
	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodGet, fmt.Sprintf("/api/leases/%d", leaseID), nil)
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)
	mustDecodeJSON(t, w.Body.Bytes(), &leaseResp)

	if leaseResp.Lease.Status != "active" {
		t.Fatalf("step4: expected lease active, got %s", leaseResp.Lease.Status)
	}
	t.Logf("step4: lease is active ✓")

	// === Step 5: Generate charges ===
	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodPost, "/api/billing/charges/generate",
		bytes.NewBufferString(`{"period_start":"2026-04-01","period_end":"2026-04-30"}`))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusCreated {
		t.Fatalf("step5 generate charges: expected 201, got %d body=%s", w.Code, w.Body.String())
	}

	var chargeResp struct {
		Lines []struct {
			ID     int64   `json:"id"`
			Amount float64 `json:"amount"`
		} `json:"lines"`
		Totals struct {
			Generated int `json:"generated"`
			Skipped   int `json:"skipped"`
		} `json:"totals"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &chargeResp)

	if chargeResp.Totals.Generated == 0 {
		t.Fatal("step5: expected at least 1 generated charge line")
	}
	chargeLineID := chargeResp.Lines[0].ID
	t.Logf("step5: generated %d charges, first line id=%d amount=%.2f",
		chargeResp.Totals.Generated, chargeLineID, chargeResp.Lines[0].Amount)

	// === Step 6: Create invoice from charges ===
	invoiceBody := fmt.Sprintf(`{
		"document_type": "invoice",
		"billing_charge_line_ids": [%d]
	}`, chargeLineID)

	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodPost, "/api/invoices", bytes.NewBufferString(invoiceBody))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusCreated {
		t.Fatalf("step6 create invoice: expected 201, got %d body=%s", w.Code, w.Body.String())
	}

	var invoiceResp struct {
		Document struct {
			ID                 int64   `json:"id"`
			Status             string  `json:"status"`
			TotalAmount        float64 `json:"total_amount"`
			WorkflowInstanceID *int64  `json:"workflow_instance_id"`
		} `json:"document"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &invoiceResp)

	if invoiceResp.Document.Status != "draft" {
		t.Fatalf("step6: expected draft invoice, got %s", invoiceResp.Document.Status)
	}
	invoiceID := invoiceResp.Document.ID
	t.Logf("step6: created invoice id=%d status=draft total=%.2f", invoiceID, invoiceResp.Document.TotalAmount)

	// === Step 7: Submit invoice for approval ===
	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodPost, fmt.Sprintf("/api/invoices/%d/submit", invoiceID),
		bytes.NewBufferString(`{"idempotency_key":"e2e-submit-invoice","comment":"E2E test submit invoice"}`))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusOK {
		t.Fatalf("step7 submit invoice: expected 200, got %d body=%s", w.Code, w.Body.String())
	}

	mustDecodeJSON(t, w.Body.Bytes(), &invoiceResp)

	if invoiceResp.Document.Status != "pending_approval" {
		t.Fatalf("step7: expected pending_approval, got %s", invoiceResp.Document.Status)
	}
	if invoiceResp.Document.WorkflowInstanceID == nil {
		t.Fatal("step7: expected workflow_instance_id on invoice")
	}
	invoiceWorkflowID := *invoiceResp.Document.WorkflowInstanceID
	t.Logf("step7: submitted invoice, workflow_instance_id=%d", invoiceWorkflowID)

	// === Step 8: Approve invoice workflow (1 step) ===
	w = approveWorkflow(t, router, authHeader, invoiceWorkflowID, "e2e-approve-invoice", "finance approved invoice")
	t.Logf("step8: invoice approved ✓")

	// === Step 9: Record payment ===
	paymentBody := fmt.Sprintf(`{
		"amount": %.2f,
		"payment_date": "2026-04-20",
		"idempotency_key": "e2e-payment-1",
		"note": "E2E full payment"
	}`, invoiceResp.Document.TotalAmount)

	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodPost, fmt.Sprintf("/api/invoices/%d/payments", invoiceID),
		bytes.NewBufferString(paymentBody))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusOK {
		t.Fatalf("step9 record payment: expected 200, got %d body=%s", w.Code, w.Body.String())
	}

	var paymentResp struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			SettlementStatus  string  `json:"settlement_status"`
		} `json:"receivable"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &paymentResp)

	if paymentResp.Receivable.SettlementStatus != "settled" {
		t.Fatalf("step9: expected settled, got %s outstanding=%.2f",
			paymentResp.Receivable.SettlementStatus, paymentResp.Receivable.OutstandingAmount)
	}
	t.Logf("step9: payment recorded, receivable settled ✓")

	// === Final: Verify receivable via GET endpoint ===
	w = httptest.NewRecorder()
	req = httptest.NewRequest(http.MethodGet, fmt.Sprintf("/api/invoices/%d/receivable", invoiceID), nil)
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusOK {
		t.Fatalf("final: expected 200 from receivable detail, got %d body=%s", w.Code, w.Body.String())
	}

	var receivableDetail struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			SettlementStatus  string  `json:"settlement_status"`
		} `json:"receivable"`
	}
	mustDecodeJSON(t, w.Body.Bytes(), &receivableDetail)

	if receivableDetail.Receivable.OutstandingAmount != 0 {
		t.Fatalf("final: expected outstanding_amount=0, got %.2f", receivableDetail.Receivable.OutstandingAmount)
	}
	if receivableDetail.Receivable.SettlementStatus != "settled" {
		t.Fatalf("final: expected settlement_status=settled, got %s", receivableDetail.Receivable.SettlementStatus)
	}

	t.Log("=== E2E SMOKE TEST PASSED: Lease→Approval→Charge→Invoice→Approval→Payment→Settled ✓ ===")
}

// approveWorkflow is a helper that approves a workflow instance.
func approveWorkflow(t *testing.T, router http.Handler, authHeader string, instanceID int64, idempotencyKey, comment string) *httptest.ResponseRecorder {
	t.Helper()
	body := fmt.Sprintf(`{"idempotency_key":"%s","comment":"%s"}`, idempotencyKey, comment)
	w := httptest.NewRecorder()
	req := httptest.NewRequest(http.MethodPost,
		fmt.Sprintf("/api/workflow/instances/%d/approve", instanceID),
		bytes.NewBufferString(body))
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", authHeader)
	router.ServeHTTP(w, req)
	if w.Code != http.StatusOK {
		t.Fatalf("approveWorkflow(%d): expected 200, got %d body=%s", instanceID, w.Code, w.Body.String())
	}
	return w
}

// mustDecodeJSON decodes JSON bytes into v or fails the test.
func mustDecodeJSON(t *testing.T, data []byte, v any) {
	t.Helper()
	if err := json.Unmarshal(data, v); err != nil {
		t.Fatalf("decode JSON: %v, body=%s", err, string(data))
	}
}
