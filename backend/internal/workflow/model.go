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

type DefinitionLifecycleStatus string

const (
	DefinitionLifecycleStatusDraft       DefinitionLifecycleStatus = "draft"
	DefinitionLifecycleStatusValidated   DefinitionLifecycleStatus = "validated"
	DefinitionLifecycleStatusPublished   DefinitionLifecycleStatus = "published"
	DefinitionLifecycleStatusSuperseded  DefinitionLifecycleStatus = "superseded"
	DefinitionLifecycleStatusDeactivated DefinitionLifecycleStatus = "deactivated"
)

type AssignmentStrategyType string

const (
	AssignmentStrategyFixedUser      AssignmentStrategyType = "fixed_user"
	AssignmentStrategyFixedRole      AssignmentStrategyType = "fixed_role"
	AssignmentStrategyDepartmentLead AssignmentStrategyType = "department_leader"
	AssignmentStrategySubmitter      AssignmentStrategyType = "submitter_context"
	AssignmentStrategyDocumentField  AssignmentStrategyType = "document_field"
)

type DefinitionAuditAction string

const (
	DefinitionAuditActionCreateDraft DefinitionAuditAction = "create_draft"
	DefinitionAuditActionEditDraft   DefinitionAuditAction = "edit_draft"
	DefinitionAuditActionValidate    DefinitionAuditAction = "validate"
	DefinitionAuditActionPublish     DefinitionAuditAction = "publish"
	DefinitionAuditActionDeactivate  DefinitionAuditAction = "deactivate"
	DefinitionAuditActionRollback    DefinitionAuditAction = "rollback"
)

type Definition struct {
	ID                 int64
	BusinessGroupID    int64
	WorkflowTemplateID int64
	Code               string
	Name               string
	VoucherType        string
	IsInitial          bool
	Status             string
	ProcessClass       string
	VersionNumber      int
	LifecycleStatus    string
	PublishedAt        *time.Time
	TransitionsEnabled bool
	Nodes              []Node
	Transitions        []Transition
}

type Template struct {
	ID                     int64  `json:"id"`
	BusinessGroupID        int64  `json:"business_group_id"`
	Code                   string `json:"code"`
	Name                   string `json:"name"`
	ProcessClass           string `json:"process_class"`
	Status                 string `json:"status"`
	PublishedDefinitionID  *int64 `json:"published_definition_id,omitempty"`
	PublishedVersionNumber *int   `json:"published_version_number,omitempty"`
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
	AssignmentRules       []AssignmentRule
}

type Transition struct {
	ID                   int64
	WorkflowDefinitionID int64
	FromNodeID           *int64
	ToNodeID             int64
	Action               Action
}

type AssignmentRule struct {
	ID             int64                  `json:"id"`
	WorkflowNodeID int64                  `json:"workflow_node_id"`
	StrategyType   AssignmentStrategyType `json:"strategy_type"`
	ConfigJSON     string                 `json:"config_json"`
}

type DefinitionAuditRecord struct {
	ID                   int64                 `json:"id"`
	WorkflowTemplateID   int64                 `json:"workflow_template_id"`
	WorkflowDefinitionID *int64                `json:"workflow_definition_id,omitempty"`
	Action               DefinitionAuditAction `json:"action"`
	ActorUserID          int64                 `json:"actor_user_id"`
	Details              *string               `json:"details,omitempty"`
	CreatedAt            time.Time             `json:"created_at"`
}

type CreateTemplateInput struct {
	BusinessGroupID int64
	Code            string
	Name            string
	ProcessClass    string
	VoucherType     string
	ActorUserID     int64
}

type CreateDraftInput struct {
	TemplateID     int64
	ActorUserID    int64
	VoucherType    string
	DefinitionName string
}

type UpdateDraftDefinitionInput struct {
	DefinitionID int64
	ActorUserID  int64
	Name         string
	VoucherType  string
	IsInitial    bool
	Nodes        []DefinitionNodeInput
	Transitions  []DefinitionTransitionInput
}

type DefinitionNodeInput struct {
	FunctionID            int64
	RoleID                int64
	StepOrder             int
	Code                  string
	Name                  string
	CanSubmitToManager    bool
	ValidatesAfterConfirm bool
	PrintsAfterConfirm    bool
	ProcessClass          string
	AssignmentRules       []AssignmentRuleInput
}

type DefinitionTransitionInput struct {
	FromNodeCode *string
	ToNodeCode   string
	Action       Action
}

type AssignmentRuleInput struct {
	StrategyType AssignmentStrategyType
	ConfigJSON   string
}

type PublishDefinitionInput struct {
	DefinitionID int64
	ActorUserID  int64
}

type DeactivateTemplateInput struct {
	TemplateID  int64
	ActorUserID int64
}

type RollbackTemplateInput struct {
	TemplateID   int64
	DefinitionID int64
	ActorUserID  int64
}

type DefinitionValidationIssue struct {
	Code    string `json:"code"`
	Field   string `json:"field,omitempty"`
	Message string `json:"message"`
}

type DefinitionValidationResult struct {
	Valid  bool                        `json:"valid"`
	Issues []DefinitionValidationIssue `json:"issues"`
}

type TemplateDraft struct {
	Template   *Template   `json:"template"`
	Definition *Definition `json:"definition"`
}

type Instance struct {
	ID                        int64          `json:"id"`
	WorkflowDefinitionID      int64          `json:"workflow_definition_id"`
	WorkflowTemplateID        int64          `json:"workflow_template_id"`
	WorkflowDefinitionVersion int            `json:"workflow_definition_version"`
	DocumentType              string         `json:"document_type"`
	DocumentID                int64          `json:"document_id"`
	Status                    InstanceStatus `json:"status"`
	CurrentNodeID             *int64         `json:"current_node_id"`
	CurrentStepOrder          *int           `json:"current_step_order"`
	CurrentCycle              int            `json:"current_cycle"`
	Version                   int            `json:"version"`
	SubmittedBy               int64          `json:"submitted_by"`
	SubmittedAt               time.Time      `json:"submitted_at"`
	CompletedAt               *time.Time     `json:"completed_at"`
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
