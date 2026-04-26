# Generalize / Media Boundary Closure (2026-04-26)

## Scope

This note closes the first-release boundary discussion for legacy `Generalize / Medium` capability by documenting how the replacement system preserves the required business outcomes without rebuilding the legacy module as a standalone bounded context.

## Boundary decision

- `docs/decision-log.md` marks `Generalize media/promotion management` as **REDESIGN**.
- The governing rationale is that legacy media concepts split across:
  - sales transaction media/payment details
  - reporting-facing Generalize outputs
  - visual-analysis output surfaces
- `docs/release-ready-summary-2026-04-19.md` records this blocker closure explicitly: `Generalize media / promotion management` was corrected into a redesign boundary rooted in legacy transaction-media handling plus current sales/report/output surfaces.

## Preserved business outcomes

The governed first-release scope still preserves the business outcomes that matter operationally:

1. **Commercial data remains available for downstream reporting and analysis**
   - Preserved through `backend/internal/sales/` and the sales-admin operating surface.

2. **Frozen Generalize reporting remains available through the governed report inventory**
   - Preserved through `backend/internal/reporting/`, `frontend/src/views/GeneralizeReportsView.vue`, and the frozen `R01-R19` contract.

3. **Visual shop analysis remains available as part of the frozen report scope**
   - Preserved through `frontend/src/views/VisualShopAnalysisView.vue` and the `R19` report surface.

## Non-goals for first release

The replacement system does **not** treat legacy `Generalize / Medium` as a required standalone rebuilt module for first release.

Specifically, first release does not require:

- page-for-page recreation of legacy `Web/Generalize/Medium/`
- a separate top-level module that mirrors legacy TV/radio/internet/print/activity/display/theme screens one-for-one
- preservation of ASP.NET page structure or legacy data-entry choreography as acceptance criteria by itself

## Current implementation anchors

- Sales-domain operational surface: `frontend/src/views/SalesAdminView.vue`
- Frozen tabular report surface: `frontend/src/views/GeneralizeReportsView.vue`
- Frozen visual report surface: `frontend/src/views/VisualShopAnalysisView.vue`
- Sales-domain backend anchor: `backend/internal/sales/`
- Reporting backend anchor: `backend/internal/reporting/`
- Report verification anchors: `backend/internal/reporting/service_integration_test.go`, `frontend/e2e/task16-reporting.spec.ts`, `frontend/e2e/task16-r19-visual.spec.ts`

## Closure statement

For the governed first-release scope, `Generalize / media / promotion management` is considered closed as a **redesign boundary**, not as a missing standalone module. The required operator outcomes are preserved through the current sales, reporting, and visual-analysis surfaces, and no blocker-class gap remains on this boundary unless the project decides to elevate legacy media/promotion administration into an explicit standalone first-release acceptance target.
