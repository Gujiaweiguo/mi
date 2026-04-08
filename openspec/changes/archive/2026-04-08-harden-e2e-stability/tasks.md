## 1. E2E contract hardening

- [x] 1.1 Audit current Playwright E2E execution paths and identify instability sources that can invalidate archive evidence reproducibility.
- [x] 1.2 Tighten E2E verification entry flow so supported first-release non-membership scenarios run deterministically from documented bootstrap assumptions.
- [x] 1.3 Ensure required E2E evidence fields for archive evaluation are consistently emitted and validated as commit-scoped output.

## 2. Evidence gate enforcement alignment

- [x] 2.1 Harden archive-gate validation so missing, stale, malformed, or failed `e2e` evidence is rejected with explicit diagnostics.
- [x] 2.2 Verify CI/archive gate separation remains intact (`unit`+`integration` for CI; `unit`+`integration`+`e2e` for archive) while preserving commit-SHA matching rules.

## 3. Verification and evidence regeneration

- [x] 3.1 Add or tighten automated checks covering E2E reproducibility assumptions and archive-gate evidence validity behavior.
- [x] 3.2 Run verification suite for the implementation commit and record machine-readable evidence in `artifacts/verification/<commit-sha>/unit.json`, `artifacts/verification/<commit-sha>/integration.json`, and `artifacts/verification/<commit-sha>/e2e.json`.
- [x] 3.3 Confirm the implementation commit satisfies change gate expectations: CI requires passing `unit` and `integration`; archive requires passing `unit`, `integration`, and `e2e` for the same commit SHA.
