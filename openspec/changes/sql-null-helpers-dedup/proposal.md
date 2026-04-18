## Why

Eight backend files (lease, invoice, billing, masterdata, structure, baseinfo, sales, reporting) collectively define 25 file-private SQL null-pointer helper functions with identical implementations. The same `nullInt64Pointer`, `nullStringPointer`, `int64PointerValue`, and `stringPointerValue` functions are copy-pasted across 3-6 files each. Adding a new nullable column scan requires adding the same helper to every file that needs it.

## What Changes

- Create `backend/internal/sqlutil/helpers.go` with 11 exported helper functions covering all duplicated patterns.
- Replace all 25 private duplicates with imports from the shared package.
- Resolve the `timePointerValue` discrepancy: sales returns a formatted date string while others return `time.Time` — keep both as `TimePointerValue` and `TimePointerDateString`.
- No API or behavior changes — pure internal refactoring.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: consolidate SQL null-pointer helpers into a shared package, reducing maintenance surface.

## Impact

- `backend/internal/sqlutil/helpers.go`: new file (~80 lines)
- 8 files modified: delete local helpers, add import
- Zero API contract changes.
