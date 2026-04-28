package overtime

import (
	"context"
	"database/sql"
	"fmt"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

type Repository struct {
	db *sql.DB
}

type leaseContext struct {
	LeaseContractID int64
	LeaseNo         string
	TenantName      string
	Status          string
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db}
}

func (r *Repository) FindLeaseContext(ctx context.Context, leaseContractID int64) (*leaseContext, error) {
	return r.findLeaseContext(ctx, r.db.QueryRowContext(ctx, `
		SELECT id, lease_no, tenant_name, status
		FROM lease_contracts
		WHERE id = ?
	`, leaseContractID))
}

func (r *Repository) FindLeaseContextForUpdate(ctx context.Context, tx *sql.Tx, leaseContractID int64) (*leaseContext, error) {
	return r.findLeaseContext(ctx, tx.QueryRowContext(ctx, `
		SELECT id, lease_no, tenant_name, status
		FROM lease_contracts
		WHERE id = ?
		FOR UPDATE
	`, leaseContractID))
}

func (r *Repository) findLeaseContext(_ context.Context, scanner interface{ Scan(dest ...any) error }) (*leaseContext, error) {
	var out leaseContext
	if err := scanner.Scan(&out.LeaseContractID, &out.LeaseNo, &out.TenantName, &out.Status); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query overtime lease context: %w", err)
	}
	return &out, nil
}

