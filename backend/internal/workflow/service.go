package workflow

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"strings"
	"time"
)

var (
	ErrDefinitionNotFound = errors.New("workflow definition not found")
	ErrInvalidState       = errors.New("invalid workflow state")
)

type Service struct {
	repository *Repository
	db         *sql.DB
}

func NewService(db *sql.DB, repository *Repository) *Service {
	return &Service{db: db, repository: repository}
}

func (s *Service) ListDefinitions(ctx context.Context) ([]Definition, error) {
	return s.repository.ListDefinitions(ctx)
}

func (s *Service) ListInstances(ctx context.Context, filter InstanceFilter) ([]Instance, error) {
	return s.repository.ListInstances(ctx, filter)
}

func (s *Service) Start(ctx context.Context, input StartInput) (*Instance, error) {
	definition, err := s.repository.FindDefinitionByCode(ctx, input.DefinitionCode)
	if err != nil {
		return nil, err
	}
	if definition == nil {
		return nil, ErrDefinitionNotFound
	}
	nodes := sortNodes(definition.Nodes)
	if len(nodes) == 0 {
		return nil, ErrInvalidState
	}
	firstNode := nodes[0]

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow start transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	instance, err := s.repository.CreateInstance(ctx, tx, definition, input, &firstNode)
	if err != nil {
		return nil, err
	}
	if err := s.repository.InsertSteps(ctx, tx, instance, nodes, input.DepartmentID); err != nil {
		return nil, err
	}

	stepOrder := firstNode.StepOrder
	if err := s.repository.InsertAudit(ctx, tx, AuditEntry{WorkflowInstanceID: instance.ID, Action: ActionSubmit, ActorUserID: input.ActorUserID, FromStatus: InstanceStatusPending, ToStatus: InstanceStatusPending, FromStepOrder: nil, ToStepOrder: &stepOrder, Comment: stringPointer(input.Comment), IdempotencyKey: input.IdempotencyKey}); err != nil {
		return nil, err
	}
	payload, err := MarshalOutboxPayload(instance, ActionSubmit, input.Comment)
	if err != nil {
		return nil, err
	}
	if err := s.repository.InsertOutbox(ctx, tx, OutboxMessage{AggregateType: "workflow_instance", AggregateID: instance.ID, EventType: "workflow.submitted", DedupeKey: outboxKey(instance.ID, ActionSubmit, input.IdempotencyKey), Payload: payload, Status: OutboxStatusPending, AttemptCount: 0}); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow start transaction: %w", err)
	}
	return s.repository.FindInstanceByID(ctx, instance.ID)
}

func (s *Service) Approve(ctx context.Context, input TransitionInput) (*Instance, error) {
	return s.transition(ctx, ActionApprove, input)
}

func (s *Service) Reject(ctx context.Context, input TransitionInput) (*Instance, error) {
	return s.transition(ctx, ActionReject, input)
}

func (s *Service) Resubmit(ctx context.Context, input TransitionInput) (*Instance, error) {
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow resubmit transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	instance, err := s.repository.FindInstanceByIDForUpdate(ctx, tx, input.InstanceID)
	if err != nil {
		return nil, err
	}
	if instance == nil {
		return nil, ErrInvalidState
	}
	if duplicate, err := s.repository.FindAuditByIdempotencyKey(ctx, tx, input.InstanceID, input.IdempotencyKey); err != nil {
		return nil, err
	} else if duplicate != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate workflow resubmit transaction: %w", err)
		}
		return s.repository.FindInstanceByID(ctx, input.InstanceID)
	}
	if instance.Status != InstanceStatusRejected {
		return nil, ErrInvalidState
	}

	definition, err := s.repository.FindDefinitionByID(ctx, instance.WorkflowDefinitionID)
	if err != nil {
		return nil, err
	}
	if definition == nil {
		return nil, ErrDefinitionNotFound
	}
	nodes := sortNodes(definition.Nodes)
	if len(nodes) == 0 {
		return nil, ErrInvalidState
	}
	firstNode := nodes[0]
	nextCycle := instance.CurrentCycle + 1
	if err := s.repository.ActivateNextCycle(ctx, tx, instance.ID, nextCycle, nodes, input.DepartmentID); err != nil {
		return nil, err
	}
	stepOrder := firstNode.StepOrder
	currentNodeID := firstNode.ID
	if err := s.repository.UpdateInstanceState(ctx, tx, instance.ID, InstanceStatusPending, &currentNodeID, &stepOrder, nextCycle, false); err != nil {
		return nil, err
	}
	if err := s.repository.InsertAudit(ctx, tx, AuditEntry{WorkflowInstanceID: instance.ID, Action: ActionResubmit, ActorUserID: input.ActorUserID, FromStatus: InstanceStatusRejected, ToStatus: InstanceStatusPending, FromStepOrder: instance.CurrentStepOrder, ToStepOrder: &stepOrder, Comment: stringPointer(input.Comment), IdempotencyKey: input.IdempotencyKey}); err != nil {
		return nil, err
	}
	payload, err := MarshalOutboxPayload(instance, ActionResubmit, input.Comment)
	if err != nil {
		return nil, err
	}
	if err := s.repository.InsertOutbox(ctx, tx, OutboxMessage{AggregateType: "workflow_instance", AggregateID: instance.ID, EventType: "workflow.resubmitted", DedupeKey: outboxKey(instance.ID, ActionResubmit, input.IdempotencyKey), Payload: payload, Status: OutboxStatusPending}); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow resubmit transaction: %w", err)
	}
	return s.repository.FindInstanceByID(ctx, instance.ID)
}

