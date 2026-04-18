## Problem

Three frontend quality issues affecting user experience and code maintainability:

1. **`getErrorMessage` helper duplicated in 4 views** — The same one-line function is copy-pasted into BaseInfoAdminView, RentableAreaAdminView, SalesAdminView, and StructureAdminView.

2. **Missing `v-loading` states** — Only 2 of 22 views (PrintPreviewView, WorkflowAdminView) use `v-loading` for loading indicators. The other 20 views show empty/stale content during API calls with no visual feedback.

3. **Missing form validation rules** — Only LoginView and LeaseCreateView use Element Plus `:rules` for form validation. All other admin views accept empty codes/names and rely on backend 400 errors.

## Proposal

1. Extract `getErrorMessage` to `composables/useErrorMessage.ts` and import in 4 views.
2. Add `isLoading` ref + `v-loading` directive to all views that make API calls.
3. Add Element Plus form validation rules to all create/edit form dialogs.

## Scope

- New file: `frontend/src/composables/useErrorMessage.ts`
- Modified: ~20 Vue view files in `frontend/src/views/`
