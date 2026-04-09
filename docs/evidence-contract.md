# Evidence Contract

This document is the human-readable reference for verification evidence written under `artifacts/verification/<commit-sha>/`.

- Normative behavior lives in `openspec/specs/platform-foundation/spec.md`.
- Machine-readable structure lives in `schemas/evidence-v1.json`.
- Executable enforcement lives in `scripts/verification/validate-gate.sh`.
- Evidence generation lives in `scripts/verification/write-evidence.py`.

If this document and the verification scripts disagree, treat the scripts as authoritative until the docs are updated. The JSON Schema captures structural rules, while some gate-context checks remain validator-enforced.

## Gate Summary

- **CI Ready** requires passing `unit.json` and `integration.json` for the current commit SHA.
- **Archive Ready** requires passing `unit.json`, `integration.json`, and `e2e.json` for the current commit SHA.
- Missing, stale, malformed, or failed evidence does not satisfy the gate that requires it.

## Evidence Location

```text
artifacts/verification/<commit-sha>/unit.json
artifacts/verification/<commit-sha>/integration.json
artifacts/verification/<commit-sha>/e2e.json
```

## Canonical Top-Level Fields

The validator requires these top-level fields in every evidence file:

| Field | Type | Rules |
| --- | --- | --- |
| `schema_version` | string | Must be exactly `"1"` |
| `project` | string | Repository/project identifier |
| `change` | string | Change name associated with the evidence |
| `commit_sha` | string | Must match the commit currently being evaluated |
| `test_type` | string | Must match the file type: `unit`, `integration`, or `e2e` |
| `status` | string | Must be `"passed"` for the gate to accept the evidence |
| `started_at` | string | UTC ISO 8601 timestamp: `YYYY-MM-DDTHH:MM:SSZ` |
| `finished_at` | string | UTC ISO 8601 timestamp and must be greater than or equal to `started_at` |
| `source` | object | Must contain canonical source fields |
| `stats` | object | Must contain canonical numeric test counters |

## `source` Object

The nested `source` object is required and all fields must be non-empty strings.

| Field | Type | Rules |
| --- | --- | --- |
| `kind` | string | Execution origin, for example `local` or `github-actions` |
| `workflow` | string | Workflow or entrypoint name |
| `run_id` | string | Run identifier |

## `stats` Object

The nested `stats` object is required.

| Field | Type | Rules |
| --- | --- | --- |
| `total` | integer | Non-negative |
| `passed` | integer | Non-negative |
| `failed` | integer | Non-negative |
| `skipped` | integer | Non-negative |

Additional invariant:

```text
passed + failed + skipped <= total
```

## `artifacts` Field

- `artifacts` may be omitted for `unit` and `integration` evidence.
- If present, it must be an array of non-empty strings.
- For `e2e` evidence, `artifacts` is required and must be a non-empty array.

## Validator-Context Rules

The JSON Schema defines the structural contract, but these checks still depend on gate runtime context and remain enforced by `scripts/verification/validate-gate.sh`:

- `commit_sha` must match the commit currently being evaluated.
- `test_type` must match the evidence file being checked (`unit.json`, `integration.json`, or `e2e.json`).
- `status` must be `"passed"` for the gate to accept the evidence.
- `e2e` evidence must include a non-empty `artifacts` array.
- `passed + failed + skipped <= total` must hold for `stats`.

## Canonical Sample Sources

The repository-owned canonical sample sources are fixture files used by schema/gate self-checks:

- CI-style sample (`unit.json`):
  `scripts/verification/testdata/pass-ci/artifacts/verification/1111111111111111111111111111111111111111/unit.json`
- Archive/e2e-style sample (`e2e.json`):
  `scripts/verification/testdata/pass-archive/artifacts/verification/2222222222222222222222222222222222222222/e2e.json`

These files are authoritative sample payloads for documentation and schema self-check references.

## Relationship to OpenSpec and Scripts

- `openspec/specs/platform-foundation/spec.md` defines the requirement-level contract for CI-ready and archive-ready evidence.
- `schemas/evidence-v1.json` defines the machine-readable structural contract used by docs and future tooling.
- `scripts/verification/write-evidence.py` is the producer-side helper that writes canonical evidence files.
- `scripts/verification/validate-gate.sh` is the enforcement layer that accepts or rejects evidence during CI/archive checks.
- `scripts/verification/self-test-schema.sh` runs a standalone schema parsing and representative-sample validation self-check.
- `docs/verification-gates.md` is the operator entrypoint for commands and workflow usage.
- `docs/verification-architecture.md` documents boundary ownership and end-to-end verification flow.

This document exists so contributors do not need to read inline shell/Python code just to understand the expected JSON shape.

## Maintenance policy

When modifying the evidence contract (schema fields, validator rules, or producer output), follow the maintenance policy and impact-check guidance in `docs/verification-architecture.md`. That document defines required revalidation and documentation sync steps for each change type. In particular:

- Schema field changes require coordinated updates to producers, validators, fixtures, and all three verification docs.
- Validator-context rule changes require updated self-test fixtures and a gate regression check.
- The missing/stale evidence rejection semantics documented above must be preserved across all maintenance changes.
