package invoice

import (
	"context"
	"database/sql"
	"fmt"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

type receivableLeaseContext struct {
	LeaseContractID int64
	CustomerID      int64
	DepartmentID    int64
	TradeID         *int64
}

type receivableOpenItemRow struct {
	BillingDocumentLineID int64
	ChargeType            string
	ChargeSource          billing.ChargeSource
	OvertimeBillID        *int64
	OvertimeFormulaID     *int64
	OvertimeChargeID      *int64
	DueDate               time.Time
	OutstandingAmount     float64
	IsDeposit             bool
}

func (r *Repository) FindReceivableLeaseContext(ctx context.Context, leaseContractID int64) (*receivableLeaseContext, error) {
	return r.findReceivableLeaseContext(ctx, r.db.QueryRowContext(ctx, `
		SELECT id, customer_id, department_id, trade_id
		FROM lease_contracts
		WHERE id = ?
	`, leaseContractID))
}

func (r *Repository) findReceivableLeaseContext(ctx context.Context, scanner rowScanner) (*receivableLeaseContext, error) {
	_ = ctx
	var result receivableLeaseContext
	var customerID sql.NullInt64
	var tradeID sql.NullInt64
	if err := scanner.Scan(&result.LeaseContractID, &customerID, &result.DepartmentID, &tradeID); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("load receivable lease context: %w", err)
	}
	if !customerID.Valid || customerID.Int64 == 0 {
		return nil, nil
	}
	result.CustomerID = customerID.Int64
	result.TradeID = sqlutil.NullInt64Pointer(tradeID)
	return &result, nil
}

func (r *Repository) UpsertReceivableOpenItem(ctx context.Context, tx *sql.Tx, leaseCtx *receivableLeaseContext, documentID int64, row receivableOpenItemRow) error {
	if leaseCtx == nil {
		return ErrReceivableContextInvalid
	}
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO ar_open_items (
			lease_contract_id, billing_document_id, billing_document_line_id, customer_id,
			department_id, trade_id, charge_type, charge_source, overtime_bill_id, overtime_formula_id, overtime_charge_id,
			due_date, outstanding_amount, settled_at, is_deposit
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
		ON DUPLICATE KEY UPDATE
			billing_document_line_id = billing_document_line_id
	`, leaseCtx.LeaseContractID, documentID, row.BillingDocumentLineID, leaseCtx.CustomerID, leaseCtx.DepartmentID, sqlutil.Int64PointerValue(leaseCtx.TradeID), row.ChargeType, row.ChargeSource, sqlutil.Int64PointerValue(row.OvertimeBillID), sqlutil.Int64PointerValue(row.OvertimeFormulaID), sqlutil.Int64PointerValue(row.OvertimeChargeID), row.DueDate, row.OutstandingAmount, nil, row.IsDeposit); err != nil {
		return fmt.Errorf("upsert receivable open item: %w", err)
	}
	return nil
}

func (r *Repository) FindOpenItemsByDocumentIDForUpdate(ctx context.Context, tx *sql.Tx, documentID int64) ([]OpenItem, error) {
	rows, err := tx.QueryContext(ctx, `
		SELECT id, lease_contract_id, billing_document_id, billing_document_line_id, customer_id, department_id, trade_id,
			charge_type, charge_source, overtime_bill_id, overtime_formula_id, overtime_charge_id, due_date, outstanding_amount, settled_at, is_deposit, created_at, updated_at
		FROM ar_open_items
		WHERE billing_document_id = ?
		ORDER BY due_date, charge_type, id
		FOR UPDATE
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query receivable open items for update: %w", err)
	}
	defer rows.Close()
	return scanOpenItems(rows)
}

func (r *Repository) ZeroOpenItemsForDocument(ctx context.Context, tx *sql.Tx, documentID int64, settledAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE ar_open_items
		SET outstanding_amount = 0, settled_at = ?
		WHERE billing_document_id = ? AND outstanding_amount <> 0
	`, settledAt, documentID); err != nil {
		return fmt.Errorf("zero receivable open items for document: %w", err)
	}
	return nil
}

func (r *Repository) CountPaymentEntries(ctx context.Context, tx *sql.Tx, documentID int64) (int64, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `SELECT COUNT(*) FROM ar_payment_entries WHERE billing_document_id = ?`, documentID).Scan(&count); err != nil {
		return 0, fmt.Errorf("count payment entries: %w", err)
	}
	return count, nil
}

func (r *Repository) FindPaymentEntryByIdempotency(ctx context.Context, tx *sql.Tx, documentID int64, idempotencyKey string) (*PaymentEntry, error) {
	entry, err := r.scanPaymentEntry(tx.QueryRowContext(ctx, `
		SELECT id, billing_document_id, lease_contract_id, payment_date, amount, note, recorded_by, idempotency_key, created_at
		FROM ar_payment_entries
		WHERE billing_document_id = ? AND idempotency_key = ?
	`, documentID, idempotencyKey))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find payment entry by idempotency: %w", err)
	}
	return entry, nil
}

func (r *Repository) InsertPaymentEntry(ctx context.Context, tx *sql.Tx, entry PaymentEntry) error {
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO ar_payment_entries (
			billing_document_id, lease_contract_id, payment_date, amount, note, recorded_by, idempotency_key
		) VALUES (?, ?, ?, ?, ?, ?, ?)
	`, entry.BillingDocumentID, entry.LeaseContractID, entry.PaymentDate, entry.Amount, sqlutil.StringPointerValue(entry.Note), entry.RecordedBy, entry.IdempotencyKey); err != nil {
		return fmt.Errorf("insert payment entry: %w", err)
	}
	return nil
}

