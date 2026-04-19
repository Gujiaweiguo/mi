## 1. Current-head release baseline

- [x] 1.1 `RRC-01` — Re-validate current HEAD archive readiness
  - **Depends On**: none
  - **Risk**: High
  - **Inputs**: current repository HEAD; `.github/workflows/ci-ready.yml`; `docs/go-live-checklist.md`; `scripts/verification/*`; `scripts/archive-ready.sh`
  - **Outputs**: `artifacts/verification/<commit-sha>/unit.json`; `artifacts/verification/<commit-sha>/integration.json`; `artifacts/verification/<commit-sha>/e2e.json`; `.sisyphus/evidence/task-1-current-head.txt`; `.sisyphus/evidence/task-1-current-head-delta.md`
  - **Acceptance**:
    - current target commit SHA is recorded
    - unit / integration / e2e evidence exists for the exact current SHA and passes
    - any mismatch with older release-ready summaries is explicitly documented
  - **Rollback**:
    - preserve failing evidence and logs
    - revert any documentation claim that current HEAD is verified if the gate fails
    - revert any harness-only edits introduced to run verification

- [x] 1.2 `RRC-02` — Rehearse production cutover on current HEAD
  - **Depends On**: `RRC-01`
  - **Risk**: High
  - **Inputs**: verified commit SHA from `RRC-01`; `scripts/cutover-rehearsal.sh`; `scripts/compose-smoke-test.sh`; `deploy/compose/docker-compose.production.yml`; `docs/go-live-checklist.md`
  - **Outputs**: `artifacts/rehearsal/<commit-sha>/cutover-rehearsal-production-<timestamp>.json`; matching rehearsal log; backup artifact under `artifacts/backups/production/`; `.sisyphus/evidence/task-2-cutover-rehearsal.json`; `.sisyphus/evidence/task-2-cutover-rehearsal-failure.md`
  - **Acceptance**:
    - rehearsal artifact exists for the exact current commit
    - rehearsal result reports `GO`
    - log confirms preflight, archive-ready, bootstrap, fresh-start, smoke, backup, and restore completion
    - backup artifact exists
  - **Rollback**:
    - restore the stack to a clean stopped state if rehearsal returns `NO-GO`
    - preserve artifacts/logs and block downstream release-summary work
    - revert temporary environment/config changes introduced solely for rehearsal

## 2. Scope-to-acceptance audit

- [x] 2.1 `RRC-03` — Build capability-to-implementation audit matrix
  - **Depends On**: none
  - **Risk**: Medium
  - **Inputs**: `docs/decision-log.md`; archived migration tasks; backend/frontend implementation surfaces; current verification evidence
  - **Outputs**: `.sisyphus/evidence/task-3-capability-matrix.md`; `.sisyphus/evidence/task-3-capability-gaps.md`
  - **Acceptance**:
    - every `PRESERVE` / `REDESIGN` row is listed exactly once
    - each row maps to backend surface, frontend surface, and evidence or explicit gap label
    - customer / prospect / brand management is explicitly mapped
  - **Rollback**:
    - revert premature “complete” claims in summary docs if mappings cannot be proven
    - keep disputed rows in gap/open state until evidence exists

- [x] 2.2 `RRC-04` — Build mandatory-output evidence matrix
  - **Depends On**: none
  - **Risk**: Medium
  - **Inputs**: `docs/output-catalog.md`; reporting/tax/docoutput/excel implementation surfaces; current verification evidence
  - **Outputs**: `.sisyphus/evidence/task-4-output-matrix.md`; `.sisyphus/evidence/task-4-output-gaps.md`
  - **Acceptance**:
    - every output ID `P01-P11`, `T01-T03`, `E01-E05`, `R01-R19` is represented exactly once
    - each output links to an implementation surface and current evidence source
    - missing or stale evidence is labeled blocking or non-blocking with rationale
  - **Rollback**:
    - revert any release notes claiming output closure if coverage cannot be proven
    - preserve the matrix and mark unsupported outputs as blockers instead of inferring parity

- [x] 2.3 `RRC-05` — Verify R19 visual acceptance on the UI layer
  - **Depends On**: none
  - **Risk**: Medium
  - **Inputs**: `docs/output-catalog.md`; `docs/decision-log.md`; `frontend/src/views/VisualShopAnalysisView.vue`; executable Playwright/browser environment
  - **Outputs**: `.sisyphus/evidence/task-5-r19-visual.png`; `.sisyphus/evidence/task-5-r19-visual-error.png`; optional `.sisyphus/evidence/task-5-r19-visual.md`
  - **Acceptance**:
    - UI-level evidence exists
    - evidence shows floor visual container, rendered unit markers, legend items, and selected-unit detail state
    - any mismatch between UI behavior and R19 acceptance language is documented as a release gap
  - **Rollback**:
    - revert any R19 acceptance claim to unverified/blocked if UI verification fails
    - preserve screenshots/error evidence and route the issue into blocker closure

