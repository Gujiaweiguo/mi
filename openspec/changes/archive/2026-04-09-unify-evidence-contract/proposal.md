## Why

Evidence files under `artifacts/verification/<commit-sha>/` have drifted in structure across different changes, which weakens gate trust and makes archive-readiness decisions inconsistent. We need one canonical evidence contract now so CI and archive gates evaluate machine-readable results with the same schema and semantics.

## What Changes

- Define a canonical evidence schema shared by `unit`, `integration`, and `e2e` evidence, including required fields, value constraints, and timestamp/stat consistency rules.
- Clarify strict compatibility expectations so stale or schema-drift evidence cannot satisfy CI/archive gates.
- Standardize producer behavior requirements for evidence-writing scripts and workflow entrypoints to emit the canonical schema for current commit SHA.
- Keep this change in verification-contract scope only, without introducing new business capabilities.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: tighten requirement-level evidence contract consistency and canonical schema enforcement for commit-scoped CI/archive gates.

## Impact

- Affects verification scripts and evidence producers that write `unit.json`, `integration.json`, and `e2e.json`.
- Affects gate determinism by removing schema ambiguity between CI-ready and archive-ready decisions.
- Does not change first-release business scope, non-membership boundaries, or cutover policy.
