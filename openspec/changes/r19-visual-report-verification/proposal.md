## Why

R19 is the only first-release report that is explicitly visual-first rather than table-first, so its acceptance risk is higher than the other reports. The implementation already exists, but it still needs acceptance-grade verification that proves the visual semantics, data mapping correctness, and associated export behavior meet the frozen report contract.

## What Changes

- Execute a focused verification pass for R19 against the report inventory, acceptance matrix, and canonical specs.
- Add or tighten executable coverage around the most important R19 behaviors: visual mapping, query flow, export flow, permission behavior, and error handling.
- Fix any small implementation or test gaps uncovered during verification, especially around validation and high-risk edge cases.

## Capabilities

### New Capabilities
- `r19-visual-report-verification`: acceptance-grade verification evidence and coverage for the visual-first R19 report

### Modified Capabilities
- `supporting-domain-management`: R19 visual acceptance is strengthened with executable verification of presentation semantics and object-to-data mapping
- `report-output-localization`: R19 associated export headers are verified against localized operator-facing terminology

## Impact

- Affected frontend R19 view and e2e/unit test coverage in `frontend/src/views/VisualShopAnalysisView.vue`, `frontend/e2e/`, and related API tests.
- Affected backend reporting tests and possibly small validation fixes in `backend/internal/reporting/` and HTTP integration coverage.
- No planned scope expansion beyond R19 verification and defects found during that verification.
