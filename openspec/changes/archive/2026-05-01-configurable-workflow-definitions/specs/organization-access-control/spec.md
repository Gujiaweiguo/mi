## MODIFIED Requirements

### Requirement: The system SHALL enforce scoped permissions for operational actions
The authorization model SHALL support action-level enforcement for view, edit, approve, print, and export behaviors, with scope rules for relevant organizational or business entities. The authorization model SHALL also enforce distinct permissions for workflow-definition administration actions, including viewing workflow definitions, editing draft definitions, validating definitions, publishing or deactivating versions, rolling back to prior versions, and managing assignment rules.

#### Scenario: Unauthorized action is blocked
- **WHEN** a user without export permission attempts to export an invoice
- **THEN** the system SHALL deny the action and SHALL return an authorization failure without generating an export artifact

#### Scenario: Unauthorized workflow publication is blocked
- **WHEN** a user without workflow-definition publication permission attempts to publish or roll back a workflow definition version
- **THEN** the system SHALL deny the action and SHALL NOT mutate the workflow-definition publication state

## ADDED Requirements

### Requirement: Workflow-definition administration permissions SHALL be separable from workflow runtime approval permissions
The system SHALL treat workflow-definition administration as a distinct authorization surface from runtime approval participation. Users authorized to approve workflow instances SHALL NOT implicitly gain permission to create, edit, validate, publish, deactivate, roll back, or manage assignment rules for workflow definitions.

#### Scenario: Workflow approver cannot edit definitions without admin permission
- **WHEN** a user has runtime workflow approval permission but lacks workflow-definition administration permission
- **THEN** the system SHALL allow runtime approval actions while denying access to workflow-definition management operations
