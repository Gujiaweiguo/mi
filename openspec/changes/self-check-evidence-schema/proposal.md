## Why

The repository now relies on `schemas/evidence-v1.json` as part of the canonical evidence contract, but the schema artifact itself is not yet protected by an explicit self-check workflow. We need a lightweight verification path now so schema drift, broken examples, or silent incompatibilities are caught before they weaken CI/archive contract trust.

## What Changes

- Add a dedicated self-check for `schemas/evidence-v1.json` parsing and example/fixture validation.
- Define which example or fixture artifacts must continue validating against the canonical schema.
- Clarify how schema self-checks fit alongside existing verification self-tests.
- Keep the scope limited to schema verification and documentation alignment, without changing gate semantics.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: strengthen evidence-contract integrity by requiring a repository-level self-check for the machine-readable schema artifact.

## Impact

- Affects schema-validation tooling and verification self-check scripts.
- Improves confidence that schema, docs, and fixtures stay aligned over time.
- Does not change runtime behavior, business scope, or gate topology.
