## Context

The migration currently has a working operator-visible path from Lease creation through charge generation, invoice handling, and approval workflow actions. The remaining gap is not basic capability existence but behavioral hardening: duplicate submissions, repeated workflow actions, invalid status transitions, and billing eligibility edges are not yet documented strongly enough as a first-release contract.

This change should strengthen the core operational chain without widening scope into unrelated modules. The project is still a replacement migration with a Go modular monolith backend and a Vue 3 frontend, so the hardening work should preserve the current architecture and focus on service-layer invariants, auditability, and regression coverage.

## Goals / Non-Goals

**Goals:**
- Enforce Lease, billing, invoice, and workflow actions through explicit server-side state guards.
- Make duplicate submissions and replayed workflow actions safe, auditable, and free of duplicate downstream side effects.
- Clarify which Lease state is eligible for downstream billing and which document states allow further actions.
- Align operator-facing UI behavior and test coverage with the hardened lifecycle rules.

**Non-Goals:**
- No new business capability outside the first-release Lease → Bill / Invoice workflow chain.
- No redesign of the workflow engine, approval template model, or broader document architecture.
- No new asynchronous timeout/escalation automation.
- No localization, deployment, or cutover scope changes.

## Decisions

### 1. Enforce lifecycle guards in backend services, not only in the frontend

**Decision:** Lease, billing, invoice, and workflow commands should validate allowed source states in backend service logic before mutating persistence or dispatching side effects.

**Why:** The frontend can reduce accidental clicks, but the system contract must hold for replayed requests, stale clients, and direct API callers.

**Alternatives considered:**
- Rely only on button disabling in the Vue frontend: rejected because it does not protect API-level correctness.

### 2. Treat duplicate actions as safe, auditable replays

**Decision:** Replayed submit/approve-like actions should preserve the existing state and avoid duplicate side effects while still remaining observable in logs or workflow audit history where appropriate.

**Why:** The highest-risk failure mode in this workflow is duplicate billing or duplicate lifecycle effects triggered by retries.

**Alternatives considered:**
- Fail all duplicate requests with hard errors: rejected because safe replay behavior is more robust for retries and operator uncertainty.

### 3. Gate billing eligibility on billing-effective approved Lease state

**Decision:** Downstream charge generation should only consume Lease records that have reached the approved/billing-effective state required by current business rules, and it should ignore pending, rejected, cancelled, or otherwise non-billable states.

**Why:** The most valuable business guarantee is that billing remains deterministic and does not advance from an incomplete contract lifecycle.

**Alternatives considered:**
- Let billing infer eligibility opportunistically from partial workflow state: rejected because it obscures the contract and increases accidental charges.

### 4. Verify hardening with boundary-focused integration and e2e coverage

**Decision:** Verification should emphasize duplicate requests, invalid transitions, and guarded UI actions in addition to the happy path.

**Why:** The current path already demonstrates the basic flow; the missing confidence is around replay and edge handling.

**Alternatives considered:**
- Add only unit tests: rejected because the risk spans service coordination and operator-visible behavior.

## Risks / Trade-offs

- **[Risk] Stricter guards may expose pre-existing assumptions in tests or seed data** → **Mitigation:** update integration and Playwright fixtures together with the rule changes, and keep no-op replay behavior explicit where retries are expected.
- **[Risk] Duplicate-safe replay behavior can hide operator confusion if feedback is vague** → **Mitigation:** keep API/UI responses auditable and user-facing feedback explicit about current state.
- **[Risk] Billing eligibility changes could accidentally narrow valid records too far** → **Mitigation:** anchor the new rules to current approved/billing-effective lifecycle checkpoints and cover them with integration assertions.

## Migration Plan

1. Document the intended lifecycle invariants in delta specs for Lease, billing/invoicing, and workflow approvals.
2. Harden backend command handlers and any supporting repository checks to enforce those invariants.
3. Align operator-facing Lease/invoice actions so disabled states and feedback match backend behavior.
4. Extend integration and Playwright coverage for duplicate, invalid, and no-op replay scenarios.
5. Record commit-scoped `unit`, `integration`, and `e2e` evidence before archive.

## Open Questions

- Should duplicate user-driven submissions return a no-op success shape or a domain-specific conflict response, as long as they remain side-effect safe?
- Are rejected billing documents expected to support resubmission in the first release, or should resubmission remain out of scope until a later workflow refinement change?
