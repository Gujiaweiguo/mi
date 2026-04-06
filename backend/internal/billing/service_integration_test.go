//go:build integration

package billing_test

import (
	"context"
	"database/sql"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

var billingWorkflowNow = func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }

func TestBillingServiceGenerateChargesAndDeduplicate(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newBillingTestDB(t, ctx)
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, billingWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingService := billing.NewService(db, billing.NewRepository(db))

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-B101", 12000, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)), "submit-b101")

	result, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate April charges: %v", err)
	}
	if result.Totals.Generated != 1 || result.Totals.Skipped != 0 || len(result.Lines) != 1 {
		t.Fatalf("expected one generated charge line, got %#v", result)
	}
	line := result.Lines[0]
	if line.LeaseContractID != activeLease.ID || line.Amount != 12000 || line.QuantityDays != 30 || line.SourceEffectiveVersion != 1 {
		t.Fatalf("unexpected generated charge line %#v", line)
	}

	rerun, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("rerun April charges: %v", err)
	}
	if rerun.Totals.Generated != 0 || rerun.Totals.Skipped != 1 {
		t.Fatalf("expected rerun to skip duplicate charge, got %#v", rerun)
	}

	list, err := billingService.ListChargeLines(ctx, billing.ChargeListFilter{LeaseContractID: &activeLease.ID})
	if err != nil {
		t.Fatalf("list charge lines: %v", err)
	}
	if list.Total != 1 || len(list.Items) != 1 {
		t.Fatalf("expected one persisted charge line, got %#v", list)
	}
}

func TestBillingServiceUsesAmendedLeaseForFutureCharges(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newBillingTestDB(t, ctx)
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, billingWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingService := billing.NewService(db, billing.NewRepository(db))

	baseLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-B201", 12000, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)), "submit-b201")
	amendmentInput := newLeaseCreateInput("CON-B201A", 15000, time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC))
	amendment, err := leaseService.CreateAmendment(ctx, lease.AmendInput{LeaseID: baseLease.ID, CreateDraftInput: amendmentInput})
	if err != nil {
		t.Fatalf("create amendment lease: %v", err)
	}
	activateAmendment(t, ctx, leaseService, workflowService, amendment.ID, "submit-b201a")

	result, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate May charges after amendment: %v", err)
	}
	if result.Totals.Generated != 1 || len(result.Lines) != 1 {
		t.Fatalf("expected one amendment-backed charge line, got %#v", result)
	}
	line := result.Lines[0]
	if line.LeaseContractID != amendment.ID || line.Amount != 15000 || line.SourceEffectiveVersion != 2 {
		t.Fatalf("expected amended lease charge line, got %#v", line)
	}

	terminatedBase, err := leaseService.GetLease(ctx, baseLease.ID)
	if err != nil {
		t.Fatalf("get terminated base lease: %v", err)
	}
	if terminatedBase.Status != lease.StatusTerminated {
		t.Fatalf("expected base lease terminated after amendment activation, got %#v", terminatedBase)
	}
}

func TestBillingServiceProratesTerminationCutoff(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newBillingTestDB(t, ctx)
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, billingWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingService := billing.NewService(db, billing.NewRepository(db))

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-B301", 12000, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)), "submit-b301")
	if _, err := leaseService.Terminate(ctx, lease.TerminateInput{LeaseID: activeLease.ID, ActorUserID: 101, TerminatedAt: time.Date(2026, 4, 15, 0, 0, 0, 0, time.UTC)}); err != nil {
		t.Fatalf("terminate active lease: %v", err)
	}

	result, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate April charges after termination: %v", err)
	}
	if result.Totals.Generated != 1 || len(result.Lines) != 1 {
		t.Fatalf("expected one prorated charge line, got %#v", result)
	}
	line := result.Lines[0]
	if line.LeaseContractID != activeLease.ID || line.QuantityDays != 14 || line.Amount != 5600 {
		t.Fatalf("expected prorated terminated lease charge line, got %#v", line)
	}

	mayResult, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate May charges after termination: %v", err)
	}
	if mayResult.Totals.Generated != 0 {
		t.Fatalf("expected no future charges after termination, got %#v", mayResult)
	}
}

func TestBillingServiceExcludesPendingApprovalLease(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newBillingTestDB(t, ctx)
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, billingWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingService := billing.NewService(db, billing.NewRepository(db))

	draft, err := leaseService.CreateDraft(ctx, newLeaseCreateInput("CON-B401", 12000, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)))
	if err != nil {
		t.Fatalf("create draft lease: %v", err)
	}
	if _, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-b401", Comment: "submit lease"}); err != nil {
		t.Fatalf("submit lease: %v", err)
	}

	result, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges for pending lease: %v", err)
	}
	if result.Totals.Generated != 0 || len(result.Lines) != 0 {
		t.Fatalf("expected pending-approval lease to be excluded from charge generation, got %#v", result)
	}
}

func newBillingTestDB(t *testing.T, ctx context.Context) *sql.DB {
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

func newLeaseCreateInput(leaseNo string, amount float64, effectiveFrom, effectiveTo time.Time) lease.CreateDraftInput {
	return lease.CreateDraftInput{
		LeaseNo:      leaseNo,
		DepartmentID: 101,
		StoreID:      101,
		BuildingID:   int64Pointer(101),
		TenantName:   "ACME Retail",
		StartDate:    effectiveFrom,
		EndDate:      effectiveTo,
		Units:        []lease.UnitInput{{UnitID: 101, RentArea: 118}},
		Terms: []lease.TermInput{{
			TermType:       lease.TermTypeRent,
			BillingCycle:   lease.BillingCycleMonthly,
			CurrencyTypeID: 101,
			Amount:         amount,
			EffectiveFrom:  effectiveFrom,
			EffectiveTo:    effectiveTo,
		}},
		ActorUserID: 101,
	}
}

func activateLease(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service, input lease.CreateDraftInput, submitKey string) *lease.Contract {
	t.Helper()
	draft, err := leaseService.CreateDraft(ctx, input)
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	activateAmendment(t, ctx, leaseService, workflowService, draft.ID, submitKey)
	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get active lease: %v", err)
	}
	return active
}

func activateAmendment(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service, leaseID int64, submitKey string) {
	t.Helper()
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: leaseID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey, Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey + "-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve billing lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync billing lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey + "-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve billing lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync billing lease step 2: %v", err)
	}
}

func int64Pointer(value int64) *int64 {
	return &value
}
