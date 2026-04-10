## Why

The first release explicitly excludes timeout/escalation automation, but we still need a clear and testable boundary definition so future extensions can be planned without destabilizing the current approval workflow guarantees.

## What Changes

- Define requirement-level research boundaries for timeout/escalation automation as a future capability, without enabling it in first release.
- Define explicit preconditions, invariants, and non-goals for introducing timeout/escalation in follow-up phases.
- Define observability and idempotency expectations that any future timeout/escalation design must satisfy before implementation.

## Capabilities

### New Capabilities

<!-- None. -->

### Modified Capabilities

- `workflow-approvals`: add a boundary-and-readiness research contract for timeout/escalation so first-release exclusions stay explicit and future rollout criteria remain verifiable.

## Impact

- OpenSpec delta under `openspec/changes/workflow-timeout-escalation-boundary-research/specs/workflow-approvals/spec.md`.
- No runtime behavior change in current release.
- Planning/architecture guidance for future workflow automation extensions.
