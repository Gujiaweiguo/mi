## Purpose

TBD: Canonical platform foundation spec for the replacement MI system.
## Requirements
### Requirement: The system SHALL provide a modular monolith foundation for the new stack
The change SHALL introduce a Vue 3 frontend, a Go modular monolith backend, and a MySQL 8 database as the first-release runtime foundation. The foundation SHALL support local development for frontend/backend with an existing Docker MySQL 8 instance and SHALL support Docker Compose-based test and production topologies.

#### Scenario: Local development foundation is available
- **WHEN** a developer starts the frontend and backend locally with the documented development configuration
- **THEN** the application SHALL connect to the configured MySQL 8 instance and expose working frontend and backend health endpoints

### Requirement: The system SHALL externalize environment configuration and runtime mounts
The change SHALL define environment-specific configuration with file-based defaults and environment-variable overrides. Test and production environments SHALL mount runtime paths for configuration, logs, generated documents, uploads, and MySQL data. Production runtime mounts SHALL also enforce documented hygiene and permission assumptions so rehearsal and go-live validation are not considered valid under contaminated runtime baselines or unsupported container runtime behavior.

#### Scenario: Production runtime paths are configured
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** explicit mounts SHALL exist for configuration, logs, generated documents/uploads, and MySQL data

#### Scenario: Production runtime mount hygiene is validated
- **WHEN** production startup or rehearsal preflight evaluates runtime mount baselines
- **THEN** the workflow SHALL reject runtime baselines that violate documented clean-start and hygiene constraints for supported production validation

#### Scenario: Runtime mount permissions are validated for supported container behavior
- **WHEN** production startup or rehearsal validation checks mounted runtime paths
- **THEN** the workflow SHALL verify required writable paths under supported container runtime assumptions and SHALL fail when those assumptions are not met

### Requirement: The system SHALL establish automated test foundations before feature slices
The change SHALL provide backend unit and integration test harnesses, frontend unit tests, Playwright end-to-end tests, and artifact comparison support for generated outputs before feature slices depend on them. End-to-end verification for first-release non-membership scope SHALL be reproducible under documented clean-checkout bootstrap assumptions so archive-evidence generation remains trustworthy. Unit and integration evidence emitted by the verification scripts SHALL derive reported test counts from actual test results rather than fixed placeholder values. The repository SHALL also provide a supported validation path for frontend typechecking, backend static analysis, and frontend build verification so non-test regressions are caught through the default delivery workflow rather than left to ad hoc local checks.

#### Scenario: Unit evidence reports real aggregated counts
- **WHEN** the unit verification workflow runs backend unit tests and frontend unit tests for a commit
- **THEN** the resulting `unit` evidence SHALL report `total`, `passed`, `failed`, and `skipped` counts derived from the actual executed test results across the covered unit suites

#### Scenario: Integration evidence reports real command results
- **WHEN** the integration verification workflow runs the integration-tagged backend tests for a commit
- **THEN** the resulting `integration` evidence SHALL report `total`, `passed`, `failed`, and `skipped` counts derived from the actual executed integration test results

#### Scenario: Delivery workflow runs frontend typecheck
- **WHEN** a contributor runs the supported CI-ready validation path for a commit
- **THEN** that path SHALL execute the repository's supported frontend typecheck command and SHALL fail before reporting CI-ready success if type errors are present

#### Scenario: Delivery workflow runs backend static analysis
- **WHEN** a contributor runs the supported CI-ready validation path for a commit
- **THEN** that path SHALL execute the repository's supported backend static-analysis command and SHALL fail before reporting CI-ready success if the analysis reports errors

