## Context

The canonical evidence contract is now represented in three forms: OpenSpec requirements, human-readable documentation, and a machine-readable JSON Schema. The remaining duplication sits in `scripts/verification/validate-gate.sh`, which still hardcodes structural field/type checks inline instead of reusing the shared schema artifact.

## Goals / Non-Goals

**Goals:**
- Make validator structure checks consume `schemas/evidence-v1.json`.
- Preserve existing contextual gate checks outside the schema-backed validation step.
- Keep behavior stable while reducing duplicated structural contract logic.

**Non-Goals:**
- No change to which evidence types each gate requires.
- No change to pass/fail semantics for current evidence files.
- No change to producer-side evidence generation in this change unless integration requires small support updates.

## Decisions

### 1. Split structural validation from gate-context validation
Schema-backed validation should own required fields, nested object requirements, basic type checks, and basic string patterns. Gate-context logic should continue to own commit-SHA matching, test-type matching, pass-status enforcement, e2e artifact non-emptiness, and stats arithmetic consistency.

### 2. Keep validator execution self-contained
The validator should use runtime-available tooling already present in the repository environment or a small local helper to load and evaluate the JSON Schema, rather than introducing a heavyweight new service dependency.

### 3. Preserve explicit diagnostics
Current gate failures produce direct diagnostics. Schema-backed validation must continue surfacing actionable messages instead of collapsing all structural failures into opaque errors.

## Risks / Trade-offs

- **[Risk] Schema-backed validation may produce less precise errors than handwritten checks** → **Mitigation:** wrap schema validation results into clear gate diagnostics.
- **[Risk] Runtime dependency gaps could make validation brittle in local environments** → **Mitigation:** choose a lightweight integration path already supported by repo tooling.
- **[Risk] Partial migration could leave duplicate rules behind** → **Mitigation:** clearly separate what moves into schema-backed validation and what intentionally remains contextual.

## Migration Plan

1. Audit which existing validator checks are structural versus contextual.
2. Add schema-backed validation path to the gate validator.
3. Retain contextual checks after schema validation succeeds.
4. Extend self-tests to cover schema-consumption behavior and ensure existing pass/fail cases still hold.

## Open Questions

- Should schema validation run through a Python helper, a Node-based validator, or another repo-native path for the best balance of availability and diagnostics?
