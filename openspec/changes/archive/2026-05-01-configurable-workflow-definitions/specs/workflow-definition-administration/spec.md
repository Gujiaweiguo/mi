## ADDED Requirements

### Requirement: Workflow definitions SHALL be managed as versioned templates
The system SHALL manage approval-flow definitions as first-class workflow templates with immutable versions. Operators SHALL be able to create a template, create and edit draft versions, and retain historical versions without mutating already published runtime behavior.

#### Scenario: Administrator creates a draft workflow template version
- **WHEN** an authorized workflow administrator creates a new workflow template and saves its first draft version
- **THEN** the system SHALL persist the template identity separately from the draft version identity and SHALL keep the draft editable until publication

#### Scenario: Editing a draft does not mutate published behavior
- **WHEN** an authorized workflow administrator edits a draft version for a template that already has a published version
- **THEN** the system SHALL preserve the currently published version for runtime use and SHALL apply the edits only to the targeted draft version

### Requirement: Workflow definitions SHALL support typed assignee-resolution strategies
The system SHALL support typed assignee-resolution strategies for workflow nodes, including fixed users, fixed roles, department leaders, submitter-context resolution, and document-field-based resolution. Publication-time validation SHALL verify that each configured strategy has the required structured configuration for its type.

#### Scenario: Administrator configures a fixed-role assignee rule
- **WHEN** an authorized workflow administrator assigns a node using the fixed-role strategy and selects a valid role
- **THEN** the system SHALL store that strategy type and role reference as structured workflow-definition data

#### Scenario: Administrator configures document-field-based assignee routing
- **WHEN** an authorized workflow administrator configures an assignee rule that resolves from a supported business-document field path
- **THEN** the system SHALL persist the field-path-based strategy and SHALL reject unsupported field references during validation

### Requirement: Workflow definitions SHALL be validated before publication
The system SHALL validate draft workflow definitions before publication. Validation SHALL reject incomplete node graphs, invalid transitions, missing terminal outcomes, unsupported assignee rules, or unresolved object applicability so that invalid definitions cannot become active runtime behavior.

#### Scenario: Invalid workflow definition cannot be published
- **WHEN** an authorized workflow administrator attempts to publish a draft version with validation errors
- **THEN** the system SHALL reject the publication request, SHALL preserve the draft version state, and SHALL return validation diagnostics suitable for operator correction

#### Scenario: Valid workflow definition can be published
- **WHEN** an authorized workflow administrator publishes a draft version that satisfies workflow-definition validation rules
- **THEN** the system SHALL mark that version as published and make it eligible for new workflow-instance creation

### Requirement: Published workflow versions SHALL govern only future workflow instances
The system SHALL bind each new workflow instance to the published workflow-definition version that is active at submission time. In-flight and completed workflow instances SHALL retain their original version binding after later publication, deactivation, or rollback actions.

#### Scenario: New workflow instance uses the current published version
- **WHEN** a supported business object is submitted after a workflow template version has been published
- **THEN** the created workflow instance SHALL bind to that published version

#### Scenario: Existing workflow instance keeps its original version after later publication
- **WHEN** a newer version of the same workflow template is published after a workflow instance is already pending or completed
- **THEN** the existing workflow instance SHALL preserve its original version binding and SHALL NOT be migrated in place to the newer definition

### Requirement: Workflow-definition publication lifecycle SHALL support deactivation and rollback
The system SHALL support controlled publication lifecycle actions for workflow templates, including publish, deactivate, and rollback to a previously valid version. Rollback SHALL affect only future workflow instances and SHALL preserve historical auditability of prior publication actions.

#### Scenario: Administrator rolls back to a prior version
- **WHEN** an authorized workflow administrator selects a previously valid historical version for rollback
- **THEN** the system SHALL make that version the active published version for future workflow instances without mutating historical instance bindings

#### Scenario: Administrator deactivates a published workflow template
- **WHEN** an authorized workflow administrator deactivates the currently published version for a workflow template
- **THEN** the system SHALL prevent new workflow-instance creation from that template until a valid version is published again

### Requirement: Workflow-definition administration SHALL be available through a non-graphical management surface in Phase 1
The system SHALL provide a structured, non-graphical administration surface for workflow templates, versions, node order, transitions, assignee rules, validation results, and publication actions. Phase 1 SHALL NOT require a graphical drag-and-drop canvas to create or maintain workflow definitions.

#### Scenario: Administrator manages nodes and transitions without a graph editor
- **WHEN** an authorized workflow administrator edits a workflow definition in Phase 1
- **THEN** the system SHALL allow node, transition, and assignee-rule maintenance through structured forms or ordered administrative controls without requiring a graphical editor

#### Scenario: Administrator can inspect version history and validation state
- **WHEN** an authorized workflow administrator opens a workflow template in the administration surface
- **THEN** the system SHALL display its version history, current publication state, and latest validation outcomes

### Requirement: Workflow-definition mutations SHALL be auditable
The system SHALL record auditable operator history for workflow-definition creation, draft edits, validation, publication, deactivation, and rollback actions.

#### Scenario: Workflow-definition publication is auditable
- **WHEN** an authorized workflow administrator publishes, deactivates, or rolls back a workflow version
- **THEN** the system SHALL persist an audit record containing operator identity, action type, targeted template/version, and timestamp
