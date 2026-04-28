## Why

The Workbench is currently reachable from authenticated navigation, but it still behaves like a shell instead of a true operator queue. Now that first-release acceptance closure is complete, the highest-value next slice is to turn the Workbench into a real day-one operational surface backed by aggregated queue data that helps a solo team focus on pending approvals, receivables, overdue receivables, and active lease workload.

## What Changes

- Add a dedicated backend aggregate endpoint for the Workbench so the frontend can load queue-focused operational data in one request.
- Update the Workbench frontend to render real queue sections and counts instead of static placeholder content.
- Define queue behavior for pending approvals, receivables, overdue receivables, and active lease workload, including empty and partial-data states.
- Add focused automated verification for backend aggregation and Workbench rendering behavior.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `workbench-dashboard`: extend the Workbench requirement from route reachability only to a queue-oriented operational workspace backed by a single aggregate API.

## Impact

- Backend: new or expanded workbench/dashboard aggregation handler and service logic across workflow, receivable, invoice, and lease summary sources.
- Frontend: `WorkbenchView.vue` and related API/composable wiring to fetch and render queue data.
- Verification: backend tests for aggregation semantics and frontend tests for Workbench loading, empty, and populated queue states.
