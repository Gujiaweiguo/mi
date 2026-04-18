## MODIFIED Requirements

### Requirement: Critical user-facing views SHALL have component tests
LoginView and DashboardView SHALL have component-level tests verifying mount, API interaction, and error handling.

#### Scenario: Login and dashboard views are tested
- **WHEN** a critical view is mounted
- **THEN** it SHALL call the correct API and handle responses appropriately
