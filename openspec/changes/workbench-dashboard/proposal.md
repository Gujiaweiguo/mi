## Why

Authenticated users currently land on the health page, which is useful for diagnostics but not for daily operations. A real workbench dashboard gives users an immediate operational overview with counts, pending work, and direct links into the priority business flows.

## What Changes

- Add a new authenticated `/dashboard` route and make it the default landing page after login.
- Create a dashboard view that shows key operational cards using existing list endpoints and their `total` counts.
- Surface pending workflow work, approval-related counts, receivables status, and quick links into the most common flows.
- Reuse existing frontend platform components and keep the first version frontend-only by aggregating existing API responses.

## Capabilities

### New Capabilities
- `workbench-dashboard`: operational dashboard for authenticated users with summary cards, work queues, and quick actions

### Modified Capabilities
- `organization-access-control`: authenticated home navigation changes from the health page fallback to the dashboard route

## Impact

- Affected frontend routing and navigation in `frontend/src/router/`, `frontend/src/auth/permissions.ts`, and `frontend/src/App.vue`.
- New dashboard UI in `frontend/src/views/` and supporting frontend API/composable code as needed.
- No backend API changes required for v1; the dashboard will aggregate existing lease, invoice, receivable, and workflow endpoints.
