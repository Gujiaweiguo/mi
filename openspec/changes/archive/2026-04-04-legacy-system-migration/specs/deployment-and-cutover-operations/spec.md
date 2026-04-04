## ADDED Requirements

### Requirement: The system SHALL provide Compose-based test and production operations
The change SHALL provide Docker Compose definitions for test and production that include nginx, frontend, backend, and MySQL 8 with explicit health checks and mounted runtime paths.

#### Scenario: Test environment starts successfully
- **WHEN** the test Docker Compose stack is started from a clean environment
- **THEN** all required services SHALL become healthy and SHALL expose the documented frontend and backend entry points

### Requirement: The system SHALL provide a cutover rehearsal and rollback model
The first release SHALL include a repeatable rehearsal flow, release gates, and rollback criteria for one-time cutover.

#### Scenario: Blocking decision prevents go-live
- **WHEN** a cutover rehearsal is run while a blocking release-gate decision remains unresolved
- **THEN** the rehearsal SHALL report a no-go outcome and SHALL NOT mark the release ready for production

### Requirement: The system SHALL start production without migrating legacy records
The first release SHALL start with reinitialized base data and SHALL NOT migrate legacy business records, pending drafts, approvals, or open operational transactions from the old system.

#### Scenario: Fresh-start cutover is enforced
- **WHEN** the cutover rehearsal or go-live checklist is executed
- **THEN** the system SHALL initialize only the approved fresh-start base data set and SHALL NOT import legacy business data into the new production system
