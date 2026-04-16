## 1. Production preflight secrets-hygiene gate

- [x] 1.1 Add a shared production secrets-hygiene validation step in `scripts/compose-preflight.sh` that rejects the documented blocked values for `MYSQL_PASSWORD`, `MYSQL_ROOT_PASSWORD`, `MI_DB_PASSWORD`, and `MI_JWT_SECRET` before container startup.
- [x] 1.2 Ensure the preflight failure output identifies each offending production secret key and keeps the gate scoped to production-oriented validation only.

## 2. Rehearsal integration and operator guidance

- [x] 2.1 Update `scripts/cutover-rehearsal.sh` to continue reusing the authoritative preflight validation so blocked placeholder secrets produce an early NO-GO before bootstrap, smoke, backup, or restore steps.
- [x] 2.2 Update deployment and release-readiness documentation to make the blocked-value contract and required production overrides explicit alongside the existing production defaults guidance.

## 3. Verification and release-gate evidence

- [x] 3.1 Add tests or self-check coverage that proves production preflight and rehearsal reject placeholder secrets while valid production overrides still pass.
- [ ] 3.2 Run the required verification commands for the implemented change and record commit-scoped evidence under `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json` for CI readiness.
- [ ] 3.3 Run the archive-only verification coverage required for this change and record commit-scoped evidence under `artifacts/verification/<commit-sha>/e2e.json`, ensuring release and archive gates use current-HEAD evidence only.
