## 1. Backend sales batch persistence

- [x] 1.1 Add batch-oriented daily-sales and customer-traffic upsert methods to the `sales` package while preserving the existing single-record create/list flows.
- [x] 1.2 Validate batch inputs against the existing natural-key semantics so repeated imports remain idempotent for `(store_id, unit_id, sale_date)` and `(store_id, traffic_date)`.

## 2. Excel templates and import orchestration

- [x] 2.1 Extend `excelio` reference loading and template generation for daily sales and customer traffic workbooks using business-facing codes plus embedded reference data.
- [x] 2.2 Implement workbook parsing, row validation, and row-level diagnostics for daily sales imports with all-or-nothing commit behavior.
- [x] 2.3 Implement workbook parsing, row validation, and row-level diagnostics for customer traffic imports with all-or-nothing commit behavior.
- [x] 2.4 Add HTTP handlers and router wiring for sales/traffic template download and import submission under the existing Excel I/O surface.

## 3. Frontend sales admin import workflow

- [x] 3.1 Extend `frontend/src/api/sales.ts` with template-download and multipart import calls for daily sales and customer traffic.
- [x] 3.2 Update `SalesAdminView.vue` to expose per-section template download, workbook upload, success feedback, and row-level diagnostic rendering.
- [x] 3.3 Preserve and verify the existing single-record daily-sales and customer-traffic maintenance workflow as a fallback path beside batch import.

## 4. Verification and evidence

- [x] 4.1 Add backend automated coverage for template generation, invalid import rejection, deterministic batch upsert, and reference validation.
- [x] 4.2 Add frontend automated coverage for upload flow, diagnostics rendering, and coexistence of batch import with single-record entry.
- [x] 4.3 Run the verification suite required for the current commit and record machine-readable evidence in `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [x] 4.4 Confirm the current commit satisfies gate expectations: CI requires passing `unit` and `integration` evidence, while archive requires passing `unit`, `integration`, and `e2e` evidence for the same commit SHA.
