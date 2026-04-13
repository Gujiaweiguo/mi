## 1. Trigger policy definition

- [x] 1.1 Audit the current `ci-ready` workflow trigger matrix and identify which `push` and `pull_request` combinations currently produce duplicate runs for the same PR update.
- [x] 1.2 Decide and document the intended event coverage so PR updates map to one authoritative `ci-ready` run while non-PR branch validation remains intentional rather than accidental.

## 2. Workflow implementation

- [x] 2.1 Update `.github/workflows/ci-ready.yml` so a single PR update no longer creates duplicate `ci-ready` runs with the same validation purpose.
- [x] 2.2 Preserve stable job/check names and current commit-SHA propagation so `unit` and `integration` evidence still resolve under `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json`.
- [x] 2.3 Verify that trigger deduplication changes only workflow invocation policy and does not change prerequisite validation, evidence production, or CI gate semantics.

## 3. Verification and documentation

- [ ] 3.1 Validate the updated workflow behavior for at least one PR-triggered update and one intended non-PR trigger path, confirming duplicate `ci-ready` runs no longer appear for the PR case.
- [x] 3.2 Update contributor-facing CI documentation to explain which event path is authoritative for PR validation and how direct pushes are handled after deduplication.
- [x] 3.3 Confirm the resulting workflow still enforces current-commit `unit` and `integration` evidence for CI-ready, and that archive requirements remain distinct (`unit`, `integration`, and `e2e` under `artifacts/verification/<commit-sha>/`).
