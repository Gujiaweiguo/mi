## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. Contributor-facing documentation SHALL point to live canonical references for the CI evidence contract.

#### Scenario: CI evidence contract documentation points to current sources
- **WHEN** contributors review repository documentation for CI-ready evidence requirements
- **THEN** the documentation SHALL reference current OpenSpec requirements and current verification-contract documentation rather than removed legacy paths

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. Contributor-facing documentation SHALL provide a standalone explanation of the canonical evidence contract and archive-specific requirements.

#### Scenario: Archive evidence contract is documented in standalone form
- **WHEN** contributors or release operators need to understand required evidence fields and invariants
- **THEN** the repository SHALL provide a standalone evidence-contract document that explains canonical fields, invariants, and the CI vs archive gate distinction without relying on removed change-local documents
