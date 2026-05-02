## MODIFIED Requirements

### Requirement: The system SHALL provide configurable approval workflows
The first release SHALL support configurable workflow templates, transitions, assignee resolution, and document-specific approval flows for all supported workflow-backed business objects. Workflow submission and action handling SHALL enforce valid source-state transitions for supported business objects before any downstream side effect is dispatched, and runtime execution SHALL resolve against the published workflow-definition version that was active when the workflow instance was created.

#### Scenario: Multi-step approval completes
- **WHEN** a supported business document is submitted to a configured workflow and all required approvers act successfully
- **THEN** the document SHALL transition through the configured states and finish in an approved state

#### Scenario: Invalid workflow transition is blocked
- **WHEN** a workflow action is requested for a document state that does not allow that transition under the configured flow
- **THEN** the system SHALL reject the action and SHALL preserve the current workflow and document state

#### Scenario: Published workflow applies to supported workflow-backed business objects
- **WHEN** a supported workflow-backed business object is configured to use a published workflow definition
- **THEN** the system SHALL execute approval transitions for that object according to the published workflow template version bound at instance creation time

### Requirement: The system SHALL record workflow audit history and protect side effects from duplication
Workflow actions SHALL be auditable and SHALL dispatch downstream side effects through an idempotent mechanism that prevents duplicate exports, notifications, or output generation from repeated requests. Replayed workflow actions for supported workflow-backed business objects SHALL preserve the current state and SHALL NOT create duplicate workflow side effects. Reminder-only automation for pending instances, when enabled, SHALL follow the same idempotent side-effect constraints.

#### Scenario: Duplicate approval request is safe
- **WHEN** the same approval request is replayed for a workflow action that has already been applied
- **THEN** the system SHALL preserve the existing state and SHALL NOT create duplicate downstream side effects

#### Scenario: Duplicate submission request is safe
- **WHEN** the same workflow submission request is replayed for a supported workflow-backed business object that is already attached to an in-flight or completed workflow instance
- **THEN** the system SHALL preserve the existing workflow linkage and SHALL NOT create a duplicate workflow instance or duplicate downstream side effects

#### Scenario: Duplicate reminder run is safe
- **WHEN** a reminder job reprocesses the same pending workflow instance within the same reminder window
- **THEN** the system SHALL avoid duplicate reminder side effects for that reminder key and SHALL preserve existing reminder audit state

## ADDED Requirements

### Requirement: Workflow instances SHALL preserve immutable definition-version binding
Each workflow instance SHALL preserve the published workflow-definition version selected at submission time for the lifetime of that instance. Later publication, deactivation, or rollback actions SHALL affect only future workflow instances.

#### Scenario: New submission binds to the active published version
- **WHEN** an operator submits a supported workflow-backed business object after a template version is published
- **THEN** the created workflow instance SHALL bind to that active published version

#### Scenario: Existing instance remains on its original definition version
- **WHEN** a newer version of the same workflow template is published while an earlier workflow instance is still pending approval
- **THEN** the pending workflow instance SHALL continue using its original definition version for subsequent transitions
