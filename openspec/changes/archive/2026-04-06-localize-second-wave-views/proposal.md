## Why

The first UI localization change established the i18n foundation and covered the shared shell plus a first wave of representative screens, but a visible set of routed pages still remains English-first. Those deferred views now create an inconsistent operator experience because the application defaults to Simplified Chinese while several important operational and admin screens still expose English copy.

## What Changes

- Extend frontend localization coverage from the first-wave shell/common surfaces into the deferred second-wave routed views.
- Replace hard-coded English copy in the remaining targeted business, reporting, print-preview, and admin screens with locale-managed message keys.
- Add verification for the newly localized second-wave views so default Simplified Chinese behavior and English switching remain test-backed.
- Keep the existing i18n foundation, locale persistence, and language switch behavior unchanged; this change expands coverage rather than redesigning the localization system.

## Capabilities

### New Capabilities

### Modified Capabilities
- `ui-localization`: Extend the localization requirement from shared shell/common surfaces and first-wave screens to the deferred second-wave routed views.

## Impact

- Affected code: deferred routed Vue views such as `BillingInvoicesView.vue`, `InvoiceDetailView.vue`, `LeaseCreateView.vue`, `LeaseDetailView.vue`, `GeneralizeReportsView.vue`, `VisualShopAnalysisView.vue`, `TaxExportsView.vue`, `PrintPreviewView.vue`, `MasterDataAdminView.vue`, `SalesAdminView.vue`, `BaseInfoAdminView.vue`, `StructureAdminView.vue`, `RentableAreaAdminView.vue`, and `WorkbenchView.vue`.
- Affected tests: frontend unit coverage may need message-key updates, and Playwright coverage will need to validate language switching/default Chinese behavior on the newly localized second-wave screens.
- Affected UX: operators will see a more consistent Simplified Chinese default experience across routed pages while retaining runtime English switching.
