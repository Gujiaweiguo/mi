package invoice

import (
	"context"
	"database/sql"
	"fmt"
	"sort"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

type Repository struct {
	db *sql.DB
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db}
}

func (r *Repository) Create(ctx context.Context, tx *sql.Tx, document *Document) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO billing_documents (
			document_type, document_no, billing_run_id, lease_contract_id, tenant_name, period_start, period_end,
			total_amount, currency_type_id, status, adjusted_from_id, created_by, updated_by
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, document.DocumentType, sqlutil.StringPointerValue(document.DocumentNo), document.BillingRunID, document.LeaseContractID, document.TenantName, document.PeriodStart, document.PeriodEnd, document.TotalAmount, document.CurrencyTypeID, document.Status, sqlutil.Int64PointerValue(document.AdjustedFromID), document.CreatedBy, document.UpdatedBy)
	if err != nil {
		return fmt.Errorf("insert billing document: %w", err)
	}
	documentID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get billing document id: %w", err)
	}
	document.ID = documentID
	for _, line := range document.Lines {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO billing_document_lines (
				billing_document_id, billing_charge_line_id, charge_type, period_start, period_end,
				quantity_days, unit_amount, amount
			) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
		`, documentID, line.BillingChargeLineID, line.ChargeType, line.PeriodStart, line.PeriodEnd, line.QuantityDays, line.UnitAmount, line.Amount); err != nil {
			return fmt.Errorf("insert billing document line: %w", err)
		}
	}
	return nil
}

func (r *Repository) FindByID(ctx context.Context, id int64) (*Document, error) {
	document, err := r.findDocument(ctx, r.db.QueryRowContext(ctx, documentSelect+` WHERE bd.id = ?`, id))
	if err != nil || document == nil {
		return document, err
	}
	if err := r.loadLines(ctx, r.db, document); err != nil {
		return nil, err
	}
	return document, nil
}

func (r *Repository) FindByIDForUpdate(ctx context.Context, tx *sql.Tx, id int64) (*Document, error) {
	document, err := r.findDocument(ctx, tx.QueryRowContext(ctx, documentSelect+` WHERE bd.id = ? FOR UPDATE`, id))
	if err != nil || document == nil {
		return document, err
	}
	if err := r.loadLines(ctx, tx, document); err != nil {
		return nil, err
	}
	return document, nil
}

func (r *Repository) List(ctx context.Context, filter ListFilter) (*pagination.ListResult[Document], error) {
	page, pageSize := pagination.NormalizePage(filter.Page, filter.PageSize)
	conditions := []string{"1=1"}
	args := make([]any, 0)
	if filter.DocumentType != nil {
		conditions = append(conditions, "bd.document_type = ?")
		args = append(args, *filter.DocumentType)
	}
	if filter.Status != nil {
		conditions = append(conditions, "bd.status = ?")
		args = append(args, *filter.Status)
	}
	if filter.LeaseContractID != nil {
		conditions = append(conditions, "bd.lease_contract_id = ?")
		args = append(args, *filter.LeaseContractID)
	}
	if filter.BillingRunID != nil {
		conditions = append(conditions, "bd.billing_run_id = ?")
		args = append(args, *filter.BillingRunID)
	}
	whereClause := strings.Join(conditions, " AND ")
	var total int64
	if err := r.db.QueryRowContext(ctx, `SELECT COUNT(*) FROM billing_documents bd WHERE `+whereClause, args...).Scan(&total); err != nil {
		return nil, fmt.Errorf("count billing documents: %w", err)
	}
	queryArgs := append(append([]any{}, args...), pageSize, (page-1)*pageSize)
	rows, err := r.db.QueryContext(ctx, documentSelect+` WHERE `+whereClause+` ORDER BY bd.id DESC LIMIT ? OFFSET ?`, queryArgs...)
	if err != nil {
		return nil, fmt.Errorf("list billing documents: %w", err)
	}
	defer rows.Close()
	items := make([]Document, 0)
	for rows.Next() {
		document, err := r.scanDocument(rows)
		if err != nil {
			return nil, err
		}
		items = append(items, *document)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate billing documents: %w", err)
	}
	return &pagination.ListResult[Document]{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func (r *Repository) AttachWorkflowInstance(ctx context.Context, tx *sql.Tx, documentID, workflowInstanceID, updatedBy int64, submittedAt time.Time) error {
	result, err := tx.ExecContext(ctx, `
		UPDATE billing_documents
		SET status = ?, workflow_instance_id = ?, submitted_at = ?, updated_by = ?
		WHERE id = ? AND status = ? AND workflow_instance_id IS NULL
	`, StatusPendingApproval, workflowInstanceID, submittedAt, updatedBy, documentID, StatusDraft)
	if err != nil {
		return fmt.Errorf("attach workflow instance to billing document: %w", err)
	}
	affected, err := result.RowsAffected()
	if err != nil {
		return fmt.Errorf("determine billing document workflow attach rows: %w", err)
	}
	if affected != 1 {
		return ErrDocumentAlreadySubmitted
	}
	return nil
}

func (r *Repository) UpdateWorkflowState(ctx context.Context, tx *sql.Tx, documentID, workflowInstanceID, updatedBy int64, status Status, documentNo *string, approvedAt *time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE billing_documents
		SET workflow_instance_id = ?, status = ?, document_no = ?, approved_at = ?, updated_by = ?
		WHERE id = ?
	`, workflowInstanceID, status, sqlutil.StringPointerValue(documentNo), sqlutil.TimePointerValue(approvedAt), updatedBy, documentID); err != nil {
		return fmt.Errorf("update billing document workflow state: %w", err)
	}
	return nil
}

