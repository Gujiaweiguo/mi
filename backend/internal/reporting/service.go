package reporting

import (
	"context"
	"errors"
	"fmt"
	"strings"
	"time"

	"github.com/xuri/excelize/v2"
)

var (
	ErrUnsupportedReport = errors.New("unsupported generalize report")
	ErrInvalidPeriod     = errors.New("invalid report period")
)

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

func ParsePeriod(value string) (time.Time, time.Time, string, error) {
	trimmed := strings.TrimSpace(value)
	if trimmed == "" {
		return time.Time{}, time.Time{}, "", ErrInvalidPeriod
	}
	periodStart, err := time.Parse(PeriodLayout, trimmed)
	if err != nil {
		return time.Time{}, time.Time{}, "", ErrInvalidPeriod
	}
	periodEnd := periodStart.AddDate(0, 1, 0).Add(-time.Nanosecond)
	return periodStart, periodEnd, periodStart.Format(PeriodLayout), nil
}

func (s *Service) QueryReport(ctx context.Context, input QueryInput) (*Result, error) {
	if input.ReportID == ReportID("r19") {
		visual, err := s.repository.QueryR19(ctx, input)
		if err != nil {
			return nil, err
		}
		columns, rows := flattenR19Visual(visual)
		result := &Result{
			ReportID:    input.ReportID,
			Columns:     columns,
			Rows:        rows,
			Visual:      visual,
			GeneratedAt: time.Now().UTC(),
		}
		if err := s.repository.InsertReportAudit(ctx, ReportAuditActionQuery, input, len(result.Rows), 0); err != nil {
			return nil, err
		}
		return result, nil
	}
	columns, rows, err := s.runReport(ctx, input)
	if err != nil {
		return nil, err
	}
	result := &Result{
		ReportID:    input.ReportID,
		Columns:     columns,
		Rows:        rows,
		GeneratedAt: time.Now().UTC(),
	}
	if err := s.repository.InsertReportAudit(ctx, ReportAuditActionQuery, input, len(result.Rows), 0); err != nil {
		return nil, err
	}
	return result, nil
}

func (s *Service) ExportReport(ctx context.Context, input QueryInput) (*ExportArtifact, error) {
	if input.ReportID == ReportID("r19") {
		visual, err := s.repository.QueryR19(ctx, input)
		if err != nil {
			return nil, err
		}
		columns, rows := flattenR19Visual(visual)
		artifact, err := exportWorkbook(input, columns, rows)
		if err != nil {
			return nil, err
		}
		if err := s.repository.InsertReportAudit(ctx, ReportAuditActionExport, input, len(rows), len(artifact.Bytes)); err != nil {
			return nil, err
		}
		return artifact, nil
	}
	columns, rows, err := s.runReport(ctx, input)
	if err != nil {
		return nil, err
	}
	artifact, err := exportWorkbook(input, columns, rows)
	if err != nil {
		return nil, err
	}
	if err := s.repository.InsertReportAudit(ctx, ReportAuditActionExport, input, len(rows), len(artifact.Bytes)); err != nil {
		return nil, err
	}
	return artifact, nil
}

func exportWorkbook(input QueryInput, columns []Column, rows []map[string]any) (*ExportArtifact, error) {
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()

	sheet := f.GetSheetName(0)
	for index, column := range columns {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(sheet, cell, column.Label)
	}
	for rowIndex, row := range rows {
		for columnIndex, column := range columns {
			cell, _ := excelize.CoordinatesToCellName(columnIndex+1, rowIndex+2)
			_ = f.SetCellValue(sheet, cell, row[column.Key])
		}
	}

	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write report workbook: %w", err)
	}

	return &ExportArtifact{
		FileName:    fmt.Sprintf("generalize-%s-%s.xlsx", input.ReportID, input.PeriodLabel),
		ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
		Bytes:       buffer.Bytes(),
	}, nil
}

