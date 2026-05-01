## Purpose

Define the configurable approval workflows, idempotent side-effect handling, reminder behavior, and first-release automation boundaries required for Lease and billing documents.

## Requirements
### Requirement: The system SHALL provide configurable approval workflows
The first release SHALL support configurable workflow templates, transitions, assignee resolution, and document-specific approval flows for all supported workflow-backed business objects. Workflow submission and action handling SHALL enforce valid source-state transitions for supported business objects before any downstream side effect is dispatched, and runtime execution SHALL resolve against the published workflow-definition version that was active when the workflow instance was created.

#### Scenario: Multi-step approval completes
- **WHEN** a supported business document is submitted to a configured workflow and all required approvers act successfully
- **THEN** the document SHALL transition through the configured states and finish in an approved state

#### Scenario: Invalid workflow transition is blocked
- **WHEN** a workflow action is requested for a document state that does not allow that transition under the configured flow
- **THEN** the system SHALL reject the action and SHALL preserve the current workflow and document state

#### Scenario: Published workflow applies to supported workflow-backed business objects
- **WHEN** a supported workflow-backed business object is configured to use a published workflow definition
- **THEN** the system SHALL execute approval transitions for that object according to the published workflow template version bound at instance creation time

### Requirement: The system SHALL record workflow audit history and protect side effects from duplication
Workflow actions SHALL be auditable and SHALL dispatch downstream side effects through an idempotent mechanism that prevents duplicate exports, notifications, or output generation from repeated requests. Replayed workflow actions for supported workflow-backed business objects SHALL preserve the current state and SHALL NOT create duplicate workflow side effects. Reminder-only automation for pending instances, when enabled, SHALL follow the same idempotent side-effect constraints.

#### Scenario: Duplicate approval request is safe
- **WHEN** the same approval request is replayed for a workflow action that has already been applied
- **THEN** the system SHALL preserve the existing state and SHALL NOT create duplicate downstream side effects

#### Scenario: Duplicate submission request is safe
- **WHEN** the same workflow submission request is replayed for a supported workflow-backed business object that is already attached to an in-flight or completed workflow instance
- **THEN** the system SHALL preserve the existing workflow linkage and SHALL NOT create a duplicate workflow instance or duplicate downstream side effects

### Requirement: Workflow instances SHALL preserve immutable definition-version binding
Each workflow instance SHALL preserve the published workflow-definition version selected at submission time for the lifetime of that instance. Later publication, deactivation, or rollback actions SHALL affect only future workflow instances.

#### Scenario: New submission binds to the active published version
- **WHEN** an operator submits a supported workflow-backed business object after a template version is published
- **THEN** the created workflow instance SHALL bind to that active published version

#### Scenario: Existing instance remains on its original definition version
- **WHEN** a newer version of the same workflow template is published while an earlier workflow instance is still pending approval
- **THEN** the pending workflow instance SHALL continue using its original definition version for subsequent transitions

#### Scenario: Duplicate reminder run is safe
- **WHEN** a reminder job reprocesses the same pending workflow instance within the same reminder window
- **THEN** the system SHALL avoid duplicate reminder side effects for that reminder key and SHALL preserve existing reminder audit state

### Requirement: The system SHALL exclude timeout and escalation automation from the first release
The first release SHALL support the mandatory approval lifecycle without implementing timeout-driven or escalation-driven workflow automation. The workflow capability SHALL also define a boundary-and-readiness contract for future timeout/escalation automation so that future implementation can be added without weakening existing state-transition, idempotency, and audit guarantees.

#### Scenario: Workflow completes without timeout automation
- **WHEN** a first-release workflow definition is configured and executed
- **THEN** the supported workflow behavior SHALL be limited to explicit user-driven approval actions and SHALL NOT require timeout or escalation automation to be considered complete

#### Scenario: Timeout/escalation remains planning-only in first release
- **WHEN** operators run first-release workflow approvals in production-like conditions
- **THEN** timeout/escalation behavior SHALL NOT execute automatically and SHALL NOT change approval outcomes

