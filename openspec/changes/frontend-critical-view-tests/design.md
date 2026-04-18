## Design

### Test Approach

Follow the pattern from existing `MasterDataAdminView.test.ts`:
- Use `@vue/test-utils` `mount` with component stubs for Element Plus
- Use `vi.mock` for API modules
- Use `createPinia()` for store access
- Use `i18n` for locale

Focus tests on:
1. **Component mounts without errors** — basic smoke test
2. **API calls happen on mount** — verify correct API function is called
3. **Error handling** — verify error display when API fails
4. **Form interaction** (LoginView) — verify form validation triggers

### Per-View Test Plan

**LoginView** (most critical):
- Mounts without error
- Renders login form with username/password inputs
- Disables submit with empty fields
- Calls login API on valid form submission
- Shows error message on failed login
- Redirects to dashboard on successful login

**DashboardView**:
- Mounts without error
- Calls getDashboardSummary on mount
- Displays summary cards with data
- Shows error alert on API failure
- Refresh button triggers reload

**LeaseListView, BillingInvoicesView, GeneralizeReportsView** (smoke only):
- Mounts without error
- Calls correct list API on mount
