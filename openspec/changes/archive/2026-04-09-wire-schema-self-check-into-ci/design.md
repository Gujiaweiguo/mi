## Context

The repository already provides `scripts/verification/self-test-schema.sh`, but contributors can still run `scripts/ci-ready.sh` without exercising schema integrity. Since the schema is now part of the canonical evidence contract, the default CI-ready path should fail fast when the schema itself drifts or representative samples no longer validate.

## Goals / Non-Goals

**Goals:**
- Add schema self-check execution to the repository CI-ready path.
- Preserve the existing CI-ready gate semantics and output expectations.
- Keep the implementation lightweight and repository-native.

**Non-Goals:**
- No change to archive-ready behavior in this change.
- No change to schema semantics or gate validator logic.
- No introduction of external CI services or workflow-specific logic.

## Decisions

### 1. Run schema self-check before gate validation
The CI-ready path should fail early if the schema or its representative samples are invalid, before checking commit-scoped evidence readiness.

### 2. Reuse existing scripts instead of duplicating checks
`scripts/ci-ready.sh` should invoke `scripts/verification/self-test-schema.sh` rather than embedding the schema-check logic again.

### 3. Keep archive path unchanged unless explicitly needed later
This change targets the default CI-ready entrypoint only. Archive-ready may adopt the same preflight later if desired, but it is not required to realize the immediate benefit.

## Risks / Trade-offs

- **[Risk] CI-ready becomes stricter than before** → **Mitigation:** that is the intended protection; the added check is lightweight and deterministic.
- **[Risk] Users may confuse schema self-check failure with evidence gate failure** → **Mitigation:** keep command output explicit about the execution stage.

## Migration Plan

1. Update `scripts/ci-ready.sh` to run schema self-check before the CI gate validator.
2. Update verification documentation to reflect the new default path.
3. Run the CI-ready path and schema self-check/gate self-tests to confirm the behavior is coherent.

## Open Questions

- Should `scripts/archive-ready.sh` eventually share the same schema self-check preflight for consistency, or remain narrower by design?
