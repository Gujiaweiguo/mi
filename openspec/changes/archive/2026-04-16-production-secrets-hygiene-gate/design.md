## Context

The repository already distinguishes runtime hygiene from release evidence hygiene, but it still treats production configuration values as structurally valid as long as the Compose env file exists and renders. `deploy/env/production.env` explicitly documents placeholder values such as `change-me`, `change-me-root`, and `change-me-production-secret` that must be replaced before real deployment, yet `scripts/compose-preflight.sh` and `scripts/cutover-rehearsal.sh` do not reject those defaults today.

This creates a false-confidence gap: the production rehearsal path can return `GO` for a configuration that is operationally insecure and explicitly documented as not production-ready. The change needs to preserve the isolated-runtime behavior already added to rehearsal while making secrets hygiene part of the same fail-fast contract.

Stakeholders:

- Release owners relying on a trustworthy production `GO`/`NO-GO` signal.
- Operators preparing production env files and running preflight or rehearsal commands.
- Reviewers auditing whether the deployment pipeline blocks obviously unsafe production configuration.

## Goals / Non-Goals

**Goals:**

- Block placeholder/default production credentials and secrets from passing the supported production preflight path.
- Ensure production rehearsal also fails early when evaluated configuration still contains blocked placeholder values.
- Define an explicit blocked-value contract for production secrets that is shared by preflight and rehearsal.
- Keep the secrets-hygiene gate scoped to production validation and documented release-confidence behavior.

**Non-Goals:**

- Introducing a full secrets manager, vault integration, or encrypted secret distribution workflow.
- Rotating or auto-generating production secrets.
- Redesigning the full deployment configuration model beyond the blocked placeholder check.
- Adding a broad policy engine for every configuration key in all environments.

## Decisions

### Decision 1: Enforce a small explicit blocked-value list rather than heuristic entropy checks

The gate will reject a small, explicit list of documented placeholder/default values for the production env file rather than attempting to measure password strength or entropy.

Why:

- The current problem is specific and documented: known placeholder strings are allowed to pass.
- Explicit values are easy to audit, deterministic, and safe to keep in tests.

Alternatives considered:

- Generic password-strength validation: rejected because it is harder to reason about, more brittle, and not necessary to block the documented false-positive path.
- Manual checklist-only enforcement: rejected because the problem is specifically that the automated gate can still return `GO`.

### Decision 2: Put the secrets-hygiene check in preflight and reuse it from rehearsal

The authoritative implementation point will be `scripts/compose-preflight.sh`, and rehearsal will continue to inherit the same production validation by invoking preflight before destructive steps.

Why:

- Preflight is already the supported place for environment and runtime validation.
- Reusing the same gate avoids drift between direct bring-up validation and rehearsal behavior.

Alternatives considered:

- Add a separate secrets check only inside rehearsal: rejected because direct production preflight would remain weaker than rehearsal.
- Duplicate the validation logic across scripts: rejected because policy drift would be likely.

### Decision 3: Scope the gate to production-oriented validation only

The blocked-value policy will apply to production env evaluation, not to development defaults or test fixtures.

Why:

- Development and test paths intentionally use convenient local defaults and should not inherit production secret rules.
- The false-confidence risk exists specifically when a production env file is treated as release-ready.

Alternatives considered:

- Enforce the same rule across every environment: rejected because it would add friction without improving the production release signal.

### Decision 4: Make failure actionable and auditable

The secrets-hygiene gate should report exactly which required production keys are still blocked and should cause rehearsal/preflight to terminate before stack startup.

Why:

- Operators need fast, specific feedback to fix the env file.
- A `NO-GO` that does not explain which placeholder values remain is less useful than the current problem.

Alternatives considered:

- Generic “invalid configuration” error: rejected because it slows remediation and weakens audit value.

## Risks / Trade-offs

- **[Risk] Blocked-value policy may miss an insecure but non-placeholder secret** → **Mitigation:** keep the gate focused on eliminating the documented false-positive path, not on solving the full secrets-management problem.
- **[Risk] Operators may rely on placeholder values in local rehearsal habits** → **Mitigation:** scope enforcement to production-oriented validation only and keep the failure messaging explicit.
- **[Risk] Future placeholder values may be added without updating the gate** → **Mitigation:** centralize the blocked-value contract in one validation path and document it near deployment defaults.
- **[Risk] Temporary rehearsal env-file rewriting could obscure which source values triggered failure** → **Mitigation:** validate the effective env file and report the affected keys/values before stack startup.

## Migration Plan

1. Add a reusable production secrets-hygiene validation step to `scripts/compose-preflight.sh`.
2. Ensure `scripts/cutover-rehearsal.sh` continues to invoke that validation before any bootstrap or stack bring-up step.
3. Add tests/self-check coverage proving placeholder values fail while valid overrides pass.
4. Update deployment documentation so the blocked-value contract is explicit in release-readiness guidance.

Rollback approach:

- Revert the preflight validation change and its documentation/tests together if the blocked-value list is shown to be too strict or incorrect.

## Open Questions

- Should the blocked-value contract include only the currently documented placeholders, or also obvious empty-string / copied-dev-value cases?
- Should future production env keys with security impact be opt-in to this gate or automatically required once added to `production.env`?
