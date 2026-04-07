//go:build integration

package reporting_test

import (
	"bytes"
	"context"
	"database/sql"
	"math"
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
	seedDailyShopSales(t, ctx, db)
	seedBudgetFacts(t, ctx, db, activeLease.ID)

	periodStart, periodEnd, periodLabel, err := reporting.ParsePeriod("2026-04")
	if err != nil {
		t.Fatalf("parse period: %v", err)
	}
	storeID := int64(101)
	shopTypeID := int64(101)
	baseInput := reporting.QueryInput{
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
	}

	for _, reportID := range []reporting.ReportID{reporting.ReportR01, reporting.ReportR02, reporting.ReportR03, reporting.ReportR04, reporting.ReportR05, reporting.ReportR06, reporting.ReportR07, reporting.ReportR08, reporting.ReportR09, reporting.ReportR10, reporting.ReportR11, reporting.ReportR12, reporting.ReportR13, reporting.ReportR14, reporting.ReportR15, reporting.ReportR16, reporting.ReportR17, reporting.ReportR18, reporting.ReportID("r19")} {
		queryInput := baseInput
		queryInput.ReportID = reportID
		result, err := service.QueryReport(ctx, queryInput)
		if err != nil {
			t.Fatalf("query %s: %v", reportID, err)
		}
		if result.ReportID != reportID || len(result.Columns) == 0 || len(result.Rows) == 0 {
			t.Fatalf("expected populated result for %s, got %#v", reportID, result)
		}

		artifact, err := service.ExportReport(ctx, queryInput)
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

	t.Run("R02 and R11 preserve contract and area semantics", func(t *testing.T) {
		r02 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR02))
		if len(r02.Rows) != 1 {
			t.Fatalf("expected one contract ledger row, got %d", len(r02.Rows))
		}
		contractRow := r02.Rows[0]
		if got := rowString(t, contractRow, "lease_no"); got != "RPT-101" {
			t.Fatalf("expected seeded lease number, got %q", got)
		}
		if got := rowFloat64(t, contractRow, "rent_area"); got != 118 {
			t.Fatalf("expected seeded rent area 118, got %v", got)
		}
		for _, key := range []string{"customer_name", "trade_name", "management_type_name", "unit_code", "unit_name", "brand_name", "shop_type_name", "department_name", "store_name"} {
			if value := rowString(t, contractRow, key); value == "" {
				t.Fatalf("expected %s to be populated", key)
			}
		}

		r11 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR11))
		if len(r11.Rows) != 1 {
			t.Fatalf("expected one leased area row, got %d", len(r11.Rows))
		}
		areaRow := r11.Rows[0]
		if got := rowString(t, areaRow, "period"); got != periodLabel {
			t.Fatalf("expected period %q, got %q", periodLabel, got)
		}
		if got := rowString(t, areaRow, "store_name"); got != rowString(t, contractRow, "store_name") {
			t.Fatalf("expected store names to align, got R02=%q R11=%q", rowString(t, contractRow, "store_name"), got)
		}
		leasedArea := rowFloat64(t, areaRow, "leased_area")
		totalArea := rowFloat64(t, areaRow, "total_area")
		if leasedArea <= 0 {
			t.Fatalf("expected seeded lease to contribute positive leased area, got %v", leasedArea)
		}
		if leasedArea > totalArea {
			t.Fatalf("expected leased area <= total area, got leased=%v total=%v", leasedArea, totalArea)
		}
	})

	t.Run("R03 and R14 preserve sales and efficiency semantics", func(t *testing.T) {
		r03 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR03))
		if len(r03.Rows) != 1 {
			t.Fatalf("expected one sales analysis row, got %d", len(r03.Rows))
		}
		salesRow := r03.Rows[0]
		if got := rowFloat64(t, salesRow, "rent_area"); got != 118 {
			t.Fatalf("expected seeded rent area 118, got %v", got)
		}
		currentSales := rowFloat64(t, salesRow, "current_sales")
		if currentSales <= 0 {
			t.Fatalf("expected positive current sales, got %v", currentSales)
		}
		samePeriodSales := rowFloat64(t, salesRow, "same_period_sales")
		if samePeriodSales <= 0 {
			t.Fatalf("expected positive same-period sales, got %v", samePeriodSales)
		}
		if got := rowFloat64(t, salesRow, "comparable_sales"); got != samePeriodSales {
			t.Fatalf("expected comparable sales %v to match same-period sales, got %v", samePeriodSales, got)
		}
		if got := rowFloat64(t, salesRow, "monthly_rent"); got != 12000 {
			t.Fatalf("expected active monthly rent 12000, got %v", got)
		}

		r14 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR14))
		if len(r14.Rows) != 1 {
			t.Fatalf("expected one efficiency row, got %d", len(r14.Rows))
		}
		efficiencyRow := r14.Rows[0]
		if got := rowString(t, efficiencyRow, "period"); got != periodLabel {
			t.Fatalf("expected period %q, got %q", periodLabel, got)
		}
		if got := rowFloat64(t, efficiencyRow, "sales_amount"); got != currentSales {
			t.Fatalf("expected efficiency sales %v to align with current sales report, got %v", currentSales, got)
		}
		if got := rowInt(t, efficiencyRow, "days_in_month"); got != 30 {
			t.Fatalf("expected 30 days in month, got %d", got)
		}
		areaTotal := rowFloat64(t, efficiencyRow, "area_total")
		efficiency := rowOptionalFloat64(t, efficiencyRow, "efficiency")
		if areaTotal <= 0 || efficiency == nil {
			t.Fatalf("expected non-zero area and calculated efficiency, got area=%v efficiency=%v", areaTotal, efficiency)
		}
		expected := rowFloat64(t, efficiencyRow, "sales_amount") / float64(rowInt(t, efficiencyRow, "days_in_month")) / areaTotal
		if math.Abs(*efficiency-expected) > 1e-9 {
			t.Fatalf("expected efficiency %v, got %v", expected, *efficiency)
		}
	})

	t.Run("R08 reconciles with R16 and R17 aging outputs", func(t *testing.T) {
		r08 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR08))
		if len(r08.Rows) != 1 {
			t.Fatalf("expected one customer aging row, got %d", len(r08.Rows))
		}
		customerRow := r08.Rows[0]
		assertAgingBucketTotal(t, customerRow)
		if got := rowFloat64(t, customerRow, "deposit_amount"); got != 3000 {
			t.Fatalf("expected deposit bucket to stay separate at 3000, got %v", got)
		}
		if got := rowFloat64(t, customerRow, "within_one_month"); got != 500 {
			t.Fatalf("expected <=30 day bucket 500, got %v", got)
		}
		if got := rowFloat64(t, customerRow, "one_to_two_months"); got != 1000 {
			t.Fatalf("expected 31-60 day bucket 1000, got %v", got)
		}
		if got := rowFloat64(t, customerRow, "two_to_three_months"); got != 2000 {
			t.Fatalf("expected 61-90 day bucket 2000, got %v", got)
		}
		if got := rowFloat64(t, customerRow, "total"); got != 3500 {
			t.Fatalf("expected non-deposit aging total 3500, got %v", got)
		}

		r16 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR16))
		if len(r16.Rows) != 1 {
			t.Fatalf("expected one department aging row, got %d", len(r16.Rows))
		}
		departmentRow := r16.Rows[0]
		assertAgingBucketTotal(t, departmentRow)
		if rowFloat64(t, departmentRow, "deposit_amount") != rowFloat64(t, customerRow, "deposit_amount") || rowFloat64(t, departmentRow, "total") != rowFloat64(t, customerRow, "total") {
			t.Fatalf("expected department aging to reconcile with customer aging, got customer deposit/total=%v/%v department=%v/%v", rowFloat64(t, customerRow, "deposit_amount"), rowFloat64(t, customerRow, "total"), rowFloat64(t, departmentRow, "deposit_amount"), rowFloat64(t, departmentRow, "total"))
		}

		r17Input := withReportID(baseInput, reporting.ReportR17)
		r17Input.ChargeType = nil
		r17 := mustQueryReport(t, ctx, service, r17Input)
		if len(r17.Rows) == 0 {
			t.Fatal("expected department aging-by-charge rows")
		}
		chargeRows := make(map[string]map[string]any, len(r17.Rows))
		var chargeTypeDeposit, chargeTypeTotal float64
		for _, row := range r17.Rows {
			assertAgingBucketTotal(t, row)
			chargeType := rowString(t, row, "charge_type")
			chargeRows[chargeType] = row
			chargeTypeDeposit += rowFloat64(t, row, "deposit_amount")
			chargeTypeTotal += rowFloat64(t, row, "total")
		}
		if got := rowFloat64(t, chargeRows["rent"], "total"); got != 1500 {
			t.Fatalf("expected rent aging total 1500, got %v", got)
		}
		if got := rowFloat64(t, chargeRows["service"], "total"); got != 2000 {
			t.Fatalf("expected service aging total 2000, got %v", got)
		}
		if got := rowFloat64(t, chargeRows["deposit"], "deposit_amount"); got != 3000 {
			t.Fatalf("expected deposit row to carry 3000 deposit, got %v", got)
		}
		if chargeTypeDeposit != rowFloat64(t, departmentRow, "deposit_amount") || chargeTypeTotal != rowFloat64(t, departmentRow, "total") {
			t.Fatalf("expected aging-by-charge to reconcile with department aging, got charge deposit/total=%v/%v department=%v/%v", chargeTypeDeposit, chargeTypeTotal, rowFloat64(t, departmentRow, "deposit_amount"), rowFloat64(t, departmentRow, "total"))
		}
	})
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

