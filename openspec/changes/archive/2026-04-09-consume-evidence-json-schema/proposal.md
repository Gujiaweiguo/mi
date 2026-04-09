## Why

The repository now has a machine-readable evidence schema, but `validate-gate.sh` still duplicates the structural contract inline instead of consuming that shared artifact. We need this change now so structural validation has one authoritative implementation source and future contract evolution does not require parallel edits in multiple places.

## What Changes

- Refactor verification gate validation to consume the canonical `schemas/evidence-v1.json` file for structural checks.
- Preserve current gate semantics for context-dependent rules such as commit SHA matching, file-type matching, pass/fail acceptance, and e2e-specific artifact requirements.
- Clarify the runtime dependency and fallback expectations for schema-backed validation.
- Keep the change scoped to validator integration, without changing CI/archive gate topology or evidence semantics.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: update how canonical evidence structure is enforced so validator behavior reuses the shared JSON Schema artifact.

## Impact

- Affects `scripts/verification/validate-gate.sh` and any helper code needed for schema-backed validation.
- Reduces duplicated contract logic between schema definition and validator structure checks.
- Does not change first-release business scope or the meaning of CI-ready/archive-ready gates.
