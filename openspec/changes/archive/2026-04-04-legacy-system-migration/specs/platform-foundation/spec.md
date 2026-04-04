## ADDED Requirements

### Requirement: The system SHALL provide a modular monolith foundation for the new stack
The change SHALL introduce a Vue 3 frontend, a Go modular monolith backend, and a MySQL 8 database as the first-release runtime foundation. The foundation SHALL support local development for frontend/backend with an existing Docker MySQL 8 instance and SHALL support Docker Compose-based test and production topologies.

#### Scenario: Local development foundation is available
- **WHEN** a developer starts the frontend and backend locally with the documented development configuration
- **THEN** the application SHALL connect to the configured MySQL 8 instance and expose working frontend and backend health endpoints

### Requirement: The system SHALL externalize environment configuration and runtime mounts
The change SHALL define environment-specific configuration with file-based defaults and environment-variable overrides. Test and production environments SHALL mount runtime paths for configuration, logs, generated documents, uploads, and MySQL data.

#### Scenario: Production runtime paths are configured
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** explicit mounts SHALL exist for configuration, logs, generated documents/uploads, and MySQL data

### Requirement: The system SHALL establish automated test foundations before feature slices
The change SHALL provide backend unit and integration test harnesses, frontend unit tests, Playwright end-to-end tests, and artifact comparison support for generated outputs before feature slices depend on them.

#### Scenario: Clean checkout can run test foundations
- **WHEN** the documented test bootstrap commands are run from a clean checkout
- **THEN** backend, frontend, and end-to-end test commands SHALL execute successfully against seeded test fixtures

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready.

#### Scenario: Missing integration evidence blocks CI readiness
- **WHEN** verification is performed for a commit that has passing unit evidence but no passing integration evidence for the same commit SHA
- **THEN** that commit SHALL NOT be considered CI-ready

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready.

#### Scenario: Missing e2e evidence blocks archive readiness
- **WHEN** archive readiness is evaluated for a commit that has passing unit and integration evidence but no passing e2e evidence for the same commit SHA
- **THEN** that commit SHALL NOT be considered archive-ready

#### Scenario: Stale evidence does not satisfy any gate
- **WHEN** an evidence file exists but its `commit_sha` does not match the current HEAD commit under evaluation
- **THEN** that evidence SHALL be treated as invalid for both CI and archive gates

#### Scenario: Malformed evidence blocks the required gate
- **WHEN** an evidence file exists but does not satisfy the required summary structure for its gate
- **THEN** that evidence SHALL be treated as invalid for the gate that requires it

#### Scenario: Failed evidence blocks the required gate
- **WHEN** an evidence file exists for the current commit but its `status` is not `passed`
- **THEN** that evidence SHALL be treated as failing for the gate that requires it
