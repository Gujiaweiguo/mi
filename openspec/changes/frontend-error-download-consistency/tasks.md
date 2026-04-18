## 1. getErrorMessage adoption

- [ ] 1.1 Replace all inline `error instanceof Error ? error.message : fallback` with imported `getErrorMessage` in 15 remaining views
- [ ] 1.2 Verify no inline error ternaries remain

## 2. downloadBlob extraction

- [ ] 2.1 Create `frontend/src/composables/useDownload.ts` with `downloadBlob` function
- [ ] 2.2 Replace inline download functions in ExcelIOView.vue and SalesAdminView.vue
- [ ] 2.3 Replace inline blob download patterns in PrintPreviewView, TaxExportsView, GeneralizeReportsView, VisualShopAnalysisView
- [ ] 2.4 Verify no duplicated download logic remains

## 3. Verification

- [ ] 3.1 `npm run build` passes from `frontend/`
- [ ] 3.2 No TypeScript errors
