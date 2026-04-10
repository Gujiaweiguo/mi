## Why

We already defined reminder-only workflow behavior at the spec level. The next step is a minimal implementation slice so teams can validate real idempotent reminder execution and audit traceability in runtime conditions.

## What Changes

- Implement a reminder runner that scans pending workflow instances and emits reminder records using deterministic reminder keys.
- Ensure replay-safe execution (same reminder key/window does not duplicate side effects).
- Persist reminder audit traces for both emitted and skipped evaluations with reason codes.
- Expose a minimal query path for reminder history per workflow instance for operator diagnostics.

## Capabilities

### New Capabilities

<!-- None. -->

### Modified Capabilities

- `workflow-approvals`: implement reminder-only automation behavior, idempotent replay handling, and auditable skip/emission records.

## Impact

- Backend workflow module (runner/service/repository) for reminder evaluation and persistence.
- Database schema/migration for reminder audit log if missing.
- API/query path for reminder history visibility.
- Integration tests and verification evidence for replay safety and audit trace requirements.
