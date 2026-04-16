## 1. Backend report label localization foundation

- [x] 1.1 Audit the backend reporting module to inventory all hard-coded operator-facing English column labels, export-header sources, aging-bucket labels, and repeated month/day label patterns used across `R01-R19`.
- [x] 1.2 Introduce shared localized report-label helpers or constants in `backend/internal/reporting/` so reused concepts such as store, department, brand, customer, aging buckets, and date-series headers do not remain duplicated inline.
- [x] 1.3 Replace the existing hard-coded English column labels in the supported `R01-R19` report column builders with the accepted localized terminology while preserving existing `columns[].key` values and row semantics.

## 2. Query and export output alignment

- [x] 2.1 Ensure exported workbook header rows reuse the same localized label contract as on-screen query output for the supported `R01-R19` report inventory.
- [x] 2.2 Localize shared helper-generated headers such as aging buckets and month/day sequences so multi-report output stays terminology-consistent instead of mixing localized and English headers.
- [x] 2.3 Verify `R19` tabular/export output uses the same localized operator-facing terminology as the rest of the Generalize reporting inventory.

## 3. Verification and acceptance closure

- [x] 3.1 Add or update focused reporting tests that prove representative query responses return localized `columns[].label` values for multiple report families.
- [x] 3.2 Add or update export-focused verification that proves generated workbook headers use the localized terminology expected for the affected reports.
- [x] 3.3 Run the supported verification required for the evaluated commit and confirm current-commit evidence files under `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` remain passing after the localization change.
- [x] 3.4 Recheck the delivered output against the frozen reporting acceptance surface so `R01-R19` remain functionally unchanged while operator-facing headers now match accepted localized terminology.
