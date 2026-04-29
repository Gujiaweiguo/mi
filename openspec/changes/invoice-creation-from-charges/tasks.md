## 1. Table Row Selection

- [ ] 1.1 Add `selectedRows` ref (`ref<ChargeLine[]>([])`) and `handleSelectionChange` handler to `BillingChargesView.vue`
- [ ] 1.2 Add `<el-table-column type="selection" />` as the first column in the charges `el-table`
- [ ] 1.3 Bind `@selection-change="handleSelectionChange"` on the `el-table` element
- [ ] 1.4 Add `canCreateDocument` computed that returns `selectedRows.value.length > 0 && !isCreating.value`

## 2. Create-Document Action and Dialog

- [ ] 2.1 Add `isCreating` ref and `documentType` ref (`ref<'bill' | 'invoice'>('invoice')`) and `showCreateDialog` ref (`ref(false)`) to `BillingChargesView.vue`
- [ ] 2.2 Add "Create Bill/Invoice" button in PageSection `#actions` slot, disabled when `!canCreateDocument`, with `data-testid="create-document-button"`
- [ ] 2.3 Add `el-dialog` with `el-radio-group` for bill/invoice choice and confirm button, wired to `showCreateDialog` and `documentType`
- [ ] 2.4 Implement `handleCreateDocument` method: set `isCreating`, call `createInvoice({ document_type, billing_charge_line_ids: selectedRows.map(r => r.id) })`, handle success and error paths

## 3. Success and Error Feedback

- [ ] 3.1 On success: set feedback to success type with document ID, clear selection, reload charges, close dialog
- [ ] 3.2 On success: include router link to invoice detail (`/billing/invoices/:id`) in the feedback description or adjacent UI element
- [ ] 3.3 On error: set feedback to error type with API message from `getErrorMessage()`, keep dialog open for retry

## 4. i18n Messages

- [ ] 4.1 Add English i18n keys under `billingCharges.createDocument.*` namespace (button label, dialog title, radio labels, success message, error messages)
- [ ] 4.2 Add Chinese i18n keys matching the English keys

## 5. Unit Tests

- [ ] 5.1 Create `BillingChargesView.test.ts` with test for selection state tracking (select rows → selectedRows updates → canCreateDocument is true)
- [ ] 5.2 Add test for create-document flow: mock `createInvoice`, verify API called with correct payload, verify success feedback set, verify selection cleared
- [ ] 5.3 Add test for create-document error: mock API rejection, verify error feedback set, verify dialog remains open
- [ ] 5.4 Add test for create button disabled state when no rows selected

## 6. Typecheck and Build Verification

- [ ] 6.1 Run `vue-tsc --noEmit` and fix any type errors introduced
- [ ] 6.2 Run `npm run build` (or `vite build`) and confirm clean build
- [ ] 6.3 Run `vitest` and confirm all new and existing tests pass

## 7. Evidence

- [ ] 7.1 Record unit-test evidence to `artifacts/verification/<commit-sha>/unit.json` (CI gate: unit + integration pass)
- [ ] 7.2 Verify no regressions in existing billing charges view behavior (generation, filtering, pagination)