## 3. Blocker closure and release hygiene

- [x] 3.1 `RRC-06` — Close release blockers from capability and output audits
  - **Depends On**: `RRC-02`, `RRC-03`, `RRC-04`, `RRC-05`
  - **Risk**: High
  - **Inputs**: blocker findings from `RRC-01` through `RRC-05`; relevant docs/code/evidence surfaces
  - **Outputs**: resolved code/docs/evidence updates; `.sisyphus/evidence/task-6-blocker-closure.md`; `.sisyphus/evidence/task-6-scope-guard.md`
  - **Acceptance**:
    - no unresolved blocking item remains in the gap artifacts
    - every closed blocker is tied to updated evidence or documentation on current HEAD
    - scope stays inside first-release boundaries
  - **Rollback**:
    - revert partial fixes if a blocker cannot be closed safely
    - keep release decision at `NO-GO` rather than leaving docs and evidence inconsistent

- [x] 3.2 `RRC-07` — Refresh release-ready summary for the actual validated HEAD
  - **Depends On**: `RRC-01`, `RRC-02`, `RRC-03`, `RRC-04`, `RRC-06`
  - **Risk**: Medium
  - **Inputs**: current verification artifacts; current rehearsal artifacts; capability/output audit results
  - **Outputs**: refreshed release-ready summary tied to current HEAD; `.sisyphus/evidence/task-7-release-summary-check.md`; `.sisyphus/evidence/task-7-release-summary-scope.md`
  - **Acceptance**:
    - a release summary exists for the actual validated HEAD
    - the summary cites current verification and rehearsal artifact paths
    - the summary does not claim broader scope than the evidence supports
  - **Rollback**:
    - do not publish a PASS summary if current HEAD is not fully validated
    - restore the prior summary or replace it with a blocked-status summary when necessary

- [x] 3.3 `RRC-08` — Harden production secret and bootstrap readiness
  - **Depends On**: `RRC-01`, `RRC-02`
  - **Risk**: High
  - **Inputs**: `deploy/compose/docker-compose.production.yml`; `docs/go-live-checklist.md`; bootstrap seed references; deployment docs
  - **Outputs**: updated deployment/go-live docs; `.sisyphus/evidence/task-8-prod-config.md`; `.sisyphus/evidence/task-8-prod-config-gaps.md`
  - **Acceptance**:
    - production docs explicitly reject placeholder DB and JWT secrets
    - bootstrap source files used for go-live are documented and cross-referenced
    - mismatches between rehearsal-safe defaults and production-safe requirements are documented and resolved
  - **Rollback**:
    - revert documentation that implies go-live safety if guidance cannot be hardened safely
    - keep the issue as a release blocker instead of weakening secret/bootstrap requirements

- [x] 3.4 `RRC-09` — Remove stale architecture placeholder signals
  - **Depends On**: `RRC-03`
  - **Risk**: Low
  - **Inputs**: `backend/internal/modules/README.md`; actual backend module layout; capability audit results
  - **Outputs**: corrected or removed stale placeholder docs; `.sisyphus/evidence/task-9-modules-readme.md`; `.sisyphus/evidence/task-9-architecture-churn.md`
  - **Acceptance**:
    - no stale placeholder remains claiming core backend modules are future work
    - repository guidance reflects the actual backend module organization
    - no unrelated structural churn is introduced
  - **Rollback**:
    - restore the previous doc only temporarily if cleanup creates confusion, then replace with corrected architecture guidance
    - do not move modules as a substitute for documentation cleanup

## 4. Final release decision

- [x] 4.1 `RRC-10` — Produce final release-decision packet
  - **Depends On**: `RRC-01`, `RRC-02`, `RRC-03`, `RRC-04`, `RRC-05`, `RRC-06`, `RRC-07`, `RRC-08`, `RRC-09`
  - **Risk**: Medium
  - **Inputs**: all evidence artifacts from `RRC-01` through `RRC-09`; `docs/go-live-checklist.md`
  - **Outputs**: `.sisyphus/evidence/task-10-release-packet.md`; `.sisyphus/evidence/task-10-release-packet-audit.md`; explicit `GO` / `NO-GO` recommendation
  - **Acceptance**:
    - packet exists and ends with explicit `GO` or `NO-GO`
    - every blocking item is either closed or listed as the reason for `NO-GO`
    - packet distinguishes blocking from non-blocking issues
  - **Rollback**:
    - revert any premature release recommendation if the packet cannot support a clean binary decision
    - publish `NO-GO` with blocker list instead of suppressing blockers to force `GO`
