//go:build integration

package reporting_test

import (
	"bytes"
	"context"
	"database/sql"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/reporting"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
	"github.com/xuri/excelize/v2"
)

func TestReportingServiceQueryAndExportCoreReports(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := newReportingTestDB(t, ctx)
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	service := reporting.NewService(reporting.NewRepository(db))

	activeLease := activateReportingLease(t, ctx, leaseService, workflowService)
	if activeLease.Status != lease.StatusActive {
		t.Fatalf("expected active lease, got %#v", activeLease)
	}
	seedAROpenItems(t, ctx, db, activeLease.ID)
	seedBudgetFacts(t, ctx, db, activeLease.ID)

	periodStart, periodEnd, periodLabel, err := reporting.ParsePeriod("2026-04")
	if err != nil {
		t.Fatalf("parse period: %v", err)
	}
	storeID := int64(101)
	shopTypeID := int64(101)

	for _, reportID := range []reporting.ReportID{reporting.ReportR01, reporting.ReportR02, reporting.ReportR03, reporting.ReportR04, reporting.ReportR05, reporting.ReportR06, reporting.ReportR07, reporting.ReportR08, reporting.ReportR09, reporting.ReportR10, reporting.ReportR11, reporting.ReportR12, reporting.ReportR13, reporting.ReportR14, reporting.ReportR15, reporting.ReportR16, reporting.ReportR17, reporting.ReportR18, reporting.ReportID("r19")} {
		result, err := service.QueryReport(ctx, reporting.QueryInput{
			ReportID:      reportID,
			PeriodStart:   periodStart,
			PeriodEnd:     periodEnd,
			PeriodLabel:   periodLabel,
			StoreID:       &storeID,
			FloorID:       int64Ptr(101),
			AreaID:        int64Ptr(101),
			UnitID:        int64Ptr(101),
			DepartmentID:  int64Ptr(101),
			CustomerID:    int64Ptr(101),
			ShopTypeID:    &shopTypeID,
			TradeID:       int64Ptr(102),
			ChargeType:    stringPointer("rent"),
			Status:        stringPointer("active"),
			RequestedByID: 101,
		})
		if err != nil {
			t.Fatalf("query %s: %v", reportID, err)
		}
		if result.ReportID != reportID || len(result.Columns) == 0 || len(result.Rows) == 0 {
			t.Fatalf("expected populated result for %s, got %#v", reportID, result)
		}

		artifact, err := service.ExportReport(ctx, reporting.QueryInput{
			ReportID:      reportID,
			PeriodStart:   periodStart,
			PeriodEnd:     periodEnd,
			PeriodLabel:   periodLabel,
			StoreID:       &storeID,
			FloorID:       int64Ptr(101),
			AreaID:        int64Ptr(101),
			UnitID:        int64Ptr(101),
			DepartmentID:  int64Ptr(101),
			CustomerID:    int64Ptr(101),
			ShopTypeID:    &shopTypeID,
			TradeID:       int64Ptr(102),
			ChargeType:    stringPointer("rent"),
			Status:        stringPointer("active"),
			RequestedByID: 101,
		})
		if err != nil {
			t.Fatalf("export %s: %v", reportID, err)
		}
		file, err := excelize.OpenReader(bytes.NewReader(artifact.Bytes))
		if err != nil {
			t.Fatalf("open workbook %s: %v", reportID, err)
		}
		rows, err := file.GetRows(file.GetSheetName(0))
		_ = file.Close()
		if err != nil {
			t.Fatalf("read workbook rows %s: %v", reportID, err)
		}
		if len(rows) < 2 {
			t.Fatalf("expected workbook data rows for %s, got %v", reportID, rows)
		}
	}
}
func activateReportingLease(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service) *lease.Contract {
	t.Helper()
	input := lease.CreateDraftInput{
		LeaseNo:          "RPT-101",
		DepartmentID:     101,
		StoreID:          101,
		BuildingID:       int64Ptr(101),
		CustomerID:       int64Ptr(101),
		BrandID:          int64Ptr(101),
		TradeID:          int64Ptr(102),
		ManagementTypeID: int64Ptr(101),
		TenantName:       "Report Tenant",
		StartDate:        time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
		EndDate:          time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
		Units:            []lease.UnitInput{{UnitID: 101, RentArea: 118}},
		Terms: []lease.TermInput{{
			TermType:       lease.TermTypeRent,
			BillingCycle:   lease.BillingCycleMonthly,
			CurrencyTypeID: 101,
			Amount:         12000,
			EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
		}},
		ActorUserID: 101,
	}
	draft, err := leaseService.CreateDraft(ctx, input)
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reporting-submit", Comment: "submit reporting lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reporting-approve-1", Comment: "approve 1"})
	if err != nil {
		t.Fatalf("approve lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reporting-approve-2", Comment: "approve 2"})
	if err != nil {
		t.Fatalf("approve lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 2: %v", err)
	}
	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get lease: %v", err)
	}
	return active
}

