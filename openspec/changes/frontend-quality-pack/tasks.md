## 1. Extract getErrorMessage composable

- [x] 1.1 Create `frontend/src/composables/useErrorMessage.ts` with exported `getErrorMessage` function
- [x] 1.2 Remove local `getErrorMessage` from `BaseInfoAdminView.vue`, import from composable
- [x] 1.3 Remove local `getErrorMessage` from `RentableAreaAdminView.vue`, import from composable
- [x] 1.4 Remove local `getErrorMessage` from `SalesAdminView.vue`, import from composable
- [x] 1.5 Remove local `getErrorMessage` from `StructureAdminView.vue`, import from composable

## 2. Add v-loading states

- [x] 2.1 Add `isLoading` ref + `v-loading` to all views that make API calls but lack loading indicators (approximately 20 views)
- [x] 2.2 Ensure `isLoading` is set to `true` before API calls and `false` after completion

## 3. Add form validation rules

- [x] 3.1 Add Element Plus `:rules` to form dialogs in all admin views that lack validation
- [x] 3.2 Add `prop` attributes to form items for validation targeting
- [x] 3.3 Add form ref validation call before submit

## 4. Verification

- [x] 4.1 Frontend builds successfully with `npm run build`
- [x] 4.2 No TypeScript errors
