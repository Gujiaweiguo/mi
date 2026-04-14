## Why

The current master-data administration slice is now functionally complete, but its frontend implementation has accumulated enough responsibility in a single view that routine maintenance is becoming harder than it needs to be. `MasterDataAdminView.vue` now carries customer lifecycle management, brand lifecycle management, budget administration, prospect administration, feedback rendering, and table/query behavior in one place, which increases the cost of future changes and makes focused testing or review less natural.

We need a maintainability-focused refactor now so the master-data UI remains easy to extend after the supporting-domain closure work, without waiting for the next feature change to push the file into a harder-to-reason-about state.

## What Changes

- Refactor the frontend master-data administration surface into smaller, purpose-focused components and shared UI logic.
- Preserve current business behavior, API contracts, and supporting-domain scope while improving maintainability and testability.
- Clarify component boundaries for customer/brand management versus budget/prospect management.
- Keep scope limited to frontend maintainability improvements for the master-data admin surface; do not introduce new operator-facing capabilities.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `supporting-domain-management`: improve the frontend implementation structure of the existing master-data administration workflows without changing their business behavior.

## Impact

- Affects frontend files under `frontend/src/views/`, related master-data UI components, and any supporting composables used by the master-data admin surface.
- May affect frontend tests covering the master-data administration view if selectors or component boundaries need to be updated.
- Does not change backend APIs, business rules, or release scope.
