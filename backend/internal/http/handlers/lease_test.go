package handlers

import (
	"bytes"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/gin-gonic/gin"
)

func setLeaseSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 1, Username: "test", DepartmentID: 1})
}

const validLeaseCreateJSON = `{
	"lease_no": "L001",
	"department_id": 1,
	"store_id": 1,
	"tenant_name": "Test Tenant",
	"start_date": "2025-01-01",
	"end_date": "2025-12-31",
	"units": [{"unit_id": 1, "rent_area": 100.0}],
	"terms": [{"term_type": "rent", "billing_cycle": "monthly", "currency_type_id": 1, "amount": 5000, "effective_from": "2025-01-01", "effective_to": "2025-12-31"}]
}`

func TestLeaseCreateRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseCreateRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseCreateRejectsInvalidStartDate(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setLeaseSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases", bytes.NewBufferString(`{
		"lease_no": "L001", "department_id": 1, "store_id": 1, "tenant_name": "Test",
		"start_date": "not-a-date", "end_date": "2025-12-31",
		"units": [{"unit_id": 1, "rent_area": 100}],
		"terms": [{"term_type": "rent", "billing_cycle": "monthly", "currency_type_id": 1, "amount": 5000, "effective_from": "2025-01-01", "effective_to": "2025-12-31"}]
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseCreateRejectsInvalidEndDate(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setLeaseSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases", bytes.NewBufferString(`{
		"lease_no": "L001", "department_id": 1, "store_id": 1, "tenant_name": "Test",
		"start_date": "2025-01-01", "end_date": "not-a-date",
		"units": [{"unit_id": 1, "rent_area": 100}],
		"terms": [{"term_type": "rent", "billing_cycle": "monthly", "currency_type_id": 1, "amount": 5000, "effective_from": "2025-01-01", "effective_to": "2025-12-31"}]
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseGetRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/leases/bad-id", nil)

	handler.Get(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseListRejectsInvalidDepartmentID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/leases?department_id=bad-id", nil)

	handler.List(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}

	var payload map[string]any
	if err := json.Unmarshal(recorder.Body.Bytes(), &payload); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if payload["message"] != "invalid department id" {
		t.Fatalf("expected invalid department id message, got %#v", payload["message"])
	}
}

func TestLeaseSubmitRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/bad-id/submit", bytes.NewBufferString(`{"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Submit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseSubmitRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/1/submit", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Submit(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseTerminateRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/bad-id/terminate", bytes.NewBufferString(`{"terminated_at":"2025-06-30"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Terminate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseTerminateRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/1/terminate", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Terminate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseTerminateRejectsInvalidTerminatedAt(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	setLeaseSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/1/terminate", bytes.NewBufferString(`{"terminated_at":"not-a-date"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Terminate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseAmendRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/bad-id/amend", bytes.NewBufferString(validLeaseCreateJSON))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Amend(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseAmendRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/leases/1/amend", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Amend(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseListRejectsInvalidStoreID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/leases?store_id=bad", nil)

	handler.List(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseListRejectsInvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/leases?page=bad", nil)

	handler.List(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestLeaseListRejectsInvalidPageSize(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/leases?page_size=bad", nil)

	handler.List(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBindCreateDraftInputParsesSubtypeDetails(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)
	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)

	request := createLeaseRequest{
		LeaseNo:      "L002",
		Subtype:      lease.ContractSubtypeAdBoard,
		DepartmentID: 1,
		StoreID:      1,
		TenantName:   "Ad Tenant",
		StartDate:    "2026-04-01",
		EndDate:      "2026-12-31",
		AdBoards: []createLeaseAdBoardRequest{{
			AdBoardID:    11,
			StartDate:    "2026-04-01",
			EndDate:      "2026-05-01",
			RentArea:     20,
			Airtime:      15,
			Frequency:    lease.AdBoardFrequencyWeek,
			FrequencyMon: true,
		}},
		Units: []createLeaseUnitRequest{{UnitID: 1, RentArea: 100}},
		Terms: []createLeaseTermRequest{{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 1, Amount: 5000, EffectiveFrom: "2026-04-01", EffectiveTo: "2026-12-31"}},
	}

	input, ok := handler.bindCreateDraftInput(ctx, request, 1)
	if !ok {
		t.Fatalf("expected subtype payload to bind, got status=%d body=%s", recorder.Code, recorder.Body.String())
	}
	if input.Subtype != lease.ContractSubtypeAdBoard || len(input.AdBoards) != 1 {
		t.Fatalf("expected ad board subtype binding, got %#v", input)
	}
	if input.AdBoards[0].AdBoardID != 11 || !input.AdBoards[0].FrequencyMon {
		t.Fatalf("expected bound ad board detail, got %#v", input.AdBoards[0])
	}
}

func TestLeaseRenderValidationFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewLeaseHandler(nil)
	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)

	handler.renderLeaseError(ctx, &lease.ValidationError{Fields: []lease.ValidationField{{Field: "joint_operation.bill_cycle", Message: "bill_cycle must be greater than zero"}}})

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
	var body map[string]any
	if err := json.Unmarshal(recorder.Body.Bytes(), &body); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	fields, ok := body["fields"].([]any)
	if !ok || len(fields) != 1 {
		t.Fatalf("expected field diagnostics, got %#v", body)
	}
}
