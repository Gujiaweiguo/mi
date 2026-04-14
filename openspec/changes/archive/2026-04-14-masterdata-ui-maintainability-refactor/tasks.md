## 1. Boundary audit and refactor plan

- [x] 1.1 Audit `frontend/src/views/MasterDataAdminView.vue` and document the stable section boundaries for customer/brand management versus budget/prospect management, including which state and handlers are section-local versus page-level.
- [x] 1.2 Define the target child-component and any narrow composable structure for the refactor so the route-level container remains the entrypoint while preserving existing interaction contracts and practical test selectors.

## 2. Customer and brand section extraction

- [x] 2.1 Extract the customer and brand administration UI into purpose-focused frontend components that keep the existing create/update and list-scale maintenance behaviors unchanged.
- [x] 2.2 Move customer/brand form state, mutation handlers, and section-local feedback as close as practical to the extracted section ownership without changing supported backend contracts.
- [x] 2.3 Keep or intentionally preserve the current selector and interaction surface needed by existing master-data operator flows so the refactor does not introduce unnecessary test churn.

## 3. Budget and prospect section extraction

- [x] 3.1 Extract the budget and prospect administration UI into focused frontend components while preserving the current unit/store budget and unit prospect workflows.
- [x] 3.2 Move budget/prospect form state, mutation handlers, and section-local feedback into the owning section structure, keeping only genuinely shared orchestration at the route level.
- [x] 3.3 Verify the extracted budget/prospect sections still use the same supported API requests, response handling, and operator-facing workflow semantics as the pre-refactor page.

## 4. Shared orchestration and maintainability cleanup

- [x] 4.1 Refactor shared page logic such as loading coordination, common feedback handling, and reusable lookup helpers only where the extraction clearly reduces duplication across sections.
- [x] 4.2 Reduce route-level cognitive load in `MasterDataAdminView.vue` so the remaining entrypoint code is primarily page composition and explicit cross-section orchestration.
- [x] 4.3 Keep the resulting frontend structure aligned with the updated `supporting-domain-management` delta spec by preserving behavior and avoiding any new operator-facing capability or validation change.

## 5. Verification and acceptance closure

- [x] 5.1 Update or add focused frontend coverage needed to prove the refactor preserves existing master-data behavior for customer, brand, budget, and prospect workflows.
- [x] 5.2 Run the frontend verification required for CI readiness and confirm current-commit unit and integration evidence remains available under `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json` where the repository gate expects it.
- [x] 5.3 Run the behavior-preserving master-data verification needed for archive readiness, including the existing e2e coverage, and confirm current-commit `unit`, `integration`, and `e2e` evidence under `artifacts/verification/<commit-sha>/` matches the implemented refactor.
