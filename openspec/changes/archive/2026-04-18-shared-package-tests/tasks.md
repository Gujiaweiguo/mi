## 1. sqlutil unit tests

- [x] 1.1 Create `backend/internal/sqlutil/helpers_test.go`
- [x] 1.2 Test all 5 Null*Pointer functions (Valid and !Valid cases)
- [x] 1.3 Test all 7 *PointerValue functions (nil and non-nil cases)
- [x] 1.4 Test InPlaceholders (0, 1, 3, negative)
- [x] 1.5 Verify `go test ./internal/sqlutil/` passes

## 2. pagination unit tests

- [x] 2.1 Create `backend/internal/pagination/pagination_test.go`
- [x] 2.2 Test NormalizePage with zero/negative page → 1
- [x] 2.3 Test NormalizePage with zero/negative pageSize → DefaultPageSize
- [x] 2.4 Test NormalizePage with pageSize > MaxPageSize → MaxPageSize
- [x] 2.5 Test NormalizePage with valid values pass through
- [x] 2.6 Verify `go test ./internal/pagination/` passes

## 3. Verification

- [x] 3.1 Run `go test ./...` from `backend/` — all tests pass
