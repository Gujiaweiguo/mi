## ADDED Requirements

### Requirement: The system SHALL provide trustworthy production-topology rehearsal validation
The release workflow SHALL treat production-topology cutover rehearsal as an explicit validation surface, and SHALL require that rehearsal outputs reflect the real supported production compose topology, current evaluated commit, and runtime assumptions needed for a credible GO/NO-GO decision.

#### Scenario: Production rehearsal runs against supported production topology
- **WHEN** an operator runs the supported cutover rehearsal command for the production environment
- **THEN** the workflow SHALL validate and execute the production compose topology and SHALL emit machine-readable rehearsal artifacts that identify the evaluated commit SHA and production environment

#### Scenario: Runtime contamination blocks rehearsal before destructive steps
- **WHEN** rehearsal preflight detects runtime paths or mounted data baselines that violate clean-start rehearsal assumptions
- **THEN** the rehearsal SHALL fail fast with a NO-GO result and SHALL NOT continue to bootstrap, smoke, backup, or restore steps

#### Scenario: Runtime mount write assumptions are validated as part of rehearsal readiness
- **WHEN** the rehearsal validates production runtime mounts and container execution assumptions
- **THEN** the workflow SHALL verify required writable runtime paths under supported container behavior and SHALL fail fast if those assumptions are not satisfied

### Requirement: The system SHALL keep production rehearsal evidence auditable and release-decisive
Production rehearsal outcomes SHALL remain machine-readable and directly usable for release decisions, and release readiness SHALL depend on current-commit evidence consistency between verification gates and production rehearsal outputs.

#### Scenario: Missing production rehearsal result blocks release readiness
- **WHEN** current-commit release evaluation is attempted without a production-environment rehearsal result artifact
- **THEN** the release workflow SHALL treat the commit as not go-live ready

#### Scenario: Current-commit rehearsal evidence must align with commit-scoped verification evidence
- **WHEN** current-commit rehearsal output exists but required verification evidence is missing, stale, malformed, or failed for the same commit SHA
- **THEN** release readiness evaluation SHALL reject the rehearsal as insufficient and SHALL report a NO-GO decision
