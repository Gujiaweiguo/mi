//go:build integration

package reporting_test

import (
	"bytes"
	"context"
	"database/sql"
	"fmt"
	"math"
	"os"
	"reflect"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/reporting"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/xuri/excelize/v2"
)

func TestReportingServiceQueryAndExportCoreReports(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	service := reporting.NewService(reporting.NewRepository(db))

	activeLease := activateReportingLease(t, ctx, leaseService, workflowService)
	if activeLease.Status != lease.StatusActive {
		t.Fatalf("expected active lease, got %#v", activeLease)
	}
	seedAROpenItems(t, ctx, db, activeLease.ID)
	seedDailyShopSales(t, ctx, db)
	seedSupplementalReportingUnits(t, ctx, db)
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
		assertWorkbookHeadersMatchColumns(t, result.Columns, rows[0])
	}

	t.Run("R19 resolves a default floor when filters are empty", func(t *testing.T) {
		queryInput := reporting.QueryInput{ReportID: reporting.ReportR19, RequestedByID: 101}
		result, err := service.QueryReport(ctx, queryInput)
		if err != nil {
			t.Fatalf("query R19 without explicit floor: %v", err)
		}
		if result.Visual == nil {
			t.Fatal("expected R19 visual payload")
		}
		if result.Visual.Floor.ID == 0 {
			t.Fatalf("expected resolved floor, got %#v", result.Visual.Floor)
		}
		if len(result.Visual.Units) == 0 {
			t.Fatal("expected resolved floor to include visual units")
		}
	})

	t.Run("localized report headers follow accepted terminology", func(t *testing.T) {
		cases := []struct {
			name     string
			reportID reporting.ReportID
			expected map[string]string
		}{
			{
				name:     "r01 area summary",
				reportID: reporting.ReportR01,
				expected: map[string]string{"store_name": "门店", "department_name": "部门", "rent_status": "出租状态", "use_area_total": "使用面积汇总"},
			},
			{
				name:     "r02 contract ledger",
				reportID: reporting.ReportR02,
				expected: map[string]string{"lease_no": "合同编号", "trade_name": "业态", "management_type_name": "经营方式", "unit_code": "商铺编码", "shop_type_name": "店铺类型", "department_name": "分公司"},
			},
			{
				name:     "r04 daily sales",
				reportID: reporting.ReportR04,
				expected: map[string]string{"unit_code": "商铺编码", "shop_type": "业态", "day_01": "1日", "day_31": "31日"},
			},
			{
				name:     "r08 customer aging",
				reportID: reporting.ReportR08,
				expected: map[string]string{"unit_collection": "商铺集合", "department_name": "分公司", "deposit_amount": "押金", "within_one_month": "1月内", "over_three_years": "3年以上", "total": "合计"},
			},
			{
				name:     "r10 traffic summary",
				reportID: reporting.ReportR10,
				expected: map[string]string{"year": "年度", "annual_total": "月度总客流", "month_01": "1月", "month_12": "12月"},
			},
			{
				name:     "r16 department aging",
				reportID: reporting.ReportR16,
				expected: map[string]string{"department_name": "分公司", "deposit_amount": "押金", "total": "合计"},
			},
			{
				name:     "r18 composite analysis",
				reportID: reporting.ReportR18,
				expected: map[string]string{"customer_name": "客户", "unit_name": "商铺", "period": "月份", "period_arrears": "本期欠费", "cumulative_arrears": "累计欠费", "efficiency": "坪效"},
			},
			{
				name:     "r19 visual export",
				reportID: reporting.ReportID("r19"),
				expected: map[string]string{"unit_code": "商铺编码", "unit_name": "商铺描述", "floor_area": "建筑面积", "shop_type_name": "店铺类型", "color_hex": "颜色"},
			},
		}

		for _, tc := range cases {
			t.Run(tc.name, func(t *testing.T) {
				result := mustQueryReport(t, ctx, service, withReportID(baseInput, tc.reportID))
				assertColumnLabels(t, result.Columns, tc.expected)

				headers := artifactHeaderRow(t, service, ctx, withReportID(baseInput, tc.reportID))
				assertWorkbookHeadersMatchColumns(t, result.Columns, headers)
				assertHeaderTextsPresent(t, headers, tc.expected)
			})
		}
	})

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

	t.Run("R01 and R12 preserve occupancy structure semantics", func(t *testing.T) {
		r01 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR01))
		leasedRow := findRowByString(t, r01.Rows, "rent_status", "leased")
		vacantRow := findRowByString(t, r01.Rows, "rent_status", "vacant")
		if got := rowFloat64(t, leasedRow, "use_area_total"); got != 118 {
			t.Fatalf("expected leased area total 118, got %v", got)
		}
		if got := rowFloat64(t, vacantRow, "use_area_total"); got != 64 {
			t.Fatalf("expected vacant area total 64, got %v", got)
		}

		r11 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR11))
		if len(r11.Rows) != 1 {
			t.Fatalf("expected one leased area summary row, got %d", len(r11.Rows))
		}
		if got := rowFloat64(t, leasedRow, "use_area_total"); got != rowFloat64(t, r11.Rows[0], "leased_area") {
			t.Fatalf("expected R01 leased area %v to reconcile with R11 leased area %v", got, rowFloat64(t, r11.Rows[0], "leased_area"))
		}

		r12 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR12))
		fashionLeased := findRowByPair(t, r12.Rows, "shop_type_name", "Fashion", "occupancy_status", "leased")
		fashionVacant := findRowByPair(t, r12.Rows, "shop_type_name", "Fashion", "occupancy_status", "vacant")
		if got := rowFloat64(t, fashionLeased, "area_total"); got != 118 {
			t.Fatalf("expected fashion leased area 118, got %v", got)
		}
		if got := rowFloat64(t, fashionVacant, "area_total"); got != 64 {
			t.Fatalf("expected fashion vacant area 64, got %v", got)
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

	t.Run("R13 and R15 preserve period-aligned sales and rent semantics", func(t *testing.T) {
		r03 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR03))
		if len(r03.Rows) != 1 {
			t.Fatalf("expected one sales analysis row for semantic baseline, got %d", len(r03.Rows))
		}
		r03Row := r03.Rows[0]
		baselineCurrent := rowFloat64(t, r03Row, "current_sales")
		baselineSamePeriod := rowFloat64(t, r03Row, "same_period_sales")

		previousPeriodStart, previousPeriodEnd, previousPeriodLabel, err := reporting.ParsePeriod("2026-03")
		if err != nil {
			t.Fatalf("parse previous period: %v", err)
		}
		previousInput := baseInput
		previousInput.PeriodStart = previousPeriodStart
		previousInput.PeriodEnd = previousPeriodEnd
		previousInput.PeriodLabel = previousPeriodLabel
		previousR03 := mustQueryReport(t, ctx, service, withReportID(previousInput, reporting.ReportR03))
		if len(previousR03.Rows) != 1 {
			t.Fatalf("expected one previous-month sales baseline row, got %d", len(previousR03.Rows))
		}
		baselinePreviousMonth := rowFloat64(t, previousR03.Rows[0], "current_sales")

		r13 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR13))
		if len(r13.Rows) != 1 {
			t.Fatalf("expected one year-over-year row, got %d", len(r13.Rows))
		}
		r13Row := r13.Rows[0]
		if got := rowString(t, r13Row, "period"); got != periodLabel {
			t.Fatalf("expected R13 period %q, got %q", periodLabel, got)
		}
		if got := rowFloat64(t, r13Row, "current_sales"); got != baselineCurrent {
			t.Fatalf("expected R13 current sales %v to align with R03 current sales, got %v", baselineCurrent, got)
		}
		ytdSales := rowFloat64(t, r13Row, "ytd_sales")
		if ytdSales < baselineCurrent {
			t.Fatalf("expected R13 YTD sales %v to be >= current sales baseline %v", ytdSales, baselineCurrent)
		}
		if ytdSales < baselinePreviousMonth {
			t.Fatalf("expected R13 YTD sales %v to be >= previous month baseline %v", ytdSales, baselinePreviousMonth)
		}
		if got := rowFloat64(t, r13Row, "prev_month_sales"); got != baselinePreviousMonth {
			t.Fatalf("expected R13 previous month sales %v to align with March R03 baseline, got %v", baselinePreviousMonth, got)
		}
		if got := rowFloat64(t, r13Row, "last_year_ytd_sales"); got < baselineSamePeriod {
			t.Fatalf("expected R13 last-year YTD sales %v to be >= same-period baseline %v", got, baselineSamePeriod)
		}

		r15 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR15))
		if len(r15.Rows) != 1 {
			t.Fatalf("expected one sales-vs-rent row, got %d", len(r15.Rows))
		}
		r15Row := r15.Rows[0]
		if got := rowString(t, r15Row, "period"); got != periodLabel {
			t.Fatalf("expected R15 period %q, got %q", periodLabel, got)
		}
		if got := rowFloat64(t, r15Row, "sales_amount"); got != baselineCurrent {
			t.Fatalf("expected R15 sales amount %v to align with R03 current sales, got %v", baselineCurrent, got)
		}
		if got := rowFloat64(t, r15Row, "rent_income"); got != 12000 {
			t.Fatalf("expected R15 rent income 12000 from active rent term, got %v", got)
		}
	})

	t.Run("R04, R07, and R10 monthly pivots reconcile to report totals", func(t *testing.T) {
		r04Input := withReportID(baseInput, reporting.ReportR04)
		r04Input.UnitID = nil
		r04 := mustQueryReport(t, ctx, service, r04Input)
		unit101 := findRowByString(t, r04.Rows, "unit_code", "U-101")
		if got := rowFloat64Sum(t, unit101, dayKeysForPeriod(baseInput.PeriodStart)...); got != rowFloat64(t, unit101, "total_sales") {
			t.Fatalf("expected R04 daily sales sum %v to equal total sales %v", got, rowFloat64(t, unit101, "total_sales"))
		}

		r07Input := withReportID(baseInput, reporting.ReportR07)
		r07Input.BrandID = nil
		r07 := mustQueryReport(t, ctx, service, r07Input)
		brandRow := findRowByString(t, r07.Rows, "brand_name", "ACME Fashion")
		if got := rowFloat64Sum(t, brandRow, monthKeys(12)...); got != rowFloat64(t, brandRow, "annual_total") {
			t.Fatalf("expected R07 monthly sum %v to equal annual total %v", got, rowFloat64(t, brandRow, "annual_total"))
		}

		r10 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR10))
		if len(r10.Rows) != 1 {
			t.Fatalf("expected one traffic summary row, got %d", len(r10.Rows))
		}
		trafficRow := r10.Rows[0]
		if got := rowIntSum(t, trafficRow, monthKeys(12)...); got != rowInt(t, trafficRow, "annual_total") {
			t.Fatalf("expected R10 monthly sum %v to equal annual total %v", got, rowInt(t, trafficRow, "annual_total"))
		}
	})

	t.Run("R05 tolerates missing lease and prospect details without query failure", func(t *testing.T) {
		r05Input := withReportID(baseInput, reporting.ReportR05)
		r05Input.UnitID = int64Ptr(102)
		r05 := mustQueryReport(t, ctx, service, r05Input)
		if len(r05.Rows) != 1 {
			t.Fatalf("expected one unit budget comparison row, got %d", len(r05.Rows))
		}
		row := r05.Rows[0]
		if got := rowString(t, row, "unit_code"); got != "U-102" {
			t.Fatalf("expected vacant unit row U-102, got %q", got)
		}
		for _, key := range []string{"budget_unit_price", "current_lease_price", "average_ticket", "prospect_rent_price", "prospect_term_months"} {
			if value := row[key]; !isNilValue(value) {
				t.Fatalf("expected %s to stay nil for missing prospect data, got %#v", key, value)
			}
		}
		for _, key := range []string{"potential_customer", "prospect_brand", "prospect_trade", "rent_increment"} {
			if !isNilOrEmptyString(row[key]) {
				t.Fatalf("expected %s to stay nil or empty for missing prospect data, got %#v", key, row[key])
			}
		}
	})

	t.Run("R14 leaves efficiency empty when area total is zero", func(t *testing.T) {
		zeroAreaShopTypeID := int64(102)
		r14Input := withReportID(baseInput, reporting.ReportR14)
		r14Input.ShopTypeID = &zeroAreaShopTypeID
		r14 := mustQueryReport(t, ctx, service, r14Input)
		if len(r14.Rows) != 1 {
			t.Fatalf("expected one zero-area efficiency row, got %d", len(r14.Rows))
		}
		row := r14.Rows[0]
		if got := rowFloat64(t, row, "sales_amount"); got != 1200 {
			t.Fatalf("expected zero-area seeded sales 1200, got %v", got)
		}
		if got := rowFloat64(t, row, "area_total"); got != 0 {
			t.Fatalf("expected zero area total, got %v", got)
		}
		if got := rowOptionalFloat64(t, row, "efficiency"); got != nil {
			t.Fatalf("expected nil efficiency for zero area, got %v", *got)
		}
	})

	t.Run("R06 and R18 use approved invoice amounts for receivable semantics", func(t *testing.T) {
		r06 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR06))
		if len(r06.Rows) != 1 {
			t.Fatalf("expected one store rent budget row, got %d", len(r06.Rows))
		}
		r06Row := r06.Rows[0]
		if got := rowFloat64(t, r06Row, "period_receivable"); got != 9000 {
			t.Fatalf("expected R06 period receivable 9000 from approved invoice amount, got %v", got)
		}
		if got := rowFloat64(t, r06Row, "period_received"); got != 9000 {
			t.Fatalf("expected R06 period received 9000 from payment ledger, got %v", got)
		}
		if got := rowFloat64(t, r06Row, "ytd_cumulative"); got != 17000 {
			t.Fatalf("expected R06 ytd cumulative 17000 from invoice payments, got %v", got)
		}

		r18 := mustQueryReport(t, ctx, service, withReportID(baseInput, reporting.ReportR18))
		if len(r18.Rows) != 1 {
			t.Fatalf("expected one composite receivable row, got %d", len(r18.Rows))
		}
		r18Row := r18.Rows[0]
		if got := rowFloat64(t, r18Row, "period_receivable"); got != 9000 {
			t.Fatalf("expected R18 period receivable 9000 from approved invoice amount, got %v", got)
		}
		if got := rowFloat64(t, r18Row, "period_received"); got != 9000 {
			t.Fatalf("expected R18 period received 9000 from payment ledger, got %v", got)
		}
		if got := rowFloat64(t, r18Row, "period_arrears"); got != 0 {
			t.Fatalf("expected R18 period arrears 0 after full payment, got %v", got)
		}
		if got := rowFloat64(t, r18Row, "cumulative_receivable"); got != 17000 {
			t.Fatalf("expected R18 cumulative receivable 17000 from approved invoice amounts, got %v", got)
		}
		if got := rowFloat64(t, r18Row, "cumulative_arrears"); got != 3500 {
			t.Fatalf("expected R18 cumulative arrears 3500 from open items, got %v", got)
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

		r09Input := withReportID(baseInput, reporting.ReportR09)
		r09Input.ChargeType = nil
		r09 := mustQueryReport(t, ctx, service, r09Input)
		if len(r09.Rows) == 0 {
			t.Fatal("expected customer aging-by-charge rows")
		}
		customerChargeRows := make(map[string]map[string]any, len(r09.Rows))
		var customerChargeDeposit, customerChargeTotal float64
		for _, row := range r09.Rows {
			assertAgingBucketTotal(t, row)
			chargeType := rowString(t, row, "charge_type")
			customerChargeRows[chargeType] = row
			customerChargeDeposit += rowFloat64(t, row, "deposit_amount")
			customerChargeTotal += rowFloat64(t, row, "total")
		}
		if got := rowFloat64(t, customerChargeRows["rent"], "total"); got != 1500 {
			t.Fatalf("expected customer rent aging total 1500, got %v", got)
		}
		if got := rowFloat64(t, customerChargeRows["service"], "total"); got != 2000 {
			t.Fatalf("expected customer service aging total 2000, got %v", got)
		}
		if got := rowFloat64(t, customerChargeRows["deposit"], "deposit_amount"); got != 3000 {
			t.Fatalf("expected customer deposit row to carry 3000 deposit, got %v", got)
		}
		if customerChargeDeposit != rowFloat64(t, customerRow, "deposit_amount") || customerChargeTotal != rowFloat64(t, customerRow, "total") {
			t.Fatalf("expected customer aging-by-charge to reconcile with R08 customer totals, got charge deposit/total=%v/%v customer=%v/%v", customerChargeDeposit, customerChargeTotal, rowFloat64(t, customerRow, "deposit_amount"), rowFloat64(t, customerRow, "total"))
		}
	})
}

