//go:build integration

package excelio_test

import (
	"bytes"
	"context"
	"database/sql"
	"errors"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/excelio"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
	"github.com/xuri/excelize/v2"
)

var excelWorkflowNow = func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }

func TestExcelIOServiceDownloadsUnitTemplate(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newExcelIOTestDB(t, ctx)
	service := excelio.NewService(excelio.NewRepository(db))

	artifact, err := service.DownloadUnitTemplate(ctx)
	if err != nil {
		t.Fatalf("download unit template: %v", err)
	}
	workbook, err := excelize.OpenReader(bytes.NewReader(artifact.Body))
	if err != nil {
		t.Fatalf("open unit template workbook: %v", err)
	}
	defer func() { _ = workbook.Close() }()
	if value, err := workbook.GetCellValue(excelio.UnitSheetName, "A1"); err != nil || value != "code" {
		t.Fatalf("expected unit template header code, got %q err=%v", value, err)
	}
	if value, err := workbook.GetCellValue(excelio.RefSheetName, "A3"); err != nil || value != "BLD-A" {
		t.Fatalf("expected building reference BLD-A, got %q err=%v", value, err)
	}
}

func TestExcelIOServiceImportsUnitsDeterministically(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newExcelIOTestDB(t, ctx)
	service := excelio.NewService(excelio.NewRepository(db))

	workbookBytes := buildUnitWorkbook(t, [][]string{{"U-201", "BLD-A", "F1", "L1", "A01", "shop", "130", "128", "128", "true", "active"}, {"U-202", "BLD-A", "F1", "L1", "A01", "shop", "140", "138", "138", "false", "inactive"}})
	result, err := service.ImportUnits(ctx, bytes.NewReader(workbookBytes))
	if err != nil {
		t.Fatalf("import valid units: %v", err)
	}
	if result.ImportedCount != 2 || len(result.Diagnostics) != 0 {
		t.Fatalf("unexpected import result %#v", result)
	}

	var count int
	if err := db.QueryRowContext(ctx, `SELECT COUNT(*) FROM units WHERE code IN ('U-201', 'U-202')`).Scan(&count); err != nil {
		t.Fatalf("count imported units: %v", err)
	}
	if count != 2 {
		t.Fatalf("expected 2 imported units, got %d", count)
	}

	reimportBytes := buildUnitWorkbook(t, [][]string{{"U-201", "BLD-A", "F1", "L1", "A01", "shop", "150", "148", "148", "true", "active"}})
	result, err = service.ImportUnits(ctx, bytes.NewReader(reimportBytes))
	if err != nil {
		t.Fatalf("reimport existing unit: %v", err)
	}
	if result.ImportedCount != 1 {
		t.Fatalf("expected 1 reimported unit, got %#v", result)
	}
	var floorArea float64
	if err := db.QueryRowContext(ctx, `SELECT floor_area FROM units WHERE code = 'U-201'`).Scan(&floorArea); err != nil {
		t.Fatalf("load updated unit area: %v", err)
	}
	if floorArea != 150 {
		t.Fatalf("expected updated floor_area 150, got %v", floorArea)
	}
}

func TestExcelIOServiceRejectsInvalidImportWithoutPartialCommit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newExcelIOTestDB(t, ctx)
	service := excelio.NewService(excelio.NewRepository(db))

	invalidWorkbook := buildUnitWorkbook(t, [][]string{{"U-301", "BLD-A", "F1", "L1", "A01", "shop", "130", "128", "128", "true", "active"}, {"U-301", "UNKNOWN", "F1", "L1", "A01", "shop", "x", "138", "138", "false", "inactive"}})
	result, err := service.ImportUnits(ctx, bytes.NewReader(invalidWorkbook))
	if !errors.Is(err, excelio.ErrInvalidImport) {
		t.Fatalf("expected invalid import error, got %v", err)
	}
	if len(result.Diagnostics) == 0 {
		t.Fatalf("expected row-level diagnostics, got %#v", result)
	}
	var count int
	if err := db.QueryRowContext(ctx, `SELECT COUNT(*) FROM units WHERE code = 'U-301'`).Scan(&count); err != nil {
		t.Fatalf("count invalid imported units: %v", err)
	}
	if count != 0 {
		t.Fatalf("expected no partial commit for invalid import, got count=%d", count)
	}
}

