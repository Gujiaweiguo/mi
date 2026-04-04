# Verification Gates

This repository enforces staged test gates using machine-readable evidence files.

## Gate Levels

- **CI Ready** = passing `unit` + `integration` evidence for the current commit
- **Archive Ready** = passing `unit` + `integration` + `e2e` evidence for the current commit

Archive readiness is stricter than CI readiness.

## Evidence Location

Evidence must exist under:

```text
artifacts/verification/<commit-sha>/unit.json
artifacts/verification/<commit-sha>/integration.json
artifacts/verification/<commit-sha>/e2e.json
```

The required JSON contract is defined in:

- `openspec/changes/legacy-system-migration/test-evidence-contract.md`

On GitHub Actions:

- `push` uses the pushed commit SHA
- `pull_request` uses the PR head SHA as the gate target commit
- `workflow_dispatch` for archive checks uses the workflow commit SHA

## Local Commands

Validate CI readiness for the current commit:

```bash
scripts/ci-ready.sh
```

Validate archive readiness for the current commit:

```bash
scripts/archive-ready.sh
```

Run validator self-tests against fixture data:

```bash
scripts/verification/self-test.sh
scripts/verification/self-test-artifacts.sh
```

Generate current-commit unit evidence:

```bash
scripts/verification/run-unit.sh
```

Generate current-commit integration evidence:

```bash
scripts/verification/run-integration.sh
```

Generate current-commit e2e evidence:

```bash
scripts/verification/run-e2e.sh
```

## Current Repository State

This repository does not yet contain the target application or real test producers, so the real gate checks are expected to fail for the current HEAD until actual test jobs write evidence files.

That is intentional: the gate should be **red** rather than silently permissive.
