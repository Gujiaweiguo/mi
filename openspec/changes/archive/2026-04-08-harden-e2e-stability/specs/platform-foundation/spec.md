## MODIFIED Requirements

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

#### Scenario: Missing current-commit e2e evidence fields block archive readiness
- **WHEN** the `e2e` evidence file exists for the evaluated commit but is missing required archive fields needed to validate commit-scoped results
- **THEN** archive readiness SHALL fail and the commit SHALL NOT be considered archive-ready
