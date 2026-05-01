package lease

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

type ExpirationReminderCandidate struct {
	LeaseID        int64
	ContractNumber string
	TenantName     string
	ExpirationDate time.Time
	DaysRemaining  int
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db}
}

func (r *Repository) Create(ctx context.Context, tx *sql.Tx, contract *Contract) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO lease_contracts (
			amended_from_id, lease_no, subtype, department_id, store_id, building_id, customer_id, brand_id, trade_id, management_type_id, tenant_name,
			start_date, end_date, status, effective_version, created_by, updated_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, sqlutil.Int64PointerValue(contract.AmendedFromID), contract.LeaseNo, normalizeSubtype(contract.Subtype), contract.DepartmentID, contract.StoreID, sqlutil.Int64PointerValue(contract.BuildingID), sqlutil.Int64PointerValue(contract.CustomerID), sqlutil.Int64PointerValue(contract.BrandID), sqlutil.Int64PointerValue(contract.TradeID), sqlutil.Int64PointerValue(contract.ManagementTypeID), contract.TenantName, contract.StartDate, contract.EndDate, contract.Status, contract.EffectiveVersion, contract.CreatedBy, contract.UpdatedBy)
	if err != nil {
		return fmt.Errorf("insert lease contract: %w", err)
	}
	contractID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get lease contract id: %w", err)
	}
	contract.ID = contractID

	for _, unit := range contract.Units {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO lease_contract_units (lease_contract_id, unit_id, rent_area)
			VALUES (?, ?, ?)
		`, contractID, unit.UnitID, unit.RentArea); err != nil {
			return fmt.Errorf("insert lease contract unit: %w", err)
		}
	}

	for _, term := range contract.Terms {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO lease_contract_terms (
				lease_contract_id, term_type, billing_cycle, currency_type_id, amount, effective_from, effective_to
			) VALUES (?, ?, ?, ?, ?, ?, ?)
		`, contractID, term.TermType, term.BillingCycle, term.CurrencyTypeID, term.Amount, term.EffectiveFrom, term.EffectiveTo); err != nil {
			return fmt.Errorf("insert lease contract term: %w", err)
		}
	}

	if err := r.saveSubtypeDetails(ctx, tx, contractID, contract); err != nil {
		return err
	}

	return nil
}

func (r *Repository) FindByID(ctx context.Context, id int64) (*Contract, error) {
	contract, err := r.findContract(ctx, r.db.QueryRowContext(ctx, leaseContractSelect+` WHERE lc.id = ?`, id))
	if err != nil {
		return nil, err
	}
	if contract == nil {
		return nil, nil
	}
	if err := r.loadChildren(ctx, contract); err != nil {
		return nil, err
	}
	return contract, nil
}

func (r *Repository) FindByIDForUpdate(ctx context.Context, tx *sql.Tx, id int64) (*Contract, error) {
	contract, err := r.findContract(ctx, tx.QueryRowContext(ctx, leaseContractSelect+` WHERE lc.id = ? FOR UPDATE`, id))
	if err != nil {
		return nil, err
	}
	if contract == nil {
		return nil, nil
	}
	if err := r.loadChildrenTx(ctx, tx, contract); err != nil {
		return nil, err
	}
	return contract, nil
}

