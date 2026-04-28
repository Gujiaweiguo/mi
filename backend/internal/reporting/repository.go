package reporting

import (
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"sort"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

type reportAuditPayload struct {
	Period           string  `json:"period,omitempty"`
	StoreID          *int64  `json:"store_id,omitempty"`
	FloorID          *int64  `json:"floor_id,omitempty"`
	AreaID           *int64  `json:"area_id,omitempty"`
	UnitID           *int64  `json:"unit_id,omitempty"`
	DepartmentID     *int64  `json:"department_id,omitempty"`
	ShopTypeID       *int64  `json:"shop_type_id,omitempty"`
	CustomerID       *int64  `json:"customer_id,omitempty"`
	BrandID          *int64  `json:"brand_id,omitempty"`
	TradeID          *int64  `json:"trade_id,omitempty"`
	ChargeType       *string `json:"charge_type,omitempty"`
	ManagementTypeID *int64  `json:"management_type_id,omitempty"`
	Status           *string `json:"status,omitempty"`
}

func (r *Repository) InsertReportAudit(ctx context.Context, action ReportAuditAction, input QueryInput, rowCount int, exportBytes int) error {
	payload, err := json.Marshal(reportAuditPayload{
		Period:           input.PeriodLabel,
		StoreID:          input.StoreID,
		FloorID:          input.FloorID,
		AreaID:           input.AreaID,
		UnitID:           input.UnitID,
		DepartmentID:     input.DepartmentID,
		ShopTypeID:       input.ShopTypeID,
		CustomerID:       input.CustomerID,
		BrandID:          input.BrandID,
		TradeID:          input.TradeID,
		ChargeType:       input.ChargeType,
		ManagementTypeID: input.ManagementTypeID,
		Status:           input.Status,
	})
	if err != nil {
		return fmt.Errorf("marshal report audit payload: %w", err)
	}
	if _, err := r.db.ExecContext(ctx, `
		INSERT INTO report_audit_logs (report_id, action, actor_user_id, row_count, export_size_bytes, request_payload)
		VALUES (?, ?, ?, ?, ?, ?)
	`, input.ReportID, action, input.RequestedByID, rowCount, exportBytes, payload); err != nil {
		return fmt.Errorf("insert report audit: %w", err)
	}
	return nil
}

func (r *Repository) QueryR01(ctx context.Context, input QueryInput) ([]R01Row, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			d.name AS department_name,
			CASE
				WHEN u.is_rentable = FALSE THEN 'non_rentable'
				WHEN EXISTS (
					SELECT 1
					FROM lease_contract_units lcu
					INNER JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
					WHERE lcu.unit_id = u.id
					  AND lc.status = 'active'
					  AND lc.start_date <= ?
					  AND lc.end_date >= ?
				) THEN 'leased'
				ELSE 'vacant'
			END AS rent_status,
			SUM(u.use_area) AS use_area_total
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		INNER JOIN departments d ON d.id = s.department_id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR d.id = ?)
		GROUP BY s.id, s.name, d.name, rent_status
		ORDER BY s.id, rent_status
	`, input.PeriodEnd, input.PeriodStart, input.StoreID, input.StoreID, input.DepartmentID, input.DepartmentID)
	if err != nil {
		return nil, fmt.Errorf("query R01 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R01Row, 0)
	for rows.Next() {
		var item R01Row
		if err := rows.Scan(&item.StoreName, &item.DepartmentName, &item.RentStatus, &item.UseAreaTotal); err != nil {
			return nil, fmt.Errorf("scan R01 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R01 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR02(ctx context.Context, input QueryInput) ([]R02Row, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			lc.lease_no,
			COALESCE(c.code, ''),
			COALESCE(c.name, lc.tenant_name),
			COALESCE(td.name, ''),
			COALESCE(mt.name, ''),
			u.code,
			u.name,
			lcu.rent_area,
			COALESCE(b.name, ''),
			COALESCE(st.name, ''),
			d.name,
			s.name
		FROM lease_contracts lc
		INNER JOIN lease_contract_units lcu ON lcu.lease_contract_id = lc.id
		INNER JOIN units u ON u.id = lcu.unit_id
		INNER JOIN stores s ON s.id = lc.store_id
		INNER JOIN departments d ON d.id = lc.department_id
		LEFT JOIN customers c ON c.id = lc.customer_id
		LEFT JOIN brands b ON b.id = lc.brand_id
		LEFT JOIN trade_definitions td ON td.id = lc.trade_id
		LEFT JOIN store_management_types mt ON mt.id = lc.management_type_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		WHERE (? IS NULL OR lc.status = ?)
		  AND (? IS NULL OR lc.store_id = ?)
		  AND (? IS NULL OR lc.department_id = ?)
		  AND (? IS NULL OR lc.management_type_id = ?)
		  AND (? IS NULL OR lc.customer_id = ?)
		  AND (? IS NULL OR lc.trade_id = ?)
		ORDER BY lc.id DESC, lcu.id
	`, input.Status, input.Status, input.StoreID, input.StoreID, input.DepartmentID, input.DepartmentID, input.ManagementTypeID, input.ManagementTypeID, input.CustomerID, input.CustomerID, input.TradeID, input.TradeID)
	if err != nil {
		return nil, fmt.Errorf("query R02 rows: %w", err)
	}
	defer rows.Close()
	items := make([]R02Row, 0)
	for rows.Next() {
		var item R02Row
		if err := rows.Scan(&item.LeaseNo, &item.CustomerCode, &item.CustomerName, &item.TradeName, &item.ManagementTypeName, &item.UnitCode, &item.UnitName, &item.RentArea, &item.BrandName, &item.ShopTypeName, &item.DepartmentName, &item.StoreName); err != nil {
			return nil, fmt.Errorf("scan R02 row: %w", err)
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) QueryR11(ctx context.Context, input QueryInput) ([]R11Row, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			? AS period,
			SUM(CASE
				WHEN EXISTS (
					SELECT 1
					FROM lease_contract_units lcu
					INNER JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
					WHERE lcu.unit_id = u.id
					  AND lc.status = 'active'
					  AND lc.start_date <= ?
					  AND lc.end_date >= ?
				) THEN u.use_area
				ELSE 0
			END) AS leased_area,
			SUM(u.use_area) AS total_area
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		INNER JOIN departments d ON d.id = s.department_id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR d.id = ?)
		GROUP BY s.id, s.name
		ORDER BY s.id
	`, input.PeriodLabel, input.PeriodEnd, input.PeriodStart, input.StoreID, input.StoreID, input.DepartmentID, input.DepartmentID)
	if err != nil {
		return nil, fmt.Errorf("query R11 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R11Row, 0)
	for rows.Next() {
		var item R11Row
		if err := rows.Scan(&item.StoreName, &item.Period, &item.LeasedArea, &item.TotalArea); err != nil {
			return nil, fmt.Errorf("scan R11 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R11 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR03(ctx context.Context, input QueryInput) ([]R03Row, error) {
	priorStart := input.PeriodStart.AddDate(-1, 0, 0)
	priorEnd := input.PeriodEnd.AddDate(-1, 0, 0)
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			COALESCE(st.name, 'Unassigned') AS shop_type_name,
			COALESCE(SUM(rent.rent_area), 0) AS rent_area,
			COALESCE(SUM(cs.sales_amount), 0) AS current_sales,
			COALESCE(SUM(lys.sales_amount), 0) AS same_period_sales,
			COALESCE(SUM(lys.sales_amount), 0) AS comparable_sales,
			COALESCE(SUM(rent.monthly_rent), 0) AS monthly_rent
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		LEFT JOIN (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		) cs ON cs.unit_id = u.id
		LEFT JOIN (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		) lys ON lys.unit_id = u.id
		LEFT JOIN (
			SELECT
				lcu.unit_id,
				SUM(lcu.rent_area) AS rent_area,
				SUM(
					CASE
						WHEN lc.status = 'active' AND lct.term_type = 'rent' THEN
							CASE lct.billing_cycle
								WHEN 'monthly' THEN lct.amount
								WHEN 'quarterly' THEN lct.amount / 3
								WHEN 'yearly' THEN lct.amount / 12
								ELSE lct.amount
							END
						ELSE 0
					END
				) AS monthly_rent
			FROM lease_contract_units lcu
			INNER JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
			INNER JOIN lease_contract_terms lct ON lct.lease_contract_id = lc.id
			WHERE lc.start_date <= ?
			  AND lc.end_date >= ?
			GROUP BY lcu.unit_id
		) rent ON rent.unit_id = u.id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR u.shop_type_id = ?)
		GROUP BY st.id, shop_type_name
		ORDER BY shop_type_name
	`, input.PeriodStart, input.PeriodEnd, priorStart, priorEnd, input.PeriodEnd, input.PeriodStart, input.StoreID, input.StoreID, input.ShopTypeID, input.ShopTypeID)
	if err != nil {
		return nil, fmt.Errorf("query R03 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R03Row, 0)
	for rows.Next() {
		var item R03Row
		if err := rows.Scan(&item.ShopTypeName, &item.RentArea, &item.CurrentSales, &item.SamePeriodSales, &item.ComparableSales, &item.MonthlyRent); err != nil {
			return nil, fmt.Errorf("scan R03 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R03 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR04(ctx context.Context, input QueryInput) ([]R04Row, error) {
	query := fmt.Sprintf(`
		SELECT
			u.code AS unit_code,
			u.name AS unit_name,
			u.rent_area,
			COALESCE(st.name, '') AS shop_type_name,
			COALESCE(SUM(dss.sales_amount), 0) AS total_sales,
			%s
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		LEFT JOIN daily_shop_sales dss ON dss.unit_id = u.id
		  AND dss.sale_date BETWEEN ? AND ?
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR u.id = ?)
		GROUP BY u.id, u.code, u.name, u.rent_area, st.name
		ORDER BY u.code
	`, dailySalesPivotSelect())
	_ = input.CustomerID // reserved for future filter expansion
	rows, err := r.db.QueryContext(ctx, query, input.PeriodStart, input.PeriodEnd, input.StoreID, input.StoreID, nilInt64(), nilInt64())
	if err != nil {
		return nil, fmt.Errorf("query R04 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R04Row, 0)
	for rows.Next() {
		var item R04Row
		if err := rows.Scan(
			&item.UnitCode, &item.UnitName, &item.RentArea, &item.ShopType, &item.TotalSales,
			&item.Day01, &item.Day02, &item.Day03, &item.Day04, &item.Day05, &item.Day06, &item.Day07, &item.Day08, &item.Day09, &item.Day10,
			&item.Day11, &item.Day12, &item.Day13, &item.Day14, &item.Day15, &item.Day16, &item.Day17, &item.Day18, &item.Day19, &item.Day20,
			&item.Day21, &item.Day22, &item.Day23, &item.Day24, &item.Day25, &item.Day26, &item.Day27, &item.Day28, &item.Day29, &item.Day30, &item.Day31,
		); err != nil {
			return nil, fmt.Errorf("scan R04 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R04 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR05(ctx context.Context, input QueryInput) ([]R05Row, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			u.code AS unit_code,
			u.floor_area,
			urb.budget_price,
			lease.current_lease_price,
			COALESCE(pc.name, '') AS potential_customer,
			COALESCE(pb.name, '') AS prospect_brand,
			COALESCE(ptd.name, '') AS prospect_trade,
			up.avg_transaction,
			up.prospect_rent_price,
			COALESCE(up.rent_increment, '') AS rent_increment,
			up.prospect_term_months
		FROM units u
		INNER JOIN floors f ON f.id = u.floor_id
		INNER JOIN buildings bld ON bld.id = u.building_id
		INNER JOIN stores s ON s.id = bld.store_id
		LEFT JOIN unit_rent_budgets urb ON urb.unit_id = u.id
		  AND urb.fiscal_year = YEAR(?)
		LEFT JOIN unit_prospects up ON up.unit_id = u.id
		  AND up.fiscal_year = YEAR(?)
		LEFT JOIN customers pc ON pc.id = up.potential_customer_id
		LEFT JOIN brands pb ON pb.id = up.prospect_brand_id
		LEFT JOIN trade_definitions ptd ON ptd.id = up.prospect_trade_id
		LEFT JOIN (
			SELECT
				lcu.unit_id,
				MAX(
					CASE lct.billing_cycle
						WHEN 'monthly' THEN lct.amount
						WHEN 'quarterly' THEN lct.amount / 3
						WHEN 'yearly' THEN lct.amount / 12
						ELSE lct.amount
					END
				) AS current_lease_price
			FROM lease_contract_units lcu
			INNER JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
			INNER JOIN lease_contract_terms lct ON lct.lease_contract_id = lc.id
			WHERE lc.status = 'active'
			  AND lc.start_date <= ?
			  AND lc.end_date >= ?
			  AND lct.term_type = 'rent'
			  AND lct.effective_from <= ?
			  AND lct.effective_to >= ?
			GROUP BY lcu.unit_id
		) lease ON lease.unit_id = u.id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR f.id = ?)
		  AND (? IS NULL OR u.id = ?)
		ORDER BY u.code
	`, input.PeriodStart, input.PeriodStart, input.PeriodEnd, input.PeriodStart, input.PeriodEnd, input.PeriodStart, input.StoreID, input.StoreID, input.FloorID, input.FloorID, input.UnitID, input.UnitID)
	if err != nil {
		return nil, fmt.Errorf("query R05 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R05Row, 0)
	for rows.Next() {
		var (
			item           R05Row
			budgetPrice    sql.NullFloat64
			leasePrice     sql.NullFloat64
			avgTransaction sql.NullFloat64
			prospectRent   sql.NullFloat64
			prospectTerm   sql.NullInt64
		)
		if err := rows.Scan(&item.UnitCode, &item.FloorArea, &budgetPrice, &leasePrice, &item.PotentialCustomer, &item.ProspectBrand, &item.ProspectTrade, &avgTransaction, &prospectRent, &item.RentIncrement, &prospectTerm); err != nil {
			return nil, fmt.Errorf("scan R05 row: %w", err)
		}
		item.BudgetUnitPrice = sqlutil.NullFloat64Pointer(budgetPrice)
		item.CurrentLeasePrice = sqlutil.NullFloat64Pointer(leasePrice)
		item.AverageTicket = sqlutil.NullFloat64Pointer(avgTransaction)
		item.ProspectRentPrice = sqlutil.NullFloat64Pointer(prospectRent)
		item.ProspectTermMonths = sqlutil.NullIntPointer(prospectTerm)
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R05 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR06(ctx context.Context, input QueryInput) ([]R06Row, error) {
	yearStart := timeDate(input.PeriodStart.Year(), 1, 1)
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			? AS period,
			COALESCE(period_recv.receivable, 0) AS period_receivable,
			COALESCE(period_paid.received, 0) AS period_received,
			COALESCE(current_budget.monthly_budget, 0) AS monthly_budget,
			COALESCE(annual_budget.annual_budget, 0) AS annual_budget,
			COALESCE(ytd_paid.ytd_received, 0) AS ytd_cumulative
		FROM stores s
		LEFT JOIN (
			SELECT ls.store_id, SUM(bdl.amount) AS receivable
			FROM billing_document_lines bdl
			INNER JOIN billing_documents bd ON bd.id = bdl.billing_document_id
			INNER JOIN (
				SELECT lcu.lease_contract_id, MIN(a.store_id) AS store_id
				FROM lease_contract_units lcu
				INNER JOIN units u ON u.id = lcu.unit_id
				INNER JOIN areas a ON a.id = u.area_id
				GROUP BY lcu.lease_contract_id
			) ls ON ls.lease_contract_id = bd.lease_contract_id
			WHERE bd.status = 'approved'
			  AND (bdl.charge_type = 'rent' OR bdl.charge_source = 'overtime')
			  AND DATE_FORMAT(bdl.period_start, '%Y-%m') = ?
			GROUP BY ls.store_id
		) period_recv ON period_recv.store_id = s.id
		LEFT JOIN (
			SELECT ls.store_id, SUM(ape.amount) AS received
			FROM ar_payment_entries ape
			INNER JOIN billing_documents bd ON bd.id = ape.billing_document_id
			INNER JOIN (
				SELECT lcu.lease_contract_id, MIN(a.store_id) AS store_id
				FROM lease_contract_units lcu
				INNER JOIN units u ON u.id = lcu.unit_id
				INNER JOIN areas a ON a.id = u.area_id
				GROUP BY lcu.lease_contract_id
			) ls ON ls.lease_contract_id = ape.lease_contract_id
			WHERE bd.document_type = 'invoice'
			  AND ape.payment_date >= ?
			  AND ape.payment_date <= ?
			GROUP BY ls.store_id
		) period_paid ON period_paid.store_id = s.id
		LEFT JOIN (
			SELECT store_id, monthly_budget
			FROM store_rent_budgets
			WHERE fiscal_year = YEAR(?)
			  AND fiscal_month = MONTH(?)
		) current_budget ON current_budget.store_id = s.id
		LEFT JOIN (
			SELECT store_id, SUM(monthly_budget) AS annual_budget
			FROM store_rent_budgets
			WHERE fiscal_year = YEAR(?)
			GROUP BY store_id
		) annual_budget ON annual_budget.store_id = s.id
		LEFT JOIN (
			SELECT ls.store_id, SUM(ape.amount) AS ytd_received
			FROM ar_payment_entries ape
			INNER JOIN billing_documents bd ON bd.id = ape.billing_document_id
			INNER JOIN (
				SELECT lcu.lease_contract_id, MIN(a.store_id) AS store_id
				FROM lease_contract_units lcu
				INNER JOIN units u ON u.id = lcu.unit_id
				INNER JOIN areas a ON a.id = u.area_id
				GROUP BY lcu.lease_contract_id
			) ls ON ls.lease_contract_id = ape.lease_contract_id
			WHERE bd.document_type = 'invoice'
			  AND ape.payment_date >= ?
			  AND ape.payment_date <= ?
			GROUP BY ls.store_id
		) ytd_paid ON ytd_paid.store_id = s.id
		WHERE (? IS NULL OR s.id = ?)
		ORDER BY s.name
	`, input.PeriodLabel, input.PeriodLabel, input.PeriodStart, input.PeriodEnd, input.PeriodStart, input.PeriodStart, input.PeriodStart, yearStart, input.PeriodEnd, input.StoreID, input.StoreID)
	if err != nil {
		return nil, fmt.Errorf("query R06 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R06Row, 0)
	for rows.Next() {
		var item R06Row
		if err := rows.Scan(&item.StoreName, &item.Period, &item.PeriodReceivable, &item.PeriodReceived, &item.MonthlyBudget, &item.AnnualBudget, &item.YTDCumulative); err != nil {
			return nil, fmt.Errorf("scan R06 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R06 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR10(ctx context.Context, input QueryInput) ([]R10Row, error) {
	yearStart := timeDate(input.PeriodStart.Year(), 1, 1)
	yearEnd := timeDate(input.PeriodStart.Year(), 12, 31)
	rows, err := r.db.QueryContext(ctx, fmt.Sprintf(`
		SELECT
			s.name AS store_name,
			YEAR(?) AS year,
			COALESCE(SUM(ct.inbound_count), 0) AS monthly_total,
			%s
		FROM stores s
		LEFT JOIN customer_traffic ct ON ct.store_id = s.id
		  AND ct.traffic_date BETWEEN ? AND ?
		WHERE (? IS NULL OR s.id = ?)
		GROUP BY s.id, s.name
		ORDER BY s.id
	`, trafficMonthlyPivotSelect()), yearStart, yearStart, yearEnd, input.StoreID, input.StoreID)
	if err != nil {
		return nil, fmt.Errorf("query R10 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R10Row, 0)
	for rows.Next() {
		var item R10Row
		if err := rows.Scan(&item.StoreName, &item.Year, &item.MonthlyTotal, &item.Month01, &item.Month02, &item.Month03, &item.Month04, &item.Month05, &item.Month06, &item.Month07, &item.Month08, &item.Month09, &item.Month10, &item.Month11, &item.Month12); err != nil {
			return nil, fmt.Errorf("scan R10 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R10 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR07(ctx context.Context, input QueryInput) ([]R07Row, error) {
	yearStart := timeDate(input.PeriodStart.Year(), 1, 1)
	yearEnd := timeDate(input.PeriodStart.Year(), 12, 31)
	rows, err := r.db.QueryContext(ctx, fmt.Sprintf(`
		SELECT
			s.name AS store_name,
			COALESCE(b.name, 'Unassigned') AS brand_name,
			COALESCE(SUM(dss.sales_amount), 0) AS annual_total,
			%s
		FROM daily_shop_sales dss
		INNER JOIN units u ON u.id = dss.unit_id
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		LEFT JOIN lease_contract_units lcu ON lcu.unit_id = u.id
		LEFT JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
			AND dss.sale_date BETWEEN lc.start_date AND lc.end_date
		LEFT JOIN brands b ON b.id = lc.brand_id
		WHERE dss.sale_date BETWEEN ? AND ?
		  AND (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR lc.brand_id = ?)
		GROUP BY s.id, s.name, brand_name
		ORDER BY s.name, brand_name
	`, salesMonthlyPivotSelect("dss.sale_date", "dss.sales_amount")), yearStart, yearEnd, input.StoreID, input.StoreID, input.BrandID, input.BrandID)
	if err != nil {
		return nil, fmt.Errorf("query R07 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R07Row, 0)
	for rows.Next() {
		var item R07Row
		if err := rows.Scan(
			&item.StoreName,
			&item.BrandName,
			&item.AnnualTotal,
			&item.Month01,
			&item.Month02,
			&item.Month03,
			&item.Month04,
			&item.Month05,
			&item.Month06,
			&item.Month07,
			&item.Month08,
			&item.Month09,
			&item.Month10,
			&item.Month11,
			&item.Month12,
		); err != nil {
			return nil, fmt.Errorf("scan R07 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R07 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR13(ctx context.Context, input QueryInput) ([]R13Row, error) {
	year := input.PeriodStart.Year()
	ytdStart := time.Date(year, 1, 1, 0, 0, 0, 0, time.UTC)
	prevStart := input.PeriodStart.AddDate(0, -1, 0)
	prevEnd := prevStart.AddDate(0, 1, 0).Add(-time.Nanosecond)
	lytdStart := ytdStart.AddDate(-1, 0, 0)
	lytdEnd := input.PeriodEnd.AddDate(-1, 0, 0)

	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			COALESCE(st.name, 'Unassigned') AS shop_type_name,
			? AS period,
			COALESCE(SUM(CASE WHEN dss.sale_date BETWEEN ? AND ? THEN dss.sales_amount ELSE 0 END), 0) AS current_sales,
			COALESCE(SUM(CASE WHEN dss.sale_date BETWEEN ? AND ? THEN dss.sales_amount ELSE 0 END), 0) AS ytd_sales,
			COALESCE(SUM(CASE WHEN dss.sale_date BETWEEN ? AND ? THEN dss.sales_amount ELSE 0 END), 0) AS prev_month_sales,
			COALESCE(SUM(CASE WHEN dss.sale_date BETWEEN ? AND ? THEN dss.sales_amount ELSE 0 END), 0) AS last_year_ytd_sales
		FROM daily_shop_sales dss
		INNER JOIN units u ON u.id = dss.unit_id
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		WHERE dss.sale_date BETWEEN ? AND ?
		  AND (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR u.shop_type_id = ?)
		GROUP BY s.id, s.name, shop_type_name
		ORDER BY s.name, shop_type_name
	`, input.PeriodLabel,
		input.PeriodStart, input.PeriodEnd,
		ytdStart, input.PeriodEnd,
		prevStart, prevEnd,
		lytdStart, lytdEnd,
		lytdStart, input.PeriodEnd,
		input.StoreID, input.StoreID,
		input.ShopTypeID, input.ShopTypeID,
	)
	if err != nil {
		return nil, fmt.Errorf("query R13 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R13Row, 0)
	for rows.Next() {
		var item R13Row
		if err := rows.Scan(&item.StoreName, &item.ShopTypeName, &item.Period, &item.CurrentSales, &item.YTDSales, &item.PrevMonthSales, &item.LastYearYTDSales); err != nil {
			return nil, fmt.Errorf("scan R13 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R13 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR14(ctx context.Context, input QueryInput) ([]R14Row, error) {
	daysInMonth := daysInMonth(input.PeriodStart)
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			COALESCE(st.name, 'Unassigned') AS shop_type_name,
			? AS period,
			COALESCE(SUM(sales.sales_amount), 0) AS sales_amount,
			COALESCE(SUM(u.rent_area), 0) AS area_total
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		LEFT JOIN (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		) sales ON sales.unit_id = u.id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR u.shop_type_id = ?)
		GROUP BY s.id, s.name, shop_type_name
		ORDER BY s.name, shop_type_name
	`, input.PeriodLabel, input.PeriodStart, input.PeriodEnd, input.StoreID, input.StoreID, input.ShopTypeID, input.ShopTypeID)
	if err != nil {
		return nil, fmt.Errorf("query R14 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R14Row, 0)
	for rows.Next() {
		var (
			item      R14Row
			salesAmt  float64
			areaTotal float64
		)
		if err := rows.Scan(&item.StoreName, &item.ShopTypeName, &item.Period, &salesAmt, &areaTotal); err != nil {
			return nil, fmt.Errorf("scan R14 row: %w", err)
		}
		item.SalesAmount = salesAmt
		item.AreaTotal = areaTotal
		item.DaysInMonth = daysInMonth
		if areaTotal > 0 {
			eff := salesAmt / float64(daysInMonth) / areaTotal
			item.Efficiency = &eff
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R14 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR15(ctx context.Context, input QueryInput) ([]R15Row, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			COALESCE(st.name, 'Unassigned') AS shop_type_name,
			? AS period,
			COALESCE(SUM(sales.sales_amount), 0) AS sales_amount,
			COALESCE(SUM(rent.monthly_rent), 0) AS rent_income
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		LEFT JOIN (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		) sales ON sales.unit_id = u.id
		LEFT JOIN (
			SELECT
				lcu.unit_id,
				SUM(
					CASE lct.billing_cycle
						WHEN 'monthly' THEN lct.amount
						WHEN 'quarterly' THEN lct.amount / 3
						WHEN 'yearly' THEN lct.amount / 12
						ELSE lct.amount
					END
				) AS monthly_rent
			FROM lease_contract_units lcu
			INNER JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
			INNER JOIN lease_contract_terms lct ON lct.lease_contract_id = lc.id
			WHERE lc.status = 'active'
			  AND lc.start_date <= ?
			  AND lc.end_date >= ?
			  AND lct.term_type = 'rent'
			  AND lct.effective_from <= ?
			  AND lct.effective_to >= ?
			GROUP BY lcu.unit_id
		) rent ON rent.unit_id = u.id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR u.shop_type_id = ?)
		GROUP BY s.id, s.name, shop_type_name
		ORDER BY s.name, shop_type_name
	`, input.PeriodLabel, input.PeriodStart, input.PeriodEnd, input.PeriodEnd, input.PeriodStart, input.PeriodEnd, input.PeriodStart, input.StoreID, input.StoreID, input.ShopTypeID, input.ShopTypeID)
	if err != nil {
		return nil, fmt.Errorf("query R15 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R15Row, 0)
	for rows.Next() {
		var item R15Row
		if err := rows.Scan(&item.StoreName, &item.ShopTypeName, &item.Period, &item.SalesAmount, &item.RentIncome); err != nil {
			return nil, fmt.Errorf("scan R15 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R15 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR18(ctx context.Context, input QueryInput) ([]R18Row, error) {
	priorStart := input.PeriodStart.AddDate(0, -1, 0)
	priorEnd := priorStart.AddDate(0, 1, 0).Add(-time.Nanosecond)
	lyStart := input.PeriodStart.AddDate(-1, 0, 0)
	lyEnd := input.PeriodEnd.AddDate(-1, 0, 0)
	days := daysInMonth(input.PeriodStart)
	rows, err := r.db.QueryContext(ctx, `
		WITH active_units AS (
			SELECT
				lc.id AS lease_contract_id,
				lc.customer_id,
				lc.brand_id,
				lc.store_id,
				lcu.unit_id,
				lcu.rent_area
			FROM lease_contracts lc
			INNER JOIN lease_contract_units lcu ON lcu.lease_contract_id = lc.id
			WHERE lc.status = 'active'
			  AND lc.start_date <= ?
			  AND lc.end_date >= ?
		),
		current_sales AS (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		),
		prior_sales AS (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		),
		ly_sales AS (
			SELECT unit_id, SUM(sales_amount) AS sales_amount
			FROM daily_shop_sales
			WHERE sale_date BETWEEN ? AND ?
			GROUP BY unit_id
		),
		period_receivable AS (
			SELECT bd.lease_contract_id, SUM(bdl.amount) AS amount
			FROM billing_document_lines bdl
			INNER JOIN billing_documents bd ON bd.id = bdl.billing_document_id
			WHERE bd.status = 'approved'
			  AND DATE_FORMAT(bdl.period_start, '%Y-%m') = ?
			GROUP BY bd.lease_contract_id
		),
		period_received AS (
			SELECT ape.lease_contract_id, SUM(ape.amount) AS amount
			FROM ar_payment_entries ape
			INNER JOIN billing_documents bd ON bd.id = ape.billing_document_id
			WHERE bd.document_type = 'invoice'
			  AND ape.payment_date >= ? AND ape.payment_date <= ?
			GROUP BY ape.lease_contract_id
		),
		cumulative_receivable AS (
			SELECT bd.lease_contract_id, SUM(bdl.amount) AS amount
			FROM billing_document_lines bdl
			INNER JOIN billing_documents bd ON bd.id = bdl.billing_document_id
			WHERE bd.status = 'approved'
			  AND bdl.period_end <= ?
			GROUP BY bd.lease_contract_id
		),
		cumulative_arrears AS (
			SELECT lease_contract_id, SUM(outstanding_amount) AS amount
			FROM ar_open_items
			WHERE is_deposit = FALSE AND due_date <= ?
			GROUP BY lease_contract_id
		)
		SELECT
			c.name AS customer_name,
			s.name AS store_name,
			u.name AS unit_name,
			COALESCE(b.name, '') AS brand_name,
			? AS period,
			tau.rent_area,
			COALESCE(cs.sales_amount, 0) AS current_sales,
			COALESCE(ps.sales_amount, 0) AS comparable_sales,
			COALESCE(lys.sales_amount, 0) AS same_period_sales,
			COALESCE(pr.amount, 0) AS period_receivable,
			COALESCE(pp.amount, 0) AS period_received,
			COALESCE(pr.amount, 0) - COALESCE(pp.amount, 0) AS period_arrears,
			COALESCE(cr.amount, 0) AS cumulative_receivable,
			COALESCE(ca.amount, 0) AS cumulative_arrears,
			? AS days_in_month,
			CASE WHEN tau.rent_area > 0 THEN COALESCE(cs.sales_amount, 0) / ? / tau.rent_area ELSE NULL END AS efficiency
		FROM active_units tau
		INNER JOIN units u ON u.id = tau.unit_id
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		INNER JOIN customers c ON c.id = tau.customer_id
		LEFT JOIN brands b ON b.id = tau.brand_id
		LEFT JOIN current_sales cs ON cs.unit_id = tau.unit_id
		LEFT JOIN prior_sales ps ON ps.unit_id = tau.unit_id
		LEFT JOIN ly_sales lys ON lys.unit_id = tau.unit_id
		LEFT JOIN period_receivable pr ON pr.lease_contract_id = tau.lease_contract_id
		LEFT JOIN period_received pp ON pp.lease_contract_id = tau.lease_contract_id
		LEFT JOIN cumulative_receivable cr ON cr.lease_contract_id = tau.lease_contract_id
		LEFT JOIN cumulative_arrears ca ON ca.lease_contract_id = tau.lease_contract_id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR tau.customer_id = ?)
		  AND (? IS NULL OR tau.brand_id = ?)
		  AND (? IS NULL OR tau.unit_id = ?)
		ORDER BY c.name, s.name, u.name
	`, input.PeriodEnd, input.PeriodStart, input.PeriodStart, input.PeriodEnd, priorStart, priorEnd, lyStart, lyEnd, input.PeriodLabel, input.PeriodStart, input.PeriodEnd, input.PeriodEnd, input.PeriodEnd, input.PeriodLabel, days, days, input.StoreID, input.StoreID, input.CustomerID, input.CustomerID, input.BrandID, input.BrandID, input.UnitID, input.UnitID)
	if err != nil {
		return nil, fmt.Errorf("query R18 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R18Row, 0)
	for rows.Next() {
		var (
			item       R18Row
			efficiency sql.NullFloat64
		)
		if err := rows.Scan(&item.CustomerName, &item.StoreName, &item.UnitName, &item.BrandName, &item.Period, &item.RentArea, &item.CurrentSales, &item.ComparableSales, &item.SamePeriodSales, &item.PeriodReceivable, &item.PeriodReceived, &item.PeriodArrears, &item.CumulativeReceivable, &item.CumulativeArrears, &item.DaysInMonth, &efficiency); err != nil {
			return nil, fmt.Errorf("scan R18 row: %w", err)
		}
		item.Efficiency = sqlutil.NullFloat64Pointer(efficiency)
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R18 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR19(ctx context.Context, input QueryInput) (*R19Result, error) {
	if input.FloorID == nil {
		return nil, ErrInvalidPeriod
	}
	var (
		floor    R19Floor
		floorURL sql.NullString
	)
	if err := r.db.QueryRowContext(ctx, `
		SELECT f.id, f.name, f.floor_plan_image_url
		FROM floors f
		INNER JOIN buildings b ON b.id = f.building_id
		WHERE f.id = ?
		  AND (? IS NULL OR b.store_id = ?)
	`, *input.FloorID, input.StoreID, input.StoreID).Scan(&floor.ID, &floor.Name, &floorURL); err != nil {
		return nil, fmt.Errorf("query R19 floor: %w", err)
	}
	floor.FloorPlanImageURL = sqlutil.NullStringPointer(floorURL)

	rows, err := r.db.QueryContext(ctx, `
		SELECT
			u.id,
			u.code,
			u.name,
			u.floor_area,
			u.rent_area,
			CASE
				WHEN u.is_rentable = FALSE THEN 'non_rentable'
				WHEN EXISTS (
					SELECT 1 FROM lease_contract_units lcu2
					INNER JOIN lease_contracts lc2 ON lc2.id = lcu2.lease_contract_id
					WHERE lcu2.unit_id = u.id AND lc2.status = 'active'
				) THEN 'leased'
				ELSE 'vacant'
			END AS rent_status,
			COALESCE(br.name, '') AS brand_name,
			COALESCE(c.name, '') AS customer_name,
			COALESCE(st.name, 'Unassigned') AS shop_type_name,
			COALESCE(ulp.pos_x, 0) AS pos_x,
			COALESCE(ulp.pos_y, 0) AS pos_y,
			COALESCE(st.color_hex, '#CCCCCC') AS color_hex
		FROM units u
		INNER JOIN floors f ON f.id = u.floor_id
		INNER JOIN buildings b ON b.id = u.building_id
		LEFT JOIN unit_layout_positions ulp ON ulp.unit_id = u.id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		LEFT JOIN lease_contract_units lcu ON lcu.unit_id = u.id
		LEFT JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id AND lc.status = 'active'
		LEFT JOIN customers c ON c.id = lc.customer_id
		LEFT JOIN brands br ON br.id = lc.brand_id
		WHERE u.floor_id = ?
		  AND (? IS NULL OR b.store_id = ?)
		  AND (? IS NULL OR u.area_id = ?)
		ORDER BY u.code
	`, *input.FloorID, input.StoreID, input.StoreID, input.AreaID, input.AreaID)
	if err != nil {
		return nil, fmt.Errorf("query R19 units: %w", err)
	}
	defer rows.Close()

	units := make([]R19Unit, 0)
	legendMap := make(map[string]string)
	for rows.Next() {
		var item R19Unit
		if err := rows.Scan(&item.UnitID, &item.UnitCode, &item.UnitName, &item.FloorArea, &item.RentArea, &item.RentStatus, &item.BrandName, &item.CustomerName, &item.ShopTypeName, &item.PosX, &item.PosY, &item.ColorHex); err != nil {
			return nil, fmt.Errorf("scan R19 unit: %w", err)
		}
		if item.ShopTypeName != "" {
			legendMap[item.ShopTypeName] = item.ColorHex
		}
		units = append(units, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R19 units: %w", err)
	}
	legend := make([]R19LegendEntry, 0, len(legendMap))
	for label, color := range legendMap {
		legend = append(legend, R19LegendEntry{Label: label, ColorHex: color})
	}
	sort.Slice(legend, func(i, j int) bool { return legend[i].Label < legend[j].Label })
	return &R19Result{Floor: floor, Units: units, Legend: legend}, nil
}

func (r *Repository) ResolveR19FloorID(ctx context.Context, input QueryInput) (*int64, error) {
	var floorID int64
	if err := r.db.QueryRowContext(ctx, `
		SELECT u.floor_id
		FROM units u
		INNER JOIN buildings b ON b.id = u.building_id
		WHERE (? IS NULL OR b.store_id = ?)
		  AND (? IS NULL OR u.area_id = ?)
		GROUP BY u.floor_id
		ORDER BY u.floor_id
		LIMIT 1
	`, input.StoreID, input.StoreID, input.AreaID, input.AreaID).Scan(&floorID); err != nil {
		if err == sql.ErrNoRows {
			return nil, fmt.Errorf("resolve R19 floor: %w", ErrUnsupportedReport)
		}
		return nil, fmt.Errorf("resolve R19 floor: %w", err)
	}
	return &floorID, nil
}

func dailySalesPivotSelect() string {
	parts := make([]string, 0, 31)
	for day := 1; day <= 31; day++ {
		parts = append(parts, fmt.Sprintf("COALESCE(SUM(CASE WHEN DAY(dss.sale_date) = %d THEN dss.sales_amount ELSE 0 END), 0) AS day_%02d", day, day))
	}
	return strings.Join(parts, ",\n\t\t\t")
}

func trafficMonthlyPivotSelect() string {
	parts := make([]string, 0, 12)
	for month := 1; month <= 12; month++ {
		parts = append(parts, fmt.Sprintf("COALESCE(SUM(CASE WHEN MONTH(ct.traffic_date) = %d THEN ct.inbound_count ELSE 0 END), 0) AS month_%02d", month, month))
	}
	return strings.Join(parts, ",\n\t\t\t")
}

func salesMonthlyPivotSelect(dateExpr, amountExpr string) string {
	parts := make([]string, 0, 12)
	for month := 1; month <= 12; month++ {
		parts = append(parts, fmt.Sprintf("COALESCE(SUM(CASE WHEN MONTH(%s) = %d THEN %s ELSE 0 END), 0) AS month_%02d", dateExpr, month, amountExpr, month))
	}
	return strings.Join(parts, ",\n\t\t\t")
}

func nilInt64() *int64 { return nil }

func timeDate(year, month, day int) string {
	return fmt.Sprintf("%04d-%02d-%02d", year, month, day)
}

func daysInMonth(value time.Time) int {
	start := time.Date(value.Year(), value.Month(), 1, 0, 0, 0, 0, time.UTC)
	end := start.AddDate(0, 1, 0).Add(-time.Nanosecond)
	return end.Day()
}

func (r *Repository) QueryR12(ctx context.Context, input QueryInput) ([]R12Row, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			s.name AS store_name,
			? AS period,
			COALESCE(st.name, 'Unassigned') AS shop_type_name,
			CASE
				WHEN u.is_rentable = FALSE THEN 'non_rentable'
				WHEN EXISTS (
					SELECT 1
					FROM lease_contract_units lcu
					INNER JOIN lease_contracts lc ON lc.id = lcu.lease_contract_id
					WHERE lcu.unit_id = u.id
					  AND lc.status = 'active'
					  AND lc.start_date <= ?
					  AND lc.end_date >= ?
				) THEN 'leased'
				ELSE 'vacant'
			END AS occupancy_status,
			SUM(u.use_area) AS area_total
		FROM units u
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN stores s ON s.id = a.store_id
		INNER JOIN departments d ON d.id = s.department_id
		LEFT JOIN shop_types st ON st.id = u.shop_type_id
		WHERE (? IS NULL OR s.id = ?)
		  AND (? IS NULL OR d.id = ?)
		  AND (? IS NULL OR u.shop_type_id = ?)
		GROUP BY s.id, s.name, shop_type_name, occupancy_status
		ORDER BY s.id, shop_type_name, occupancy_status
	`, input.PeriodLabel, input.PeriodEnd, input.PeriodStart, input.StoreID, input.StoreID, input.DepartmentID, input.DepartmentID, input.ShopTypeID, input.ShopTypeID)
	if err != nil {
		return nil, fmt.Errorf("query R12 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R12Row, 0)
	for rows.Next() {
		var item R12Row
		if err := rows.Scan(&item.StoreName, &item.Period, &item.ShopTypeName, &item.OccupancyStatus, &item.AreaTotal); err != nil {
			return nil, fmt.Errorf("scan R12 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R12 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR08(ctx context.Context, input QueryInput) ([]R08Row, error) {
	args := agingBucketArgs(input.PeriodEnd)
	args = append(args, input.PeriodEnd, input.DepartmentID, input.DepartmentID, input.CustomerID, input.CustomerID, input.TradeID, input.TradeID)
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			COALESCE(GROUP_CONCAT(DISTINCT u.code ORDER BY u.code SEPARATOR ', '), '') AS unit_collection,
			c.name AS customer_name,
			COALESCE(td.name, '') AS trade_name,
			d.name AS department_name,
			lc.lease_no,
			COALESCE(SUM(CASE WHEN ai.is_deposit THEN ai.outstanding_amount ELSE 0 END), 0) AS deposit_amount,
			`+agingBucketSelectSQL("ai")+`
		FROM ar_open_items ai
		INNER JOIN lease_contracts lc ON lc.id = ai.lease_contract_id
		INNER JOIN customers c ON c.id = ai.customer_id
		INNER JOIN departments d ON d.id = ai.department_id
		LEFT JOIN trade_definitions td ON td.id = ai.trade_id
		LEFT JOIN lease_contract_units lcu ON lcu.lease_contract_id = lc.id
		LEFT JOIN units u ON u.id = lcu.unit_id
		WHERE ai.due_date <= ?
		  AND (? IS NULL OR ai.department_id = ?)
		  AND (? IS NULL OR ai.customer_id = ?)
		  AND (? IS NULL OR ai.trade_id = ?)
		GROUP BY c.id, c.name, trade_name, d.id, d.name, lc.id, lc.lease_no
		ORDER BY d.name, c.name, lc.lease_no
	`, args...)
	if err != nil {
		return nil, fmt.Errorf("query R08 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R08Row, 0)
	for rows.Next() {
		var item R08Row
		if err := rows.Scan(
			&item.UnitCollection,
			&item.CustomerName,
			&item.TradeName,
			&item.DepartmentName,
			&item.LeaseNo,
			&item.DepositAmount,
			&item.WithinOneMonth,
			&item.OneToTwoMonths,
			&item.TwoToThreeMonths,
			&item.ThreeToSixMonths,
			&item.SixToNineMonths,
			&item.NineToTwelveMonths,
			&item.OneToTwoYears,
			&item.TwoToThreeYears,
			&item.OverThreeYears,
			&item.Total,
		); err != nil {
			return nil, fmt.Errorf("scan R08 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R08 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR09(ctx context.Context, input QueryInput) ([]R09Row, error) {
	args := agingBucketArgs(input.PeriodEnd)
	args = append(args, input.PeriodEnd, input.DepartmentID, input.DepartmentID, input.CustomerID, input.CustomerID, input.TradeID, input.TradeID, sqlutil.StringPointerValue(input.ChargeType), sqlutil.StringPointerValue(input.ChargeType))
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			COALESCE(GROUP_CONCAT(DISTINCT u.code ORDER BY u.code SEPARATOR ', '), '') AS unit_collection,
			c.name AS customer_name,
			COALESCE(td.name, '') AS trade_name,
			d.name AS department_name,
			lc.lease_no,
			COALESCE(SUM(CASE WHEN ai.is_deposit THEN ai.outstanding_amount ELSE 0 END), 0) AS deposit_amount,
			ai.charge_type,
			`+agingBucketSelectSQL("ai")+`
		FROM ar_open_items ai
		INNER JOIN lease_contracts lc ON lc.id = ai.lease_contract_id
		INNER JOIN customers c ON c.id = ai.customer_id
		INNER JOIN departments d ON d.id = ai.department_id
		LEFT JOIN trade_definitions td ON td.id = ai.trade_id
		LEFT JOIN lease_contract_units lcu ON lcu.lease_contract_id = lc.id
		LEFT JOIN units u ON u.id = lcu.unit_id
		WHERE ai.due_date <= ?
		  AND (? IS NULL OR ai.department_id = ?)
		  AND (? IS NULL OR ai.customer_id = ?)
		  AND (? IS NULL OR ai.trade_id = ?)
		  AND (? IS NULL OR ai.charge_type = ?)
		GROUP BY c.id, c.name, trade_name, d.id, d.name, lc.id, lc.lease_no, ai.charge_type
		ORDER BY d.name, c.name, lc.lease_no, ai.charge_type
	`, args...)
	if err != nil {
		return nil, fmt.Errorf("query R09 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R09Row, 0)
	for rows.Next() {
		var item R09Row
		if err := rows.Scan(
			&item.UnitCollection,
			&item.CustomerName,
			&item.TradeName,
			&item.DepartmentName,
			&item.LeaseNo,
			&item.DepositAmount,
			&item.ChargeType,
			&item.WithinOneMonth,
			&item.OneToTwoMonths,
			&item.TwoToThreeMonths,
			&item.ThreeToSixMonths,
			&item.SixToNineMonths,
			&item.NineToTwelveMonths,
			&item.OneToTwoYears,
			&item.TwoToThreeYears,
			&item.OverThreeYears,
			&item.Total,
		); err != nil {
			return nil, fmt.Errorf("scan R09 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R09 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR16(ctx context.Context, input QueryInput) ([]R16Row, error) {
	args := agingBucketArgs(input.PeriodEnd)
	args = append(args, input.PeriodEnd, input.DepartmentID, input.DepartmentID)
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			d.name AS department_name,
			COALESCE(SUM(CASE WHEN ai.is_deposit THEN ai.outstanding_amount ELSE 0 END), 0) AS deposit_amount,
			`+agingBucketSelectSQL("ai")+`
		FROM ar_open_items ai
		INNER JOIN departments d ON d.id = ai.department_id
		WHERE ai.due_date <= ?
		  AND (? IS NULL OR ai.department_id = ?)
		GROUP BY d.id, d.name
		ORDER BY d.name
	`, args...)
	if err != nil {
		return nil, fmt.Errorf("query R16 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R16Row, 0)
	for rows.Next() {
		var item R16Row
		if err := rows.Scan(
			&item.DepartmentName,
			&item.DepositAmount,
			&item.WithinOneMonth,
			&item.OneToTwoMonths,
			&item.TwoToThreeMonths,
			&item.ThreeToSixMonths,
			&item.SixToNineMonths,
			&item.NineToTwelveMonths,
			&item.OneToTwoYears,
			&item.TwoToThreeYears,
			&item.OverThreeYears,
			&item.Total,
		); err != nil {
			return nil, fmt.Errorf("scan R16 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R16 rows: %w", err)
	}
	return items, nil
}

func (r *Repository) QueryR17(ctx context.Context, input QueryInput) ([]R17Row, error) {
	args := agingBucketArgs(input.PeriodEnd)
	args = append(args, input.PeriodEnd, input.DepartmentID, input.DepartmentID, sqlutil.StringPointerValue(input.ChargeType), sqlutil.StringPointerValue(input.ChargeType))
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			d.name AS department_name,
			ai.charge_type,
			COALESCE(SUM(CASE WHEN ai.is_deposit THEN ai.outstanding_amount ELSE 0 END), 0) AS deposit_amount,
			`+agingBucketSelectSQL("ai")+`
		FROM ar_open_items ai
		INNER JOIN departments d ON d.id = ai.department_id
		WHERE ai.due_date <= ?
		  AND (? IS NULL OR ai.department_id = ?)
		  AND (? IS NULL OR ai.charge_type = ?)
		GROUP BY d.id, d.name, ai.charge_type
		ORDER BY d.name, ai.charge_type
	`, args...)
	if err != nil {
		return nil, fmt.Errorf("query R17 rows: %w", err)
	}
	defer rows.Close()

	items := make([]R17Row, 0)
	for rows.Next() {
		var item R17Row
		if err := rows.Scan(
			&item.DepartmentName,
			&item.ChargeType,
			&item.DepositAmount,
			&item.WithinOneMonth,
			&item.OneToTwoMonths,
			&item.TwoToThreeMonths,
			&item.ThreeToSixMonths,
			&item.SixToNineMonths,
			&item.NineToTwelveMonths,
			&item.OneToTwoYears,
			&item.TwoToThreeYears,
			&item.OverThreeYears,
			&item.Total,
		); err != nil {
			return nil, fmt.Errorf("scan R17 row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate R17 rows: %w", err)
	}
	return items, nil
}

func agingBucketArgs(cutoff time.Time) []any {
	args := make([]any, 0, 9)
	for i := 0; i < 9; i++ {
		args = append(args, cutoff)
	}
	return args
}

func agingBucketSelectSQL(alias string) string {
	prefix := alias + "."
	return strings.Join([]string{
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 0 AND 30 THEN %soutstanding_amount ELSE 0 END), 0) AS within_one_month", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 31 AND 60 THEN %soutstanding_amount ELSE 0 END), 0) AS one_to_two_months", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 61 AND 90 THEN %soutstanding_amount ELSE 0 END), 0) AS two_to_three_months", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 91 AND 180 THEN %soutstanding_amount ELSE 0 END), 0) AS three_to_six_months", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 181 AND 270 THEN %soutstanding_amount ELSE 0 END), 0) AS six_to_nine_months", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 271 AND 365 THEN %soutstanding_amount ELSE 0 END), 0) AS nine_to_twelve_months", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 366 AND 730 THEN %soutstanding_amount ELSE 0 END), 0) AS one_to_two_years", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) BETWEEN 731 AND 1095 THEN %soutstanding_amount ELSE 0 END), 0) AS two_to_three_years", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE AND DATEDIFF(?, %sdue_date) > 1095 THEN %soutstanding_amount ELSE 0 END), 0) AS over_three_years", prefix, prefix, prefix),
		fmt.Sprintf("COALESCE(SUM(CASE WHEN %sis_deposit = FALSE THEN %soutstanding_amount ELSE 0 END), 0) AS total", prefix, prefix),
	}, ",\n\t\t\t")
}
