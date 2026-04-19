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
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"go.uber.org/zap"
)

func TestIntegrationWorkflowReminderHistoryRoute(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))

	baseTime := time.Date(2026, 4, 10, 9, 0, 0, 0, time.UTC)
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, func() time.Time { return baseTime }))
	instance, err := workflowService.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9401,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9401",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	if _, err := workflowService.RunReminders(ctx, baseTime.Add(25*time.Hour), workflow.ReminderConfig{
		ReminderType:     "standard",
		MinPendingAge:    24 * time.Hour,
		WindowTruncation: 24 * time.Hour,
	}); err != nil {
		t.Fatalf("run reminders: %v", err)
	}

	router := httpapi.NewRouter(&config.Config{
		App:  config.AppConfig{Name: "mi-backend", Environment: "test"},
		Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600},
	}, db, zap.NewNop())
	token := loginAsAdmin(t, router)

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/api/workflow/instances/"+strconv.FormatInt(instance.ID, 10)+"/reminders", nil)
	request.Header.Set("Authorization", "Bearer "+token)
	router.ServeHTTP(recorder, request)
	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200 from workflow reminder history endpoint, got %d body=%s", recorder.Code, recorder.Body.String())
	}

	var body struct {
		Reminders []struct {
			WorkflowInstanceID int64   `json:"workflow_instance_id"`
			ReminderType       string  `json:"reminder_type"`
			Outcome            string  `json:"outcome"`
			ReasonCode         *string `json:"reason_code"`
		} `json:"reminders"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &body); err != nil {
		t.Fatalf("decode reminder history response: %v", err)
	}
	if len(body.Reminders) != 1 {
		t.Fatalf("expected one reminder history row, got body=%s", recorder.Body.String())
	}
	if body.Reminders[0].WorkflowInstanceID != instance.ID || body.Reminders[0].ReminderType != "standard" || body.Reminders[0].Outcome != string(workflow.ReminderOutcomeEmitted) || body.Reminders[0].ReasonCode != nil {
		t.Fatalf("expected emitted reminder payload, got body=%s", recorder.Body.String())
	}
}

func TestIntegrationWorkflowReminderTriggerRoute(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))

	// Create a pending workflow instance so the reminder sweep has something to process.
	_, err := workflow.NewService(db, workflow.NewRepository(db)).Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9501,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-trigger-9501",
		Comment:        "submit for trigger test",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}

	if _, err := db.ExecContext(ctx, `UPDATE workflow_instances SET submitted_at = DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 DAY) WHERE document_type = 'lease_contract' AND document_id = 9501`); err != nil {
		t.Fatalf("backdate workflow submitted_at: %v", err)
	}

	_, err = db.ExecContext(ctx, `
		INSERT INTO users (id, department_id, username, display_name, password_hash, status)
		VALUES (201, 101, 'viewer', 'Test Viewer', '$2a$10$32RDlfSKfGJDcHhJWP3JoOBi8SyorV7r2lWcs8hixdhFA/AtOI1gC', 'active')
		ON DUPLICATE KEY UPDATE username = VALUES(username)
	`)
	if err != nil {
		t.Fatalf("insert viewer user: %v", err)
	}
	_, err = db.ExecContext(ctx, `
		INSERT INTO user_roles (user_id, role_id, department_id)
		VALUES (201, 102, 101)
		ON DUPLICATE KEY UPDATE role_id = VALUES(role_id)
	`)
	if err != nil {
		t.Fatalf("insert viewer role: %v", err)
	}

	router := httpapi.NewRouter(&config.Config{
		App:  config.AppConfig{Name: "mi-backend", Environment: "test"},
		Auth: config.AuthConfig{JWTSecret: "test-secret", TokenExpirySeconds: 3600},
	}, db, zap.NewNop())
	token := loginAsAdmin(t, router)

	t.Run("returns_200_with_reminders", func(t *testing.T) {
		recorder := httptest.NewRecorder()
		request := httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", bytes.NewBufferString(`{}`))
		request.Header.Set("Content-Type", "application/json")
		request.Header.Set("Authorization", "Bearer "+token)
		router.ServeHTTP(recorder, request)

		if recorder.Code != http.StatusOK {
			t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
		}

		var body struct {
			Reminders []struct {
				WorkflowInstanceID int64  `json:"workflow_instance_id"`
				ReminderType       string `json:"reminder_type"`
				Outcome            string `json:"outcome"`
			} `json:"reminders"`
		}
		if err := json.Unmarshal(recorder.Body.Bytes(), &body); err != nil {
			t.Fatalf("decode response: %v", err)
		}
		if len(body.Reminders) == 0 {
			t.Fatalf("expected at least one reminder, got body=%s", recorder.Body.String())
		}
		if body.Reminders[0].Outcome != string(workflow.ReminderOutcomeEmitted) {
			t.Fatalf("expected emitted outcome, got %s body=%s", body.Reminders[0].Outcome, recorder.Body.String())
		}
	})

	t.Run("returns_400_on_bad_input", func(t *testing.T) {
		recorder := httptest.NewRecorder()
		request := httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", bytes.NewBufferString(`{bad`))
		request.Header.Set("Content-Type", "application/json")
		request.Header.Set("Authorization", "Bearer "+token)
		router.ServeHTTP(recorder, request)

		if recorder.Code != http.StatusBadRequest {
			t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})

	t.Run("returns_403_without_permission", func(t *testing.T) {
		loginRec := httptest.NewRecorder()
		loginReq := httptest.NewRequest(http.MethodPost, "/api/auth/login", bytes.NewBufferString(`{"username":"viewer","password":"password"}`))
		loginReq.Header.Set("Content-Type", "application/json")
		router.ServeHTTP(loginRec, loginReq)
		if loginRec.Code != http.StatusOK {
			t.Fatalf("expected 200 from viewer login, got %d body=%s", loginRec.Code, loginRec.Body.String())
		}
		var loginBody struct {
			Token string `json:"token"`
		}
		if err := json.Unmarshal(loginRec.Body.Bytes(), &loginBody); err != nil {
			t.Fatalf("decode viewer login response: %v", err)
		}

		recorder := httptest.NewRecorder()
		request := httptest.NewRequest(http.MethodPost, "/api/workflow/reminders/run", bytes.NewBufferString(`{}`))
		request.Header.Set("Content-Type", "application/json")
		request.Header.Set("Authorization", "Bearer "+loginBody.Token)
		router.ServeHTTP(recorder, request)

		if recorder.Code != http.StatusForbidden {
			t.Fatalf("expected 403, got %d body=%s", recorder.Code, recorder.Body.String())
		}
	})
}
