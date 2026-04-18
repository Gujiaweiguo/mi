## Design

### Backend Endpoint

`GET /api/dashboard/summary` (authenticated, requires any valid token)

Response shape:
```json
{
  "summary": {
    "active_leases": 12,
    "pending_lease_approvals": 3,
    "pending_invoice_approvals": 4,
    "open_receivables": 9,
    "overdue_receivables": 2,
    "pending_workflows": 5
  }
}
```

### Implementation

1. **New package**: `backend/internal/dashboard/` with `service.go`
   - `DashboardService` receives `*sql.DB`
   - `GetSummary(ctx) (*DashboardSummary, error)` runs 6 COUNT queries in parallel using `errgroup`
   - Each query is a simple `SELECT COUNT(*) FROM table WHERE condition`
   - Uses `errgroup.WithContext` for cancellation propagation
   - Returns a typed `DashboardSummary` struct

2. **New handler**: `backend/internal/http/handlers/dashboard.go`
   - `DashboardHandler` with a `Summary` method
   - Calls `DashboardService.GetSummary`
   - Returns `gin.H{"summary": result}`

3. **Route**: Register `GET /api/dashboard/summary` in router.go
   - Auth required (RequireAuth middleware)
   - No specific permission required — any authenticated user can see the dashboard

4. **Frontend update**:
   - `frontend/src/api/dashboard.ts`: Replace the 6-call `getDashboardSummary` with a single `GET /api/dashboard/summary`
   - `frontend/src/views/DashboardView.vue`: Simplify — single loading state, single error state

### Query Details

| Metric | Query |
|--------|-------|
| active_leases | `SELECT COUNT(*) FROM lease_contracts WHERE status = 'active'` |
| pending_lease_approvals | `SELECT COUNT(*) FROM lease_contracts WHERE status = 'pending_approval'` |
| pending_invoice_approvals | `SELECT COUNT(*) FROM billing_documents WHERE document_type = 'invoice' AND status = 'pending_approval'` |
| open_receivables | `SELECT COUNT(*) FROM ar_open_items WHERE settlement_status = 'outstanding'` |
| overdue_receivables | `SELECT COUNT(*) FROM ar_open_items WHERE settlement_status = 'outstanding' AND due_date < CURDATE()` |
| pending_workflows | `SELECT COUNT(*) FROM workflow_instances WHERE status = 'pending'` |

### Verification

- `go build ./...` passes
- `go test ./...` passes
- `npm run build` passes
