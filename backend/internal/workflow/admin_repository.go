package workflow

import (
	"context"
	"database/sql"
	"fmt"
	"strings"
	"time"
)

func (r *Repository) FindTemplateByID(ctx context.Context, id int64) (*Template, error) {
	const query = `
		SELECT id, business_group_id, code, name, process_class, status, published_definition_id, published_version_number
		FROM workflow_templates
		WHERE id = ?`
	return r.findTemplate(ctx, query, id)
}

func (r *Repository) FindTemplateByCode(ctx context.Context, code string) (*Template, error) {
	const query = `
		SELECT id, business_group_id, code, name, process_class, status, published_definition_id, published_version_number
		FROM workflow_templates
		WHERE code = ?`
	return r.findTemplate(ctx, query, code)
}

func (r *Repository) ListTemplates(ctx context.Context) ([]Template, error) {
	const query = `
		SELECT id, business_group_id, code, name, process_class, status, published_definition_id, published_version_number
		FROM workflow_templates
		ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("query workflow templates: %w", err)
	}
	defer rows.Close()

	templates := make([]Template, 0)
	for rows.Next() {
		template, err := scanTemplate(rows)
		if err != nil {
			return nil, err
		}
		templates = append(templates, *template)
	}
	return templates, rows.Err()
}

func (r *Repository) CreateTemplate(ctx context.Context, tx *sql.Tx, template Template) (*Template, error) {
	if tx == nil {
		return nil, fmt.Errorf("insert workflow template: nil transaction")
	}
	if template.Status == "" {
		template.Status = "active"
	}
	_, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_templates (id, business_group_id, code, name, process_class, status, published_definition_id, published_version_number)
		VALUES (?, ?, ?, ?, ?, ?, ?, ?)
	`, template.ID, template.BusinessGroupID, template.Code, template.Name, template.ProcessClass, template.Status, template.PublishedDefinitionID, template.PublishedVersionNumber)
	if err != nil {
		return nil, fmt.Errorf("insert workflow template: %w", err)
	}
	return &template, nil
}

func (r *Repository) CreateDefinition(ctx context.Context, tx *sql.Tx, definition Definition) (*Definition, error) {
	if tx == nil {
		return nil, fmt.Errorf("insert workflow definition: nil transaction")
	}
	if definition.Status == "" {
		definition.Status = "active"
	}
	if definition.VoucherType == "" {
		definition.VoucherType = "application"
	}
	if definition.LifecycleStatus == "" {
		definition.LifecycleStatus = string(DefinitionLifecycleStatusDraft)
	}
	if !definition.TransitionsEnabled {
		definition.TransitionsEnabled = true
	}
	_, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_definitions (
			id, business_group_id, workflow_template_id, code, version_number, name, voucher_type, is_initial,
			status, lifecycle_status, published_at, transitions_enabled, process_class
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, definition.ID, definition.BusinessGroupID, definition.WorkflowTemplateID, definition.Code, definition.VersionNumber, definition.Name, definition.VoucherType, definition.IsInitial, definition.Status, definition.LifecycleStatus, definition.PublishedAt, definition.TransitionsEnabled, definition.ProcessClass)
	if err != nil {
		return nil, fmt.Errorf("insert workflow definition: %w", err)
	}
	return &definition, nil
}

func (r *Repository) ListDefinitionVersions(ctx context.Context, templateID int64) ([]Definition, error) {
	const query = `SELECT id FROM workflow_definitions WHERE workflow_template_id = ? ORDER BY version_number DESC, id DESC`
	rows, err := r.db.QueryContext(ctx, query, templateID)
	if err != nil {
		return nil, fmt.Errorf("query workflow definition versions: %w", err)
	}
	defer rows.Close()

	definitions := make([]Definition, 0)
	for rows.Next() {
		var id int64
		if err := rows.Scan(&id); err != nil {
			return nil, fmt.Errorf("scan workflow definition version id: %w", err)
		}
		definition, err := r.FindDefinitionByID(ctx, id)
		if err != nil {
			return nil, err
		}
		if definition != nil {
			definitions = append(definitions, *definition)
		}
	}
	return definitions, rows.Err()
}