func (r *Repository) List(ctx context.Context, filter ListFilter) (*pagination.ListResult[Summary], error) {
	page, pageSize := pagination.NormalizePage(filter.Page, filter.PageSize)
	conditions := []string{"1=1"}
	args := make([]any, 0)

	if filter.Status != nil {
		conditions = append(conditions, "lc.status = ?")
		args = append(args, *filter.Status)
	}
	if filter.StoreID != nil {
		conditions = append(conditions, "lc.store_id = ?")
		args = append(args, *filter.StoreID)
	}
	if filter.Subtype != nil {
		conditions = append(conditions, "lc.subtype = ?")
		args = append(args, *filter.Subtype)
	}
	if filter.DepartmentID != nil {
		conditions = append(conditions, "lc.department_id = ?")
		args = append(args, *filter.DepartmentID)
	}
	if strings.TrimSpace(filter.LeaseNo) != "" {
		conditions = append(conditions, "lc.lease_no LIKE ?")
		args = append(args, "%"+strings.TrimSpace(filter.LeaseNo)+"%")
	}

	whereClause := strings.Join(conditions, " AND ")
	var total int64
	if err := r.db.QueryRowContext(ctx, `SELECT COUNT(*) FROM lease_contracts lc WHERE `+whereClause, args...).Scan(&total); err != nil {
		return nil, fmt.Errorf("count lease contracts: %w", err)
	}

	queryArgs := append(append([]any{}, args...), pageSize, (page-1)*pageSize)
	rows, err := r.db.QueryContext(ctx, `
		SELECT lc.id, lc.lease_no, lc.subtype, lc.tenant_name, lc.department_id, lc.store_id, lc.building_id,
			lc.customer_id, lc.brand_id, lc.trade_id, lc.management_type_id,
			lc.start_date, lc.end_date, lc.status, lc.workflow_instance_id, lc.billing_effective_at, lc.updated_at
		FROM lease_contracts lc
		WHERE `+whereClause+`
		ORDER BY lc.id DESC
		LIMIT ? OFFSET ?
	`, queryArgs...)
	if err != nil {
		return nil, fmt.Errorf("list lease contracts: %w", err)
	}
	defer rows.Close()

	items := make([]Summary, 0)
	for rows.Next() {
		var summary Summary
		var buildingID sql.NullInt64
		var customerID sql.NullInt64
		var brandID sql.NullInt64
		var tradeID sql.NullInt64
		var managementTypeID sql.NullInt64
		var workflowInstanceID sql.NullInt64
		var billingEffectiveAt sql.NullTime
		if err := rows.Scan(&summary.ID, &summary.LeaseNo, &summary.Subtype, &summary.TenantName, &summary.DepartmentID, &summary.StoreID, &buildingID, &customerID, &brandID, &tradeID, &managementTypeID, &summary.StartDate, &summary.EndDate, &summary.Status, &workflowInstanceID, &billingEffectiveAt, &summary.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan lease contract summary: %w", err)
		}
		summary.BuildingID = sqlutil.NullInt64Pointer(buildingID)
		summary.CustomerID = sqlutil.NullInt64Pointer(customerID)
		summary.BrandID = sqlutil.NullInt64Pointer(brandID)
		summary.TradeID = sqlutil.NullInt64Pointer(tradeID)
		summary.ManagementTypeID = sqlutil.NullInt64Pointer(managementTypeID)
		summary.WorkflowInstanceID = sqlutil.NullInt64Pointer(workflowInstanceID)
		summary.BillingEffectiveAt = sqlutil.NullTimePointer(billingEffectiveAt)
		items = append(items, summary)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate lease contract summaries: %w", err)
	}

	return &pagination.ListResult[Summary]{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func (r *Repository) AttachWorkflowInstance(ctx context.Context, tx *sql.Tx, leaseID, workflowInstanceID, updatedBy int64, submittedAt time.Time) error {
	result, err := tx.ExecContext(ctx, `
		UPDATE lease_contracts
		SET status = ?, workflow_instance_id = ?, submitted_at = ?, updated_by = ?
		WHERE id = ? AND status = ? AND workflow_instance_id IS NULL
	`, StatusPendingApproval, workflowInstanceID, submittedAt, updatedBy, leaseID, StatusDraft)
	if err != nil {
		return fmt.Errorf("attach workflow instance to lease contract: %w", err)
	}
	affectedRows, err := result.RowsAffected()
	if err != nil {
		return fmt.Errorf("determine attached workflow rows: %w", err)
	}
	if affectedRows != 1 {
		return ErrLeaseAlreadySubmitted
	}
	return nil
}

func (r *Repository) UpdateWorkflowState(ctx context.Context, tx *sql.Tx, leaseID, workflowInstanceID, updatedBy int64, status Status, approvedAt, billingEffectiveAt, terminatedAt *time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE lease_contracts
		SET workflow_instance_id = ?, status = ?, approved_at = ?, billing_effective_at = ?, terminated_at = ?, updated_by = ?
		WHERE id = ?
	`, workflowInstanceID, status, sqlutil.TimePointerValue(approvedAt), sqlutil.TimePointerValue(billingEffectiveAt), sqlutil.TimePointerValue(terminatedAt), updatedBy, leaseID); err != nil {
		return fmt.Errorf("update lease contract workflow state: %w", err)
	}
	return nil
}

func (r *Repository) Terminate(ctx context.Context, tx *sql.Tx, leaseID, updatedBy int64, terminatedAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE lease_contracts
		SET status = ?, terminated_at = ?, updated_by = ?
		WHERE id = ?
	`, StatusTerminated, terminatedAt, updatedBy, leaseID); err != nil {
		return fmt.Errorf("terminate lease contract: %w", err)
	}
	return nil
}

func (r *Repository) CountBlockingBillingDocuments(ctx context.Context, tx *sql.Tx, leaseID int64) (int64, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `
		SELECT COUNT(*)
		FROM billing_documents
		WHERE lease_contract_id = ?
		  AND status IN ('draft', 'pending_approval', 'approved', 'rejected')
	`, leaseID).Scan(&count); err != nil {
		return 0, fmt.Errorf("count blocking billing documents for lease: %w", err)
	}
	return count, nil
}

