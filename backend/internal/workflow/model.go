package workflow

import "time"

type Action string

const (
	ActionSubmit   Action = "submit"
	ActionApprove  Action = "approve"
	ActionReject   Action = "reject"
	ActionResubmit Action = "resubmit"
)

type InstanceStatus string

const (
	InstanceStatusPending  InstanceStatus = "pending"
	InstanceStatusApproved InstanceStatus = "approved"
	InstanceStatusRejected InstanceStatus = "rejected"
)

type StepStatus string

const (
	StepStatusWaiting  StepStatus = "waiting"
	StepStatusPending  StepStatus = "pending"
	StepStatusApproved StepStatus = "approved"
	StepStatusRejected StepStatus = "rejected"
)

type OutboxStatus string

const (
	OutboxStatusPending OutboxStatus = "pending"
)

type Definition struct {
	ID           int64
	Code         string
	Name         string
	ProcessClass string
	Nodes        []Node
	Transitions  []Transition
}

type Node struct {
	ID                    int64
	WorkflowDefinitionID  int64
	FunctionID            int64
	RoleID                int64
	StepOrder             int
	Code                  string
	Name                  string
	CanSubmitToManager    bool
	ValidatesAfterConfirm bool
	PrintsAfterConfirm    bool
	ProcessClass          string
}

type Transition struct {
	ID                   int64
	WorkflowDefinitionID int64
	FromNodeID           *int64
	ToNodeID             int64
	Action               Action
}

type Instance struct {
	ID                   int64          `json:"id"`
	WorkflowDefinitionID int64          `json:"workflow_definition_id"`
	DocumentType         string         `json:"document_type"`
	DocumentID           int64          `json:"document_id"`
	Status               InstanceStatus `json:"status"`
	CurrentNodeID        *int64         `json:"current_node_id"`
	CurrentStepOrder     *int           `json:"current_step_order"`
	CurrentCycle         int            `json:"current_cycle"`
	Version              int            `json:"version"`
	SubmittedBy          int64          `json:"submitted_by"`
	SubmittedAt          time.Time      `json:"submitted_at"`
	CompletedAt          *time.Time     `json:"completed_at"`
}

type Step struct {
	ID                   int64      `json:"id"`
	WorkflowInstanceID   int64      `json:"workflow_instance_id"`
	WorkflowNodeID       int64      `json:"workflow_node_id"`
	StepOrder            int        `json:"step_order"`
	Cycle                int        `json:"cycle"`
	AssigneeRoleID       int64      `json:"assignee_role_id"`
	AssigneeDepartmentID int64      `json:"assignee_department_id"`
	AssigneeUserID       *int64     `json:"assignee_user_id"`
	Status               StepStatus `json:"status"`
	ActionComment        *string    `json:"action_comment"`
	ActedBy              *int64     `json:"acted_by"`
	ActedAt              *time.Time `json:"acted_at"`
}

type AuditEntry struct {
	ID                 int64          `json:"id"`
	WorkflowInstanceID int64          `json:"workflow_instance_id"`
	Action             Action         `json:"action"`
	ActorUserID        int64          `json:"actor_user_id"`
	FromStatus         InstanceStatus `json:"from_status"`
	ToStatus           InstanceStatus `json:"to_status"`
	FromStepOrder      *int           `json:"from_step_order"`
	ToStepOrder        *int           `json:"to_step_order"`
	Comment            *string        `json:"comment"`
	IdempotencyKey     string         `json:"idempotency_key"`
	CreatedAt          time.Time      `json:"created_at"`
}

type OutboxMessage struct {
	ID            int64        `json:"id"`
	AggregateType string       `json:"aggregate_type"`
	AggregateID   int64        `json:"aggregate_id"`
	EventType     string       `json:"event_type"`
	DedupeKey     string       `json:"dedupe_key"`
	Payload       string       `json:"payload"`
	Status        OutboxStatus `json:"status"`
	AttemptCount  int          `json:"attempt_count"`
	CreatedAt     time.Time    `json:"created_at"`
}

type StartInput struct {
	DefinitionCode string
	DocumentType   string
	DocumentID     int64
	ActorUserID    int64
	DepartmentID   int64
	IdempotencyKey string
	Comment        string
}

type InstanceFilter struct {
	Status       *InstanceStatus
	DocumentType *string
	DocumentID   *int64
}

type TransitionInput struct {
	InstanceID     int64
	ActorUserID    int64
	DepartmentID   int64
	IdempotencyKey string
	Comment        string
}

type ReminderOutcome string

const (
	ReminderOutcomeEmitted ReminderOutcome = "emitted"
	ReminderOutcomeSkipped ReminderOutcome = "skipped"
)

type ReminderReasonCode string

const (
	ReminderReasonNotDue         ReminderReasonCode = "not_due"
	ReminderReasonAlreadyEmitted ReminderReasonCode = "already_emitted"
	ReminderReasonNotPending     ReminderReasonCode = "not_pending"
)

type ReminderAuditRecord struct {
	ID                  int64           `json:"id"`
	WorkflowInstanceID  int64           `json:"workflow_instance_id"`
	ReminderType        string          `json:"reminder_type"`
	ReminderKey         string          `json:"reminder_key"`
	ReminderWindowStart time.Time       `json:"reminder_window_start"`
	Outcome             ReminderOutcome `json:"outcome"`
	ReasonCode          *string         `json:"reason_code,omitempty"`
	CreatedAt           time.Time       `json:"created_at"`
}

type ReminderConfig struct {
	ReminderType     string
	MinPendingAge    time.Duration
	WindowTruncation time.Duration
}
