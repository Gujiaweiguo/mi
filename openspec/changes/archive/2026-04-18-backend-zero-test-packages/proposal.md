## Why

6 backend packages have zero unit tests: `baseinfo`, `dashboard`, `logging`, `masterdata`, `sales`, `structure`. While their repository layers are covered by integration tests, the service-layer validation logic and pure helper functions are completely untested at the unit level.

## What Changes

Add unit tests for the 6 zero-test backend packages, focusing on:
- **Service-layer validation**: input normalization, duplicate detection, error mapping
- **Pure functions**: `normalizeLimit`, `normalizeOffset`, `trimStringPointer`, `isDuplicateEntry`, `buildSearchClause`, etc.
- **Logger initialization**: valid/invalid log levels

Repository methods that execute SQL are NOT tested here (covered by integration tests).

## Scope

New test files:
- `backend/internal/logging/logger_test.go`
- `backend/internal/sales/sales_test.go`
- `backend/internal/baseinfo/baseinfo_test.go`
- `backend/internal/masterdata/masterdata_test.go`
- `backend/internal/structure/structure_test.go`
- `backend/internal/dashboard/service_test.go`
