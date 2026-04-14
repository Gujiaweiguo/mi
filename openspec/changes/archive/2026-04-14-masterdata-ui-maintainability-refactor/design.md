## Context

The master-data administration frontend has recently become functionally complete for first-release supporting-domain needs, including customer and brand lifecycle maintenance plus budget/prospect administration. That functional closure came at the cost of concentration: `MasterDataAdminView.vue` now owns too many responsibilities in one file, including API orchestration, form state, feedback messages, table behavior, and multiple distinct domain sections.

This is not yet a user-facing bug, but it is now a maintainability risk. Future supporting-domain changes will be harder to review, test, and safely extend if this view remains the sole container for all of that logic. The right time to restructure is immediately after behavior stabilization, while the current workflows are still fresh and before another feature layer increases complexity further.

## Goals / Non-Goals

**Goals:**
- Split the current master-data administration view into smaller, purpose-focused frontend components with clearer ownership boundaries.
- Extract shared state or UI logic where it reduces duplication and makes the view easier to extend.
- Preserve existing business behavior, request/response contracts, and supporting-domain scope.
- Keep the resulting structure friendly to focused frontend testing and future maintenance.

**Non-Goals:**
- No backend API changes.
- No new master-data capabilities or operator workflows.
- No visual redesign beyond incidental layout adjustments required by component extraction.
- No new validation semantics or business-rule changes.

## Decisions

### 1. Keep one route-level container and move domain sections into child components

**Decision:** `MasterDataAdminView.vue` should remain the route entrypoint, but its domain sections should move into child components organized around business concerns such as customer/brand maintenance and budget/prospect maintenance.

**Why:** This keeps routing and top-level page composition stable while reducing the maintenance burden inside the route file itself.

**Alternatives considered:**
- Split the page into multiple routes. Rejected because the current operator workflow is intentionally unified in one admin surface.
- Leave everything in one file and only add comments. Rejected because structure, not documentation, is the main problem.

### 2. Extract shared composition logic where it serves more than one section

**Decision:** Shared feedback handling, common load patterns, and reusable master-data page state should move into composables or utilities only when the extraction reduces duplication across sections.

**Why:** A refactor for maintainability should reduce cognitive load, not replace one oversized file with many tiny files that still depend on implicit coupling.

**Alternatives considered:**
- Extract every reactive block into a composable. Rejected because over-extraction would make the flow harder to follow.

### 3. Preserve selectors and interaction contracts where practical

**Decision:** Existing test selectors and user-facing interaction contracts should be preserved where practical so the refactor stays behavior-preserving and avoids unnecessary test churn.

**Why:** This change is intended to improve maintainability, not to create avoidable instability in the test surface.

**Alternatives considered:**
- Freely rename selectors during refactor. Rejected because it adds churn without maintainability benefit.

### 4. Separate section-local form state from page-level orchestration

**Decision:** Form state and mutation handlers should live as close as possible to the component section that owns them, while page-level loading and shared dependencies remain at the route container or are passed through explicit props/composables.

**Why:** This clarifies what each section controls and reduces the number of unrelated reactive variables living in one scope.

**Alternatives considered:**
- Move all state into a single store. Rejected because this page-level workflow does not require global persistence and would overcomplicate local behavior.

## Risks / Trade-offs

- **[Risk] The refactor accidentally changes behavior while rearranging code** → **Mitigation:** preserve existing API contracts, selectors, and test coverage, and verify through the existing master-data e2e path.**
- **[Risk] Over-extraction creates too many small files with unclear boundaries** → **Mitigation:** organize components around business sections, not arbitrary technical fragments.**
- **[Risk] Shared state becomes harder to trace after extraction** → **Mitigation:** keep section-local state local and only extract truly shared logic.**
- **[Risk] The route container still remains too large after refactor** → **Mitigation:** define target boundaries up front and move entire domain sections rather than only cosmetic fragments.**

## Migration Plan

1. Identify stable section boundaries inside `MasterDataAdminView.vue`.
2. Extract section components with explicit props/events or narrow shared composables.
3. Keep the route-level integration behavior the same while migrating one section at a time.
4. Re-run typecheck/build and the existing master-data e2e coverage to confirm behavior preservation.

Rollback is straightforward because the change is frontend-only and behavior-preserving: revert the component extraction if the refactor introduces instability.

## Open Questions

- Should customer/brand and budget/prospect each become a single section component, or should they be split further into separate maintenance and table subcomponents?
- Is a small page-scoped composable warranted for shared feedback/loading behavior, or is explicit local component state clearer?
