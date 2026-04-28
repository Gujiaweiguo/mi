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
	primary := seedApprovedInvoiceForReceivableRoutes(t, ctx, db, "ROUTE-I101", 12000, "route-submit-i101", "route-invoice-101")
	secondary := seedApprovedInvoiceForReceivableRoutes(t, ctx, db, "ROUTE-I102", 12000, "route-submit-i102", "route-invoice-102")
	documentID := primary.documentID
	lineID := primary.lineID

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
	if len(listBody.Items) != 2 {
		t.Fatalf("expected two outstanding receivable rows, got body=%s", listRecorder.Body.String())
	}
	if listBody.Items[0].BillingDocumentID != documentID || listBody.Items[0].OutstandingAmount != 12000 {
		t.Fatalf("expected primary outstanding receivable row first, got body=%s", listRecorder.Body.String())
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

	discountRecorder := httptest.NewRecorder()
	discountRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(documentID, 10)+"/discounts", bytes.NewBufferString(`{"billing_document_line_id":`+strconv.FormatInt(lineID, 10)+`,"amount":2000,"reason":"route discount","idempotency_key":"route-discount-1"}`))
	discountRequest.Header.Set("Content-Type", "application/json")
	discountRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(discountRecorder, discountRequest)
	if discountRecorder.Code != http.StatusConflict && discountRecorder.Code != http.StatusOK {
		t.Fatalf("expected discount endpoint to return a handled response, got %d body=%s", discountRecorder.Code, discountRecorder.Body.String())
	}

	overpayRecorder := httptest.NewRecorder()
	overpayRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(documentID, 10)+"/payments", bytes.NewBufferString(`{"amount":6000,"payment_date":"2026-04-21","idempotency_key":"route-payment-2","note":"too much"}`))
	overpayRequest.Header.Set("Content-Type", "application/json")
	overpayRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(overpayRecorder, overpayRequest)
	if overpayRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from over-application payment endpoint, got %d body=%s", overpayRecorder.Code, overpayRecorder.Body.String())
	}
	var overpayBody struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			CustomerSurplus   float64 `json:"customer_surplus_available"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(overpayRecorder.Body.Bytes(), &overpayBody); err != nil {
		t.Fatalf("decode overpayment response: %v", err)
	}
	if overpayBody.Receivable.OutstandingAmount != 0 || overpayBody.Receivable.CustomerSurplus != 1000 {
		t.Fatalf("expected overpayment to settle receivable and create surplus, got body=%s", overpayRecorder.Body.String())
	}

	surplusRecorder := httptest.NewRecorder()
	surplusRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(secondary.documentID, 10)+"/surplus-applications", bytes.NewBufferString(`{"billing_document_line_id":`+strconv.FormatInt(secondary.lineID, 10)+`,"amount":1000,"note":"apply surplus","idempotency_key":"route-surplus-1"}`))
	surplusRequest.Header.Set("Content-Type", "application/json")
	surplusRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(surplusRecorder, surplusRequest)
	if surplusRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from surplus application endpoint, got %d body=%s", surplusRecorder.Code, surplusRecorder.Body.String())
	}
	var surplusBody struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			CustomerSurplus   float64 `json:"customer_surplus_available"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(surplusRecorder.Body.Bytes(), &surplusBody); err != nil {
		t.Fatalf("decode surplus response: %v", err)
	}
	if surplusBody.Receivable.OutstandingAmount != 11000 || surplusBody.Receivable.CustomerSurplus != 0 {
		t.Fatalf("expected surplus application to reduce receivable and consume surplus, got body=%s", surplusRecorder.Body.String())
	}

	interestRecorder := httptest.NewRecorder()
	interestRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(secondary.documentID, 10)+"/interest", bytes.NewBufferString(`{"billing_document_line_id":`+strconv.FormatInt(secondary.lineID, 10)+`,"as_of_date":"2026-05-10","idempotency_key":"route-interest-1"}`))
	interestRequest.Header.Set("Content-Type", "application/json")
	interestRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(interestRecorder, interestRequest)
	if interestRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from interest generation endpoint, got %d body=%s", interestRecorder.Code, interestRecorder.Body.String())
	}
	var interestBody struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
			InterestHistory   []struct {
				InterestAmount float64 `json:"interest_amount"`
			} `json:"interest_history"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(interestRecorder.Body.Bytes(), &interestBody); err != nil {
		t.Fatalf("decode interest response: %v", err)
	}
	if interestBody.Receivable.OutstandingAmount != 11000 || len(interestBody.Receivable.InterestHistory) != 1 || interestBody.Receivable.InterestHistory[0].InterestAmount != 33 {
		t.Fatalf("expected interest generation to add auditable history without mutating source outstanding, got body=%s", interestRecorder.Body.String())
	}

	depositSource := seedApprovedDepositInvoiceForReceivableRoutes(t, ctx, db, "ROUTE-D101", 5000, "route-submit-d101", "route-deposit-101")
	depositApplyRecorder := httptest.NewRecorder()
	depositApplyRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(depositSource.documentID, 10)+"/deposit-applications", bytes.NewBufferString(`{"billing_document_line_id":`+strconv.FormatInt(depositSource.lineID, 10)+`,"target_document_id":`+strconv.FormatInt(secondary.documentID, 10)+`,"target_billing_document_line_id":`+strconv.FormatInt(secondary.lineID, 10)+`,"amount":1000,"note":"apply deposit","idempotency_key":"route-deposit-apply-1"}`))
	depositApplyRequest.Header.Set("Content-Type", "application/json")
	depositApplyRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(depositApplyRecorder, depositApplyRequest)
	if depositApplyRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from deposit application endpoint, got %d body=%s", depositApplyRecorder.Code, depositApplyRecorder.Body.String())
	}
	var depositApplyBody struct {
		Receivable struct {
			OutstandingAmount float64 `json:"outstanding_amount"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(depositApplyRecorder.Body.Bytes(), &depositApplyBody); err != nil {
		t.Fatalf("decode deposit application response: %v", err)
	}
	if depositApplyBody.Receivable.OutstandingAmount != 10000 {
		t.Fatalf("expected deposit application to reduce target outstanding to 10000, got body=%s", depositApplyRecorder.Body.String())
	}

	depositRefundSeed := seedApprovedDepositInvoiceForReceivableRoutes(t, ctx, db, "ROUTE-D102", 5000, "route-submit-d102", "route-deposit-102")
	depositRefundRecorder := httptest.NewRecorder()
	depositRefundRequest := httptest.NewRequest(http.MethodPost, "/api/invoices/"+strconv.FormatInt(depositRefundSeed.documentID, 10)+"/deposit-refunds", bytes.NewBufferString(`{"billing_document_line_id":`+strconv.FormatInt(depositRefundSeed.lineID, 10)+`,"amount":5000,"reason":"lease ended","idempotency_key":"route-deposit-refund-1"}`))
	depositRefundRequest.Header.Set("Content-Type", "application/json")
	depositRefundRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(depositRefundRecorder, depositRefundRequest)
	if depositRefundRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from deposit refund endpoint, got %d body=%s", depositRefundRecorder.Code, depositRefundRecorder.Body.String())
	}
	var depositRefundBody struct {
		Receivable struct {
			OutstandingAmount   float64 `json:"outstanding_amount"`
			DepositRefundHistory []struct {
				Amount float64 `json:"amount"`
			} `json:"deposit_refund_history"`
		} `json:"receivable"`
	}
	if err := json.Unmarshal(depositRefundRecorder.Body.Bytes(), &depositRefundBody); err != nil {
		t.Fatalf("decode deposit refund response: %v", err)
	}
	if depositRefundBody.Receivable.OutstandingAmount != 0 || len(depositRefundBody.Receivable.DepositRefundHistory) != 1 || depositRefundBody.Receivable.DepositRefundHistory[0].Amount != 5000 {
		t.Fatalf("expected deposit refund to settle deposit and record history, got body=%s", depositRefundRecorder.Body.String())
	}
}

