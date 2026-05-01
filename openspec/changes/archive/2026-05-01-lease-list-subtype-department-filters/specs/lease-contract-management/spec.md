## ADDED Requirements

### Requirement: Lease list SHALL support subtype and department filtering
The first-release lease contract list view SHALL let operators filter contract queues by lease subtype and department so they can isolate the relevant operational slice without manually scanning mixed contract portfolios. These filters SHALL be optional and SHALL preserve the existing lease list behavior when they are not supplied.

#### Scenario: Operator filters lease queue by subtype
- **WHEN** an operator selects a supported lease subtype in the lease list filters
- **THEN** the system SHALL return only lease contracts of that subtype while preserving the existing response shape and pagination behavior

#### Scenario: Operator filters lease queue by department
- **WHEN** an operator selects a department in the lease list filters
- **THEN** the system SHALL return only lease contracts belonging to that department while preserving the existing response shape and pagination behavior

#### Scenario: Lease list keeps current behavior without new filters
- **WHEN** the lease list is queried without subtype or department filters
- **THEN** the system SHALL preserve the pre-existing lease number, status, store, and pagination behavior for the lease queue