func (r *Repository) FindDraftDefinitionByTemplate(ctx context.Context, templateID int64) (*Definition, error) {
	const query = `
		SELECT id, business_group_id, workflow_template_id, code, version_number, name, voucher_type, is_initial, status, lifecycle_status, published_at, transitions_enabled, process_class
		FROM workflow_definitions
		WHERE workflow_template_id = ? AND lifecycle_status = 'draft'
		ORDER BY version_number DESC, id DESC
		LIMIT 1`
	return r.findDefinition(ctx, query, templateID)
}

func (r *Repository) FindPublishedDefinitionByTemplate(ctx context.Context, templateID int64) (*Definition, error) {
	const query = `
		SELECT id, business_group_id, workflow_template_id, code, version_number, name, voucher_type, is_initial, status, lifecycle_status, published_at, transitions_enabled, process_class
		FROM workflow_definitions
		WHERE workflow_template_id = ? AND status = 'active' AND lifecycle_status = 'published'
		ORDER BY version_number DESC, id DESC
		LIMIT 1`
	return r.findDefinition(ctx, query, templateID)
}

func (r *Repository) UpdateDefinitionDraftFields(ctx context.Context, tx *sql.Tx, definition Definition) error {
	if tx == nil {
		return fmt.Errorf("update workflow definition draft fields: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `
		UPDATE workflow_definitions
		SET name = ?, voucher_type = ?, is_initial = ?, transitions_enabled = ?, updated_at = CURRENT_TIMESTAMP
		WHERE id = ?
	`, definition.Name, definition.VoucherType, definition.IsInitial, definition.TransitionsEnabled, definition.ID)
	if err != nil {
		return fmt.Errorf("update workflow definition draft fields: %w", err)
	}
	return nil
}

func (r *Repository) CreateNode(ctx context.Context, tx *sql.Tx, node Node) (*Node, error) {
	if tx == nil {
		return nil, fmt.Errorf("insert workflow node: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_nodes (
			id, workflow_definition_id, function_id, role_id, step_order, code, name,
			can_submit_to_manager, validates_after_confirm, prints_after_confirm, process_class
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, node.ID, node.WorkflowDefinitionID, node.FunctionID, node.RoleID, node.StepOrder, node.Code, node.Name, node.CanSubmitToManager, node.ValidatesAfterConfirm, node.PrintsAfterConfirm, node.ProcessClass)
	if err != nil {
		return nil, fmt.Errorf("insert workflow node: %w", err)
	}
	return &node, nil
}

func (r *Repository) ListNodesByDefinition(ctx context.Context, definitionID int64) ([]Node, error) {
	return r.listNodesByDefinition(ctx, definitionID)
}

func (r *Repository) CreateTransition(ctx context.Context, tx *sql.Tx, transition Transition) (*Transition, error) {
	if tx == nil {
		return nil, fmt.Errorf("insert workflow transition: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_transitions (id, workflow_definition_id, from_node_id, to_node_id, action)
		VALUES (?, ?, ?, ?, ?)
	`, transition.ID, transition.WorkflowDefinitionID, transition.FromNodeID, transition.ToNodeID, transition.Action)
	if err != nil {
		return nil, fmt.Errorf("insert workflow transition: %w", err)
	}
	return &transition, nil
}

func (r *Repository) DeleteTransitionsByDefinition(ctx context.Context, tx *sql.Tx, definitionID int64) error {
	if tx == nil {
		return fmt.Errorf("delete workflow transitions: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `DELETE FROM workflow_transitions WHERE workflow_definition_id = ?`, definitionID)
	if err != nil {
		return fmt.Errorf("delete workflow transitions: %w", err)
	}
	return nil
}

func (r *Repository) ListTransitionsByDefinition(ctx context.Context, definitionID int64) ([]Transition, error) {
	return r.listTransitionsByDefinition(ctx, definitionID)
}

func (r *Repository) CreateAssignmentRule(ctx context.Context, tx *sql.Tx, rule AssignmentRule) (*AssignmentRule, error) {
	if tx == nil {
		return nil, fmt.Errorf("insert workflow assignment rule: nil transaction")
	}
	result, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_assignment_rules (workflow_node_id, strategy_type, config_json)
		VALUES (?, ?, ?)
	`, rule.WorkflowNodeID, rule.StrategyType, rule.ConfigJSON)
	if err != nil {
		return nil, fmt.Errorf("insert workflow assignment rule: %w", err)
	}
	ruleID, err := result.LastInsertId()
	if err != nil {
		return nil, fmt.Errorf("resolve workflow assignment rule id: %w", err)
	}
	rule.ID = ruleID
	return &rule, nil
}

func (r *Repository) ListAssignmentRulesByNode(ctx context.Context, nodeID int64) ([]AssignmentRule, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, workflow_node_id, strategy_type, config_json
		FROM workflow_assignment_rules
		WHERE workflow_node_id = ?
		ORDER BY id
	`, nodeID)
	if err != nil {
		return nil, fmt.Errorf("query workflow assignment rules: %w", err)
	}
	defer rows.Close()

	rules := make([]AssignmentRule, 0)
	for rows.Next() {
		var rule AssignmentRule
		if err := rows.Scan(&rule.ID, &rule.WorkflowNodeID, &rule.StrategyType, &rule.ConfigJSON); err != nil {
			return nil, fmt.Errorf("scan workflow assignment rule: %w", err)
		}
		rules = append(rules, rule)
	}
	return rules, rows.Err()
}

func (r *Repository) DeleteAssignmentRulesByNode(ctx context.Context, tx *sql.Tx, nodeID int64) error {
	if tx == nil {
		return fmt.Errorf("delete workflow assignment rules: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `DELETE FROM workflow_assignment_rules WHERE workflow_node_id = ?`, nodeID)
	if err != nil {
		return fmt.Errorf("delete workflow assignment rules: %w", err)
	}
	return nil
}

func (r *Repository) DeleteNodesByDefinition(ctx context.Context, tx *sql.Tx, definitionID int64) error {
	if tx == nil {
		return fmt.Errorf("delete workflow nodes: nil transaction")
	}
	rows, err := tx.QueryContext(ctx, `SELECT id FROM workflow_nodes WHERE workflow_definition_id = ?`, definitionID)
	if err != nil {
		return fmt.Errorf("query workflow nodes for delete: %w", err)
	}
	defer rows.Close()

	nodeIDs := make([]int64, 0)
	for rows.Next() {
		var nodeID int64
		if err := rows.Scan(&nodeID); err != nil {
			return fmt.Errorf("scan workflow node for delete: %w", err)
		}
		nodeIDs = append(nodeIDs, nodeID)
	}
	if err := rows.Err(); err != nil {
		return fmt.Errorf("iterate workflow nodes for delete: %w", err)
	}
	for _, nodeID := range nodeIDs {
		if err := r.DeleteAssignmentRulesByNode(ctx, tx, nodeID); err != nil {
			return err
		}
	}
	_, err = tx.ExecContext(ctx, `DELETE FROM workflow_nodes WHERE workflow_definition_id = ?`, definitionID)
	if err != nil {
		return fmt.Errorf("delete workflow nodes: %w", err)
	}
	return nil
}

func (r *Repository) UpdateDefinitionLifecycleStatus(ctx context.Context, tx *sql.Tx, definitionID int64, lifecycleStatus DefinitionLifecycleStatus, publishedAt *time.Time) error {
	if tx == nil {
		return fmt.Errorf("update workflow definition lifecycle status: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `
		UPDATE workflow_definitions
		SET lifecycle_status = ?, published_at = ?, updated_at = CURRENT_TIMESTAMP
		WHERE id = ?
	`, lifecycleStatus, publishedAt, definitionID)
	if err != nil {
		return fmt.Errorf("update workflow definition lifecycle status: %w", err)
	}
	return nil
}

func (r *Repository) UpdateTemplatePublishedVersion(ctx context.Context, tx *sql.Tx, templateID int64, publishedDefinitionID *int64, publishedVersionNumber *int) error {
	if tx == nil {
		return fmt.Errorf("update workflow template publication: nil transaction")
	}
	_, err := tx.ExecContext(ctx, `
		UPDATE workflow_templates
		SET published_definition_id = ?, published_version_number = ?, updated_at = CURRENT_TIMESTAMP
		WHERE id = ?
	`, publishedDefinitionID, publishedVersionNumber, templateID)
	if err != nil {
		return fmt.Errorf("update workflow template publication: %w", err)
	}
	return nil
}

func (r *Repository) InsertDefinitionAudit(ctx context.Context, tx *sql.Tx, record DefinitionAuditRecord) (*DefinitionAuditRecord, error) {
	if tx == nil {
		return nil, fmt.Errorf("insert workflow definition audit: nil transaction")
	}
	result, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_definition_audit (workflow_template_id, workflow_definition_id, action, actor_user_id, details)
		VALUES (?, ?, ?, ?, ?)
	`, record.WorkflowTemplateID, record.WorkflowDefinitionID, record.Action, record.ActorUserID, record.Details)
	if err != nil {
		return nil, fmt.Errorf("insert workflow definition audit: %w", err)
	}
	recordID, err := result.LastInsertId()
	if err != nil {
		return nil, fmt.Errorf("resolve workflow definition audit id: %w", err)
	}
	record.ID = recordID
	record.CreatedAt = r.now()
	return &record, nil
}

func (r *Repository) ListDefinitionAudit(ctx context.Context, templateID int64) ([]DefinitionAuditRecord, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, workflow_template_id, workflow_definition_id, action, actor_user_id, details, created_at
		FROM workflow_definition_audit
		WHERE workflow_template_id = ?
		ORDER BY id
	`, templateID)
	if err != nil {
		return nil, fmt.Errorf("query workflow definition audit: %w", err)
	}
	defer rows.Close()

	records := make([]DefinitionAuditRecord, 0)
	for rows.Next() {
		var record DefinitionAuditRecord
		var definitionID sql.NullInt64
		var details sql.NullString
		if err := rows.Scan(&record.ID, &record.WorkflowTemplateID, &definitionID, &record.Action, &record.ActorUserID, &details, &record.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan workflow definition audit: %w", err)
		}
		if definitionID.Valid {
			record.WorkflowDefinitionID = &definitionID.Int64
		}
		if details.Valid {
			record.Details = &details.String
		}
		records = append(records, record)
	}
	return records, rows.Err()
}

func (r *Repository) NextTemplateID(ctx context.Context, tx *sql.Tx) (int64, error) {
	return r.nextID(ctx, tx, "workflow_templates")
}

func (r *Repository) NextDefinitionID(ctx context.Context, tx *sql.Tx) (int64, error) {
	return r.nextID(ctx, tx, "workflow_definitions")
}

func (r *Repository) NextNodeID(ctx context.Context, tx *sql.Tx) (int64, error) {
	return r.nextID(ctx, tx, "workflow_nodes")
}

func (r *Repository) NextTransitionID(ctx context.Context, tx *sql.Tx) (int64, error) {
	return r.nextID(ctx, tx, "workflow_transitions")
}

func (r *Repository) findTemplate(ctx context.Context, query string, arg any) (*Template, error) {
	row := r.db.QueryRowContext(ctx, query, arg)
	template, err := scanTemplate(row)
	if err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query workflow template: %w", err)
	}
	return template, nil
}

type templateScanner interface {
	Scan(dest ...any) error
}

func scanTemplate(scanner templateScanner) (*Template, error) {
	var template Template
	var publishedDefinitionID sql.NullInt64
	var publishedVersionNumber sql.NullInt64
	if err := scanner.Scan(&template.ID, &template.BusinessGroupID, &template.Code, &template.Name, &template.ProcessClass, &template.Status, &publishedDefinitionID, &publishedVersionNumber); err != nil {
		return nil, err
	}
	if publishedDefinitionID.Valid {
		template.PublishedDefinitionID = &publishedDefinitionID.Int64
	}
	if publishedVersionNumber.Valid {
		value := int(publishedVersionNumber.Int64)
		template.PublishedVersionNumber = &value
	}
	return &template, nil
}

func (r *Repository) nextID(ctx context.Context, tx *sql.Tx, table string) (int64, error) {
	if tx == nil {
		return 0, fmt.Errorf("resolve next id for %s: nil transaction", table)
	}
	switch table {
	case "workflow_templates", "workflow_definitions", "workflow_nodes", "workflow_transitions":
	default:
		return 0, fmt.Errorf("resolve next id for %s: unsupported table", table)
	}
	query := fmt.Sprintf("SELECT COALESCE(MAX(id), 0) + 1 FROM %s", table)
	var nextID int64
	if err := tx.QueryRowContext(ctx, query).Scan(&nextID); err != nil {
		return 0, fmt.Errorf("resolve next id for %s: %w", table, err)
	}
	return nextID, nil
}

func normalizeVoucherType(value string) string {
	trimmed := strings.TrimSpace(value)
	if trimmed == "" {
		return "application"
	}
	return trimmed
}
