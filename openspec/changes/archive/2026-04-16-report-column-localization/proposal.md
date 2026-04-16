## Why

The first-release `Generalize` reporting surface is functionally complete, but the backend still emits English column labels and export headers across `R01-R19`. That leaves a visible mismatch between the delivered operator-facing reports and the Chinese acceptance expectations frozen in the reporting inventory and matrix.

## What Changes

- Localize backend-provided report column labels for the supported `R01-R19` reporting surface so on-screen report tables no longer expose hard-coded English headers by default.
- Localize exported report workbook headers for the same report inventory so downloadable output matches the accepted operator-facing terminology.
- Define the operator-facing label contract for report output, including shared aging-bucket labels, monthly column labels, and other reused reporting header patterns.
- Add focused verification for localized report headers so report output acceptance covers terminology correctness as well as query correctness.

## Capabilities

### New Capabilities
- `report-output-localization`: Localized operator-facing report column and export-header output for the frozen `R01-R19` reporting inventory.

### Modified Capabilities
- `supporting-domain-management`: Tighten the accepted reporting surface so first-release `Generalize` output includes operator-facing localized headers rather than only functionally correct data columns.

## Impact

- Affected backend code: `backend/internal/reporting/` report column/header generation and shared reporting label helpers.
- Affected frontend/report surfaces: `frontend/src/views/GeneralizeReportsView.vue` and any report views that render backend-provided column labels.
- Affected exports: generated workbook headers for `R01-R19` report downloads.
- Affected verification: report-focused tests/evidence for on-screen and exported header terminology.
