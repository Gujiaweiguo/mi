## Context

The canonical specs now describe the intended first-release non-membership product surface, and several recent changes have already introduced commit-scoped unit/integration/e2e evidence gates. What remains missing is a final acceptance-closure layer that turns those separate checks into a single current-commit release-readiness decision tied to the bounded first-release scope rather than to ad hoc human judgment.

## Goals / Non-Goals

**Goals:**

- Define how a current commit is evaluated against the bounded first-release scope before it is treated as release-ready.
- Make blocker classification explicit: distinguish must-fix issues from known deferred items that remain out of scope or post-release.
- Require a machine-readable readiness summary that references current-commit evidence instead of relying on narrative-only status updates.
- Keep the acceptance closure aligned with existing archive and rehearsal gates rather than inventing a parallel release process.

**Non-Goals:**

- No new membership / `Associator` scope.
- No expansion of reporting beyond frozen `R01-R19`.
- No replacement of existing unit/integration/e2e evidence conventions.
- No new product capabilities unrelated to release-readiness closure itself.

## Decisions

- Reuse `deployment-and-cutover-operations` as the capability that owns final release-readiness closure. This keeps release gating, rehearsal, rollback, and acceptance decision logic in one operational spec instead of scattering it across business-domain capabilities.
- Model acceptance closure as a current-commit decision backed by machine-readable artifacts, not as a branch-wide or date-wide status. The repository already uses commit-scoped evidence files, so the final readiness decision should inherit that same identity boundary.
- Treat missing or stale evidence as a hard blocker, but keep deferred out-of-scope items explicit instead of silently folding them into release failure. This preserves the first-release boundary from `AGENTS.md` while still forcing must-fix operational gaps to be visible.
- Require one final summary artifact that synthesizes scope coverage, blockers, and current-commit evidence references. Alternatives considered: relying only on raw unit/integration/e2e files or only on prose release notes. Raw evidence alone is too fragmented for GO/NO-GO review, while prose alone is not gateable.

## Risks / Trade-offs

- **Risk:** Acceptance closure can become a vague checklist with no executable backing. → **Mitigation:** tie the closure requirement to commit-scoped machine-readable evidence and explicit blocker classification.
- **Risk:** The change may accidentally widen scope by trying to re-spec every business capability. → **Mitigation:** keep this change focused on release-readiness criteria and reuse existing canonical capability specs as the product boundary.
- **Risk:** Operators may confuse archive readiness, rehearsal readiness, and final release readiness. → **Mitigation:** define how CI, archive, rehearsal, and final acceptance gates relate to each other and which evidence each one requires.
- **Risk:** A final readiness artifact can drift from actual verification outputs. → **Mitigation:** require the summary to reference current-commit evidence and treat mismatched SHA or failed status as blocking.

## Migration Plan

1. Audit the existing canonical scope and evidence conventions.
2. Add the acceptance-closure requirement to `deployment-and-cutover-operations` with explicit current-commit gate semantics.
3. Implement or update the automation/docs needed to produce the final readiness summary artifact.
4. Run the bounded verification set, generate current-commit evidence, and validate the final acceptance artifact against those outputs.

## Open Questions

- Should the final readiness summary live under `artifacts/release-readiness/<commit-sha>/` or alongside rehearsal artifacts?
- Which bounded first-release verification set is mandatory for final acceptance beyond existing unit/integration/e2e gates: production rehearsal only, or also explicit scope-by-scope acceptance summary?
- Do we want a stable machine-readable blocker taxonomy (for example `must-fix`, `deferred`, `out-of-scope`) enforced by schema, or only documented convention?
