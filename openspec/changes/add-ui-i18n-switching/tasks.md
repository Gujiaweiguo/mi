## 1. Locale foundation

- [x] 1.1 Add frontend localization dependencies and bootstrap wiring in `frontend/src/main.ts` for application messages and Element Plus locale integration.
- [x] 1.2 Create locale message resources for `zh-CN` and `en-US`, plus a shared locale resolver that defaults to Simplified Chinese.
- [x] 1.3 Add centralized locale state and browser-local persistence so the selected language survives refreshes in the same browser.

## 2. Language switching UX

- [x] 2.1 Add a runtime language switch entry point in the shared frontend shell and define the authenticated/unauthenticated visibility behavior.
- [x] 2.2 Wire the active locale so switching updates both app-authored copy and Element Plus framework locale text during the active session.

## 3. Shared copy migration

- [x] 3.1 Replace hard-coded English text in `frontend/src/App.vue` with locale-managed message keys.
- [x] 3.2 Replace hard-coded navigation labels in `frontend/src/auth/permissions.ts` with locale-managed labels.
- [x] 3.3 Migrate login/auth-facing copy and shared global error text to locale-managed messages.
- [x] 3.4 Migrate common platform components and other shared user-facing primitives that currently embed English copy.

## 4. First-wave screen localization

- [x] 4.1 Localize the first-wave representative operational/admin screens required for this change and document any explicitly deferred screens.
- [x] 4.2 Verify the default Simplified Chinese experience and English switch behavior on the first-wave screens without changing backend business behavior or APIs.

### First-wave deferrals for this batch

- Deferred beyond this batch: `BillingInvoicesView.vue`, `InvoiceDetailView.vue`, `LeaseCreateView.vue`, `LeaseDetailView.vue`, `GeneralizeReportsView.vue`, `VisualShopAnalysisView.vue`, `TaxExportsView.vue`, `PrintPreviewView.vue`, `MasterDataAdminView.vue`, `SalesAdminView.vue`, `BaseInfoAdminView.vue`, `StructureAdminView.vue`, `RentableAreaAdminView.vue`, and the generic `WorkbenchView.vue` scaffold copy.

## 5. Verification and evidence

- [x] 5.1 Add or update frontend unit tests for locale defaulting, locale persistence, and locale-driven shared label resolution.
- [x] 5.2 Add or update end-to-end coverage for language switching on key user-facing screens, including login and shared shell/navigation behavior.
- [ ] 5.3 Run the required verification for the current commit and record machine-readable evidence using the project convention: `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [ ] 5.4 Confirm the change satisfies current gate expectations: CI requires passing `unit` and `integration` evidence for the current commit, while archive requires passing `unit`, `integration`, and `e2e` evidence for the current commit.

Blocked for this commit (`4c6ce4319fab54ed5caef919ad2543105762a57c`): current-commit `unit.json` and `e2e.json` exist and pass for `add-ui-i18n-switching`, but `integration` evidence cannot be generated because the repository-wide integration suite is already failing outside this change in existing backend modules (`internal/billing`, `internal/docoutput`, `internal/invoice`, and `internal/taxexport`).
