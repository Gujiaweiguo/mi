## 1. AR schema and lifecycle groundwork

- [x] 1.1 Add the database migration changes needed to support auditable payment tracking for first-release receivable operations, including the minimal payment-ledger persistence required by the design.
- [x] 1.2 Define and implement the repository/model structures needed to load, lock, create, and update document-linked receivable open items and recorded payment entries.
- [x] 1.3 Finalize the first-release invariants for receivable booking, settlement, and due-date derivation so approval, payment, cancellation, and adjustment flows share one deterministic contract.

## 2. Invoice-driven receivable lifecycle

- [x] 2.1 Extend the invoice approval workflow path so approved bills and invoices create or update receivable open items transactionally and replay-safely from document-line financial data.
- [x] 2.2 Extend cancellation and adjustment behavior so receivable state stays consistent with allowed document transitions and rejects unsupported post-payment edge cases where necessary.
- [x] 2.3 Ensure receivable booking preserves downstream reporting dimensions such as customer, department, trade, charge type, due date, and document linkage.

## 3. Payment application and receivable query APIs

- [x] 3.1 Implement backend payment-recording service logic that validates outstanding balance, rejects over-application, records immutable payment history, and applies balance reductions transactionally.
- [x] 3.2 Add authenticated HTTP handlers and router wiring for operator payment entry and outstanding receivable queries within the existing financial permission surface.
- [x] 3.3 Return operator-facing response shapes that expose current outstanding balance, settlement state, and auditable payment history for the affected receivable/document workflow.

## 4. Frontend operator workflow

- [x] 4.1 Add frontend API bindings and types for receivable query and payment entry operations.
- [x] 4.2 Update the relevant billing/invoice operator views to show outstanding receivable state and support document-linked payment recording.
- [x] 4.3 Add or preserve stable test selectors and operator feedback states needed to verify successful payment entry, over-application rejection, and settled-balance behavior.

## 5. Reporting consistency and verification

- [x] 5.1 Update backend integration coverage for approval-driven receivable booking, duplicate replay safety, payment application, over-application rejection, and settlement behavior.
- [x] 5.2 Update report-oriented verification so receivable-dependent outputs continue to align with workflow-driven `ar_open_items` data and payment-backed received metrics where applicable.
- [x] 5.3 Add frontend automated coverage for receivable review and payment entry flows, including invalid payment attempts and post-payment UI state updates.
- [x] 5.4 Run the verification suite for the current commit and record machine-readable evidence in `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [x] 5.5 Confirm the current commit satisfies gate expectations: CI requires passing `unit` and `integration` evidence, while archive requires passing `unit`, `integration`, and `e2e` evidence for the same commit SHA.
