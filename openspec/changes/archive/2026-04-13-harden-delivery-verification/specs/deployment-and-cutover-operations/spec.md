## MODIFIED Requirements

### Requirement: The system SHALL provide Compose-based test and production operations
The change SHALL provide Docker Compose definitions for test and production that include nginx, frontend, backend, and MySQL 8 with explicit health checks, mounted runtime paths, rendered environment-specific configuration, and executable bring-up validation for the documented runtime assumptions. The supported operational workflow SHALL also provide a real-stack verification path that can start the live frontend, backend, and MySQL boundary together under repository-supported runtime assumptions so release validation is not limited to mock-only browser execution.

#### Scenario: Test environment starts successfully
- **WHEN** the test Docker Compose stack is started from a clean environment
- **THEN** all required services SHALL become healthy and SHALL expose the documented frontend and backend entry points

#### Scenario: Compose configuration is rendered before startup
- **WHEN** an operator prepares the test or production environment for bring-up
- **THEN** the system SHALL render and validate the target Compose configuration before attempting container startup so path, env-file, and syntax issues fail fast

#### Scenario: Runtime mount assumptions are validated
- **WHEN** the test or production stack is started through the supported operational workflow
- **THEN** the system SHALL validate that required runtime directories and mounted paths exist and are writable before declaring the environment ready

#### Scenario: Real-stack verification path uses the supported runtime boundary
- **WHEN** a release-validation workflow invokes the repository's supported live-stack verification path
- **THEN** that workflow SHALL exercise the real frontend, backend, and MySQL boundary through the supported runtime topology instead of substituting mocked API responses for the primary application seam

## ADDED Requirements

### Requirement: The system SHALL validate a live-stack operator flow before release readiness is claimed
The release-validation surface SHALL include at least one stable end-to-end operator flow that runs against the real frontend, backend, and MySQL stack and verifies the frontend-backend integration seam under first-release runtime assumptions. This live-stack verification SHALL complement mocked browser coverage and backend integration tests; it SHALL NOT be satisfied by browser tests that intercept or replace the primary API seam with mocked responses.

#### Scenario: Release validation includes at least one live-stack flow
- **WHEN** contributors evaluate release readiness for a candidate commit
- **THEN** the repository SHALL provide at least one supported end-to-end flow that runs against the live application stack and produces a pass/fail result

#### Scenario: Mock-only browser coverage does not satisfy live-stack requirement
- **WHEN** all available browser verification for a candidate commit depends on API interception or mocked backend responses
- **THEN** release validation SHALL treat the live-stack verification requirement as unsatisfied

#### Scenario: Live-stack verification failure blocks release readiness
- **WHEN** the supported live-stack operator flow fails against the real frontend, backend, and MySQL stack for the evaluated commit
- **THEN** the repository SHALL not treat that commit as release-ready until the live-stack path passes
