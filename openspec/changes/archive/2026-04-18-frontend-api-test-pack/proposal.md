## Why

14 of 16 frontend API modules have zero test coverage. Only `http.ts` and `dashboard.ts` have tests. This means 100 exported API functions are completely unverified — incorrect URL construction, wrong HTTP methods, broken response unwrapping, and missing request configs would all go undetected until runtime.

## What Changes

Add unit tests for all 13 untested API modules (auth.ts doesn't exist), following the existing `vi.mock('./http')` pattern from `dashboard.test.ts`. Each test file verifies:
- Correct HTTP method (GET/POST/PUT)
- Correct URL with interpolated params
- Correct return value shape (response unwrapping)
- Correct request config (responseType, validateStatus where applicable)
- Error propagation

## Scope

New test files (one per untested module):
- `frontend/src/api/lease.test.ts`
- `frontend/src/api/invoice.test.ts`
- `frontend/src/api/billing.test.ts`
- `frontend/src/api/workflow.test.ts`
- `frontend/src/api/reports.test.ts`
- `frontend/src/api/tax.test.ts`
- `frontend/src/api/masterdata.test.ts`
- `frontend/src/api/print.test.ts`
- `frontend/src/api/excel.test.ts`
- `frontend/src/api/org.test.ts`
- `frontend/src/api/baseinfo.test.ts`
- `frontend/src/api/structure.test.ts`
- `frontend/src/api/sales.test.ts`
