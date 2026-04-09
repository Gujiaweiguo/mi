## Why

The repository now has a standalone schema self-check, but CI entrypoints do not invoke it automatically. We need this change now so schema drift is caught in the same default path contributors use to judge CI readiness, instead of relying on manual memory.

## What Changes

- Wire the schema self-check into the repository CI-ready execution path.
- Clarify the execution order between schema self-checks and evidence gate validation.
- Keep CI/archive gate semantics unchanged while strengthening default preflight coverage.
- Limit the scope to repository entrypoints and related documentation, without changing evidence rules.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: require the machine-readable evidence schema self-check to run as part of the default CI-ready path.

## Impact

- Affects `scripts/ci-ready.sh` and related verification documentation.
- Makes schema integrity part of routine CI-ready validation.
- Does not change business scope, evidence schema semantics, or archive-gate requirements.
