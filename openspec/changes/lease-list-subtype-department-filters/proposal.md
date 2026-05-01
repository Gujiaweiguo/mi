## Why

Operators can already review lease contracts in `LeaseListView`, but they cannot narrow the queue by lease subtype or department even though both attributes are first-class lease fields. This makes contract lookup noisy for mixed portfolios and forces manual scanning on the main lease operations screen.

## What Changes

- Extend the lease list flow to accept optional `subtype` and `department_id` filters in the existing `/leases` list contract
- Add subtype and department filter controls to `LeaseListView.vue`
- Preserve current lease list behavior when the new filters are omitted
- Add frontend and backend tests covering filter parsing and filter application

## Capabilities

### New Capabilities

_(none — this change extends an existing first-release lease capability)_

### Modified Capabilities

- `lease-contract-management`: lease list review surfaces SHALL support filtering contracts by subtype and department so operators can target the relevant contract queue faster

## Impact

- **Frontend**: `frontend/src/views/LeaseListView.vue`, `frontend/src/views/LeaseListView.test.ts`, `frontend/src/api/lease.ts`, `frontend/src/api/lease.test.ts`, i18n message files
- **Backend**: `backend/internal/http/handlers/lease.go`, `backend/internal/http/handlers/lease_test.go`, `backend/internal/lease/model.go`, `backend/internal/lease/repository.go`, `backend/internal/lease/service_integration_test.go`
- **API**: `GET /leases` gains optional `subtype` and `department_id` query parameters
- **Dependencies**: no new dependencies; reuses existing department loading and lease subtype labels
