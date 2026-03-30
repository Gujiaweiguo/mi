## ADDED Requirements

### Requirement: The system SHALL generate billable charges from approved Lease state
The billing subsystem SHALL convert approved Lease contracts and supported rule definitions into deterministic charge lines for an accounting window.

#### Scenario: Monthly charge generation succeeds
- **WHEN** a valid approved Lease contract is processed for a billing period
- **THEN** the system SHALL generate the expected charge lines once without creating duplicates on rerun

### Requirement: The system SHALL manage bill and invoice lifecycle states
The first release SHALL support creation, numbering, approval, rejection, cancellation, and allowed adjustments for bill and invoice documents required by the business flow.

#### Scenario: Approved invoice is numbered and auditable
- **WHEN** a valid invoice is created from generated charges and approved
- **THEN** the system SHALL assign a deterministic document number, persist audit history, and expose the approved document for downstream output generation
