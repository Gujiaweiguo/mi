## Why

Workflow timeout/escalation is intentionally excluded from first release, but teams still need a low-risk automation step to reduce manual follow-up overhead for pending approvals. A reminder-only POC provides execution experience without changing approval outcomes.

## What Changes

- Add a reminder-only workflow automation slice that detects pending approval instances and emits reminder records/events.
- Keep approval decisions user-driven: no auto-approve, auto-reject, or escalation side effects.
- Define idempotent reminder behavior and audit visibility requirements for repeated reminder runs.

## Capabilities

### New Capabilities

<!-- None. -->

### Modified Capabilities

- `workflow-approvals`: add reminder-only automation requirements with explicit non-goals (no timeout/escalation decision automation) and idempotent replay expectations.

## Impact

- OpenSpec delta under `openspec/changes/workflow-auto-reminder-poc/specs/workflow-approvals/spec.md`.
- Future implementation scope likely touches workflow scheduler/trigger path, audit records, and reminder delivery adapter boundaries.
- No change to first-release approval decision semantics.
