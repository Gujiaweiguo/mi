## Context

The repository already has two useful contract layers for verification evidence: prose documentation in `docs/evidence-contract.md` and executable enforcement in `scripts/verification/validate-gate.sh`. What is still missing is a machine-readable schema artifact that can be referenced by tooling, tests, and future validator refactors without copying field definitions again.

## Goals / Non-Goals

**Goals:**
- Introduce a canonical JSON Schema for verification evidence files.
- Keep the schema aligned with the current documented and enforced contract.
- Make room for future validator consumption of the schema without requiring that refactor in this change.

**Non-Goals:**
- No change to CI/archive gate pass criteria.
- No replacement of the current validator implementation in this change.
- No expansion of evidence semantics beyond the currently documented contract.

## Decisions

### 1. Use one schema artifact for shared structure
The repository should add a single schema file such as `schemas/evidence-v1.json` to express the canonical evidence structure, required fields, nested object requirements, and basic type constraints.

### 2. Keep semantic invariants explicitly documented
Some invariants are better enforced procedurally even if they are partially representable in schema form, such as `commit_sha` matching the evaluated commit or `status == "passed"` for gate acceptance. The schema should define structure and local constraints, while the validator remains responsible for gate-context checks.

### 3. Align prose, schema, and executable enforcement
The standalone contract document, OpenSpec requirements, and validator implementation should all point to the schema artifact as the shared structural contract.

## Risks / Trade-offs

- **[Risk] Schema and validator drift apart later** → **Mitigation:** document the intended layering clearly and add follow-up validation adoption work.
- **[Risk] Contributors assume schema alone is sufficient for gate acceptance** → **Mitigation:** explicitly distinguish structural validation from gate-context checks like commit SHA and pass status.

## Migration Plan

1. Define OpenSpec requirements for machine-readable evidence schema coverage.
2. Add the schema artifact and document its relationship to existing docs and validator logic.
3. Update contributor-facing docs to point to the schema artifact.
4. Follow up later with validator/schema integration if desired.

## Open Questions

- Should the eventual validator integration consume JSON Schema directly at runtime, or should the schema remain a documentation/test artifact first?
