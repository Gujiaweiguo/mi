## Why

The repository's CI-ready workflow is currently correct but noisy: the same pull request update can trigger duplicate `ci-ready` runs, producing two copies of the same checks and making failures harder to interpret. This wastes CI time, obscures signal during review, and makes the default verification path feel less trustworthy even when the underlying jobs are healthy.

We need a small focused change now so contributors see one clear expected CI-ready result per PR update, while preserving the protection we still need for direct branch pushes and other intended workflow entrypoints.

## What Changes

- Deduplicate `ci-ready` workflow triggering so a single PR update does not create redundant runs for the same verification path.
- Clarify which GitHub events should execute CI-ready validation and which should not.
- Preserve the current validation semantics and required checks; this change is about trigger policy, not test or gate behavior.
- Keep scope limited to GitHub Actions workflow wiring and the documentation needed to explain the intended trigger model.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: refine CI-ready workflow trigger policy so contributors receive one expected verification run per PR update instead of duplicate runs.

## Impact

- Affects GitHub Actions workflow definitions under `.github/workflows/`.
- Affects contributor-facing CI expectations and PR review ergonomics.
- Does not change business scope, verification evidence schema, or gate validation rules.
