## 1. API layer

- [ ] 1.1 Add an `UpsertTaxRuleSetRequest` interface to `frontend/src/api/tax.ts` with fields matching the backend's `upsertTaxRuleSetRequest` struct (code, name, document_type, rules array with sequence_no, entry_side, charge_type_filter, account_number, account_name, explanation_template, use_tenant_name, is_balancing_entry).
- [ ] 1.2 Add an `upsertTaxRuleSet` function to `frontend/src/api/tax.ts` that sends a `POST /tax/rule-sets` request with the typed payload and returns the backend response.

## 2. Dialog component and state

- [ ] 2.1 Add dialog visibility state, form model (code, name, document_type, rules array), and an editing flag to `TaxExportsView.vue` script setup to track whether the dialog is in create or edit mode.
- [ ] 2.2 Add an `openCreateDialog` function that resets the form model to empty defaults and sets the dialog visible.
- [ ] 2.3 Add an `openEditDialog` function that populates the form model from a selected `TaxRuleSet` and sets the dialog visible in edit mode.
- [ ] 2.4 Add an `addRuleRow` function that appends a new empty rule entry with an auto-incremented `sequence_no`, and a `removeRuleRow` function that removes a rule entry by index without renumbering remaining entries.

## 3. Dialog template

- [ ] 3.1 Add an `el-dialog` to `TaxExportsView.vue` template with a header form (code, name, document_type select with invoice/receipt options) and a rules table showing columns for sequence_no, entry_side (debit/credit select), charge_type_filter, account_number, account_name, explanation_template, use_tenant_name (checkbox), is_balancing_entry (checkbox), and a remove action column.
- [ ] 3.2 Add "Add Row" button below the rules table and a "Submit" button in the dialog footer that calls `upsertTaxRuleSet` with the form model, handles success (close dialog, reload rule sets, show feedback) and error (show feedback, keep dialog open).
- [ ] 3.3 Add an "Add Rule Set" button to the rule sets card header (next to the existing total tag) that calls `openCreateDialog`, and wire row-click on the rule sets table to call `openEditDialog` with the clicked row's data.

## 4. i18n labels

- [ ] 4.1 Add i18n keys under `taxExports.dialog` in both `zh-CN.ts` and `en-US.ts` for dialog title (create/edit), field labels (code, name, documentType, rule fields), action labels (addRuleSet, addRow, removeRow, submit), entry side options (debit/credit), document type options (invoice/receipt), and success/error feedback messages for upsert.

## 5. Verification

- [ ] 5.1 Run the frontend typecheck and build to confirm no type errors or build regressions from the added dialog, API function, and i18n keys.
