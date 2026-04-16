## Why

The production deployment defaults intentionally document placeholder credentials and secrets that must be replaced before real go-live, but the current preflight and rehearsal path do not reject those placeholders. That means the repository can still produce a misleading production `GO` signal while insecure default values remain in the evaluated production configuration.

## What Changes

- Add explicit production secrets-hygiene validation to the supported preflight and rehearsal path so placeholder/default secrets cannot pass as production-ready configuration.
- Define the accepted blocked-value contract for production credentials and secrets used by the Compose env file and backend runtime configuration.
- Fail production rehearsal and go-live-oriented validation early when secret values remain at documented placeholder defaults.
- Document the secrets-hygiene gate as part of the release-confidence chain so production `GO` requires both current-commit evidence and non-placeholder production configuration.

## Capabilities

### New Capabilities
- `production-secrets-hygiene`: Validation and policy for blocking placeholder/default production secrets from passing supported deployment and rehearsal workflows.

### Modified Capabilities
- `deployment-and-cutover-operations`: Tighten preflight and cutover requirements so production bring-up and rehearsal reject placeholder/default credentials before stack startup.
- `production-rehearsal-hardening`: Extend trustworthy production rehearsal requirements so configuration security hygiene is part of the `GO`/`NO-GO` contract.

## Impact

- Affected scripts: `scripts/compose-preflight.sh`, `scripts/cutover-rehearsal.sh`, and any supporting validation helpers used by production bring-up.
- Affected configuration surfaces: `deploy/env/production.env`, related deployment defaults documentation, and any supported production secret inputs consumed by the Compose stack.
- Affected documentation: release/deployment docs that define preflight, rehearsal, and production override expectations.
- Affected verification: tests or self-checks that prove placeholder secrets are rejected while valid production overrides still pass.