### Requirement: The system SHALL define readiness criteria before enabling timeout or escalation automation
Before timeout or escalation automation is enabled in any future release, the workflow capability SHALL define deterministic trigger rules, idempotent replay handling, audit-trail recording for automated actions, and explicit operator recovery controls. Any future implementation SHALL satisfy this readiness contract prior to rollout.

#### Scenario: Future timeout trigger design requires deterministic replay rules
- **WHEN** a future change proposes timeout-triggered workflow actions
- **THEN** the proposed design SHALL define deterministic trigger source, replay-safe idempotency behavior, and duplicate side-effect prevention

#### Scenario: Future escalation design requires audit and operator recovery
- **WHEN** a future change proposes escalation-triggered workflow actions
- **THEN** the proposed design SHALL include auditable automated action records and explicit operator override or recovery paths

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

### Requirement: Rejected workflow instances SHALL be resubmittable from the admin view
The workflow admin view SHALL display a resubmit action button for workflow instances whose status is `rejected`. Clicking the resubmit button SHALL call the existing `resubmitWorkflow(id, { idempotency_key })` API with a client-generated `crypto.randomUUID()` idempotency key. The button SHALL show a loading state while the request is in flight and SHALL NOT appear for instances with any status other than `rejected`.

#### Scenario: Resubmit button appears for rejected instance
- **WHEN** a workflow instance has status `rejected`
- **THEN** the admin view SHALL display a "Resubmit" action button in the actions column for that row

#### Scenario: Resubmit button hidden for non-rejected instance
- **WHEN** a workflow instance has status other than `rejected` (e.g., `pending`, `approved`)
- **THEN** the admin view SHALL NOT display the "Resubmit" action button for that row

#### Scenario: Successful resubmission
- **WHEN** the operator clicks the "Resubmit" button on a rejected instance
- **THEN** the system SHALL call `resubmitWorkflow(id, { idempotency_key: <uuid> })`, show a loading indicator during the request, display a success message on completion, and refresh the instances list

### Requirement: Workflow instance audit history SHALL be viewable from the admin view
The workflow admin view SHALL provide a detail drawer for any workflow instance that displays the full audit history timeline. The drawer SHALL be triggered by clicking an instance row or a "Details" action button. The audit history SHALL be fetched via the existing `getWorkflowAuditHistory(id)` API and rendered as a timeline showing each entry's action, actor, from/to status transition, comment, and timestamp.

#### Scenario: Opening instance detail drawer
- **WHEN** the operator clicks a workflow instance row or the "Details" button
- **THEN** a right-side drawer SHALL open displaying instance metadata and the audit history timeline

#### Scenario: Audit history timeline rendering
- **WHEN** the detail drawer opens for a workflow instance
- **THEN** the system SHALL fetch audit history and render each entry as a timeline node showing action, actor, status transition, comment, and timestamp

#### Scenario: Empty audit history
- **WHEN** the audit history API returns an empty list
- **THEN** the drawer SHALL display a "No audit history" placeholder message

### Requirement: Workflow administrators SHALL be able to trigger manual reminder evaluation
The workflow admin view SHALL provide a "Run Reminders" button in the instances table header area. Clicking the button SHALL show a confirmation dialog, and upon confirmation SHALL call the existing `runReminders()` API.

#### Scenario: Confirmed reminder run executes
- **WHEN** the operator confirms the reminder run dialog
- **THEN** the system SHALL call `runReminders()`, disable the trigger button during the request, and display a success message upon completion

### Requirement: Reminder history SHALL be viewable per workflow instance in the detail drawer
The instance detail drawer SHALL display a reminder history section below the audit timeline. The section SHALL fetch data via the existing `getReminderHistory(instanceId)` API and list each reminder entry.

#### Scenario: Reminder history displayed in drawer
- **WHEN** the detail drawer is open for a workflow instance
- **THEN** the drawer SHALL fetch and display reminder history below the audit timeline

#### Scenario: Empty reminder history
- **WHEN** the reminder history API returns an empty list
- **THEN** the section SHALL display a "No reminder history" placeholder message
