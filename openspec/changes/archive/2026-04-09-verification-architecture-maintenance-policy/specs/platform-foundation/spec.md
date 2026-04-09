## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide architecture documentation that explicitly describes responsibility boundaries between evidence producers, schema-driven structure validation, contextual gate validation, and CI-ready entrypoint orchestration. The repository SHALL also define maintenance policy expectations for verification-stack changes so that CI gate behavior, schema validation, and related documentation remain synchronized.

#### Scenario: Verification architecture responsibilities are documented for CI path
- **WHEN** contributors need to modify CI-related verification behavior
- **THEN** the repository SHALL provide architecture documentation that identifies where producer logic, schema rules, validator contextual checks, and CI entrypoint wiring each belong

#### Scenario: CI gate blocks when required evidence is missing
- **WHEN** unit or integration evidence for the evaluated commit is missing
- **THEN** CI-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: CI gate blocks stale evidence after commit changes
- **WHEN** evidence files exist but `commit_sha` does not match the currently evaluated commit
- **THEN** CI-ready evaluation SHALL reject the evidence as stale and SHALL fail the gate

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL provide architecture documentation that explains how archive evaluation reuses shared verification components while preserving archive-specific requirements. The repository SHALL also define maintenance policy expectations for archive-path verification changes so regression checks and documentation updates are performed together.

#### Scenario: Verification architecture documents archive-specific concerns
- **WHEN** contributors need to reason about archive-ready behavior
- **THEN** the architecture documentation SHALL explain shared versus archive-specific verification responsibilities without requiring reverse engineering from scripts alone

#### Scenario: Archive gate blocks when required evidence is missing
- **WHEN** any required archive evidence (`unit`, `integration`, or `e2e`) for the evaluated commit is missing
- **THEN** archive-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: Archive gate blocks stale evidence after commit changes
- **WHEN** archive evidence exists but references a different commit SHA than the evaluated commit
- **THEN** archive-ready evaluation SHALL reject the stale evidence and SHALL fail the gate
