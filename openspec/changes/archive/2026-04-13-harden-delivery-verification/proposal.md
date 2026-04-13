## Why

The migration workspace is functionally mature, but the release-confidence gaps are now concentrated in verification realism and default quality gates rather than in missing feature breadth. CI currently relies on test execution without explicit frontend typecheck or backend static analysis, the Playwright suite mainly exercises mocked API flows rather than the live frontend-backend-database seam, and a few repository/runtime hygiene gaps still leak into daily tooling.

We need a focused hardening change now so release readiness is enforced by the default delivery path contributors actually use, and so the repository's verification story matches the maturity of the implemented business slices.

## What Changes

- Add default delivery verification coverage for frontend type safety and backend static analysis in the CI-ready path.
- Introduce at least one executable full-stack verification path that exercises the real frontend, backend, and MySQL boundary instead of API-mocked browser-only flows.
- Tighten build and runtime hygiene so repository-level issues that break tooling or hide build regressions are caught automatically.
- Keep first-release business scope unchanged; this change is about delivery confidence, not new operator-facing capabilities.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: strengthen CI-ready validation to cover typecheck/static-analysis expectations and executable build verification in addition to existing test evidence.
- `deployment-and-cutover-operations`: clarify the minimum full-stack verification required before a release candidate can be considered operationally trustworthy.

## Impact

- Affects GitHub Actions workflows, repository verification scripts, and evidence-producing CI entrypoints under `.github/workflows/` and `scripts/`.
- Affects frontend and backend validation commands by making typecheck/static-analysis/build checks part of the default hardening path.
- Affects Playwright or equivalent release verification surfaces by requiring at least one real-stack end-to-end path instead of mock-only browser verification.
- Affects repository/runtime hygiene around tracked artifacts that interfere with standard tooling, such as stale runtime filesystem entries under `deploy/`.
