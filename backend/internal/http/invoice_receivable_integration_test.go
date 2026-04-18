//go:build integration

package http_test

import (
	"bytes"
	"context"
	"database/sql"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"os"
	"strconv"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/config"
	httpapi "github.com/Gujiaweiguo/mi/backend/internal/http"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"go.uber.org/zap"
)

func TestIntegrationInvoiceReceivableRoutes(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))

	router := httpapi.NewRouter(&config.Config{App: config.AppConfig{Name: "mi-backend", Environment: "test"}, Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600}}, db, zap.NewNop())
	token := loginAsAdmin(t, router)
	documentID := seedApprovedInvoiceForReceivableRoutes(t, ctx, db)

	receivableRecorder := httptest.NewRecorder()
	receivableRequest := httptest.NewRequest(http.MethodGet, "/api/invoices/"+strconv.FormatInt(documentID, 10)+"/receivable", nil)
	receivableRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(receivableRecorder, receivableRequest)
	if receivableRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from receivable detail endpoint, got %d body=%s", receivableRecorder.Code, receivableRecorder.Body.String())
	}
	var receivableBody struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			SettlementStatus  string  `json:"settlement_status"`
			Items             []struct {
				ChargeType string `json:"charge_type"`
			} `json:"items"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(receivableRecorder.Body.Bytes(), &receivableBody); err != nil {
		t.Fatalf("decode receivable detail response: %v", err)
	}
	if receivableBody.Receivable.OutstandingAmount != 12000 || receivableBody.Receivable.SettlementStatus != "outstanding" || len(receivableBody.Receivable.Items) != 1 {
		t.Fatalf("expected open receivable payload, got body=%s", receivableRecorder.Body.String())
	}

	listRecorder := httptest.NewRecorder()
	listRequest := httptest.NewRequest(http.MethodGet, "/api/receivables?customer_id=101&department_id=101&due_date_start=2026-04-01&due_date_end=2026-04-30", nil)
	listRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(listRecorder, listRequest)
	if listRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from receivable list endpoint, got %d body=%s", listRecorder.Code, listRecorder.Body.String())
	}
	var listBody struct {
		Items []struct {
			BillingDocumentID int64   `json:"billing_document_id"`
			OutstandingAmount float64 `json:"outstanding_amount"`
		} `json:"items"`
	}
	if err := json.Unmarshal(listRecorder.Body.Bytes(), &listBody); err != nil {
		t.Fatalf("decode receivable list response: %v", err)
	}
	if len(listBody.Items) != 1 || listBody.Items[0].BillingDocumentID != documentID || listBody.Items[0].OutstandingAmount != 12000 {
		t.Fatalf("expected one outstanding receivable row, got body=%s", listRecorder.Body.String())
	}

	paymentRecorder := httptest.NewRecorder()
	paymentRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(documentID, 10)+"/payments", bytes.NewBufferString(`{"amount":7000,"payment_date":"2026-04-20","idempotency_key":"route-payment-1","note":"partial payment"}`))
	paymentRequest.Header.Set("Content-Type", "application/json")
	paymentRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(paymentRecorder, paymentRequest)
	if paymentRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from payment endpoint, got %d body=%s", paymentRecorder.Code, paymentRecorder.Body.String())
	}
	var paymentBody struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			PaymentHistory    []struct {
				Amount float64 `json:"amount"`
			} `json:"payment_history"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(paymentRecorder.Body.Bytes(), &paymentBody); err != nil {
		t.Fatalf("decode payment response: %v", err)
	}
	if paymentBody.Receivable.OutstandingAmount != 5000 || len(paymentBody.Receivable.PaymentHistory) != 1 || paymentBody.Receivable.PaymentHistory[0].Amount != 7000 {
		t.Fatalf("expected payment endpoint to reduce outstanding balance, got body=%s", paymentRecorder.Body.String())
	}

	overpayRecorder := httptest.NewRecorder()
	overpayRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(documentID, 10)+"/payments", bytes.NewBufferString(`{"amount":6000,"payment_date":"2026-04-21","idempotency_key":"route-payment-2","note":"too much"}`))
	overpayRequest.Header.Set("Content-Type", "application/json")
	overpayRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(overpayRecorder, overpayRequest)
	if overpayRecorder.Code != http.StatusConflict {
		t.Fatalf("expected 409 from over-application payment endpoint, got %d body=%s", overpayRecorder.Code, overpayRecorder.Body.String())
	}
}

func seedApprovedInvoiceForReceivableRoutes(t *testing.T, ctx context.Context, db *sql.DB) int64 {
	t.Helper()
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)
	activateRouteLease(t, ctx, leaseService, workflowService, routeLeaseInput("ROUTE-I101", 12000), "route-submit-i101")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "route-submit-invoice-101", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "route-approve-invoice-101", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync invoice workflow state: %v", err)
	}
	return document.ID
}

func loginAsAdmin(t *testing.T, router http.Handler) string {
	t.Helper()
	loginRecorder := httptest.NewRecorder()
	loginRequest := httptest.NewRequest(http.MethodPost, "/api/auth/login", bytes.NewBufferString(`{"username":"admin","password":"password"}`))
	loginRequest.Header.Set("Content-Type", "application/json")
	router.ServeHTTP(loginRecorder, loginRequest)
	if loginRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from login, got %d body=%s", loginRecorder.Code, loginRecorder.Body.String())
	}
	var loginBody struct {
		Token string `json:"token"`
	}
	if err := json.Unmarshal(loginRecorder.Body.Bytes(), &loginBody); err != nil {
		t.Fatalf("decode login response: %v", err)
	}
	if loginBody.Token == "" {
		t.Fatal("expected login token")
	}
	return loginBody.Token
}

func routeLeaseInput(leaseNo string, amount float64) lease.CreateDraftInput {
	start := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	end := time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)
	buildingID := int64(101)
	customerID := int64(101)
	brandID := int64(101)
	tradeID := int64(102)
	managementTypeID := int64(101)
	return lease.CreateDraftInput{LeaseNo: leaseNo, DepartmentID: 101, StoreID: 101, BuildingID: &buildingID, CustomerID: &customerID, BrandID: &brandID, TradeID: &tradeID, ManagementTypeID: &managementTypeID, TenantName: "Route Tenant", StartDate: start, EndDate: end, Units: []lease.UnitInput{{UnitID: 101, RentArea: 118}}, Terms: []lease.TermInput{{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: amount, EffectiveFrom: start, EffectiveTo: end}}, ActorUserID: 101}
}

func activateRouteLease(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service, input lease.CreateDraftInput, submitKey string) *lease.Contract {
	t.Helper()
	draft, err := leaseService.CreateDraft(ctx, input)
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey, Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey + "-step1", Comment: "approve lease step1"})
	if err != nil {
		t.Fatalf("approve lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey + "-step2", Comment: "approve lease step2"})
	if err != nil {
		t.Fatalf("approve lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 2: %v", err)
	}
	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get active lease: %v", err)
	}
	return active
}
