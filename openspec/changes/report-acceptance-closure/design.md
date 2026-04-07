## Context

The migration change already froze the first-release `Generalize` reporting boundary through `report-inventory.md` and `report-acceptance-matrix.md`, and Task 16 declared the report slice implemented. What is still missing is a closure workflow that verifies each report family against the matrix, records any remaining acceptance gaps, and defines what counts as “good enough for first release” versus “must fix before closure.”

This change should not re-open reporting scope. It should convert the frozen reporting inventory into a practical acceptance program using the existing report IDs, existing report families, and executable verification evidence. The work remains inside the replacement migration target and should avoid page-by-page legacy recreation beyond the frozen matrix.

## Goals / Non-Goals

**Goals:**
- Define a concrete acceptance-closure process for `R01-R19`.
- Group report verification by report family so execution is tractable and traceable.
- Clarify how implemented, partially implemented, and blocked report states are documented.
- Require explicit verification and evidence for query behavior, export behavior, and cross-report consistency where the matrix implies reconciliation.

**Non-Goals:**
- No expansion beyond the frozen `R01-R19` report inventory.
- No redesign of the report UI shell, report routing model, or export architecture.
- No net-new analytics capability outside the existing first-release `Generalize` slice.
- No assumption that every report must be rewritten; this change is about acceptance closure, not automatic reimplementation.

## Decisions

### 1. Accept reports by verification family, not as nineteen unrelated one-offs

**Decision:** The closure workflow should verify reports in grouped families: lease/area, contract ledger, sales/traffic, budget/pricing, aging/receivable, composite, and visual.

**Why:** The matrix spans nineteen reports, but many acceptance rules share the same data contracts and verification mechanics. Grouping by family makes the work auditable and reduces duplicate verification effort.

**Alternatives considered:**
- Verify all reports individually without grouping: rejected because it obscures shared dependencies and makes closure harder to track.

### 2. Treat matrix compliance as the canonical acceptance contract

**Decision:** Report acceptance should be measured against the frozen matrix fields, filters, output form, and acceptance checks, using `Report ID` as the stable identifier even if UI titles evolve.

**Why:** The matrix is already the frozen release contract. Re-centering verification on `Report ID` avoids ambiguity from renamed UI labels and keeps implementation aligned with the archived migration decision record.

**Alternatives considered:**
- Use whatever the current UI exposes as the acceptance baseline: rejected because it could silently diverge from the frozen report scope.

### 3. Document unresolved gaps explicitly instead of silently treating them as complete

**Decision:** If a report or acceptance check is incomplete, the closure flow must classify it as fix-now or documented non-go-live exception rather than leaving implicit uncertainty.

**Why:** The first-release boundary is only trustworthy if unresolved gaps are explicit. Hidden “almost done” reports are riskier than documented exceptions.

**Alternatives considered:**
- Close the change when most reports look acceptable: rejected because the matrix exists specifically to avoid subjective closure.

### 4. Require executable evidence for closure, not spreadsheet-only signoff

**Decision:** Report acceptance closure should culminate in executable verification and commit-scoped evidence under the existing `unit` / `integration` / `e2e` convention, with report-focused tests or evidence extensions where needed.

**Why:** The repository already treats machine-readable evidence as the gate for CI and archive. Reporting closure should follow the same standard instead of relying on ad hoc human confirmation.

**Alternatives considered:**
- Manual checklist signoff only: rejected because it would weaken the repository’s established gate discipline.

## Risks / Trade-offs

- **[Risk] Some reports may appear implemented but fail reconciliation checks** → **Mitigation:** allow the change to surface explicit fix-now vs. documented-gap decisions instead of forcing false closure.
- **[Risk] Visual report `R19` may need different acceptance mechanics than tabular reports** → **Mitigation:** define `R19` as a separate visual family with output-specific acceptance criteria.
- **[Risk] Cross-report checks can expose upstream data-contract issues outside reporting code** → **Mitigation:** document those dependencies directly in tasks and classify them as required fixes or explicit exceptions.

## Migration Plan

1. Map `R01-R19` into report verification families using the frozen inventory and acceptance matrix.
2. Verify delivered report behavior against matrix expectations for query filters, core fields, and output forms.
3. Run cross-report reconciliation checks where the matrix implies totals must align.
4. Record unresolved gaps as explicit fix-now items or non-go-live exceptions.
5. Produce commit-scoped verification evidence before archive.

## Open Questions

- Should any unresolved report gap be allowed to remain as a documented non-go-live exception, or must `report-acceptance-closure` close only when all `R01-R19` matrix checks pass?
- For `R19`, is visual inspection plus stable data mapping evidence sufficient, or is there an expected structured export/assertion contract we should encode in tests?
