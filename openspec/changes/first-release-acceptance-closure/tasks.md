## 1. Scope and gate definition

- [x] 1.1 Audit the current canonical first-release scope and identify which existing specs and frozen report boundaries must participate in final acceptance closure.
- [x] 1.2 Define the current-commit release-readiness decision model, including required evidence inputs, blocker classes, and GO/NO-GO semantics.
- [x] 1.3 Update the relevant operational spec and supporting docs so archive, rehearsal, and final acceptance gates are clearly distinguished.

## 2. Readiness artifact and workflow

- [x] 2.1 Implement or update the workflow that produces a machine-readable final readiness summary for the current commit.
- [x] 2.2 Ensure the readiness summary validates current-commit SHA matching and fails on missing, stale, malformed, or failed evidence.
- [x] 2.3 Record deferred findings separately from must-fix blockers so out-of-scope or post-release items remain visible without widening first-release scope.

## 3. Verification and closure

- [x] 3.1 Run the bounded first-release verification set required for the evaluated commit and regenerate any missing commit-scoped evidence files.
- [x] 3.2 Produce machine-readable current-commit evidence under `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json` for CI readiness.
- [x] 3.3 Produce machine-readable current-commit evidence under `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json` for archive and final acceptance readiness.
- [x] 3.4 Generate the final current-commit release-readiness summary and confirm it reports the correct GO/NO-GO outcome for the bounded first-release scope.
