## MODIFIED Requirements

### Requirement: The system SHALL provide Compose-based test and production operations
The change SHALL provide Docker Compose definitions for test and production that include nginx, frontend, backend, and MySQL 8 with explicit health checks, mounted runtime paths, rendered environment-specific configuration, and executable bring-up validation for the documented runtime assumptions.

#### Scenario: Test environment starts successfully
- **WHEN** the test Docker Compose stack is started from a clean environment
- **THEN** all required services SHALL become healthy and SHALL expose the documented frontend and backend entry points

#### Scenario: Compose configuration is rendered before startup
- **WHEN** an operator prepares the test or production environment for bring-up
- **THEN** the system SHALL render and validate the target Compose configuration before attempting container startup so path, env-file, and syntax issues fail fast

#### Scenario: Runtime mount assumptions are validated
- **WHEN** the test or production stack is started through the supported operational workflow
- **THEN** the system SHALL validate that required runtime directories and mounted paths exist and are writable before declaring the environment ready

### Requirement: The system SHALL provide a cutover rehearsal and rollback model
The first release SHALL include a repeatable rehearsal flow, release gates, rollback criteria, backup rehearsal, restore rehearsal, and machine-readable GO/NO-GO output for one-time cutover.

#### Scenario: Blocking decision prevents go-live
- **WHEN** a cutover rehearsal is run while a blocking release-gate decision remains unresolved
- **THEN** the rehearsal SHALL report a no-go outcome and SHALL NOT mark the release ready for production

#### Scenario: Missing current-commit archive evidence blocks rehearsal
- **WHEN** cutover rehearsal is started without passing `unit`, `integration`, and `e2e` evidence for the current HEAD commit
- **THEN** the rehearsal SHALL fail fast, report a no-go outcome, and SHALL NOT continue to a go-live-ready decision

#### Scenario: Stale current-commit archive evidence blocks rehearsal
- **WHEN** the available archive-ready evidence exists but does not match the current HEAD commit SHA
- **THEN** the rehearsal SHALL treat the evidence as invalid, report a no-go outcome, and SHALL NOT mark the release ready for production

#### Scenario: Backup and restore rehearsal are verified together
- **WHEN** a cutover rehearsal is executed for the supported target environment
- **THEN** the workflow SHALL produce a backup bundle, restore it through the supported restore path, and run explicit post-restore validation before the rehearsal can report a go outcome

#### Scenario: Rehearsal emits machine-readable result artifacts
- **WHEN** the cutover rehearsal completes in either success or failure state
- **THEN** the system SHALL write machine-readable result artifacts and logs under `artifacts/rehearsal/<commit-sha>/` that identify the evaluated commit and the binary GO/NO-GO result

### Requirement: The system SHALL start production without migrating legacy records
The first release SHALL start with reinitialized base data and SHALL NOT migrate legacy business records, pending drafts, approvals, or open operational transactions from the old system. Rehearsal and go-live execution SHALL validate bootstrap-only initialization and SHALL reject any path that attempts to import legacy transactional business data.

#### Scenario: Fresh-start cutover is enforced
- **WHEN** the cutover rehearsal or go-live checklist is executed
- **THEN** the system SHALL initialize only the approved fresh-start base data set and SHALL NOT import legacy business data into the new production system

#### Scenario: Legacy transactional import causes no-go
- **WHEN** rehearsal input or operator configuration attempts to include migrated legacy business records, open operational items, or in-flight approvals as starting state
- **THEN** the cutover workflow SHALL reject the attempt, report a no-go outcome, and preserve the repository’s fresh-start cutover rule
