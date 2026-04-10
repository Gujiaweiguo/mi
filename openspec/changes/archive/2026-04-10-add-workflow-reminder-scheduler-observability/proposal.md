## Why

Workflow reminder scheduling now supports distributed locking, but operators still lack clear runtime visibility into scheduler health and trend behavior. We need first-class observability for run outcomes, timing, lock-skip frequency, and consecutive failures to make production diagnosis and alerting reliable.

## What Changes

- Add scheduler execution telemetry in backend runtime for reminder scheduling (run counts, success/failure/skip counts, durations, last run timestamps).
- Emit structured logs for each scheduler tick with outcome details and cumulative counters.
- Add warning-level signal when consecutive scheduler failures exceed a defined threshold.
- Add unit tests for scheduler observability state transitions and outcome accounting.

## Capabilities

### New Capabilities
- _None._

### Modified Capabilities
- `workflow-approvals`: extend scheduled reminder behavior with explicit observability guarantees for outcome logging and failure trend signaling.

## Impact

- **Affected code**: `backend/internal/app/app.go`, `backend/internal/app/app_test.go`.
- **Operations**: improved troubleshooting and alerting hooks from structured scheduler telemetry.
- **Risk**: low; no change to reminder decision behavior or workflow business logic.
