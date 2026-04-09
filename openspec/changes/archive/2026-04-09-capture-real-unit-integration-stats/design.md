## Context

The repository now uses real E2E result counts for evidence, but `run-unit.sh` and `run-integration.sh` still hard-code `stats` values after running test commands. That creates an avoidable trust gap: the evidence claims a count, but the count is not actually derived from the test run.

## Goals / Non-Goals

**Goals:**
- Capture real unit and integration test counts from the existing backend/frontend test commands.
- Keep evidence schema and gate semantics unchanged while improving evidence accuracy.
- Make the collection path deterministic and automation-friendly.

**Non-Goals:**
- No redesign of the current test framework mix.
- No change to which evidence files gates require.
- No expansion of evidence fields beyond the existing schema.

## Decisions

### 1. Derive counts from machine-readable or parseable test output
Evidence stats should come from test outputs that can be parsed reliably, rather than inferred from script stages. If one test command lacks a good machine-readable output path, the implementation should use the least fragile parseable format available in the current toolchain.

### 2. Preserve a single unit evidence artifact
`unit.json` currently covers both backend unit tests and frontend unit tests. The implementation should preserve that artifact shape while aggregating real counts from both sources into one evidence payload.

### 3. Keep integration evidence scoped to the integration command
`integration.json` should derive its counts solely from the backend integration test command and continue using the existing file path and schema.

## Risks / Trade-offs

- **[Risk] Different test runners expose counts differently** → **Mitigation:** use explicit output formats and small parsing helpers instead of brittle free-form log scraping where possible.
- **[Risk] Aggregating backend and frontend unit counts may obscure per-runner detail** → **Mitigation:** preserve the current single-artifact contract while keeping parsing logic transparent and testable.
- **[Risk] Test output formats may drift with tool upgrades** → **Mitigation:** add regression checks around the parsing path and keep parser logic minimal.

## Migration Plan

1. Audit current unit/integration commands and identify stable result-capture formats.
2. Implement parsing helpers and integrate them into `run-unit.sh` / `run-integration.sh`.
3. Update self-tests or targeted verification checks to ensure emitted stats are derived from real results.
4. Regenerate evidence for an implementation commit and confirm gates still pass.

## Open Questions

- Should backend unit and frontend unit counts eventually be split into separate evidence files, or remain intentionally aggregated into `unit.json` for gate simplicity?
