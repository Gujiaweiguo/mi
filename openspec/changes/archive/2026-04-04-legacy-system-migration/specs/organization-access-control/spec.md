## ADDED Requirements

### Requirement: The system SHALL initialize fresh organization and access-control data
The first release SHALL start with reinitialized users, roles, departments, shops/buildings, and other required master data rather than migrating historical user data from the legacy system.

#### Scenario: Fresh admin baseline is created
- **WHEN** bootstrap data is loaded into an empty environment
- **THEN** initial admin users, roles, departments, and shop/building master data SHALL be available for login and operations

### Requirement: The system SHALL enforce scoped permissions for operational actions
The authorization model SHALL support action-level enforcement for view, edit, approve, print, and export behaviors, with scope rules for relevant organizational or business entities.

#### Scenario: Unauthorized action is blocked
- **WHEN** a user without export permission attempts to export an invoice
- **THEN** the system SHALL deny the action and SHALL return an authorization failure without generating an export artifact
