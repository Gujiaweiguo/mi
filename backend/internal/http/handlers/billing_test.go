package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

func setBillingSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 1, Username: "test", DepartmentID: 1})
}

func TestBillingGenerateChargesRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/billing/generate", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.GenerateCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingGenerateChargesRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/billing/generate", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.GenerateCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingGenerateChargesRejectsInvalidPeriodStart(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setBillingSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/billing/generate", bytes.NewBufferString(`{"period_start":"not-a-date","period_end":"2025-01-31"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.GenerateCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingGenerateChargesRejectsInvalidPeriodEnd(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setBillingSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/billing/generate", bytes.NewBufferString(`{"period_start":"2025-01-01","period_end":"not-a-date"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.GenerateCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingListChargesRejectsInvalidLeaseContractID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/billing/charges?lease_contract_id=bad", nil)

	handler.ListCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingListChargesRejectsInvalidPeriodStart(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/billing/charges?period_start=bad", nil)

	handler.ListCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingListChargesRejectsInvalidPeriodEnd(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/billing/charges?period_end=bad", nil)

	handler.ListCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingListChargesRejectsInvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/billing/charges?page=bad", nil)

	handler.ListCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestBillingListChargesRejectsInvalidPageSize(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewBillingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/billing/charges?page_size=bad", nil)

	handler.ListCharges(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
