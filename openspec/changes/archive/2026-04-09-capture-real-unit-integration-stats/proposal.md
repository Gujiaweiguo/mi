## Why

`run-unit.sh` and `run-integration.sh` still emit hard-coded stats in their evidence payloads even though the underlying test commands run real suites. We need this change now so unit and integration evidence reflects actual test counts instead of script-level placeholders, bringing their trust level closer to the e2e path.

## What Changes

- Update unit and integration evidence generation to derive `total/passed/failed/skipped` from real test results.
- Define acceptable result-capture approaches for Go and frontend unit test commands used in current verification scripts.
- Preserve current gate semantics while improving evidence fidelity for commit-scoped CI/archive checks.
- Keep the scope limited to evidence production and verification support, without changing business functionality.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: strengthen verification evidence fidelity so unit and integration evidence is based on real result counts rather than fixed placeholders.

## Impact

- Affects `scripts/verification/run-unit.sh`, `scripts/verification/run-integration.sh`, and any helper logic used to parse test output.
- Improves confidence in unit/integration evidence used by CI-ready and archive-ready gates.
- Does not alter the required gate topology or evidence schema.
