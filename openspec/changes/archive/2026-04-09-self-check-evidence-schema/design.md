## Context

The canonical evidence contract now spans OpenSpec requirements, human-readable docs, machine-readable schema, and executable validators. While the validator and fixtures are already tested, the schema artifact itself can still drift unless the repository explicitly checks that it parses and still validates representative examples.

## Goals / Non-Goals

**Goals:**
- Add a lightweight, repeatable self-check for `schemas/evidence-v1.json`.
- Validate that representative canonical examples or fixtures still satisfy the schema.
- Keep schema self-checks aligned with existing verification self-tests.

**Non-Goals:**
- No changes to evidence schema semantics.
- No changes to gate pass/fail policy.
- No replacement of the existing gate self-test suite.

## Decisions

### 1. Treat schema parsing and sample validation as separate checks
The self-check should confirm both that the schema is syntactically valid and that representative canonical examples still validate against it.

### 2. Reuse repository-native tooling
The self-check should rely on the same local Python/jsonschema path already used elsewhere in the repository rather than introducing a second validation stack.

### 3. Keep sample coverage small but meaningful
The self-check should validate at least one CI-style example and one archive/e2e-style example or fixture so both common evidence shapes remain protected.

## Risks / Trade-offs

- **[Risk] Example checks can become stale if they diverge from real fixtures** → **Mitigation:** choose sources already maintained in docs or verification fixtures and keep the check explicit.
- **[Risk] Extra self-check scripts may overlap with existing validator tests** → **Mitigation:** scope this change to schema integrity, not end-to-end gate behavior.

## Migration Plan

1. Identify representative evidence examples or fixtures for schema validation.
2. Add a dedicated schema self-check script.
3. Wire documentation or verification entrypoints to mention the new self-check.
4. Run the schema self-check and existing validator self-test together to confirm coherence.

## Open Questions

- Should the long-term source for schema validation examples be docs examples, verification fixtures, or a dedicated schema-fixture directory?
