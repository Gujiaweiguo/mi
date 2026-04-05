package workflow

import (
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"sort"
	"strings"
	"time"
)

type Repository struct {
	db  *sql.DB
	now func() time.Time
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db, now: time.Now().UTC}
}

func NewRepositoryWithNowFunc(db *sql.DB, now func() time.Time) *Repository {
	repo := NewRepository(db)
	if now != nil {
		repo.now = now
	}
	return repo
}

func (r *Repository) FindDefinitionByCode(ctx context.Context, code string) (*Definition, error) {
	const definitionQuery = `SELECT id, code, name, process_class FROM workflow_definitions WHERE code = ? AND status = 'active'`
	return r.findDefinition(ctx, definitionQuery, code)
}

func (r *Repository) FindDefinitionByID(ctx context.Context, id int64) (*Definition, error) {
	const definitionQuery = `SELECT id, code, name, process_class FROM workflow_definitions WHERE id = ? AND status = 'active'`
	return r.findDefinition(ctx, definitionQuery, id)
}

func (r *Repository) findDefinition(ctx context.Context, query string, arg any) (*Definition, error) {
	var definition Definition
	if err := r.db.QueryRowContext(ctx, query, arg).Scan(&definition.ID, &definition.Code, &definition.Name, &definition.ProcessClass); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query workflow definition: %w", err)
	}

	nodes, err := r.listNodesByDefinition(ctx, definition.ID)
	if err != nil {
		return nil, err
	}
	transitions, err := r.listTransitionsByDefinition(ctx, definition.ID)
	if err != nil {
		return nil, err
	}
	definition.Nodes = nodes
	definition.Transitions = transitions
	return &definition, nil
}

func (r *Repository) listNodesByDefinition(ctx context.Context, definitionID int64) ([]Node, error) {
	const query = `SELECT id, workflow_definition_id, function_id, role_id, step_order, code, name, can_submit_to_manager, validates_after_confirm, prints_after_confirm, process_class FROM workflow_nodes WHERE workflow_definition_id = ? ORDER BY step_order, id`
	rows, err := r.db.QueryContext(ctx, query, definitionID)
	if err != nil {
		return nil, fmt.Errorf("query workflow nodes: %w", err)
	}
	defer rows.Close()

	nodes := make([]Node, 0)
	for rows.Next() {
		var node Node
		if err := rows.Scan(&node.ID, &node.WorkflowDefinitionID, &node.FunctionID, &node.RoleID, &node.StepOrder, &node.Code, &node.Name, &node.CanSubmitToManager, &node.ValidatesAfterConfirm, &node.PrintsAfterConfirm, &node.ProcessClass); err != nil {
			return nil, fmt.Errorf("scan workflow node: %w", err)
		}
		nodes = append(nodes, node)
	}
	return nodes, rows.Err()
}

func (r *Repository) listTransitionsByDefinition(ctx context.Context, definitionID int64) ([]Transition, error) {
	const query = `SELECT id, workflow_definition_id, from_node_id, to_node_id, action FROM workflow_transitions WHERE workflow_definition_id = ? ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query, definitionID)
	if err != nil {
		return nil, fmt.Errorf("query workflow transitions: %w", err)
	}
	defer rows.Close()

	transitions := make([]Transition, 0)
	for rows.Next() {
		var transition Transition
		if err := rows.Scan(&transition.ID, &transition.WorkflowDefinitionID, &transition.FromNodeID, &transition.ToNodeID, &transition.Action); err != nil {
			return nil, fmt.Errorf("scan workflow transition: %w", err)
		}
		transitions = append(transitions, transition)
	}
	return transitions, rows.Err()
}

func (r *Repository) CreateInstance(ctx context.Context, tx *sql.Tx, definition *Definition, input StartInput, firstNode *Node) (*Instance, error) {
	now := r.now()
	result, err := tx.ExecContext(ctx, `
		INSERT INTO workflow_instances (workflow_definition_id, document_type, document_id, status, current_node_id, current_step_order, current_cycle, version, submitted_by, submitted_at)
		VALUES (?, ?, ?, ?, ?, ?, 1, 1, ?, ?)
	`, definition.ID, input.DocumentType, input.DocumentID, InstanceStatusPending, firstNode.ID, firstNode.StepOrder, input.ActorUserID, now)
	if err != nil {
		return nil, fmt.Errorf("insert workflow instance: %w", err)
	}

	instanceID, err := result.LastInsertId()
	if err != nil {
		return nil, fmt.Errorf("resolve workflow instance id: %w", err)
	}

	currentNodeID := firstNode.ID
	currentStepOrder := firstNode.StepOrder
	return &Instance{ID: instanceID, WorkflowDefinitionID: definition.ID, DocumentType: input.DocumentType, DocumentID: input.DocumentID, Status: InstanceStatusPending, CurrentNodeID: &currentNodeID, CurrentStepOrder: &currentStepOrder, CurrentCycle: 1, Version: 1, SubmittedBy: input.ActorUserID, SubmittedAt: now}, nil
}

func (r *Repository) InsertSteps(ctx context.Context, tx *sql.Tx, instance *Instance, nodes []Node, departmentID int64) error {
	for _, node := range nodes {
		status := StepStatusWaiting
		if instance.CurrentNodeID != nil && node.ID == *instance.CurrentNodeID {
			status = StepStatusPending
		}
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO workflow_instance_steps (workflow_instance_id, workflow_node_id, step_order, cycle, assignee_role_id, assignee_department_id, status)
			VALUES (?, ?, ?, ?, ?, ?, ?)
		`, instance.ID, node.ID, node.StepOrder, instance.CurrentCycle, node.RoleID, departmentID, status); err != nil {
			return fmt.Errorf("insert workflow step instance: %w", err)
		}
	}
	return nil
}

