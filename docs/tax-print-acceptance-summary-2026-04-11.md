# Tax Export / Document Output Acceptance Summary (2026-04-11)

## Scope

This acceptance summary covers the tax voucher export and document output slice for `legacy-system-migration`, with focus on Kingdee-format voucher workbook generation, multi-rule tax export with debit/credit entry pairs, invalid setup fail-fast, HTML template rendering with golden comparison, PDF generation via headless Chromium, multiple output modes (invoice-batch, invoice-detail, bill-state), and current-head verification evidence.

## Included commits

No new commits were required for this acceptance closure. The tax export and document output capabilities were implemented in prior changes and are verified against current HEAD.

## Spec baseline

- `openspec/changes/archive/2026-04-04-legacy-system-migration/specs/` (tax export and print/document output capabilities)

## Acceptance evidence (current HEAD)

Current HEAD:

- `49e141e62d2f7400373edfe73f4f45043690a922`

Verification evidence:

- `artifacts/verification/49e141e62d2f7400373edfe73f4f45043690a922/unit.json` — PASS (50/50)
- `artifacts/verification/49e141e62d2f7400373edfe73f4f45043690a922/integration.json` — PASS (84/84)
- `artifacts/verification/49e141e62d2f7400373edfe73f4f45043690a922/e2e.json` — PASS (41/41)

## Executed checks

### Tax export integration tests

1. `TestTaxExportServiceGeneratesKingdeeWorkbook`
   - Command:
     - `go test -tags=integration ./internal/taxexport -run TestTaxExportServiceGeneratesKingdeeWorkbook -count=1`
   - Result: PASS
   - Upsert rule set with debit/credit entry pair → export voucher workbook → verify 2 entries, 1 document → open Excel → verify FDate header, INV-101 voucher group, debit account 1122, credit amount 12000

2. `TestTaxExportServiceFailsFastOnInvalidSetup`
   - Command:
     - `go test -tags=integration ./internal/taxexport -run TestTaxExportServiceFailsFastOnInvalidSetup -count=1`
   - Result: PASS
   - Upsert single-sided rule (debit only) → export → verify ErrInvalidTaxSetup returned

### Document output integration tests

3. `TestDocOutputServiceRendersInvoiceBatchGoldenHTMLAndPDF`
   - Command:
     - `go test -tags=integration ./internal/docoutput -run TestDocOutputServiceRendersInvoiceBatchGoldenHTMLAndPDF -count=1`
   - Result: PASS
   - Upsert invoice-batch template → render HTML → compare against golden file → render PDF → verify %PDF header and content type → verify generated documents path exists

4. `TestDocOutputServiceRendersBillStateGoldenHTML`
   - Command:
     - `go test -tags=integration ./internal/docoutput -run TestDocOutputServiceRendersBillStateGoldenHTML -count=1`
   - Result: PASS
   - Upsert bill-state template → render HTML → compare against golden file

5. `TestDocOutputServiceRendersInvoiceDetailGoldenHTML`
   - Command:
     - `go test -tags=integration ./internal/docoutput -run TestDocOutputServiceRendersInvoiceDetailGoldenHTML -count=1`
   - Result: PASS
   - Upsert invoice-detail template → render HTML → compare against golden file

### Router integration test coverage

6. Tax export and print render endpoints
   - Command:
     - `go test -tags=integration ./internal/http -run TestIntegrationAuthAndOrgRoutes -count=1`
   - Result: PASS
   - Tax export voucher endpoint and print render endpoints exercised through HTTP

### E2E tests

7. `task15-workflow.spec.ts` (tax export and print segments)
   - Command:
     - `npx playwright test e2e/task15-workflow.spec.ts`
   - Result: PASS
   - Tax rule set selection → date range → export voucher → verify feedback → print template selection → render PDF → verify feedback

### Full verification gates (current HEAD)

8. Unit verification
   - Command:
     - `./scripts/verification/run-unit.sh 49e141e62d2f7400373edfe73f4f45043690a922`
   - Result: PASS

9. Integration verification
   - Command:
     - `./scripts/verification/run-integration.sh 49e141e62d2f7400373edfe73f4f45043690a922`
   - Result: PASS (84/84)

10. E2E verification
    - Command:
      - `./scripts/verification/run-e2e.sh 49e141e62d2f7400373edfe73f4f45043690a922`
    - Result: PASS (41/41)

11. CI gate
    - Command:
      - `./scripts/ci-ready.sh`
    - Result: PASS (`CI Ready: YES`)

12. Archive gate
    - Command:
      - `./scripts/archive-ready.sh`
    - Result: PASS (`Archive Ready: YES`)

## Acceptance outcomes

- Tax voucher export generates valid Kingdee-format Excel workbooks with configurable debit/credit entry pairs.
- Multi-rule rule sets produce correct entry counts and account numbers.
- Invalid tax setup (e.g., unbalanced single-sided rules) fails fast with a descriptive error.
- Document output supports three output modes: invoice-batch, invoice-detail, and bill-state.
- HTML rendering matches golden comparison files for all three output modes.
- PDF generation produces valid PDF output via headless Chromium.
- Generated documents are persisted to the configured storage path.
- Tax export and print render endpoints are accessible through authenticated HTTP routes.
- Frontend tax export page supports rule set selection, date range input, and export feedback.
- Frontend print preview page supports template selection, document ID input, and PDF rendering.
- Current HEAD is both `CI Ready` and `Archive Ready`.

## Traceability notes

- Tax export service: `backend/internal/taxexport/service.go`
- Tax export data access: `backend/internal/taxexport/repository.go`
- Tax export models: `backend/internal/taxexport/model.go`
- Tax export integration tests: `backend/internal/taxexport/service_integration_test.go`
- Tax export HTTP handler: `backend/internal/http/handlers/taxexport.go`
- Document output service: `backend/internal/docoutput/service.go`
- Document output data access: `backend/internal/docoutput/repository.go`
- Document output models: `backend/internal/docoutput/model.go`
- Document output integration tests: `backend/internal/docoutput/service_integration_test.go`
- Document output golden files: `backend/internal/docoutput/testdata/`
- Tax export HTTP routes: `backend/internal/http/router.go`
- Frontend tax API client: `frontend/src/api/tax.ts`
- Frontend print API client: `frontend/src/api/print.ts`
- Lease-billing E2E (tax/print segments): `frontend/e2e/task15-workflow.spec.ts`

## Conclusion

The tax export / document output acceptance slice is accepted for the covered first-release scope. Kingdee-format voucher generation, invalid setup fail-fast, golden-tested HTML rendering for three output modes, PDF generation, and current-head verification gates all pass on HEAD `49e141e62d2f7400373edfe73f4f45043690a922`.
