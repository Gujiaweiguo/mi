## Why

12 of 16 backend HTTP handler files have zero unit tests. Only `auth`, `health`, `org`, and `workflow` handlers have tests. This means input validation, error rendering, and parameter parsing in the other handlers are completely unverified at the unit level. While integration tests exist, they are expensive to run and don't isolate handler-layer concerns.

## What Changes

Add unit tests for all 12 untested handler files, focusing on:
- **Input validation**: invalid JSON payloads, missing required fields, malformed request bodies
- **Parameter parsing**: invalid `:id` route params, bad query string values, invalid date formats
- **Error rendering**: verify error-to-HTTP-status mapping in `render*Error` methods

This does NOT add service-layer mocks or happy-path tests — those are already covered by integration tests. The goal is to verify the HTTP boundary layer.

## Scope

New test files (one per handler):
- `backend/internal/http/handlers/billing_test.go`
- `backend/internal/http/handlers/invoice_test.go`
- `backend/internal/http/handlers/lease_test.go`
- `backend/internal/http/handlers/reporting_test.go`
- `backend/internal/http/handlers/sales_test.go`
- `backend/internal/http/handlers/structure_test.go`
- `backend/internal/http/handlers/taxexport_test.go`
- `backend/internal/http/handlers/dashboard_test.go`
- `backend/internal/http/handlers/docoutput_test.go`
- `backend/internal/http/handlers/excelio_test.go`
- `backend/internal/http/handlers/masterdata_test.go`
- `backend/internal/http/handlers/baseinfo_test.go`
