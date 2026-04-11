# Report Acceptance Release Summary (2026-04-11)

## Scope

This release acceptance summary covers the reporting hardening work for `legacy-system-migration`, with focus on frozen report scope `R01-R19`, matrix-level acceptance semantics, persisted query/export audit evidence, and test-environment cutover readiness.

## Included commits

1. `4022c6b` — `Add report audit log migration schema`
2. `392b0f2` — `Persist report query and export audit records`
3. `b508953` — `Harden report acceptance coverage for matrix semantics`

## Spec and matrix baseline

- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`
- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`
- `openspec/specs/supporting-domain-management/spec.md`

## Acceptance evidence (current HEAD)

Current HEAD:

- `b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264`

Verification evidence:

- `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/unit.json`
- `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/integration.json`
- `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/e2e.json`

Traceability snapshot:

- `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/report-acceptance-traceability.md`

## Executed checks

### Reporting hardening verification

1. Backend reporting integration suite
   - Command:
     - `go test -tags=integration ./internal/reporting -run TestReportingServiceQueryAndExportCoreReports -count=1`
   - Result: PASS

2. Router/reporting integration suite
   - Command:
     - `go test -tags=integration ./internal/http -run TestIntegrationAuthAndOrgRoutes -count=1`
   - Result: PASS

3. Reporting acceptance E2E coverage
   - Command:
     - `npx playwright test e2e/task16-reporting.spec.ts --grep "covers acceptance-visible occupancy fields for R01 and R12"`
   - Result: PASS

### Full verification gates (current HEAD)

4. Unit verification evidence
   - Command:
     - `./scripts/verification/run-unit.sh b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264`
   - Result: PASS (`46/46`)

5. Integration verification evidence
   - Command:
     - `./scripts/verification/run-integration.sh b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264`
   - Result: PASS (`73/73`)

6. E2E verification evidence
   - Command:
     - `./scripts/verification/run-e2e.sh b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264`
   - Result: PASS (`41/41`)

7. CI gate
   - Command:
     - `./scripts/ci-ready.sh`
   - Result: PASS (`CI Ready: YES`)

8. Archive gate
   - Command:
     - `./scripts/archive-ready.sh`
   - Result: PASS (`Archive Ready: YES`)

### Cutover rehearsal

9. Test-environment cutover rehearsal
   - Command:
     - `./scripts/cutover-rehearsal.sh test`
   - Result: PASS (`GO`)
   - Artifact:
     - `artifacts/rehearsal/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/cutover-rehearsal-test-20260411T052721Z.json`

## Acceptance outcomes

- Report scope `R01-R19` is covered with explicit matrix-level acceptance evidence.
- High-risk cross-report rules are now asserted, including occupancy reconciliation, monthly pivot totals, aging breakdown reconciliation, null/zero-area handling, and period-aligned sales/rent semantics.
- Report query/export operations now persist audit evidence in `report_audit_logs`, including actor, action, request payload, row count, and export size.
- Current HEAD is both `CI Ready` and `Archive Ready`.
- Test cutover rehearsal completed with overall result `GO`, including bootstrap, smoke, backup, restore, and restore smoke validation.

## Traceability notes

- Report audit persistence is implemented in:
  - `backend/internal/reporting/service.go`
  - `backend/internal/reporting/repository.go`
  - `backend/internal/platform/database/migrations/000018_report_audit_schema.up.sql`
- Matrix-semantic reporting assertions are implemented in:
  - `backend/internal/reporting/service_integration_test.go`
  - `frontend/e2e/task16-reporting.spec.ts`
- End-to-end report audit evidence is validated in:
  - `backend/internal/http/router_integration_test.go`

## Conclusion

The reporting release slice is accepted for the covered first-release reporting scope. `R01-R19` matrix closure, audit evidence persistence, full verification gates, and test-environment cutover rehearsal all pass on current HEAD `b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264`.
