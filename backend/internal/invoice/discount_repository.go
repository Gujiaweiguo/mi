package invoice

import (
	"context"
	"database/sql"
	"fmt"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

func (r *Repository) CreateDiscount(ctx context.Context, tx *sql.Tx, discount *Discount) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO invoice_discounts (
			billing_document_id, billing_document_line_id, lease_contract_id, charge_type, requested_amount,
			requested_rate, reason, status, workflow_instance_id, idempotency_key, submitted_at,
			approved_at, rejected_at, created_by, updated_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, discount.BillingDocumentID, discount.BillingDocumentLineID, discount.LeaseContractID, discount.ChargeType, discount.RequestedAmount, discount.RequestedRate, discount.Reason, discount.Status, sqlutil.Int64PointerValue(discount.WorkflowInstanceID), discount.IdempotencyKey, sqlutil.TimePointerValue(discount.SubmittedAt), sqlutil.TimePointerValue(discount.ApprovedAt), sqlutil.TimePointerValue(discount.RejectedAt), discount.CreatedBy, discount.UpdatedBy)
	if err != nil {
		return fmt.Errorf("insert invoice discount: %w", err)
	}
	discountID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get invoice discount id: %w", err)
	}
	discount.ID = discountID
	return nil
}

func (r *Repository) FindDiscountByIdempotency(ctx context.Context, tx *sql.Tx, documentID int64, idempotencyKey string) (*Discount, error) {
	discount, err := r.scanDiscount(tx.QueryRowContext(ctx, `
		SELECT id, billing_document_id, billing_document_line_id, lease_contract_id, charge_type, requested_amount,
			requested_rate, reason, status, workflow_instance_id, idempotency_key, submitted_at, approved_at,
			rejected_at, created_by, updated_by, created_at, updated_at
		FROM invoice_discounts
		WHERE billing_document_id = ? AND idempotency_key = ?
	`, documentID, idempotencyKey))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find invoice discount by idempotency: %w", err)
	}
	return discount, nil
}

func (r *Repository) FindDiscountByIDForUpdate(ctx context.Context, tx *sql.Tx, discountID int64) (*Discount, error) {
	discount, err := r.scanDiscount(tx.QueryRowContext(ctx, `
		SELECT id, billing_document_id, billing_document_line_id, lease_contract_id, charge_type, requested_amount,
			requested_rate, reason, status, workflow_instance_id, idempotency_key, submitted_at, approved_at,
			rejected_at, created_by, updated_by, created_at, updated_at
		FROM invoice_discounts
		WHERE id = ?
		FOR UPDATE
	`, discountID))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find invoice discount for update: %w", err)
	}
	return discount, nil
}

func (r *Repository) FindDiscountByID(ctx context.Context, discountID int64) (*Discount, error) {
	discount, err := r.scanDiscount(r.db.QueryRowContext(ctx, `
		SELECT id, billing_document_id, billing_document_line_id, lease_contract_id, charge_type, requested_amount,
			requested_rate, reason, status, workflow_instance_id, idempotency_key, submitted_at, approved_at,
			rejected_at, created_by, updated_by, created_at, updated_at
		FROM invoice_discounts
		WHERE id = ?
	`, discountID))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find invoice discount by id: %w", err)
	}
	return discount, nil
}

func (r *Repository) AttachDiscountWorkflowInstance(ctx context.Context, tx *sql.Tx, discountID, workflowInstanceID, updatedBy int64, submittedAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE invoice_discounts
		SET status = ?, workflow_instance_id = ?, submitted_at = ?, updated_by = ?
		WHERE id = ?
	`, DiscountStatusPendingApproval, workflowInstanceID, submittedAt, updatedBy, discountID); err != nil {
		return fmt.Errorf("attach workflow instance to invoice discount: %w", err)
	}
	return nil
}

func (r *Repository) UpdateDiscountWorkflowState(ctx context.Context, tx *sql.Tx, discountID, workflowInstanceID, updatedBy int64, status DiscountStatus, approvedAt, rejectedAt *time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE invoice_discounts
		SET workflow_instance_id = ?, status = ?, approved_at = ?, rejected_at = ?, updated_by = ?
		WHERE id = ?
	`, workflowInstanceID, status, sqlutil.TimePointerValue(approvedAt), sqlutil.TimePointerValue(rejectedAt), updatedBy, discountID); err != nil {
		return fmt.Errorf("update invoice discount workflow state: %w", err)
	}
	return nil
}

