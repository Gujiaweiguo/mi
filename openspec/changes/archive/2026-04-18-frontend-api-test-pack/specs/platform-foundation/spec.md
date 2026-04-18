## MODIFIED Requirements

### Requirement: Frontend API modules SHALL have unit test coverage
All frontend API modules SHALL have unit tests that verify correct HTTP method, URL construction, response unwrapping, and error propagation.

#### Scenario: Each API module has HTTP boundary tests
- **WHEN** an API function is called
- **THEN** it SHALL call the correct HTTP method on the correct URL and return properly unwrapped data