func (r *Repository) FindInstanceByID(ctx context.Context, instanceID int64) (*Instance, error) {
	const query = `SELECT id, workflow_definition_id, document_type, document_id, status, current_node_id, current_step_order, current_cycle, version, submitted_by, submitted_at, completed_at FROM workflow_instances WHERE id = ?`
	var instance Instance
	if err := r.db.QueryRowContext(ctx, query, instanceID).Scan(&instance.ID, &instance.WorkflowDefinitionID, &instance.DocumentType, &instance.DocumentID, &instance.Status, &instance.CurrentNodeID, &instance.CurrentStepOrder, &instance.CurrentCycle, &instance.Version, &instance.SubmittedBy, &instance.SubmittedAt, &instance.CompletedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query workflow instance: %w", err)
	}
	return &instance, nil
}

func (r *Repository) ListInstances(ctx context.Context, filter InstanceFilter) ([]Instance, error) {
	query := `SELECT id, workflow_definition_id, document_type, document_id, status, current_node_id, current_step_order, current_cycle, version, submitted_by, submitted_at, completed_at FROM workflow_instances`
	conditions := make([]string, 0, 3)
	args := make([]any, 0, 3)

	if filter.Status != nil {
		conditions = append(conditions, "status = ?")
		args = append(args, *filter.Status)
	}
	if filter.DocumentType != nil {
		conditions = append(conditions, "document_type = ?")
		args = append(args, *filter.DocumentType)
	}
	if filter.DocumentID != nil {
		conditions = append(conditions, "document_id = ?")
		args = append(args, *filter.DocumentID)
	}
	if len(conditions) > 0 {
		query += " WHERE " + strings.Join(conditions, " AND ")
	}
	query += ` ORDER BY id DESC`

	rows, err := r.db.QueryContext(ctx, query, args...)
	if err != nil {
		return nil, fmt.Errorf("query workflow instances: %w", err)
	}
	defer rows.Close()

	instances := make([]Instance, 0)
	for rows.Next() {
		var instance Instance
		if err := rows.Scan(&instance.ID, &instance.WorkflowDefinitionID, &instance.DocumentType, &instance.DocumentID, &instance.Status, &instance.CurrentNodeID, &instance.CurrentStepOrder, &instance.CurrentCycle, &instance.Version, &instance.SubmittedBy, &instance.SubmittedAt, &instance.CompletedAt); err != nil {
			return nil, fmt.Errorf("scan workflow instance: %w", err)
		}
		instances = append(instances, instance)
	}

	return instances, rows.Err()
}

func (r *Repository) FindInstanceByIDForUpdate(ctx context.Context, tx *sql.Tx, instanceID int64) (*Instance, error) {
	const query = `SELECT id, workflow_definition_id, document_type, document_id, status, current_node_id, current_step_order, current_cycle, version, submitted_by, submitted_at, completed_at FROM workflow_instances WHERE id = ? FOR UPDATE`
	var instance Instance
	if err := tx.QueryRowContext(ctx, query, instanceID).Scan(&instance.ID, &instance.WorkflowDefinitionID, &instance.DocumentType, &instance.DocumentID, &instance.Status, &instance.CurrentNodeID, &instance.CurrentStepOrder, &instance.CurrentCycle, &instance.Version, &instance.SubmittedBy, &instance.SubmittedAt, &instance.CompletedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query workflow instance for update: %w", err)
	}
	return &instance, nil
}

