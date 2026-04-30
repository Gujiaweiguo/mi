## 1. Backend filter support

- [x] 1.1 Extend receivable filter types and request parsing to accept `lease_contract_id`
- [x] 1.2 Update receivable repository queries to apply optional `lease_contract_id` filtering without changing default list behavior
- [x] 1.3 Add or update backend tests covering receivable list filtering by `lease_contract_id`

## 2. Frontend downstream panel

- [x] 2.1 Extend frontend invoice API types so receivable queries can request `lease_contract_id`
- [x] 2.2 Add lease-detail downstream state loading for charges, invoices, and receivables alongside the existing overtime section
- [x] 2.3 Render a lease-detail downstream summary / timeline panel with identifiers, statuses, dates, and supported quick-entry links

## 3. Verification

- [x] 3.1 Add or update frontend unit tests covering downstream panel loading, empty states, and section-level error handling
- [x] 3.2 Run frontend lint, build, and unit tests; run relevant backend tests for the receivable filter change
- [x] 3.3 Capture verification evidence appropriate for the resulting commit scope (CI requires `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json`; archive additionally requires `artifacts/verification/<commit-sha>/e2e.json`)
