package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

func setTaxExportSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 1, Username: "test", DepartmentID: 1})
}

func TestTaxExportUpsertRuleSetRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/tax-export/rule-sets", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpsertRuleSet(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestTaxExportUpsertRuleSetRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/tax-export/rule-sets", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.UpsertRuleSet(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestTaxExportListRuleSetsRejectsInvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/tax-export/rule-sets?page=bad", nil)

	handler.ListRuleSets(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestTaxExportListRuleSetsRejectsInvalidPageSize(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/tax-export/rule-sets?page_size=bad", nil)

	handler.ListRuleSets(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestTaxExportExportVoucherWorkbookRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/tax-export/vouchers/export", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ExportVoucherWorkbook(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestTaxExportExportVoucherWorkbookRejectsInvalidFromDate(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setTaxExportSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/tax-export/vouchers/export", bytes.NewBufferString(`{"rule_set_code":"default","from_date":"not-a-date","to_date":"2025-01-31"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ExportVoucherWorkbook(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestTaxExportExportVoucherWorkbookRejectsInvalidToDate(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewTaxExportHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setTaxExportSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/tax-export/vouchers/export", bytes.NewBufferString(`{"rule_set_code":"default","from_date":"2025-01-01","to_date":"not-a-date"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ExportVoucherWorkbook(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
