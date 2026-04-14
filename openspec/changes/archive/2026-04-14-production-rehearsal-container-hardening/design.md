## Context

The repository already enforces commit-scoped CI and archive evidence gates and already defines a cutover rehearsal workflow, but current release confidence is still limited by production-topology rehearsal reliability and runtime hygiene consistency.

Observed constraints and pain points driving this design:

- Go-live policy requires a successful production-topology rehearsal and machine-readable GO/NO-GO evidence for the current commit.
- Production runtime assumptions currently allow drift between intended hardening and what is actually exercised during rehearsal (for example permissive container user/mount behavior or contaminated runtime data paths).
- Rehearsal quality depends on fast-fail validation before destructive operations begin; weak preflight checks create noisy late failures and ambiguous NO-GO outcomes.
- The change must preserve the project’s fresh-start cutover rule and must not reintroduce any legacy transactional migration path.

Stakeholders:

- Release owners deciding GO/NO-GO for cutover.
- Developers and operators running preflight, smoke, backup, restore, and rehearsal scripts.
- Reviewers relying on machine-readable verification/rehearsal artifacts as release evidence.

## Goals / Non-Goals

**Goals:**

- Establish a trustworthy production-topology rehearsal baseline that can produce current-commit GO outcomes under documented runtime assumptions.
- Harden runtime/container assumptions so rehearsal validates realistic production behavior rather than passing because of permissive local state.
- Strengthen fast-fail operational validation (preflight/smoke/rehearsal guards) to block unsafe or misleading rehearsal attempts early.
- Keep evidence and operator documentation synchronized with the hardened workflow so release decisions remain auditable.

**Non-Goals:**

- Redesigning business workflows or domain behavior (lease/billing/report logic is out of scope).
- Introducing microservices or changing the modular-monolith architecture.
- Expanding first-release scope beyond current frozen boundaries.
- Replacing existing CI/archive evidence contracts with a new schema version.

## Decisions

### Decision 1: Treat production-topology rehearsal as the primary release-confidence path

The hardened path will explicitly prioritize production Compose topology rehearsal as the authoritative operational proof point for GO/NO-GO.

Why:

- Go-live requirements already define production-topology rehearsal as mandatory.
- Test-only or mock-friendly rehearsal success is not enough to prove runtime readiness.

Alternatives considered:

- Keep relying on test-environment rehearsal artifacts: rejected because topology mismatch can hide production-only failures.
- Add only documentation reminders without hard checks: rejected because manual adherence is too fragile.

### Decision 2: Enforce stricter runtime hygiene and writable-mount assumptions before rehearsal proceeds

Preflight validation will be hardened so runtime contamination and unsafe mount/user assumptions fail before bootstrap/smoke/backup/restore steps.

Why:

- Early rejection is cheaper and clearer than late-stage partial rehearsal failures.
- Runtime contamination directly reduces trust in resulting GO/NO-GO artifacts.

Alternatives considered:

- Keep current permissive checks and rely on post-hoc artifact review: rejected due to weak guardrails and inconsistent operator outcomes.

### Decision 3: Align container runtime behavior with documented hardening expectations

Compose/Docker/runtime changes will be treated as part of the cutover contract when they affect mount permissions, writeability, and realistic execution conditions.

Why:

- If container runtime behavior differs from intended hardening, rehearsal confidence is overstated.
- Runtime correctness (user/mount/path behavior) is a foundational dependency for reliable rehearsal and smoke checks.

Alternatives considered:

- Defer container/runtime fixes to a later follow-up change: rejected because it would keep the current rehearsal baseline untrustworthy.

### Decision 4: Keep verification and rehearsal evidence machine-readable and commit-scoped

No relaxation of existing evidence policy: the hardened workflow continues to require current-commit evidence alignment and produces machine-readable results consumable by release checks.

Why:

- Commit-scoped evidence is central to reproducible release decisions.
- Consistency with existing gate contracts avoids introducing review ambiguity.

Alternatives considered:

- Allow ad hoc/manual evidence exceptions for rehearsal: rejected because it weakens auditability.

## Risks / Trade-offs

- **[Risk] Increased strictness can temporarily increase NO-GO outcomes** → **Mitigation:** make failures explicit, actionable, and localized to clear preflight/smoke checks.
- **[Risk] Runtime hardening changes may expose environment-specific permission issues** → **Mitigation:** codify expected runtime paths/ownership and validate these assumptions before destructive steps.
- **[Risk] Operational scripts become more complex** → **Mitigation:** keep changes modular, preserve existing command surface where possible, and update docs alongside script behavior.
- **[Risk] Longer rehearsal execution during stabilization period** → **Mitigation:** prefer deterministic checks and fail-fast sequencing to reduce wasted time on doomed runs.

## Migration Plan

1. Update deployment/runtime assumptions and validation scripts in a way that preserves existing command entrypoints.
2. Tighten preflight and rehearsal guardrails for runtime hygiene and production-topology readiness.
3. Execute production-topology rehearsal and capture machine-readable GO/NO-GO artifacts for the current commit.
4. Update operational documentation/checklists to reflect hardened behavior and required evidence.
5. If failures occur, keep NO-GO as authoritative, fix root causes, and rerun until a trustworthy GO baseline is produced.

Rollback approach:

- Revert hardening changes as a single change-set if they cause unrecoverable operational regression.
- Preserve artifact history from failed runs to diagnose and iterate rather than masking failures.

## Open Questions

- Should production rehearsal become an automated scheduled/triggered workflow after this hardening pass, or remain manual-but-strict for first release?
- Which runtime directories should be explicitly blocked from repository-tracked data to prevent contamination by default?
- Do we require additional non-root runtime assertions in smoke/preflight beyond current health and writability checks?
