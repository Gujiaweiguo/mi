## Why

The project has integration tests for individual modules (lease, billing, invoice, workflow) but no end-to-end test that exercises the complete Lease → Approval → Charge Generation → Invoice → Approval → Payment chain through the HTTP API. This chain is the core business workflow. A single E2E smoke test serves as a release gate: if it passes, the entire operational chain works end-to-end.

## What Changes

- Create a single Go integration test file (`backend/internal/http/e2e_smoke_test.go`) with `//go:build integration` tag.
- The test exercises the full business chain via HTTP API: login → create lease → submit → approve (2 steps) → generate charges → create invoice → submit → approve → record payment → verify receivable settled.
- Uses existing test infrastructure (`NewTestDB`, `httptest`, `loginAsAdmin`).
- No production code changes — test file only.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: add E2E smoke test covering the full Lease→Invoice→Payment business chain.

## Impact

- `backend/internal/http/e2e_smoke_test.go`: new file (~200 lines)
- Zero production code changes.
