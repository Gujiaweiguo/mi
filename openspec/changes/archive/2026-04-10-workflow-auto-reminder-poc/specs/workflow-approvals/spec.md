## MODIFIED Requirements

### Requirement: The system SHALL record workflow audit history and protect side effects from duplication
Workflow actions SHALL be auditable and SHALL dispatch downstream side effects through an idempotent mechanism that prevents duplicate exports, notifications, or output generation from repeated requests. Replayed Lease and billing-document workflow actions SHALL preserve the current state and SHALL NOT create duplicate workflow side effects. Reminder-only automation for pending instances, when enabled, SHALL follow the same idempotent side-effect constraints.

#### Scenario: Duplicate approval request is safe
- **WHEN** the same approval request is replayed for a workflow action that has already been applied
- **THEN** the system SHALL preserve the existing state and SHALL NOT create duplicate downstream side effects

#### Scenario: Duplicate submission request is safe
- **WHEN** the same workflow submission request is replayed for a Lease or billing document that is already attached to an in-flight or completed workflow instance
- **THEN** the system SHALL preserve the existing workflow linkage and SHALL NOT create a duplicate workflow instance or duplicate downstream side effects

#### Scenario: Duplicate reminder run is safe
- **WHEN** a reminder job reprocesses the same pending workflow instance within the same reminder window
- **THEN** the system SHALL avoid duplicate reminder side effects for that reminder key and SHALL preserve existing reminder audit state

## ADDED Requirements

### Requirement: The system SHALL support reminder-only automation without decision mutation
The workflow subsystem MAY emit reminder records or notifications for pending approval instances based on configured reminder criteria. Reminder automation SHALL be informational only and SHALL NOT auto-approve, auto-reject, or escalate workflow decisions in first release behavior. Reminder eligibility and deduplication SHALL be deterministic for a given reminder key and reminder window.

#### Scenario: Reminder emission for pending instance
- **WHEN** a workflow instance remains pending and satisfies configured reminder criteria
- **THEN** the system SHALL emit a reminder record or reminder notification entry linked to that instance

#### Scenario: Reminder skip is auditable for diagnostics
- **WHEN** a workflow instance is evaluated by reminder automation but does not satisfy reminder criteria (or was already reminded for the same reminder key/window)
- **THEN** the system SHALL persist a traceable skip record with reason codes suitable for operator diagnostics

#### Scenario: Reminder automation does not mutate approval state
- **WHEN** reminder automation runs for a pending instance
- **THEN** the workflow decision state SHALL remain unchanged unless an explicit user action is submitted through normal approval transitions
