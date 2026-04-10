## Context

The workflow domain now defines reminder-only automation semantics with strict non-goals (no decision mutation) and idempotency requirements. This change implements the smallest runtime vertical slice to operationalize those requirements.

## Goals / Non-Goals

**Goals:**
- Implement deterministic reminder candidate selection for pending workflow instances.
- Implement idempotent reminder emission keyed by instance + reminder window + reminder type.
- Implement auditable reminder records for both emitted and skipped outcomes.
- Provide a minimal read API for reminder history diagnostics.

**Non-Goals:**
- No auto-approve / auto-reject / escalation behavior.
- No external notification provider orchestration beyond persisted reminder records in this slice.
- No major workflow state-machine redesign.

## Decisions

### 1) Runner + service split
- Runner determines execution window and calls workflow reminder service.
- Service evaluates candidates, computes deterministic keys, writes emitted/skip audit rows.

### 2) Deterministic reminder key
- Use stable key dimensions: `workflow_instance_id + reminder_type + reminder_window_start`.
- Enforce uniqueness at persistence layer to guarantee replay safety.

### 3) Audit-first persistence
- Persist both outcomes:
  - `emitted`
  - `skipped` with reason code (`not_due`, `already_emitted`, `not_pending`, etc.)

### 4) Read path for diagnostics
- Expose instance-scoped reminder history endpoint/query with outcome, reason, and timestamps.

## Risks / Trade-offs

- **Risk:** over-notifying due to bad window boundaries.
  - **Mitigation:** deterministic window calculation + integration tests around boundary timestamps.
- **Risk:** duplicate reminder side effects under retries.
  - **Mitigation:** unique key + idempotent upsert semantics and replay tests.
- **Risk:** operational ambiguity when reminders are skipped.
  - **Mitigation:** mandatory skip reason codes and queryable audit history.

## Migration Plan

1. Add persistence model/migration for reminder audit records (if table absent).
2. Implement service + runner logic with deterministic keying.
3. Add query API for reminder history.
4. Add integration tests for emitted/skip/replay behavior.
5. Run verification gates and archive when evidence is green.

## Open Questions

- Should reminder scheduling cadence be fixed in app config first, or per-workflow-template in future changes?
