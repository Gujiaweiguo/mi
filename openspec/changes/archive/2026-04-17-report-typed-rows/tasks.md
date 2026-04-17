## 1. ToMap Methods on Row Structs

- [x] 1.1 Add `ToMap() map[string]any` methods on all row structs in `model.go`: R01Row, R02Row, R03Row, R04Row, R05Row, R06Row, R07Row, R08Row, R09Row, R10Row, R11Row, R12Row, R13Row, R14Row, R15Row, R16Row, R17Row, R18Row, R19Unit, and AgingBuckets
- [x] 1.2 Add `ReportR19` constant in `model.go` alongside the other ReportID constants

## 2. Simplify Service Dispatch

- [x] 2.1 Replace the 220-line switch-case in `service.go:runReport()` with simplified cases that call `item.ToMap()` instead of inline map literals
- [x] 2.2 Move `agingBucketMap`, `agingCustomerRow`, `agingDepartmentRow` helper logic into `ToMap()` methods on AgingBuckets, R08Row, R09Row, R16Row, R17Row
- [x] 2.3 Update `flattenR19Visual` to use `R19Unit.ToMap()`

## 3. Verification

- [x] 3.1 Run `go build ./...` from `backend/` — compiles cleanly
- [x] 3.2 Run existing integration tests — all 19 reports pass with identical output
