//go:build integration

package workflow_test

import (
	"context"
	"database/sql"
	"os"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

func TestWorkflowServiceApproveAndDeduplicate(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newWorkflowTestDB(t, ctx)
	service := workflow.NewService(db, workflow.NewRepository(db))

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9001,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9001",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	if instance.Status != workflow.InstanceStatusPending || instance.CurrentStepOrder == nil || *instance.CurrentStepOrder != 1 {
		t.Fatalf("expected pending instance at step 1, got %#v", instance)
	}

	instance, err = service.Approve(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-step-1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve first step: %v", err)
	}
	if instance.CurrentStepOrder == nil || *instance.CurrentStepOrder != 2 || instance.Status != workflow.InstanceStatusPending {
		t.Fatalf("expected pending instance at step 2, got %#v", instance)
	}

	duplicateInstance, err := service.Approve(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-step-1", Comment: "manager approved duplicate"})
	if err != nil {
		t.Fatalf("duplicate approve should be safe: %v", err)
	}
	if duplicateInstance.Version != instance.Version {
		t.Fatalf("expected duplicate approve to preserve version %d, got %d", instance.Version, duplicateInstance.Version)
	}

	history, err := service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history: %v", err)
	}
	if len(history) != 2 {
		t.Fatalf("expected 2 audit entries after duplicate approve, got %d", len(history))
	}

	outbox, err := service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox messages: %v", err)
	}
	if len(outbox) != 2 {
		t.Fatalf("expected 2 outbox messages after duplicate approve, got %d", len(outbox))
	}

	instance, err = service.Approve(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-step-2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve final step: %v", err)
	}
	if instance.Status != workflow.InstanceStatusApproved || instance.CompletedAt == nil {
		t.Fatalf("expected approved completed instance, got %#v", instance)
	}

	history, err = service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history after completion: %v", err)
	}
	if len(history) != 3 {
		t.Fatalf("expected 3 audit entries after completion, got %d", len(history))
	}

	outbox, err = service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox messages after completion: %v", err)
	}
	if len(outbox) != 3 {
		t.Fatalf("expected 3 outbox messages after completion, got %d", len(outbox))
	}
}

func TestWorkflowServiceRejectAndResubmit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newWorkflowTestDB(t, ctx)
	service := workflow.NewService(db, workflow.NewRepository(db))

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "invoice-approval",
		DocumentType:   "invoice",
		DocumentID:     9101,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9101",
		Comment:        "submit invoice approval",
	})
	if err != nil {
		t.Fatalf("start invoice workflow: %v", err)
	}

	instance, err = service.Reject(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reject-9101", Comment: "reject invoice"})
	if err != nil {
		t.Fatalf("reject workflow: %v", err)
	}
	if instance.Status != workflow.InstanceStatusRejected {
		t.Fatalf("expected rejected instance, got %#v", instance)
	}

	instance, err = service.Resubmit(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "resubmit-9101", Comment: "resubmit invoice"})
	if err != nil {
		t.Fatalf("resubmit workflow: %v", err)
	}
	if instance.Status != workflow.InstanceStatusPending || instance.CurrentCycle != 2 || instance.CurrentStepOrder == nil || *instance.CurrentStepOrder != 1 {
		t.Fatalf("expected pending resubmitted instance at cycle 2 step 1, got %#v", instance)
	}

	history, err := service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history after resubmit: %v", err)
	}
	if len(history) != 3 {
		t.Fatalf("expected 3 audit entries after reject/resubmit, got %d", len(history))
	}

	outbox, err := service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox messages after resubmit: %v", err)
	}
	if len(outbox) != 3 {
		t.Fatalf("expected 3 outbox messages after reject/resubmit, got %d", len(outbox))
	}
}

func TestWorkflowServiceListInstancesFilters(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newWorkflowTestDB(t, ctx)
	service := workflow.NewService(db, workflow.NewRepository(db))

	leaseInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9201,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9201",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start lease workflow: %v", err)
	}

	invoiceInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "invoice-approval",
		DocumentType:   "invoice",
		DocumentID:     9202,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9202",
		Comment:        "submit invoice approval",
	})
	if err != nil {
		t.Fatalf("start invoice workflow: %v", err)
	}

	invoiceInstance, err = service.Reject(ctx, workflow.TransitionInput{InstanceID: invoiceInstance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reject-9202", Comment: "reject invoice"})
	if err != nil {
		t.Fatalf("reject invoice workflow: %v", err)
	}

	status := workflow.InstanceStatusRejected
	rejectedInstances, err := service.ListInstances(ctx, workflow.InstanceFilter{Status: &status})
	if err != nil {
		t.Fatalf("list rejected workflow instances: %v", err)
	}
	if len(rejectedInstances) != 1 || rejectedInstances[0].ID != invoiceInstance.ID {
		t.Fatalf("expected only rejected invoice instance, got %#v", rejectedInstances)
	}

	documentType := "lease_contract"
	leaseInstances, err := service.ListInstances(ctx, workflow.InstanceFilter{DocumentType: &documentType})
	if err != nil {
		t.Fatalf("list lease workflow instances: %v", err)
	}
	if len(leaseInstances) != 1 || leaseInstances[0].ID != leaseInstance.ID {
		t.Fatalf("expected only lease instance, got %#v", leaseInstances)
	}

	allInstances, err := service.ListInstances(ctx, workflow.InstanceFilter{})
	if err != nil {
		t.Fatalf("list all workflow instances: %v", err)
	}
	if len(allInstances) != 2 {
		t.Fatalf("expected 2 workflow instances, got %d", len(allInstances))
	}
	if allInstances[0].ID != invoiceInstance.ID || allInstances[1].ID != leaseInstance.ID {
		t.Fatalf("expected newest-first ordering, got %#v", allInstances)
	}
	if allInstances[0].DocumentType != "invoice" || allInstances[1].DocumentType != "lease_contract" {
		t.Fatalf("expected stable instance ordering and payloads, got %#v", allInstances)
	}
}

func newWorkflowTestDB(t *testing.T, ctx context.Context) *sql.DB {
	t.Helper()

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
	t.Cleanup(func() { _ = container.Terminate(context.Background()) })

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
		t.Fatalf("open mysql connection: %v", err)
	}
	t.Cleanup(func() { _ = db.Close() })

	if err := waitForDatabase(ctx, db); err != nil {
		t.Fatalf("wait for mysql: %v", err)
	}

	migrator := platformdb.NewMigrator(db, os.DirFS("../platform/database"), "migrations")
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("apply migrations: %v", err)
	}

	bootstrapRunner := platformdb.NewBootstrapRunner(db, bootstrap.All()...)
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("run bootstrap seeds: %v", err)
	}

	return db
}

func waitForDatabase(ctx context.Context, db *sql.DB) error {
	deadline := time.Now().Add(30 * time.Second)
	var lastErr error
	for time.Now().Before(deadline) {
		pingCtx, cancel := context.WithTimeout(ctx, 5*time.Second)
		lastErr = db.PingContext(pingCtx)
		cancel()
		if lastErr == nil {
			return nil
		}
		time.Sleep(500 * time.Millisecond)
	}
	return lastErr
}
