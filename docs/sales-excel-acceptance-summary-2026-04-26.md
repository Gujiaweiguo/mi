# Sales / Excel Acceptance Summary (2026-04-26)

## Scope

This acceptance summary covers the first-release Sales / POS / commercial-transaction capture slice together with the mandatory Excel import/export flows used to refresh operational datasets at business scale.

## Spec baseline

- `openspec/specs/supporting-domain-management/spec.md`
- `openspec/specs/excel-import-export/spec.md`
- `openspec/specs/tax-document-and-excel-output/spec.md`
- `docs/output-catalog.md`

## Validated head

- Validated repository head: `e0ba41653750c59b270ad99635a3ab79747b9a7c`
- Verification root: `artifacts/verification/e0ba41653750c59b270ad99635a3ab79747b9a7c/`
  - `unit.json` — PASS (`766/766`)
  - `integration.json` — PASS (`581/581`)
  - `e2e.json` — PASS (`42/42`)

## Executed checks

### Sales admin E2E coverage

1. `task16-sales-admin.spec.ts` — workbook download/import alongside single-record maintenance
   - Exercises daily-sales template download, daily-sales workbook import, manual daily-sale creation, traffic template download, traffic workbook import, and manual traffic creation.
   - Result: PASS

2. `task16-sales-admin.spec.ts` — import diagnostics with manual fallback preserved
   - Exercises row-level diagnostics for invalid daily-sales imports while preserving single-record fallback.
   - Result: PASS

3. `task16-sales-admin.spec.ts` — daily-sales filtering by store and date range
   - Exercises operator-facing retrieval and filtering behavior after sales data exists.
   - Result: PASS

### Excel I/O integration coverage

4. `TestExcelIOServiceDownloadsUnitTemplate`
   - Verifies unit template generation includes expected headers and reference-sheet data.
   - Result: PASS

5. `TestExcelIOServiceImportsUnitsDeterministically`
   - Verifies unit imports commit trusted rows and upsert deterministically on reimport.
   - Result: PASS

6. `TestExcelIOServiceRejectsInvalidImportWithoutPartialCommit`
   - Verifies invalid unit imports return row-level diagnostics and do not partially commit data.
   - Result: PASS

7. `TestExcelIOServiceExportsOperationalDatasets`
   - Verifies operational workbook exports for `invoices` and `billing_charges` with expected headers and business keys.
   - Result: PASS

8. `TestExcelIOServiceDownloadsSalesTemplates`
   - Verifies daily-sales and traffic templates expose required headers and reference guidance.
   - Result: PASS

9. `TestExcelIOServiceImportsSalesDataDeterministically`
   - Verifies daily-sales and traffic workbook imports persist rows and upsert deterministically on reimport.
   - Result: PASS

10. `TestExcelIOServiceRejectsInvalidSalesImportsWithoutPartialCommit`
    - Verifies invalid daily-sales and traffic imports return diagnostics and prevent partial commits.
    - Result: PASS

### Request-validation coverage

11. `backend/internal/http/handlers/sales_test.go`
    - Verifies sales and traffic create/list endpoints reject invalid JSON, missing required fields, and invalid date/store filters.
    - Result: PASS

12. `backend/internal/http/handlers/excelio_test.go`
    - Verifies Excel import endpoints reject missing session context and missing uploaded files.
    - Result: PASS

### Full verification gates on validated head

13. `scripts/verification/run-unit.sh e0ba41653750c59b270ad99635a3ab79747b9a7c`
    - Result: PASS

14. `scripts/verification/run-integration.sh e0ba41653750c59b270ad99635a3ab79747b9a7c`
    - Result: PASS

15. `scripts/verification/run-e2e.sh e0ba41653750c59b270ad99635a3ab79747b9a7c`
    - Result: PASS

16. `scripts/archive-ready.sh`
    - Result: PASS (`Archive Ready: YES`)

## Acceptance outcomes

- Operators can maintain daily sales and customer traffic through both batch workbook import and single-record fallback paths.
- Daily-sales and traffic template downloads include the expected headers and reference guidance needed to prepare valid imports.
- Sales and traffic workbook imports apply deterministically and support idempotent reimport behavior for matching business keys.
- Invalid workbook imports surface row-level diagnostics and do not partially commit untrusted data.
- Operational Excel exports for the governed datasets remain available with stable business-facing headers and values.
- Sales-admin filters remain usable after imported or manually entered data is present.
- The combined Sales / Excel slice is covered by current commit-scoped verification evidence on the validated head.

## Traceability notes

- Sales UI surface: `frontend/src/views/SalesAdminView.vue`
- Sales domain tests: `frontend/e2e/task16-sales-admin.spec.ts`, `backend/internal/http/handlers/sales_test.go`
- Excel I/O service: `backend/internal/excelio/service.go`
- Excel I/O integration tests: `backend/internal/excelio/service_integration_test.go`
- Excel import handler tests: `backend/internal/http/handlers/excelio_test.go`
- Output inventory baseline: `docs/output-catalog.md`

## Conclusion

The Sales / Excel slice is accepted for the governed first-release scope on validated head `e0ba41653750c59b270ad99635a3ab79747b9a7c`. Sales/POS operational capture, workbook-driven ingestion, deterministic reimport behavior, row-level import diagnostics, and mandatory Excel I/O coverage all pass with current verification evidence.
