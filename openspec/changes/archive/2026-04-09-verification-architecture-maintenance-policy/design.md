## Context

The repository already enforces commit-scoped evidence gates with shared schema and validator components, and now also includes a dedicated verification architecture document. The remaining gap is maintenance governance: contributors do not yet have one normative policy describing required revalidation, documentation sync expectations, and regression safeguards when verification internals change.

## Goals / Non-Goals

**Goals:**
- Define a maintenance policy for verification architecture evolution across producer/schema/validator/gate/entrypoints.
- Define impact-driven regression expectations for verification changes.
- Reduce drift risk between executable scripts and verification documentation.

**Non-Goals:**
- No change to gate pass/fail semantics.
- No schema version bump in this change.
- No expansion of business-domain scope beyond platform verification foundations.

## Decisions

### 1) Requirement-level policy in `platform-foundation`
Keep policy normative by modifying existing platform-foundation release/test gate requirements instead of adding ad hoc process notes only in docs.

### 2) Impact-check matrix as enforcement anchor
Define that each verification-layer change type (schema, producer, validator/gate, entrypoint) MUST trigger explicit revalidation steps (schema self-check, gate checks, evidence compatibility checks, docs alignment review).

### 3) Documentation sync as a first-class maintenance duty
Treat `docs/verification-architecture.md`, `docs/verification-gates.md`, and `docs/evidence-contract.md` as synchronized references; changes to verification scripts or schema require explicit consistency review.

### 4) Regression policy remains compatible with current workflow
Policy will reuse current command surface (`scripts/ci-ready.sh`, `scripts/archive-ready.sh`, existing self-tests) to avoid introducing new CI topology complexity.

## Risks / Trade-offs

- **[Risk] Policy too generic becomes non-actionable** → **Mitigation:** anchor scenarios to concrete missing/stale evidence and documentation-sync outcomes.
- **[Risk] Maintenance overhead slows minor changes** → **Mitigation:** scope checks by impact type and reuse existing self-check commands.
- **[Risk] Requirement/doc mismatch over time** → **Mitigation:** require explicit consistency review in change tasks.

## Migration Plan

1. Add OpenSpec requirement deltas under `specs/platform-foundation/spec.md` for maintenance policy behavior.
2. Add task checklist that encodes maintenance-policy implementation and verification steps.
3. Validate change completeness so it is ready for apply execution.

## Open Questions

- Should a future change introduce a dedicated `docs/verification-maintenance.md` playbook, or keep policy embedded in existing verification documents plus spec requirements?