func (r *Repository) UpdateOpenItemBalance(ctx context.Context, tx *sql.Tx, itemID int64, outstandingAmount float64, settledAt *time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE ar_open_items
		SET outstanding_amount = ?, settled_at = ?
		WHERE id = ?
	`, outstandingAmount, sqlutil.TimePointerValue(settledAt), itemID); err != nil {
		return fmt.Errorf("update receivable open item balance: %w", err)
	}
	return nil
}

func (r *Repository) GetReceivableSummary(ctx context.Context, documentID int64) (*ReceivableSummary, error) {
	document, err := r.FindByID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, nil
	}
	items, err := r.getOpenItemsByDocumentID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	history, err := r.getPaymentHistoryByDocumentID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	discountHistory, err := r.getDiscountHistoryByDocumentID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	interestHistory, err := r.GetInterestHistoryByDocumentID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	depositAppHistory, err := r.getDepositApplicationHistoryByDocumentID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	depositRefundHistory, err := r.getDepositRefundHistoryByDocumentID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	leaseCtx, err := r.FindReceivableLeaseContext(ctx, document.LeaseContractID)
	if err != nil {
		return nil, err
	}
	customerSurplus := 0.0
	surplusHistory := []SurplusEntry{}
	if leaseCtx != nil {
		balance, err := r.FindSurplusBalanceByCustomer(ctx, leaseCtx.CustomerID)
		if err != nil {
			return nil, err
		}
		if balance != nil {
			customerSurplus = balance.AvailableAmount
		}
		history, err := r.GetSurplusHistoryByCustomer(ctx, leaseCtx.CustomerID)
		if err != nil {
			return nil, err
		}
		surplusHistory = history
	}
	total := 0.0
	for _, item := range items {
		total += item.OutstandingAmount
	}
	return &ReceivableSummary{
		BillingDocumentID:         document.ID,
		DocumentNo:                document.DocumentNo,
		DocumentType:              document.DocumentType,
		TenantName:                document.TenantName,
		LeaseContractID:           document.LeaseContractID,
		OutstandingAmount:         total,
		CustomerSurplus:           customerSurplus,
		SettlementStatus:          settlementStatus(total),
		Items:                     items,
		PaymentHistory:            history,
		DiscountHistory:           discountHistory,
		SurplusHistory:            surplusHistory,
		InterestHistory:           interestHistory,
		DepositApplicationHistory: depositAppHistory,
		DepositRefundHistory:      depositRefundHistory,
	}, nil
}

func (r *Repository) ListReceivables(ctx context.Context, filter ReceivableFilter) (*pagination.ListResult[ReceivableListItem], error) {
	page, pageSize := pagination.NormalizePage(filter.Page, filter.PageSize)
	conditions := []string{"ai.outstanding_amount > 0"}
	args := make([]any, 0)
	if filter.CustomerID != nil {
		conditions = append(conditions, "ai.customer_id = ?")
		args = append(args, *filter.CustomerID)
	}
	if filter.DepartmentID != nil {
		conditions = append(conditions, "ai.department_id = ?")
		args = append(args, *filter.DepartmentID)
	}
	if filter.DueDateStart != nil {
		conditions = append(conditions, "ai.due_date >= ?")
		args = append(args, *filter.DueDateStart)
	}
	if filter.DueDateEnd != nil {
		conditions = append(conditions, "ai.due_date <= ?")
		args = append(args, *filter.DueDateEnd)
	}
	whereClause := strings.Join(conditions, " AND ")
	countArgs := append([]any{}, args...)
	var total int64
	if err := r.db.QueryRowContext(ctx, `
		SELECT COUNT(*)
		FROM (
			SELECT ai.billing_document_id
			FROM ar_open_items ai
			WHERE `+whereClause+`
			GROUP BY ai.billing_document_id
		) counted
	`, countArgs...).Scan(&total); err != nil {
		return nil, fmt.Errorf("count receivables: %w", err)
	}
	queryArgs := append(append([]any{}, args...), pageSize, (page-1)*pageSize)
	rows, err := r.db.QueryContext(ctx, `
		SELECT
			bd.id,
			bd.document_type,
			bd.document_no,
			bd.tenant_name,
			bd.status,
			bd.lease_contract_id,
			ai.customer_id,
			ai.department_id,
			ai.trade_id,
			MIN(ai.due_date) AS earliest_due_date,
			MAX(ai.due_date) AS latest_due_date,
			SUM(ai.outstanding_amount) AS outstanding_amount
		FROM ar_open_items ai
		INNER JOIN billing_documents bd ON bd.id = ai.billing_document_id
		WHERE `+whereClause+`
		GROUP BY bd.id, bd.document_type, bd.document_no, bd.tenant_name, bd.status, bd.lease_contract_id, ai.customer_id, ai.department_id, ai.trade_id
		ORDER BY earliest_due_date, bd.id
		LIMIT ? OFFSET ?
	`, queryArgs...)
	if err != nil {
		return nil, fmt.Errorf("list receivables: %w", err)
	}
	defer rows.Close()
	items := make([]ReceivableListItem, 0)
	for rows.Next() {
		var item ReceivableListItem
		var documentNo sql.NullString
		var tradeID sql.NullInt64
		if err := rows.Scan(&item.BillingDocumentID, &item.DocumentType, &documentNo, &item.TenantName, &item.DocumentStatus, &item.LeaseContractID, &item.CustomerID, &item.DepartmentID, &tradeID, &item.EarliestDueDate, &item.LatestDueDate, &item.OutstandingAmount); err != nil {
			return nil, fmt.Errorf("scan receivable list item: %w", err)
		}
		item.DocumentNo = sqlutil.NullStringPointer(documentNo)
		item.TradeID = sqlutil.NullInt64Pointer(tradeID)
		item.SettlementStatus = settlementStatus(item.OutstandingAmount)
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate receivables: %w", err)
	}
	return &pagination.ListResult[ReceivableListItem]{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func (r *Repository) getOpenItemsByDocumentID(ctx context.Context, documentID int64) ([]OpenItem, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, lease_contract_id, billing_document_id, billing_document_line_id, customer_id, department_id, trade_id,
			charge_type, charge_source, overtime_bill_id, overtime_formula_id, overtime_charge_id, due_date, outstanding_amount, settled_at, is_deposit, created_at, updated_at
		FROM ar_open_items
		WHERE billing_document_id = ?
		ORDER BY due_date, charge_type, id
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query receivable open items: %w", err)
	}
	defer rows.Close()
	return scanOpenItems(rows)
}

func scanOpenItems(rows *sql.Rows) ([]OpenItem, error) {
	items := make([]OpenItem, 0)
	for rows.Next() {
		var item OpenItem
		var billingDocumentLineID sql.NullInt64
		var tradeID sql.NullInt64
		var overtimeBillID sql.NullInt64
		var overtimeFormulaID sql.NullInt64
		var overtimeChargeID sql.NullInt64
		var settledAt sql.NullTime
		if err := rows.Scan(&item.ID, &item.LeaseContractID, &item.BillingDocumentID, &billingDocumentLineID, &item.CustomerID, &item.DepartmentID, &tradeID, &item.ChargeType, &item.ChargeSource, &overtimeBillID, &overtimeFormulaID, &overtimeChargeID, &item.DueDate, &item.OutstandingAmount, &settledAt, &item.IsDeposit, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan receivable open item: %w", err)
		}
		item.BillingDocumentLineID = sqlutil.NullInt64Pointer(billingDocumentLineID)
		item.TradeID = sqlutil.NullInt64Pointer(tradeID)
		item.OvertimeBillID = sqlutil.NullInt64Pointer(overtimeBillID)
		item.OvertimeFormulaID = sqlutil.NullInt64Pointer(overtimeFormulaID)
		item.OvertimeChargeID = sqlutil.NullInt64Pointer(overtimeChargeID)
		item.SettledAt = sqlutil.NullTimePointer(settledAt)
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate receivable open items: %w", err)
	}
	return items, nil
}

func (r *Repository) getPaymentHistoryByDocumentID(ctx context.Context, documentID int64) ([]PaymentEntry, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, billing_document_id, lease_contract_id, payment_date, amount, note, recorded_by, idempotency_key, created_at
		FROM ar_payment_entries
		WHERE billing_document_id = ?
		ORDER BY payment_date, id
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query payment history: %w", err)
	}
	defer rows.Close()
	history := make([]PaymentEntry, 0)
	for rows.Next() {
		var note sql.NullString
		var entry PaymentEntry
		if err := rows.Scan(&entry.ID, &entry.BillingDocumentID, &entry.LeaseContractID, &entry.PaymentDate, &entry.Amount, &note, &entry.RecordedBy, &entry.IdempotencyKey, &entry.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan payment history entry: %w", err)
		}
		entry.Note = sqlutil.NullStringPointer(note)
		history = append(history, entry)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate payment history: %w", err)
	}
	return history, nil
}

func (r *Repository) scanPaymentEntry(scanner rowScanner) (*PaymentEntry, error) {
	var entry PaymentEntry
	var note sql.NullString
	if err := scanner.Scan(&entry.ID, &entry.BillingDocumentID, &entry.LeaseContractID, &entry.PaymentDate, &entry.Amount, &note, &entry.RecordedBy, &entry.IdempotencyKey, &entry.CreatedAt); err != nil {
		return nil, err
	}
	entry.Note = sqlutil.NullStringPointer(note)
	return &entry, nil
}

func settlementStatus(outstandingAmount float64) SettlementStatus {
	if outstandingAmount <= 0 {
		return SettlementStatusSettled
	}
	return SettlementStatusOutstanding
}
