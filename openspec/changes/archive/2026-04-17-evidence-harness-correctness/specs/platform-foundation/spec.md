## MODIFIED Requirements

### Requirement: The system SHALL establish automated test foundations before feature slices
The change SHALL provide backend unit and integration test harnesses, frontend unit tests, Playwright end-to-end tests, and artifact comparison support for generated outputs before feature slices depend on them. End-to-end verification for first-release non-membership scope SHALL be reproducible under documented clean-checkout bootstrap assumptions so archive-evidence generation remains trustworthy. Unit and integration evidence emitted by the verification scripts SHALL derive reported test counts from actual test results rather than fixed placeholder values. The repository SHALL also provide a supported validation path for frontend typechecking, backend static analysis, and frontend build verification so non-test regressions are caught through the default delivery workflow rather than left to ad hoc local checks. Verification scripts that produce `unit` or `integration` evidence SHALL always emit commit-scoped evidence for the evaluated commit regardless of whether the underlying test command succeeds or fails, and SHALL reflect the real pass/fail outcome in the evidence `status` field rather than hardcoding success semantics.

#### Scenario: Unit evidence reports real aggregated counts
- **WHEN** the unit verification workflow runs backend unit tests and frontend unit tests for a commit
- **THEN** the resulting `unit` evidence SHALL report `total`, `passed`, `failed`, and `skipped` counts derived from the actual executed test results across the covered unit suites

#### Scenario: Integration evidence reports real command results
- **WHEN** the integration verification workflow runs the integration-tagged backend tests for a commit
- **THEN** the resulting `integration` evidence SHALL report `total`, `passed`, `failed`, and `skipped` counts derived from the actual executed integration test results

#### Scenario: Unit evidence reflects actual pass/fail outcome
- **WHEN** the unit verification workflow runs and one or more unit tests fail for the evaluated commit
- **THEN** the resulting `unit` evidence SHALL carry `status` equal to `failed` and SHALL report nonzero `failed` counts, and the evidence file SHALL be written before the verification script exits with a non-zero exit code

#### Scenario: Integration evidence reflects actual pass/fail outcome
- **WHEN** the integration verification workflow runs and one or more integration tests fail for the evaluated commit
- **THEN** the resulting `integration` evidence SHALL carry `status` equal to `failed` and SHALL report nonzero `failed` counts, and the evidence file SHALL be written before the verification script exits with a non-zero exit code

#### Scenario: Unit verification runs both backend and frontend suites even when one fails
- **WHEN** the unit verification workflow runs for a commit and either backend or frontend unit tests fail
- **THEN** the workflow SHALL still execute the other suite and SHALL aggregate results from both into a single `unit` evidence file for the evaluated commit

#### Scenario: Delivery workflow runs frontend typecheck
- **WHEN** a contributor runs the supported CI-ready validation path for a commit
- **THEN** that path SHALL execute the repository's supported frontend typecheck command and SHALL fail before reporting CI-ready success if type errors are present

#### Scenario: Delivery workflow runs backend static analysis
- **WHEN** a contributor runs the supported CI-ready validation path for a commit
- **THEN** that path SHALL execute the repository's supported backend static-analysis command and SHALL fail before reporting CI-ready success if the analysis reports errors

#### Scenario: Delivery workflow verifies frontend build health
- **WHEN** a contributor runs the supported CI-ready validation path for a commit
- **THEN** that path SHALL execute the supported frontend production build command and SHALL fail before reporting CI-ready success if the build does not complete successfully
