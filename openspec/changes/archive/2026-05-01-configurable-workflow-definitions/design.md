## Context

The current workflow subsystem already executes approval flows, reminder evaluation, audit history, and replay-safe downstream side effects for Lease and billing-related business objects. However, the running definitions are effectively seed-managed and code-coupled, which means operational teams cannot safely create or evolve workflow definitions without a deployment.

This change introduces Phase 1 of configurable workflow definitions. Phase 1 is intentionally limited to a structured administration surface and runtime model for managing workflow definitions, versions, publication state, and assignment rules. The requested graphical workflow editor is explicitly deferred to Phase 2 so that the core workflow engine, versioning guarantees, and assignee-resolution semantics can be stabilized first.

The design must fit the existing Vue 3 frontend, Go modular monolith backend, and MySQL 8 persistence model. It must preserve the current workflow guarantees around auditability, idempotency, reminder behavior, and explicit exclusion of timeout/escalation automation from first release. It must also integrate with organization-access-control so workflow-definition administration is protected by explicit permissions and auditable operator actions.

## Goals / Non-Goals

**Goals:**
- Introduce runtime-managed workflow definitions for all supported workflow-backed business objects instead of relying on seed-only definitions.
- Support full lifecycle management for workflow definitions: draft, validation, publish, deactivate, and rollback to a prior published version.
- Model workflow templates with nodes, transitions, conditions, assignment rules, and object-specific applicability.
- Support assignee resolution strategies for fixed users, fixed roles, department leaders, submitter context, and document-field-based routing.
- Bind each workflow instance to an immutable published definition version so in-flight instances remain stable while new submissions adopt newer published versions.
- Provide a non-graphical administration UI that lets operators manage definitions, version histories, validation issues, and publication actions.
- Preserve existing workflow runtime guarantees for replay safety, audit history, reminder operations, and downstream side-effect idempotency.

**Non-Goals:**
- A graphical workflow editor, drag-and-drop canvas, or visual connection editing.
- Timeout-driven or escalation-driven workflow automation.
- Free-form scripting or arbitrary user-defined code execution inside workflow conditions or assignment rules.
- Redesigning the existing approval runtime into a generic BPM engine with gateways, timers, or parallel-branch semantics beyond what Phase 1 explicitly models.
- Reopening first-release exclusions such as membership, email delivery, or historical workflow-instance migration.

## Decisions

### 1. Separate workflow template identity from version identity

The system will distinguish between a long-lived **workflow template** and immutable **workflow template versions**.

- A template represents the business concept, such as `lease-approval` or `invoice-discount-approval`.
- A version captures the exact node graph, transition rules, and assignment configuration used at publication time.
- Workflow instances will store a foreign key to the published version they were created from, not just the template key.

**Why:** this is the safest way to satisfy the requirement that new instances adopt new behavior while in-flight instances remain stable. It also enables rollback without mutating historical execution semantics.

**Alternatives considered:**
- Mutating a single in-place definition record was rejected because it would make in-flight instances ambiguous and would weaken auditability.
- Snapshotting JSON into each workflow instance without a version table was rejected because it would make administration, rollback, comparison, and validation much harder.

### 2. Use explicit publication states instead of editing live definitions

Each template version will move through explicit lifecycle states such as `draft`, `validated`, `published`, `superseded`, and `deactivated`.

- Operators edit only draft versions.
- Publication creates the active version for new instances of the targeted business object.
- Rollback is implemented by publishing a previously valid version again as the current active version for future instances.
- Existing instances keep their original version binding regardless of later publication actions.

**Why:** this keeps change management explicit, auditable, and reversible without introducing unsafe live mutation.

**Alternatives considered:**
- Allowing direct edits to the currently published version was rejected because it complicates rollback and can change runtime semantics mid-flight.
- Deleting old versions was rejected because audit and historical execution review require retained definition history.

### 3. Constrain workflow modeling to a validated Phase 1 graph model

Phase 1 will support a structured graph model with:

- object type applicability
- sequential approval nodes
- explicit transitions
- node-level assignment rules
- transition conditions using a bounded condition DSL or typed condition model
- terminal outcomes such as approved/rejected/cancelled where applicable

The model will explicitly exclude arbitrary user scripting and advanced BPM constructs that are not required for first-release business flows.

**Why:** the business need is configurable approval routing, not an unconstrained workflow programming platform. A bounded model keeps validation, audit, and runtime predictability manageable.

**Alternatives considered:**
- Storing arbitrary scripting expressions was rejected for security, audit, and debugging reasons.
- Building a fully generic BPMN-style engine in Phase 1 was rejected as unnecessary complexity, especially with the graphical editor deferred.

### 4. Implement assignee resolution through typed strategies, not ad hoc per-flow logic

Assignment rules will be represented as explicit strategy types with structured configuration payloads. Supported strategies for Phase 1 are:

- fixed user
- fixed role
- department leader
- submitter context
- document-field-based resolution

Resolution will occur at workflow runtime using the business object context and organization/access-control data. Validation will ensure that each assignment rule references resolvable entities or resolvable field paths before publication.

**Why:** typed strategies make validation and audit tractable and prevent each flow from embedding custom resolution logic.

