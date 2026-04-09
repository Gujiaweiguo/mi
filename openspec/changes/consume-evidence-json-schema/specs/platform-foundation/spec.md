## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. When a machine-readable schema artifact exists, CI evidence structural validation SHALL reuse that shared schema definition rather than duplicating the same structural rules independently in the validator.

#### Scenario: CI structural validation reuses the shared schema
- **WHEN** CI evidence is structurally validated for gate evaluation
- **THEN** the validator SHALL consume the shared machine-readable evidence schema artifact for structural checks before applying contextual gate rules

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. Archive validation SHALL preserve contextual checks that cannot be expressed solely by the shared schema artifact.

#### Scenario: Archive contextual rules remain explicit
- **WHEN** archive evidence passes shared-schema structural validation
- **THEN** the validator SHALL still enforce context-dependent archive rules such as commit-SHA matching, file-type matching, pass-status acceptance, stats arithmetic consistency, and e2e artifact non-emptiness
