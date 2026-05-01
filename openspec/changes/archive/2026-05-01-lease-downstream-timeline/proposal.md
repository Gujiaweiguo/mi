## Why

Operators can already review a lease contract and manage overtime bills from `LeaseDetailView`, but they cannot see the downstream charges, invoices, and receivables that determine whether the lease has actually progressed through the first-release financial chain. The backend and frontend API layers already support most of the required lease-linked queries, so this is a high-value visibility gap that can be closed with a focused UI enhancement plus a small receivable filter addition.

## What Changes

- Add a downstream business panel to `LeaseDetailView` that shows lease-linked overtime bills, billing charges, invoices, and receivables in one operator-facing surface
- Add quick-entry links from the lease detail view into downstream document detail or list flows where supported
- Show downstream records in chronological / status-aware order so operators can understand the lease-to-cash progression without cross-module manual searches
- Extend receivable list filtering to support `lease_contract_id` so the lease detail view can query receivable state directly instead of relying on client-side workarounds
- Add frontend tests for the new downstream panel behavior and the receivable filter usage

## Capabilities

### New Capabilities

_(none — this change extends existing first-release lease and financial capabilities)_

### Modified Capabilities

- `lease-contract-management`: Add operator-visible downstream timeline / summary expectations from lease detail so approved or active contracts expose their linked overtime, billing, invoice, and receivable progression
- `billing-and-invoicing`: Add lease-linked downstream review requirements so charges, invoices, and receivables can be queried and surfaced from the originating lease context

## Impact

- **Frontend**: `frontend/src/views/LeaseDetailView.vue` gains a downstream timeline / summary panel and quick links
- **Frontend API clients**: existing lease, billing, and invoice clients are reused; receivable query params will be extended to accept `lease_contract_id`
- **Backend**: small receivable filter enhancement in invoice/receivable query path so `/receivables` can filter by `lease_contract_id`
- **Tests**: frontend unit coverage for the new downstream panel and backend coverage for the receivable filter if needed
