## 1. Create Shared Pagination Package

- [ ] 1.1 Create `backend/internal/pagination/pagination.go` with `ListResult[T any]` generic struct (Items/Total/Page/PageSize with JSON tags), `DefaultPage=1`/`DefaultPageSize=20`/`MaxPageSize=100` constants, and `NormalizePage(page, pageSize int) (int, int)` function
- [ ] 1.2 Verify `go build ./internal/pagination/` passes

## 2. Migrate 5 Standard Modules

- [ ] 2.1 Migrate `lease` — remove local constants, ListResult struct, normalizePage(); import pagination; update repository and handler to use `pagination.ListResult[Summary]` and `pagination.NormalizePage()`
- [ ] 2.2 Migrate `invoice` — remove local constants, ListResult, ReceivableListResult, normalizePage(); use `pagination.ListResult[Document]` and `pagination.ListResult[ReceivableListItem]`
- [ ] 2.3 Migrate `billing` — remove local constants, ChargeListResult, normalizePage(); use `pagination.ListResult[ChargeLine]`
- [ ] 2.4 Migrate `docoutput` — remove local constants, ListResult, normalizePage(); use `pagination.ListResult[Template]`
- [ ] 2.5 Migrate `taxexport` — remove local constants, ListResult, normalizePage(); use `pagination.ListResult[RuleSet]`

## 3. Fix Masterdata Inconsistencies

- [ ] 3.1 Fix masterdata: change Total from `int` to `int64`, change default page size from 10 to 20, add JSON tags to result structs, replace `normalizeListFilter()` inline logic with `pagination.NormalizePage()`

## 4. Verification

- [ ] 4.1 Run `go build ./...` from `backend/` — compiles cleanly
- [ ] 4.2 Run `go test ./...` from `backend/` — all existing tests pass