func seedDailyShopSales(t *testing.T, ctx context.Context, db *sql.DB) {
	t.Helper()
	entries := []struct {
		saleDate string
		amount   float64
	}{
		{saleDate: "2026-04-05", amount: 3100},
		{saleDate: "2026-04-18", amount: 900},
		{saleDate: "2025-04-10", amount: 3000},
	}
	for _, entry := range entries {
		if _, err := db.ExecContext(ctx, `INSERT INTO daily_shop_sales (store_id, unit_id, sale_date, sales_amount) VALUES (101, 101, ?, ?) ON DUPLICATE KEY UPDATE sales_amount = VALUES(sales_amount)`, entry.saleDate, entry.amount); err != nil {
			t.Fatalf("seed daily shop sale: %v", err)
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

func withReportID(input reporting.QueryInput, reportID reporting.ReportID) reporting.QueryInput {
	input.ReportID = reportID
	return input
}

func mustQueryReport(t *testing.T, ctx context.Context, service *reporting.Service, input reporting.QueryInput) *reporting.Result {
	t.Helper()
	result, err := service.QueryReport(ctx, input)
	if err != nil {
		t.Fatalf("query %s: %v", input.ReportID, err)
	}
	return result
}

func assertAgingBucketTotal(t *testing.T, row map[string]any) {
	t.Helper()
	bucketSum := rowFloat64(t, row, "within_one_month") +
		rowFloat64(t, row, "one_to_two_months") +
		rowFloat64(t, row, "two_to_three_months") +
		rowFloat64(t, row, "three_to_six_months") +
		rowFloat64(t, row, "six_to_nine_months") +
		rowFloat64(t, row, "nine_to_twelve_months") +
		rowFloat64(t, row, "one_to_two_years") +
		rowFloat64(t, row, "two_to_three_years") +
		rowFloat64(t, row, "over_three_years")
	if bucketSum != rowFloat64(t, row, "total") {
		t.Fatalf("expected aging buckets %v to equal total %v", bucketSum, rowFloat64(t, row, "total"))
	}
}

func rowString(t *testing.T, row map[string]any, key string) string {
	t.Helper()
	value, ok := row[key]
	if !ok {
		t.Fatalf("missing row key %q", key)
	}
	stringValue, ok := value.(string)
	if !ok {
		t.Fatalf("expected string for %q, got %T", key, value)
	}
	return stringValue
}

func rowFloat64(t *testing.T, row map[string]any, key string) float64 {
	t.Helper()
	value, ok := row[key]
	if !ok {
		t.Fatalf("missing row key %q", key)
	}
	switch v := value.(type) {
	case float64:
		return v
	case float32:
		return float64(v)
	case int:
		return float64(v)
	case int64:
		return float64(v)
	default:
		t.Fatalf("expected numeric value for %q, got %T", key, value)
		return 0
	}
}

func rowOptionalFloat64(t *testing.T, row map[string]any, key string) *float64 {
	t.Helper()
	value, ok := row[key]
	if !ok {
		t.Fatalf("missing row key %q", key)
	}
	if value == nil {
		return nil
	}
	switch v := value.(type) {
	case *float64:
		return v
	case float64:
		return &v
	default:
		t.Fatalf("expected optional float64 for %q, got %T", key, value)
		return nil
	}
}

func rowInt(t *testing.T, row map[string]any, key string) int {
	t.Helper()
	value, ok := row[key]
	if !ok {
		t.Fatalf("missing row key %q", key)
	}
	switch v := value.(type) {
	case int:
		return v
	case int64:
		return int(v)
	default:
		t.Fatalf("expected int value for %q, got %T", key, value)
		return 0
	}
}

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
