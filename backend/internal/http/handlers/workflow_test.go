package handlers

import (
	"bytes"
	"context"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type mockWorkflowService struct {
	listDefinitionsFunc func(ctx context.Context) ([]workflow.Definition, error)
	listInstancesFunc   func(ctx context.Context, filter workflow.InstanceFilter) ([]workflow.Instance, error)
	startFunc           func(ctx context.Context, input workflow.StartInput) (*workflow.Instance, error)
	approveFunc         func(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error)
	rejectFunc          func(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error)
	resubmitFunc        func(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error)
	getInstanceFunc     func(ctx context.Context, instanceID int64) (*workflow.Instance, error)
	auditHistoryFunc    func(ctx context.Context, instanceID int64) ([]workflow.AuditEntry, error)
	reminderHistoryFunc func(ctx context.Context, instanceID int64) ([]workflow.ReminderAuditRecord, error)
	runRemindersFunc    func(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error)
}

func (m *mockWorkflowService) ListDefinitions(ctx context.Context) ([]workflow.Definition, error) {
	if m.listDefinitionsFunc != nil {
		return m.listDefinitionsFunc(ctx)
	}
	return nil, nil
}
func (m *mockWorkflowService) ListInstances(ctx context.Context, filter workflow.InstanceFilter) ([]workflow.Instance, error) {
	if m.listInstancesFunc != nil {
		return m.listInstancesFunc(ctx, filter)
	}
	return nil, nil
}
func (m *mockWorkflowService) Start(ctx context.Context, input workflow.StartInput) (*workflow.Instance, error) {
	if m.startFunc != nil {
		return m.startFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowService) Approve(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
	if m.approveFunc != nil {
		return m.approveFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowService) Reject(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
	if m.rejectFunc != nil {
		return m.rejectFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowService) Resubmit(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
	if m.resubmitFunc != nil {
		return m.resubmitFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowService) GetInstance(ctx context.Context, instanceID int64) (*workflow.Instance, error) {
	if m.getInstanceFunc != nil {
		return m.getInstanceFunc(ctx, instanceID)
	}
	return nil, nil
}
func (m *mockWorkflowService) AuditHistory(ctx context.Context, instanceID int64) ([]workflow.AuditEntry, error) {
	if m.auditHistoryFunc != nil {
		return m.auditHistoryFunc(ctx, instanceID)
	}
	return nil, nil
}
func (m *mockWorkflowService) ReminderHistory(ctx context.Context, instanceID int64) ([]workflow.ReminderAuditRecord, error) {
	if m.reminderHistoryFunc != nil {
		return m.reminderHistoryFunc(ctx, instanceID)
	}
	return nil, nil
}
func (m *mockWorkflowService) RunReminders(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
	if m.runRemindersFunc == nil {
		return nil, nil
	}
	return m.runRemindersFunc(ctx, now, config)
}

func testWorkflowSessionUser() *auth.SessionUser {
	return &auth.SessionUser{ID: 101, Username: "admin", DepartmentID: 101}
}

func testWorkflowInstance() *workflow.Instance {
	stepOrder := 1
	currentNodeID := int64(201)
	return &workflow.Instance{
		ID:                   301,
		WorkflowDefinitionID: 101,
		DocumentType:         "lease_contract",
		DocumentID:           9001,
		Status:               workflow.InstanceStatusPending,
		CurrentNodeID:        &currentNodeID,
		CurrentStepOrder:     &stepOrder,
		CurrentCycle:         1,
		Version:              1,
		SubmittedBy:          101,
		SubmittedAt:          time.Date(2026, 4, 10, 14, 32, 1, 0, time.UTC),
	}
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
	var resp map[string]any
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
		Reminders []any `json:"reminders"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if len(resp.Reminders) != 0 {
		t.Fatalf("expected empty reminders array, got %d", len(resp.Reminders))
	}
}

func TestWorkflowListDefinitions(t *testing.T) {
	gin.SetMode(gin.TestMode)
	mock := &mockWorkflowService{listDefinitionsFunc: func(ctx context.Context) ([]workflow.Definition, error) {
		return []workflow.Definition{{ID: 101, Code: "lease.approval", Name: "Lease Approval", ProcessClass: "lease"}}, nil
	}}
	handler := NewWorkflowHandler(mock, nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/definitions", nil)

	handler.ListDefinitions(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
	var resp struct {
		Definitions []workflow.Definition `json:"definitions"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if len(resp.Definitions) != 1 || resp.Definitions[0].Code != "lease.approval" {
		t.Fatalf("unexpected definitions: %+v", resp.Definitions)
	}
}

func TestWorkflowListInstances(t *testing.T) {
	gin.SetMode(gin.TestMode)
	instance := *testWorkflowInstance()

	t.Run("without query params", func(t *testing.T) {
		var captured workflow.InstanceFilter
		mock := &mockWorkflowService{listInstancesFunc: func(ctx context.Context, filter workflow.InstanceFilter) ([]workflow.Instance, error) {
			captured = filter
			return []workflow.Instance{instance}, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances", nil)

		handler.ListInstances(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
		if captured.Status != nil || captured.DocumentType != nil || captured.DocumentID != nil {
			t.Fatalf("expected empty filter, got %+v", captured)
		}
	})

	t.Run("with query params", func(t *testing.T) {
		var captured workflow.InstanceFilter
		mock := &mockWorkflowService{listInstancesFunc: func(ctx context.Context, filter workflow.InstanceFilter) ([]workflow.Instance, error) {
			captured = filter
			return []workflow.Instance{instance}, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances?status=pending&document_type=lease_contract&document_id=9001", nil)

		handler.ListInstances(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
		if captured.Status == nil || *captured.Status != workflow.InstanceStatusPending {
			t.Fatalf("unexpected status filter: %+v", captured.Status)
		}
		if captured.DocumentType == nil || *captured.DocumentType != "lease_contract" {
			t.Fatalf("unexpected document type filter: %+v", captured.DocumentType)
		}
		if captured.DocumentID == nil || *captured.DocumentID != 9001 {
			t.Fatalf("unexpected document id filter: %+v", captured.DocumentID)
		}
	})
}

func TestWorkflowApprove(t *testing.T) {
	gin.SetMode(gin.TestMode)
	instance := testWorkflowInstance()

	t.Run("valid request", func(t *testing.T) {
		var captured workflow.TransitionInput
		mock := &mockWorkflowService{approveFunc: func(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
			captured = input
			return instance, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances/301/approve", bytes.NewBufferString(`{"idempotency_key":"approve-301","comment":"looks good"}`))
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Set("session_user", testWorkflowSessionUser())

		handler.Approve(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
		if captured.InstanceID != 301 || captured.ActorUserID != 101 || captured.DepartmentID != 101 || captured.IdempotencyKey != "approve-301" || captured.Comment != "looks good" {
			t.Fatalf("unexpected approve input: %+v", captured)
		}
	})

	t.Run("missing required fields", func(t *testing.T) {
		handler := NewWorkflowHandler(&mockWorkflowService{}, nil)
		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances/301/approve", bytes.NewBufferString(`{"comment":"missing key"}`))
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Set("session_user", testWorkflowSessionUser())

		handler.Approve(ctx)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}

func TestWorkflowReject(t *testing.T) {
	gin.SetMode(gin.TestMode)
	instance := testWorkflowInstance()

	t.Run("valid request", func(t *testing.T) {
		var captured workflow.TransitionInput
		mock := &mockWorkflowService{rejectFunc: func(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
			captured = input
			return instance, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances/301/reject", bytes.NewBufferString(`{"idempotency_key":"reject-301","comment":"needs changes"}`))
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Set("session_user", testWorkflowSessionUser())

		handler.Reject(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
		if captured.IdempotencyKey != "reject-301" || captured.Comment != "needs changes" {
			t.Fatalf("unexpected reject input: %+v", captured)
		}
	})

	t.Run("missing required fields", func(t *testing.T) {
		handler := NewWorkflowHandler(&mockWorkflowService{}, nil)
		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances/301/reject", bytes.NewBufferString(`{"comment":"missing key"}`))
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Set("session_user", testWorkflowSessionUser())

		handler.Reject(ctx)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}

func TestWorkflowResubmit(t *testing.T) {
	gin.SetMode(gin.TestMode)
	instance := testWorkflowInstance()

	t.Run("valid request", func(t *testing.T) {
		var captured workflow.TransitionInput
		mock := &mockWorkflowService{resubmitFunc: func(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error) {
			captured = input
			return instance, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances/301/resubmit", bytes.NewBufferString(`{"idempotency_key":"resubmit-301","comment":"resubmitted"}`))
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Set("session_user", testWorkflowSessionUser())

		handler.Resubmit(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
		if captured.IdempotencyKey != "resubmit-301" || captured.Comment != "resubmitted" {
			t.Fatalf("unexpected resubmit input: %+v", captured)
		}
	})

	t.Run("missing required fields", func(t *testing.T) {
		handler := NewWorkflowHandler(&mockWorkflowService{}, nil)
		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/instances/301/resubmit", bytes.NewBufferString(`{"comment":"missing key"}`))
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Set("session_user", testWorkflowSessionUser())

		handler.Resubmit(ctx)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}

func TestWorkflowGetInstance(t *testing.T) {
	gin.SetMode(gin.TestMode)
	instance := testWorkflowInstance()

	t.Run("valid instance id", func(t *testing.T) {
		mock := &mockWorkflowService{getInstanceFunc: func(ctx context.Context, instanceID int64) (*workflow.Instance, error) {
			if instanceID != 301 {
				t.Fatalf("expected instance id 301, got %d", instanceID)
			}
			return instance, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances/301", nil)

		handler.GetInstance(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})

	t.Run("invalid instance id", func(t *testing.T) {
		handler := NewWorkflowHandler(&mockWorkflowService{}, nil)
		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
		ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances/bad-id", nil)

		handler.GetInstance(ctx)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}

func TestWorkflowAuditHistory(t *testing.T) {
	gin.SetMode(gin.TestMode)

	t.Run("valid instance id", func(t *testing.T) {
		stepOrder := 1
		comment := "approved"
		mock := &mockWorkflowService{auditHistoryFunc: func(ctx context.Context, instanceID int64) ([]workflow.AuditEntry, error) {
			if instanceID != 301 {
				t.Fatalf("expected instance id 301, got %d", instanceID)
			}
			return []workflow.AuditEntry{{ID: 1, WorkflowInstanceID: 301, Action: workflow.ActionApprove, ActorUserID: 101, FromStatus: workflow.InstanceStatusPending, ToStatus: workflow.InstanceStatusApproved, FromStepOrder: &stepOrder, Comment: &comment, IdempotencyKey: "approve-301", CreatedAt: time.Now().UTC()}}, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "301"}}
		ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances/301/audit", nil)

		handler.AuditHistory(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})

	t.Run("invalid instance id", func(t *testing.T) {
		handler := NewWorkflowHandler(&mockWorkflowService{}, nil)
		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
		ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/instances/bad-id/audit", nil)

		handler.AuditHistory(ctx)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}

func TestWorkflowRunReminders(t *testing.T) {
	gin.SetMode(gin.TestMode)

	t.Run("valid payload", func(t *testing.T) {
		var captured workflow.ReminderConfig
		mock := &mockWorkflowService{runRemindersFunc: func(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			captured = config
			return []workflow.ReminderAuditRecord{{ID: 1, WorkflowInstanceID: 301, ReminderType: "standard", Outcome: workflow.ReminderOutcomeEmitted, CreatedAt: now}}, nil
		}}
		handler := NewWorkflowHandler(mock, nil)

		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		body := bytes.NewBufferString(`{"reminder_type":"standard","min_pending_age_sec":120,"window_truncation_sec":60}`)
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", body)
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Request.ContentLength = int64(body.Len())

		handler.RunReminders(ctx)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}
		if captured.ReminderType != "standard" || captured.MinPendingAge != 120*time.Second || captured.WindowTruncation != 60*time.Second {
			t.Fatalf("unexpected reminder config: %+v", captured)
		}
	})

	t.Run("invalid payload", func(t *testing.T) {
		handler := NewWorkflowHandler(&mockWorkflowService{}, nil)
		recorder := httptest.NewRecorder()
		ctx, _ := gin.CreateTestContext(recorder)
		body := bytes.NewBufferString(`{"min_pending_age_sec":"bad"}`)
		ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", body)
		ctx.Request.Header.Set("Content-Type", "application/json")
		ctx.Request.ContentLength = int64(body.Len())

		handler.RunReminders(ctx)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}
