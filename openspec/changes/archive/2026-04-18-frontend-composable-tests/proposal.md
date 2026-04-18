## Why

The frontend now relies on several shared composables and lightweight API helpers, but the newest shared utilities still have little or no direct test coverage. Adding focused unit tests now protects recently extracted frontend infrastructure before more views depend on it.

## What Changes

- Add unit tests for shared frontend composables that are reused across multiple views.
- Add focused tests for download and error-message helpers to lock in expected behavior.
- Add tests for dashboard summary aggregation logic so the new workbench dashboard can evolve safely.

## Capabilities

### New Capabilities
- `frontend-composable-tests`: unit-test coverage for shared frontend composables and lightweight dashboard aggregation helpers

### Modified Capabilities
- `platform-foundation`: frontend shared utilities gain executable verification through unit tests

## Impact

- Affected frontend test files under `frontend/src/composables/` and `frontend/src/api/`.
- No production behavior changes or backend API changes.
- Strengthens confidence in shared client-side infrastructure used by multiple views.
