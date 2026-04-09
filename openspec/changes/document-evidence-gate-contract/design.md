## Context

The canonical evidence contract now lives across the platform foundation spec and executable verification scripts, but contributor-facing docs still reference a removed legacy path. This change adds a durable documentation bridge so humans can find the contract quickly without weakening the rule that executable validation remains authoritative.

## Goals / Non-Goals

**Goals:**
- Replace stale documentation references with current, reachable sources.
- Add a standalone contract document summarizing canonical evidence JSON structure, invariants, and gate semantics.
- Keep docs aligned with `platform-foundation` requirements and verification scripts.

**Non-Goals:**
- No changes to validation logic, evidence writers, or gate scripts.
- No new schema version or contract semantics.
- No archive/CI workflow topology changes.

## Decisions

### 1. Keep source-of-truth layered
OpenSpec requirements remain the normative behavior contract, and verification scripts remain the executable enforcement layer. The new standalone document is a contributor-facing synthesis, not a competing authority.

### 2. Update existing gate doc instead of replacing it
`docs/verification-gates.md` already acts as the operator entrypoint. This change should repair its references and point to the new standalone contract document plus the platform spec.

### 3. Document the contract in concrete JSON terms
The standalone document should include required fields, nested structures, invariants, and the CI vs archive distinction so users do not need to inspect inline Python to understand the format.

## Risks / Trade-offs

- **[Risk] Docs can drift from implementation over time** → **Mitigation:** explicitly point readers to both the platform spec and verification scripts as the enforcement sources.
- **[Risk] Duplicate documentation may imply a second source of truth** → **Mitigation:** describe the standalone contract doc as a human-readable summary of the enforced contract.

## Migration Plan

1. Add proposal, design, and spec delta for documentation clarity around evidence-gate requirements.
2. Update `docs/verification-gates.md` to remove dead references and link to current sources.
3. Add a standalone evidence contract document with canonical field/invariant summary and examples.
4. Verify links and command references remain accurate.

## Open Questions

- Should the standalone contract document eventually be backed by a machine-readable JSON Schema file, or remain prose-plus-example documentation until schema tooling is needed?
