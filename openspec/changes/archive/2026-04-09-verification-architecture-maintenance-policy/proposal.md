## Why

The verification stack is now mature, but it still lacks a stable maintenance policy that tells contributors how to evolve producer/schema/validator/gate/entrypoint behavior without drifting docs or weakening gate reliability. We need a clear, repeatable policy so future verification changes stay coherent and test-backed.

## What Changes

- Define a verification architecture maintenance policy covering producer, schema, validator, gate, and CI/archive entrypoints.
- Define mandatory impact checks for common change types (schema change, producer change, gate-rule change, entrypoint change).
- Define required regression verification for verification-system changes, including schema self-check and gate-level checks.
- Define documentation alignment rules so architecture and contract docs remain synchronized with executable scripts.

## Capabilities

### New Capabilities

<!-- None. -->

### Modified Capabilities

- `platform-foundation`: add normative maintenance-policy requirements for verification architecture evolution, regression checks, and documentation synchronization.

## Impact

- OpenSpec requirement delta under `openspec/changes/verification-architecture-maintenance-policy/specs/platform-foundation/spec.md`.
- Verification documentation and process guidance under `docs/`.
- Verification workflow maintenance expectations for `scripts/verification/*`, `scripts/ci-ready.sh`, and `scripts/archive-ready.sh`.
