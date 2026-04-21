package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

func setReportingSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 1, Username: "test", DepartmentID: 1})
}

func TestReportingQueryRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewReportingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "reportId", Value: "r01"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/reports/r01/query", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Query(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestReportingExportRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewReportingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "reportId", Value: "r01"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/reports/r01/export", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Export(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestReportingQueryRejectsInvalidPeriod(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewReportingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "reportId", Value: "r01"}}
	setReportingSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/reports/r01/query", bytes.NewBufferString(`{"period":"not-a-period"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Query(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestReportingBuildInputAllowsR19WithoutPeriod(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewReportingHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "reportId", Value: "r19"}}
	setReportingSessionUser(ctx)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/reports/r19/query", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	input, ok := handler.buildInput(ctx)
	if !ok {
		t.Fatalf("expected R19 buildInput to accept an empty body, got status=%d body=%s", recorder.Code, recorder.Body.String())
	}
	if input.ReportID != "r19" {
		t.Fatalf("expected report ID r19, got %q", input.ReportID)
	}
	if input.PeriodLabel != "visual" {
		t.Fatalf("expected visual period label, got %q", input.PeriodLabel)
	}
	if !input.PeriodStart.IsZero() || !input.PeriodEnd.IsZero() {
		t.Fatalf("expected R19 to skip period parsing, got start=%v end=%v", input.PeriodStart, input.PeriodEnd)
	}
}
