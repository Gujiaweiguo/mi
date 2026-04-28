//go:build integration

package http_test

import (
	"bytes"
	"context"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"os"
	"strconv"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	httpapi "github.com/Gujiaweiguo/mi/backend/internal/http"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"go.uber.org/zap"
)

func TestIntegrationOvertimeRoutesAndWorkflowSync(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	activeLease := createActiveLeaseForHTTP(t, ctx, leaseService, workflowService)
	router := httpapi.NewRouter(&config.Config{App: config.AppConfig{Name: "mi-backend", Environment: "test"}, Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600}}, db, zap.NewNop())
	token := loginToken(t, router)
	createBody := `{"lease_contract_id":` + strconv.FormatInt(activeLease.ID, 10) + `,"period_start":"2026-04-01","period_end":"2026-04-30","note":"HTTP overtime","formulas":[{"charge_type":"overtime_rent","formula_type":"fixed","rate_type":"daily","effective_from":"2026-04-01","effective_to":"2026-04-30","currency_type_id":101,"total_area":10,"unit_price":2}]}`
	createRecorder := httptest.NewRecorder()
	createRequest := httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(createBody))
	createRequest.Header.Set("Content-Type", "application/json")
	createRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(createRecorder, createRequest)
	if createRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from overtime create, got %d body=%s", createRecorder.Code, createRecorder.Body.String())
	}
	var createResponse struct { Bill struct { ID int64 `json:"id"`; WorkflowInstanceID *int64 `json:"workflow_instance_id"` } `json:"bill"` }
	if err := json.Unmarshal(createRecorder.Body.Bytes(), &createResponse); err != nil {
		t.Fatalf("decode overtime create response: %v", err)
	}
	submitRecorder := httptest.NewRecorder()
	submitRequest := httptest.NewRequest(http.MethodPost, "/api/overtime/bills/"+strconv.FormatInt(createResponse.Bill.ID, 10)+"/submit", bytes.NewBufferString(`{"idempotency_key":"submit-http-ot-1","comment":"submit overtime"}`))
	submitRequest.Header.Set("Content-Type", "application/json")
	submitRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(submitRecorder, submitRequest)
	if submitRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from overtime submit, got %d body=%s", submitRecorder.Code, submitRecorder.Body.String())
	}
	var submitResponse struct { Bill struct { WorkflowInstanceID *int64 `json:"workflow_instance_id"` } `json:"bill"` }
	if err := json.Unmarshal(submitRecorder.Body.Bytes(), &submitResponse); err != nil {
		t.Fatalf("decode overtime submit response: %v", err)
	}
	if submitResponse.Bill.WorkflowInstanceID == nil {
		t.Fatalf("expected overtime workflow instance after submit, got body=%s", submitRecorder.Body.String())
	}
	approveWorkflow(t, router, "Bearer "+token, *submitResponse.Bill.WorkflowInstanceID, "approve-http-ot-1", "manager approved")
	approveWorkflow(t, router, "Bearer "+token, *submitResponse.Bill.WorkflowInstanceID, "approve-http-ot-2", "finance approved")
	getRecorder := httptest.NewRecorder()
	getRequest := httptest.NewRequest(http.MethodGet, "/api/overtime/bills/"+strconv.FormatInt(createResponse.Bill.ID, 10), nil)
	getRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(getRecorder, getRequest)
	if getRecorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from overtime get, got %d body=%s", getRecorder.Code, getRecorder.Body.String())
	}
	if !bytes.Contains(getRecorder.Body.Bytes(), []byte(`"status":"approved"`)) {
		t.Fatalf("expected approved overtime bill after workflow sync, got body=%s", getRecorder.Body.String())
	}
	generateRecorder := httptest.NewRecorder()
	generateRequest := httptest.NewRequest(http.MethodPost, "/api/overtime/bills/"+strconv.FormatInt(createResponse.Bill.ID, 10)+"/generate", bytes.NewBufferString(`{}`))
	generateRequest.Header.Set("Content-Type", "application/json")
	generateRequest.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(generateRecorder, generateRequest)
	if generateRecorder.Code != http.StatusCreated {
		t.Fatalf("expected 201 from overtime generate, got %d body=%s", generateRecorder.Code, generateRecorder.Body.String())
	}
	if !bytes.Contains(generateRecorder.Body.Bytes(), []byte(`"generated":1`)) {
		t.Fatalf("expected one generated overtime charge, got body=%s", generateRecorder.Body.String())
	}
}

func createActiveLeaseForHTTP(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service) *lease.Contract {
	t.Helper()
	input := lease.CreateDraftInput{LeaseNo: "CON-HTTP-OT-1", DepartmentID: 101, StoreID: 101, BuildingID: int64PointerHTTP(101), TenantName: "HTTP OT Tenant", StartDate: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), EndDate: time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC), Units: []lease.UnitInput{{UnitID: 101, RentArea: 118}}, Terms: []lease.TermInput{{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: 12000, EffectiveFrom: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), EffectiveTo: time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)}}, ActorUserID: 101}
	draft, err := leaseService.CreateDraft(ctx, input)
	if err != nil {
		t.Fatalf("create http lease draft: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-http-lease-ot-1", Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit http lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-http-lease-ot-1-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve http lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync http lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-http-lease-ot-1-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve http lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync http lease step 2: %v", err)
	}
	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get http active lease: %v", err)
	}
	return active
}

func loginToken(t *testing.T, router http.Handler) string {
	t.Helper()
	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodPost, "/api/auth/login", bytes.NewBufferString(`{"username":"admin","password":"password"}`))
	request.Header.Set("Content-Type", "application/json")
	router.ServeHTTP(recorder, request)
	if recorder.Code != http.StatusOK {
		t.Fatalf("login failed: %d body=%s", recorder.Code, recorder.Body.String())
	}
	var body struct { Token string `json:"token"` }
	if err := json.Unmarshal(recorder.Body.Bytes(), &body); err != nil {
		t.Fatalf("decode login response: %v", err)
	}
	return body.Token
}

func int64PointerHTTP(value int64) *int64 { return &value }
