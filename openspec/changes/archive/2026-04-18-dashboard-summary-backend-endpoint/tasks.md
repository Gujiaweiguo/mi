## 1. Backend Dashboard Package

- [x] 1.1 Create `backend/internal/dashboard/service.go` with `DashboardSummary` struct and `GetSummary` method
- [x] 1.2 Implement 6 parallel COUNT queries using `errgroup`
- [x] 1.3 Verify `go build ./internal/dashboard/` passes

## 2. Backend Handler and Route

- [x] 2.1 Create `backend/internal/http/handlers/dashboard.go` with `DashboardHandler.Summary` method
- [x] 2.2 Register `GET /api/dashboard/summary` in `backend/internal/http/router.go`
- [x] 2.3 Verify `go build ./...` passes

## 3. Frontend Update

- [x] 3.1 Simplify `frontend/src/api/dashboard.ts` to call `GET /api/dashboard/summary`
- [x] 3.2 Update `frontend/src/views/DashboardView.vue` to use simplified API
- [x] 3.3 Verify `npm run build` passes

## 4. Verification

- [x] 4.1 `go test ./...` from `backend/` passes
- [x] 4.2 `npm run build` from `frontend/` passes
