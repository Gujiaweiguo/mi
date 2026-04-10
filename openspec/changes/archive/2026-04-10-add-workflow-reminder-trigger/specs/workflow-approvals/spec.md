## MODIFIED Requirements

### Requirement: The system SHALL support reminder-only automation without decision mutation
The workflow subsystem MAY emit reminder records or notifications for pending approval instances based on configured reminder criteria. Reminder automation SHALL be informational only and SHALL NOT auto-approve, auto-reject, or escalate workflow decisions in first release behavior. Reminder eligibility and deduplication SHALL be deterministic for a given reminder key and reminder window. The implementation SHALL persist auditable emitted/skip outcomes and SHALL provide a query path for reminder history diagnostics. A workflow administrator SHALL be able to manually trigger a reminder evaluation run via a dedicated endpoint, and the trigger SHALL return the resulting reminder audit entries. Invalid trigger configuration SHALL be rejected without mutating any workflow decision state.

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
