## Context

The first-release Lease → Charge → Invoice operational chain has a gap at the invoice-creation step. The backend `POST /api/invoices` endpoint is fully implemented and returns `{ document: InvoiceDocument }`. The frontend API client `createInvoice()` exists in `frontend/src/api/invoice.ts` (line 266). The `BillingChargesView.vue` displays charge lines in an `el-table` with pagination and filtering. No row-selection or document-creation action exists on this view.

This change is purely frontend: add table row selection, a create-document action, a document-type chooser dialog, and wire to the existing API. No backend changes, no new API endpoints, no new files beyond i18n additions.

## Goals / Non-Goals

**Goals:**
- Allow operators to select one or more charge lines in BillingChargesView
- Provide a "Create Bill/Invoice" action that collects a document type choice and calls `createInvoice()`
- Show success feedback with the created document ID and a navigation link to the invoice detail view
- Show validation error feedback when the API rejects ineligible charge lines
- Maintain existing table pagination, filtering, and generation behavior unchanged

**Non-Goals:**
- Backend API changes (endpoint already implemented)
- New API client functions (`createInvoice` already exists)
- Bulk eligibility pre-checking before submission (the API validates eligibility server-side)
- New standalone views or routes
- Modifying the invoice detail view or invoice list view
- Modifying canonical specs (this is an implementation-gap fill, not a spec change)

## Decisions

### 1. Use Element Plus table selection (`type="selection"` column + `@selection-change`)

**Rationale**: The existing `el-table` in `BillingChargesView.vue` (line 279) already uses `row-key="id"`. Adding a selection column is a single `<el-table-column type="selection" />` addition. Element Plus handles cross-page selection clearing automatically when data reloads. This matches the pattern used in other list views in the project.

**Alternative considered**: Manual checkbox column with v-model binding — rejected because Element Plus provides the selection column as a built-in feature with `@selection-change` callback, which is simpler and more consistent.

### 2. Track selected rows as `ref<ChargeLine[]>([])` with `@selection-change` handler

**Rationale**: The `@selection-change` event fires with the currently selected rows. Storing them in a reactive ref gives a computed `canCreateDocument` that disables the action button when no rows are selected. Selection is cleared automatically when `loadCharges()` replaces `rows.value`.

### 3. Document-type dialog using `el-dialog` with `el-radio-group`

**Rationale**: The `CreateInvoiceRequest` requires `document_type: 'bill' | 'invoice'`. A small dialog with a radio group and confirm button is the minimal UX. This avoids a separate route or drawer for what is essentially a two-option choice. Pattern matches `el-dialog` usage in other frontend changes (e.g., invoice-adjustment-frontend).

**Alternative considered**: Inline dropdown next to the button — rejected because a dialog gives space for a confirmation step and prevents accidental clicks.

### 4. Create action button in PageSection `#actions` slot

**Rationale**: The existing generate button already lives in the `PageSection` `#actions` slot (line 188). Adding the create button there keeps the action bar consistent. The button is disabled when `selectedRows.length === 0` or when `isCreating` is true.

### 5. Success feedback shows document ID with router link

**Rationale**: After `createInvoice()` resolves with `{ document: InvoiceDocument }`, the feedback alert should include the document ID and a clickable link to the invoice detail route. This follows the success-feedback pattern already established in the view (the `lastGeneratedRun` display).

### 6. Error feedback from API validation

**Rationale**: The backend validates eligibility (already-invoiced lines, wrong status, etc.) and returns error responses. The frontend should display the error message from the API in the existing `feedback` ref pattern. No client-side pre-validation needed — the API is the source of truth.

### 7. No new composables or components

**Rationale**: The change is small enough to keep all logic in `BillingChargesView.vue` using local refs and methods. The existing `useFilterForm`, `usePagination`, and `getErrorMessage` composables cover all shared needs.

## Risks / Trade-offs

- **Cross-page selection lost on pagination**: When the user paginates, Element Plus table selection is lost because `rows` is replaced. This is acceptable for the first release because operators typically create documents from a single page of results. → Mitigation: Document this behavior; future iteration could add persistent selection via a composable if needed.

- **No client-side eligibility preview**: Operators only discover ineligible lines after submission. → Mitigation: API error messages are specific. The feedback pattern already handles this. A future iteration could add a visual indicator for already-invoiced lines if the list API returns that status.

- **Dialog minimalism**: The document-type dialog is intentionally minimal (radio + confirm). If more options are needed later (e.g., remarks, custom numbering), the dialog can be extended. → Mitigation: Accept minimal v1; the dialog structure is easy to extend.