func TestExcelIOServiceExportsOperationalDatasets(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newExcelIOTestDB(t, ctx)
	service := excelio.NewService(excelio.NewRepository(db))
	prepareApprovedInvoiceForExcel(t, ctx, db)

	invoiceExport, err := service.ExportOperationalDataset(ctx, excelio.ExportInput{Dataset: "invoices"})
	if err != nil {
		t.Fatalf("export invoices dataset: %v", err)
	}
	invoiceWorkbook, err := excelize.OpenReader(bytes.NewReader(invoiceExport.Body))
	if err != nil {
		t.Fatalf("open invoice export workbook: %v", err)
	}
	defer func() { _ = invoiceWorkbook.Close() }()
	sheet := invoiceWorkbook.GetSheetName(0)
	if value, err := invoiceWorkbook.GetCellValue(sheet, "A1"); err != nil || value != "document_no" {
		t.Fatalf("expected invoice export header, got %q err=%v", value, err)
	}
	if value, err := invoiceWorkbook.GetCellValue(sheet, "A2"); err != nil || value != "INV-101" {
		t.Fatalf("expected invoice export first document INV-101, got %q err=%v", value, err)
	}

	chargeExport, err := service.ExportOperationalDataset(ctx, excelio.ExportInput{Dataset: "billing_charges"})
	if err != nil {
		t.Fatalf("export charges dataset: %v", err)
	}
	chargeWorkbook, err := excelize.OpenReader(bytes.NewReader(chargeExport.Body))
	if err != nil {
		t.Fatalf("open charge export workbook: %v", err)
	}
	defer func() { _ = chargeWorkbook.Close() }()
	chargeSheet := chargeWorkbook.GetSheetName(0)
	if value, err := chargeWorkbook.GetCellValue(chargeSheet, "A1"); err != nil || value != "lease_no" {
		t.Fatalf("expected charge export header, got %q err=%v", value, err)
	}
	if value, err := chargeWorkbook.GetCellValue(chargeSheet, "A2"); err != nil || value != "CON-EXCEL-101" {
		t.Fatalf("expected charge export lease number CON-EXCEL-101, got %q err=%v", value, err)
	}
}

func buildUnitWorkbook(t *testing.T, dataRows [][]string) []byte {
	t.Helper()
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	_ = f.SetSheetName(f.GetSheetName(0), excelio.UnitSheetName)
	headers := []string{"code", "building_code", "floor_code", "location_code", "area_code", "unit_type_code", "floor_area", "use_area", "rent_area", "is_rentable", "status"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(excelio.UnitSheetName, cell, header)
	}
	for rowIndex, row := range dataRows {
		for colIndex, value := range row {
			cell, _ := excelize.CoordinatesToCellName(colIndex+1, rowIndex+2)
			_ = f.SetCellValue(excelio.UnitSheetName, cell, value)
		}
	}
	buffer, err := f.WriteToBuffer()
	if err != nil {
		t.Fatalf("write unit workbook: %v", err)
	}
	return buffer.Bytes()
}

func newExcelIOTestDB(t *testing.T, ctx context.Context) *sql.DB {
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

func prepareApprovedInvoiceForExcel(t *testing.T, ctx context.Context, db *sql.DB) {
	t.Helper()
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, excelWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)
	start := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	end := time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)
	buildingID := int64(101)
	draft, err := leaseService.CreateDraft(ctx, lease.CreateDraftInput{LeaseNo: "CON-EXCEL-101", DepartmentID: 101, StoreID: 101, BuildingID: &buildingID, TenantName: "ACME Retail", StartDate: start, EndDate: end, Units: []lease.UnitInput{{UnitID: 101, RentArea: 118}}, Terms: []lease.TermInput{{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: 12000, EffectiveFrom: start, EffectiveTo: end}}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	submittedLease, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-excel-lease", Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submittedLease.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-excel-lease-1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submittedLease.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-excel-lease-2", Comment: "finance approved"})
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
	invoiceSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-excel-invoice", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice: %v", err)
	}
	invoiceApproved, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *invoiceSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-excel-invoice", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, invoiceApproved, 101); err != nil {
		t.Fatalf("sync invoice approval: %v", err)
	}
}
