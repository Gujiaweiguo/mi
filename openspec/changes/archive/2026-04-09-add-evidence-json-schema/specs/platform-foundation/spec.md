## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide a machine-readable schema artifact that describes the canonical evidence structure used by CI evidence documentation and tooling.

#### Scenario: Canonical CI evidence structure is available as JSON Schema
- **WHEN** contributors or tooling need the structural definition of canonical `unit` or `integration` evidence
- **THEN** the repository SHALL provide a machine-readable JSON Schema artifact that describes required fields, nested objects, and basic type constraints for the canonical evidence contract

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL explain which archive-evidence rules are structurally represented in schema form versus enforced by gate-context validation logic.

#### Scenario: JSON Schema is linked from evidence contract references
- **WHEN** contributors review canonical evidence contract references for archive readiness
- **THEN** the standalone contract documentation SHALL point to the machine-readable schema artifact and explain that contextual gate checks still rely on executable validation logic
