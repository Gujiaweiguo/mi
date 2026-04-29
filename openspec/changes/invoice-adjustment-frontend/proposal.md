## Why

Invoice adjustment is already part of the first-release billing lifecycle in `openspec/specs/billing-and-invoicing/spec.md`, and the backend plus frontend API already support it, but operators still cannot trigger or review adjustments from the Vue application. This leaves an explicitly supported financial correction flow unreachable in day-one operations and creates a gap in the bill / invoice lifecycle after approval.

## What Changes

- Add operator-facing invoice adjustment entry points from the supported invoice review surfaces.
- Define the first-release frontend behavior for drafting an invoice adjustment from an approved document, submitting updated line amounts through the existing adjustment API, and routing operators to the newly created draft document.
- Surface adjusted-document status and origin linkage so the original and replacement documents remain understandable in list and detail views.
- Add focused frontend automated coverage for adjustment entry, submission, and adjusted-document status rendering.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `billing-and-invoicing`: extend the first-release invoice lifecycle requirement so adjustment becomes operator-reachable from the frontend and adjusted documents remain visible with status/origin context in the supported invoice review surfaces.

## Impact

- Frontend: `frontend/src/views/InvoiceDetailView.vue`, `frontend/src/views/BillingInvoicesView.vue`, related tests, and invoice i18n strings.
- Frontend API reuse: `frontend/src/api/invoice.ts` adjustment request path remains the integration contract.
- Verification: focused Vue tests for adjustment actions, adjusted status rendering, and successful navigation to the replacement draft document.