type routeInvoiceSeed struct {
	documentID int64
	lineID     int64
}

func seedApprovedInvoiceForReceivableRoutes(t *testing.T, ctx context.Context, db *sql.DB, leaseNo string, amount float64, submitKey string, invoiceKey string) routeInvoiceSeed {
	t.Helper()
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)
	activeLease := activateRouteLease(t, ctx, leaseService, workflowService, routeLeaseInput(leaseNo, amount), submitKey)
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	var targetLineID int64
	for _, line := range charges.Lines {
		if line.LeaseContractID == activeLease.ID {
			targetLineID = line.ID
			break
		}
	}
	if targetLineID == 0 {
		t.Fatalf("expected generated charge line for lease %d, got %#v", activeLease.ID, charges)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{targetLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-submit", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-approve", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync invoice workflow state: %v", err)
	}
	return routeInvoiceSeed{documentID: document.ID, lineID: document.Lines[0].ID}
}

func seedApprovedDepositInvoiceForReceivableRoutes(t *testing.T, ctx context.Context, db *sql.DB, leaseNo string, amount float64, submitKey string, invoiceKey string) routeInvoiceSeed {
	t.Helper()
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)
	activeLease := activateRouteLease(t, ctx, leaseService, workflowService, routeLeaseInput(leaseNo, 1000), submitKey)

	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin deposit charge transaction: %v", err)
	}
	run := &billing.Run{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), Status: billing.RunStatusCompleted, TriggeredBy: 101, GeneratedCount: 1, SkippedCount: 0}
	if err := billingRepo.CreateRun(ctx, tx, run); err != nil {
		t.Fatalf("create deposit billing run: %v", err)
	}
	chargeLine := &billing.ChargeLine{BillingRunID: run.ID, LeaseContractID: activeLease.ID, LeaseNo: activeLease.LeaseNo, TenantName: activeLease.TenantName, LeaseTermID: activeLease.Terms[0].ID, ChargeType: string(lease.TermTypeDeposit), PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), QuantityDays: 30, UnitAmount: amount, Amount: amount, CurrencyTypeID: 101, SourceEffectiveVersion: activeLease.EffectiveVersion}
	if err := billingRepo.InsertChargeLine(ctx, tx, chargeLine); err != nil {
		t.Fatalf("insert deposit charge line: %v", err)
	}
	if err := tx.Commit(); err != nil {
		t.Fatalf("commit deposit charge transaction: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{chargeLine.ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create deposit invoice document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-submit", Comment: "submit deposit invoice"})
	if err != nil {
		t.Fatalf("submit deposit invoice document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-approve", Comment: "approve deposit invoice"})
	if err != nil {
		t.Fatalf("approve deposit workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync deposit workflow state: %v", err)
	}
	return routeInvoiceSeed{documentID: document.ID, lineID: document.Lines[0].ID}
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
