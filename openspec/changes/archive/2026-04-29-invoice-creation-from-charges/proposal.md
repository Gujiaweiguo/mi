## Why

The first-release Lease → Charge → Invoice operational chain is broken at the invoice creation step. The backend `POST /api/invoices` endpoint is fully implemented and the frontend API client `createInvoice()` exists in `frontend/src/api/invoice.ts`, but no frontend view wires this API. Operators can generate charges (BillingChargesView) and manage invoices (BillingInvoicesView), but cannot create a new bill or invoice document from selected charge lines. This gap prevents the core billing workflow from completing end-to-end.

## What Changes

- Add row-selection capability to the charges table in `BillingChargesView.vue` so operators can select one or more charge lines
- Add a "Create Bill/Invoice" action that opens a dialog/drawer for the operator to choose document type (`bill` or `invoice`)
- Wire the action to the existing `createInvoice()` API client, passing `{ document_type, billing_charge_line_ids }` from the selected rows
- Show success feedback with a link to the newly created document
- Show validation error feedback when selected lines are not eligible (e.g., already invoiced, wrong status)

## Capabilities

### New Capabilities

_(none — this change fills an implementation gap in an existing capability)_

### Modified Capabilities

- `billing-and-invoicing`: Frontend operational flow for creating bill/invoice documents from charge lines. The canonical spec already requires "creation of bill and invoice documents" (`openspec/specs/billing-and-invoicing/spec.md`). This change adds the frontend wiring that implements that requirement. No backend API or spec-level requirement changes — the gap is purely in the frontend view layer.

## Impact

- **Frontend files modified**:
  - `frontend/src/views/BillingChargesView.vue` — add row selection, create-action button, document-type dialog, API call, and feedback
- **Frontend API client**: Uses existing `createInvoice()` from `frontend/src/api/invoice.ts` (line 266) with `CreateInvoiceRequest` type (line 45) — no new API functions needed
- **Backend**: No changes — `POST /api/invoices` is fully implemented
- **Dependencies**: No new dependencies; relies on existing Element Plus table selection and dialog components
