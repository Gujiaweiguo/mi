## Purpose

TBD: Canonical platform foundation spec for the replacement MI system.
## Requirements
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
The change SHALL provide backend unit and integration test harnesses, frontend unit tests, Playwright end-to-end tests, and artifact comparison support for generated outputs before feature slices depend on them. End-to-end verification for first-release non-membership scope SHALL be reproducible under documented clean-checkout bootstrap assumptions so archive-evidence generation remains trustworthy.

#### Scenario: Clean checkout can run test foundations
- **WHEN** the documented test bootstrap commands are run from a clean checkout
- **THEN** backend, frontend, and end-to-end test commands SHALL execute successfully against seeded test fixtures

#### Scenario: Supported e2e flows are reproducible for archive evidence
- **WHEN** operators run the supported E2E verification workflow for a commit within first-release non-membership scope
- **THEN** the workflow SHALL produce reproducible pass/fail outcomes and SHALL be usable to generate valid `e2e` evidence for archive gate evaluation

#### Scenario: E2E contract remains within first-release non-membership scope
- **WHEN** E2E gate coverage is evaluated for archive readiness
- **THEN** required E2E verification SHALL remain bounded to accepted first-release non-membership flows and SHALL NOT introduce implied scope for excluded membership capabilities

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated.

#### Scenario: CI evidence must satisfy canonical schema
- **WHEN** CI readiness is evaluated for the current commit
- **THEN** required `unit` and `integration` evidence SHALL include canonical top-level fields, canonical `source` fields, canonical `stats` fields, and valid timestamp/stat invariants

#### Scenario: Schema-drift CI evidence is invalid
- **WHEN** `unit` or `integration` evidence for the evaluated commit uses non-canonical field names, incompatible value types, or inconsistent stat/timestamp values
- **THEN** that evidence SHALL be treated as malformed and SHALL NOT satisfy CI readiness

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references.

#### Scenario: Archive evidence must satisfy canonical schema
- **WHEN** archive readiness is evaluated for the current commit
- **THEN** required `unit`, `integration`, and `e2e` evidence SHALL satisfy canonical field/type constraints and stat/timestamp invariants

#### Scenario: E2E evidence requires artifact references
- **WHEN** `e2e` evidence exists for the evaluated commit but omits artifact references or provides invalid artifact entries
- **THEN** that `e2e` evidence SHALL be treated as malformed and SHALL NOT satisfy archive readiness

#### Scenario: Schema-drift archive evidence is invalid
- **WHEN** required archive evidence uses incompatible schema structure even if `status` is `passed`
- **THEN** that evidence SHALL be rejected and the commit SHALL NOT be considered archive-ready

### Requirement: The system SHALL provide frontend locale infrastructure as part of the application foundation
The frontend foundation SHALL include locale infrastructure that can be initialized during app startup, provide locale-managed application messages, and align shared framework locale behavior with the active application locale.

#### Scenario: Frontend startup initializes locale infrastructure
- **WHEN** the frontend application starts in local development or deployed runtime
- **THEN** the application SHALL initialize its locale infrastructure before rendering localized user-facing screens

#### Scenario: Shared framework locale follows application locale
- **WHEN** the active application locale is resolved during startup or changed at runtime
- **THEN** shared frontend framework locale behavior SHALL follow the same active locale instead of remaining fixed in English

