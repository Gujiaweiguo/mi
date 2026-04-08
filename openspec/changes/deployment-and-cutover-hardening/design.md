## Context

The repository already has most of the structural pieces required by `deployment-and-cutover-operations`: Compose manifests live under `deploy/compose/`, runtime mounts are documented in `docs/deployment-topology.md`, and cutover expectations are described in `docs/cutover-runbook.md` and `docs/go-live-checklist.md`. However, this surface is still only partially closed operationally. The documentation defines what a healthy test/production environment, backup bundle, restore flow, and cutover rehearsal should look like, but the current system has not yet been hardened into a single repeatable operator workflow that produces authoritative pass/fail artifacts for deployment readiness.

This change is cross-cutting but bounded. It touches deployment scripts, runtime directory assumptions, rehearsal artifact generation, and the relationship between archive-ready evidence and go-live readiness. It does not introduce a new business capability; instead, it converts an already-specified operational surface into an executable, verifiable release discipline for the first-release system.

## Goals / Non-Goals

**Goals:**
- Turn test and production Compose bring-up into a repeatable, validated workflow that checks rendered config, service health, and required runtime mounts.
- Make backup and restore rehearsal executable and auditable rather than documentation-only, including validation that the restored environment is still operable.
- Make `scripts/cutover-rehearsal.sh` or equivalent cutover execution produce machine-readable GO/NO-GO artifacts tied to the current commit SHA.
- Preserve the fresh-start cutover rule by ensuring rehearsal logic refuses any path that implies legacy transactional migration.
- Align `docs/cutover-runbook.md`, `docs/go-live-checklist.md`, and deployment scripts so that the operator-facing documents describe the exact executable workflow.

**Non-Goals:**
- No new application business flows in Lease, billing, reporting, or supporting-domain operations.
- No migration of historical legacy records, open drafts, approvals, or in-flight transactions.
- No new deployment topology beyond the existing nginx + frontend + backend + MySQL four-service shape.
- No full production orchestration redesign outside the current Compose-based model.
- No relaxation of current commit-scoped evidence gates.

## Decisions

### 1. Keep `deployment-and-cutover-operations` as an operational hardening slice, not a new platform subsystem

**Decision:** This change modifies the existing `deployment-and-cutover-operations` capability instead of introducing a new capability for rehearsal tooling or release management.

**Why:** The core requirements already exist in the main spec. The gap is not missing scope, but incomplete execution and verification of that scope. Keeping the work inside the current capability avoids creating an artificial split between Compose operations, backup/restore, and cutover rehearsal that operators will experience as one release-readiness flow.

**Alternatives considered:**
- Create a separate capability such as `release-rehearsal-automation`. Rejected because the current spec already owns this operational surface and splitting it would add ceremony without reducing complexity.

### 2. Make the cutover rehearsal the single top-level operational entry point

**Decision:** The hardened workflow should treat cutover rehearsal as the top-level operator command, with Compose rendering/bring-up, bootstrap validation, smoke tests, backup rehearsal, restore rehearsal, and GO/NO-GO evaluation executed as staged substeps.

**Why:** The current docs already frame go-live readiness as a binary outcome with multiple prerequisite checks. Operators need one authoritative path that answers “is this release go-live ready?” rather than a loose collection of independent scripts.

**Alternatives considered:**
- Keep separate independent scripts only and rely on humans to sequence them manually. Rejected because that is exactly the current gap: the workflow exists in prose, but not as one executable release discipline.

### 3. Reuse commit-scoped evidence as a hard prerequisite for rehearsal, not as a rehearsal output

**Decision:** Cutover rehearsal will require current-HEAD archive-ready evidence (`unit`, `integration`, `e2e`) to already exist before GO/NO-GO evaluation can succeed. Rehearsal outputs will be additional artifacts under `artifacts/rehearsal/<commit-sha>/`, not a replacement for verification evidence.

**Why:** The repository already has a clear gate hierarchy: CI evidence, archive evidence, then go-live readiness. Preserving that ordering keeps release discipline coherent and prevents rehearsal from being used to “paper over” missing application verification.

**Alternatives considered:**
- Allow rehearsal to run and decide GO even when archive evidence is stale or absent. Rejected because it would weaken the evidence contract already encoded in the docs and prior changes.

