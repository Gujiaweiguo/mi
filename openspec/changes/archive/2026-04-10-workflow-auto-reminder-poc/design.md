## Context

The current workflow model guarantees valid transitions, idempotent side-effect protection, and explicit user-driven approval actions. A reminder-only POC should strengthen operator awareness for pending items while preserving those guarantees and avoiding timeout/escalation automation.

## Goals / Non-Goals

**Goals:**
- Define a minimal reminder automation contract for pending workflow instances.
- Define idempotency and audit requirements for reminder emissions.
- Keep reminder behavior decoupled from approval state mutation.

**Non-Goals:**
- No auto-approve, auto-reject, or escalation decision logic.
- No SLA redefinition for first release.
- No mandatory external notification provider integration in this planning artifact.

## Decisions

### 1) Reminder-only behavior boundary
Reminder automation may emit reminder records/events for pending instances but must not mutate workflow decision state.

### 2) Idempotent replay requirement
Repeated reminder job runs over the same eligible window must avoid duplicate reminder side effects for the same reminder key.

### 3) Audit-first observability
Reminder emissions and skip decisions should be auditable so operators can trace reminder history and suppression causes.

## Risks / Trade-offs

- **Risk:** reminder scope drifts into escalation automation.
  - **Mitigation:** hard non-goal language and scenario checks in spec.
- **Risk:** duplicate reminders on retries/noisy scheduling.
  - **Mitigation:** require deterministic reminder key and idempotent write path.

## Migration Plan

1. Add workflow-approvals spec delta for reminder-only automation boundary.
2. Add tasks for idempotency/audit requirements and verification scenarios.
3. Keep change apply-ready before any runtime implementation.

## Open Questions

- Should first implementation persist reminders in workflow tables or in a dedicated reminder log table?
