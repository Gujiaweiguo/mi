## Context

The backend has 12+ integration tests covering individual modules (lease, billing, invoice, workflow, reporting, etc.) but none that exercise the full HTTP chain. The existing `router_integration_test.go` and `invoice_receivable_integration_test.go` demonstrate the HTTP-level testing pattern using `httptest.NewRecorder` and a `loginAsAdmin` helper.

## Goals / Non-Goals

**Goals:**
- A single test that proves the Lease → Invoice → Payment chain works through the HTTP API.
- The test is runnable via `go test -tags=integration -run TestE2E ./internal/http/`.
- Each step asserts the expected HTTP status and key response fields.

**Non-Goals:**
- Frontend E2E (Playwright) tests.
- Edge case coverage (error paths, validation failures) — that's covered by module-level tests.
- Performance testing.

## Decisions

### Decision 1 — HTTP-level test over service-level

**Choice:** Test through the full Gin router + middleware stack using `httptest`.

**Rationale:** This tests the real auth, middleware, and handler wiring. Service-level tests already exist for individual modules. The E2E value is in testing the integration between modules.

### Decision 2 — Single test function

**Choice:** One `TestE2ELeaseToInvoiceSmoke` function with sequential steps.

**Rationale:** The chain is inherently sequential. Breaking it into sub-tests adds complexity without value since each step depends on the previous step's output.

### Decision 3 — Reuse seed data IDs

**Choice:** Hardcode seed data IDs (store 101, unit 101, customer 101, etc.).

**Rationale:** The bootstrap seeds are deterministic. Hardcoding IDs makes the test readable and avoids unnecessary API calls to look up entities.
