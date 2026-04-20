package notification

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"strings"
	"time"
)

// Repository persists and queries notification outbox records.
type Repository struct {
	db  *sql.DB
	now func() time.Time
}

// NewRepository constructs a notification repository.
func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db, now: time.Now().UTC}
}

// NewRepositoryWithNowFunc constructs a repository with a custom clock.
func NewRepositoryWithNowFunc(db *sql.DB, now func() time.Time) *Repository {
	repo := NewRepository(db)
	if now != nil {
		repo.now = now
	}
	return repo
}

// InsertOutbox inserts a notification outbox row inside the caller transaction.
func (r *Repository) InsertOutbox(ctx context.Context, tx *sql.Tx, entry *OutboxEntry) error {
	if tx == nil {
		return errors.New("insert notification outbox: nil transaction")
	}
	if entry == nil {
		return errors.New("insert notification outbox: nil entry")
	}

	result, err := tx.ExecContext(ctx, `
		INSERT INTO notification_outbox (
			event_type,
			aggregate_type,
			aggregate_id,
			recipient_to,
			recipient_cc,
			subject,
			template_name,
			template_data,
			status,
			attempt_count,
			max_attempts,
			next_attempt_at,
			sent_at,
			last_error
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`,
		entry.EventType,
		entry.AggregateType,
		entry.AggregateID,
		entry.RecipientTo,
		nullableString(entry.RecipientCc),
		entry.Subject,
		entry.TemplateName,
		[]byte(entry.TemplateData),
		entry.Status,
		entry.AttemptCount,
		entry.MaxAttempts,
		entry.NextAttemptAt,
		entry.SentAt,
		nullablePointerString(entry.LastError),
	)
	if err != nil {
		return fmt.Errorf("insert notification outbox: %w", err)
	}

	id, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("resolve notification outbox id: %w", err)
	}
	entry.ID = id
	return nil
}

// FetchPending claims pending or retryable outbox rows for processing.
func (r *Repository) FetchPending(ctx context.Context, db *sql.DB, batchSize int) ([]*OutboxEntry, error) {
	if batchSize <= 0 {
		batchSize = 20
	}
	if db == nil {
		db = r.db
	}
	if db == nil {
		return nil, errors.New("fetch notification outbox: nil database")
	}

	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin notification fetch transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	rows, err := tx.QueryContext(ctx, `
		SELECT id, event_type, aggregate_type, aggregate_id, recipient_to, recipient_cc, subject, template_name, template_data, status, attempt_count, max_attempts, next_attempt_at, sent_at, last_error, created_at, updated_at
		FROM notification_outbox
		WHERE status IN ('pending', 'failed')
		  AND (next_attempt_at IS NULL OR next_attempt_at <= UTC_TIMESTAMP())
		ORDER BY created_at ASC
		LIMIT ?
		FOR UPDATE SKIP LOCKED
	`, batchSize)
	if err != nil {
		return nil, fmt.Errorf("query pending notification outbox: %w", err)
	}
	defer rows.Close()

	entries := make([]*OutboxEntry, 0, batchSize)
	ids := make([]int64, 0, batchSize)
	for rows.Next() {
		entry, scanErr := scanOutboxEntry(rows)
		if scanErr != nil {
			return nil, scanErr
		}
		entries = append(entries, entry)
		ids = append(ids, entry.ID)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate pending notification outbox: %w", err)
	}

	if len(ids) == 0 {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit empty notification fetch transaction: %w", err)
		}
		return entries, nil
	}

	placeholders := strings.TrimRight(strings.Repeat("?,", len(ids)), ",")
	args := make([]any, 0, len(ids)+1)
	args = append(args, StatusSending)
	for _, id := range ids {
		args = append(args, id)
	}
	query := fmt.Sprintf("UPDATE notification_outbox SET status = ?, updated_at = CURRENT_TIMESTAMP WHERE id IN (%s)", placeholders)
	if _, err := tx.ExecContext(ctx, query, args...); err != nil {
		return nil, fmt.Errorf("mark notification outbox sending: %w", err)
	}
	for _, entry := range entries {
		entry.Status = StatusSending
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit notification fetch transaction: %w", err)
	}
	return entries, nil
}

// UpdateStatus persists the latest delivery state for an outbox entry.
func (r *Repository) UpdateStatus(ctx context.Context, entry *OutboxEntry) error {
	if entry == nil {
		return errors.New("update notification outbox status: nil entry")
	}
	if r.db == nil {
		return errors.New("update notification outbox status: nil database")
	}

	_, err := r.db.ExecContext(ctx, `
		UPDATE notification_outbox
		SET status = ?,
		    attempt_count = ?,
		    max_attempts = ?,
		    next_attempt_at = ?,
		    sent_at = ?,
		    last_error = ?,
		    subject = ?,
		    updated_at = CURRENT_TIMESTAMP
		WHERE id = ?
	`, entry.Status, entry.AttemptCount, entry.MaxAttempts, entry.NextAttemptAt, entry.SentAt, nullablePointerString(entry.LastError), entry.Subject, entry.ID)
	if err != nil {
		return fmt.Errorf("update notification outbox status: %w", err)
	}
	return nil
}

