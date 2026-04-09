## Context

Lease → Bill/Invoice is the first-release priority chain and spans multiple modules: Lease lifecycle, charge generation, and billing document lifecycle. Current specs already cover normal flows and basic duplicate safety, but boundary exceptions (state drift, replay after partial progress, and cross-stage inconsistency) need tighter and testable requirements so implementations converge on deterministic behavior.

## Goals / Non-Goals

**Goals:**
- Harden requirement-level behavior for Lease billing eligibility boundaries.
- Harden requirement-level behavior for Bill/Invoice generation and lifecycle under retries and invalid state transitions.
- Add explicit cross-stage consistency scenarios so rejected operations preserve current valid state.

**Non-Goals:**
- No new business domain or module introduction.
- No schema or infrastructure change unrelated to Lease/Billing workflow hardening.
- No redesign of approval topology beyond first-release constraints.

## Decisions

### 1) Modify existing capabilities instead of adding new capability
This change refines first-release behavior in existing capability boundaries (`lease-contract-management`, `billing-and-invoicing`) rather than introducing a separate capability.

### 2) Boundary-focused hardening with deterministic rejection semantics
Requirements will explicitly describe what must happen when actions are replayed, when lifecycle state has changed since request creation, and when operations are attempted from invalid source states.

### 3) Cross-stage consistency contract
The chain from Lease billing-effective state to generated charges and invoice lifecycle will require consistency preservation: invalid transitions must be rejected without duplicate downstream financial side effects.

### 4) Scenario-first verification shape
Each hardened requirement will include concrete WHEN/THEN scenarios to drive both API/integration tests and e2e coverage.

## Risks / Trade-offs

- **[Risk] Over-constraining edge behavior may slow implementation** → **Mitigation:** keep hardening focused on deterministic replay/idempotency and invalid-transition rejection, not new feature expansion.
- **[Risk] Ambiguous ownership between Lease and Billing modules** → **Mitigation:** define Lease-side eligibility rules and Billing-side mutation rules separately with explicit handoff scenarios.
- **[Risk] Existing implementations may differ from tightened wording** → **Mitigation:** require incremental alignment via apply tasks and test-backed verification.

## Migration Plan

1. Add OpenSpec deltas for `lease-contract-management` and `billing-and-invoicing`.
2. Add implementation tasks for service logic, validation guards, and replay-safe behavior checks.
3. Validate by running relevant workflow, billing, and e2e test slices before archive.

## Open Questions

- Should we introduce a shared idempotency key convention across Lease and Billing APIs in a follow-up change, or keep idempotency behavior internal at service boundaries for first release?