func (r *Repository) FindCurrentStepForUpdate(ctx context.Context, tx *sql.Tx, instanceID int64, cycle int) (*Step, error) {
	const query = `SELECT id, workflow_instance_id, workflow_node_id, step_order, cycle, assignee_role_id, assignee_department_id, assignee_user_id, status, action_comment, acted_by, acted_at FROM workflow_instance_steps WHERE workflow_instance_id = ? AND cycle = ? AND status = 'pending' ORDER BY step_order LIMIT 1 FOR UPDATE`
	var step Step
	if err := tx.QueryRowContext(ctx, query, instanceID, cycle).Scan(&step.ID, &step.WorkflowInstanceID, &step.WorkflowNodeID, &step.StepOrder, &step.Cycle, &step.AssigneeRoleID, &step.AssigneeDepartmentID, &step.AssigneeUserID, &step.Status, &step.ActionComment, &step.ActedBy, &step.ActedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query current workflow step: %w", err)
	}
	return &step, nil
}

func (r *Repository) UpdateStep(ctx context.Context, tx *sql.Tx, stepID int64, status StepStatus, actorID int64, comment string) error {
	now := r.now()
	var commentValue any
	if strings.TrimSpace(comment) != "" {
		commentValue = comment
	}
	_, err := tx.ExecContext(ctx, `UPDATE workflow_instance_steps SET status = ?, action_comment = ?, acted_by = ?, acted_at = ?, updated_at = CURRENT_TIMESTAMP WHERE id = ?`, status, commentValue, actorID, now, stepID)
	if err != nil {
		return fmt.Errorf("update workflow step: %w", err)
	}
	return nil
}

func (r *Repository) ActivateNextCycle(ctx context.Context, tx *sql.Tx, instanceID int64, nextCycle int, nodes []Node, departmentID int64) error {
	for index, node := range nodes {
		status := StepStatusWaiting
		if index == 0 {
			status = StepStatusPending
		}
		if _, err := tx.ExecContext(ctx, `INSERT INTO workflow_instance_steps (workflow_instance_id, workflow_node_id, step_order, cycle, assignee_role_id, assignee_department_id, status) VALUES (?, ?, ?, ?, ?, ?, ?)`, instanceID, node.ID, node.StepOrder, nextCycle, node.RoleID, departmentID, status); err != nil {
			return fmt.Errorf("insert resubmitted workflow step: %w", err)
		}
	}
	return nil
}

func (r *Repository) SetStepPending(ctx context.Context, tx *sql.Tx, instanceID int64, cycle int, nodeID int64) error {
	_, err := tx.ExecContext(ctx, `UPDATE workflow_instance_steps SET status = 'pending', updated_at = CURRENT_TIMESTAMP WHERE workflow_instance_id = ? AND cycle = ? AND workflow_node_id = ?`, instanceID, cycle, nodeID)
	if err != nil {
		return fmt.Errorf("set next workflow step pending: %w", err)
	}
	return nil
}

func (r *Repository) UpdateInstanceState(ctx context.Context, tx *sql.Tx, instanceID int64, status InstanceStatus, currentNodeID *int64, currentStepOrder *int, cycle int, completed bool) error {
	versionQuery := `UPDATE workflow_instances SET status = ?, current_node_id = ?, current_step_order = ?, current_cycle = ?, version = version + 1, updated_at = CURRENT_TIMESTAMP`
	args := []any{status, currentNodeID, currentStepOrder, cycle}
	if completed {
		now := r.now()
		versionQuery += `, completed_at = ?`
		args = append(args, now)
	}
	versionQuery += ` WHERE id = ?`
	args = append(args, instanceID)
	_, err := tx.ExecContext(ctx, versionQuery, args...)
	if err != nil {
		return fmt.Errorf("update workflow instance state: %w", err)
	}
	return nil
}

func (r *Repository) FindAuditByIdempotencyKey(ctx context.Context, tx *sql.Tx, instanceID int64, idempotencyKey string) (*AuditEntry, error) {
	const query = `SELECT id, workflow_instance_id, action, actor_user_id, from_status, to_status, from_step_order, to_step_order, comment, idempotency_key, created_at FROM workflow_audit_history WHERE workflow_instance_id = ? AND idempotency_key = ?`
	var entry AuditEntry
	if err := tx.QueryRowContext(ctx, query, instanceID, idempotencyKey).Scan(&entry.ID, &entry.WorkflowInstanceID, &entry.Action, &entry.ActorUserID, &entry.FromStatus, &entry.ToStatus, &entry.FromStepOrder, &entry.ToStepOrder, &entry.Comment, &entry.IdempotencyKey, &entry.CreatedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query workflow audit by idempotency key: %w", err)
	}
	return &entry, nil
}