### 4. Treat backup and restore as a rehearsal pair with explicit validation after restore

**Decision:** Backup rehearsal and restore rehearsal should be modeled as a pair inside the operational flow: create a bundle, restore it into the target environment, then run concrete post-restore checks against the restored stack.

**Why:** A backup is not operationally meaningful unless restore works. The current topology doc already defines both commands, but release confidence requires proof that the restored environment still satisfies the expected health and runtime assumptions.

**Alternatives considered:**
- Mark backup creation alone as sufficient. Rejected because it does not verify that the bundle can actually recover the environment.

### 5. Keep rehearsal results machine-readable and commit-addressed

**Decision:** Rehearsal outputs should remain machine-readable JSON plus log files under `artifacts/rehearsal/<commit-sha>/`, with a binary `GO` / `NO-GO` result and enough step-level detail to identify the failing gate.

**Why:** This matches the project’s established evidence model and makes operational readiness reviewable in the same way as test readiness. It also prevents ambiguous “manual pass” claims.

**Alternatives considered:**
- Use only markdown checklists or console output. Rejected because the rest of the repository already relies on machine-readable evidence and artifact paths.

### 6. Keep runtime and bootstrap validation conservative and first-release scoped

**Decision:** Rehearsal logic should validate only the explicitly documented first-release environment assumptions: runtime directory existence/writability, environment-specific config/env pairing, bootstrap-only data initialization, minimal smoke flows, and mandatory output/report checks referenced by existing docs.

**Why:** The project is a fresh-start cutover, not a generalized migration/orchestration framework. Conservative validation is easier to trust and less likely to drift beyond the frozen first-release contract.

**Alternatives considered:**
- Expand rehearsal into a generalized operations framework covering broader infrastructure scenarios. Rejected because it would widen scope away from the first-release migration target.

## Risks / Trade-offs

- **[Risk] Runtime directories or mounted paths differ across environments and make rehearsal brittle** → **Mitigation:** validate runtime directory existence and writability before Compose startup and fail fast with explicit diagnostics.
- **[Risk] Backup/restore rehearsal becomes destructive to a developer’s local environment** → **Mitigation:** keep test/production environment targets explicit, use isolated runtime roots, and make destructive actions opt-in at the script layer.
- **[Risk] GO/NO-GO logic duplicates rules that already exist in docs and drifts over time** → **Mitigation:** align script checks directly to the checklist/runbook items and keep documentation updates in the same change whenever execution rules change.
- **[Risk] Rehearsal may pass infrastructure checks while missing application-level readiness** → **Mitigation:** require current-commit archive-ready evidence before a GO result can be issued.
- **[Risk] Backup bundles become large or slow to restore in CI-like environments** → **Mitigation:** keep first-release rehearsal focused on deterministic operational data and bounded runtime paths rather than broad host snapshots.

## Migration Plan

1. Audit the current Compose scripts, runtime directories, backup/restore scripts, and cutover rehearsal entry point against the deployment topology and go-live checklist.
2. Harden script sequencing so render → startup → health validation → backup → restore → smoke validation → GO/NO-GO evaluation follows one deterministic order.
3. Add or tighten rehearsal artifact generation under `artifacts/rehearsal/<commit-sha>/` with structured pass/fail output.
4. Align operator-facing docs with the executable workflow, updating only the sections needed to match actual commands, prerequisites, and artifact paths.
5. Add verification coverage for the hardened operational workflow at the closest executable level available in the repo.
6. Validate the hardened flow against the current commit-scoped evidence contract before any later archive step for this change.

Rollback remains straightforward because this change is operational and additive: if the hardened rehearsal path proves unstable, the repository can continue using the existing Compose manifests and docs while the new orchestration logic is corrected. No business data migration or irreversible application-state transformation is introduced by the design itself.

## Open Questions

- Should backup/restore rehearsal be required in both `test` and `production` modes, or is `test` the mandatory gate with `production` treated as a topology validation path only?
- What exact smoke checks after restore are sufficient to declare the restored stack operational for first release?
- Does the current repository need an explicit rehearsal artifact schema version for `artifacts/rehearsal/*.json`, similar to verification evidence?
