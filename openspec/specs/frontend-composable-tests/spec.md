## ADDED Requirements

### Requirement: Shared frontend composables SHALL have executable unit tests
Shared frontend composables that are reused across multiple views SHALL have unit tests covering their expected behavior and edge cases.

#### Scenario: Error-message helper is tested
- **WHEN** the frontend unit test suite runs
- **THEN** the shared error-message helper SHALL be verified for both `Error` and non-`Error` inputs

#### Scenario: Download helper is tested
- **WHEN** the frontend unit test suite runs
- **THEN** the shared blob-download helper SHALL be verified to create, use, and revoke an object URL correctly

### Requirement: Dashboard aggregation logic SHALL be testable in isolation
The dashboard summary helper SHALL have unit tests that verify successful aggregation and partial-failure behavior without requiring live backend services.

#### Scenario: Dashboard summary aggregation succeeds
- **WHEN** all mocked dashboard data sources succeed
- **THEN** the helper SHALL return the expected summary counts

#### Scenario: Dashboard summary aggregation partially fails
- **WHEN** one or more mocked dashboard data sources fail
- **THEN** the helper SHALL return partial results and identify failed metrics without throwing
