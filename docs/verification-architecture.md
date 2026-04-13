# Verification Architecture

This document defines responsibility boundaries for the verification system, and explains how commit-scoped evidence flows into CI-ready and archive-ready gate decisions.

## Scope and intent

- Focus: producer / schema / validator / gate / entrypoint boundaries.
- Focus: structural vs contextual validation split.
- Non-goal: changing gate behavior or schema semantics.

## End-to-end data flow

1. Prerequisite validation checks repository readiness before gate evaluation.
   - `scripts/verification/run-prerequisites.sh`
   - Checks: backend static analysis, frontend typecheck, frontend build validation
2. Test runners execute and collect result statistics.
   - `scripts/verification/run-unit.sh`
   - `scripts/verification/run-integration.sh`
   - `scripts/verification/run-e2e.sh`
   - `scripts/verification/run-e2e-live.sh`
3. Producers write canonical evidence JSON for the current commit.
   - `scripts/verification/write-evidence.py`
   - Output location: `artifacts/verification/<commit_sha>/<test_type>.json`
4. Gate evaluation loads required evidence per gate level.
   - `scripts/verification/validate-gate.sh`
5. Each evidence file is checked in two layers:
   - Structural check via schema validator:
     - `scripts/verification/validate-evidence-structure.py`
     - `schemas/evidence-v1.json`
   - Contextual gate check in gate validator:
     - commit/test-type alignment
     - status gating
     - stats/timestamp/e2e-artifact rules
6. Entrypoints orchestrate gate invocation:
   - CI path: `scripts/ci-ready.sh`
   - Archive path: `scripts/archive-ready.sh`

## Boundary ownership model

| Boundary | Responsibility | Primary files |
|---|---|---|
| Producer | Generate commit-scoped evidence from test outputs and enforce producer-side invariants before writing files | `scripts/verification/run-unit.sh`, `scripts/verification/run-integration.sh`, `scripts/verification/run-e2e.sh`, `scripts/verification/write-evidence.py`, `scripts/verification/collect-test-stats.py` |
| Schema | Define canonical evidence shape, required fields, enums/patterns, and structural constraints | `schemas/evidence-v1.json` |
| Validator | Execute schema validation and contextual gate validation for each evidence file | `scripts/verification/validate-evidence-structure.py`, `scripts/verification/validate-gate.sh` |
| Gate | Define required evidence set by gate level and aggregate pass/fail decision | `scripts/verification/validate-gate.sh` |
| Entrypoints | Provide operator/CI commands that invoke prerequisite validation and gate checks in the correct sequence | `scripts/verification/run-prerequisites.sh`, `scripts/ci-ready.sh`, `scripts/archive-ready.sh` |

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
  2. Prerequisite validation (`scripts/verification/run-prerequisites.sh`)
  3. CI gate evaluation (`scripts/verification/validate-gate.sh` with `ci` gate argument)
- Required evidence types: `unit`, `integration`
- Prerequisite validation is fail-fast and does not emit additional evidence files

### Archive-ready path

- Entrypoint: `scripts/archive-ready.sh`
- Validation sequence:
  1. Archive gate evaluation (`scripts/verification/validate-gate.sh` with `archive` gate argument)
- Required evidence types: `unit`, `integration`, `e2e`

Both paths reuse the same schema and validator components; archive applies a stricter required evidence set and `e2e` artifact expectations. Archive workflows may additionally produce live-stack e2e evidence through `scripts/verification/run-e2e-live.sh`, but gate evaluation still consumes the canonical `e2e.json` artifact.

## Handoff contracts

1. **Runners → Producer writer**: tests emit machine-readable outputs that are normalized into canonical evidence.
2. **Producer → Schema validator**: evidence must satisfy canonical structure.
3. **Schema validator → Gate validator**: only structurally valid evidence proceeds to contextual checks.
4. **Gate validator → Entrypoints**: gate returns pass/fail exit status consumed by CI/archive workflows.

## Maintenance policy

The verification stack is a coordinated system. Changes to one layer can break others if they are not updated together. This section defines what must happen when each kind of verification component changes.

### Change types and required actions

| Change type | What to update | Revalidation |
|---|---|---|
| **Schema** (`schemas/evidence-v1.json`) | Update producers, validators, contract docs, and fixture data in one coordinated change. | Run `scripts/verification/self-test-schema.sh`, `scripts/verification/self-test.sh`, and verify CI/archive gate paths produce correct pass/fail outcomes. |
| **Producer** (`run-unit.sh`, `run-integration.sh`, `run-e2e.sh`, `write-evidence.py`, `collect-test-stats.py`) | Ensure evidence output matches the current schema. Update fixtures if sample data changes. | Produce evidence for each test type, then validate with `scripts/verification/validate-gate.sh`. |
| **Validator / Gate** (`validate-evidence-structure.py`, `validate-gate.sh`) | Update docs if contextual checks change. Update fixtures if rejection behavior changes. | Run `scripts/verification/self-test.sh` and `scripts/verification/self-test-artifacts.sh` to confirm unchanged outcomes for existing fixture data. |
| **Entrypoints** (`ci-ready.sh`, `archive-ready.sh`) | Update operator docs in `docs/verification-gates.md` if the invocation sequence changes. | Run the entrypoint and verify gate pass/fail outcome matches expectation. |

### Documentation sync

After any verification-layer change, confirm these documents remain consistent:

1. `docs/verification-architecture.md` (this document) for boundary ownership and flow descriptions
2. `docs/verification-gates.md` for commands, gate levels, and evidence location
3. `docs/evidence-contract.md` for field descriptions, validator rules, and sample references
4. `schemas/evidence-v1.json` for the machine-readable structural contract
5. `openspec/specs/platform-foundation/spec.md` for normative requirement definitions

If a change modifies evidence structure or gate behavior, update all five references before marking the change complete.

### Impact-check guidance

Before merging any verification-stack change, perform these checks:

1. **Schema self-check**: run `scripts/verification/self-test-schema.sh` to confirm the schema parses and fixture data still validates.
2. **Gate regression check**: run `scripts/verification/self-test.sh` and `scripts/verification/self-test-artifacts.sh` to confirm gate behavior is unchanged for existing fixture evidence.
3. **Evidence compatibility check**: if schema fields change, produce fresh evidence and validate through the full CI or archive path.
4. **Documentation alignment review**: read through the five documents listed above and confirm cross-references, field descriptions, and command references are accurate.

### Missing and stale evidence rejection

CI and archive gates must continue to reject evidence that is missing or stale (commit SHA mismatch). Any maintenance change to producer, schema, validator, gate, or entrypoint logic must preserve this behavior:

- Missing evidence for a required test type must cause gate failure.
- Evidence whose `commit_sha` does not match the evaluated commit must be rejected as stale.
- These rejection semantics are non-negotiable and must not be weakened by maintenance changes.

## Change guidance

- If you add or modify evidence fields, start with `schemas/evidence-v1.json`, then update producer/validator/docs consistently.
- If you add gate semantics (new contextual rule), update `scripts/verification/validate-gate.sh` and related self-tests.
- If you change how verification is invoked in automation, update `scripts/ci-ready.sh` or `scripts/archive-ready.sh`.
- Keep this architecture doc aligned with:
  - `docs/verification-gates.md`
  - `docs/evidence-contract.md`
  - `openspec/specs/platform-foundation/spec.md`
- For the full maintenance policy covering all verification-layer change types, see the Maintenance policy section above.
