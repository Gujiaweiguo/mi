## Why

The first-release migration path already proves the happy path from Lease through billing and invoice workflow, but it still needs stronger guardrails around duplicate submissions, invalid lifecycle actions, and billing eligibility boundaries. This is the highest-value operational chain in scope, so we should harden it now before expanding into broader business slices.

## What Changes

- Harden Lease lifecycle commands so submission, approval-adjacent transitions, termination, and downstream billing eligibility are enforced by domain state rules instead of relying on UI behavior alone.
- Harden bill and invoice lifecycle actions so duplicate or invalid operations are safe, auditable, and do not create duplicate downstream side effects.
- Tighten workflow approval behavior for Lease and billing documents so replayed workflow actions preserve current state instead of producing duplicate effects.
- Add regression coverage for the core Lease → Bill / Invoice → Approval chain, including boundary and unhappy-path scenarios, and keep verification evidence aligned with the current commit.

## Capabilities

### New Capabilities

### Modified Capabilities

- `lease-contract-management`: Clarify and enforce state-guarded Lease lifecycle actions and billing-effective state transitions.
- `billing-and-invoicing`: Clarify billing eligibility, safe document lifecycle transitions, and duplicate-safe invoice operations.
- `workflow-approvals`: Extend approval workflow guarantees so replayed or invalid actions remain auditable without duplicating downstream side effects.

## Impact

- Affected backend code: Lease, billing, invoice, and workflow service/repository layers in the Go modular monolith.
- Affected frontend code: operator-facing Lease and invoice action surfaces that expose submission, termination, or other guarded workflow actions.
- Affected tests: backend unit/integration coverage and Playwright workflow coverage for happy path, replay path, and invalid-transition path behavior.
- Affected operations: the first-release Lease → Bill / Invoice approval chain becomes stricter and more deterministic, reducing accidental duplicate actions before broader rollout.