// ListOutbox returns paginated notification history rows and the total match count.
func (r *Repository) ListOutbox(ctx context.Context, db *sql.DB, params ListParams) ([]*OutboxEntry, int, error) {
	if db == nil {
		db = r.db
	}
	if db == nil {
		return nil, 0, errors.New("list notification outbox: nil database")
	}

	page, pageSize := normalizePagination(params.Page, params.PageSize)
	where, args := buildListFilters(params)

	countQuery := "SELECT COUNT(*) FROM notification_outbox"
	if where != "" {
		countQuery += " WHERE " + where
	}
	var total int
	if err := db.QueryRowContext(ctx, countQuery, args...).Scan(&total); err != nil {
		return nil, 0, fmt.Errorf("count notification outbox: %w", err)
	}

	query := `SELECT id, event_type, aggregate_type, aggregate_id, recipient_to, recipient_cc, subject, template_name, template_data, status, attempt_count, max_attempts, next_attempt_at, sent_at, last_error, created_at, updated_at FROM notification_outbox`
	if where != "" {
		query += " WHERE " + where
	}
	query += ` ORDER BY created_at DESC LIMIT ? OFFSET ?`
	listArgs := append(append([]any{}, args...), pageSize, (page-1)*pageSize)

	rows, err := db.QueryContext(ctx, query, listArgs...)
	if err != nil {
		return nil, 0, fmt.Errorf("query notification outbox: %w", err)
	}
	defer rows.Close()

	items := make([]*OutboxEntry, 0, pageSize)
	for rows.Next() {
		entry, scanErr := scanOutboxEntry(rows)
		if scanErr != nil {
			return nil, 0, scanErr
		}
		items = append(items, entry)
	}
	if err := rows.Err(); err != nil {
		return nil, 0, fmt.Errorf("iterate notification outbox rows: %w", err)
	}

	return items, total, nil
}

func scanOutboxEntry(scanner interface{ Scan(dest ...any) error }) (*OutboxEntry, error) {
	var entry OutboxEntry
	var recipientCc sql.NullString
	var nextAttemptAt sql.NullTime
	var sentAt sql.NullTime
	var lastError sql.NullString
	if err := scanner.Scan(
		&entry.ID,
		&entry.EventType,
		&entry.AggregateType,
		&entry.AggregateID,
		&entry.RecipientTo,
		&recipientCc,
		&entry.Subject,
		&entry.TemplateName,
		&entry.TemplateData,
		&entry.Status,
		&entry.AttemptCount,
		&entry.MaxAttempts,
		&nextAttemptAt,
		&sentAt,
		&lastError,
		&entry.CreatedAt,
		&entry.UpdatedAt,
	); err != nil {
		return nil, fmt.Errorf("scan notification outbox row: %w", err)
	}
	if recipientCc.Valid {
		entry.RecipientCc = recipientCc.String
	}
	if nextAttemptAt.Valid {
		value := nextAttemptAt.Time
		entry.NextAttemptAt = &value
	}
	if sentAt.Valid {
		value := sentAt.Time
		entry.SentAt = &value
	}
	if lastError.Valid {
		value := lastError.String
		entry.LastError = &value
	}
	return &entry, nil
}

func buildListFilters(params ListParams) (string, []any) {
	conditions := make([]string, 0, 4)
	args := make([]any, 0, 4)
	if strings.TrimSpace(params.EventType) != "" {
		conditions = append(conditions, "event_type = ?")
		args = append(args, strings.TrimSpace(params.EventType))
	}
	if strings.TrimSpace(params.AggregateType) != "" {
		conditions = append(conditions, "aggregate_type = ?")
		args = append(args, strings.TrimSpace(params.AggregateType))
	}
	if params.AggregateID != nil {
		conditions = append(conditions, "aggregate_id = ?")
		args = append(args, *params.AggregateID)
	}
	if strings.TrimSpace(params.Status) != "" {
		conditions = append(conditions, "status = ?")
		args = append(args, strings.TrimSpace(params.Status))
	}
	return strings.Join(conditions, " AND "), args
}

func normalizePagination(page, pageSize int) (int, int) {
	if page <= 0 {
		page = 1
	}
	if pageSize <= 0 {
		pageSize = 20
	}
	if pageSize > 100 {
		pageSize = 100
	}
	return page, pageSize
}

func nullableString(value string) any {
	if strings.TrimSpace(value) == "" {
		return nil
	}
	return value
}

func nullablePointerString(value *string) any {
	if value == nil {
		return nil
	}
	if strings.TrimSpace(*value) == "" {
		return nil
	}
	return *value
}
