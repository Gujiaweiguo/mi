package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestWorkflowStartRejectsInvalidPayload(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowHandler(nil, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances", bytes.NewBufferString(`{"definition_code":""}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Start(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestWorkflowReminderHistoryRejectsInvalidInstanceID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowHandler(nil, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances/bad-id/reminders", nil)

	handler.ReminderHistory(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
