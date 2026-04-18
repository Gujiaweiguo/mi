## 1. Pure Function Tests

- [ ] 1.1 Create `backend/internal/logging/logger_test.go` — test New() with valid/invalid levels
- [ ] 1.2 Create `backend/internal/sales/sales_test.go` — test normalizeLimit, normalizeOffset
- [ ] 1.3 Create `backend/internal/baseinfo/baseinfo_test.go` — test normalizeInput, trimStringPointer, isDuplicateEntry, constructors
- [ ] 1.4 Create `backend/internal/masterdata/masterdata_test.go` — test normalizeListFilter, normalizeStatus, buildSearchClause, constructors
- [ ] 1.5 Create `backend/internal/structure/structure_test.go` — test normalize*Input, mapRepositoryError, isDuplicateEntry, isForeignKeyViolation

## 2. Dashboard Tests

- [ ] 2.1 Create `backend/internal/dashboard/service_test.go` — test constructor, JSON tags

## 3. Verification

- [ ] 3.1 `go test ./internal/logging/ ./internal/sales/ ./internal/baseinfo/ ./internal/masterdata/ ./internal/structure/ ./internal/dashboard/ -count=1` passes
- [ ] 3.2 `go build ./...` passes