type rowScanner interface {
	Scan(dest ...any) error
}

const leaseContractSelect = `
	SELECT lc.id, lc.amended_from_id, lc.lease_no, lc.subtype, lc.department_id, lc.store_id, lc.building_id,
		lc.customer_id, lc.brand_id, lc.trade_id, lc.management_type_id,
		lc.tenant_name, lc.start_date, lc.end_date, lc.status, lc.workflow_instance_id,
		lc.effective_version, lc.submitted_at, lc.approved_at, lc.billing_effective_at,
		lc.terminated_at, lc.created_by, lc.updated_by, lc.created_at, lc.updated_at
	FROM lease_contracts lc
`

func (r *Repository) findContract(_ context.Context, scanner rowScanner) (*Contract, error) {
	var contract Contract
	var amendedFromID sql.NullInt64
	var buildingID sql.NullInt64
	var customerID sql.NullInt64
	var brandID sql.NullInt64
	var tradeID sql.NullInt64
	var managementTypeID sql.NullInt64
	var workflowInstanceID sql.NullInt64
	var submittedAt sql.NullTime
	var approvedAt sql.NullTime
	var billingEffectiveAt sql.NullTime
	var terminatedAt sql.NullTime
	if err := scanner.Scan(&contract.ID, &amendedFromID, &contract.LeaseNo, &contract.Subtype, &contract.DepartmentID, &contract.StoreID, &buildingID, &customerID, &brandID, &tradeID, &managementTypeID, &contract.TenantName, &contract.StartDate, &contract.EndDate, &contract.Status, &workflowInstanceID, &contract.EffectiveVersion, &submittedAt, &approvedAt, &billingEffectiveAt, &terminatedAt, &contract.CreatedBy, &contract.UpdatedBy, &contract.CreatedAt, &contract.UpdatedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("scan lease contract: %w", err)
	}
	contract.AmendedFromID = sqlutil.NullInt64Pointer(amendedFromID)
	contract.BuildingID = sqlutil.NullInt64Pointer(buildingID)
	contract.CustomerID = sqlutil.NullInt64Pointer(customerID)
	contract.BrandID = sqlutil.NullInt64Pointer(brandID)
	contract.TradeID = sqlutil.NullInt64Pointer(tradeID)
	contract.ManagementTypeID = sqlutil.NullInt64Pointer(managementTypeID)
	contract.WorkflowInstanceID = sqlutil.NullInt64Pointer(workflowInstanceID)
	contract.SubmittedAt = sqlutil.NullTimePointer(submittedAt)
	contract.ApprovedAt = sqlutil.NullTimePointer(approvedAt)
	contract.BillingEffectiveAt = sqlutil.NullTimePointer(billingEffectiveAt)
	contract.TerminatedAt = sqlutil.NullTimePointer(terminatedAt)
	contract.Subtype = normalizeSubtype(contract.Subtype)
	return &contract, nil
}

func (r *Repository) loadChildren(ctx context.Context, contract *Contract) error {
	if err := r.loadSubtypeChildren(ctx, r.db, contract); err != nil {
		return err
	}
	units, err := r.listUnits(ctx, r.db, contract.ID)
	if err != nil {
		return err
	}
	terms, err := r.listTerms(ctx, r.db, contract.ID)
	if err != nil {
		return err
	}
	contract.Units = units
	contract.Terms = terms
	return nil
}

