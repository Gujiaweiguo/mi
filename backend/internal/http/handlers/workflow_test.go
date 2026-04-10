package handlers

import (
	"bytes"
	"context"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type mockWorkflowService struct {
	runRemindersFunc func(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error)
}

func (m *mockWorkflowService) ListDefinitions(ctx context.Context) ([]workflow.Definition, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) ListInstances(ctx context.Context, filter workflow.InstanceFilter) ([]workflow.Instance, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) Start(ctx context.Context, input workflow.StartInput) (*workflow.Instance, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) Approve(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) Reject(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) Resubmit(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) GetInstance(ctx context.Context, instanceID int64) (*workflow.Instance, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) AuditHistory(ctx context.Context, instanceID int64) ([]workflow.AuditEntry, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) ReminderHistory(ctx context.Context, instanceID int64) ([]workflow.ReminderAuditRecord, error) {
	panic("unexpected call")
}
func (m *mockWorkflowService) RunReminders(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
	return m.runRemindersFunc(ctx, now, config)
}

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

func TestRunRemindersRejectsMalformedJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowHandler(nil, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	body := bytes.NewBufferString(`{bad json`)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", body)
	ctx.Request.Header.Set("Content-Type", "application/json")
	ctx.Request.ContentLength = int64(body.Len())

	handler.RunReminders(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
	var resp map[string]interface{}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if resp["message"] != "invalid request body" {
		t.Fatalf("expected 'invalid request body', got %v", resp["message"])
	}
}

func TestRunRemindersRejectsNegativeMinPendingAgeSec(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowHandler(nil, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	body := bytes.NewBufferString(`{"min_pending_age_sec": -5}`)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", body)
	ctx.Request.Header.Set("Content-Type", "application/json")
	ctx.Request.ContentLength = int64(body.Len())

	handler.RunReminders(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestRunRemindersRejectsNegativeWindowTruncSec(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowHandler(nil, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	body := bytes.NewBufferString(`{"window_truncation_sec": -1}`)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", body)
	ctx.Request.Header.Set("Content-Type", "application/json")
	ctx.Request.ContentLength = int64(body.Len())

	handler.RunReminders(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestRunRemindersSuccessPath(t *testing.T) {
	gin.SetMode(gin.TestMode)

	ts := time.Date(2026, 4, 10, 14, 32, 1, 0, time.UTC)
	records := []workflow.ReminderAuditRecord{
		{
			ID:                  42,
			WorkflowInstanceID:  7,
			ReminderType:        "standard",
			ReminderKey:         "reminder:7:standard:2026-04-10T00:00:00Z",
			ReminderWindowStart: time.Date(2026, 4, 10, 0, 0, 0, 0, time.UTC),
			Outcome:             workflow.ReminderOutcomeEmitted,
			CreatedAt:           ts,
		},
	}
	mock := &mockWorkflowService{
		runRemindersFunc: func(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			return records, nil
		},
	}
	handler := NewWorkflowHandler(mock, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	body := bytes.NewBufferString(`{"reminder_type":"standard","min_pending_age_sec":3600}`)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", body)
	ctx.Request.Header.Set("Content-Type", "application/json")
	ctx.Request.ContentLength = int64(body.Len())

	handler.RunReminders(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}

	var resp struct {
		Reminders []struct {
			ID                 int64  `json:"id"`
			WorkflowInstanceID int64  `json:"workflow_instance_id"`
			ReminderType       string `json:"reminder_type"`
			Outcome            string `json:"outcome"`
		} `json:"reminders"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if len(resp.Reminders) != 1 {
		t.Fatalf("expected 1 reminder, got %d", len(resp.Reminders))
	}
	r := resp.Reminders[0]
	if r.ID != 42 || r.WorkflowInstanceID != 7 || r.ReminderType != "standard" || r.Outcome != string(workflow.ReminderOutcomeEmitted) {
		t.Fatalf("unexpected reminder: id=%d instance=%d type=%q outcome=%q", r.ID, r.WorkflowInstanceID, r.ReminderType, r.Outcome)
	}
}

func TestRunRemindersEmptyBodyDefaults(t *testing.T) {
	gin.SetMode(gin.TestMode)

	var capturedConfig workflow.ReminderConfig
	mock := &mockWorkflowService{
		runRemindersFunc: func(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			capturedConfig = config
			return []workflow.ReminderAuditRecord{}, nil
		},
	}
	handler := NewWorkflowHandler(mock, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", nil)

	handler.RunReminders(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
	if capturedConfig.ReminderType != "" {
		t.Fatalf("expected empty reminder type default, got %q", capturedConfig.ReminderType)
	}
	if capturedConfig.MinPendingAge != 0 {
		t.Fatalf("expected zero min pending age default, got %v", capturedConfig.MinPendingAge)
	}
	if capturedConfig.WindowTruncation != 0 {
		t.Fatalf("expected zero window truncation default, got %v", capturedConfig.WindowTruncation)
	}
	var resp struct {
		Reminders []interface{} `json:"reminders"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if len(resp.Reminders) != 0 {
		t.Fatalf("expected empty reminders array, got %d", len(resp.Reminders))
	}
}
