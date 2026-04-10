//go:build integration

package http_test

import (
	"context"
	"database/sql"
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
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

func TestIntegrationWorkflowReminderHistoryRoute(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	container, err := testcontainers.GenericContainer(ctx, testcontainers.GenericContainerRequest{
		ContainerRequest: testcontainers.ContainerRequest{
			Image:        "mysql:8.0",
			ExposedPorts: []string{"3306/tcp"},
			Env: map[string]string{
				"MYSQL_DATABASE":      "mi_integration",
				"MYSQL_USER":          "mi_user",
				"MYSQL_PASSWORD":      "mi_password",
				"MYSQL_ROOT_PASSWORD": "mi_root_password",
			},
			WaitingFor: wait.ForListeningPort("3306/tcp").WithStartupTimeout(3 * time.Minute),
		},
		Started: true,
	})
	if err != nil {
		t.Fatalf("start mysql container: %v", err)
	}
	defer func() { _ = container.Terminate(context.Background()) }()

	host, err := container.Host(ctx)
	if err != nil {
		t.Fatalf("resolve mysql host: %v", err)
	}
	port, err := container.MappedPort(ctx, "3306/tcp")
	if err != nil {
		t.Fatalf("resolve mysql port: %v", err)
	}

	db, err := sql.Open("mysql", platformdb.Config{Host: host, Port: port.Int(), Name: "mi_integration", User: "mi_user", Password: "mi_password"}.DSN())
	if err != nil {
		t.Fatalf("open mysql: %v", err)
	}
	defer db.Close()

	if err := waitForDatabase(ctx, db); err != nil {
		t.Fatalf("ping mysql: %v", err)
	}
	migrator := platformdb.NewMigrator(db, os.DirFS("../platform/database"), "migrations")
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("apply migrations: %v", err)
	}
	bootstrapRunner := platformdb.NewBootstrapRunner(db, bootstrap.All()...)
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("run bootstrap seeds: %v", err)
	}

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
	}, db)
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
