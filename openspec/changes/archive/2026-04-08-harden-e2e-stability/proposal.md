## Why

Archive readiness in this repository depends on commit-scoped `unit`/`integration`/`e2e` evidence, but E2E reliability is still too easy to degrade through unstable assertions, environment assumptions, or stale evidence paths. We need a focused hardening change now so E2E evidence remains a trustworthy release signal for first-release scope.

## What Changes

- Strengthen the E2E verification contract so current-commit E2E evidence is reproducible and valid for archive gates.
- Tighten requirement-level expectations for bounded first-release E2E coverage across supported non-membership operator flows.
- Clarify that stale, missing, malformed, or failed E2E evidence cannot satisfy archive readiness.
- Keep this change strictly in verification/reliability scope, without adding new business features.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `platform-foundation`: strengthen requirement-level E2E reliability and evidence validity expectations for archive-ready verification.

## Impact

- Affects Playwright-driven E2E verification workflows and related verification scripts/evidence generation behavior.
- Affects release-readiness confidence because archive gating depends on trustworthy E2E evidence.
- Does not expand first-release scope beyond non-membership capabilities and does not alter fresh-start cutover policy.
