## Why

The verification pipeline has matured quickly (producer, schema, validator, gate, CI entrypoint), but the architecture and responsibility boundaries are spread across many files and recent archived changes. We need one architecture document now so contributors can understand and extend the system without re-discovering implicit coupling.

## What Changes

- Add a dedicated verification architecture document describing end-to-end flow and ownership boundaries across producer, schema, validator, gate, and CI-ready entrypoints.
- Define which checks are structural (schema-driven) versus contextual (gate-driven) and where each is enforced.
- Document the canonical execution sequence and failure surface for local and CI usage.
- Keep this change documentation-only, with no behavior changes to scripts or gate semantics.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: clarify verification architecture and role boundaries for maintaining CI-ready/archive-ready evidence workflows.

## Impact

- Affects verification documentation and contributor onboarding clarity.
- Reduces maintenance ambiguity around where to implement future verification changes.
- Does not change runtime behavior, schema semantics, or gate policy.
