## Why

The repository now enforces a canonical evidence contract in code, but the human-facing documentation still points to a dead legacy path and does not provide a single standalone contract reference. We need a documentation-focused change now so contributors can follow CI/archive gate rules without reverse-engineering scripts.

## What Changes

- Replace the stale verification-gate documentation reference with live canonical sources.
- Add a standalone evidence gate contract document that explains required JSON fields, invariants, and gate expectations for `unit`, `integration`, and `e2e` evidence.
- Clarify how the standalone contract relates to the platform-foundation OpenSpec requirements and executable verification scripts.
- Keep this change documentation-only with no verification logic or product-scope changes.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: document-level clarification of how canonical evidence-gate requirements are consumed by contributors and release operators.

## Impact

- Affects `docs/verification-gates.md` and the new standalone evidence contract documentation.
- Reduces contributor confusion around CI-ready/archive-ready evidence expectations and source-of-truth paths.
- Does not alter runtime behavior, gate logic, or first-release scope.
