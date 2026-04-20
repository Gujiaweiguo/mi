## Why

The migration repository now appears feature-complete for first-release scope, but the release decision is still exposed to three risks:

1. **Current-HEAD evidence drift** — the repository contains release-ready summaries tied to older validated commits, so the exact commit intended for release may not yet have a current verification and rehearsal trail.
2. **Scope-to-acceptance ambiguity** — preserved and redesigned business capabilities, mandatory outputs, and R19 visual acceptance are implemented across the codebase, but there is not yet a single current-head audit artifact that proves all of them remain closed for go-live.
3. **Operational closure gaps** — production secret guidance, bootstrap expectations, and stale repository placeholders can still mislead operators or contributors even when the underlying implementation is present.

This change turns the project from “apparently complete” into “release-decision complete” by requiring current-head verification, production rehearsal, explicit capability/output audit artifacts, and cleanup of stale readiness signals.

## What Changes

### Current-HEAD release validation
- Re-validate archive readiness on the exact current HEAD instead of relying on historical verified commits.
- Re-run the supported production cutover rehearsal for the exact current HEAD and capture GO/NO-GO artifacts.
- Refresh release-ready status documentation to point only at current evidence.

### Scope-to-acceptance closure
- Produce a capability-to-implementation matrix for all `PRESERVE` and `REDESIGN` areas.
- Produce a mandatory-output evidence matrix for `P01-P11`, `T01-T03`, `E01-E05`, and `R01-R19`.
- Verify `R19` visual acceptance on the UI layer rather than inferring it only from backend tests.

### Go-live hygiene and decision packet
- Harden production secret/bootstrap guidance so placeholder values are never treated as go-live safe.
- Remove stale repository placeholder guidance that suggests core migration modules are still pending.
- Publish a binary release-decision packet with explicit `GO` / `NO-GO` outcome.

## Capabilities

### Modified Capabilities
- `platform-foundation`: current-head verification evidence, release summaries, and release-decision artifacts must remain commit-scoped and current.
- `deployment-and-cutover-operations`: the supported rehearsal path must be used to prove go-live readiness for the exact release commit, with production secret/bootstrap guidance treated as blocking.
- `supporting-domain-management`: frozen first-release outputs and R19 visual acceptance must be backed by explicit current-head evidence, not assumed from implementation presence.

## Impact

- Affects current-head verification and release documentation under `docs/`, `artifacts/verification/`, and `artifacts/rehearsal/`.
- Affects deployment/go-live operator documentation and production readiness guidance.
- May introduce small targeted fixes only where audits reveal a real first-release blocker.
- Does **not** expand first-release scope beyond the frozen non-membership boundary.