func (r *Repository) CreateBill(ctx context.Context, tx *sql.Tx, bill *Bill) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO overtime_bills (
			lease_contract_id, period_start, period_end, status, note,
			created_by, updated_by
		) VALUES (?, ?, ?, ?, ?, ?, ?)
	`, bill.LeaseContractID, bill.PeriodStart, bill.PeriodEnd, bill.Status, bill.Note, bill.CreatedBy, bill.UpdatedBy)
	if err != nil {
		return fmt.Errorf("insert overtime bill: %w", err)
	}
	billID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get overtime bill id: %w", err)
	}
	bill.ID = billID
	for _, formula := range bill.Formulas {
		current := formula
		current.OvertimeBillID = billID
		if err := r.insertFormula(ctx, tx, &current); err != nil {
			return err
		}
	}
	return nil
}

func (r *Repository) insertFormula(ctx context.Context, tx *sql.Tx, formula *Formula) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO overtime_bill_formulas (
			overtime_bill_id, charge_type, formula_type, rate_type, effective_from, effective_to,
			currency_type_id, total_area, unit_price, base_amount, fixed_rental,
			percentage_option, minimum_option, sort_order
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, formula.OvertimeBillID, formula.ChargeType, formula.FormulaType, formula.RateType, formula.EffectiveFrom, formula.EffectiveTo, formula.CurrencyTypeID, formula.TotalArea, formula.UnitPrice, formula.BaseAmount, formula.FixedRental, formula.PercentageOption, formula.MinimumOption, formula.SortOrder)
	if err != nil {
		return fmt.Errorf("insert overtime formula: %w", err)
	}
	formulaID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get overtime formula id: %w", err)
	}
	formula.ID = formulaID
	for _, tier := range formula.PercentageTiers {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO overtime_formula_percentage_tiers (formula_id, sales_to, percentage_rate, sort_order)
			VALUES (?, ?, ?, ?)
		`, formulaID, tier.SalesTo, tier.Percentage, tier.SortOrder); err != nil {
			return fmt.Errorf("insert overtime percentage tier: %w", err)
		}
	}
	for _, tier := range formula.MinimumTiers {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO overtime_formula_minimum_tiers (formula_id, sales_to, minimum_sum, sort_order)
			VALUES (?, ?, ?, ?)
		`, formulaID, tier.SalesTo, tier.MinimumSum, tier.SortOrder); err != nil {
			return fmt.Errorf("insert overtime minimum tier: %w", err)
		}
	}
	return nil
}

func (r *Repository) FindBillByID(ctx context.Context, billID int64) (*Bill, error) {
	bill, err := r.findBill(ctx, r.db.QueryRowContext(ctx, billSelect+` WHERE ob.id = ?`, billID))
	if err != nil || bill == nil {
		return bill, err
	}
	if err := r.loadBillDetails(ctx, r.db, bill); err != nil {
		return nil, err
	}
	return bill, nil
}

func (r *Repository) FindBillByIDForUpdate(ctx context.Context, tx *sql.Tx, billID int64) (*Bill, error) {
	bill, err := r.findBill(ctx, tx.QueryRowContext(ctx, billSelect+` WHERE ob.id = ? FOR UPDATE`, billID))
	if err != nil || bill == nil {
		return bill, err
	}
	if err := r.loadBillDetails(ctx, tx, bill); err != nil {
		return nil, err
	}
	return bill, nil
}

func (r *Repository) findBill(_ context.Context, scanner interface{ Scan(dest ...any) error }) (*Bill, error) {
	var bill Bill
	var workflowInstanceID sql.NullInt64
	var submittedAt sql.NullTime
	var approvedAt sql.NullTime
	var rejectedAt sql.NullTime
	var cancelledAt sql.NullTime
	var stoppedAt sql.NullTime
	var generatedAt sql.NullTime
	if err := scanner.Scan(&bill.ID, &bill.LeaseContractID, &bill.LeaseNo, &bill.TenantName, &bill.PeriodStart, &bill.PeriodEnd, &bill.Status, &workflowInstanceID, &bill.Note, &submittedAt, &approvedAt, &rejectedAt, &cancelledAt, &stoppedAt, &generatedAt, &bill.CreatedBy, &bill.UpdatedBy, &bill.CreatedAt, &bill.UpdatedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query overtime bill: %w", err)
	}
	bill.WorkflowInstanceID = sqlutil.NullInt64Pointer(workflowInstanceID)
	bill.SubmittedAt = sqlutil.NullTimePointer(submittedAt)
	bill.ApprovedAt = sqlutil.NullTimePointer(approvedAt)
	bill.RejectedAt = sqlutil.NullTimePointer(rejectedAt)
	bill.CancelledAt = sqlutil.NullTimePointer(cancelledAt)
	bill.StoppedAt = sqlutil.NullTimePointer(stoppedAt)
	bill.GeneratedAt = sqlutil.NullTimePointer(generatedAt)
	return &bill, nil
}

type queryer interface {
	QueryContext(context.Context, string, ...any) (*sql.Rows, error)
}

func (r *Repository) loadBillDetails(ctx context.Context, db queryer, bill *Bill) error {
	rows, err := db.QueryContext(ctx, `
		SELECT id, overtime_bill_id, charge_type, formula_type, rate_type, effective_from, effective_to,
			currency_type_id, total_area, unit_price, base_amount, fixed_rental,
			percentage_option, minimum_option, sort_order, created_at, updated_at
		FROM overtime_bill_formulas
		WHERE overtime_bill_id = ?
		ORDER BY sort_order, id
	`, bill.ID)
	if err != nil {
		return fmt.Errorf("query overtime formulas: %w", err)
	}
	defer rows.Close()

	formulas := make([]Formula, 0)
	formulaIDs := make([]int64, 0)
	formulaIndex := make(map[int64]int)
	for rows.Next() {
		var formula Formula
		if err := rows.Scan(&formula.ID, &formula.OvertimeBillID, &formula.ChargeType, &formula.FormulaType, &formula.RateType, &formula.EffectiveFrom, &formula.EffectiveTo, &formula.CurrencyTypeID, &formula.TotalArea, &formula.UnitPrice, &formula.BaseAmount, &formula.FixedRental, &formula.PercentageOption, &formula.MinimumOption, &formula.SortOrder, &formula.CreatedAt, &formula.UpdatedAt); err != nil {
			return fmt.Errorf("scan overtime formula: %w", err)
		}
		formulaIndex[formula.ID] = len(formulas)
		formulaIDs = append(formulaIDs, formula.ID)
		formulas = append(formulas, formula)
	}
	if err := rows.Err(); err != nil {
		return fmt.Errorf("iterate overtime formulas: %w", err)
	}
	if err := r.loadFormulaTiers(ctx, db, formulas, formulaIndex, formulaIDs); err != nil {
		return err
	}
	bill.Formulas = formulas
	bill.GeneratedCharges, err = r.listGeneratedCharges(ctx, db, bill.ID)
	if err != nil {
		return err
	}
	return nil
}

func (r *Repository) loadFormulaTiers(ctx context.Context, db queryer, formulas []Formula, formulaIndex map[int64]int, formulaIDs []int64) error {
	if len(formulaIDs) == 0 {
		return nil
	}
	args := make([]any, 0, len(formulaIDs))
	for _, id := range formulaIDs {
		args = append(args, id)
	}
	percentRows, err := db.QueryContext(ctx, `
		SELECT id, formula_id, sales_to, percentage_rate, sort_order, created_at
		FROM overtime_formula_percentage_tiers
		WHERE formula_id IN (`+sqlutil.InPlaceholders(len(formulaIDs))+`)
		ORDER BY formula_id, sort_order, id
	`, args...)
	if err != nil {
		return fmt.Errorf("query overtime percentage tiers: %w", err)
	}
	defer percentRows.Close()
	for percentRows.Next() {
		var tier PercentTier
		if err := percentRows.Scan(&tier.ID, &tier.FormulaID, &tier.SalesTo, &tier.Percentage, &tier.SortOrder, &tier.CreatedAt); err != nil {
			return fmt.Errorf("scan overtime percentage tier: %w", err)
		}
		index := formulaIndex[tier.FormulaID]
		formulas[index].PercentageTiers = append(formulas[index].PercentageTiers, tier)
	}
	if err := percentRows.Err(); err != nil {
		return fmt.Errorf("iterate overtime percentage tiers: %w", err)
	}
	minimumRows, err := db.QueryContext(ctx, `
		SELECT id, formula_id, sales_to, minimum_sum, sort_order, created_at
		FROM overtime_formula_minimum_tiers
		WHERE formula_id IN (`+sqlutil.InPlaceholders(len(formulaIDs))+`)
		ORDER BY formula_id, sort_order, id
	`, args...)
	if err != nil {
		return fmt.Errorf("query overtime minimum tiers: %w", err)
	}
	defer minimumRows.Close()
	for minimumRows.Next() {
		var tier MinimumTier
		if err := minimumRows.Scan(&tier.ID, &tier.FormulaID, &tier.SalesTo, &tier.MinimumSum, &tier.SortOrder, &tier.CreatedAt); err != nil {
			return fmt.Errorf("scan overtime minimum tier: %w", err)
		}
		index := formulaIndex[tier.FormulaID]
		formulas[index].MinimumTiers = append(formulas[index].MinimumTiers, tier)
	}
	if err := minimumRows.Err(); err != nil {
		return fmt.Errorf("iterate overtime minimum tiers: %w", err)
	}
	return nil
}

func (r *Repository) listGeneratedCharges(ctx context.Context, db queryer, billID int64) ([]GeneratedCharge, error) {
	rows, err := db.QueryContext(ctx, `
		SELECT id, billing_run_id, overtime_bill_id, overtime_formula_id, lease_contract_id, workflow_instance_id,
			charge_type, formula_type, rate_type, period_start, period_end, quantity,
			total_area, unit_price, base_amount, fixed_rental, percentage_option, minimum_option,
			applied_percentage_rate, applied_minimum_amount, unit_amount, amount, currency_type_id,
			generated_by, created_at
		FROM overtime_generated_charges
		WHERE overtime_bill_id = ?
		ORDER BY id
	`, billID)
	if err != nil {
		return nil, fmt.Errorf("query overtime generated charges: %w", err)
	}
	defer rows.Close()
	charges := make([]GeneratedCharge, 0)
	for rows.Next() {
		var charge GeneratedCharge
		var workflowInstanceID sql.NullInt64
		if err := rows.Scan(&charge.ID, &charge.BillingRunID, &charge.OvertimeBillID, &charge.OvertimeFormulaID, &charge.LeaseContractID, &workflowInstanceID, &charge.ChargeType, &charge.FormulaType, &charge.RateType, &charge.PeriodStart, &charge.PeriodEnd, &charge.Quantity, &charge.TotalArea, &charge.UnitPrice, &charge.BaseAmount, &charge.FixedRental, &charge.PercentageOption, &charge.MinimumOption, &charge.AppliedPercentageRate, &charge.AppliedMinimumAmount, &charge.UnitAmount, &charge.Amount, &charge.CurrencyTypeID, &charge.GeneratedBy, &charge.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan overtime generated charge: %w", err)
		}
		charge.WorkflowInstanceID = sqlutil.NullInt64Pointer(workflowInstanceID)
		charges = append(charges, charge)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate overtime generated charges: %w", err)
	}
	return charges, nil
}

func (r *Repository) ListBills(ctx context.Context, filter ListFilter) (*pagination.ListResult[Bill], error) {
	page, pageSize := pagination.NormalizePage(filter.Page, filter.PageSize)
	conditions := []string{"1=1"}
	args := make([]any, 0)
	if filter.LeaseContractID != nil {
		conditions = append(conditions, "ob.lease_contract_id = ?")
		args = append(args, *filter.LeaseContractID)
	}
	if filter.Status != nil {
		conditions = append(conditions, "ob.status = ?")
		args = append(args, *filter.Status)
	}
	if filter.PeriodStart != nil {
		conditions = append(conditions, "ob.period_start >= ?")
		args = append(args, *filter.PeriodStart)
	}
	if filter.PeriodEnd != nil {
		conditions = append(conditions, "ob.period_end <= ?")
		args = append(args, *filter.PeriodEnd)
	}
	whereClause := strings.Join(conditions, " AND ")
	var total int64
	if err := r.db.QueryRowContext(ctx, `SELECT COUNT(*) FROM overtime_bills ob WHERE `+whereClause, args...).Scan(&total); err != nil {
		return nil, fmt.Errorf("count overtime bills: %w", err)
	}
	queryArgs := append(append([]any{}, args...), pageSize, (page-1)*pageSize)
	rows, err := r.db.QueryContext(ctx, billSelect+` WHERE `+whereClause+` ORDER BY ob.id DESC LIMIT ? OFFSET ?`, queryArgs...)
	if err != nil {
		return nil, fmt.Errorf("list overtime bills: %w", err)
	}
	defer rows.Close()
	items := make([]Bill, 0)
	for rows.Next() {
		bill, err := r.findBill(ctx, rows)
		if err != nil {
			return nil, err
		}
		items = append(items, *bill)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate overtime bills: %w", err)
	}
	return &pagination.ListResult[Bill]{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func (r *Repository) AttachWorkflowInstance(ctx context.Context, tx *sql.Tx, billID, workflowInstanceID, updatedBy int64, submittedAt time.Time) error {
	result, err := tx.ExecContext(ctx, `
		UPDATE overtime_bills
		SET status = ?, workflow_instance_id = ?, submitted_at = ?, updated_by = ?
		WHERE id = ? AND status IN (?, ?) AND workflow_instance_id IS NULL
	`, BillStatusPendingApproval, workflowInstanceID, submittedAt, updatedBy, billID, BillStatusDraft, BillStatusRejected)
	if err != nil {
		return fmt.Errorf("attach overtime workflow instance: %w", err)
	}
	affected, err := result.RowsAffected()
	if err != nil {
		return fmt.Errorf("determine overtime workflow attach rows: %w", err)
	}
	if affected != 1 {
		return ErrOvertimeAlreadySubmitted
	}
	return nil
}

func (r *Repository) UpdateWorkflowState(ctx context.Context, tx *sql.Tx, billID, workflowInstanceID, updatedBy int64, status BillStatus, approvedAt, rejectedAt *time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE overtime_bills
		SET workflow_instance_id = ?, status = ?, approved_at = ?, rejected_at = ?, updated_by = ?
		WHERE id = ?
	`, workflowInstanceID, status, sqlutil.TimePointerValue(approvedAt), sqlutil.TimePointerValue(rejectedAt), updatedBy, billID); err != nil {
		return fmt.Errorf("update overtime bill workflow state: %w", err)
	}
	return nil
}

