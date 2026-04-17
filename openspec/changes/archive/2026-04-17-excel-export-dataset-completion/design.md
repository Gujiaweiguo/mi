## Context

The `excelio` package already implements two working export methods: `exportInvoices` and `exportBillingCharges`. Each follows the same three-step pattern: a repository method runs a SQL query and returns a typed slice, the service method creates an `excelize.File` and writes header plus data rows, then returns an `ExportArtifact` with the buffered workbook bytes. The `ExportOperationalDataset` method dispatches to the correct export based on the `dataset` string in the request.

The frontend `ExcelIOView.vue` drives exports through a dropdown of dataset options and a single export button. The dropdown currently lists `billing_charges`, `lease_contracts`, and `unit_data`. The backend only handles `invoices` and `billing_charges`, leaving two dataset selections broken and one backend dataset invisible.

This change is narrow: add two missing backend export methods and one missing frontend dropdown entry. No new API endpoints, no new import paths, no filtering or pagination.

## Goals / Non-Goals

**Goals:**
- Make `lease_contracts` and `unit_data` exports work end-to-end when selected from the frontend dropdown.
- Make the existing `invoices` export visible in the frontend dropdown.
- Follow the existing export pattern exactly: repository query returns a typed slice, service writes headers and rows into an excelize workbook.
- Resolve foreign-key IDs to codes in the SQL query so the exported workbook contains human-readable values, not internal IDs.

**Non-Goals:**
- No new import functionality. This change is export-only.
- No filtering, date-range selection, or pagination on exports. Each export returns all approved/applicable records.
- No changes to the API endpoint structure or routing.
- No new capabilities or modules.

## Decisions

### Decision: Lease contracts export columns and JOINs

The lease contracts export will include these columns: `lease_no`, `tenant_name`, `store_code`, `department_code`, `start_date`, `end_date`, `status`, `effective_version`.

The repository query will join `lease_contracts` against `stores` (to resolve `store_id` to `store_code`) and against `departments` (to resolve `department_id` to `department_code`). Date columns will be formatted using the existing `DateLayout` constant (`2006-01-02`).

**Alternative considered:** export raw `store_id` and `department_id` values and let operators resolve them separately. Rejected because every other working export already resolves IDs to codes in the query, and exporting raw IDs would be inconsistent and less useful.

### Decision: Unit data export columns and JOINs

The unit data export will include these columns: `code`, `building_code`, `floor_code`, `location_code`, `area_code`, `unit_type_code`, `floor_area`, `use_area`, `rent_area`, `is_rentable`, `status`.

The repository query will join `units` against `buildings`, `floors`, `locations`, `areas`, and `unit_types` to resolve each `_id` foreign key to the corresponding `code`. The `is_rentable` boolean will be written as `"true"` or `"false"` string values in the workbook cell.

**Alternative considered:** reuse the existing `UnitImportRow` type for the export model. Rejected because `UnitImportRow` is an import-oriented type that uses string codes, while the export row type needs to carry values straight from the SQL scan. Keeping them separate avoids coupling import and export models.

### Decision: Frontend adds `invoices` option with i18n label

The `datasetOptions` computed property in `ExcelIOView.vue` will include a fourth entry: `{ label: t('excel.datasets.invoices'), value: 'invoices' }`. The corresponding i18n key will be added to all locale files alongside the existing `excel.datasets.billingCharges`, `excel.datasets.leaseContracts`, and `excel.datasets.unitData` keys.

**Alternative considered:** rename the backend `invoices` dataset key to match a different frontend convention. Rejected because the backend key `invoices` already works and renaming it would break any existing API consumers.

### Decision: No filtering on exports

All new export methods will query all applicable records without date filters or status filters, matching the behavior of the existing `exportInvoices` and `exportBillingCharges` methods. The `exportInvoices` method already filters to `status = 'approved'`; the lease contracts and unit data exports will return all records since those tables represent master/operational data rather than approval-gated documents.

**Alternative considered:** add date-range parameters to export requests. Rejected because the task scope is completing the missing dataset coverage, not adding new query capabilities. Filtering can be addressed in a separate change if needed.

## Risks / Trade-offs

- **Large result sets** → The export-all approach could produce large workbooks if lease contracts or units grow into the tens of thousands. For first release, this is acceptable since the same pattern is already used for billing charges. Future work can add streaming or chunked exports if needed.
- **JOIN performance on units table** → The unit data export joins five reference tables. For first-release data volumes this is negligible. Index verification on foreign-key columns should be part of the verification step.
- **i18n key rollout** → Adding `excel.datasets.invoices` requires updating all locale files simultaneously. Missing a locale file will cause a fallback key to display rather than a crash, which is an acceptable degradation.

## Migration Plan

1. Add `exportLeaseContractRow` and `exportUnitDataRow` types to `model.go`.
2. Add `ListLeaseContractExportRows` and `ListUnitDataExportRows` repository methods with JOIN queries to `repository.go`.
3. Add `exportLeaseContracts` and `exportUnitData` service methods to `service.go`.
4. Wire the two new dataset keys into the `ExportOperationalDataset` switch statement.
5. Add the `invoices` option to `datasetOptions` in `ExcelIOView.vue`.
6. Add the `excel.datasets.invoices` i18n key to all locale files.
7. Run backend build and tests to confirm compilation and existing test pass.
8. Verify the export flow end-to-end for all four datasets.

Rollback is trivial: the change is purely additive. Reverting the commit removes the new exports and dropdown entry without affecting existing functionality.
