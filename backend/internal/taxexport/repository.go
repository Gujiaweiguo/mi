package taxexport

import (
	"context"
	"database/sql"
	"fmt"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
)

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

func (r *Repository) UpsertRuleSet(ctx context.Context, tx *sql.Tx, ruleSet *RuleSet) error {
	var existingID int64
	err := tx.QueryRowContext(ctx, `SELECT id FROM tax_voucher_rule_sets WHERE code = ? FOR UPDATE`, ruleSet.Code).Scan(&existingID)
	if err != nil && err != sql.ErrNoRows {
		return fmt.Errorf("load tax voucher rule set: %w", err)
	}
	if err == sql.ErrNoRows {
		result, err := tx.ExecContext(ctx, `
			INSERT INTO tax_voucher_rule_sets (code, name, document_type, status, created_by, updated_by)
			VALUES (?, ?, ?, ?, ?, ?)
		`, ruleSet.Code, ruleSet.Name, ruleSet.DocumentType, ruleSet.Status, ruleSet.CreatedBy, ruleSet.UpdatedBy)
		if err != nil {
			return fmt.Errorf("insert tax voucher rule set: %w", err)
		}
		ruleSet.ID, err = result.LastInsertId()
		if err != nil {
			return fmt.Errorf("get tax voucher rule set id: %w", err)
		}
	} else {
		ruleSet.ID = existingID
		if _, err := tx.ExecContext(ctx, `
			UPDATE tax_voucher_rule_sets SET name = ?, document_type = ?, status = ?, updated_by = ? WHERE id = ?
		`, ruleSet.Name, ruleSet.DocumentType, ruleSet.Status, ruleSet.UpdatedBy, ruleSet.ID); err != nil {
			return fmt.Errorf("update tax voucher rule set: %w", err)
		}
		if _, err := tx.ExecContext(ctx, `DELETE FROM tax_voucher_rules WHERE rule_set_id = ?`, ruleSet.ID); err != nil {
			return fmt.Errorf("delete existing tax voucher rules: %w", err)
		}
	}
	for _, rule := range ruleSet.Rules {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO tax_voucher_rules (
				rule_set_id, sequence_no, entry_side, charge_type_filter, account_number, account_name,
				explanation_template, use_tenant_name, is_balancing_entry
			) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
		`, ruleSet.ID, rule.SequenceNo, rule.EntrySide, rule.ChargeTypeFilter, rule.AccountNumber, rule.AccountName, rule.ExplanationTemplate, rule.UseTenantName, rule.IsBalancingEntry); err != nil {
			return fmt.Errorf("insert tax voucher rule: %w", err)
		}
	}
	return nil
}

func (r *Repository) ListRuleSets(ctx context.Context, filter ListFilter) (*pagination.ListResult[RuleSet], error) {
	page, pageSize := pagination.NormalizePage(filter.Page, filter.PageSize)
	var total int64
	if err := r.db.QueryRowContext(ctx, `SELECT COUNT(*) FROM tax_voucher_rule_sets`).Scan(&total); err != nil {
		return nil, fmt.Errorf("count tax voucher rule sets: %w", err)
	}
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, code, name, document_type, status, created_by, updated_by, created_at, updated_at
		FROM tax_voucher_rule_sets ORDER BY id DESC LIMIT ? OFFSET ?
	`, pageSize, (page-1)*pageSize)
	if err != nil {
		return nil, fmt.Errorf("list tax voucher rule sets: %w", err)
	}
	defer rows.Close()
	items := make([]RuleSet, 0)
	for rows.Next() {
		var item RuleSet
		if err := rows.Scan(&item.ID, &item.Code, &item.Name, &item.DocumentType, &item.Status, &item.CreatedBy, &item.UpdatedBy, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan tax voucher rule set: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate tax voucher rule sets: %w", err)
	}
	for index := range items {
		rules, err := r.loadRules(ctx, items[index].ID)
		if err != nil {
			return nil, err
		}
		items[index].Rules = rules
	}
	return &pagination.ListResult[RuleSet]{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func (r *Repository) FindRuleSetByCode(ctx context.Context, code string) (*RuleSet, error) {
	var item RuleSet
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, code, name, document_type, status, created_by, updated_by, created_at, updated_at
		FROM tax_voucher_rule_sets WHERE code = ?
	`, code).Scan(&item.ID, &item.Code, &item.Name, &item.DocumentType, &item.Status, &item.CreatedBy, &item.UpdatedBy, &item.CreatedAt, &item.UpdatedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("find tax voucher rule set: %w", err)
	}
	rules, err := r.loadRules(ctx, item.ID)
	if err != nil {
		return nil, err
	}
	item.Rules = rules
	return &item, nil
}

func (r *Repository) ListApprovedDocumentsForExport(ctx context.Context, documentType string, fromDate, toDate time.Time) ([]exportDocument, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT bd.id, bd.document_no, bd.document_type, bd.tenant_name, bd.approved_at, bd.period_start, bd.period_end, bd.total_amount, bd.currency_type_id,
			bdl.billing_charge_line_id, bdl.charge_type, bdl.period_start, bdl.period_end, bdl.quantity_days, bdl.unit_amount, bdl.amount
		FROM billing_documents bd
		INNER JOIN billing_document_lines bdl ON bdl.billing_document_id = bd.id
		WHERE bd.document_type = ? AND bd.status = 'approved' AND bd.approved_at IS NOT NULL
		  AND DATE(bd.approved_at) >= ? AND DATE(bd.approved_at) <= ?
		ORDER BY bd.approved_at, bd.id, bdl.id
	`, documentType, fromDate, toDate)
	if err != nil {
		return nil, fmt.Errorf("list approved documents for tax export: %w", err)
	}
	defer rows.Close()
	byID := make(map[int64]*exportDocument)
	orderedIDs := make([]int64, 0)
	for rows.Next() {
		var (
			docID          int64
			docNo          sql.NullString
			docType        string
			tenant         string
			approvedAt     time.Time
			periodStart    time.Time
			periodEnd      time.Time
			totalAmount    float64
			currencyTypeID int64
			line           exportLine
		)
		if err := rows.Scan(&docID, &docNo, &docType, &tenant, &approvedAt, &periodStart, &periodEnd, &totalAmount, &currencyTypeID, &line.BillingChargeLineID, &line.ChargeType, &line.PeriodStart, &line.PeriodEnd, &line.QuantityDays, &line.UnitAmount, &line.Amount); err != nil {
			return nil, fmt.Errorf("scan approved tax export document: %w", err)
		}
		doc := byID[docID]
		if doc == nil {
			doc = &exportDocument{DocumentID: docID, DocumentNo: docNo.String, DocumentType: docType, TenantName: tenant, ApprovedAt: approvedAt, PeriodStart: periodStart, PeriodEnd: periodEnd, TotalAmount: totalAmount, CurrencyTypeID: currencyTypeID, Lines: []exportLine{}}
			byID[docID] = doc
			orderedIDs = append(orderedIDs, docID)
		}
		doc.Lines = append(doc.Lines, line)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate approved tax export documents: %w", err)
	}
	items := make([]exportDocument, 0, len(orderedIDs))
	for _, id := range orderedIDs {
		items = append(items, *byID[id])
	}
	return items, nil
}

func (r *Repository) loadRules(ctx context.Context, ruleSetID int64) ([]Rule, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, rule_set_id, sequence_no, entry_side, charge_type_filter, account_number, account_name, explanation_template, use_tenant_name, is_balancing_entry, created_at, updated_at
		FROM tax_voucher_rules WHERE rule_set_id = ? ORDER BY sequence_no
	`, ruleSetID)
	if err != nil {
		return nil, fmt.Errorf("query tax voucher rules: %w", err)
	}
	defer rows.Close()
	rules := make([]Rule, 0)
	for rows.Next() {
		var rule Rule
		if err := rows.Scan(&rule.ID, &rule.RuleSetID, &rule.SequenceNo, &rule.EntrySide, &rule.ChargeTypeFilter, &rule.AccountNumber, &rule.AccountName, &rule.ExplanationTemplate, &rule.UseTenantName, &rule.IsBalancingEntry, &rule.CreatedAt, &rule.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan tax voucher rule: %w", err)
		}
		rules = append(rules, rule)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate tax voucher rules: %w", err)
	}
	return rules, nil
}
