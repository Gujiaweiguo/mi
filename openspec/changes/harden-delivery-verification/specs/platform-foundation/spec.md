## MODIFIED Requirements

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
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide architecture documentation that explicitly describes responsibility boundaries between evidence producers, schema-driven structure validation, contextual gate validation, and CI-ready entrypoint orchestration. The repository SHALL also define maintenance policy expectations for verification-stack changes so that CI gate behavior, schema validation, and related documentation remain synchronized. CI-ready execution SHALL additionally require all supported prerequisite validation steps for that path, including typecheck, static analysis, and build verification, to pass before the evidence gate can be treated as satisfied.

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