func (r *Repository) loadChildrenTx(ctx context.Context, tx *sql.Tx, contract *Contract) error {
	if err := r.loadSubtypeChildren(ctx, tx, contract); err != nil {
		return err
	}
	units, err := r.listUnits(ctx, tx, contract.ID)
	if err != nil {
		return err
	}
	terms, err := r.listTerms(ctx, tx, contract.ID)
	if err != nil {
		return err
	}
	contract.Units = units
	contract.Terms = terms
	return nil
}

type queryer interface {
	QueryContext(ctx context.Context, query string, args ...any) (*sql.Rows, error)
}

type execer interface {
	ExecContext(ctx context.Context, query string, args ...any) (sql.Result, error)
}

func (r *Repository) listUnits(ctx context.Context, db queryer, leaseID int64) ([]Unit, error) {
	rows, err := db.QueryContext(ctx, `
		SELECT id, lease_contract_id, unit_id, rent_area, created_at, updated_at
		FROM lease_contract_units
		WHERE lease_contract_id = ?
		ORDER BY id
	`, leaseID)
	if err != nil {
		return nil, fmt.Errorf("query lease contract units: %w", err)
	}
	defer rows.Close()

	units := make([]Unit, 0)
	for rows.Next() {
		var unit Unit
		if err := rows.Scan(&unit.ID, &unit.LeaseContractID, &unit.UnitID, &unit.RentArea, &unit.CreatedAt, &unit.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan lease contract unit: %w", err)
		}
		units = append(units, unit)
	}
	return units, rows.Err()
}

func (r *Repository) listTerms(ctx context.Context, db queryer, leaseID int64) ([]Term, error) {
	rows, err := db.QueryContext(ctx, `
		SELECT id, lease_contract_id, term_type, billing_cycle, currency_type_id, amount, effective_from, effective_to, created_at, updated_at
		FROM lease_contract_terms
		WHERE lease_contract_id = ?
		ORDER BY id
	`, leaseID)
	if err != nil {
		return nil, fmt.Errorf("query lease contract terms: %w", err)
	}
	defer rows.Close()

	terms := make([]Term, 0)
	for rows.Next() {
		var term Term
		if err := rows.Scan(&term.ID, &term.LeaseContractID, &term.TermType, &term.BillingCycle, &term.CurrencyTypeID, &term.Amount, &term.EffectiveFrom, &term.EffectiveTo, &term.CreatedAt, &term.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan lease contract term: %w", err)
		}
		terms = append(terms, term)
	}
	return terms, rows.Err()
}

