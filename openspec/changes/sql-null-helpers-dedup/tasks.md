## 1. Create Shared sqlutil Package

- [x] 1.1 Create `backend/internal/sqlutil/helpers.go` with 11 exported functions: NullInt64Pointer, NullStringPointer, NullTimePointer, NullFloat64Pointer, NullIntPointer, Int64PointerValue, StringPointerValue, TimePointerValue, TimePointerDateString, BoolPointerValue, Float64PointerValue
- [x] 1.2 Verify `go build ./internal/sqlutil/` passes

## 2. Migrate 8 Modules

- [x] 2.1 Migrate `lease/repository.go` — delete nullInt64Pointer, nullTimePointer, int64PointerValue, timePointerValue; use sqlutil.*
- [x] 2.2 Migrate `invoice/repository.go` — delete nullInt64Pointer, nullTimePointer, nullStringPointer, int64PointerValue, stringPointerValue, timePointerValue; use sqlutil.*
- [x] 2.3 Migrate `billing/repository.go` — delete nullTimePointer; use sqlutil.NullTimePointer
- [x] 2.4 Migrate `masterdata/masterdata.go` — delete nullInt64Pointer, int64PointerValue, nullFloat64Pointer, nullStringPointer, nullIntPointer, float64PointerValue, stringPointerValue; use sqlutil.*
- [x] 2.5 Migrate `structure/structure.go` — delete int64PointerValue, stringPointerValue, nullStringPointer; use sqlutil.*
- [x] 2.6 Migrate `baseinfo/baseinfo.go` — delete stringPointerValue, boolPointerValue, int64PointerValue, intPointerValue; use sqlutil.*
- [x] 2.7 Migrate `sales/sales.go` — delete int64PointerValue, timePointerValue; use sqlutil.* (note: sales timePointerValue returns formatted string, use sqlutil.TimePointerDateString)
- [x] 2.8 Migrate `reporting/repository.go` — delete nullFloat64Pointer, nullIntPointer, nullStringPointer, stringPointerValue; use sqlutil.*

## 3. Verification

- [x] 3.1 Run `go build ./...` from `backend/` — compiles cleanly
- [x] 3.2 Run `go test ./...` from `backend/` — all tests pass
