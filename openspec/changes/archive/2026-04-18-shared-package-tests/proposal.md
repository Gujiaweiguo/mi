## Problem

The two most critical shared backend packages — `sqlutil` (13 helper functions) and `pagination` (generic types + constants) — have **zero unit test coverage**. These packages are imported by 17+ backend modules. Any regression in these pure functions would silently corrupt every dependent module.

## Proposal

Add comprehensive unit tests for both packages. Since they are pure functions with no database or external dependencies, tests are straightforward and fast.

## Scope

- `backend/internal/sqlutil/helpers_test.go` — test all 13 exported functions
- `backend/internal/pagination/pagination_test.go` — test NormalizePage, ListResult, constants
