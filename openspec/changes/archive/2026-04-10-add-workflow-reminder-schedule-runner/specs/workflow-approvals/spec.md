## MODIFIED Requirements

### Requirement: The system SHALL support reminder-only automation without decision mutation
The workflow subsystem MAY emit reminder records or notifications for pending approval instances based on configured reminder criteria. Reminder automation SHALL be informational only and SHALL NOT auto-approve, auto-reject, or escalate workflow decisions in first release behavior. Reminder eligibility and deduplication SHALL be deterministic for a given reminder key and reminder window. The implementation SHALL persist auditable emitted/skip outcomes and SHALL provide a query path for reminder history diagnostics. A workflow administrator SHALL be able to manually trigger a reminder evaluation run via a dedicated endpoint, and the trigger SHALL return the resulting reminder audit entries. Invalid trigger configuration SHALL be rejected without mutating any workflow decision state. The backend SHALL also support periodic scheduled invocation of reminder evaluation using explicit runtime configuration.

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
