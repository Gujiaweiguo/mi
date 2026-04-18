## Problem

Two frontend consistency issues remain:

1. **`getErrorMessage` composable not adopted in 15 views** — The `getErrorMessage` function was extracted to `@/composables/useErrorMessage` but only 4 of 19 views import it. The other 15 views still use the inline ternary `error instanceof Error ? error.message : fallback` pattern (35 instances total).

2. **`downloadBlob` helper duplicated across 6 views** — An identical blob download function is copy-pasted in ExcelIOView and SalesAdminView, and the same `URL.createObjectURL` + click + `revokeObjectURL` pattern is inlined in 4 more views.

## Proposal

1. Replace all inline `error instanceof Error ? ...` ternaries with the imported `getErrorMessage` composable across all 15 remaining views.
2. Extract `downloadBlob` to `composables/useDownload.ts` and refactor 6 call sites.

## Scope

- New file: `frontend/src/composables/useDownload.ts`
- Modified: ~15 Vue view files for getErrorMessage adoption, ~6 for downloadBlob
