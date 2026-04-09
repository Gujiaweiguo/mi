## Why

Lease → Bill/Invoice is the highest-priority operational chain for first release, but edge-case behavior across lifecycle boundaries still risks inconsistent billing inputs, duplicate downstream effects, or ambiguous rejection handling. We need explicit requirement hardening now so implementation and verification remain deterministic under replay, state drift, and cross-stage failures.

## What Changes

- Tighten Lease lifecycle-to-billing eligibility rules for boundary states and replayed transitions.
- Tighten Bill/Invoice generation and lifecycle rules for duplicate-safe processing across retries and partial failures.
- Add explicit consistency expectations for cross-stage transitions (Lease approved/effective state → charge generation → bill/invoice lifecycle) so invalid state combinations are rejected deterministically.
- Add concrete scenarios for exception handling (replayed actions, stale lifecycle state, invalid transition attempts) across the chain.

## Capabilities

### New Capabilities

<!-- None. -->

### Modified Capabilities

- `lease-contract-management`: strengthen billing-eligibility and replay-safe lifecycle boundary requirements.
- `billing-and-invoicing`: strengthen duplicate-safe charge/invoice processing and invalid-transition rejection at Lease-to-billing boundaries.

## Impact

- OpenSpec requirement deltas under:
  - `openspec/changes/lease-bill-workflow-hardening/specs/lease-contract-management/spec.md`
  - `openspec/changes/lease-bill-workflow-hardening/specs/billing-and-invoicing/spec.md`
- Downstream implementation and tests in Lease/Billing modules (backend workflows, API behavior, and e2e test flows) to align with hardened boundary semantics.
- Acceptance and verification scenarios for replay safety, state consistency, and deterministic rejection paths in the Lease → Bill/Invoice chain.
