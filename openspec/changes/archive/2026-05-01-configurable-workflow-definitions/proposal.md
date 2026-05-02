## Why

The current system can execute seeded approval workflows, but it cannot manage workflow definitions as first-class business configuration. This blocks operational teams from adapting approval paths, assignee rules, and supported workflow objects without code changes, and it leaves the existing configurable-workflow requirement only partially satisfied.

## What Changes

- Introduce supported administration of workflow definitions, including template creation, node and transition management, publish/deactivate behavior, and versioned rollout for new workflow instances.
- Expand workflow configuration from the current seeded Lease and billing focus to all supported workflow-backed business objects in first-release scope.
- Add dynamic assignee-resolution rules covering fixed users, fixed roles, department leaders, submitter-context resolution, and document-field-based resolution.
- Require workflow-version binding so in-flight instances continue on their original definition version while newly submitted instances use the currently published version.
- Add a non-graphical workflow administration surface for Phase 1 to manage workflow definitions, versions, and validation outcomes.
- Define audit, permission, and safety rules for workflow-definition changes, publication, deactivation, rollback, and runtime replay behavior.
- **BREAKING** Existing assumptions that workflow definitions are seed-only and code-managed will be replaced by supported runtime-managed definitions with explicit publication and versioning rules.
- Explicitly defer the graphical workflow editor, drag-and-drop canvas, and visual connection editing to a later Phase 2 change.

## Capabilities

### New Capabilities
- `workflow-definition-administration`: Manage workflow templates, versions, assignment rules, publication lifecycle, validation, and non-graphical administration for supported workflow objects.

### Modified Capabilities
- `workflow-approvals`: Change workflow behavior from seeded configuration for Lease and billing documents to versioned, runtime-managed definitions for all supported workflow-backed business objects.
- `organization-access-control`: Add explicit permission and authorization requirements for workflow-definition administration, publication, rollback, and assignment-rule management.

## Impact

- Affected backend systems: workflow domain model, workflow APIs, assignee-resolution engine, persistence for definitions/versions/rules, audit history, and publish/rollback safeguards.
- Affected frontend systems: workflow admin experience, validation/error presentation, version-management flows, and workflow-definition maintenance screens.
- Affected business integrations: Lease, billing/invoice, overtime, discount, and other workflow-backed approval flows that will bind to published workflow-definition versions.
- Affected operational controls: permission model, audit observability, rollout safety, and change-management procedures for approval-flow updates.
- New dependencies are not yet proposed in this artifact, but design will need to define how version-safe workflow modeling and runtime validation fit the existing Go modular monolith and Vue admin surfaces.
