## Why

The backend `POST /tax/rule-sets` endpoint (`UpsertRuleSet`) is fully implemented and production-ready. It accepts a complete rule set payload with code, name, document type, and an array of rule entries, and returns the persisted rule set. However, the frontend `TaxExportsView.vue` can only list existing rule sets and export tax vouchers. There is no way for operators to create or edit rule sets from the UI. This blocks the full tax export workflow: operators must have API-level access or rely on seed data to define the accounting rules that drive voucher exports.

## What Changes

- Add an `upsertTaxRuleSet` API function to `frontend/src/api/tax.ts` that calls `POST /tax/rule-sets` with the full rule set payload.
- Add a create/edit dialog to `frontend/src/views/TaxExportsView.vue` with a rule entry table, triggered by an "Add Rule Set" button in the rule sets card header and by clicking a table row for editing.
- Add i18n labels for all dialog fields, buttons, and feedback messages to both `zh-CN.ts` and `en-US.ts`.
- No backend changes. No new routes. No new Vue files.

## Capabilities

### New Capabilities

- `tax-export`: Frontend CRUD for tax rule set management, enabling operators to create and edit tax accounting rule sets directly from the tax exports view.

### Modified Capabilities

- None. The tax export workflow itself (voucher generation, rule set listing) is unchanged; this change adds the missing management surface.

## Impact

- `frontend/src/api/tax.ts`: add `upsertTaxRuleSet` function and request interface.
- `frontend/src/views/TaxExportsView.vue`: add dialog component, dialog state, row-click handler, and submit logic.
- `frontend/src/i18n/messages/zh-CN.ts` and `en-US.ts`: add labels under the existing `taxExports` namespace for dialog fields, actions, and feedback.
- No backend, routing, or database changes.
