## Context

The current Sell-domain implementation already supports daily sales and customer traffic as first-release operational data, but only through single-record create flows. `backend/internal/sales/sales.go` owns the existing persistence semantics and already uses natural-key `ON DUPLICATE KEY UPDATE` behavior for `(store_id, unit_id, sale_date)` and `(store_id, traffic_date)`. `backend/internal/excelio/service.go` already provides a working Excel pattern for unit data: downloadable template with reference data, workbook parsing, full-batch validation, row-level diagnostics, and all-or-nothing import.

This change is cross-cutting because it extends the Excel I/O subsystem, the Sell-domain service layer, HTTP routing, and the Sales admin UI together. The design therefore needs to preserve the existing modular-monolith boundaries while reusing as much of the proven unit-import pattern as possible.

## Goals / Non-Goals

**Goals:**
- Reuse the existing `excelio` template/download/parse/diagnostic workflow for daily sales and customer traffic imports.
- Add operator-facing batch import capability without removing the current single-record fallback in the Sell-domain UI.
- Keep batch persistence idempotent by reusing the same natural-key upsert semantics already used by single-record sales and traffic writes.
- Return deterministic import results with row-level diagnostics and no partial trusted write on invalid files.
- Keep the implementation inside the current Vue + Go modular monolith without introducing new dependencies or asynchronous processing infrastructure.

**Non-Goals:**
- No membership-related import paths.
- No asynchronous job queue, background worker, or import-history feature.
- No new external storage format beyond the existing `.xlsx` workbook approach.
- No replacement of the current single-record `createDailySale` / `createCustomerTraffic` workflows.
- No widening of reporting scope beyond feeding the already-frozen first-release reports with operationally maintainable data inputs.

## Decisions

### Decision: Reuse `excelio` for workbook handling, keep `sales` as the owner of persistence semantics

The Excel subsystem will remain responsible for template generation, workbook parsing, reference validation, diagnostic collection, and import orchestration. The `sales` package will remain the owner of daily-sales and customer-traffic persistence semantics, including the natural-key upsert behavior already used in the existing single-record create methods.

This split avoids duplicating sales-table SQL in two modules. It also keeps the design aligned with the modular monolith: `excelio` handles transport and workbook concerns, while `sales` handles domain writes.

**Alternative considered:** move sales upsert SQL directly into `excelio` alongside unit import persistence. Rejected because the `sales` package already owns the relevant write semantics, and duplicating them would create two sources of truth for the same domain behavior.

### Decision: Add batch-oriented sales import methods rather than rewriting existing create flows

The current single-record methods in `sales.Service` and `sales.Repository` will stay intact. New batch-oriented methods will be added for daily sales and customer traffic so workbook imports can use a transaction-friendly path without distorting the current CRUD API.

The batch methods will reuse the same business keys and last-write-wins upsert semantics already proven in the single-record methods. This keeps correction behavior consistent whether an operator fixes one row manually or reimports a workbook.

**Alternative considered:** loop over the existing single-record create methods from `excelio`. Rejected because that would couple batch import performance and transaction handling to record-by-record calls, making it harder to enforce deterministic all-or-nothing behavior.

### Decision: Use business-facing codes in templates, not raw internal IDs

The daily sales template will use columns such as `store_code`, `unit_code`, `sale_date`, and `sales_amount`. The customer traffic template will use `store_code`, `traffic_date`, and `inbound_count`. Reference sheets will be embedded in the workbook so operators can prepare valid files without querying internal IDs from the database.

This matches the existing unit-template pattern and makes the import path operationally usable. The server will resolve codes to IDs during validation and reject unknown references before any trusted write occurs.

**Alternative considered:** expose raw `store_id` / `unit_id` in the template. Rejected because it leaks internal identifiers into operator workflows and recreates the exact usability problem this change is trying to solve.

### Decision: Keep imports all-or-nothing with full diagnostic reporting

Sales and traffic imports will follow the same contract already used by `ImportUnits`: parse all rows, collect all diagnostics, reject the entire batch when diagnostics exist, and only commit when the workbook passes validation. The response contract will include `ImportedCount` plus row-level diagnostics.

This is required by the existing Excel spec behavior and makes retries deterministic. Operators can fix all reported issues and rerun the same workbook without worrying about partial writes having already changed operational data.

**Alternative considered:** partially commit valid rows and only reject invalid ones. Rejected because it weakens trust in import outcomes and complicates safe re-runs.

### Decision: Keep Excel endpoints under the existing Excel I/O surface

Template download and import submission for sales data will be added under the existing Excel-oriented HTTP surface rather than under `/sales/*` CRUD endpoints. This keeps workbook-driven behavior grouped with the existing unit-template and import routes, while the `/sales/*` endpoints remain focused on listing and single-record maintenance.

This also aligns with the capability boundary in OpenSpec: the change modifies Excel I/O behavior and Sell-domain behavior together, but workbook transport remains an Excel concern.

**Alternative considered:** place all batch-import endpoints under `/sales/*`. Rejected because it mixes workbook transport concerns into a domain CRUD surface that currently does not handle files.

### Decision: Extend `SalesAdminView.vue` with import controls per data family

The Sales admin screen will gain a template-download action and a workbook-upload/import action for each data family: daily sales and customer traffic. Import feedback will present imported counts on success and row-level diagnostics on failure. The existing single-record create forms and list filters will stay in place as the fallback path for isolated corrections.

This keeps the operator experience in one place instead of forcing users to switch to a separate import-only screen.

**Alternative considered:** build a separate batch-import page. Rejected because the current sales admin page already owns the relevant master-data context and operational feedback surface.

## Risks / Trade-offs

- **Large workbook memory usage** → Limit imports to practical first-release batch sizes and reuse the existing `excelize` pattern already accepted in the repository.
- **Cross-package coupling between `excelio` and `sales`** → Keep the coupling narrow by introducing a small import-oriented interface or service boundary rather than moving workbook logic into the `sales` package.
- **Reference data drift between downloaded templates and later imports** → Generate templates from live reference data on demand and fail fast on unknown codes during import.
- **Mock-friendly UI tests overstating backend contract confidence** → Cover the import path with backend integration tests around parsing, validation, and upsert behavior; use frontend tests for operator workflow and diagnostics rendering, not as the only contract evidence.
- **Operators relying only on bulk import and ignoring single-record correction** → Preserve the single-record UI so exceptions and one-off corrections remain fast.

## Migration Plan

1. Add batch-oriented daily-sales and customer-traffic persistence methods to the `sales` package while preserving current single-record APIs.
2. Extend `excelio` reference loading, template generation, row parsing, and import orchestration for the two new workbook types.
3. Add Excel download/import handlers and router wiring for daily sales and customer traffic templates/imports.
4. Extend the frontend sales API module with template-download and import-upload calls.
5. Extend `SalesAdminView.vue` with per-section download/upload controls and diagnostics feedback.
6. Add backend integration coverage for template generation, invalid workbook rejection, deterministic batch upsert, and reference validation.
7. Add frontend tests for upload flow, import feedback, and continued availability of single-record fallback behavior.

Rollback is low risk because the change is additive: the previous single-record path remains intact. If the batch import path proves unstable, the new endpoints and UI controls can be disabled while operators continue using the existing manual entry flow.

## Open Questions

- Should the sales and traffic templates expose only business codes, or should they also include optional human-readable names in the data sheet for operator convenience?
- Does the current HTTP upload stack need an explicit file-size cap beyond row-count validation for first-release operational batches?
- Is customer traffic definitively store-level only for first release, or is there any requirement to ingest traffic at unit/shop granularity later?
