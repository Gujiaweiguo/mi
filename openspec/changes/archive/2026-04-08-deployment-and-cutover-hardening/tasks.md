## 1. Compose bring-up hardening

- [x] 1.1 Audit and tighten the existing Compose render/startup workflow so test and production environments validate rendered configuration before container startup.
- [x] 1.2 Add explicit runtime-directory and mounted-path validation for the supported deployment environments before the workflow can report environment readiness.
- [x] 1.3 Verify that the supported bring-up path checks nginx, frontend, backend, and MySQL health against the documented entry points and runtime assumptions.

## 2. Backup and restore rehearsal

- [x] 2.1 Harden the backup workflow so the supported environment can produce a deterministic backup bundle containing the required database, runtime, and config artifacts.
- [x] 2.2 Harden the restore workflow so a generated backup bundle can be restored through the supported operator path with explicit handling for runtime-file restoration.
- [x] 2.3 Add post-restore validation so backup rehearsal is not considered complete until the restored environment passes the required operational smoke checks.

## 3. Cutover rehearsal and GO/NO-GO output

- [x] 3.1 Implement or tighten the cutover rehearsal entry flow so render, startup, bootstrap validation, smoke checks, backup, restore, and release-gate evaluation run in one deterministic sequence.
- [x] 3.2 Enforce current-HEAD archive-ready evidence checks inside rehearsal, using `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` and rejecting missing or stale evidence.
- [x] 3.3 Enforce fresh-start cutover validation so rehearsal rejects any path that attempts legacy transactional business-data migration.
- [x] 3.4 Emit machine-readable rehearsal result artifacts and logs under `artifacts/rehearsal/<commit-sha>/` with a binary GO/NO-GO outcome and enough detail to identify the failing gate.

## 4. Documentation alignment and verification

- [x] 4.1 Update the operator-facing deployment and cutover documentation so commands, prerequisites, artifact paths, and decision rules match the hardened executable workflow.
- [x] 4.2 Add or tighten automated verification for Compose validation, backup/restore rehearsal behavior, and cutover GO/NO-GO artifact generation at the closest executable level available in the repository.
- [x] 4.3 Run the verification suite for the current commit and record machine-readable evidence in `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [x] 4.4 Confirm the current commit satisfies gate expectations for this change: CI requires passing `unit` and `integration` evidence, while archive requires passing `unit`, `integration`, and `e2e` evidence for the same commit SHA.