func (s *Service) transition(ctx context.Context, action Action, input TransitionInput) (*Instance, error) {
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow transition transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	instance, err := s.repository.FindInstanceByIDForUpdate(ctx, tx, input.InstanceID)
	if err != nil {
		return nil, err
	}
	if instance == nil || instance.Status != InstanceStatusPending {
		return nil, ErrInvalidState
	}

	if duplicate, err := s.repository.FindAuditByIdempotencyKey(ctx, tx, input.InstanceID, input.IdempotencyKey); err != nil {
		return nil, err
	} else if duplicate != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate workflow transition transaction: %w", err)
		}
		return s.repository.FindInstanceByID(ctx, input.InstanceID)
	}

	step, err := s.repository.FindCurrentStepForUpdate(ctx, tx, input.InstanceID, instance.CurrentCycle)
	if err != nil {
		return nil, err
	}
	if step == nil {
		return nil, ErrInvalidState
	}

	fromStatus := instance.Status
	fromStepOrder := step.StepOrder
	stepStatus := StepStatusApproved
	nextStatus := InstanceStatusPending
	nextStepOrder := (*int)(nil)
	nextNodeID := (*int64)(nil)
	completed := false
	if action == ActionReject {
		stepStatus = StepStatusRejected
		nextStatus = InstanceStatusRejected
		completed = true
	} else {
		definition, err := s.repository.FindDefinitionByID(ctx, instance.WorkflowDefinitionID)
		if err != nil {
			return nil, err
		}
		if definition == nil {
			return nil, ErrDefinitionNotFound
		}
		if transition := nextTransition(definition.Transitions, step.WorkflowNodeID, ActionApprove); transition != nil {
			nodes := indexNodes(definition.Nodes)
			nextNode := nodes[transition.ToNodeID]
			nextStepOrder = &nextNode.StepOrder
			nodeID := nextNode.ID
			nextNodeID = &nodeID
			if err := s.repository.SetStepPending(ctx, tx, instance.ID, instance.CurrentCycle, nextNode.ID); err != nil {
				return nil, err
			}
		} else {
			nextStatus = InstanceStatusApproved
			completed = true
		}
	}

	if err := s.repository.UpdateStep(ctx, tx, step.ID, stepStatus, input.ActorUserID, input.Comment); err != nil {
		return nil, err
	}
	if err := s.repository.UpdateInstanceState(ctx, tx, instance.ID, nextStatus, nextNodeID, nextStepOrder, instance.CurrentCycle, completed); err != nil {
		return nil, err
	}
	if err := s.repository.InsertAudit(ctx, tx, AuditEntry{WorkflowInstanceID: instance.ID, Action: action, ActorUserID: input.ActorUserID, FromStatus: fromStatus, ToStatus: nextStatus, FromStepOrder: &fromStepOrder, ToStepOrder: nextStepOrder, Comment: stringPointer(input.Comment), IdempotencyKey: input.IdempotencyKey}); err != nil {
		return nil, err
	}
	updatedInstance := *instance
	updatedInstance.Status = nextStatus
	updatedInstance.CurrentNodeID = nextNodeID
	updatedInstance.CurrentStepOrder = nextStepOrder
	if completed {
		now := timeNowUTC()
		updatedInstance.CompletedAt = &now
	}
	payload, err := MarshalOutboxPayload(&updatedInstance, action, input.Comment)
	if err != nil {
		return nil, err
	}
	eventType := "workflow.approved"
	if action == ActionReject {
		eventType = "workflow.rejected"
	} else if completed {
		eventType = "workflow.completed"
	}
	if err := s.repository.InsertOutbox(ctx, tx, OutboxMessage{AggregateType: "workflow_instance", AggregateID: instance.ID, EventType: eventType, DedupeKey: outboxKey(instance.ID, action, input.IdempotencyKey), Payload: payload, Status: OutboxStatusPending}); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow transition transaction: %w", err)
	}
	return s.repository.FindInstanceByID(ctx, input.InstanceID)
}

func (s *Service) GetInstance(ctx context.Context, instanceID int64) (*Instance, error) {
	return s.repository.FindInstanceByID(ctx, instanceID)
}

