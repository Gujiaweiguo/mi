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
The change SHALL provide backend unit and integration test harnesses, frontend unit tests, Playwright end-to-end tests, and artifact comparison support for generated outputs before feature slices depend on them. End-to-end verification for first-release non-membership scope SHALL be reproducible under documented clean-checkout bootstrap assumptions so archive-evidence generation remains trustworthy. Unit and integration evidence emitted by the verification scripts SHALL derive reported test counts from actual test results rather than fixed placeholder values.

#### Scenario: Unit evidence reports real aggregated counts
- **WHEN** the unit verification workflow runs backend unit tests and frontend unit tests for a commit
- **THEN** the resulting `unit` evidence SHALL report `total`, `passed`, `failed`, and `skipped` counts derived from the actual executed test results across the covered unit suites

#### Scenario: Integration evidence reports real command results
- **WHEN** the integration verification workflow runs the integration-tagged backend tests for a commit
- **THEN** the resulting `integration` evidence SHALL report `total`, `passed`, `failed`, and `skipped` counts derived from the actual executed integration test results

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide a repeatable self-check that confirms the canonical machine-readable evidence schema still parses and still validates representative CI evidence examples or fixtures. Representative CI evidence samples used for documentation and schema self-checks SHALL resolve to a centralized canonical repository source.

#### Scenario: CI evidence references use canonical sample source
- **WHEN** contributors review representative CI evidence examples or maintainers run schema self-checks
- **THEN** both documentation and self-check tooling SHALL point to the same canonical repository-owned CI evidence sample source

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL include representative archive/e2e evidence in schema self-check coverage. Representative archive/e2e evidence samples used for documentation and schema self-checks SHALL resolve to a centralized canonical repository source.

#### Scenario: Archive evidence references use canonical sample source
- **WHEN** contributors review representative archive/e2e evidence examples or maintainers run schema self-checks
- **THEN** both documentation and self-check tooling SHALL point to the same canonical repository-owned archive/e2e evidence sample source

### Requirement: The system SHALL provide frontend locale infrastructure as part of the application foundation
The frontend foundation SHALL include locale infrastructure that can be initialized during app startup, provide locale-managed application messages, and align shared framework locale behavior with the active application locale.

#### Scenario: Frontend startup initializes locale infrastructure
- **WHEN** the frontend application starts in local development or deployed runtime
- **THEN** the application SHALL initialize its locale infrastructure before rendering localized user-facing screens

#### Scenario: Shared framework locale follows application locale
- **WHEN** the active application locale is resolved during startup or changed at runtime
- **THEN** shared frontend framework locale behavior SHALL follow the same active locale instead of remaining fixed in English

