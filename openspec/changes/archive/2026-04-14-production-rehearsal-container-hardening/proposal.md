## Why

The repository's go-live contract requires a successful production-topology cutover rehearsal for the current commit, but the recorded rehearsal history has not established a trustworthy passing baseline for the supported production runtime. At the same time, the production container/runtime surface still contains hygiene gaps that can invalidate rehearsal results or hide deployment risk until late.

## What Changes

- Harden the supported production-topology rehearsal path so the repository can produce a real current-commit GO rehearsal against the documented production Compose stack.
- Tighten runtime and container assumptions around production mounts, writable paths, and runtime data hygiene so rehearsal and smoke validation match intended production constraints instead of relying on permissive or contaminated local state.
- Add or refine validation guardrails that fail fast when the production runtime layout, tracked runtime data, or container user/mount assumptions would make the rehearsal misleading or unsafe.
- Update the operational documentation and acceptance surfaces needed to treat production rehearsal output as the authoritative release-confidence signal for go-live readiness.

## Capabilities

### New Capabilities
- `production-rehearsal-hardening`: Production-topology rehearsal hardening, runtime hygiene checks, and operational validation needed to establish a trustworthy GO/NO-GO result for the supported deployment path.

### Modified Capabilities
- `deployment-and-cutover-operations`: Tighten the deployment and cutover requirements so production-topology rehearsal, clean runtime assumptions, and fast-fail operational validation are explicit parts of the supported cutover model.
- `platform-foundation`: Tighten runtime foundation requirements where production container user, mount, and runtime-directory behavior must be validated as part of the supported platform assumptions.

## Impact

- Deployment assets under `deploy/compose/`, `deploy/docker/`, `deploy/runtime/`, and related environment/config defaults.
- Operational scripts and validation paths such as `scripts/cutover-rehearsal.sh`, `scripts/compose-preflight.sh`, `scripts/compose-smoke-test.sh`, and related verification helpers.
- Go-live and deployment documentation, plus any machine-readable rehearsal or verification artifacts used to prove production readiness for the current commit.