func (r *Repository) saveSubtypeDetails(ctx context.Context, tx execer, leaseID int64, contract *Contract) error {
	if contract == nil {
		return nil
	}
	if contract.JointOperation != nil {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO lease_contract_joint_operation_details (
				lease_contract_id, bill_cycle, rent_inc, account_cycle, tax_rate, tax_type,
				settlement_currency_type_id, in_tax_rate, out_tax_rate, month_settle_days,
				late_pay_interest_rate, interest_grace_days
			) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
		`, leaseID, contract.JointOperation.BillCycle, contract.JointOperation.RentInc, contract.JointOperation.AccountCycle, contract.JointOperation.TaxRate, contract.JointOperation.TaxType, contract.JointOperation.SettlementCurrencyTypeID, contract.JointOperation.InTaxRate, contract.JointOperation.OutTaxRate, contract.JointOperation.MonthSettleDays, contract.JointOperation.LatePayInterestRate, contract.JointOperation.InterestGraceDays); err != nil {
			return fmt.Errorf("insert lease joint operation detail: %w", err)
		}
	}
	for _, detail := range contract.AdBoards {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO lease_contract_ad_board_details (
				lease_contract_id, ad_board_id, description, status, start_date, end_date,
				rent_area, airtime, frequency, frequency_days, frequency_mon, frequency_tue,
				frequency_wed, frequency_thu, frequency_fri, frequency_sat, frequency_sun,
				between_from, between_to, store_id, building_id
			) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
		`, leaseID, detail.AdBoardID, detail.Description, detail.Status, detail.StartDate, detail.EndDate, detail.RentArea, detail.Airtime, detail.Frequency, detail.FrequencyDays, detail.FrequencyMon, detail.FrequencyTue, detail.FrequencyWed, detail.FrequencyThu, detail.FrequencyFri, detail.FrequencySat, detail.FrequencySun, detail.BetweenFrom, detail.BetweenTo, sqlutil.Int64PointerValue(detail.StoreID), sqlutil.Int64PointerValue(detail.BuildingID)); err != nil {
			return fmt.Errorf("insert lease ad board detail: %w", err)
		}
	}
	for _, detail := range contract.AreaGrounds {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO lease_contract_area_ground_details (
				lease_contract_id, code, name, type_id, description, status, start_date, end_date, rent_area
			) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
		`, leaseID, detail.Code, detail.Name, detail.TypeID, detail.Description, detail.Status, detail.StartDate, detail.EndDate, detail.RentArea); err != nil {
			return fmt.Errorf("insert lease area/ground detail: %w", err)
		}
	}
	return nil
}

func (r *Repository) loadSubtypeChildren(ctx context.Context, db queryer, contract *Contract) error {
	jointOperation, err := r.getJointOperationDetail(ctx, db, contract.ID)
	if err != nil {
		return err
	}
	adBoards, err := r.listAdBoardDetails(ctx, db, contract.ID)
	if err != nil {
		return err
	}
	areaGrounds, err := r.listAreaGroundDetails(ctx, db, contract.ID)
	if err != nil {
		return err
	}
	contract.JointOperation = jointOperation
	contract.AdBoards = adBoards
	contract.AreaGrounds = areaGrounds
	return nil
}

func (r *Repository) getJointOperationDetail(ctx context.Context, db queryer, leaseID int64) (*JointOperationFields, error) {
	rows, err := db.QueryContext(ctx, `
		SELECT lease_contract_id, bill_cycle, rent_inc, account_cycle, tax_rate, tax_type,
			settlement_currency_type_id, in_tax_rate, out_tax_rate, month_settle_days,
			late_pay_interest_rate, interest_grace_days, created_at, updated_at
		FROM lease_contract_joint_operation_details
		WHERE lease_contract_id = ?
	`, leaseID)
	if err != nil {
		return nil, fmt.Errorf("query lease joint operation detail: %w", err)
	}
	defer rows.Close()
	if !rows.Next() {
		return nil, rows.Err()
	}
	var detail JointOperationFields
	if err := rows.Scan(&detail.LeaseContractID, &detail.BillCycle, &detail.RentInc, &detail.AccountCycle, &detail.TaxRate, &detail.TaxType, &detail.SettlementCurrencyTypeID, &detail.InTaxRate, &detail.OutTaxRate, &detail.MonthSettleDays, &detail.LatePayInterestRate, &detail.InterestGraceDays, &detail.CreatedAt, &detail.UpdatedAt); err != nil {
		return nil, fmt.Errorf("scan lease joint operation detail: %w", err)
	}
	return &detail, rows.Err()
}

func (r *Repository) listAdBoardDetails(ctx context.Context, db queryer, leaseID int64) ([]AdBoardDetail, error) {
	rows, err := db.QueryContext(ctx, `
		SELECT id, lease_contract_id, ad_board_id, description, status, start_date, end_date,
			rent_area, airtime, frequency, frequency_days, frequency_mon, frequency_tue,
			frequency_wed, frequency_thu, frequency_fri, frequency_sat, frequency_sun,
			between_from, between_to, store_id, building_id, created_at, updated_at
		FROM lease_contract_ad_board_details
		WHERE lease_contract_id = ?
		ORDER BY id
	`, leaseID)
	if err != nil {
		return nil, fmt.Errorf("query lease ad board details: %w", err)
	}
	defer rows.Close()
	items := make([]AdBoardDetail, 0)
	for rows.Next() {
		var detail AdBoardDetail
		var storeID sql.NullInt64
		var buildingID sql.NullInt64
		if err := rows.Scan(&detail.ID, &detail.LeaseContractID, &detail.AdBoardID, &detail.Description, &detail.Status, &detail.StartDate, &detail.EndDate, &detail.RentArea, &detail.Airtime, &detail.Frequency, &detail.FrequencyDays, &detail.FrequencyMon, &detail.FrequencyTue, &detail.FrequencyWed, &detail.FrequencyThu, &detail.FrequencyFri, &detail.FrequencySat, &detail.FrequencySun, &detail.BetweenFrom, &detail.BetweenTo, &storeID, &buildingID, &detail.CreatedAt, &detail.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan lease ad board detail: %w", err)
		}
		detail.StoreID = sqlutil.NullInt64Pointer(storeID)
		detail.BuildingID = sqlutil.NullInt64Pointer(buildingID)
		items = append(items, detail)
	}
	return items, rows.Err()
}

func (r *Repository) listAreaGroundDetails(ctx context.Context, db queryer, leaseID int64) ([]AreaGroundDetail, error) {
	rows, err := db.QueryContext(ctx, `
		SELECT id, lease_contract_id, code, name, type_id, description, status, start_date, end_date, rent_area, created_at, updated_at
		FROM lease_contract_area_ground_details
		WHERE lease_contract_id = ?
		ORDER BY id
	`, leaseID)
	if err != nil {
		return nil, fmt.Errorf("query lease area/ground details: %w", err)
	}
	defer rows.Close()
	items := make([]AreaGroundDetail, 0)
	for rows.Next() {
		var detail AreaGroundDetail
		if err := rows.Scan(&detail.ID, &detail.LeaseContractID, &detail.Code, &detail.Name, &detail.TypeID, &detail.Description, &detail.Status, &detail.StartDate, &detail.EndDate, &detail.RentArea, &detail.CreatedAt, &detail.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan lease area/ground detail: %w", err)
		}
		items = append(items, detail)
	}
	return items, rows.Err()
}

func (r *Repository) ListExpirationReminderCandidates(ctx context.Context, asOf time.Time, thresholdDays int) ([]ExpirationReminderCandidate, error) {
	endDate := asOf.UTC().AddDate(0, 0, thresholdDays)
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, lease_no, tenant_name, end_date, DATEDIFF(end_date, ?) AS days_remaining
		FROM lease_contracts
		WHERE status = ?
		  AND end_date >= ?
		  AND end_date <= ?
		ORDER BY end_date ASC, id ASC
	`, asOf.UTC(), StatusActive, asOf.UTC(), endDate)
	if err != nil {
		return nil, fmt.Errorf("query lease expiration reminder candidates: %w", err)
	}
	defer rows.Close()

	items := make([]ExpirationReminderCandidate, 0)
	for rows.Next() {
		var item ExpirationReminderCandidate
		if err := rows.Scan(&item.LeaseID, &item.ContractNumber, &item.TenantName, &item.ExpirationDate, &item.DaysRemaining); err != nil {
			return nil, fmt.Errorf("scan lease expiration reminder candidate: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate lease expiration reminder candidates: %w", err)
	}
	return items, nil
}

