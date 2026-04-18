## Why

22 of 23 frontend views have zero tests. While composable and API layers are fully tested, the two most critical user-facing entry points — LoginView (authentication gate) and DashboardView (post-login landing) — have no verification at the component level. A regression in either would break the core user flow immediately.

## What Changes

Add focused component tests for the 2 most critical views:
- `LoginView.vue` — form validation, submission, error display, redirect
- `DashboardView.vue` — summary loading, error handling, refresh

Plus lightweight smoke tests for 3 additional key views:
- `LeaseListView.vue` — mount + API call verification
- `BillingInvoicesView.vue` — mount + API call verification
- `GeneralizeReportsView.vue` — mount + report type rendering

## Scope

New test files:
- `frontend/src/views/LoginView.test.ts`
- `frontend/src/views/DashboardView.test.ts`
- `frontend/src/views/LeaseListView.test.ts`
- `frontend/src/views/BillingInvoicesView.test.ts`
- `frontend/src/views/GeneralizeReportsView.test.ts`
