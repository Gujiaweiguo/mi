## Why

The repository already contains Compose manifests, deployment topology notes, backup/restore commands, and cutover runbook/checklist documents, but the deployment-and-cutover surface is not yet closed as an executable first-release slice. We now need a focused hardening change so test/production bring-up, backup/restore rehearsal, and cutover GO/NO-GO evidence become repeatable operational workflows instead of partially documented intent.

## What Changes

- Harden the Compose-based test and production operating flow so environment rendering, startup, health validation, and runtime mount assumptions are executed and verified consistently.
- Add or tighten backup and restore rehearsal behavior so operators can produce a backup bundle, restore it into the target environment, and verify the restored state with explicit pass/fail criteria.
- Add a cutover rehearsal path that turns the existing runbook and checklist into executable release-gate output with machine-readable GO/NO-GO results.
- Ensure fresh-start cutover rules remain enforced during rehearsal, including bootstrap-only initialization and explicit rejection of legacy business-record migration.
- Tighten deployment and rehearsal documentation only where needed to match the executable workflow and artifact outputs.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `deployment-and-cutover-operations`: strengthen Compose operations, backup/restore rehearsal, and cutover-go-live execution so first-release deployment readiness is validated through executable operational checks and artifacts.

## Impact

- Affects deployment scripts, Compose operational validation, and runtime directory handling under `deploy/` and `scripts/`.
- Affects cutover and go-live operational artifacts under `docs/`, especially rehearsal and rollback execution surfaces.
- Affects artifact output paths under `artifacts/rehearsal/` and any related operational evidence generated during rehearsal.
- Affects release readiness for first-release environments by making deployment and cutover gates executable rather than documentation-only.
