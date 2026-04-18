package handlers

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

func setExcelIOSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 1, Username: "test", DepartmentID: 1})
}

func TestExcelIOImportUnitsRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewExcelIOHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/excelio/import/units", nil)

	handler.ImportUnits(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

func TestExcelIOImportUnitsRejectsMissingFile(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewExcelIOHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setExcelIOSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/excelio/import/units", nil)

	handler.ImportUnits(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestExcelIOImportDailySalesRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewExcelIOHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/excelio/import/daily-sales", nil)

	handler.ImportDailySales(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

func TestExcelIOImportDailySalesRejectsMissingFile(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewExcelIOHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setExcelIOSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/excelio/import/daily-sales", nil)

	handler.ImportDailySales(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestExcelIOImportTrafficRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewExcelIOHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/excelio/import/traffic", nil)

	handler.ImportTraffic(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

func TestExcelIOImportTrafficRejectsMissingFile(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewExcelIOHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	setExcelIOSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/excelio/import/traffic", nil)

	handler.ImportTraffic(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
