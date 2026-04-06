## 1. Lease and document lifecycle guards

- [x] 1.1 Audit the current Lease, billing, invoice, and workflow state transitions and codify the allowed command preconditions in the relevant backend services.
- [x] 1.2 Enforce duplicate-safe Lease submit and termination behavior so replayed or invalid actions preserve state and do not dispatch duplicate workflow side effects.
- [x] 1.3 Enforce duplicate-safe bill/invoice lifecycle actions so replayed submit, approve, cancel, or equivalent actions preserve state and avoid duplicate downstream effects.

## 2. Billing eligibility hardening

- [x] 2.1 Restrict charge generation to approved, billing-effective Lease state and explicitly exclude pending or otherwise non-billable contracts.
- [x] 2.2 Align invoice and related document action handling with the hardened lifecycle state rules so downstream output generation only runs from valid document states.

## 3. Operator-facing alignment

- [x] 3.1 Update Lease and invoice operator-facing actions, disabled states, and feedback messages so the frontend reflects the hardened backend lifecycle rules.
- [x] 3.2 Preserve or add stable selectors only where needed so replay, invalid-transition, and guarded-action behavior remains testable in Playwright.

## 4. Verification and evidence

- [x] 4.1 Add or extend backend unit/integration coverage for duplicate submissions, invalid transitions, and billing-eligibility boundary cases across Lease, billing, invoice, and workflow modules.
- [x] 4.2 Add or extend Playwright coverage for the Lease → Bill / Invoice → Approval chain so replayed actions and invalid operator flows remain regression-tested.
- [x] 4.3 Run the required verification for the current commit and record machine-readable evidence using `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [x] 4.4 Confirm the change satisfies current gate expectations: CI requires passing `unit` and `integration` evidence for the current commit, while archive requires passing `unit`, `integration`, and `e2e` evidence for the current commit.