#### Scenario: Delivery workflow verifies frontend build health
- **WHEN** a contributor runs the supported CI-ready validation path for a commit
- **THEN** that path SHALL execute the supported frontend production build command and SHALL fail before reporting CI-ready success if the build does not complete successfully

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide architecture documentation that explicitly describes responsibility boundaries between evidence producers, schema-driven structure validation, contextual gate validation, and CI-ready entrypoint orchestration. The repository SHALL also define maintenance policy expectations for verification-stack changes so that CI gate behavior, schema validation, and related documentation remain synchronized. CI-ready execution SHALL additionally require all supported prerequisite validation steps for that path, including typecheck, static analysis, and build verification, to pass before the evidence gate can be treated as satisfied. The CI-ready workflow trigger policy SHALL avoid creating duplicate runs for the same PR update while preserving one authoritative validation result for the evaluated commit.

#### Scenario: Verification architecture responsibilities are documented for CI path
- **WHEN** contributors need to modify CI-related verification behavior
- **THEN** the repository SHALL provide architecture documentation that identifies where producer logic, schema rules, validator contextual checks, and CI entrypoint wiring each belong

#### Scenario: CI gate blocks when required evidence is missing
- **WHEN** unit or integration evidence for the evaluated commit is missing
- **THEN** CI-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: CI gate blocks stale evidence after commit changes
- **WHEN** evidence files exist but `commit_sha` does not match the currently evaluated commit
- **THEN** CI-ready evaluation SHALL reject the evidence as stale and SHALL fail the gate

#### Scenario: CI-ready path blocks on failed prerequisite validation
- **WHEN** the supported CI-ready path encounters a failing typecheck, static-analysis, or build-validation step for the evaluated commit
- **THEN** the CI-ready path SHALL fail and SHALL not report the commit as ready even if `unit` and `integration` evidence exist

#### Scenario: A pull request update produces one CI-ready run
- **WHEN** a contributor updates a branch that already has an open pull request
- **THEN** the repository SHALL produce one authoritative `ci-ready` workflow run for that PR update instead of duplicate runs with the same validation intent

#### Scenario: Trigger deduplication does not weaken commit-scoped validation
- **WHEN** CI-ready runs for an evaluated commit after trigger deduplication is applied
- **THEN** the run SHALL still validate `unit` and `integration` evidence for the current commit SHA and SHALL fail if required evidence is missing or stale

#### Scenario: Non-PR CI coverage remains intentional
- **WHEN** a branch update occurs outside the PR lifecycle but still matches the repository's intended CI-ready coverage policy
- **THEN** the workflow SHALL either run by design or be explicitly excluded by documented trigger rules, rather than being left ambiguous by overlapping events

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL provide architecture documentation that explains how archive evaluation reuses shared verification components while preserving archive-specific requirements. The repository SHALL also define maintenance policy expectations for archive-path verification changes so regression checks and documentation updates are performed together.

#### Scenario: Verification architecture documents archive-specific concerns
- **WHEN** contributors need to reason about archive-ready behavior
- **THEN** the architecture documentation SHALL explain shared versus archive-specific verification responsibilities without requiring reverse engineering from scripts alone

#### Scenario: Archive gate blocks when required evidence is missing
- **WHEN** any required archive evidence (`unit`, `integration`, or `e2e`) for the evaluated commit is missing
- **THEN** archive-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: Archive gate blocks stale evidence after commit changes
- **WHEN** archive evidence exists but references a different commit SHA than the evaluated commit
- **THEN** archive-ready evaluation SHALL reject the stale evidence and SHALL fail the gate

### Requirement: The system SHALL provide frontend locale infrastructure as part of the application foundation
The frontend foundation SHALL include locale infrastructure that can be initialized during app startup, provide locale-managed application messages, and align shared framework locale behavior with the active application locale.

#### Scenario: Frontend startup initializes locale infrastructure
- **WHEN** the frontend application starts in local development or deployed runtime
- **THEN** the application SHALL initialize its locale infrastructure before rendering localized user-facing screens

#### Scenario: Shared framework locale follows application locale
- **WHEN** the active application locale is resolved during startup or changed at runtime
- **THEN** shared frontend framework locale behavior SHALL follow the same active locale instead of remaining fixed in English