func (s *Service) runReport(ctx context.Context, input QueryInput) ([]Column, []map[string]any, error) {
	switch input.ReportID {
	case ReportR02:
		items, err := s.repository.QueryR02(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR02Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"lease_no": item.LeaseNo, "customer_code": item.CustomerCode, "customer_name": item.CustomerName, "trade_name": item.TradeName, "management_type_name": item.ManagementTypeName, "unit_code": item.UnitCode, "unit_name": item.UnitName, "rent_area": item.RentArea, "brand_name": item.BrandName, "shop_type_name": item.ShopTypeName, "department_name": item.DepartmentName, "store_name": item.StoreName})
		}
		return columns, rows, nil
	case ReportR01:
		items, err := s.repository.QueryR01(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR01Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "department_name": item.DepartmentName, "rent_status": item.RentStatus, "use_area_total": item.UseAreaTotal})
		}
		return columns, rows, nil
	case ReportR03:
		items, err := s.repository.QueryR03(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR03Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"shop_type_name": item.ShopTypeName, "rent_area": item.RentArea, "current_sales": item.CurrentSales, "same_period_sales": item.SamePeriodSales, "comparable_sales": item.ComparableSales, "monthly_rent": item.MonthlyRent})
		}
		return columns, rows, nil
	case ReportR04:
		items, err := s.repository.QueryR04(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR04Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			row := map[string]any{"unit_code": item.UnitCode, "unit_name": item.UnitName, "rent_area": item.RentArea, "shop_type": item.ShopType, "total_sales": item.TotalSales}
			for day := 1; day <= 31; day++ {
				row[fmt.Sprintf("day_%02d", day)] = item.DayValue(day)
			}
			rows = append(rows, row)
		}
		return columns, rows, nil
	case ReportR05:
		items, err := s.repository.QueryR05(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR05Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"unit_code": item.UnitCode, "floor_area": item.FloorArea, "budget_unit_price": item.BudgetUnitPrice, "current_lease_price": item.CurrentLeasePrice, "potential_customer": item.PotentialCustomer, "prospect_brand": item.ProspectBrand, "prospect_trade": item.ProspectTrade, "average_ticket": item.AverageTicket, "prospect_rent_price": item.ProspectRentPrice, "rent_increment": item.RentIncrement, "prospect_term_months": item.ProspectTermMonths})
		}
		return columns, rows, nil
	case ReportR06:
		items, err := s.repository.QueryR06(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR06Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "period": item.Period, "period_receivable": item.PeriodReceivable, "period_received": item.PeriodReceived, "monthly_budget": item.MonthlyBudget, "annual_budget": item.AnnualBudget, "ytd_cumulative": item.YTDCumulative})
		}
		return columns, rows, nil
	case ReportR07:
		items, err := s.repository.QueryR07(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR07Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			row := map[string]any{"store_name": item.StoreName, "brand_name": item.BrandName, "annual_total": item.AnnualTotal}
			row["month_01"] = item.Month01
			row["month_02"] = item.Month02
			row["month_03"] = item.Month03
			row["month_04"] = item.Month04
			row["month_05"] = item.Month05
			row["month_06"] = item.Month06
			row["month_07"] = item.Month07
			row["month_08"] = item.Month08
			row["month_09"] = item.Month09
			row["month_10"] = item.Month10
			row["month_11"] = item.Month11
			row["month_12"] = item.Month12
			rows = append(rows, row)
		}
		return columns, rows, nil
	case ReportR08:
		items, err := s.repository.QueryR08(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := agingCustomerColumns(false)
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, agingCustomerRow(item.UnitCollection, item.CustomerName, item.TradeName, item.DepartmentName, item.LeaseNo, item.DepositAmount, "", item.AgingBuckets))
		}
		return columns, rows, nil
	case ReportR09:
		items, err := s.repository.QueryR09(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := agingCustomerColumns(true)
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, agingCustomerRow(item.UnitCollection, item.CustomerName, item.TradeName, item.DepartmentName, item.LeaseNo, item.DepositAmount, item.ChargeType, item.AgingBuckets))
		}
		return columns, rows, nil
	case ReportR10:
		items, err := s.repository.QueryR10(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR10Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			row := map[string]any{"store_name": item.StoreName, "year": item.Year, "annual_total": item.MonthlyTotal}
			for month := 1; month <= 12; month++ {
				row[fmt.Sprintf("month_%02d", month)] = item.MonthValue(month)
			}
			rows = append(rows, row)
		}
		return columns, rows, nil
	case ReportR11:
		items, err := s.repository.QueryR11(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR11Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "period": item.Period, "leased_area": item.LeasedArea, "total_area": item.TotalArea})
		}
		return columns, rows, nil
	case ReportR13:
		items, err := s.repository.QueryR13(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR13Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "shop_type_name": item.ShopTypeName, "period": item.Period, "current_sales": item.CurrentSales, "ytd_sales": item.YTDSales, "prev_month_sales": item.PrevMonthSales, "last_year_ytd_sales": item.LastYearYTDSales})
		}
		return columns, rows, nil
	case ReportR14:
		items, err := s.repository.QueryR14(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR14Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "shop_type_name": item.ShopTypeName, "period": item.Period, "sales_amount": item.SalesAmount, "area_total": item.AreaTotal, "days_in_month": item.DaysInMonth, "efficiency": item.Efficiency})
		}
		return columns, rows, nil
	case ReportR15:
		items, err := s.repository.QueryR15(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR15Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "shop_type_name": item.ShopTypeName, "period": item.Period, "sales_amount": item.SalesAmount, "rent_income": item.RentIncome})
		}
		return columns, rows, nil
	case ReportR16:
		items, err := s.repository.QueryR16(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := agingDepartmentColumns(false)
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, agingDepartmentRow(item.DepartmentName, "", item.DepositAmount, item.AgingBuckets))
		}
		return columns, rows, nil
	case ReportR17:
		items, err := s.repository.QueryR17(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := agingDepartmentColumns(true)
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, agingDepartmentRow(item.DepartmentName, item.ChargeType, item.DepositAmount, item.AgingBuckets))
		}
		return columns, rows, nil
	case ReportR18:
		items, err := s.repository.QueryR18(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR18Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"customer_name": item.CustomerName, "store_name": item.StoreName, "unit_name": item.UnitName, "brand_name": item.BrandName, "period": item.Period, "rent_area": item.RentArea, "current_sales": item.CurrentSales, "comparable_sales": item.ComparableSales, "same_period_sales": item.SamePeriodSales, "period_receivable": item.PeriodReceivable, "period_received": item.PeriodReceived, "period_arrears": item.PeriodArrears, "cumulative_receivable": item.CumulativeReceivable, "cumulative_arrears": item.CumulativeArrears, "days_in_month": item.DaysInMonth, "efficiency": item.Efficiency})
		}
		return columns, rows, nil
	case ReportR12:
		items, err := s.repository.QueryR12(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		columns := reportR12Columns()
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, map[string]any{"store_name": item.StoreName, "period": item.Period, "shop_type_name": item.ShopTypeName, "occupancy_status": item.OccupancyStatus, "area_total": item.AreaTotal})
		}
		return columns, rows, nil
	default:
		return nil, nil, ErrUnsupportedReport
	}
}

