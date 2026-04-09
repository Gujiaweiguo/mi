## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide a repeatable self-check that confirms the canonical machine-readable evidence schema still parses and still validates representative CI evidence examples or fixtures.

#### Scenario: CI schema self-check validates representative examples
- **WHEN** maintainers run the repository schema self-check workflow
- **THEN** it SHALL confirm that the canonical evidence schema artifact parses successfully and validates representative CI-style evidence samples

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL include representative archive/e2e evidence in schema self-check coverage.

#### Scenario: Archive schema self-check covers e2e-shaped evidence
- **WHEN** maintainers run the repository schema self-check workflow
- **THEN** it SHALL validate at least one archive/e2e-style evidence sample against the canonical schema so e2e-specific structural expectations remain protected
