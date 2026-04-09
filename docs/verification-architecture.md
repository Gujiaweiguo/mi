# Verification Architecture

This document defines responsibility boundaries for the verification system, and explains how commit-scoped evidence flows into CI-ready and archive-ready gate decisions.

## Scope and intent

- Focus: producer / schema / validator / gate / entrypoint boundaries.
- Focus: structural vs contextual validation split.
- Non-goal: changing gate behavior or schema semantics.

## End-to-end data flow

1. Test runners execute and collect result statistics.
   - `scripts/verification/run-unit.sh`
   - `scripts/verification/run-integration.sh`
   - `scripts/verification/run-e2e.sh`
2. Producers write canonical evidence JSON for the current commit.
   - `scripts/verification/write-evidence.py`
   - Output location: `artifacts/verification/<commit_sha>/<test_type>.json`
3. Gate evaluation loads required evidence per gate level.
   - `scripts/verification/validate-gate.sh`
4. Each evidence file is checked in two layers:
   - Structural check via schema validator:
     - `scripts/verification/validate-evidence-structure.py`
     - `schemas/evidence-v1.json`
   - Contextual gate check in gate validator:
     - commit/test-type alignment
     - status gating
     - stats/timestamp/e2e-artifact rules
5. Entrypoints orchestrate gate invocation:
   - CI path: `scripts/ci-ready.sh`
   - Archive path: `scripts/archive-ready.sh`

## Boundary ownership model

| Boundary | Responsibility | Primary files |
|---|---|---|
| Producer | Generate commit-scoped evidence from test outputs and enforce producer-side invariants before writing files | `scripts/verification/run-unit.sh`, `scripts/verification/run-integration.sh`, `scripts/verification/run-e2e.sh`, `scripts/verification/write-evidence.py`, `scripts/verification/collect-test-stats.py` |
| Schema | Define canonical evidence shape, required fields, enums/patterns, and structural constraints | `schemas/evidence-v1.json` |
| Validator | Execute schema validation and contextual gate validation for each evidence file | `scripts/verification/validate-evidence-structure.py`, `scripts/verification/validate-gate.sh` |
| Gate | Define required evidence set by gate level and aggregate pass/fail decision | `scripts/verification/validate-gate.sh` |
| Entrypoints | Provide operator/CI commands that invoke gate checks in the correct sequence | `scripts/ci-ready.sh`, `scripts/archive-ready.sh` |

## Structural vs contextual validation split

### Structural validation (schema-driven)

Structural validation answers: “Is this evidence document well-formed according to the canonical contract?”

- Enforced by `schemas/evidence-v1.json`
- Executed by `scripts/verification/validate-evidence-structure.py`
- Typical checks:
  - required fields exist
  - field types are valid
  - enum/pattern constraints pass
  - disallowed extra properties are rejected

### Contextual validation (gate-driven)

Contextual validation answers: “Is this evidence valid for this gate decision on this commit?”

- Enforced in `scripts/verification/validate-gate.sh`
- Typical checks:
  - evidence `commit_sha` matches the evaluated commit
  - evidence `test_type` matches expected required type
  - `status` is `passed` for accepted evidence
  - stats arithmetic and timestamp ordering are valid
  - archive-required `e2e` evidence includes non-empty artifact references

## CI and archive evaluation paths

### CI-ready path

- Entrypoint: `scripts/ci-ready.sh`
- Validation sequence:
  1. Schema self-check (`scripts/verification/self-test-schema.sh`)
  2. CI gate evaluation (`scripts/verification/validate-gate.sh` with `ci` gate argument)
- Required evidence types: `unit`, `integration`

### Archive-ready path

- Entrypoint: `scripts/archive-ready.sh`
- Validation sequence:
  1. Archive gate evaluation (`scripts/verification/validate-gate.sh` with `archive` gate argument)
- Required evidence types: `unit`, `integration`, `e2e`

Both paths reuse the same schema and validator components; archive applies a stricter required evidence set and `e2e` artifact expectations.

## Handoff contracts

1. **Runners → Producer writer**: tests emit machine-readable outputs that are normalized into canonical evidence.
2. **Producer → Schema validator**: evidence must satisfy canonical structure.
3. **Schema validator → Gate validator**: only structurally valid evidence proceeds to contextual checks.
4. **Gate validator → Entrypoints**: gate returns pass/fail exit status consumed by CI/archive workflows.

## Change guidance

- If you add or modify evidence fields, start with `schemas/evidence-v1.json`, then update producer/validator/docs consistently.
- If you add gate semantics (new contextual rule), update `scripts/verification/validate-gate.sh` and related self-tests.
- If you change how verification is invoked in automation, update `scripts/ci-ready.sh` or `scripts/archive-ready.sh`.
- Keep this architecture doc aligned with:
  - `docs/verification-gates.md`
  - `docs/evidence-contract.md`
  - `openspec/specs/platform-foundation/spec.md`
