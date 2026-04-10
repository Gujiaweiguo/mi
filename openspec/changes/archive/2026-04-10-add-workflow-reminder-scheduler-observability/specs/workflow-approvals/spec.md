## MODIFIED Requirements

### Requirement: The system SHALL support reminder-only automation without decision mutation
The workflow subsystem MAY emit reminder records or notifications for pending approval instances based on configured reminder criteria. Reminder automation SHALL be informational only and SHALL NOT auto-approve, auto-reject, or escalate workflow decisions in first release behavior. Reminder eligibility and deduplication SHALL be deterministic for a given reminder key and reminder window. The implementation SHALL persist auditable emitted/skip outcomes and SHALL provide a query path for reminder history diagnostics. A workflow administrator SHALL be able to manually trigger a reminder evaluation run via a dedicated endpoint, and the trigger SHALL return the resulting reminder audit entries. Invalid trigger configuration SHALL be rejected without mutating any workflow decision state. The backend SHALL also support periodic scheduled invocation of reminder evaluation using explicit runtime configuration. Scheduled invocation SHALL enforce single active scheduler ownership across replicas using distributed locking. Scheduled invocation SHALL emit structured observability signals describing per-tick outcomes and cumulative scheduler health trends.

#### Scenario: Scheduled reminder run logs structured success telemetry
- **WHEN** a scheduled reminder run executes and completes successfully
- **THEN** the scheduler SHALL log run timestamp, duration, emitted/skipped counts, and cumulative success/failure/skip totals

#### Scenario: Scheduled reminder lock-skip is observable
- **WHEN** a scheduler tick cannot acquire ownership lock
- **THEN** the scheduler SHALL log a structured lock-skip outcome and update cumulative lock-skip count without recording a failure

#### Scenario: Consecutive scheduler failures trigger warning signal
- **WHEN** scheduled reminder runs fail in consecutive ticks beyond the configured warning threshold
- **THEN** the scheduler SHALL emit a warning-level observability signal including consecutive failure count and last failure context
