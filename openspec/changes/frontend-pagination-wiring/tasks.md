## 1. Pagination Composable

- [x] 1.1 Create `frontend/src/composables/usePagination.ts` with `page`, `pageSize`, `total` refs (defaults: 1, 20, 0), a `paginationParams` computed returning `{ page, page_size }`, and a `resetPage()` function that sets page to 1
- [x] 1.2 Verify `vue-tsc --noEmit` passes

## 2. Wire LeaseListView

- [x] 2.1 Import and use `usePagination` in `LeaseListView.vue`, spread `paginationParams` into `listLeases()` call, update `total` from response, add `<el-pagination>` below the table, reset page on filter submit

## 3. Wire BillingInvoicesView

- [x] 3.1 Import and use `usePagination` in `BillingInvoicesView.vue`, spread `paginationParams` into `listInvoices()` call, update `total` from response, add `<el-pagination>` below the table, reset page on filter submit

## 4. Wire BillingChargesView

- [x] 4.1 Import and use `usePagination` in `BillingChargesView.vue`, spread `paginationParams` into `listCharges()` call, update `total` from response, add `<el-pagination>` below the table, reset page on filter submit

## 5. Wire ReceivablesView

- [x] 5.1 Import and use `usePagination` in `ReceivablesView.vue`, spread `paginationParams` into `listReceivables()` call, update `total` from response, add `<el-pagination>` below the table, reset page on filter submit

## 6. Verification

- [x] 6.1 Run `vue-tsc --noEmit` from `frontend/` — typechecks cleanly
