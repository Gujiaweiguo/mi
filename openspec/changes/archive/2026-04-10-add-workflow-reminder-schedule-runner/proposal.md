## Why

Manual reminder triggering is now available, but production operations still depend on humans invoking it. We need a deterministic scheduled trigger so reminder evaluation runs continuously without introducing approval-state mutation or changing reminder business rules.

## What Changes

- Add backend scheduler configuration for workflow reminder runs (enable flag, interval, reminder config payload).
- Add in-process periodic runner wiring in backend app startup that calls existing `workflow.Service.RunReminders`.
- Add execution logging and non-overlap guard so one run does not overlap the next interval window.
- Add tests for configuration loading/default behavior and scheduler wiring behavior.
- No changes to reminder evaluation rules, reminder audit schema, or reminder HTTP endpoint contracts.

## Capabilities

### New Capabilities
- _None._

### Modified Capabilities
- `workflow-approvals`: extend reminder-only automation requirements to support scheduled execution of the existing reminder runner with replay-safe behavior and no decision-state mutation.

## Impact

- **Affected code**: `backend/internal/config/*`, `backend/internal/app/*`, and supporting tests.
- **Runtime behavior**: when enabled, backend periodically invokes existing reminder service logic.
- **Operations**: removes manual-only dependency for reminder execution while preserving current manual trigger endpoint.
- **Risk**: low-to-medium; scheduler misconfiguration could create too-frequent runs, mitigated by defaults, validation, and idempotent reminder logic.
