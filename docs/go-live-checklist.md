# Go-Live Checklist

This checklist is the Task 18 operator-facing acceptance surface for `legacy-system-migration`.

## Rehearsal Command

Run the destructive rehearsal against the production Compose topology:

```bash
scripts/cutover-rehearsal.sh production --build
```

The supported command now renders the production topology against an isolated
temporary runtime root, isolated host ports, and an isolated read-only config
copy so rehearsal proves the production service graph without depending on
contaminated repository runtime state or conflicting local host ports.

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
- Backup artifact created during rehearsal:
  - `artifacts/backups/production/mi-production-backup-<timestamp>.tar.gz`

## Checklist

- [ ] Current commit is archive-ready.
- [ ] CI-ready evidence for the current commit (`unit` + `integration`) passed before archive/go-live validation.
- [ ] Production env validation passed with no blocked placeholder secrets for `MYSQL_PASSWORD`, `MYSQL_ROOT_PASSWORD`, `MI_DATABASE_PASSWORD`, or `MI_AUTH_JWT_SECRET`.
- [ ] Production-topology rehearsal used a clean runtime directory and fresh-start bootstrap data only.
- [ ] Production-topology rehearsal used the supported isolated host-port/runtime/config path rather than reusing contaminated repository runtime state.
- [ ] Migrations applied successfully.
- [ ] Cutover bootstrap seeds loaded successfully.
- [ ] Fresh-start verification passed with no imported operational business data.
- [ ] Production stack smoke validation passed.
- [ ] Backup rehearsal completed.
- [ ] Restore rehearsal completed.
- [ ] Restore rehearsal included runtime-file restoration and post-restore smoke validation.
- [ ] Mandatory outputs remain validated against `docs/output-catalog.md` through current-commit archive evidence.
- [ ] Report scope `R01-R19` remains validated against `report-acceptance-matrix.md` through current-commit archive evidence.
- [ ] Archive-ready e2e evidence reflects the supported default e2e suite for the current commit, while production cutover confidence comes from the separate rehearsal GO artifact.

## Blocking Decisions Resolved By Repository Artifacts

- Permission action map: `backend/internal/platform/database/bootstrap/access_seed.go`
- Initial workflow template inventory: `backend/internal/platform/database/bootstrap/workflow_seed.go`
- Numbering initialization: `backend/internal/platform/database/bootstrap/workflow_seed.go`
- Organization root initialization: `backend/internal/platform/database/bootstrap/org_seed.go`
- Bootstrap admin baseline: `backend/internal/platform/database/bootstrap/access_seed.go`
- Tax/voucher configuration baseline: `backend/internal/platform/database/bootstrap/commercial_seed.go`
- Final cutover bootstrap pack: `backend/internal/platform/database/bootstrap/seed.go`

The committed `deploy/env/production.env` file is a template only. Rehearsal and go-live must use reviewed non-placeholder secret values, whether via the real env file or an isolated temporary copy prepared specifically for the rehearsal run.

## Binary Decision Rule

- **GO** only when every checklist item above is satisfied and the rehearsal result artifact reports `"status": "GO"`.
- **NO-GO** if any checklist item fails or the rehearsal result artifact reports `"status": "NO-GO"`.