func (r *Repository) Cancel(ctx context.Context, tx *sql.Tx, documentID, updatedBy int64, cancelledAt time.Time) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE billing_documents
		SET status = ?, cancelled_at = ?, updated_by = ?
		WHERE id = ?
	`, StatusCancelled, cancelledAt, updatedBy, documentID); err != nil {
		return fmt.Errorf("cancel billing document: %w", err)
	}
	return nil
}

func (r *Repository) MarkAdjusted(ctx context.Context, tx *sql.Tx, documentID, updatedBy int64) error {
	result, err := tx.ExecContext(ctx, `
		UPDATE billing_documents
		SET status = ?, updated_by = ?
		WHERE id = ? AND status = ?
	`, StatusAdjusted, updatedBy, documentID, StatusApproved)
	if err != nil {
		return fmt.Errorf("mark billing document adjusted: %w", err)
	}
	affectedRows, err := result.RowsAffected()
	if err != nil {
		return fmt.Errorf("determine adjusted billing document rows: %w", err)
	}
	if affectedRows != 1 {
		return ErrInvalidDocumentState
	}
	return nil
}

func (r *Repository) CountReservedChargeLines(ctx context.Context, tx *sql.Tx, chargeLineIDs []int64) (int64, error) {
	if len(chargeLineIDs) == 0 {
		return 0, nil
	}
	args := make([]any, 0, len(chargeLineIDs)+4)
	for _, id := range chargeLineIDs {
		args = append(args, id)
	}
	args = append(args, StatusDraft, StatusPendingApproval, StatusApproved, StatusRejected)
	var count int64
	query := `
		SELECT COUNT(DISTINCT bdl.billing_charge_line_id)
		FROM billing_document_lines bdl
		INNER JOIN billing_documents bd ON bd.id = bdl.billing_document_id
		WHERE bdl.billing_charge_line_id IN (` + sqlutil.InPlaceholders(len(chargeLineIDs)) + `)
		  AND bd.status IN (?, ?, ?, ?)
	`
	if err := tx.QueryRowContext(ctx, query, args...).Scan(&count); err != nil {
		return 0, fmt.Errorf("count reserved billing charge lines: %w", err)
	}
	return count, nil
}

func (r *Repository) AllocateNumber(ctx context.Context, tx *sql.Tx, sequenceCode string) (string, error) {
	var prefix string
	var nextValue int64
	if err := tx.QueryRowContext(ctx, `SELECT prefix, next_value FROM numbering_sequences WHERE code = ? FOR UPDATE`, sequenceCode).Scan(&prefix, &nextValue); err != nil {
		return "", fmt.Errorf("load numbering sequence %s: %w", sequenceCode, err)
	}
	if _, err := tx.ExecContext(ctx, `UPDATE numbering_sequences SET next_value = next_value + 1 WHERE code = ?`, sequenceCode); err != nil {
		return "", fmt.Errorf("increment numbering sequence %s: %w", sequenceCode, err)
	}
	return fmt.Sprintf("%s-%d", prefix, nextValue), nil
}

type rowScanner interface{ Scan(dest ...any) error }
type queryer interface {
	QueryContext(context.Context, string, ...any) (*sql.Rows, error)
}

const documentSelect = `
	SELECT bd.id, bd.document_type, bd.document_no, bd.billing_run_id, bd.lease_contract_id, bd.tenant_name,
		bd.period_start, bd.period_end, bd.total_amount, bd.currency_type_id, bd.status, bd.workflow_instance_id,
		bd.adjusted_from_id, bd.submitted_at, bd.approved_at, bd.cancelled_at, bd.created_by, bd.updated_by,
		bd.created_at, bd.updated_at
	FROM billing_documents bd
