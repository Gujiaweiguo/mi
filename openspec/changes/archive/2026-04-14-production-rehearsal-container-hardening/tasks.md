## 1. Runtime and container baseline hardening

- [x] 1.1 Audit the supported production runtime layout under `deploy/compose/`, `deploy/docker/`, `deploy/runtime/`, and related env defaults to identify tracked runtime data, unsafe mount assumptions, and container user mismatches that make rehearsal results untrustworthy.
- [x] 1.2 Remove or prevent repository-tracked production runtime contamination so production-topology rehearsal starts from documented clean runtime baselines rather than leftover MySQL or generated runtime state.
- [x] 1.3 Align production container execution and mount behavior with documented hardening assumptions, including required writable runtime paths under the supported container user/runtime model.

## 2. Preflight and rehearsal guardrail hardening

- [x] 2.1 Tighten `scripts/compose-preflight.sh` and any related validation helpers so production-oriented startup fails fast when runtime directories, mount hygiene, or container-runtime assumptions violate the supported contract.
- [x] 2.2 Update `scripts/cutover-rehearsal.sh` and related operational scripts so current-commit production-topology rehearsal remains the authoritative GO/NO-GO path and aborts early on invalid verification evidence or runtime baselines.
- [x] 2.3 Strengthen smoke and restore-path validation where needed so backup, restore, and post-restore checks continue to exercise the supported production topology under the hardened runtime assumptions.

## 3. Verification evidence and production rehearsal proof

- [x] 3.1 Run the supported CI-ready and archive-ready verification needed for the evaluated commit and confirm the required evidence files exist under `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` with passing current-commit results.
- [x] 3.2 Execute the supported production cutover rehearsal command against a clean runtime baseline and confirm it writes machine-readable rehearsal output under `artifacts/rehearsal/<commit-sha>/` with a production-environment result for the evaluated commit.
- [x] 3.3 Resolve any NO-GO causes surfaced by the hardened production rehearsal until the supported workflow can produce a trustworthy GO result for the current commit.

## 4. Documentation and release-readiness closure

- [x] 4.1 Update go-live, deployment, and verification documentation so production-topology rehearsal, runtime hygiene expectations, and container-runtime assumptions are explicit and consistent with the hardened scripts.
- [x] 4.2 Document the release-readiness evidence chain for this hardening work so reviewers can verify that current-commit unit and integration evidence satisfy CI requirements while unit, integration, e2e, and production rehearsal outputs satisfy archive and go-live requirements.
- [x] 4.3 Recheck the change against the updated specs and design, ensuring the final implementation preserves fresh-start cutover rules and does not introduce any legacy transactional migration path.
