## Design

### Test Approach

These packages use concrete `*sql.DB` types in their repositories, making full mocking impractical. The tests focus on **testable pure functions and service-layer logic** that don't require database connections.

### Per-Package Test Plan

**logging/logger_test.go** (3-4 tests):
- `New("debug")` returns dev logger, no error
- `New("info")` returns production logger, no error
- `New("error")` returns production logger, no error
- `New("invalid")` returns error

**sales/sales_test.go** (4-5 tests):
- `normalizeLimit(0)` → default
- `normalizeLimit(-5)` → default
- `normalizeLimit(100)` → 100
- `normalizeOffset(0, 20)` → 0
- `normalizeOffset(3, 20)` → 60

**baseinfo/baseinfo_test.go** (8-10 tests):
- `normalizeInput` trims whitespace from Code/Name
- `normalizeInput` handles nil pointers
- `trimStringPointer` with nil, empty, whitespace input
- `isDuplicateEntry` with duplicate-code error vs other error
- `NewService(nil)` returns non-nil service
- `NewRepository(nil)` returns non-nil repository

**masterdata/masterdata_test.go** (8-10 tests):
- `normalizeListFilter` applies defaults
- `normalizeStatus` handles empty, "active", "inactive"
- `buildSearchClause` with empty/non-empty query
- `isDuplicateEntry` detection
- `NewService(nil)` / `NewRepository(nil)`

**structure/structure_test.go** (12-15 tests):
- `normalizeStoreInput` trims fields
- `normalizeBuildingInput` trims fields
- `normalizeFloorInput` trims fields
- `normalizeAreaInput` trims fields
- `normalizeLocationInput` trims fields
- `normalizeUnitInput` trims fields
- `normalizeStatus` handles empty, "active", "inactive"
- `mapRepositoryError` maps duplicate entry → ErrDuplicateCode
- `mapRepositoryError` maps FK violation → ErrParentReferenceNotFound
- `isDuplicateEntry` / `isForeignKeyViolation` detection

**dashboard/service_test.go** (2-3 tests):
- `NewDashboardService(nil)` returns non-nil
- `DashboardSummary` JSON tags produce expected field names

### Verification
- `go test ./internal/logging/ ./internal/sales/ ./internal/baseinfo/ ./internal/masterdata/ ./internal/structure/ ./internal/dashboard/ -count=1` passes
- `go build ./...` passes
