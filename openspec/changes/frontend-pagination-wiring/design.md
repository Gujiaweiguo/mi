## Context

The backend already supports offset-based pagination for all 4 target endpoints (`GET /api/leases`, `GET /api/invoices`, `GET /api/billing/charges`, `GET /api/receivables`) via `page` and `page_size` query parameters, returning `{items, total, page, page_size}`. The frontend `PaginatedResponse<T>` type already exists in `api/types.ts`. However, none of the 4 list views pass these parameters or render `<el-pagination>`.

## Goals / Non-Goals

**Goals:**
- Every business list view lets users navigate beyond the first page of results.
- Pagination state (page, pageSize, total) is managed via a single reusable composable.
- Filter changes reset the page to 1.

**Non-Goals:**
- Adding pagination to admin/reference data views (structure, baseinfo, workflow, sales).
- Backend pagination deduplication (shared types, normalizePage helpers).
- Cursor-based pagination.
- Persisting pagination state across navigation (keep-alive or URL query params).

## Decisions

### Decision 1 — Composable over component

**Choice:** Create `usePagination` composable, not a shared `<PaginationBar>` component.

**Rationale:** Each view already has `<el-pagination>` available from Element Plus. A composable handles the state logic (page, pageSize, total, reset) while each view keeps its own `<el-pagination>` markup with consistent props. This avoids over-abstracting the UI layer.

### Decision 2 — Default page size = 20

**Choice:** Match the backend default (`DefaultPageSize = 20`).

**Rationale:** Consistency with backend behavior. The backend already clamps page_size to a max of 100.

### Decision 3 — Filter submit resets page to 1

**Choice:** When `FilterForm` emits `submit`, reset page to 1 before loading.

**Rationale:** After changing filters, the total result set changes. Staying on page 5 of a new filtered set could show an empty page.

## Risks / Trade-offs

- **No URL query param sync** → Page state is lost on navigation. Acceptable for first release; can be added later.
- **Charges view has dual mode** (list + generate) → After generating, the generated lines replace the table data without pagination. Only the list mode needs pagination.
