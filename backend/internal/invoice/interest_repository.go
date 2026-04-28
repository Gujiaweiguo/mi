package invoice

import (
	"context"
	"database/sql"
	"fmt"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

func (r *Repository) FindInterestRateConfig(ctx context.Context, chargeType string) (*InterestRateConfig, error) {
	config, err := r.scanInterestRateConfig(r.db.QueryRowContext(ctx, `
		SELECT id, charge_type_filter, daily_rate, grace_days, is_default, status, created_by, updated_by, created_at, updated_at
		FROM interest_rate_configs
		WHERE status = ? AND charge_type_filter = ?
		ORDER BY is_default DESC, id ASC
		LIMIT 1
	`, InterestConfigStatusActive, chargeType))
	if err == nil {
		return config, nil
	}
	if err != sql.ErrNoRows {
		return nil, fmt.Errorf("find interest rate config by charge type: %w", err)
	}
	config, err = r.scanInterestRateConfig(r.db.QueryRowContext(ctx, `
		SELECT id, charge_type_filter, daily_rate, grace_days, is_default, status, created_by, updated_by, created_at, updated_at
		FROM interest_rate_configs
		WHERE status = ? AND is_default = TRUE
		ORDER BY id ASC
		LIMIT 1
	`, InterestConfigStatusActive))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find default interest rate config: %w", err)
	}
	return config, nil
}

func (r *Repository) FindInterestEntryBySourceAndIdempotency(ctx context.Context, tx *sql.Tx, sourceOpenItemID int64, idempotencyKey string) (*InterestEntry, error) {
	entry, err := r.scanInterestEntry(tx.QueryRowContext(ctx, `
		SELECT id, source_ar_open_item_id, source_billing_document_id, source_billing_document_line_id,
			generated_billing_document_id, generated_billing_document_line_id, charge_type, principal_amount,
			daily_rate, grace_days, covered_start_date, covered_end_date, interest_days, interest_amount,
			idempotency_key, created_by, created_at
		FROM invoice_interest_entries
		WHERE source_ar_open_item_id = ? AND idempotency_key = ?
	`, sourceOpenItemID, idempotencyKey))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find interest entry by idempotency: %w", err)
	}
	return entry, nil
}

func (r *Repository) FindLatestInterestEntryForSource(ctx context.Context, tx *sql.Tx, sourceOpenItemID int64) (*InterestEntry, error) {
	entry, err := r.scanInterestEntry(tx.QueryRowContext(ctx, `
		SELECT id, source_ar_open_item_id, source_billing_document_id, source_billing_document_line_id,
			generated_billing_document_id, generated_billing_document_line_id, charge_type, principal_amount,
			daily_rate, grace_days, covered_start_date, covered_end_date, interest_days, interest_amount,
			idempotency_key, created_by, created_at
		FROM invoice_interest_entries
		WHERE source_ar_open_item_id = ?
		ORDER BY covered_end_date DESC, id DESC
		LIMIT 1
	`, sourceOpenItemID))
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find latest interest entry for source: %w", err)
	}
	return entry, nil
}

func (r *Repository) InsertInterestEntry(ctx context.Context, tx *sql.Tx, entry InterestEntry) error {
	if _, err := tx.ExecContext(ctx, `
		INSERT INTO invoice_interest_entries (
			source_ar_open_item_id, source_billing_document_id, source_billing_document_line_id,
			generated_billing_document_id, generated_billing_document_line_id, charge_type,
			principal_amount, daily_rate, grace_days, covered_start_date, covered_end_date,
			interest_days, interest_amount, idempotency_key, created_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, entry.SourceAROpenItemID, entry.SourceBillingDocumentID, entry.SourceBillingLineID, entry.GeneratedDocumentID, entry.GeneratedLineID, entry.ChargeType, entry.PrincipalAmount, entry.DailyRate, entry.GraceDays, entry.CoveredStartDate, entry.CoveredEndDate, entry.InterestDays, entry.InterestAmount, entry.IdempotencyKey, entry.CreatedBy); err != nil {
		return fmt.Errorf("insert interest entry: %w", err)
	}
	return nil
}

func (r *Repository) GetInterestHistoryByDocumentID(ctx context.Context, documentID int64) ([]InterestEntry, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, source_ar_open_item_id, source_billing_document_id, source_billing_document_line_id,
			generated_billing_document_id, generated_billing_document_line_id, charge_type, principal_amount,
			daily_rate, grace_days, covered_start_date, covered_end_date, interest_days, interest_amount,
			idempotency_key, created_by, created_at
		FROM invoice_interest_entries
		WHERE source_billing_document_id = ?
		ORDER BY id DESC
	`, documentID)
	if err != nil {
		return nil, fmt.Errorf("query interest history: %w", err)
	}
	defer rows.Close()
	history := make([]InterestEntry, 0)
	for rows.Next() {
		entry, err := r.scanInterestEntry(rows)
		if err != nil {
			return nil, err
		}
		history = append(history, *entry)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate interest history: %w", err)
	}
	return history, nil
}

func (r *Repository) scanInterestRateConfig(scanner rowScanner) (*InterestRateConfig, error) {
	var config InterestRateConfig
	var chargeTypeFilter sql.NullString
	if err := scanner.Scan(&config.ID, &chargeTypeFilter, &config.DailyRate, &config.GraceDays, &config.IsDefault, &config.Status, &config.CreatedBy, &config.UpdatedBy, &config.CreatedAt, &config.UpdatedAt); err != nil {
		return nil, err
	}
	config.ChargeTypeFilter = sqlutil.NullStringPointer(chargeTypeFilter)
	return &config, nil
}

func (r *Repository) scanInterestEntry(scanner rowScanner) (*InterestEntry, error) {
	var entry InterestEntry
	if err := scanner.Scan(&entry.ID, &entry.SourceAROpenItemID, &entry.SourceBillingDocumentID, &entry.SourceBillingLineID, &entry.GeneratedDocumentID, &entry.GeneratedLineID, &entry.ChargeType, &entry.PrincipalAmount, &entry.DailyRate, &entry.GraceDays, &entry.CoveredStartDate, &entry.CoveredEndDate, &entry.InterestDays, &entry.InterestAmount, &entry.IdempotencyKey, &entry.CreatedBy, &entry.CreatedAt); err != nil {
		return nil, err
	}
	return &entry, nil
}
