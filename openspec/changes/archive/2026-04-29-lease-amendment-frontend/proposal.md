## Why

Lease amendment is already part of the first-release contract lifecycle in `openspec/specs/lease-contract-management/spec.md`, and the backend plus frontend API already support it, but operators still cannot initiate an amendment from the Vue application. This leaves a specified billing-effective workflow unreachable in day-one operations and creates an avoidable gap in the Lease → Bill / Invoice chain.

## What Changes

- Add an operator-facing amendment entry point from the lease detail experience for amendment-eligible contracts.
- Reuse the existing lease creation/editing flow to capture amendment input instead of introducing a separate amendment screen.
- Define the first-release frontend behavior for loading an amendment draft from the source lease, submitting it through the existing amendment API, and returning the operator to the resulting amended contract.
- Add focused frontend automated coverage for amendment entry, prefilled form behavior, and successful amendment submission.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `lease-contract-management`: extend the Lease lifecycle requirement so the first-release frontend exposes amendment initiation and preserves the amended-contract flow as an operator-reachable action.

## Impact

- Frontend: `frontend/src/views/LeaseDetailView.vue`, `frontend/src/views/LeaseCreateView.vue`, router/query handling, and related i18n strings.
- Frontend API reuse: `frontend/src/api/lease.ts` amendment request path remains the integration contract.
- Verification: focused Vue unit tests around amendment entry and submission, plus normal typecheck/test execution for the touched surface.