func (r *Repository) MarkCancelled(ctx context.Context, tx *sql.Tx, billID, updatedBy int64, cancelledAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE overtime_bills
		SET status = ?, cancelled_at = ?, updated_by = ?
		WHERE id = ?
	`, BillStatusCancelled, cancelledAt, updatedBy, billID); err != nil {
		return fmt.Errorf("cancel overtime bill: %w", err)
	}
	return nil
}

func (r *Repository) MarkStopped(ctx context.Context, tx *sql.Tx, billID, updatedBy int64, stoppedAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE overtime_bills
		SET status = ?, stopped_at = ?, updated_by = ?
		WHERE id = ?
	`, BillStatusStopped, stoppedAt, updatedBy, billID); err != nil {
		return fmt.Errorf("stop overtime bill: %w", err)
	}
	return nil
}

func (r *Repository) MarkGenerated(ctx context.Context, tx *sql.Tx, billID, updatedBy int64, generatedAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE overtime_bills
		SET status = ?, generated_at = COALESCE(generated_at, ?), updated_by = ?
		WHERE id = ?
	`, BillStatusGenerated, generatedAt, updatedBy, billID); err != nil {
		return fmt.Errorf("mark overtime bill generated: %w", err)
	}
	return nil
}

func (r *Repository) CountGeneratedChargesByBill(ctx context.Context, tx *sql.Tx, billID int64) (int64, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `SELECT COUNT(*) FROM overtime_generated_charges WHERE overtime_bill_id = ?`, billID).Scan(&count); err != nil {
		return 0, fmt.Errorf("count overtime generated charges: %w", err)
	}
	return count, nil
}

func (r *Repository) InsertGeneratedCharge(ctx context.Context, tx *sql.Tx, charge *GeneratedCharge) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO overtime_generated_charges (
			billing_run_id, overtime_bill_id, overtime_formula_id, lease_contract_id, workflow_instance_id,
			charge_type, formula_type, rate_type, period_start, period_end, quantity,
			total_area, unit_price, base_amount, fixed_rental, percentage_option, minimum_option,
			applied_percentage_rate, applied_minimum_amount, unit_amount, amount, currency_type_id,
			generated_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, charge.BillingRunID, charge.OvertimeBillID, charge.OvertimeFormulaID, charge.LeaseContractID, sqlutil.Int64PointerValue(charge.WorkflowInstanceID), charge.ChargeType, charge.FormulaType, charge.RateType, charge.PeriodStart, charge.PeriodEnd, charge.Quantity, charge.TotalArea, charge.UnitPrice, charge.BaseAmount, charge.FixedRental, charge.PercentageOption, charge.MinimumOption, charge.AppliedPercentageRate, charge.AppliedMinimumAmount, charge.UnitAmount, charge.Amount, charge.CurrencyTypeID, charge.GeneratedBy)
	if err != nil {
		return fmt.Errorf("insert overtime generated charge: %w", err)
	}
	chargeID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get overtime generated charge id: %w", err)
	}
	charge.ID = chargeID
	return nil
}

const billSelect = `
	SELECT ob.id, ob.lease_contract_id, lc.lease_no, lc.tenant_name, ob.period_start, ob.period_end,
		ob.status, ob.workflow_instance_id, ob.note, ob.submitted_at, ob.approved_at,
		ob.rejected_at, ob.cancelled_at, ob.stopped_at, ob.generated_at,
		ob.created_by, ob.updated_by, ob.created_at, ob.updated_at
	FROM overtime_bills ob
	INNER JOIN lease_contracts lc ON lc.id = ob.lease_contract_id
`
