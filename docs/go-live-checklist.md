# Go-Live Checklist

This checklist is the Task 18 operator-facing acceptance surface for `legacy-system-migration`.

## Rehearsal Command

Run the destructive rehearsal in the test environment:

```bash
scripts/cutover-rehearsal.sh test --build
```

The binary result is written to:

```text
artifacts/rehearsal/<commit-sha>/cutover-rehearsal-test-<timestamp>.json
```

## Required Evidence

- Archive-ready gate for the current commit:
  - `artifacts/verification/<commit-sha>/unit.json`
  - `artifacts/verification/<commit-sha>/integration.json`
  - `artifacts/verification/<commit-sha>/e2e.json`
- Rehearsal result:
  - `artifacts/rehearsal/<commit-sha>/cutover-rehearsal-test-<timestamp>.json`
- Rehearsal log:
  - `artifacts/rehearsal/<commit-sha>/cutover-rehearsal-test-<timestamp>.log`

## Checklist

- [ ] Current commit is archive-ready.
- [ ] Test rehearsal used a clean runtime directory and fresh-start bootstrap data only.
- [ ] Migrations applied successfully.
- [ ] Cutover bootstrap seeds loaded successfully.
- [ ] Fresh-start verification passed with no imported operational business data.
- [ ] Test stack smoke validation passed.
- [ ] Backup rehearsal completed.
- [ ] Restore rehearsal completed.
- [ ] Mandatory outputs remain validated against `docs/output-catalog.md` through current-commit archive evidence.
- [ ] Report scope `R01-R19` remains validated against `report-acceptance-matrix.md` through current-commit archive evidence.

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
