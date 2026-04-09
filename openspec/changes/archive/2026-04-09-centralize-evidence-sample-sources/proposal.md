## Why

The repository currently describes canonical evidence examples in documentation while separately maintaining representative fixture files for schema and gate self-tests. We need a single authoritative sample source now so schema checks, docs, and fixtures stay aligned without repeated manual updates.

## What Changes

- Establish a canonical repository-owned source for representative CI-style and archive/e2e-style evidence samples.
- Update docs and self-check entrypoints to reference the centralized sample source instead of duplicating inline examples where possible.
- Clarify the relationship between centralized samples, schema validation, and gate self-tests.
- Keep the scope limited to sample-source alignment and documentation/verification references, without changing gate semantics.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: require canonical evidence sample sources to stay aligned across schema self-checks and contributor-facing documentation.

## Impact

- Affects evidence-contract documentation, schema self-check inputs, and possibly fixture/reference layout.
- Reduces duplicate maintenance across docs and verification assets.
- Does not alter evidence schema semantics, gate behavior, or business scope.
