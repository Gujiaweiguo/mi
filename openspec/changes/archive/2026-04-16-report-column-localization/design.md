## Context

The current reporting stack already treats report column labels as backend-owned output: `backend/internal/reporting/service.go` constructs `[]Column` with hard-coded English `Label` values, those labels are returned through `frontend/src/api/reports.ts`, rendered directly by `GeneralizeReportsView.vue`, and reused as exported workbook headers. That makes report terminology consistent across screen and export, but it also means the existing frontend locale infrastructure does not affect report column headers.

This change targets the frozen `R01-R19` reporting inventory only. It needs to align operator-facing output with Chinese acceptance expectations without changing report IDs, query parameters, row semantics, or the existing API shape (`columns[].key`, `columns[].label`, `rows[]`).

Stakeholders:

- Operators reading on-screen report tables and downloaded workbook headers.
- Acceptance reviewers validating report output against the frozen reporting matrix.
- Backend maintainers who own report column construction and export generation.

## Goals / Non-Goals

**Goals:**

- Localize backend-owned report column labels and export headers for the supported `R01-R19` inventory.
- Preserve the existing report API contract and frontend rendering flow while changing only the operator-facing labels.
- Centralize repeated report-label patterns (aging buckets, month/day headings, shared entity labels) so terminology remains consistent across reports.
- Add focused verification that catches label regressions in both query and export output.

**Non-Goals:**

- Reworking report filters, lookup UX, or report navigation.
- Changing report query logic, row values, sorting, or aggregation semantics.
- Introducing full runtime locale negotiation for reports beyond the localized label contract needed now.
- Renaming report IDs or changing the report payload shape consumed by the frontend.

## Decisions

### Decision 1: Keep report label ownership in the backend

The backend will remain the source of truth for report column labels and workbook headers.

Why:

- The same `Column` metadata already drives both on-screen tables and exported workbook headers.
- Moving label ownership to the frontend would either duplicate label mappings or require a new export-header contract.

Alternatives considered:

- Shift all labels to frontend i18n only: rejected because exports are generated in the backend and would drift from on-screen labels.
- Add a second parallel label mapping layer in the frontend: rejected because it duplicates the canonical report-output contract.

### Decision 2: Introduce shared backend label helpers rather than inline per-report strings

Report labels will be centralized in backend reporting helpers/constants for shared concepts and reused column sets.

Why:

- Existing inline strings are spread across many report branches and helper functions.
- Shared concepts such as store/department/brand/customer labels, aging buckets, and month/day headings should not be manually duplicated in multiple report cases.

Alternatives considered:

- Localize each report case independently in place: rejected because it would preserve duplication and raise long-term maintenance cost.

### Decision 3: Preserve the existing `Column` API shape and change only label content

The API will continue returning `columns[].key` and `columns[].label`; only the `label` values change.

Why:

- `GeneralizeReportsView.vue` and report export consumers already depend on this shape.
- This is a presentation-contract correction, not a transport or data-model redesign.

Alternatives considered:

- Add locale metadata or multi-language label payloads to the API: rejected because the current requirement is to localize accepted operator-facing output, not to redesign report localization architecture.

### Decision 4: Treat verification as output-level regression coverage

Verification will focus on concrete column/header terminology in report query responses and export output rather than broad UI i18n checks.

Why:

- The bug surface is backend-generated output, not frontend-authored shell copy.
- Output-level checks directly protect the acceptance surface this change is targeting.

Alternatives considered:

- Rely only on manual review: rejected because terminology regressions are easy to reintroduce silently.

## Risks / Trade-offs

- **[Risk] Acceptance terminology may differ from current operator expectations in a few columns** → **Mitigation:** anchor shared labels to the frozen report inventory/acceptance wording and keep terminology helpers explicit.
- **[Risk] Centralizing labels could unintentionally alter reports outside the intended inventory** → **Mitigation:** scope helper usage to `R01-R19` reporting paths only and verify representative outputs.
- **[Risk] Export verification may be more brittle than query verification** → **Mitigation:** keep tests focused on header rows and stable label sets rather than workbook formatting details.
- **[Risk] This does not solve broader report-filter usability issues** → **Mitigation:** keep that as a follow-up change rather than diluting this bounded output-localization fix.

## Migration Plan

1. Introduce shared localized report-label helpers in the backend reporting module.
2. Replace hard-coded English labels in report column builders and shared aging/month/day label helpers.
3. Verify query responses and export headers for representative reports across the frozen inventory.
4. Land without frontend route or API-shape changes so rollout is a pure output-text update.

Rollback approach:

- Revert the reporting label helper changes and associated verification updates as a single change set if terminology needs to be revised.

## Open Questions

- Should this change localize only Simplified Chinese operator output, or also formalize an eventual English-report-header contract for runtime locale switching later?
- Which exact acceptance source should break ties when report inventory wording and current UI summaries differ slightly?