func (s *Service) AuditHistory(ctx context.Context, instanceID int64) ([]AuditEntry, error) {
	return s.repository.ListAuditHistory(ctx, instanceID)
}

func (s *Service) OutboxMessages(ctx context.Context, instanceID int64) ([]OutboxMessage, error) {
	return s.repository.ListOutboxMessages(ctx, "workflow_instance", instanceID)
}

func (s *Service) RunReminders(ctx context.Context, now time.Time, config ReminderConfig) ([]ReminderAuditRecord, error) {
	config = normalizeReminderConfig(config)
	windowStart := now.UTC().Truncate(config.WindowTruncation)
	pendingInstances, err := s.repository.FindPendingInstances(ctx)
	if err != nil {
		return nil, err
	}

	records := make([]ReminderAuditRecord, 0, len(pendingInstances))
	for _, pendingInstance := range pendingInstances {
		reminderKey := reminderAuditKey(pendingInstance.ID, config.ReminderType, windowStart)

		tx, err := s.db.BeginTx(ctx, nil)
		if err != nil {
			return nil, fmt.Errorf("begin workflow reminder transaction: %w", err)
		}

		var record ReminderAuditRecord
		func() {
			defer func() { _ = tx.Rollback() }()

			currentInstance, currentErr := s.repository.FindInstanceByIDForUpdate(ctx, tx, pendingInstance.ID)
			if currentErr != nil {
				err = currentErr
				return
			}

			record = ReminderAuditRecord{
				WorkflowInstanceID:  pendingInstance.ID,
				ReminderType:        config.ReminderType,
				ReminderKey:         reminderKey,
				ReminderWindowStart: windowStart,
			}

			if currentInstance == nil || currentInstance.Status != InstanceStatusPending {
				record.Outcome = ReminderOutcomeSkipped
				record.ReasonCode = reminderReasonPointer(ReminderReasonNotPending)
			} else if existing, currentErr := s.repository.FindReminderAuditByKey(ctx, tx, reminderKey); currentErr != nil {
				err = currentErr
				return
			} else if existing != nil {
				record.Outcome = ReminderOutcomeSkipped
				record.ReasonCode = reminderReasonPointer(ReminderReasonAlreadyEmitted)
			} else if now.UTC().Sub(currentInstance.SubmittedAt.UTC()) < config.MinPendingAge {
				record.Outcome = ReminderOutcomeSkipped
				record.ReasonCode = reminderReasonPointer(ReminderReasonNotDue)
			} else {
				record.Outcome = ReminderOutcomeEmitted
			}

			if currentErr := s.repository.InsertReminderAudit(ctx, tx, record); currentErr != nil {
				err = currentErr
				return
			}
			if currentErr := tx.Commit(); currentErr != nil {
				err = fmt.Errorf("commit workflow reminder transaction: %w", currentErr)
				return
			}
		}()
		if err != nil {
			return nil, err
		}
		records = append(records, record)
	}

	return records, nil
}

func (s *Service) ReminderHistory(ctx context.Context, instanceID int64) ([]ReminderAuditRecord, error) {
	return s.repository.ListReminderHistory(ctx, instanceID)
}

func nextTransition(transitions []Transition, fromNodeID int64, action Action) *Transition {
	for _, transition := range transitions {
		if transition.Action == action && transition.FromNodeID != nil && *transition.FromNodeID == fromNodeID {
			transitionCopy := transition
			return &transitionCopy
		}
	}
	return nil
}

func indexNodes(nodes []Node) map[int64]Node {
	indexed := make(map[int64]Node, len(nodes))
	for _, node := range nodes {
		indexed[node.ID] = node
	}
	return indexed
}

func outboxKey(instanceID int64, action Action, idempotencyKey string) string {
	return fmt.Sprintf("workflow:%d:%s:%s", instanceID, action, idempotencyKey)
}

func stringPointer(value string) *string {
	trimmed := strings.TrimSpace(value)
	if trimmed == "" {
		return nil
	}
	return &trimmed
}

func timeNowUTC() time.Time {
	return time.Now().UTC()
}

func normalizeReminderConfig(config ReminderConfig) ReminderConfig {
	if strings.TrimSpace(config.ReminderType) == "" {
		config.ReminderType = "standard"
	}
	if config.WindowTruncation <= 0 {
		config.WindowTruncation = 24 * time.Hour
	}
	if config.MinPendingAge < 0 {
		config.MinPendingAge = 0
	}
	return config
}

func reminderAuditKey(instanceID int64, reminderType string, windowStart time.Time) string {
	return fmt.Sprintf("reminder:%d:%s:%s", instanceID, reminderType, windowStart.UTC().Format(time.RFC3339))
}

func reminderReasonPointer(reason ReminderReasonCode) *string {
	value := string(reason)
	return &value
}
