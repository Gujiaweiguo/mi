## MODIFIED Requirements

### Requirement: The system SHALL provide configurable approval workflows
The first release SHALL support configurable workflow templates, transitions, assignee resolution, and document-specific approval flows for supported business objects. Workflow submission and action handling SHALL enforce valid source-state transitions for supported Lease and billing documents before any downstream side effect is dispatched. When a workflow is submitted or advances to a new approval step and email notifications are enabled, the system SHALL enqueue a `workflow.approval_requested` notification event addressed to the approvers at the current workflow node. The notification event SHALL be enqueued within the same database transaction as the workflow state change. If email is disabled, the workflow SHALL proceed without notification side effects.

#### Scenario: Multi-step approval completes
- **WHEN** a supported business document is submitted to a configured workflow and all required approvers act successfully
- **THEN** the document SHALL transition through the configured states and finish in an approved state

#### Scenario: Invalid workflow transition is blocked
- **WHEN** a workflow action is requested for a document state that does not allow that transition under the configured flow
- **THEN** the system SHALL reject the action and SHALL preserve the current workflow and document state

#### Scenario: Workflow submission triggers approval notification
- **WHEN** a workflow is submitted to the first approval step and email notifications are enabled
- **THEN** the system SHALL enqueue a `workflow.approval_requested` notification within the same transaction, with recipient addresses resolved from the approver roles assigned to the current workflow node

#### Scenario: Workflow advance to next step triggers notification
- **WHEN** an approver approves the current step and the workflow advances to a subsequent approval step and email is enabled
- **THEN** the system SHALL enqueue a `workflow.approval_requested` notification addressed to the approvers at the new step

#### Scenario: Notification enqueue failure does not block workflow transition
- **WHEN** a workflow transition succeeds but the notification enqueue call fails
- **THEN** the system SHALL log the notification error and the workflow state change SHALL still be committed; the notification failure SHALL NOT roll back the workflow transition

### Requirement: The system SHALL support reminder-only automation without decision mutation
The workflow subsystem MAY emit reminder records or notifications for pending approval instances based on configured reminder criteria. Reminder automation SHALL be informational only and SHALL NOT auto-approve, auto-reject, or escalate workflow decisions in first release behavior. Reminder eligibility and deduplication SHALL be deterministic for a given reminder key and reminder window. The implementation SHALL persist auditable emitted/skip outcomes and SHALL provide a query path for reminder history diagnostics. A workflow administrator SHALL be able to manually trigger a reminder evaluation run via a dedicated endpoint, and the trigger SHALL return the resulting reminder audit entries. Invalid trigger configuration SHALL be rejected without mutating any workflow decision state. The backend SHALL also support periodic scheduled invocation of reminder evaluation using explicit runtime configuration. Scheduled invocation SHALL enforce single active scheduler ownership across replicas using distributed locking. Scheduled invocation SHALL emit structured observability signals describing per-tick outcomes and cumulative scheduler health trends.

#### Scenario: Reminder emission for pending instance
- **WHEN** a workflow instance remains pending and satisfies configured reminder criteria
- **THEN** the system SHALL emit a reminder record or reminder notification entry linked to that instance

#### Scenario: Reminder skip is auditable for diagnostics
- **WHEN** a workflow instance is evaluated by reminder automation but does not satisfy reminder criteria (or was already reminded for the same reminder key/window)
- **THEN** the system SHALL persist a traceable skip record with reason codes suitable for operator diagnostics

#### Scenario: Reminder automation does not mutate approval state
- **WHEN** reminder automation runs for a pending instance
- **THEN** the workflow decision state SHALL remain unchanged unless an explicit user action is submitted through normal approval transitions

#### Scenario: Reminder replay does not duplicate side effects
- **WHEN** the reminder runner is retried for the same instance and reminder key/window
- **THEN** the system SHALL preserve a single emitted reminder side effect and SHALL record replay-safe audit outcome without duplication

#### Scenario: Operator can query reminder history
- **WHEN** an operator requests reminder diagnostics for a workflow instance
- **THEN** the system SHALL return reminder history entries including outcome, reason code, and timestamps

#### Scenario: Workflow admin can manually trigger a reminder run
- **WHEN** a workflow administrator submits a manual reminder trigger request and is authorized with the workflow admin approve permission
- **THEN** the system SHALL invoke the reminder evaluation runner against all pending workflow instances and SHALL return the resulting reminder audit entries

#### Scenario: Manual reminder trigger returns audit entries
- **WHEN** the manually triggered reminder run completes
- **THEN** the system SHALL return a list of reminder audit records, each containing instance identifier, reminder type, reminder key, window start, outcome, reason code, and creation timestamp, and an empty list SHALL be returned when no pending instances qualify

#### Scenario: Invalid reminder trigger configuration is rejected without state mutation
- **WHEN** a manual reminder trigger request contains malformed JSON or negative duration field values
- **THEN** the system SHALL reject the request with a validation error and SHALL NOT invoke the reminder runner or mutate any workflow decision state

#### Scenario: Scheduled reminder runner invokes existing evaluation path
- **WHEN** workflow reminder scheduler is enabled and a configured interval elapses
- **THEN** the backend SHALL invoke reminder evaluation against pending workflow instances using the same reminder service semantics as manual trigger execution

#### Scenario: Scheduled run does not mutate workflow decision state
- **WHEN** a scheduled reminder execution completes successfully or with skip outcomes
- **THEN** workflow decision state SHALL remain unchanged unless explicit approval actions are submitted through normal transitions

#### Scenario: Scheduled run failure is recoverable
- **WHEN** a scheduled reminder execution fails due to transient runtime or database error
- **THEN** the system SHALL record failure in operational logs and SHALL continue attempting subsequent scheduled intervals

#### Scenario: Scheduler can be disabled by configuration
- **WHEN** scheduler configuration is disabled
- **THEN** no automatic reminder run SHALL execute and manual trigger behavior SHALL remain available

#### Scenario: Only one replica executes scheduled reminder run
- **WHEN** multiple backend replicas have workflow reminder scheduler enabled for the same lock name
- **THEN** at most one replica SHALL acquire scheduler ownership and execute reminder evaluation for a given tick window

#### Scenario: Replica without lock skips run safely
- **WHEN** a replica fails to acquire scheduler ownership lock
- **THEN** it SHALL skip reminder execution for that tick and SHALL NOT mutate workflow decision state

#### Scenario: Lock acquisition failure is recoverable
- **WHEN** scheduler lock acquisition or release encounters transient database errors
- **THEN** the scheduler SHALL log the failure and continue evaluating future ticks

#### Scenario: Scheduled reminder run logs structured success telemetry
- **WHEN** a scheduled reminder run executes and completes successfully
- **THEN** the scheduler SHALL log run timestamp, duration, emitted/skipped counts, and cumulative success/failure/skip totals

#### Scenario: Scheduled reminder lock-skip is observable
- **WHEN** a scheduler tick cannot acquire ownership lock
- **THEN** the scheduler SHALL log a structured lock-skip outcome and update cumulative lock-skip count without recording a failure

#### Scenario: Consecutive scheduler failures trigger warning signal
- **WHEN** scheduled reminder runs fail in consecutive ticks beyond the configured warning threshold
- **THEN** the scheduler SHALL emit a warning-level observability signal including consecutive failure count and last failure context
