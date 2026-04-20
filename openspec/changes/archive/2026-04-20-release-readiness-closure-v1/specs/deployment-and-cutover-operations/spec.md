## MODIFIED Requirements

### Requirement: The system SHALL prove go-live readiness through current-head production rehearsal
The supported release workflow SHALL prove go-live readiness using the documented production cutover rehearsal against the exact current release commit. Rehearsal outcomes SHALL be tied to the evaluated commit SHA and SHALL be treated as blocking when preflight, archive-ready validation, bootstrap, fresh-start verification, smoke validation, backup, or restore steps fail.

#### Scenario: Production rehearsal is run for the exact release commit
- **WHEN** the project is evaluated for go-live
- **THEN** the supported production rehearsal SHALL generate a result artifact and log under the current commit SHA for that exact evaluated commit

#### Scenario: Failed rehearsal blocks release
- **WHEN** the production rehearsal result for the evaluated commit reports `NO-GO` or any failing gate
- **THEN** release status SHALL be blocked until the failing gate is closed or explicitly accepted as `NO-GO`

### Requirement: The system SHALL treat production secret and bootstrap readiness as blocking go-live criteria
Production deployment guidance SHALL explicitly reject placeholder secrets for required production credentials and JWT configuration, and SHALL document the bootstrap sources used for go-live initialization. Rehearsal-safe defaults SHALL NOT be interpreted as production-safe deployment values.

#### Scenario: Placeholder production secrets are documented as invalid for go-live
- **WHEN** an operator prepares a production deployment
- **THEN** the documentation SHALL identify placeholder DB and JWT secret values as blocking and invalid for go-live

#### Scenario: Go-live bootstrap sources are documented
- **WHEN** an operator prepares cutover or go-live initialization
- **THEN** the documentation SHALL identify the repository bootstrap sources used to establish the production baseline
