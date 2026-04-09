## Context

The repository now has a robust verification stack: evidence producers, a machine-readable schema, a schema-consuming gate validator, fixture-driven self-tests, and CI-ready entrypoint wiring. However, the architecture model is not captured in one place, so responsibilities can be misunderstood when future changes land.

## Goals / Non-Goals

**Goals:**
- Document the verification system architecture in one concise, actionable reference.
- Make boundary ownership explicit (producer vs schema vs validator vs gate vs entrypoints).
- Explain how commit-scoped evidence flows from test execution to CI/archive decisions.

**Non-Goals:**
- No code or gate behavior changes.
- No schema evolution in this change.
- No workflow topology changes.

## Decisions

### 1. Single architecture document under docs/
Create one document dedicated to verification architecture so maintainers do not need to reconstruct behavior from scripts and archived change notes.

### 2. Boundary-first structure
Organize content by component responsibilities and handoff contracts rather than by file listing alone.

### 3. Explicit structural vs contextual checks
Document that schema handles structural checks while validator logic enforces contextual gate checks (commit SHA match, status gating, e2e artifact non-emptiness, etc.).

## Risks / Trade-offs

- **[Risk] Architecture doc can drift from implementation** → **Mitigation:** link directly to source scripts/spec/schema and keep the document scoped to stable boundaries.
- **[Risk] Too much detail reduces readability** → **Mitigation:** focus on flow + ownership + change guidance, not line-by-line implementation commentary.

## Migration Plan

1. Draft architecture doc with flow overview and boundary ownership.
2. Link the document from existing verification docs entrypoints.
3. Run doc-level consistency checks against current scripts/schema/spec.

## Open Questions

- Should future changes maintain an architecture changelog section inside the same document, or keep historical context in archived OpenSpec changes only?