func (r *Repository) InsertAudit(ctx context.Context, tx *sql.Tx, entry AuditEntry) error {
	_, err := tx.ExecContext(ctx, `INSERT INTO workflow_audit_history (workflow_instance_id, action, actor_user_id, from_status, to_status, from_step_order, to_step_order, comment, idempotency_key) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`, entry.WorkflowInstanceID, entry.Action, entry.ActorUserID, entry.FromStatus, entry.ToStatus, entry.FromStepOrder, entry.ToStepOrder, entry.Comment, entry.IdempotencyKey)
	if err != nil {
		return fmt.Errorf("insert workflow audit: %w", err)
	}
	return nil
}

func (r *Repository) InsertOutbox(ctx context.Context, tx *sql.Tx, message OutboxMessage) error {
	_, err := tx.ExecContext(ctx, `INSERT INTO outbox_messages (aggregate_type, aggregate_id, event_type, dedupe_key, payload, status, attempt_count) VALUES (?, ?, ?, ?, ?, ?, ?)`, message.AggregateType, message.AggregateID, message.EventType, message.DedupeKey, message.Payload, message.Status, message.AttemptCount)
	if err != nil {
		return fmt.Errorf("insert outbox message: %w", err)
	}
	return nil
}

func (r *Repository) ListAuditHistory(ctx context.Context, instanceID int64) ([]AuditEntry, error) {
	const query = `SELECT id, workflow_instance_id, action, actor_user_id, from_status, to_status, from_step_order, to_step_order, comment, idempotency_key, created_at FROM workflow_audit_history WHERE workflow_instance_id = ? ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query, instanceID)
	if err != nil {
		return nil, fmt.Errorf("query workflow audit history: %w", err)
	}
	defer rows.Close()

	history := make([]AuditEntry, 0)
	for rows.Next() {
		var entry AuditEntry
		if err := rows.Scan(&entry.ID, &entry.WorkflowInstanceID, &entry.Action, &entry.ActorUserID, &entry.FromStatus, &entry.ToStatus, &entry.FromStepOrder, &entry.ToStepOrder, &entry.Comment, &entry.IdempotencyKey, &entry.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan workflow audit history: %w", err)
		}
		history = append(history, entry)
	}
	return history, rows.Err()
}

func (r *Repository) ListOutboxMessages(ctx context.Context, aggregateType string, aggregateID int64) ([]OutboxMessage, error) {
	const query = `SELECT id, aggregate_type, aggregate_id, event_type, dedupe_key, payload, status, attempt_count, created_at FROM outbox_messages WHERE aggregate_type = ? AND aggregate_id = ? ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query, aggregateType, aggregateID)
	if err != nil {
		return nil, fmt.Errorf("query outbox messages: %w", err)
	}
	defer rows.Close()

	messages := make([]OutboxMessage, 0)
	for rows.Next() {
		var message OutboxMessage
		if err := rows.Scan(&message.ID, &message.AggregateType, &message.AggregateID, &message.EventType, &message.DedupeKey, &message.Payload, &message.Status, &message.AttemptCount, &message.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan outbox message: %w", err)
		}
		messages = append(messages, message)
	}
	return messages, rows.Err()
}

func (r *Repository) ListDefinitions(ctx context.Context) ([]Definition, error) {
	const query = `SELECT code FROM workflow_definitions WHERE status = 'active' ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("query workflow definition codes: %w", err)
	}
	defer rows.Close()

	definitions := make([]Definition, 0)
	for rows.Next() {
		var code string
		if err := rows.Scan(&code); err != nil {
			return nil, fmt.Errorf("scan workflow definition code: %w", err)
		}
		definition, err := r.FindDefinitionByCode(ctx, code)
		if err != nil {
			return nil, err
		}
		if definition != nil {
			definitions = append(definitions, *definition)
		}
	}
	return definitions, rows.Err()
}

func MarshalOutboxPayload(instance *Instance, action Action, comment string) (string, error) {
	payload := map[string]any{
		"workflow_instance_id": instance.ID,
		"document_type":        instance.DocumentType,
		"document_id":          instance.DocumentID,
		"action":               action,
		"status":               instance.Status,
	}
	if strings.TrimSpace(comment) != "" {
		payload["comment"] = comment
	}

	bytes, err := json.Marshal(payload)
	if err != nil {
		return "", fmt.Errorf("marshal workflow outbox payload: %w", err)
	}
	return string(bytes), nil
}

func sortNodes(nodes []Node) []Node {
	ordered := append([]Node(nil), nodes...)
	sort.Slice(ordered, func(i, j int) bool {
		if ordered[i].StepOrder == ordered[j].StepOrder {
			return ordered[i].ID < ordered[j].ID
		}
		return ordered[i].StepOrder < ordered[j].StepOrder
	})
	return ordered
}
