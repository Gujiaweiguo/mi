## 1. Data model and baseline migration

- [x] 1.1 Add persistence models and migrations for workflow templates, workflow template versions, node definitions, transition definitions, assignment rules, publication states, and workflow-definition audit records
- [x] 1.2 Define supported workflow-backed object types and baseline template identity keys for Lease, billing/invoice, overtime, discount, and other in-scope approval objects
- [x] 1.3 Import the current seeded workflow definitions as managed baseline templates and published baseline versions without changing existing runtime behavior
- [x] 1.4 Add repository coverage for creating, reading, listing, and versioning workflow definitions and their related nodes, transitions, and assignment rules

## 2. Validation, versioning, and assignee resolution engine

- [x] 2.1 Implement workflow-definition validation for graph completeness, valid transitions, terminal outcomes, object applicability, and unsupported field references
- [x] 2.2 Implement typed assignee-resolution strategies for fixed users, fixed roles, department leaders, submitter context, and document-field-based routing
- [x] 2.3 Implement publication lifecycle actions for draft validation, publish, deactivate, and rollback to a prior valid version
- [x] 2.4 Bind new workflow instances to immutable published template-version references and preserve existing instance bindings after later publication changes

## 3. Runtime workflow integration

- [x] 3.1 Refactor workflow instance creation to resolve published template versions instead of seed-only definitions
- [x] 3.2 Update workflow submission and action execution paths to use version-bound node, transition, and assignee-resolution data for all supported workflow-backed business objects
- [x] 3.3 Preserve existing duplicate-safe workflow submission, approval, rejection, resubmission, reminder, and downstream side-effect behavior after the versioned-definition switch
- [x] 3.4 Add integration coverage proving that publishing a newer definition affects only future instances while in-flight instances continue on their original version

## 4. Administration APIs and permissions

- [x] 4.1 Add administration APIs for template CRUD, draft version editing, validation, publication, deactivation, rollback, and version-history retrieval
- [x] 4.2 Extend organization-access-control with explicit permissions for viewing workflow definitions, editing drafts, validating definitions, publishing/deactivating/rolling back versions, and managing assignment rules
- [x] 4.3 Enforce permission checks and audit logging on all workflow-definition administration actions
- [x] 4.4 Add handler and service tests covering authorization failures, validation failures, publish success, deactivate success, and rollback success

## 5. Phase 1 workflow-definition administration UI

- [x] 5.1 Add frontend API modules and state management for workflow-template listing, version-history loading, draft editing, validation, publication, deactivation, and rollback
- [x] 5.2 Build a non-graphical workflow-definition administration view with structured editing for node order, transitions, assignment rules, publication state, and validation diagnostics
- [x] 5.3 Surface workflow-definition administration permissions separately from runtime workflow-approval permissions in the frontend routing and action controls
- [x] 5.4 Add unit tests for definition management flows, validation feedback, publication actions, rollback handling, and permission-gated UI behavior

## 6. Verification and release evidence

- [x] 6.1 Add integration tests for baseline seed import, template version publication, immutable instance-version binding, assignee-resolution strategies, and rollback behavior
- [x] 6.2 Add e2e coverage for the non-graphical workflow-definition administration flow, including draft editing, validation failure, publish success, and rollback visibility
- [x] 6.3 Produce CI evidence files for the implementation commit at `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json`
- [x] 6.4 Produce archive evidence files for the implementation commit at `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`
