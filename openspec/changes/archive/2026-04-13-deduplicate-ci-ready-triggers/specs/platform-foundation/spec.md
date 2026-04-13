## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide architecture documentation that explicitly describes responsibility boundaries between evidence producers, schema-driven structure validation, contextual gate validation, and CI-ready entrypoint orchestration. The repository SHALL also define maintenance policy expectations for verification-stack changes so that CI gate behavior, schema validation, and related documentation remain synchronized. The CI-ready workflow trigger policy SHALL avoid creating duplicate runs for the same PR update while preserving one authoritative validation result for the evaluated commit.

#### Scenario: Verification architecture responsibilities are documented for CI path
- **WHEN** contributors need to modify CI-related verification behavior
- **THEN** the repository SHALL provide architecture documentation that identifies where producer logic, schema rules, validator contextual checks, and CI entrypoint wiring each belong

#### Scenario: CI gate blocks when required evidence is missing
- **WHEN** unit or integration evidence for the evaluated commit is missing
- **THEN** CI-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: CI gate blocks stale evidence after commit changes
- **WHEN** evidence files exist but `commit_sha` does not match the currently evaluated commit
- **THEN** CI-ready evaluation SHALL reject the evidence as stale and SHALL fail the gate

#### Scenario: A pull request update produces one CI-ready run
- **WHEN** a contributor updates a branch that already has an open pull request
- **THEN** the repository SHALL produce one authoritative `ci-ready` workflow run for that PR update instead of duplicate runs with the same validation intent

#### Scenario: Trigger deduplication does not weaken commit-scoped validation
- **WHEN** CI-ready runs for an evaluated commit after trigger deduplication is applied
- **THEN** the run SHALL still validate `unit` and `integration` evidence for the current commit SHA and SHALL fail if required evidence is missing or stale

#### Scenario: Non-PR CI coverage remains intentional
- **WHEN** a branch update occurs outside the PR lifecycle but still matches the repository's intended CI-ready coverage policy
- **THEN** the workflow SHALL either run by design or be explicitly excluded by documented trigger rules, rather than being left ambiguous by overlapping events
