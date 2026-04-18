## Design

### Overview

The first dashboard version will be implemented entirely in the frontend. It will call a small set of existing authenticated endpoints in parallel, read `total` counts from their responses, and render an operational summary page that becomes the post-login default.

### Route and Navigation

- Add a new flat route: `/dashboard`
- Insert a dashboard item as the first entry in `NAVIGATION_ITEMS`
- Preserve the existing `resolveAuthorizedHomePath` behavior so the first visible authorized item becomes the home route; making dashboard first naturally turns it into the landing page
- Keep `/health` available in navigation for diagnostics, but no longer as the default destination

### Dashboard Data Sources

The dashboard will use existing endpoints only:

- `GET /api/leases?status=active&page_size=1` → active lease count
- `GET /api/leases?status=pending_approval&page_size=1` → pending lease approvals
- `GET /api/invoices?status=pending_approval&page_size=1` → pending invoice approvals
- `GET /api/receivables?page_size=1` → open receivable count
- `GET /api/receivables?due_date_end=<today>&page_size=1` → overdue receivable count
- `GET /api/workflow/instances?status=pending` → pending workflow count via `items.length`

### View Structure

`DashboardView.vue` will contain:

1. **Hero section**
   - welcome title
   - summary copy describing the workbench purpose
   - optional refresh action

2. **Summary card grid**
   - active leases
   - pending lease approvals
   - pending invoice approvals
   - open receivables
   - overdue receivables
   - pending workflows

3. **Quick action cards**
   - create lease
   - open billing invoices
   - open receivables
   - open reports / health as secondary operations

4. **Priority queue panels**
   - approvals panel highlighting pending lease + invoice approvals
   - collections panel highlighting open + overdue receivables

### Component Strategy

- Reuse `PageSection` for the header area
- Use Element Plus `el-card`, `el-row`, `el-col`, `el-statistic`, and `el-button`
- Keep v1 in a single dedicated view file unless a card pattern becomes large enough to justify extraction
- Use existing `v-loading` pattern for the whole page while the initial parallel load is running

### Error Handling

- Use the shared `getErrorMessage` composable
- If one endpoint fails, show a page-level alert and keep cards that did load visible
- Avoid blocking the entire dashboard forever; always clear loading state in `finally`

### Verification

- Frontend build must pass
- Route should resolve for authenticated users
- Dashboard should become the landing page via navigation ordering and authorized home resolution
