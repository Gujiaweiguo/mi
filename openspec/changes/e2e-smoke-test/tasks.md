## 1. Create E2E Smoke Test

- [ ] 1.1 Create `backend/internal/http/e2e_smoke_test.go` with `//go:build integration` tag, implementing `TestE2ELeaseToInvoiceSmoke` that exercises: login → create lease → submit → approve (2 steps) → generate charges → create invoice → submit → approve → record payment → verify receivable settled
- [ ] 1.2 Verify `go build -tags=integration ./internal/http/` compiles from `backend/`

## 2. Verification

- [ ] 2.1 Run `go test -tags=integration -run TestE2E ./internal/http/` — test passes (requires Docker)
