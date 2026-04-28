## 1. Lease subtype and overtime billing

- [x] 1.1 Clarify subtype data requirements for standard, joint-operation, ad-board, and area/ground contracts using legacy references and current business acceptance input.
- [x] 1.2 Add backend schema/model/API support for approved subtype fields without introducing membership scope.
- [x] 1.3 Implement overtime billing formula configuration, approval workflow integration, and duplicate-safe overtime charge generation.
- [x] 1.4 Add frontend operator flows for subtype fields and overtime billing approval/generation where required.
- [x] 1.5 Add unit and integration tests for subtype validation, overtime formula calculation, approval, cancellation/stop, and duplicate prevention.

## 2. Invoice financial adjustments

- [x] 2.1 Implement approved invoice discount handling with rate/amount validation and receivable reduction.
- [x] 2.2 Implement surplus/customer balance tracking for overpayments and authorized application to open receivables.
- [x] 2.3 Implement late-payment interest rate configuration and idempotent interest charge/document generation.
- [x] 2.4 Implement deposit application and refund/release workflows with blocking-obligation validation.
- [x] 2.5 Add unit and integration tests for discount limits, surplus creation/application, interest calculation, deposit application/refund, and accounting audit trails.

## 3. Lease contract print output

- [x] 3.1 Decide whether `lease_contract` remains a supported print document type for first-release go-live.
- [x] 3.2 If supported, implement lease contract data loading and HTML/PDF rendering with template tests.
- [x] 3.3 If not supported, remove `lease_contract` from frontend/backend selectable trusted document types and update specs before archive. Not applicable: `lease_contract` is now supported.

## 4. Release verification

- [x] 4.1 Run backend unit and integration verification for changed lease, billing, invoice, workflow, print, and database code.
- [x] 4.2 Run frontend unit and E2E verification for new operator flows.
- [x] 4.3 Produce commit-scoped evidence files under `artifacts/verification/<commit-sha>/unit.json`, `integration.json`, and `e2e.json` before archive.
