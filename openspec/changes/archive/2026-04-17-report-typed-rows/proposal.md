## Why

The reporting module defines 18 typed row structs (R01Row–R18Row) and an R19Unit struct, but the service layer immediately erases type information by converting every struct into `map[string]any` via a 220-line switch-case in `runReport()`. Each struct-to-map conversion is hand-written as a single-line map literal. Adding a column to any report requires changes in three places: the struct definition, the column labels, and the map literal — and the third is easy to get wrong because the keys are opaque strings with no compile-time link to the struct fields. Moving the conversion into methods on each row struct collocates the mapping with the type definition and eliminates the switch-case boilerplate.

## What Changes

- Add a `ToMap() map[string]any` method on every report row struct (R01Row through R18Row, R19Unit).
- Add a `ToMap() map[string]any` method on `AgingBuckets` for the shared aging columns.
- Replace the 220-line switch-case in `service.go:runReport()` with a generic dispatch that calls `ToMap()` on each item.
- Add `ReportR19` constant (currently missing — R19 is matched via raw string `ReportID("r19")`).
- Move `agingBucketMap`, `agingCustomerRow`, `agingDepartmentRow` helpers to model.go as methods on the relevant structs.
- No API contract changes — the JSON output shape remains identical.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: report row structs carry their own serialization logic, reducing maintenance surface when adding or modifying report columns.

## Impact

- `backend/internal/reporting/model.go`: add `ToMap()` methods on all row structs + `ReportR19` constant + move aging helpers.
- `backend/internal/reporting/service.go`: replace 220-line switch-case with generic dispatch, simplify `runReport()`.
- Zero API contract changes. Zero new dependencies.
