## Design

### Verification Goals

This change verifies R19 across the two acceptance axes called out by the canonical spec:

1. **Visual presentation semantics**
2. **Correctness of mapping between visual objects and underlying shop/unit data**

### Current Implementation Surface

- Frontend route: `frontend/src/router/index.ts` → `/reports/visual-shop`
- Frontend page: `frontend/src/views/VisualShopAnalysisView.vue`
- Frontend API: `frontend/src/api/reports.ts` → `POST /reports/r19/query` and `POST /reports/r19/export`
- Backend handler: `backend/internal/http/handlers/reporting.go`
- Backend service/repository: `backend/internal/reporting/service.go`, `backend/internal/reporting/repository.go`

### Acceptance Targets

The verification pass must cover:

1. Visual surface renders as a graphical layout rather than only a table
2. Visual objects expose core report fields and correctly map to unit/shop data
3. Filters (store, floor, area) drive the visual result correctly
4. Export path for the associated structured artifact still works
5. Localized export headers remain correct for the R19 tabular artifact
6. Permission behavior and failure states are correctly handled

### Planned Executable Checks

#### Frontend
- Run and extend R19 Playwright coverage for query, render, marker interaction, and export
- Add focused frontend unit tests for the R19 page when current e2e coverage leaves important behavior unverified
- Verify error-state handling for failed query/export interactions if currently uncovered

#### Backend
- Run the existing reporting integration tests and HTTP integration tests that already include R19
- Add isolated backend coverage only where broad integration tests do not make the R19 behavior explicit enough
- If necessary, fix misleading or weak validation surfaced by verification (for example, floor-specific validation)

### Evidence Standard

The output of this change is not just “tests pass”; it is a documented and executable acceptance pass that links the frozen R19 contract to:

- concrete frontend behavior
- concrete backend query/export behavior
- localized operator-facing output

### Out of Scope

- Re-architecting the R19 page
- Introducing a new visualization system
- Splitting permissions beyond current first-release scope unless verification proves current behavior incorrect
