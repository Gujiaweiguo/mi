## Why

The repository now covers most first-release non-membership capabilities, but there is not yet a single canonical change that defines how to decide whether a specific commit is actually release-ready across the bounded scope. We need an explicit acceptance-closure contract now so final go/no-go decisions, must-fix blockers, and evidence expectations stop living in scattered notes and recent archive history.

## What Changes

- Define the first-release acceptance closure rules for the current canonical scope, including what must be verified before a commit can be treated as release-ready.
- Establish a commit-scoped readiness decision model that distinguishes must-fix blockers from known deferred items that remain outside the first-release boundary.
- Require machine-readable evidence and summary outputs for release acceptance so the final decision is reproducible and auditable.
- Keep membership / `Associator`, legacy transaction migration, extra reports outside `R01-R19`, email delivery, and device-side tax integration out of scope.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `deployment-and-cutover-operations`: add explicit first-release acceptance closure, current-commit readiness evidence, blocker classification, and machine-readable release decision requirements.

## Impact

- Affects release-readiness workflows, verification artifacts, and the operational criteria used before archive, rehearsal, and go-live decisions.
- Likely affects verification scripts, evidence generation, and any operational documentation or automation that determines GO/NO-GO state for a commit.
- Does not widen functional business scope beyond the existing first-release non-membership boundary.
