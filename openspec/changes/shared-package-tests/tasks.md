## 1. sqlutil unit tests

- [ ] 1.1 Create `backend/internal/sqlutil/helpers_test.go`
- [ ] 1.2 Test all 5 Null*Pointer functions (Valid and !Valid cases)
- [ ] 1.3 Test all 7 *PointerValue functions (nil and non-nil cases)
- [ ] 1.4 Test InPlaceholders (0, 1, 3, negative)
- [ ] 1.5 Verify `go test ./internal/sqlutil/` passes

## 2. pagination unit tests

- [ ] 2.1 Create `backend/internal/pagination/pagination_test.go`
- [ ] 2.2 Test NormalizePage with zero/negative page → 1
- [ ] 2.3 Test NormalizePage with zero/negative pageSize → DefaultPageSize
- [ ] 2.4 Test NormalizePage with pageSize > MaxPageSize → MaxPageSize
- [ ] 2.5 Test NormalizePage with valid values pass through
- [ ] 2.6 Verify `go test ./internal/pagination/` passes

## 3. Verification

- [ ] 3.1 Run `go test ./...` from `backend/` — all tests pass
