## Why

Four list views (Lease, Invoices, Charges, Receivables) call backend paginated APIs that accept `page`/`page_size` parameters and return `PaginatedResponse<T>`, but none of these views pass pagination parameters or render pagination controls. Users can only see the first page of results. There is no reusable pagination composable — the only working pagination exists in `MasterDataCustomerBrandSection` with duplicated boilerplate.

## What Changes

- Create a `usePagination` composable (`frontend/src/composables/usePagination.ts`) that encapsulates page, pageSize, total state and a `paginationParams` computed property for API calls.
- Add `<el-pagination>` to LeaseListView, BillingInvoicesView, BillingChargesView, and ReceivablesView.
- Pass `page` and `page_size` parameters to the existing API calls in each view.
- Reset page to 1 when filters change.
- No backend changes — all 4 APIs already support pagination.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: add reusable frontend pagination composable and wire pagination controls to the four main business list views.

## Impact

- `frontend/src/composables/usePagination.ts`: new file (~30 lines)
- `frontend/src/views/LeaseListView.vue`: add pagination params + `<el-pagination>`
- `frontend/src/views/BillingInvoicesView.vue`: add pagination params + `<el-pagination>`
- `frontend/src/views/BillingChargesView.vue`: add pagination params + `<el-pagination>`
- `frontend/src/views/ReceivablesView.vue`: add pagination params + `<el-pagination>`
- Zero backend changes. Zero API contract changes.