func TestReportingServiceR06IncludesOvertimeBackedInvoiceAmounts(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	service := reporting.NewService(reporting.NewRepository(db))

	activeLease := activateReportingLease(t, ctx, leaseService, workflowService)
	seedBudgetFacts(t, ctx, db, activeLease.ID)
	seedOvertimeBudgetFact(t, ctx, db, activeLease.ID)

	periodStart, periodEnd, periodLabel, err := reporting.ParsePeriod("2026-04")
	if err != nil {
		t.Fatalf("parse period: %v", err)
	}
	storeID := int64(101)
	result, err := service.QueryReport(ctx, reporting.QueryInput{ReportID: reporting.ReportR06, PeriodStart: periodStart, PeriodEnd: periodEnd, PeriodLabel: periodLabel, StoreID: &storeID, RequestedByID: 101})
	if err != nil {
		t.Fatalf("query R06: %v", err)
	}
	if len(result.Rows) != 1 {
		t.Fatalf("expected one R06 row, got %#v", result.Rows)
	}
	if got := rowFloat64(t, result.Rows[0], "period_receivable"); got != 9600 {
		t.Fatalf("expected overtime-backed R06 receivable 9600, got %v", got)
	}
}

func seedSupplementalReportingUnits(t *testing.T, ctx context.Context, db *sql.DB) {
	t.Helper()
	if _, err := db.ExecContext(ctx, `INSERT INTO shop_types (id, code, name, color_hex, status) VALUES (102, 'zero-area', 'Zero Area', '#999999', 'active') ON DUPLICATE KEY UPDATE code = VALUES(code), name = VALUES(name), color_hex = VALUES(color_hex), status = VALUES(status)`); err != nil {
		t.Fatalf("seed supplemental shop type: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO units (id, building_id, floor_id, location_id, area_id, unit_type_id, shop_type_id, code, name, floor_area, use_area, rent_area, is_rentable, status) VALUES
		(102, 101, 101, 101, 101, 101, 101, 'U-102', 'Unit 102', 64.00, 64.00, 64.00, TRUE, 'active'),
		(103, 101, 101, 101, 101, 101, 102, 'U-103', 'Unit 103', 0.00, 0.00, 0.00, TRUE, 'active')
	ON DUPLICATE KEY UPDATE
		shop_type_id = VALUES(shop_type_id),
		name = VALUES(name),
		floor_area = VALUES(floor_area),
		use_area = VALUES(use_area),
		rent_area = VALUES(rent_area),
		is_rentable = VALUES(is_rentable),
		status = VALUES(status)`); err != nil {
		t.Fatalf("seed supplemental units: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO daily_shop_sales (store_id, unit_id, sale_date, sales_amount) VALUES (101, 103, '2026-04-12', 1200.00) ON DUPLICATE KEY UPDATE sales_amount = VALUES(sales_amount)`); err != nil {
		t.Fatalf("seed zero-area shop sales: %v", err)
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
	documentLineResult, err := db.ExecContext(ctx, `INSERT INTO billing_document_lines (billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount) VALUES (?, ?, 'rent', '2026-04-01', '2026-04-30', 30, 12000.00, 9000.00)`, documentID, chargeLineID)
	if err != nil {
		t.Fatalf("seed billing document line: %v", err)
	}
	documentLineID, err := documentLineResult.LastInsertId()
	if err != nil {
		t.Fatalf("billing document line id: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO ar_open_items (lease_contract_id, billing_document_id, billing_document_line_id, customer_id, department_id, trade_id, charge_type, due_date, outstanding_amount, settled_at, is_deposit) VALUES (?, ?, ?, 101, 101, 102, 'rent', '2026-04-30', 0.00, NOW(), FALSE)`, leaseID, documentID, documentLineID); err != nil {
		t.Fatalf("seed april receivable open item: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO ar_payment_entries (billing_document_id, lease_contract_id, payment_date, amount, note, recorded_by, idempotency_key) VALUES (?, ?, '2026-04-25', 9000.00, 'seed payment april', 101, 'seed-payment-april')`, documentID, leaseID); err != nil {
		t.Fatalf("seed april payment entry: %v", err)
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
	priorDocumentLineResult, err := db.ExecContext(ctx, `INSERT INTO billing_document_lines (billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount) VALUES (?, ?, 'rent', '2026-03-01', '2026-03-31', 31, 12000.00, 8000.00)`, priorDocumentID, priorChargeLineID)
	if err != nil {
		t.Fatalf("seed prior billing document line: %v", err)
	}
	priorDocumentLineID, err := priorDocumentLineResult.LastInsertId()
	if err != nil {
		t.Fatalf("prior billing document line id: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO ar_open_items (lease_contract_id, billing_document_id, billing_document_line_id, customer_id, department_id, trade_id, charge_type, due_date, outstanding_amount, settled_at, is_deposit) VALUES (?, ?, ?, 101, 101, 102, 'rent', '2026-03-31', 0.00, NOW(), FALSE)`, leaseID, priorDocumentID, priorDocumentLineID); err != nil {
		t.Fatalf("seed march receivable open item: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO ar_payment_entries (billing_document_id, lease_contract_id, payment_date, amount, note, recorded_by, idempotency_key) VALUES (?, ?, '2026-03-20', 8000.00, 'seed payment march', 101, 'seed-payment-march')`, priorDocumentID, leaseID); err != nil {
		t.Fatalf("seed march payment entry: %v", err)
	}
}

func seedOvertimeBudgetFact(t *testing.T, ctx context.Context, db *sql.DB, leaseID int64) {
	t.Helper()
	result, err := db.ExecContext(ctx, `INSERT INTO billing_runs (period_start, period_end, status, triggered_by, generated_count, skipped_count) VALUES ('2026-04-01', '2026-04-30', 'completed', 101, 1, 0)`)
	if err != nil {
		t.Fatalf("seed overtime billing run: %v", err)
	}
	billingRunID, err := result.LastInsertId()
	if err != nil {
		t.Fatalf("overtime billing run id: %v", err)
	}
	chargeResult, err := db.ExecContext(ctx, `INSERT INTO billing_charge_lines (billing_run_id, lease_contract_id, lease_term_id, charge_type, charge_source, period_start, period_end, quantity_days, unit_amount, amount, currency_type_id, source_effective_version) VALUES (?, ?, NULL, 'overtime_rent', 'overtime', '2026-04-01', '2026-04-30', 30, 20.00, 600.00, 101, 1)`, billingRunID, leaseID)
	if err != nil {
		t.Fatalf("seed overtime billing charge line: %v", err)
	}
	chargeLineID, err := chargeResult.LastInsertId()
	if err != nil {
		t.Fatalf("overtime billing charge line id: %v", err)
	}
	docResult, err := db.ExecContext(ctx, `INSERT INTO billing_documents (document_type, document_no, billing_run_id, lease_contract_id, tenant_name, period_start, period_end, total_amount, currency_type_id, status, approved_at, created_by, updated_by) VALUES ('invoice', 'INV-2026-04-OT', ?, ?, 'Report Tenant', '2026-04-01', '2026-04-30', 600.00, 101, 'approved', NOW(), 101, 101)`, billingRunID, leaseID)
	if err != nil {
		t.Fatalf("seed overtime billing document: %v", err)
	}
	documentID, err := docResult.LastInsertId()
	if err != nil {
		t.Fatalf("overtime billing document id: %v", err)
	}
	if _, err := db.ExecContext(ctx, `INSERT INTO billing_document_lines (billing_document_id, billing_charge_line_id, charge_type, charge_source, period_start, period_end, quantity_days, unit_amount, amount) VALUES (?, ?, 'overtime_rent', 'overtime', '2026-04-01', '2026-04-30', 30, 20.00, 600.00)`, documentID, chargeLineID); err != nil {
		t.Fatalf("seed overtime billing document line: %v", err)
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

func findRowByString(t *testing.T, rows []map[string]any, key, want string) map[string]any {
	t.Helper()
	for _, row := range rows {
		if rowString(t, row, key) == want {
			return row
		}
	}
	t.Fatalf("expected row where %s=%q, got %#v", key, want, rows)
	return nil
}

func findRowByPair(t *testing.T, rows []map[string]any, firstKey, firstWant, secondKey, secondWant string) map[string]any {
	t.Helper()
	for _, row := range rows {
		if rowString(t, row, firstKey) == firstWant && rowString(t, row, secondKey) == secondWant {
			return row
		}
	}
	t.Fatalf("expected row where %s=%q and %s=%q, got %#v", firstKey, firstWant, secondKey, secondWant, rows)
	return nil
}

func rowFloat64Sum(t *testing.T, row map[string]any, keys ...string) float64 {
	t.Helper()
	var sum float64
	for _, key := range keys {
		sum += rowFloat64(t, row, key)
	}
	return sum
}

func rowIntSum(t *testing.T, row map[string]any, keys ...string) int {
	t.Helper()
	var sum int
	for _, key := range keys {
		sum += rowInt(t, row, key)
	}
	return sum
}

func dayKeys(days int) []string {
	keys := make([]string, 0, days)
	for day := 1; day <= days; day++ {
		keys = append(keys, fmt.Sprintf("day_%02d", day))
	}
	return keys
}

func dayKeysForPeriod(periodStart time.Time) []string {
	nextMonth := time.Date(periodStart.Year(), periodStart.Month()+1, 1, 0, 0, 0, 0, time.UTC)
	days := nextMonth.Add(-time.Nanosecond).Day()
	return dayKeys(days)
}

func monthKeys(months int) []string {
	keys := make([]string, 0, months)
	for month := 1; month <= months; month++ {
		keys = append(keys, fmt.Sprintf("month_%02d", month))
	}
	return keys
}

func artifactHeaderRow(t *testing.T, service *reporting.Service, ctx context.Context, input reporting.QueryInput) []string {
	t.Helper()
	artifact, err := service.ExportReport(ctx, input)
	if err != nil {
		t.Fatalf("export %s: %v", input.ReportID, err)
	}
	file, err := excelize.OpenReader(bytes.NewReader(artifact.Bytes))
	if err != nil {
		t.Fatalf("open workbook %s: %v", input.ReportID, err)
	}
	rows, err := file.GetRows(file.GetSheetName(0))
	_ = file.Close()
	if err != nil {
		t.Fatalf("read workbook rows %s: %v", input.ReportID, err)
	}
	if len(rows) < 2 {
		t.Fatalf("expected workbook data rows for %s, got %v", input.ReportID, rows)
	}
	return rows[0]
}

func assertWorkbookHeadersMatchColumns(t *testing.T, columns []reporting.Column, headers []string) {
	t.Helper()
	if len(headers) < len(columns) {
		t.Fatalf("expected at least %d headers, got %d (%v)", len(columns), len(headers), headers)
	}
	for index, column := range columns {
		if headers[index] != column.Label {
			t.Fatalf("expected header %d to equal %q for key %s, got %q", index, column.Label, column.Key, headers[index])
		}
	}
}

func assertColumnLabels(t *testing.T, columns []reporting.Column, expected map[string]string) {
	t.Helper()
	actual := make(map[string]string, len(columns))
	for _, column := range columns {
		actual[column.Key] = column.Label
	}
	for key, want := range expected {
		if got, ok := actual[key]; !ok {
			t.Fatalf("expected column key %q in %#v", key, columns)
		} else if got != want {
			t.Fatalf("expected label for %q to be %q, got %q", key, want, got)
		}
	}
}

func assertHeaderTextsPresent(t *testing.T, headers []string, expected map[string]string) {
	t.Helper()
	available := make(map[string]struct{}, len(headers))
	for _, header := range headers {
		available[header] = struct{}{}
	}
	for _, want := range expected {
		if _, ok := available[want]; !ok {
			t.Fatalf("expected header %q in %v", want, headers)
		}
	}
}

func isNilValue(value any) bool {
	if value == nil {
		return true
	}
	rv := reflect.ValueOf(value)
	switch rv.Kind() {
	case reflect.Chan, reflect.Func, reflect.Interface, reflect.Map, reflect.Pointer, reflect.Slice:
		return rv.IsNil()
	default:
		return false
	}
}

func isNilOrEmptyString(value any) bool {
	if isNilValue(value) {
		return true
	}
	stringValue, ok := value.(string)
	return ok && stringValue == ""
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
