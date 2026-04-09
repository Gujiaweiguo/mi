## Why

The evidence contract is now documented in prose and enforced in code, but the repository still lacks a machine-readable schema that tools can validate against directly. We need a canonical JSON Schema now so documentation, validators, and future tooling can converge on one structural definition instead of duplicating field rules.

## What Changes

- Add a machine-readable JSON Schema file for the canonical verification evidence contract.
- Define how the schema relates to the human-readable evidence contract doc and the executable gate validator.
- Clarify which evidence invariants remain validator-enforced even if they are also represented in schema form.
- Keep the change scoped to contract definition and integration preparation, without changing CI/archive gate behavior yet.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: extend the verification contract definition so canonical evidence is described in both human-readable and machine-readable forms.

## Impact

- Affects verification documentation and future validator integration points.
- Introduces a stable schema artifact that other tooling can consume.
- Does not alter current runtime behavior, first-release scope, or gate topology.
