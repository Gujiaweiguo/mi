## 1. Backend Dashboard Package

- [ ] 1.1 Create `backend/internal/dashboard/service.go` with `DashboardSummary` struct and `GetSummary` method
- [ ] 1.2 Implement 6 parallel COUNT queries using `errgroup`
- [ ] 1.3 Verify `go build ./internal/dashboard/` passes

## 2. Backend Handler and Route

- [ ] 2.1 Create `backend/internal/http/handlers/dashboard.go` with `DashboardHandler.Summary` method
- [ ] 2.2 Register `GET /api/dashboard/summary` in `backend/internal/http/router.go`
- [ ] 2.3 Verify `go build ./...` passes

## 3. Frontend Update

- [ ] 3.1 Simplify `frontend/src/api/dashboard.ts` to call `GET /api/dashboard/summary`
- [ ] 3.2 Update `frontend/src/views/DashboardView.vue` to use simplified API
- [ ] 3.3 Verify `npm run build` passes

## 4. Verification

- [ ] 4.1 `go test ./...` from `backend/` passes
- [ ] 4.2 `npm run build` from `frontend/` passes
