## MODIFIED Requirements

### Requirement: The system SHALL support reminder-only automation without decision mutation
The workflow subsystem MAY emit reminder records or notifications for pending approval instances based on configured reminder criteria. Reminder automation SHALL be informational only and SHALL NOT auto-approve, auto-reject, or escalate workflow decisions in first release behavior. Reminder eligibility and deduplication SHALL be deterministic for a given reminder key and reminder window. The implementation SHALL persist auditable emitted/skip outcomes and SHALL provide a query path for reminder history diagnostics. A workflow administrator SHALL be able to manually trigger a reminder evaluation run via a dedicated endpoint, and the trigger SHALL return the resulting reminder audit entries. Invalid trigger configuration SHALL be rejected without mutating any workflow decision state. The backend SHALL also support periodic scheduled invocation of reminder evaluation using explicit runtime configuration. Scheduled invocation SHALL enforce single active scheduler ownership across replicas using distributed locking.

#### Scenario: Only one replica executes scheduled reminder run
- **WHEN** multiple backend replicas have workflow reminder scheduler enabled for the same lock name
- **THEN** at most one replica SHALL acquire scheduler ownership and execute reminder evaluation for a given tick window

#### Scenario: Replica without lock skips run safely
- **WHEN** a replica fails to acquire scheduler ownership lock
- **THEN** it SHALL skip reminder execution for that tick and SHALL NOT mutate workflow decision state

#### Scenario: Lock acquisition failure is recoverable
- **WHEN** scheduler lock acquisition or release encounters transient database errors
- **THEN** the scheduler SHALL log the failure and continue evaluating future ticks
