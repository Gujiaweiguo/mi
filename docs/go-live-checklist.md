# Go-Live Checklist

This checklist is the Task 18 operator-facing acceptance surface for `legacy-system-migration`.

## Rehearsal Command

Run the destructive rehearsal against the production Compose topology:

```bash
scripts/cutover-rehearsal.sh production --build
```

The binary result is written to:

```text
artifacts/rehearsal/<commit-sha>/cutover-rehearsal-production-<timestamp>.json
```

## Required Evidence

- Archive-ready gate for the current commit:
  - `artifacts/verification/<commit-sha>/unit.json`
  - `artifacts/verification/<commit-sha>/integration.json`
  - `artifacts/verification/<commit-sha>/e2e.json`
- Rehearsal result:
  - `artifacts/rehearsal/<commit-sha>/cutover-rehearsal-production-<timestamp>.json`
- Rehearsal log:
  - `artifacts/rehearsal/<commit-sha>/cutover-rehearsal-production-<timestamp>.log`

## Checklist

- [ ] Current commit is archive-ready.
- [ ] Production-topology rehearsal used a clean runtime directory and fresh-start bootstrap data only.
- [ ] Migrations applied successfully.
- [ ] Cutover bootstrap seeds loaded successfully.
- [ ] Fresh-start verification passed with no imported operational business data.
- [ ] Production stack smoke validation passed.
- [ ] Backup rehearsal completed.
- [ ] Restore rehearsal completed.
- [ ] Restore rehearsal included runtime-file restoration and post-restore smoke validation.
- [ ] Mandatory outputs remain validated against `docs/output-catalog.md` through current-commit archive evidence.
- [ ] Report scope `R01-R19` remains validated against `report-acceptance-matrix.md` through current-commit archive evidence.
- [ ] Archive-ready e2e evidence includes a passing live-stack operator flow against the supported runtime topology rather than mock-only browser coverage.

## Blocking Decisions Resolved By Repository Artifacts

- Permission action map: `backend/internal/platform/database/bootstrap/access_seed.go`
- Initial workflow template inventory: `backend/internal/platform/database/bootstrap/workflow_seed.go`
- Numbering initialization: `backend/internal/platform/database/bootstrap/workflow_seed.go`
- Organization root initialization: `backend/internal/platform/database/bootstrap/org_seed.go`
- Bootstrap admin baseline: `backend/internal/platform/database/bootstrap/access_seed.go`
- Tax/voucher configuration baseline: `backend/internal/platform/database/bootstrap/commercial_seed.go`
- Final cutover bootstrap pack: `backend/internal/platform/database/bootstrap/seed.go`

## Binary Decision Rule

- **GO** only when every checklist item above is satisfied and the rehearsal result artifact reports `"status": "GO"`.
- **NO-GO** if any checklist item fails or the rehearsal result artifact reports `"status": "NO-GO"`.
