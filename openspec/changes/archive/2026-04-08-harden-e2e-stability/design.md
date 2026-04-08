## Context

The repository already enforces commit-scoped evidence gates and requires `e2e` evidence for archive readiness. In practice, E2E outcomes can still become unreliable when scenario scope, fixture assumptions, and evidence validity handling drift across changes. This change hardens the E2E contract at specification level so archive decisions remain trustworthy for first-release non-membership scope.

## Goals / Non-Goals

**Goals:**
- Make E2E evidence a stable archive-quality signal for the current HEAD commit.
- Clarify requirement-level expectations for reproducible E2E verification in first-release supported flows.
- Preserve existing CI/archive gate hierarchy while tightening E2E-specific validity and scope boundaries.

**Non-Goals:**
- No new business features or membership scope.
- No replacement of current test stack or major tooling migration.
- No relaxation of commit-scoped evidence policy.
- No changes to fresh-start cutover policy.

## Decisions

### 1. Keep scope in `platform-foundation`
E2E stability is a verification foundation concern, not a new business capability. The change modifies only `platform-foundation` requirements.

### 2. Harden requirement-level E2E reproducibility
The test foundation requirement is extended to require reproducible execution for supported first-release flows from clean checkout/bootstrap assumptions.

### 3. Harden archive gate treatment of E2E evidence failures
Archive-gate requirement is extended with explicit E2E failure modes (missing/stale/malformed/failed) and clear no-ready outcomes.

### 4. Keep first-release scope boundaries explicit
E2E hardening remains bounded to non-membership first-release flows already accepted by OpenSpec and does not widen product scope.

## Risks / Trade-offs

- **[Risk] Over-hardening could increase false negatives in E2E gates** → **Mitigation:** keep requirement text focused on deterministic supported flows and explicit evidence validity rules.
- **[Risk] Scope creep into broad test-infra redesign** → **Mitigation:** limit changes to requirement-level contracts for existing Playwright/evidence workflows.
- **[Risk] Ambiguity between CI gate and archive gate behavior** → **Mitigation:** keep archive-specific E2E rules in archive-gate requirement while preserving existing CI rule definitions.

## Migration Plan

1. Add this change proposal and design for E2E hardening scope.
2. Add delta spec updates in `platform-foundation` requirements.
3. Define executable tasks for contract hardening and evidence verification.
4. Implement in a later apply phase with verification artifacts generated for the resulting commit.

Rollback is low-risk because this change is contract hardening: if implementation proves unstable, execution can be adjusted while retaining existing gate baseline until corrected.

## Open Questions

- Should first-release archive evidence require one focused canonical E2E suite definition in docs, or remain command-agnostic as long as evidence contract is satisfied?
- Do we need an explicit stability metric in evidence stats (e.g., rerun consistency), or is pass/fail with strict validity sufficient for this release?
