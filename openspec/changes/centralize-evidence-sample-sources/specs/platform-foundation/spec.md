## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide a repeatable self-check that confirms the canonical machine-readable evidence schema still parses and still validates representative CI evidence examples or fixtures. Representative CI evidence samples used for documentation and schema self-checks SHALL resolve to a centralized canonical repository source.

#### Scenario: CI evidence references use canonical sample source
- **WHEN** contributors review representative CI evidence examples or maintainers run schema self-checks
- **THEN** both documentation and self-check tooling SHALL point to the same canonical repository-owned CI evidence sample source

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL include representative archive/e2e evidence in schema self-check coverage. Representative archive/e2e evidence samples used for documentation and schema self-checks SHALL resolve to a centralized canonical repository source.

#### Scenario: Archive evidence references use canonical sample source
- **WHEN** contributors review representative archive/e2e evidence examples or maintainers run schema self-checks
- **THEN** both documentation and self-check tooling SHALL point to the same canonical repository-owned archive/e2e evidence sample source
