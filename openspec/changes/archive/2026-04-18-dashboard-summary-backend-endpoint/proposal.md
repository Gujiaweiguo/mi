## Why

The frontend dashboard currently makes 6 parallel API requests on load to aggregate operational counts. This creates unnecessary network overhead, makes the dashboard slower on high-latency connections, and complicates future expansion of dashboard metrics.

## What Changes

- Add a single `GET /api/dashboard/summary` backend endpoint that aggregates all 6 metrics in one server-side call.
- Update the frontend `DashboardView` to call the new endpoint instead of 6 separate APIs.
- Preserve the existing error-tolerant behavior: partial failures return null for affected metrics rather than failing the entire request.

## Scope

- New backend handler: `backend/internal/http/handlers/dashboard.go`
- New backend service: `backend/internal/dashboard/service.go`
- Route registration in `backend/internal/http/router.go`
- Frontend update: `frontend/src/api/dashboard.ts` and `frontend/src/views/DashboardView.vue`
