## Why

The frontend `ExcelIOView.vue` offers three export datasets in its dropdown: `billing_charges`, `lease_contracts`, and `unit_data`. The backend `ExportOperationalDataset` switch statement only handles `invoices` and `billing_charges`. When a user selects `lease_contracts` or `unit_data`, the backend returns HTTP 400 with `ErrInvalidDataset`. At the same time, the backend already supports an `invoices` export that the frontend dropdown does not expose, so that working export path is invisible to operators.

This mismatch means the Excel export feature is half-broken and half-hidden.

## What Changes

- Add `exportLeaseContracts` to the backend `excelio` service, with a repository query that joins `lease_contracts` against `stores` and `departments` to resolve foreign-key IDs into human-readable codes.
- Add `exportUnitData` to the backend `excelio` service, with a repository query that joins `units` against `buildings`, `floors`, `locations`, `areas`, and `unit_types` to resolve all foreign-key columns.
- Wire both new datasets into the `ExportOperationalDataset` switch statement so they respond to the dataset keys the frontend already sends.
- Add `invoices` as a fourth option in the frontend `datasetOptions` computed property, with a matching i18n label `excel.datasets.invoices`.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `tax-document-and-excel-output`: expand the Excel export contract to include `lease_contracts` and `unit_data` as operational export datasets, and surface the existing `invoices` export in the frontend.

## Impact

- `backend/internal/excelio/service.go`: add `exportLeaseContracts` and `exportUnitData` methods, extend the switch statement.
- `backend/internal/excelio/model.go`: add `exportLeaseContractRow` and `exportUnitDataRow` types.
- `backend/internal/excelio/repository.go`: add `ListLeaseContractExportRows` and `ListUnitDataExportRows` query methods.
- `frontend/src/views/ExcelIOView.vue`: add `invoices` to the `datasetOptions` array.
- Frontend i18n locale files: add the `excel.datasets.invoices` translation key.
