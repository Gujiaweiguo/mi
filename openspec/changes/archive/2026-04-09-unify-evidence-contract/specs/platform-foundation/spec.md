## MODIFIED Requirements

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated.

#### Scenario: CI evidence must satisfy canonical schema
- **WHEN** CI readiness is evaluated for the current commit
- **THEN** required `unit` and `integration` evidence SHALL include canonical top-level fields, canonical `source` fields, canonical `stats` fields, and valid timestamp/stat invariants

#### Scenario: Schema-drift CI evidence is invalid
- **WHEN** `unit` or `integration` evidence for the evaluated commit uses non-canonical field names, incompatible value types, or inconsistent stat/timestamp values
- **THEN** that evidence SHALL be treated as malformed and SHALL NOT satisfy CI readiness

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references.

#### Scenario: Archive evidence must satisfy canonical schema
- **WHEN** archive readiness is evaluated for the current commit
- **THEN** required `unit`, `integration`, and `e2e` evidence SHALL satisfy canonical field/type constraints and stat/timestamp invariants

#### Scenario: E2E evidence requires artifact references
- **WHEN** `e2e` evidence exists for the evaluated commit but omits artifact references or provides invalid artifact entries
- **THEN** that `e2e` evidence SHALL be treated as malformed and SHALL NOT satisfy archive readiness

#### Scenario: Schema-drift archive evidence is invalid
- **WHEN** required archive evidence uses incompatible schema structure even if `status` is `passed`
- **THEN** that evidence SHALL be rejected and the commit SHALL NOT be considered archive-ready