func seedAROpenItems(t *testing.T, ctx context.Context, db *sql.DB, leaseID int64) {
	t.Helper()
	entries := []struct {
		chargeType string
		dueDate    string
		amount     float64
		isDeposit  bool
	}{
		{chargeType: "rent", dueDate: "2026-04-15", amount: 500, isDeposit: false},
		{chargeType: "rent", dueDate: "2026-03-10", amount: 1000, isDeposit: false},
		{chargeType: "service", dueDate: "2026-02-10", amount: 2000, isDeposit: false},
		{chargeType: "deposit", dueDate: "2026-01-01", amount: 3000, isDeposit: true},
	}
	for _, entry := range entries {
		if _, err := db.ExecContext(ctx, `
			INSERT INTO ar_open_items (lease_contract_id, customer_id, department_id, trade_id, charge_type, due_date, outstanding_amount, is_deposit)
			VALUES (?, ?, ?, ?, ?, ?, ?, ?)
		`, leaseID, 101, 101, 102, entry.chargeType, entry.dueDate, entry.amount, entry.isDeposit); err != nil {
			t.Fatalf("seed ar open item: %v", err)
		}
	}
}

func seedBudgetFacts(t *testing.T, ctx context.Context, db *sql.DB, leaseID int64) {
	t.Helper()
	var leaseTermID int64
	if err := db.QueryRowContext(ctx, `SELECT id FROM lease_contract_terms WHERE lease_contract_id = ? AND term_type = 'rent' ORDER BY id LIMIT 1`, leaseID).Scan(&leaseTermID); err != nil {
		t.Fatalf("load lease term id: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO unit_rent_budgets (unit_id, fiscal_year, budget_price) VALUES (101, 2026, 95.00) ON DUPLICATE KEY UPDATE budget_price = VALUES(budget_price)`); err != nil {
		t.Fatalf("seed unit rent budgets: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO unit_prospects (unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id, avg_transaction, prospect_rent_price, rent_increment, prospect_term_months) VALUES (101, 2026, 101, 101, 102, 280.00, 110.00, '5% yearly', 36) ON DUPLICATE KEY UPDATE potential_customer_id = VALUES(potential_customer_id), prospect_brand_id = VALUES(prospect_brand_id), prospect_trade_id = VALUES(prospect_trade_id), avg_transaction = VALUES(avg_transaction), prospect_rent_price = VALUES(prospect_rent_price), rent_increment = VALUES(rent_increment), prospect_term_months = VALUES(prospect_term_months)`); err != nil {
		t.Fatalf("seed unit prospects: %v", err)
	}
	for month := 1; month <= 12; month++ {
		if _, err := db.ExecContext(ctx, `INSERT INTO store_rent_budgets (store_id, fiscal_year, fiscal_month, monthly_budget) VALUES (?, 2026, ?, 10000.00) ON DUPLICATE KEY UPDATE monthly_budget = VALUES(monthly_budget)`, 101, month); err != nil {
			t.Fatalf("seed store rent budget month %d: %v", month, err)
		}
	}
	result, err := db.ExecContext(ctx, `INSERT INTO billing_runs (period_start, period_end, status, triggered_by, generated_count, skipped_count) VALUES ('2026-04-01', '2026-04-30', 'completed', 101, 1, 0)`)
	if err != nil {
		t.Fatalf("seed billing run: %v", err)
	}
	billingRunID, err := result.LastInsertId()
	if err != nil {
		t.Fatalf("billing run id: %v", err)
	}
	chargeResult, err := db.ExecContext(ctx, `INSERT INTO billing_charge_lines (billing_run_id, lease_contract_id, lease_term_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount, currency_type_id, source_effective_version) VALUES (?, ?, ?, 'rent', '2026-04-01', '2026-04-30', 30, 12000.00, 12000.00, 101, 1)`, billingRunID, leaseID, leaseTermID)
	if err != nil {
		t.Fatalf("seed billing charge line: %v", err)
	}
	chargeLineID, err := chargeResult.LastInsertId()
	if err != nil {
		t.Fatalf("billing charge line id: %v", err)
	}
	docResult, err := db.ExecContext(ctx, `INSERT INTO billing_documents (document_type, document_no, billing_run_id, lease_contract_id, tenant_name, period_start, period_end, total_amount, currency_type_id, status, approved_at, created_by, updated_by) VALUES ('invoice', 'INV-2026-04', ?, ?, 'Report Tenant', '2026-04-01', '2026-04-30', 9000.00, 101, 'approved', NOW(), 101, 101)`, billingRunID, leaseID)
	if err != nil {
		t.Fatalf("seed billing document: %v", err)
	}
	documentID, err := docResult.LastInsertId()
	if err != nil {
		t.Fatalf("billing document id: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO billing_document_lines (billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount) VALUES (?, ?, 'rent', '2026-04-01', '2026-04-30', 30, 12000.00, 9000.00)`, documentID, chargeLineID); err != nil {
		t.Fatalf("seed billing document line: %v", err)
	}
	priorChargeResult, err := db.ExecContext(ctx, `INSERT INTO billing_charge_lines (billing_run_id, lease_contract_id, lease_term_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount, currency_type_id, source_effective_version) VALUES (?, ?, ?, 'rent', '2026-03-01', '2026-03-31', 31, 12000.00, 8000.00, 101, 1)`, billingRunID, leaseID, leaseTermID)
	if err != nil {
		t.Fatalf("seed prior billing charge line: %v", err)
	}
	priorChargeLineID, err := priorChargeResult.LastInsertId()
	if err != nil {
		t.Fatalf("prior billing charge line id: %v", err)
	}
	priorDocResult, err := db.ExecContext(ctx, `INSERT INTO billing_documents (document_type, document_no, billing_run_id, lease_contract_id, tenant_name, period_start, period_end, total_amount, currency_type_id, status, approved_at, created_by, updated_by) VALUES ('invoice', 'INV-2026-03', ?, ?, 'Report Tenant', '2026-03-01', '2026-03-31', 8000.00, 101, 'approved', NOW(), 101, 101)`, billingRunID, leaseID)
	if err != nil {
		t.Fatalf("seed prior billing document: %v", err)
	}
	priorDocumentID, err := priorDocResult.LastInsertId()
	if err != nil {
		t.Fatalf("prior billing document id: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO billing_document_lines (billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount) VALUES (?, ?, 'rent', '2026-03-01', '2026-03-31', 31, 12000.00, 8000.00)`, priorDocumentID, priorChargeLineID); err != nil {
		t.Fatalf("seed prior billing document line: %v", err)
	}
}

func int64Ptr(value int64) *int64 { return &value }

func stringPointer(value string) *string { return &value }

func newReportingTestDB(t *testing.T, ctx context.Context) *sql.DB {
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
		t.Fatalf("open mysql: %v", err)
	}
	t.Cleanup(func() { _ = db.Close() })
	if err := waitForReportingDatabase(ctx, db); err != nil {
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
	return db
}

func waitForReportingDatabase(ctx context.Context, db *sql.DB) error {
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