**Alternatives considered:**
- Hardcoding different resolution logic for each business object was rejected because it does not scale to all workflow-backed objects.
- Deferring resolution until action time without publication-time validation was rejected because it would shift too much failure risk into production execution.

### 5. Keep workflow-definition administration separate from runtime execution APIs

The backend will expose a dedicated administration API surface for templates, versions, validation, publication, rollback, and assignment-rule management. Existing runtime APIs for submit/approve/reject/resubmit/reminders will continue to operate against published versions and workflow instances.

**Why:** administration and runtime execution have different permission, audit, and failure semantics. Separating them avoids coupling operational runtime endpoints to definition editing behavior.

**Alternatives considered:**
- Extending current runtime endpoints to also mutate definitions was rejected because it conflates business actions with configuration management.

### 6. Preserve current reminder and idempotency behavior without broadening automation scope

Reminder execution, audit history, duplicate-safe replay behavior, and downstream side-effect idempotency remain part of the existing runtime subsystem. Phase 1 must ensure that moving to runtime-managed definitions does not weaken those guarantees.

- Reminder jobs continue to target workflow instances, not template drafts.
- Duplicate submission and duplicate action handling continue to resolve against the instance and its bound version.
- Publication changes affect only future instances.

**Why:** this is a behavior-preserving refactor at the runtime boundary, not a redesign of workflow side effects.

**Alternatives considered:**
- Reworking reminder or side-effect semantics at the same time was rejected because it would broaden the blast radius and obscure regression causes.

### 7. Introduce explicit permissions for workflow-definition administration

Organization-access-control will be extended with dedicated permissions for:

- viewing workflow definitions
- editing draft definitions
- validating definitions
- publishing / deactivating / rolling back versions
- managing assignment rules

These permissions will be separate from normal workflow approval permissions.

**Why:** the ability to change an approval system is materially different from the ability to participate in approval actions. It requires stronger authorization and clearer audit boundaries.

**Alternatives considered:**
- Reusing existing workflow-admin approve permissions was rejected because definition mutation and runtime approval are distinct operator responsibilities.

### 8. Deliver Phase 1 with a structured admin UI, not a graph canvas

The frontend will provide a non-graphical administration surface using forms, ordered node/transition tables, validation feedback panels, version history views, and publication dialogs.

**Why:** this supports the required operator workflows while keeping Phase 1 focused on the engine and data model. It also gives Phase 2 a stable API/model foundation for a later graphical editor.

**Alternatives considered:**
- Delivering the graph editor immediately was rejected by scope decision because it would dominate the change and delay the core configurability outcome.

## Risks / Trade-offs

- **[Version sprawl and operator confusion]** → Mitigate with explicit template/version history, single active published version per object/scope, and clear publication labels in the admin UI.
- **[Assignment rule misconfiguration causes blocked approvals]** → Mitigate with publication-time validation, dry-run previews where possible, and explicit unresolved-assignee diagnostics.
- **[Runtime behavior drift between old and new versions]** → Mitigate by immutable version binding on workflow instance creation and audit display of bound version metadata.
- **[Permission overreach allows unauthorized workflow changes]** → Mitigate with dedicated admin permissions, audit logging for all definition mutations, and no implicit inheritance from approval permissions.
- **[Phase 1 UI may feel less ergonomic without a graph canvas]** → Mitigate by making node order, transitions, and validation states explicit in the admin UI and treating the later graph editor as an enhancement, not a dependency.
- **[Data migration from seeded definitions to managed templates may be error-prone]** → Mitigate by importing seed definitions as initial managed templates/versions and validating parity before enabling runtime publication changes.
- **[Cross-object rollout complexity]** → Mitigate by standardizing template applicability and assignment-rule contracts across all supported workflow-backed objects instead of per-object ad hoc shapes.

## Migration Plan

1. Introduce persistent workflow template and template-version storage alongside the current runtime model.
2. Convert existing seeded definitions into managed baseline templates and baseline published versions.
3. Add administration APIs and non-graphical admin UI without switching runtime submission behavior yet.
4. Validate imported baseline definitions against current runtime behavior for Lease, billing, overtime, discount, and other workflow-backed objects.
5. Switch new workflow instance creation to resolve through the active published template version.
6. Enable controlled publication/deactivation/rollback operations for authorized administrators.
7. Keep rollback available by republishing a prior validated version for future instances.

Rollback strategy:

- If runtime regressions are found after rollout, republish the previously validated version as the active version for future instances.
- If the administration subsystem itself must be disabled, keep existing instance execution intact and temporarily freeze new definition publication actions.
- Historical instances remain auditable because they retain their original version binding.

## Open Questions

- Should workflow template applicability support one global active version per object type, or should there be additional scope dimensions such as department/store/business subtype in Phase 1?
- How expressive should the Phase 1 condition model be for transitions and assignee routing before it becomes too close to embedded scripting?
- Should publication require a secondary approval or four-eyes control for high-impact workflow objects?
- How should unresolved department-leader or document-field routing failures surface at submission time versus publication time?
- Which existing seeded definitions should be treated as mandatory baseline templates versus optional examples during migration?
