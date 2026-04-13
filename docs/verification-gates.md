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

The canonical evidence contract is documented in:

- `docs/verification-architecture.md` — system-level flow and boundary ownership across producer/schema/validator/gate/entrypoints
- `docs/evidence-contract.md` — human-readable field, invariant, and example reference
- `schemas/evidence-v1.json` — machine-readable JSON Schema for canonical evidence structure
- `openspec/specs/platform-foundation/spec.md` — normative OpenSpec requirements
- `scripts/verification/validate-gate.sh` — executable enforcement used by CI/archive gates

The schema covers structural validation. Gate-context checks such as commit-SHA matching, file-type matching, pass/fail acceptance, and e2e-specific artifact requirements remain enforced by the validator script.

On GitHub Actions:

- `push` is reserved for direct updates to `main` and uses the pushed commit SHA
- `pull_request` is the authoritative CI-ready event for PR validation and uses the PR head SHA as the gate target commit
- `workflow_dispatch` for archive checks uses the workflow commit SHA

For `ci-ready`, a branch update with an open pull request is expected to produce one authoritative workflow run rather than duplicate `push` and `pull_request` runs for the same validation purpose. Feature-branch pushes without a PR are intentionally excluded from `ci-ready`; direct pushes to `main` remain covered.

## Local Commands

Validate CI readiness for the current commit:

```bash
scripts/ci-ready.sh
```

`scripts/ci-ready.sh` now runs the schema self-check first, then executes prerequisite validation (`go vet`, frontend typecheck, and frontend build), and only then evaluates the commit-scoped CI evidence gate.

Validate archive readiness for the current commit:

```bash
scripts/archive-ready.sh
```

Run validator self-tests against fixture data:

```bash
scripts/verification/self-test.sh
scripts/verification/self-test-artifacts.sh
scripts/verification/self-test-rehearsal.sh
```

Run schema self-check against the canonical evidence schema:

```bash
scripts/verification/self-test-schema.sh
```

Validate a cutover rehearsal result artifact:

```bash
scripts/verification/validate-rehearsal-result.sh artifacts/rehearsal/<commit-sha>/<result-file>.json --commit-sha <commit-sha>
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

Generate current-commit live-stack e2e evidence for archive validation:

```bash
scripts/verification/run-e2e-live.sh
```

## Current Repository State

This repository does not yet contain the target application or real test producers, so the real gate checks are expected to fail for the current HEAD until actual test jobs write evidence files.

That is intentional: the gate should be **red** rather than silently permissive.

## Maintenance expectations

When modifying verification scripts, schema, or gate logic, follow the maintenance policy in `docs/verification-architecture.md`. That document defines required revalidation steps, impact checks, and documentation sync duties for each change type (schema, producer, validator/gate, entrypoint).

Key points:

- Missing or stale evidence must always be rejected, regardless of changes to other verification components.
- After any verification-layer change, run `scripts/verification/self-test.sh` and `scripts/verification/self-test-schema.sh` to confirm regression-free behavior.
- CI-ready prerequisite validation is a fail-fast pre-gate stage; it does not create new evidence types and does not weaken the `unit` + `integration` evidence requirement.
- Keep this document, `docs/verification-architecture.md`, and `docs/evidence-contract.md` synchronized when commands or gate levels change.
