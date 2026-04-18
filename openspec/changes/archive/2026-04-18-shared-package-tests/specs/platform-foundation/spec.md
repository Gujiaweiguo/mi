## ADDED Requirements

### Requirement: Shared backend utility packages SHALL have unit test coverage
The `sqlutil` and `pagination` shared packages SHALL have unit tests covering all exported functions and types. These tests SHALL verify both happy-path and edge-case behavior for each function.

#### Scenario: sqlutil helpers are tested
- **WHEN** the sqlutil test suite runs
- **THEN** all 13 exported functions SHALL be tested with valid and invalid inputs

#### Scenario: pagination functions are tested
- **WHEN** the pagination test suite runs
- **THEN** NormalizePage SHALL be tested with boundary values and ListResult generic SHALL be verified
