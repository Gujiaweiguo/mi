## 1. Change Artifacts

- [x] 1.1 Create proposal.md for workbench-dashboard
- [x] 1.2 Create design.md for workbench-dashboard
- [x] 1.3 Create specs/workbench-dashboard/spec.md
- [x] 1.4 Create tasks.md

## 2. Dashboard Routing and Navigation

- [ ] 2.1 Add a new authenticated `/dashboard` route in `frontend/src/router/index.ts`
- [ ] 2.2 Add dashboard as the first navigation item in `frontend/src/auth/permissions.ts`
- [ ] 2.3 Ensure authenticated root redirect lands on dashboard instead of health

## 3. Dashboard UI

- [ ] 3.1 Create `frontend/src/views/DashboardView.vue`
- [ ] 3.2 Add hero section and summary card grid
- [ ] 3.3 Add quick action links into core business flows
- [ ] 3.4 Add priority queue / work summary panels
- [ ] 3.5 Add shared loading and error states

## 4. Dashboard Data Wiring

- [ ] 4.1 Add frontend API calls that fetch dashboard counts from existing endpoints in parallel
- [ ] 4.2 Map existing `total` and workflow array counts into dashboard card state
- [ ] 4.3 Refresh dashboard data on initial mount

## 5. Verification

- [ ] 5.1 Run `npm run build` from `frontend/`
- [ ] 5.2 Verify the dashboard route is reachable for authenticated users
- [ ] 5.3 Verify dashboard is the default post-login landing page
