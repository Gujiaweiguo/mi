## 1. Adjustment entry and status visibility

- [x] 1.1 Update supported invoice review surfaces to render the `adjusted` lifecycle status and include invoice-adjustment entry points for adjustment-eligible approved documents.
- [x] 1.2 Surface the original/replacement document relationship in the invoice detail experience so operators can understand adjustment lineage.

## 2. Adjustment drafting flow

- [x] 2.1 Add a focused adjustment drafting UI in `InvoiceDetailView.vue` that lets operators edit replacement line amounts using the existing invoice line context.
- [x] 2.2 Submit adjustment drafts through `adjustInvoice()` and redirect to the returned replacement draft document on success.
- [x] 2.3 Add adjustment-specific copy, success/error feedback, and eligibility guards that align with the current backend rules without changing the API contract.

## 3. Verification

- [x] 3.1 Add or update Vue unit tests that cover adjusted status rendering, adjustment entry, and successful adjustment submission/navigation behavior.
- [x] 3.2 Run the relevant frontend verification commands for the touched surfaces, including unit tests and typecheck/build, and fix any change-caused failures.
- [ ] 3.3 If this change is prepared for CI or archive evidence, record current-commit machine-readable verification artifacts under `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`, remembering that CI requires current-commit unit + integration evidence while archive additionally requires current-commit e2e evidence.
