package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

func setOvertimeSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 1, Username: "test", DepartmentID: 1})
}

const validOvertimeCreateJSON = `{
	"lease_contract_id": 1,
	"period_start": "2025-01-01",
	"period_end": "2025-12-31",
	"formulas": [
		{
			"charge_type": "rent",
			"formula_type": "fixed",
			"rate_type": "monthly",
			"effective_from": "2025-01-01",
			"effective_to": "2025-12-31",
			"currency_type_id": 1,
			"total_area": 100.0,
			"unit_price": 50.0
		}
	]
}`

// --- CreateBill tests ---

func TestOvertimeCreateBillRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeCreateBillRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeCreateBillRejectsInvalidPeriodStart(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setOvertimeSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(`{
		"lease_contract_id": 1,
		"period_start": "not-a-date",
		"period_end": "2025-12-31",
		"formulas": [{"charge_type": "rent", "formula_type": "fixed", "rate_type": "monthly", "currency_type_id": 1}]
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeCreateBillRejectsInvalidPeriodEnd(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setOvertimeSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(`{
		"lease_contract_id": 1,
		"period_start": "2025-01-01",
		"period_end": "not-a-date",
		"formulas": [{"charge_type": "rent", "formula_type": "fixed", "rate_type": "monthly", "currency_type_id": 1}]
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeCreateBillRejectsInvalidFormulaEffectiveFrom(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setOvertimeSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(`{
		"lease_contract_id": 1,
		"period_start": "2025-01-01",
		"period_end": "2025-12-31",
		"formulas": [{"charge_type": "rent", "formula_type": "fixed", "rate_type": "monthly", "currency_type_id": 1, "effective_from": "not-a-date"}]
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeCreateBillRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills", bytes.NewBufferString(validOvertimeCreateJSON))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateBill(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

// --- GetBill tests ---

func TestOvertimeGetBillRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/overtime/bills/bad-id", nil)

	handler.GetBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

// --- ListBills tests ---

func TestOvertimeListBillsRejectsInvalidLeaseContractID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/overtime/bills?lease_contract_id=bad", nil)

	handler.ListBills(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeListBillsRejectsInvalidPeriodStart(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/overtime/bills?period_start=bad", nil)

	handler.ListBills(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeListBillsRejectsInvalidPeriodEnd(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/overtime/bills?period_end=bad", nil)

	handler.ListBills(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeListBillsRejectsInvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/overtime/bills?page=bad", nil)

	handler.ListBills(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeListBillsRejectsInvalidPageSize(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/overtime/bills?page_size=bad", nil)

	handler.ListBills(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

// --- SubmitBill tests ---

func TestOvertimeSubmitBillRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/bad-id/submit", bytes.NewBufferString(`{"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SubmitBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeSubmitBillRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/1/submit", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SubmitBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeSubmitBillRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/1/submit", bytes.NewBufferString(`{"idempotency_key":"k1"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SubmitBill(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

// --- CancelBill tests ---

func TestOvertimeCancelBillRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/bad-id/cancel", nil)

	handler.CancelBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeCancelBillRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/1/cancel", nil)

	handler.CancelBill(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

// --- StopBill tests ---

func TestOvertimeStopBillRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/bad-id/stop", nil)

	handler.StopBill(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeStopBillRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/1/stop", nil)

	handler.StopBill(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

// --- GenerateCharges tests ---

func TestOvertimeGenerateChargesRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/bad-id/generate", nil)

	handler.GenerateCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestOvertimeGenerateChargesRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewOvertimeHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/overtime/bills/1/generate", nil)

	handler.GenerateCharges(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}
