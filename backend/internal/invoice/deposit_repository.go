package invoice

import (
	"context"
	"database/sql"
	"fmt"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

func (r *Repository) InsertDepositApplication(ctx context.Context, tx *sql.Tx, app DepositApplication) error {
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO deposit_applications (
			source_billing_document_id, source_billing_document_line_id, source_ar_open_item_id,
			target_billing_document_id, target_billing_document_line_id, target_ar_open_item_id,
			lease_contract_id, amount, note, idempotency_key, applied_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, app.SourceBillingDocumentID, app.SourceBillingDocumentLineID, app.SourceAROpenItemID,
		app.TargetBillingDocumentID, app.TargetBillingDocumentLineID, app.TargetAROpenItemID,
		app.LeaseContractID, app.Amount, sqlutil.StringPointerValue(app.Note), app.IdempotencyKey, app.AppliedBy); err != nil {
		return fmt.Errorf("insert deposit application: %w", err)
	}
	return nil
}

func (r *Repository) FindDepositApplicationByIdempotency(ctx context.Context, tx *sql.Tx, sourceDocumentID int64, idempotencyKey string) (*DepositApplication, error) {
	app, err := r.scanDepositApplication(tx.QueryRowContext(ctx, `
		SELECT id, source_billing_document_id, source_billing_document_line_id, source_ar_open_item_id,
			target_billing_document_id, target_billing_document_line_id, target_ar_open_item_id,
			lease_contract_id, amount, note, idempotency_key, applied_by, created_at
		FROM deposit_applications
		WHERE source_billing_document_id = ? AND idempotency_key = ?
	`, sourceDocumentID, idempotencyKey))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find deposit application by idempotency: %w", err)
	}
	return app, nil
}

func (r *Repository) InsertDepositRefund(ctx context.Context, tx *sql.Tx, refund DepositRefund) error {
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO deposit_refunds (
			billing_document_id, billing_document_line_id, ar_open_item_id,
			lease_contract_id, amount, reason, idempotency_key, refunded_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
	`, refund.BillingDocumentID, refund.BillingDocumentLineID, refund.AROpenItemID,
		refund.LeaseContractID, refund.Amount, refund.Reason, refund.IdempotencyKey, refund.RefundedBy); err != nil {
		return fmt.Errorf("insert deposit refund: %w", err)
	}
	return nil
}

func (r *Repository) FindDepositRefundByIdempotency(ctx context.Context, tx *sql.Tx, billingDocumentID int64, idempotencyKey string) (*DepositRefund, error) {
	refund, err := r.scanDepositRefund(tx.QueryRowContext(ctx, `
		SELECT id, billing_document_id, billing_document_line_id, ar_open_item_id,
			lease_contract_id, amount, reason, idempotency_key, refunded_by, created_at
		FROM deposit_refunds
		WHERE billing_document_id = ? AND idempotency_key = ?
	`, billingDocumentID, idempotencyKey))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find deposit refund by idempotency: %w", err)
	}
	return refund, nil
}

func (r *Repository) CountOutstandingReceivablesForLease(ctx context.Context, tx *sql.Tx, leaseContractID int64) (int64, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `
		SELECT COUNT(*)
		FROM ar_open_items
		WHERE lease_contract_id = ?
		  AND is_deposit = FALSE
		  AND outstanding_amount > 0
	`, leaseContractID).Scan(&count); err != nil {
		return 0, fmt.Errorf("count outstanding receivables for lease: %w", err)
	}
	return count, nil
}

func (r *Repository) CountPendingFinancialWorkflowsForLease(ctx context.Context, tx *sql.Tx, leaseContractID int64) (int64, error) {
	var count int64
	if err := tx.QueryRowContext(ctx, `
		SELECT COUNT(*)
		FROM billing_documents
		WHERE lease_contract_id = ?
		  AND status = ?
	`, leaseContractID, StatusPendingApproval).Scan(&count); err != nil {
		return 0, fmt.Errorf("count pending financial workflows for lease: %w", err)
	}
	return count, nil
}

func (r *Repository) getDepositApplicationHistoryByDocumentID(ctx context.Context, documentID int64) ([]DepositApplication, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, source_billing_document_id, source_billing_document_line_id, source_ar_open_item_id,
			target_billing_document_id, target_billing_document_line_id, target_ar_open_item_id,
			lease_contract_id, amount, note, idempotency_key, applied_by, created_at
		FROM deposit_applications
		WHERE source_billing_document_id = ?
		ORDER BY id DESC
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query deposit application history: %w", err)
	}
	defer rows.Close()
	history := make([]DepositApplication, 0)
	for rows.Next() {
		app, err := r.scanDepositApplication(rows)
		if err != nil {
			return nil, err
		}
		history = append(history, *app)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate deposit application history: %w", err)
	}
	return history, nil
}

func (r *Repository) getDepositRefundHistoryByDocumentID(ctx context.Context, documentID int64) ([]DepositRefund, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, billing_document_id, billing_document_line_id, ar_open_item_id,
			lease_contract_id, amount, reason, idempotency_key, refunded_by, created_at
		FROM deposit_refunds
		WHERE billing_document_id = ?
		ORDER BY id DESC
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query deposit refund history: %w", err)
	}
	defer rows.Close()
	history := make([]DepositRefund, 0)
	for rows.Next() {
		refund, err := r.scanDepositRefund(rows)
		if err != nil {
			return nil, err
		}
		history = append(history, *refund)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate deposit refund history: %w", err)
	}
	return history, nil
}

func (r *Repository) scanDepositApplication(scanner rowScanner) (*DepositApplication, error) {
	var app DepositApplication
	var note sql.NullString
	if err := scanner.Scan(&app.ID, &app.SourceBillingDocumentID, &app.SourceBillingDocumentLineID, &app.SourceAROpenItemID,
		&app.TargetBillingDocumentID, &app.TargetBillingDocumentLineID, &app.TargetAROpenItemID,
		&app.LeaseContractID, &app.Amount, &note, &app.IdempotencyKey, &app.AppliedBy, &app.CreatedAt); err != nil {
		return nil, err
	}
	app.Note = sqlutil.NullStringPointer(note)
	return &app, nil
}

func (r *Repository) scanDepositRefund(scanner rowScanner) (*DepositRefund, error) {
	var refund DepositRefund
	if err := scanner.Scan(&refund.ID, &refund.BillingDocumentID, &refund.BillingDocumentLineID, &refund.AROpenItemID,
		&refund.LeaseContractID, &refund.Amount, &refund.Reason, &refund.IdempotencyKey, &refund.RefundedBy, &refund.CreatedAt); err != nil {
		return nil, err
	}
	return &refund, nil
}
