## 1. Backend model types

- [ ] 1.1 Add `exportLeaseContractRow` struct to `model.go` with fields: `LeaseNo`, `TenantName`, `StoreCode`, `DepartmentCode`, `StartDate`, `EndDate`, `Status`, `EffectiveVersion`.
- [ ] 1.2 Add `exportUnitDataRow` struct to `model.go` with fields: `Code`, `BuildingCode`, `FloorCode`, `LocationCode`, `AreaCode`, `UnitTypeCode`, `FloorArea`, `UseArea`, `RentArea`, `IsRentable`, `Status`.

## 2. Backend repository queries

- [ ] 2.1 Add `ListLeaseContractExportRows` to `repository.go` that joins `lease_contracts` against `stores` and `departments`, selecting `lease_no`, `tenant_name`, `stores.code AS store_code`, `departments.code AS department_code`, `start_date`, `end_date`, `status`, `effective_version`, ordered by `lease_contracts.id`.
- [ ] 2.2 Add `ListUnitDataExportRows` to `repository.go` that joins `units` against `buildings`, `floors`, `locations`, `areas`, and `unit_types`, selecting `units.code`, `buildings.code AS building_code`, `floors.code AS floor_code`, `locations.code AS location_code`, `areas.code AS area_code`, `unit_types.code AS unit_type_code`, `floor_area`, `use_area`, `rent_area`, `is_rentable`, `units.status`, ordered by `units.id`.

## 3. Backend service methods and switch wiring

- [ ] 3.1 Add `exportLeaseContracts` method to `service.go` that calls `ListLeaseContractExportRows`, creates an excelize workbook, writes the header row, iterates data rows with date formatting, and returns `ExportArtifact` with filename `operational-lease-contracts.xlsx`.
- [ ] 3.2 Add `exportUnitData` method to `service.go` that calls `ListUnitDataExportRows`, creates an excelize workbook, writes the header row, iterates data rows (converting `is_rentable` bool to string), and returns `ExportArtifact` with filename `operational-unit-data.xlsx`.
- [ ] 3.3 Add `"lease_contracts"` and `"unit_data"` cases to the `ExportOperationalDataset` switch statement in `service.go`, dispatching to the new methods.

## 4. Frontend dataset dropdown and i18n

- [ ] 4.1 Add `{ label: t('excel.datasets.invoices'), value: 'invoices' }` to the `datasetOptions` computed property in `ExcelIOView.vue`.
- [ ] 4.2 Add `excel.datasets.invoices` i18n key to all frontend locale files (en, zh-CN, and any others present).

## 5. Verification

- [ ] 5.1 Run `go build ./...` in the backend and confirm the package compiles without errors.
- [ ] 5.2 Run `go test ./internal/excelio/...` and confirm all existing tests pass.
- [ ] 5.3 Run frontend build (`npm run build` or equivalent) and confirm no compilation errors.

## 6. Final Verification Wave

- [ ] 6.1 Confirm the backend handles all four dataset keys (`invoices`, `billing_charges`, `lease_contracts`, `unit_data`) in the `ExportOperationalDataset` switch without returning `ErrInvalidDataset`.
- [ ] 6.2 Confirm the frontend dropdown lists all four dataset options with valid i18n labels.
- [ ] 6.3 Confirm no existing export or import behavior was broken by the change.
