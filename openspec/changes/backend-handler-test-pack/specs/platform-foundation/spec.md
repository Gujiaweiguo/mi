## MODIFIED Requirements

### Requirement: Handler unit tests SHALL verify input validation
All HTTP handler files SHALL have unit tests that verify input validation (invalid JSON, missing fields, bad route params, bad query params) returns appropriate HTTP 400 responses.

#### Scenario: Each handler has input validation tests
- **WHEN** a handler receives malformed input
- **THEN** it SHALL return HTTP 400 with a descriptive message
