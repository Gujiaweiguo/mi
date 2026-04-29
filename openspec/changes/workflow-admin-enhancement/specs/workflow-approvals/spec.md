## ADDED Requirements

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

#### Scenario: Resubmit with loading state isolation
- **WHEN** the resubmit request is in flight for a specific instance
- **THEN** only the resubmit button for that instance SHALL show a loading state; other rows SHALL remain interactive

### Requirement: Workflow instance audit history SHALL be viewable from the admin view
The workflow admin view SHALL provide a detail drawer for any workflow instance that displays the full audit history timeline. The drawer SHALL be triggered by clicking an instance row or a "Details" action button. The audit history SHALL be fetched via the existing `getWorkflowAuditHistory(id)` API and rendered as a timeline showing each entry's action, actor, from/to status transition, comment, and timestamp.

#### Scenario: Opening instance detail drawer
- **WHEN** the operator clicks a workflow instance row or the "Details" button
- **THEN** a right-side drawer SHALL open displaying instance metadata (document type, document ID, status, created date, updated date) and the audit history timeline

#### Scenario: Audit history timeline rendering
- **WHEN** the detail drawer opens for a workflow instance
- **THEN** the system SHALL fetch audit history via `getWorkflowAuditHistory(id)` and render each `AuditEntry` as a timeline node showing action, actor user ID, from/to status, from/to step order, comment, and created_at timestamp

#### Scenario: Loading state while fetching audit history
- **WHEN** audit history data is being fetched
- **THEN** the drawer SHALL display a loading indicator until data is available

#### Scenario: Empty audit history
- **WHEN** the audit history API returns an empty list
- **THEN** the drawer SHALL display a "No audit history" placeholder message

### Requirement: Workflow administrators SHALL be able to trigger manual reminder evaluation
The workflow admin view SHALL provide a "Run Reminders" button in the instances table header area. Clicking the button SHALL show a confirmation dialog, and upon confirmation SHALL call the existing `runReminders()` API. The button SHALL be disabled while a request is in flight and SHALL display success or error feedback upon completion.

#### Scenario: Manual reminder trigger button visible
- **WHEN** the workflow admin view is displayed
- **THEN** a "Run Reminders" button SHALL be visible in the instances table header area

#### Scenario: Manual reminder trigger with confirmation
- **WHEN** the operator clicks the "Run Reminders" button
- **THEN** a confirmation dialog SHALL appear asking the operator to confirm the reminder run

#### Scenario: Confirmed reminder run executes
- **WHEN** the operator confirms the reminder run dialog
- **THEN** the system SHALL call `runReminders()`, disable the trigger button during the request, and display a success message with result summary upon completion

#### Scenario: Reminder run button disabled during request
- **WHEN** a reminder run request is in flight
- **THEN** the "Run Reminders" button SHALL be disabled and show a loading indicator

### Requirement: Reminder history SHALL be viewable per workflow instance in the detail drawer
The instance detail drawer SHALL display a reminder history section below the audit timeline. The section SHALL fetch data via the existing `getReminderHistory(instanceId)` API and list each reminder entry. The section MAY be collapsible to reduce visual clutter.

#### Scenario: Reminder history displayed in drawer
- **WHEN** the detail drawer is open for a workflow instance
- **THEN** the drawer SHALL fetch reminder history via `getReminderHistory(instanceId)` and display the results below the audit timeline

#### Scenario: Reminder history loading state
- **WHEN** reminder history data is being fetched
- **THEN** the section SHALL display a loading indicator until data is available

#### Scenario: Empty reminder history
- **WHEN** the reminder history API returns an empty list
- **THEN** the section SHALL display a "No reminder history" placeholder message