`

func (r *Repository) findDocument(_ context.Context, scanner rowScanner) (*Document, error) {
	document, err := r.scanDocument(scanner)
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, err
	}
	return document, nil
}

func (r *Repository) scanDocument(scanner rowScanner) (*Document, error) {
	var document Document
	var documentNo sql.NullString
	var workflowInstanceID sql.NullInt64
	var adjustedFromID sql.NullInt64
	var submittedAt sql.NullTime
	var approvedAt sql.NullTime
	var cancelledAt sql.NullTime
	if err := scanner.Scan(&document.ID, &document.DocumentType, &documentNo, &document.BillingRunID, &document.LeaseContractID, &document.TenantName, &document.PeriodStart, &document.PeriodEnd, &document.TotalAmount, &document.CurrencyTypeID, &document.Status, &workflowInstanceID, &adjustedFromID, &submittedAt, &approvedAt, &cancelledAt, &document.CreatedBy, &document.UpdatedBy, &document.CreatedAt, &document.UpdatedAt); err != nil {
		return nil, err
	}
	document.DocumentNo = sqlutil.NullStringPointer(documentNo)
	document.WorkflowInstanceID = sqlutil.NullInt64Pointer(workflowInstanceID)
	document.AdjustedFromID = sqlutil.NullInt64Pointer(adjustedFromID)
	document.SubmittedAt = sqlutil.NullTimePointer(submittedAt)
	document.ApprovedAt = sqlutil.NullTimePointer(approvedAt)
	document.CancelledAt = sqlutil.NullTimePointer(cancelledAt)
	return &document, nil
}

func (r *Repository) loadLines(ctx context.Context, db queryer, document *Document) error {
	rows, err := db.QueryContext(ctx, `
		SELECT id, billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount, created_at
		FROM billing_document_lines WHERE billing_document_id = ? ORDER BY id
	`, document.ID)
	if err != nil {
		return fmt.Errorf("query billing document lines: %w", err)
	}
	defer rows.Close()
	lines := make([]Line, 0)
	for rows.Next() {
		var line Line
		if err := rows.Scan(&line.ID, &line.BillingDocumentID, &line.BillingChargeLineID, &line.ChargeType, &line.PeriodStart, &line.PeriodEnd, &line.QuantityDays, &line.UnitAmount, &line.Amount, &line.CreatedAt); err != nil {
			return fmt.Errorf("scan billing document line: %w", err)
		}
		lines = append(lines, line)
	}
	if err := rows.Err(); err != nil {
		return fmt.Errorf("iterate billing document lines: %w", err)
	}
	document.Lines = lines
	return nil
}

func normalizeChargeLineIDs(ids []int64) []int64 {
	unique := make(map[int64]struct{}, len(ids))
	result := make([]int64, 0, len(ids))
	for _, id := range ids {
		if id == 0 {
			continue
		}
		if _, ok := unique[id]; ok {
			continue
		}
		unique[id] = struct{}{}
		result = append(result, id)
	}
	sort.Slice(result, func(i, j int) bool { return result[i] < result[j] })
	return result
}