func (r *Repository) CountPendingDiscountsByLine(ctx context.Context, tx *sql.Tx, billingDocumentLineID int64) (int64, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `
		SELECT COUNT(*)
		FROM invoice_discounts
		WHERE billing_document_line_id = ? AND status = ?
	`, billingDocumentLineID, DiscountStatusPendingApproval).Scan(&count); err != nil {
		return 0, fmt.Errorf("count pending invoice discounts by line: %w", err)
	}
	return count, nil
}

func (r *Repository) InsertDiscountEntry(ctx context.Context, tx *sql.Tx, entry DiscountEntry) error {
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO ar_discount_entries (
			invoice_discount_id, billing_document_id, billing_document_line_id, ar_open_item_id,
			lease_contract_id, amount, recorded_by
		) VALUES (?, ?, ?, ?, ?, ?, ?)
	`, entry.InvoiceDiscountID, entry.BillingDocumentID, entry.BillingDocumentLineID, entry.AROpenItemID, entry.LeaseContractID, entry.Amount, entry.RecordedBy); err != nil {
		return fmt.Errorf("insert discount entry: %w", err)
	}
	return nil
}

func (r *Repository) HasDiscountEntry(ctx context.Context, tx *sql.Tx, discountID int64) (bool, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `SELECT COUNT(*) FROM ar_discount_entries WHERE invoice_discount_id = ?`, discountID).Scan(&count); err != nil {
		return false, fmt.Errorf("count discount entries: %w", err)
	}
	return count > 0, nil
}

func (r *Repository) FindOpenItemByDocumentLineIDForUpdate(ctx context.Context, tx *sql.Tx, documentID, billingDocumentLineID int64) (*OpenItem, error) {
	items, err := scanOpenItemsRow(tx.QueryContext(ctx, `
		SELECT id, lease_contract_id, billing_document_id, billing_document_line_id, customer_id, department_id, trade_id,
			charge_type, charge_source, overtime_bill_id, overtime_formula_id, overtime_charge_id, due_date, outstanding_amount, settled_at, is_deposit, created_at, updated_at
		FROM ar_open_items
		WHERE billing_document_id = ? AND billing_document_line_id = ?
		FOR UPDATE
	`, documentID, billingDocumentLineID))
	if err != nil {
		return nil, err
	}
	if len(items) == 0 {
		return nil, nil
	}
	return &items[0], nil
}

func (r *Repository) getDiscountHistoryByDocumentID(ctx context.Context, documentID int64) ([]Discount, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, billing_document_id, billing_document_line_id, lease_contract_id, charge_type, requested_amount,
			requested_rate, reason, status, workflow_instance_id, idempotency_key, submitted_at, approved_at,
			rejected_at, created_by, updated_by, created_at, updated_at
		FROM invoice_discounts
		WHERE billing_document_id = ?
		ORDER BY id DESC
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query invoice discount history: %w", err)
	}
	defer rows.Close()
	history := make([]Discount, 0)
	for rows.Next() {
		discount, err := r.scanDiscount(rows)
		if err != nil {
			return nil, err
		}
		history = append(history, *discount)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate invoice discount history: %w", err)
	}
	return history, nil
}

func (r *Repository) scanDiscount(scanner rowScanner) (*Discount, error) {
	var discount Discount
	var workflowInstanceID sql.NullInt64
	var submittedAt sql.NullTime
	var approvedAt sql.NullTime
	var rejectedAt sql.NullTime
	if err := scanner.Scan(&discount.ID, &discount.BillingDocumentID, &discount.BillingDocumentLineID, &discount.LeaseContractID, &discount.ChargeType, &discount.RequestedAmount, &discount.RequestedRate, &discount.Reason, &discount.Status, &workflowInstanceID, &discount.IdempotencyKey, &submittedAt, &approvedAt, &rejectedAt, &discount.CreatedBy, &discount.UpdatedBy, &discount.CreatedAt, &discount.UpdatedAt); err != nil {
		return nil, err
	}
	discount.WorkflowInstanceID = sqlutil.NullInt64Pointer(workflowInstanceID)
	discount.SubmittedAt = sqlutil.NullTimePointer(submittedAt)
	discount.ApprovedAt = sqlutil.NullTimePointer(approvedAt)
	discount.RejectedAt = sqlutil.NullTimePointer(rejectedAt)
	return &discount, nil
}

func scanOpenItemsRow(rows *sql.Rows, err error) ([]OpenItem, error) {
	if err != nil {
		return nil, fmt.Errorf("query receivable open items: %w", err)
	}
	defer rows.Close()
	return scanOpenItems(rows)
}
