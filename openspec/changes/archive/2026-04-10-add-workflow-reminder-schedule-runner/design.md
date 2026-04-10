## Context

The workflow module already provides reminder evaluation (`RunReminders`) and both manual trigger and history query endpoints. The missing piece is autonomous invocation. The current backend has no scheduler framework; startup wiring lives in `internal/app/app.go`, and configuration is loaded from YAML/Viper in `internal/config`.

## Goals / Non-Goals

**Goals:**
- Add a configurable periodic scheduler in backend runtime to invoke existing reminder evaluation.
- Keep scheduler logic thin: configuration parsing, interval loop, non-overlap guard, service call, and summary logging.
- Preserve existing reminder semantics (idempotency, no workflow decision mutation).

**Non-Goals:**
- No changes to reminder business logic, reminder key format, or reminder audit persistence.
- No new external scheduler dependency (cron library, queue, or distributed lock).
- No UI or permission model changes.

## Decisions

1. **Use in-process ticker in `app.Run`**
   - **Why**: minimal change and no new infra dependency; fits current app startup architecture.
   - **Alternative considered**: external cron or dedicated worker process. Rejected for this slice because it needs additional deployment wiring and split-runtime management.

2. **Add explicit config section `workflow_reminder_scheduler`**
   - Fields: `enabled`, `interval_seconds`, `reminder_type`, `min_pending_age_seconds`, `window_truncation_seconds`.
   - **Why**: keeps scheduler behavior environment-configurable and operationally transparent.

3. **Non-overlap guard with in-process mutex/flag**
   - **Why**: protects against long runs spilling into next tick.
   - **Alternative considered**: skip guard due to current runtime speed. Rejected to avoid accidental overlap under load.

4. **Failure policy = log and continue**
   - **Why**: scheduler should be resilient; transient failures should not disable future runs.

## Risks / Trade-offs

- **[Single-process scheduler]** In multi-replica deployment, each replica may run the scheduler. → **Mitigation**: keep reminder side effects idempotent (already true), document this boundary for future distributed locking change.
- **[Misconfigured interval]** Very small interval may increase DB load. → **Mitigation**: enforce minimum positive interval default and validation.
- **[Startup coupling]** Scheduler runs inside API process. → **Mitigation**: keep it opt-in via `enabled` flag.

## Migration Plan

1. Add new config struct and YAML keys with `enabled: false` default.
2. Wire scheduler in app startup; no behavior change while disabled.
3. Enable in selected environment via config/env override.
4. Rollback by setting `enabled: false` (no schema rollback required).

## Open Questions

- Do we need per-environment guardrails for minimum interval in production stricter than development?
- Future: should scheduler responsibility move to a dedicated worker role once horizontal scaling is introduced?
