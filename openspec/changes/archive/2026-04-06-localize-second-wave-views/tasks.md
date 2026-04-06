## 1. Billing and invoice views

- [x] 1.1 Localize `BillingInvoicesView.vue` and `InvoiceDetailView.vue`, including page-section copy, filter labels, table labels, status labels, action labels, and feedback messages.
- [x] 1.2 Update any billing/invoice Playwright assertions or selectors affected by second-wave localization changes.

## 2. Lease views

- [x] 2.1 Localize `LeaseCreateView.vue` and `LeaseDetailView.vue`, including form labels, validation messages, action labels, summary text, and feedback states.
- [x] 2.2 Verify that localized lease flows still preserve the existing business behavior and stable test selectors.

## 3. Reporting, tax, print, and visual-analysis views

- [x] 3.1 Localize `GeneralizeReportsView.vue`, `VisualShopAnalysisView.vue`, `TaxExportsView.vue`, and `PrintPreviewView.vue`, including report filters, action labels, summary text, and user-facing feedback.
- [x] 3.2 Decide and implement the intended treatment of report option labels in this wave, while keeping stable report IDs/codes unchanged.

## 4. Admin and operations console views

- [x] 4.1 Localize `MasterDataAdminView.vue`, `SalesAdminView.vue`, `BaseInfoAdminView.vue`, `StructureAdminView.vue`, and `RentableAreaAdminView.vue` using the established second-wave message taxonomy.
- [x] 4.2 Localize the remaining scaffold/admin-facing shell copy in `WorkbenchView.vue` and any shared deferred admin feedback states reached by this batch.

## 5. Verification and evidence

- [x] 5.1 Add or update Playwright coverage for the newly localized second-wave views so default Simplified Chinese behavior and runtime English switching are verified on the affected screens.
- [x] 5.2 Run the required verification for the current commit and record machine-readable evidence using the project convention: `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [x] 5.3 Confirm the change satisfies current gate expectations: CI requires passing `unit` and `integration` evidence for the current commit, while archive requires passing `unit`, `integration`, and `e2e` evidence for the current commit.
