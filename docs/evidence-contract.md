# Evidence Contract

This document is the human-readable reference for verification evidence written under `artifacts/verification/<commit-sha>/`.

- Normative behavior lives in `openspec/specs/platform-foundation/spec.md`.
- Executable enforcement lives in `scripts/verification/validate-gate.sh`.
- Evidence generation lives in `scripts/verification/write-evidence.py`.

If this document and the verification scripts disagree, treat the scripts as authoritative until the docs are updated.

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

## Canonical Examples

### CI-Passing `unit.json`

```json
{
  "schema_version": "1",
  "project": "mi",
  "change": "example-change",
  "commit_sha": "0123456789abcdef0123456789abcdef01234567",
  "test_type": "unit",
  "status": "passed",
  "started_at": "2026-04-09T00:00:00Z",
  "finished_at": "2026-04-09T00:01:00Z",
  "source": {
    "kind": "local",
    "workflow": "run-unit",
    "run_id": "local"
  },
  "stats": {
    "total": 2,
    "passed": 2,
    "failed": 0,
    "skipped": 0
  },
  "artifacts": []
}
```

### Archive-Passing `e2e.json`

```json
{
  "schema_version": "1",
  "project": "mi",
  "change": "example-change",
  "commit_sha": "0123456789abcdef0123456789abcdef01234567",
  "test_type": "e2e",
  "status": "passed",
  "started_at": "2026-04-09T00:10:00Z",
  "finished_at": "2026-04-09T00:15:00Z",
  "source": {
    "kind": "github-actions",
    "workflow": "e2e",
    "run_id": "123456789"
  },
  "stats": {
    "total": 41,
    "passed": 41,
    "failed": 0,
    "skipped": 0
  },
  "artifacts": [
    "frontend/test-results/e2e-results.json"
  ]
}
```

## Relationship to OpenSpec and Scripts

- `openspec/specs/platform-foundation/spec.md` defines the requirement-level contract for CI-ready and archive-ready evidence.
- `scripts/verification/write-evidence.py` is the producer-side helper that writes canonical evidence files.
- `scripts/verification/validate-gate.sh` is the enforcement layer that accepts or rejects evidence during CI/archive checks.
- `docs/verification-gates.md` is the operator entrypoint for commands and workflow usage.

This document exists so contributors do not need to read inline shell/Python code just to understand the expected JSON shape.
