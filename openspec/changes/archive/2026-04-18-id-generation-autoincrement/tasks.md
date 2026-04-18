## 1. Database Migration

- [x] 1.1 Create `backend/internal/platform/database/migrations/000020_auto_increment_ids.up.sql` — ALTER 13 tables to add AUTO_INCREMENT on `id`
- [x] 1.2 Create `backend/internal/platform/database/migrations/000020_auto_increment_ids.down.sql` — reverse migration
- [x] 1.3 Verify migration applies cleanly with `go test ./internal/platform/database/...`

## 2. Go Code: baseinfo

- [x] 2.1 Delete `nextID()` method from `baseinfo/baseinfo.go`
- [x] 2.2 Modify `buildInsertQuery()` to not include `id` column and placeholder
- [x] 2.3 Modify `Create()` to use `result.LastInsertId()` instead of `nextID()`
- [x] 2.4 Verify `go build ./internal/baseinfo/` passes

## 3. Go Code: structure

- [x] 3.1 Delete `nextID()` method from `structure/structure.go`
- [x] 3.2 Modify `CreateStore()` — INSERT without `id`, use `LastInsertId()`
- [x] 3.3 Modify `CreateBuilding()` — same pattern
- [x] 3.4 Modify `CreateFloor()` — same pattern
- [x] 3.5 Modify `CreateArea()` — same pattern
- [x] 3.6 Modify `CreateLocation()` — same pattern
- [x] 3.7 Modify `CreateUnit()` — same pattern
- [x] 3.8 Verify `go build ./internal/structure/` passes

## 4. Go Code: excelio

- [x] 4.1 Delete `nextUnitID()` function from `excelio/repository.go`
- [x] 4.2 Modify `UpsertUnits()` — INSERT new units without `id`, use `LastInsertId()` per row
- [x] 4.3 Verify `go build ./internal/excelio/` passes

## 5. Verification

- [x] 5.1 Run `go build ./...` from `backend/` — compiles cleanly
- [x] 5.2 Run `go test ./...` from `backend/` — all tests pass