func (r *Repository) HasReminderQueuedSince(ctx context.Context, tx *sql.Tx, eventType, aggregateType string, aggregateID int64, since time.Time) (bool, error) {
	if tx == nil {
		return false, fmt.Errorf("query queued lease reminder: nil transaction")
	}
	var count int
	if err := tx.QueryRowContext(ctx, `
		SELECT COUNT(*)
		FROM notification_outbox
		WHERE event_type = ?
		  AND aggregate_type = ?
		  AND aggregate_id = ?
		  AND created_at >= ?
	`, eventType, aggregateType, aggregateID, since.UTC()).Scan(&count); err != nil {
		return false, fmt.Errorf("query queued lease reminder: %w", err)
	}
	return count > 0, nil
}

func (r *Repository) ListOperationsRecipientEmails(ctx context.Context, tx *sql.Tx) ([]string, error) {
	if tx == nil {
		return nil, fmt.Errorf("query operations recipient emails: nil transaction")
	}
	rows, err := tx.QueryContext(ctx, `
		SELECT DISTINCT u.username
		FROM users u
		INNER JOIN user_roles ur ON ur.user_id = u.id
		INNER JOIN role_permissions rp ON rp.role_id = ur.role_id
		INNER JOIN functions f ON f.id = rp.function_id
		WHERE u.status = 'active'
		  AND f.code = 'lease.contract'
		ORDER BY u.id
	`)
	if err != nil {
		return nil, fmt.Errorf("query operations recipient emails: %w", err)
	}
	defer rows.Close()

	recipients := make([]string, 0)
	for rows.Next() {
		var username string
		if err := rows.Scan(&username); err != nil {
			return nil, fmt.Errorf("scan operations recipient email: %w", err)
		}
		recipients = append(recipients, username)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate operations recipient emails: %w", err)
	}
	return recipients, nil
}
