## Context

Current workflow approvals intentionally rely on explicit user-driven actions and idempotent side-effect protection. Timeout/escalation is out of first-release scope, but engineering needs a stable boundary definition to avoid accidental scope creep and to guide future design when automation is introduced.

## Goals / Non-Goals

**Goals:**
- Define a normative boundary for timeout/escalation in the workflow domain.
- Define readiness criteria for future implementation (data model, scheduler behavior, idempotency, observability).
- Keep first-release behavior unchanged and explicitly protected.

**Non-Goals:**
- No implementation of timeout/escalation automation in this change.
- No new scheduler/worker process introduction.
- No modification to current approval outcomes or SLA semantics.

## Decisions

### 1) Boundary-first spec update in existing workflow capability
Use `workflow-approvals` as the single capability for this planning boundary instead of introducing a new capability.

### 2) Readiness contract before implementation
Future timeout/escalation implementation must satisfy explicit preconditions:
- deterministic trigger source and replay handling
- idempotent escalation side effects
- auditability for auto actions
- clear operator override and recovery paths

### 3) First-release exclusion remains enforceable
The spec must continue to assert that first release is complete without timeout/escalation automation.

## Risks / Trade-offs

- **Risk:** ambiguous wording could be interpreted as implementation scope now.
  - **Mitigation:** use SHALL/SHALL NOT language that distinguishes boundary research from runtime features.
- **Risk:** future implementation may bypass idempotency/audit requirements.
  - **Mitigation:** make readiness criteria normative and scenario-tested in future change.

## Migration Plan

1. Add delta requirements to `workflow-approvals` spec for boundary/readiness criteria.
2. Add tasks that require documentation traceability and readiness checklist completion.
3. Keep change apply-ready without runtime code modifications.

## Open Questions

- Should timeout/escalation readiness be implemented via in-process scheduler first, or directly as a dedicated background worker in a later release phase?
