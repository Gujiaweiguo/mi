package invoice

import (
	"context"
	"database/sql"
	"fmt"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

func (r *Repository) FindSurplusBalanceByCustomer(ctx context.Context, customerID int64) (*SurplusBalance, error) {
	balance, err := r.scanSurplusBalance(r.db.QueryRowContext(ctx, `
		SELECT id, customer_id, available_amount, last_applied_at, created_by, updated_by, created_at, updated_at
		FROM customer_surplus_balances
		WHERE customer_id = ?
	`, customerID))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find surplus balance by customer: %w", err)
	}
	return balance, nil
}

func (r *Repository) FindSurplusBalanceByCustomerForUpdate(ctx context.Context, tx *sql.Tx, customerID int64) (*SurplusBalance, error) {
	balance, err := r.scanSurplusBalance(tx.QueryRowContext(ctx, `
		SELECT id, customer_id, available_amount, last_applied_at, created_by, updated_by, created_at, updated_at
		FROM customer_surplus_balances
		WHERE customer_id = ?
		FOR UPDATE
	`, customerID))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find surplus balance for update: %w", err)
	}
	return balance, nil
}

func (r *Repository) UpsertSurplusBalance(ctx context.Context, tx *sql.Tx, customerID, actorUserID int64, delta float64, appliedAt *time.Time) (*SurplusBalance, error) {
	balance, err := r.FindSurplusBalanceByCustomerForUpdate(ctx, tx, customerID)
	if err != nil {
		return nil, err
	}
	if balance == nil {
		result, err := tx.ExecContext(ctx, `
			INSERT INTO customer_surplus_balances (customer_id, available_amount, last_applied_at, created_by, updated_by)
			VALUES (?, ?, ?, ?, ?)
		`, customerID, roundMoney(delta), sqlutil.TimePointerValue(appliedAt), actorUserID, actorUserID)
		if err != nil {
			return nil, fmt.Errorf("insert surplus balance: %w", err)
		}
		id, err := result.LastInsertId()
		if err != nil {
			return nil, fmt.Errorf("get surplus balance id: %w", err)
		}
		return &SurplusBalance{ID: id, CustomerID: customerID, AvailableAmount: roundMoney(delta), LastAppliedAt: appliedAt, CreatedBy: actorUserID, UpdatedBy: actorUserID}, nil
	}
	newAmount := roundMoney(balance.AvailableAmount + delta)
	if _, err := tx.ExecContext(ctx, `
		UPDATE customer_surplus_balances
		SET available_amount = ?, last_applied_at = ?, updated_by = ?
		WHERE id = ?
	`, newAmount, sqlutil.TimePointerValue(appliedAt), actorUserID, balance.ID); err != nil {
		return nil, fmt.Errorf("update surplus balance: %w", err)
	}
	balance.AvailableAmount = newAmount
	balance.LastAppliedAt = appliedAt
	balance.UpdatedBy = actorUserID
	return balance, nil
}

func (r *Repository) InsertSurplusEntry(ctx context.Context, tx *sql.Tx, entry SurplusEntry) error {
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO ar_surplus_entries (
			surplus_balance_id, entry_type, customer_id, billing_document_id, ar_open_item_id,
			amount, note, idempotency_key, recorded_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, entry.SurplusBalanceID, entry.EntryType, entry.CustomerID, sqlutil.Int64PointerValue(entry.BillingDocumentID), sqlutil.Int64PointerValue(entry.AROpenItemID), entry.Amount, sqlutil.StringPointerValue(entry.Note), entry.IdempotencyKey, entry.RecordedBy); err != nil {
		return fmt.Errorf("insert surplus entry: %w", err)
	}
	return nil
}

func (r *Repository) FindSurplusEntryByTypeAndIdempotency(ctx context.Context, tx *sql.Tx, documentID int64, entryType SurplusEntryType, idempotencyKey string) (*SurplusEntry, error) {
	entry, err := r.scanSurplusEntry(tx.QueryRowContext(ctx, `
		SELECT id, surplus_balance_id, entry_type, customer_id, billing_document_id, ar_open_item_id, amount, note, idempotency_key, recorded_by, created_at
		FROM ar_surplus_entries
		WHERE billing_document_id = ? AND entry_type = ? AND idempotency_key = ?
	`, documentID, entryType, idempotencyKey))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find surplus entry by idempotency: %w", err)
	}
	return entry, nil
}

func (r *Repository) GetSurplusHistoryByCustomer(ctx context.Context, customerID int64) ([]SurplusEntry, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, surplus_balance_id, entry_type, customer_id, billing_document_id, ar_open_item_id, amount, note, idempotency_key, recorded_by, created_at
		FROM ar_surplus_entries
		WHERE customer_id = ?
		ORDER BY id DESC
	`, customerID)
	if err != nil {
		return nil, fmt.Errorf("query surplus history: %w", err)
	}
	defer rows.Close()
	history := make([]SurplusEntry, 0)
	for rows.Next() {
		entry, err := r.scanSurplusEntry(rows)
		if err != nil {
			return nil, err
		}
		history = append(history, *entry)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate surplus history: %w", err)
	}
	return history, nil
}

func (r *Repository) scanSurplusBalance(scanner rowScanner) (*SurplusBalance, error) {
	var balance SurplusBalance
	var lastAppliedAt sql.NullTime
	if err := scanner.Scan(&balance.ID, &balance.CustomerID, &balance.AvailableAmount, &lastAppliedAt, &balance.CreatedBy, &balance.UpdatedBy, &balance.CreatedAt, &balance.UpdatedAt); err != nil {
		return nil, err
	}
	balance.LastAppliedAt = sqlutil.NullTimePointer(lastAppliedAt)
	return &balance, nil
}

func (r *Repository) scanSurplusEntry(scanner rowScanner) (*SurplusEntry, error) {
	var entry SurplusEntry
	var billingDocumentID sql.NullInt64
	var arOpenItemID sql.NullInt64
	var note sql.NullString
	if err := scanner.Scan(&entry.ID, &entry.SurplusBalanceID, &entry.EntryType, &entry.CustomerID, &billingDocumentID, &arOpenItemID, &entry.Amount, &note, &entry.IdempotencyKey, &entry.RecordedBy, &entry.CreatedAt); err != nil {
		return nil, err
	}
	entry.BillingDocumentID = sqlutil.NullInt64Pointer(billingDocumentID)
	entry.AROpenItemID = sqlutil.NullInt64Pointer(arOpenItemID)
	entry.Note = sqlutil.NullStringPointer(note)
	return &entry, nil
}
