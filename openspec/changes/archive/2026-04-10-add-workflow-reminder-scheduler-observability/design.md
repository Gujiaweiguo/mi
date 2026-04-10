## Context

The scheduler currently logs basic events but does not maintain structured cumulative counters or failure trend state. Operators must infer health from sparse logs. We need deterministic per-tick telemetry and trend signals without changing reminder evaluation semantics.

## Goals / Non-Goals

**Goals**
- Track scheduler run outcomes in-memory with consistent counters.
- Emit structured logs for success/failure/lock-skip including duration and cumulative totals.
- Signal potentially degraded state via warning log when consecutive failures reach threshold.

**Non-Goals**
- No external metrics backend integration in this slice.
- No UI/dashboard endpoint in this slice.
- No changes to reminder business rules or distributed lock semantics.

## Design

1. Introduce internal scheduler observability state in `app.go`:
   - totals: `total_runs`, `successful_runs`, `failed_runs`, `lock_skipped_runs`
   - trend: `consecutive_failures`
   - timestamps: `last_run_at`, `last_success_at`, `last_failure_at`, `last_lock_skip_at`
   - duration: `last_run_duration_ms`

2. Update scheduler tick flow:
   - start timer at tick begin
   - classify outcome into one of: success / failure / lock-skip
   - update observability state
   - emit structured log payload with both per-run and cumulative fields

3. Failure trend warning:
   - add constant threshold (`3`) for warning-level signal
   - when `consecutive_failures >= threshold`, emit `Warnw` with context

4. Tests:
   - validate state transitions reset/increment behavior
   - validate lock-skip does not count as failure
   - validate success clears consecutive failures

## Trade-offs

- In-memory counters reset on process restart; acceptable for this slice because objective is runtime diagnostics, not historical persistence.
- Logging-based observability is lightweight and immediate, while external metrics integration remains future work.
