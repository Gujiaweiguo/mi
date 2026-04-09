## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide a repeatable self-check that confirms the canonical machine-readable evidence schema still parses and still validates representative CI evidence examples or fixtures. The default CI-ready repository entrypoint SHALL execute that schema self-check before evaluating commit-scoped CI evidence readiness.

#### Scenario: CI-ready entrypoint runs schema self-check first
- **WHEN** contributors run the repository CI-ready entrypoint
- **THEN** it SHALL execute the schema self-check workflow before running commit-scoped CI evidence gate evaluation
