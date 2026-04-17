## Why

Six backend modules (lease, invoice, billing, docoutput, taxexport, masterdata) each define their own identical `ListFilter` struct with `Page`/`PageSize` fields, their own `ListResult` struct with `Items`/`Total`/`Page`/`PageSize` fields, their own `DefaultPage`/`DefaultPageSize`/`MaxPageSize` constants, and their own identical `normalizePage()` function. This is approximately 300 lines of copy-pasted boilerplate. Additionally, masterdata has inconsistencies: `Total int` instead of `int64`, default page size of 10 instead of 20, no JSON tags on result structs, and no exported constants.

## What Changes

- Create `backend/internal/pagination/` package with shared types: `ListResult[T any]` generic struct, `NormalizePage()` function, and `DefaultPage`/`DefaultPageSize`/`MaxPageSize` constants.
- Update all 6 modules to use the shared package instead of local definitions.
- Fix masterdata inconsistencies: `Total int64`, default page size 20, add JSON tags.
- No API contract changes — the JSON output shape remains identical (all fields use the same keys).

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: consolidate duplicated pagination types into a shared package, reducing maintenance surface and fixing inconsistencies.

## Impact

- `backend/internal/pagination/pagination.go`: new file (~40 lines)
- `backend/internal/lease/model.go`: remove local constants and ListResult struct, import pagination
- `backend/internal/lease/repository.go`: remove local normalizePage(), import pagination
- Same pattern for invoice, billing, docoutput, taxexport, masterdata (10 files modified total)
- Zero API contract changes.