func flattenR19Visual(visual *R19Result) ([]Column, []map[string]any) {
	columns := reportR19Columns()
	rows := make([]map[string]any, 0, len(visual.Units))
	for _, item := range visual.Units {
		rows = append(rows, map[string]any{"unit_code": item.UnitCode, "unit_name": item.UnitName, "floor_area": item.FloorArea, "rent_area": item.RentArea, "rent_status": item.RentStatus, "brand_name": item.BrandName, "customer_name": item.CustomerName, "shop_type_name": item.ShopTypeName, "pos_x": item.PosX, "pos_y": item.PosY, "color_hex": item.ColorHex})
	}
	return columns, rows
}

func agingCustomerRow(unitCollection, customerName, tradeName, departmentName, leaseNo string, depositAmount float64, chargeType string, buckets AgingBuckets) map[string]any {
	row := agingBucketMap(buckets)
	row["unit_collection"] = unitCollection
	row["customer_name"] = customerName
	row["trade_name"] = tradeName
	row["department_name"] = departmentName
	row["lease_no"] = leaseNo
	row["deposit_amount"] = depositAmount
	if chargeType != "" {
		row["charge_type"] = chargeType
	}
	return row
}

func agingDepartmentRow(departmentName, chargeType string, depositAmount float64, buckets AgingBuckets) map[string]any {
	row := agingBucketMap(buckets)
	row["department_name"] = departmentName
	row["deposit_amount"] = depositAmount
	if chargeType != "" {
		row["charge_type"] = chargeType
	}
	return row
}

func agingBucketMap(b AgingBuckets) map[string]any {
	return map[string]any{
		"within_one_month":      b.WithinOneMonth,
		"one_to_two_months":     b.OneToTwoMonths,
		"two_to_three_months":   b.TwoToThreeMonths,
		"three_to_six_months":   b.ThreeToSixMonths,
		"six_to_nine_months":    b.SixToNineMonths,
		"nine_to_twelve_months": b.NineToTwelveMonths,
		"one_to_two_years":      b.OneToTwoYears,
		"two_to_three_years":    b.TwoToThreeYears,
		"over_three_years":      b.OverThreeYears,
		"total":                 b.Total,
	}
}

func (r R04Row) DayValue(day int) float64 {
	switch day {
	case 1:
		return r.Day01
	case 2:
		return r.Day02
	case 3:
		return r.Day03
	case 4:
		return r.Day04
	case 5:
		return r.Day05
	case 6:
		return r.Day06
	case 7:
		return r.Day07
	case 8:
		return r.Day08
	case 9:
		return r.Day09
	case 10:
		return r.Day10
	case 11:
		return r.Day11
	case 12:
		return r.Day12
	case 13:
		return r.Day13
	case 14:
		return r.Day14
	case 15:
		return r.Day15
	case 16:
		return r.Day16
	case 17:
		return r.Day17
	case 18:
		return r.Day18
	case 19:
		return r.Day19
	case 20:
		return r.Day20
	case 21:
		return r.Day21
	case 22:
		return r.Day22
	case 23:
		return r.Day23
	case 24:
		return r.Day24
	case 25:
		return r.Day25
	case 26:
		return r.Day26
	case 27:
		return r.Day27
	case 28:
		return r.Day28
	case 29:
		return r.Day29
	case 30:
		return r.Day30
	case 31:
		return r.Day31
	default:
		return 0
	}
}

func (r R10Row) MonthValue(month int) int {
	switch month {
	case 1:
		return r.Month01
	case 2:
		return r.Month02
	case 3:
		return r.Month03
	case 4:
		return r.Month04
	case 5:
		return r.Month05
	case 6:
		return r.Month06
	case 7:
		return r.Month07
	case 8:
		return r.Month08
	case 9:
		return r.Month09
	case 10:
		return r.Month10
	case 11:
		return r.Month11
	case 12:
		return r.Month12
	default:
		return 0
	}
}
