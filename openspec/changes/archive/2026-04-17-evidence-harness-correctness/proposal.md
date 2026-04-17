## Why

The repository now relies on commit-scoped machine-readable evidence to decide CI-ready and archive-ready status, but the `unit` and `integration` evidence producers still hardcode success semantics and can exit before writing failed evidence. We need to correct that now so verification gates can distinguish failed test execution from missing evidence and keep release-confidence signals trustworthy.

## What Changes

- Correct the `unit` and `integration` evidence producers so they always emit evidence for the evaluated commit, including failed runs.
- Align `unit` and `integration` producer behavior with the existing `e2e` pattern by capturing command exit status, writing accurate `status` values, and then returning the original failing exit code.
- Tighten verification self-check coverage so failed evidence production is validated explicitly instead of only the missing-evidence path.
- Keep this change scoped to verification harness correctness; it does not add new business capability or widen first-release product scope.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: strengthen commit-scoped verification producer requirements so `unit` and `integration` evidence reflect real pass/fail outcomes instead of only successful runs or missing artifacts.

## Impact

- Affects verification scripts under `scripts/verification/`, especially `run-unit.sh`, `run-integration.sh`, and related self-check coverage.
- Affects CI-ready and archive-ready gate diagnostics by making failed verification runs produce explicit failed evidence for the current commit.
- Does not change operator-facing business workflows, deployment topology, or report/output scope.
