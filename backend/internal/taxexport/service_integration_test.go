//go:build integration

package taxexport_test

import (
	"bytes"
	"context"
	"database/sql"
	"errors"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/taxexport"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
	"github.com/xuri/excelize/v2"
)

func TestTaxExportServiceGeneratesKingdeeWorkbook(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newTaxExportTestDB(t, ctx)
	service := taxexport.NewService(taxexport.NewRepository(db))
	prepareApprovedInvoice(t, ctx, db)
	fromDate := time.Date(2026, 1, 1, 0, 0, 0, 0, time.UTC)
	toDate := time.Date(2026, 12, 31, 0, 0, 0, 0, time.UTC)

	_, err := service.UpsertRuleSet(ctx, taxexport.UpsertRuleSetInput{Code: "kingdee-default", Name: "Kingdee Default", DocumentType: "invoice", Status: taxexport.RuleSetStatusActive, ActorUserID: 101, Rules: []taxexport.UpsertRuleInput{{SequenceNo: 1, EntrySide: taxexport.EntrySideDebit, ChargeTypeFilter: "rent", AccountNumber: "1122", AccountName: "应收账款", ExplanationTemplate: "YYYYMMDD-YYYYMMDD ITEMCODE", UseTenantName: true}, {SequenceNo: 2, EntrySide: taxexport.EntrySideCredit, ChargeTypeFilter: "rent", AccountNumber: "6001", AccountName: "租金收入", ExplanationTemplate: "SYYYYMM ITEMCODE", UseTenantName: false}}})
	if err != nil {
		t.Fatalf("upsert tax rule set: %v", err)
	}

	artifact, err := service.ExportVoucherWorkbook(ctx, taxexport.ExportInput{RuleSetCode: "kingdee-default", FromDate: fromDate, ToDate: toDate, ActorUserID: 101})
	if err != nil {
		t.Fatalf("export tax workbook: %v", err)
	}
	if artifact.EntryCount != 2 || artifact.DocumentCount != 1 || len(artifact.Bytes) == 0 {
		t.Fatalf("unexpected tax export artifact %#v", artifact)
	}
	workbook, err := excelize.OpenReader(bytes.NewReader(artifact.Bytes))
	if err != nil {
		t.Fatalf("open exported workbook: %v", err)
	}
	defer func() { _ = workbook.Close() }()
	sheet := workbook.GetSheetName(0)
	if value, err := workbook.GetCellValue(sheet, "A1"); err != nil || value != "FDate" {
		t.Fatalf("expected FDate header, got %q err=%v", value, err)
	}
	if value, err := workbook.GetCellValue(sheet, "D2"); err != nil || value != "INV-101" {
		t.Fatalf("expected first voucher group INV-101, got %q err=%v", value, err)
	}
	if value, err := workbook.GetCellValue(sheet, "F2"); err != nil || value != "'1122" {
		t.Fatalf("expected debit account number, got %q err=%v", value, err)
	}
	if value, err := workbook.GetCellValue(sheet, "L3"); err != nil || value != "12000" {
		t.Fatalf("expected credit amount 12000, got %q err=%v", value, err)
	}
}

func TestTaxExportServiceFailsFastOnInvalidSetup(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newTaxExportTestDB(t, ctx)
	service := taxexport.NewService(taxexport.NewRepository(db))
	prepareApprovedInvoice(t, ctx, db)
	fromDate := time.Date(2026, 1, 1, 0, 0, 0, 0, time.UTC)
	toDate := time.Date(2026, 12, 31, 0, 0, 0, 0, time.UTC)

	_, err := service.UpsertRuleSet(ctx, taxexport.UpsertRuleSetInput{Code: "broken-default", Name: "Broken Default", DocumentType: "invoice", Status: taxexport.RuleSetStatusActive, ActorUserID: 101, Rules: []taxexport.UpsertRuleInput{{SequenceNo: 1, EntrySide: taxexport.EntrySideDebit, ChargeTypeFilter: "rent", AccountNumber: "1122", AccountName: "应收账款", ExplanationTemplate: "YYYYMMDD-YYYYMMDD ITEMCODE", UseTenantName: true}}})
	if err != nil {
		t.Fatalf("upsert broken tax rule set: %v", err)
	}

	_, err = service.ExportVoucherWorkbook(ctx, taxexport.ExportInput{RuleSetCode: "broken-default", FromDate: fromDate, ToDate: toDate, ActorUserID: 101})
	if !errors.Is(err, taxexport.ErrInvalidTaxSetup) {
		t.Fatalf("expected invalid tax setup error, got %v", err)
	}
}

func newTaxExportTestDB(t *testing.T, ctx context.Context) *sql.DB {
	t.Helper()
	container, err := testcontainers.GenericContainer(ctx, testcontainers.GenericContainerRequest{ContainerRequest: testcontainers.ContainerRequest{Image: "mysql:8.0", ExposedPorts: []string{"3306/tcp"}, Env: map[string]string{"MYSQL_DATABASE": "mi_integration", "MYSQL_USER": "mi_user", "MYSQL_PASSWORD": "mi_password", "MYSQL_ROOT_PASSWORD": "mi_root_password"}, WaitingFor: wait.ForListeningPort("3306/tcp").WithStartupTimeout(3 * time.Minute)}, Started: true})
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

func prepareApprovedInvoice(t *testing.T, ctx context.Context, db *sql.DB) {
	t.Helper()
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)
	start := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	end := time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)
	buildingID := int64(101)
	draft, err := leaseService.CreateDraft(ctx, lease.CreateDraftInput{LeaseNo: "CON-TAX-101", DepartmentID: 101, StoreID: 101, BuildingID: &buildingID, TenantName: "ACME Retail", StartDate: start, EndDate: end, Units: []lease.UnitInput{{UnitID: 101, RentArea: 118}}, Terms: []lease.TermInput{{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: 12000, EffectiveFrom: start, EffectiveTo: end}}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-tax-lease", Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-tax-lease-1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-tax-lease-2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 2: %v", err)
	}
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice: %v", err)
	}
	invoiceSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-tax-invoice", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice: %v", err)
	}
	invoiceApproved, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *invoiceSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-tax-invoice", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, invoiceApproved, 101); err != nil {
		t.Fatalf("sync invoice approval: %v", err)
	}
}
