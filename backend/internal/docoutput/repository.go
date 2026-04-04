package docoutput

import (
	"context"
	"database/sql"
	"fmt"
	"strings"
	"time"
)

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

func (r *Repository) UpsertTemplate(ctx context.Context, tx *sql.Tx, template *Template) error {
	var existingID int64
	err := tx.QueryRowContext(ctx, `SELECT id FROM print_templates WHERE code = ? FOR UPDATE`, template.Code).Scan(&existingID)
	if err != nil && err != sql.ErrNoRows {
		return fmt.Errorf("load print template: %w", err)
	}
	headerLines := joinLines(template.HeaderLines)
	footerLines := joinLines(template.FooterLines)
	if err == sql.ErrNoRows {
		result, err := tx.ExecContext(ctx, `
			INSERT INTO print_templates (code, name, document_type, output_mode, status, title, subtitle, header_lines, footer_lines, created_by, updated_by)
			VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
		`, template.Code, template.Name, template.DocumentType, template.OutputMode, template.Status, template.Title, nullableString(template.Subtitle), headerLines, footerLines, template.CreatedBy, template.UpdatedBy)
		if err != nil {
			return fmt.Errorf("insert print template: %w", err)
		}
		templateID, err := result.LastInsertId()
		if err != nil {
			return fmt.Errorf("get print template id: %w", err)
		}
		template.ID = templateID
		return nil
	}
	template.ID = existingID
	if _, err := tx.ExecContext(ctx, `
		UPDATE print_templates
		SET name = ?, document_type = ?, output_mode = ?, status = ?, title = ?, subtitle = ?, header_lines = ?, footer_lines = ?, updated_by = ?
		WHERE id = ?
	`, template.Name, template.DocumentType, template.OutputMode, template.Status, template.Title, nullableString(template.Subtitle), headerLines, footerLines, template.UpdatedBy, template.ID); err != nil {
		return fmt.Errorf("update print template: %w", err)
	}
	return nil
}

func (r *Repository) FindTemplateByCode(ctx context.Context, code string) (*Template, error) {
	var template Template
	var subtitle sql.NullString
	var headerLines string
	var footerLines string
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, code, name, document_type, output_mode, status, title, subtitle, header_lines, footer_lines, created_by, updated_by, created_at, updated_at
		FROM print_templates WHERE code = ?
	`, code).Scan(&template.ID, &template.Code, &template.Name, &template.DocumentType, &template.OutputMode, &template.Status, &template.Title, &subtitle, &headerLines, &footerLines, &template.CreatedBy, &template.UpdatedBy, &template.CreatedAt, &template.UpdatedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find print template: %w", err)
	}
	template.Subtitle = subtitle.String
	template.HeaderLines = splitLines(headerLines)
	template.FooterLines = splitLines(footerLines)
	return &template, nil
}

func (r *Repository) ListTemplates(ctx context.Context, filter ListFilter) (*ListResult, error) {
	page, pageSize := normalizePage(filter.Page, filter.PageSize)
	var total int64
	if err := r.db.QueryRowContext(ctx, `SELECT COUNT(*) FROM print_templates`).Scan(&total); err != nil {
		return nil, fmt.Errorf("count print templates: %w", err)
	}
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, code, name, document_type, output_mode, status, title, subtitle, header_lines, footer_lines, created_by, updated_by, created_at, updated_at
		FROM print_templates ORDER BY id DESC LIMIT ? OFFSET ?
	`, pageSize, (page-1)*pageSize)
	if err != nil {
		return nil, fmt.Errorf("list print templates: %w", err)
	}
	defer rows.Close()
	items := make([]Template, 0)
	for rows.Next() {
		var item Template
		var subtitle sql.NullString
		var headerLines string
		var footerLines string
		if err := rows.Scan(&item.ID, &item.Code, &item.Name, &item.DocumentType, &item.OutputMode, &item.Status, &item.Title, &subtitle, &headerLines, &footerLines, &item.CreatedBy, &item.UpdatedBy, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan print template: %w", err)
		}
		item.Subtitle = subtitle.String
		item.HeaderLines = splitLines(headerLines)
		item.FooterLines = splitLines(footerLines)
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate print templates: %w", err)
	}
	return &ListResult{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func normalizePage(page, pageSize int) (int, int) {
	if page < 1 {
		page = DefaultPage
	}
	if pageSize < 1 {
		pageSize = DefaultPageSize
	}
	if pageSize > MaxPageSize {
		pageSize = MaxPageSize
	}
	return page, pageSize
}

func joinLines(lines []string) string {
	trimmed := make([]string, 0, len(lines))
	for _, line := range lines {
		line = strings.TrimSpace(line)
		if line == "" {
			continue
		}
		trimmed = append(trimmed, line)
	}
	return strings.Join(trimmed, "\n")
}

func splitLines(value string) []string {
	if strings.TrimSpace(value) == "" {
		return []string{}
	}
	parts := strings.Split(value, "\n")
	result := make([]string, 0, len(parts))
	for _, part := range parts {
		part = strings.TrimSpace(part)
		if part == "" {
			continue
		}
		result = append(result, part)
	}
	return result
}

func nullableString(value string) any {
	value = strings.TrimSpace(value)
	if value == "" {
		return nil
	}
	return value
}

var _ = time.Time{}
