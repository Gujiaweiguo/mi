## Context

The repository now has a stronger CI-ready path than before: schema self-checks, prerequisite validation, unit/integration evidence production, and gate validation all run in a defined order. Functionally this is working, but the GitHub Actions trigger policy is still noisy. A single pull request update can cause duplicate `ci-ready` workflow runs, which means the same contributor action creates two copies of the same check names and two parallel status narratives for one logical validation path.

This is not a gate-correctness problem; it is a workflow-trigger design problem. The required evidence types, schema validation rules, and CI-ready gate semantics should remain unchanged. What needs refinement is when the workflow is triggered, especially across `push` and `pull_request` events for branch updates that are already represented in the PR lifecycle.

## Goals / Non-Goals

**Goals:**
- Ensure a single PR update produces one expected `ci-ready` workflow run rather than duplicate runs.
- Preserve the current CI-ready validation semantics, including prerequisite validation and commit-scoped evidence gates.
- Keep direct branch protections that are still needed outside the PR path, especially for branches or events where `pull_request` is not the source of truth.
- Make the trigger model understandable enough that contributors can predict when CI-ready will run.

**Non-Goals:**
- No changes to evidence schema, evidence contents, or gate pass/fail semantics.
- No changes to archive-ready trigger policy unless needed only for explanation or consistency.
- No business-code or verification-script behavior changes beyond what is necessary to support the trigger-policy refinement.
- No attempt to redesign all repository workflows; this change only addresses duplicate CI-ready execution.

## Decisions

### 1. Prefer one authoritative event family for PR validation

**Decision:** CI-ready validation for reviewable pull request updates should be driven by the `pull_request` event family, while plain branch `push` should be reserved only for cases that are not already covered by the PR lifecycle.

**Why:** Duplicate runs happen because the same branch update is being observed through two valid but overlapping event sources. For PR review ergonomics, the PR event is the more natural authority because that is also where required checks are surfaced and evaluated.

**Alternatives considered:**
- Keep both `push` and `pull_request` for all branches. Rejected because it preserves the duplication problem.
- Use only `push`. Rejected because PR checks and contributor expectations are centered around the PR lifecycle, not raw branch pushes.

### 2. Preserve commit-scoped validation semantics while changing only trigger policy

**Decision:** The workflow trigger model may change, but the jobs within a `ci-ready` run will continue to validate the current commit SHA, produce the same evidence types, and enforce the same gate rules.

**Why:** The problem is excess workflow invocation, not incorrect validation semantics. Changing the gate contract at the same time would widen scope and create unnecessary risk.

**Alternatives considered:**
- Soften gate requirements to reduce duplication pain. Rejected because that treats a workflow-config symptom as a validation-policy problem.

### 3. Keep check names stable where possible

**Decision:** The visible PR check names should remain stable, even if workflow trigger filters or event wiring change underneath them.

**Why:** Required-check configuration and reviewer recognition depend more on stable check identity than on the exact trigger implementation. Keeping names stable reduces administrative fallout.

**Alternatives considered:**
- Rename jobs or split check families during trigger cleanup. Rejected because the change is meant to reduce noise, not introduce new reviewer-facing concepts.

### 4. Document trigger intent alongside workflow changes

**Decision:** The refined trigger model should be documented in the same change so contributors understand why a PR update produces one run instead of two and how non-PR pushes are still covered.

**Why:** Trigger policy is easy to forget or re-expand accidentally if it only lives in YAML. A short explicit explanation reduces regression risk.

**Alternatives considered:**
- Rely on workflow YAML alone as the source of truth. Rejected because the problem itself is contributor-facing confusion.

## Risks / Trade-offs

- **[Risk] Trigger deduplication accidentally removes validation for a path that previously relied on `push`** → **Mitigation:** define the intended event matrix explicitly and preserve non-PR push coverage where it is still required.**
- **[Risk] Required-check configuration on GitHub becomes inconsistent with the new trigger model** → **Mitigation:** keep job names stable and verify PR status behavior after the change.**
- **[Risk] Contributors misread reduced runs as reduced protection** → **Mitigation:** document that the validation semantics remain unchanged and only redundant invocation is removed.**
- **[Risk] Workflow changes are tested only on one PR path and miss edge cases such as direct branch pushes** → **Mitigation:** validate both PR-triggered behavior and at least one non-PR push scenario where applicable.**

## Migration Plan

1. Identify the overlapping `push` and `pull_request` trigger paths that currently cause duplicate CI-ready execution.
2. Refine workflow triggers or conditional job execution so PR updates map to one authoritative CI-ready run.
3. Verify that the resulting run still uses the same commit SHA and required evidence semantics.
4. Confirm that required check names remain usable in the PR UI and that no unexpected coverage gap was introduced for direct pushes.
5. Update contributor-facing documentation to describe the intended trigger model.

Rollback is straightforward: revert the workflow trigger change if any required validation path disappears or if check-reporting behavior becomes inconsistent.

## Open Questions

- Should non-PR pushes to feature branches continue to run `ci-ready`, or should `push` be narrowed to protected branches only?
- Is the cleanest deduplication strategy trigger filtering at the workflow level, or conditional job execution keyed off event context?
- Are there any branch-protection settings that rely on the current duplicate check appearance and would need follow-up cleanup?
