# Test Evidence Contract

This document defines the machine-readable evidence required to satisfy the staged test gates for `legacy-system-migration`.

## Gate Model

- **CI Ready** requires:
  - passing `unit` evidence for the current commit
  - passing `integration` evidence for the current commit
- **Archive Ready** requires:
  - passing `unit` evidence for the current commit
  - passing `integration` evidence for the current commit
  - passing `e2e` evidence for the current commit

`Archive Ready` is strictly stronger than `CI Ready`.

## Evidence Path Convention

Evidence is stored under a commit-scoped directory:

```text
artifacts/verification/<commit-sha>/unit.json
artifacts/verification/<commit-sha>/integration.json
artifacts/verification/<commit-sha>/e2e.json
```

Optional additional files may exist beside these summaries, such as `junit.xml`, HTML reports, screenshots, or export artifacts, but the JSON summaries are the gate inputs.

## Required Summary Structure

Each evidence file must be valid JSON and include at least:

```json
{
  "schema_version": "1",
  "project": "mi",
  "change": "legacy-system-migration",
  "commit_sha": "<full sha>",
  "test_type": "unit",
  "status": "passed",
  "started_at": "2026-03-30T10:00:00Z",
  "finished_at": "2026-03-30T10:03:12Z",
  "source": {
    "kind": "github-actions",
    "workflow": "ci-unit",
    "run_id": "123456789"
  },
  "stats": {
    "total": 120,
    "passed": 120,
    "failed": 0,
    "skipped": 3
  }
}
```

## Acceptance Rules

- `commit_sha` must match the current HEAD commit being verified or archived.
- `test_type` must match the file purpose: `unit`, `integration`, or `e2e`.
- `status` must be `passed`.
- Missing files fail the corresponding gate.
- Malformed JSON fails the corresponding gate.
- Evidence for a different commit does not satisfy the gate.
- Human claims or local memory do not satisfy the gate without evidence files.

## Immediate Repository Reality

This repository does not yet contain the target application or real test suites. Therefore, landing this contract makes the repository intentionally **gate-red** until real tests and evidence producers are implemented.

That is expected and correct.
