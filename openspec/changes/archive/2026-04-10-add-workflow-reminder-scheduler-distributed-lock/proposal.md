## Why

The reminder scheduler currently runs in-process without cross-instance coordination. In multi-replica deployment this can cause concurrent runs from multiple pods, so we need a database-backed distributed lock to enforce single active scheduler ownership.

## What Changes

- Add distributed lock acquisition/release around scheduled reminder execution using MySQL lock primitives.
- Keep current reminder business logic unchanged; only protect scheduler ownership.
- Add failure-safe lock release and timeout behavior so crashed/stuck runs do not permanently block future runs.
- Add tests covering lock contention and lock-loss behavior for scheduler runs.

## Capabilities

### New Capabilities
- _None._

### Modified Capabilities
- `workflow-approvals`: extend scheduled reminder automation requirement with single-run distributed ownership semantics across replicas.

## Impact

- **Affected code**: `backend/internal/app/*` scheduler wiring, possible workflow repository/helper for lock calls, and related tests.
- **Infrastructure**: depends on MySQL lock behavior (no new external dependency).
- **Operations**: reduces duplicate scheduled reminder executions in horizontally scaled environments.
