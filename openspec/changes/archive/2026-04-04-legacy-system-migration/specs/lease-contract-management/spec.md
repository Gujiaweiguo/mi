## ADDED Requirements

### Requirement: The system SHALL support Lease contract lifecycle management
The first release SHALL support creation, submission, approval, activation, amendment, termination, and querying of Lease contracts needed by the operational business flow.

#### Scenario: Contract reaches active state
- **WHEN** an operator creates a Lease contract and the required approvals complete successfully
- **THEN** the contract SHALL enter an active state and SHALL become eligible for downstream billing

### Requirement: The system SHALL preserve billing-relevant contract state changes
Amendments, terminations, and other supported Lease lifecycle changes SHALL update the billing-effective contract state used by downstream charge generation and invoice flows.

#### Scenario: Approved amendment changes billing inputs
- **WHEN** an approved contract amendment updates a billing-effective field
- **THEN** future charge generation SHALL use the amended contract values
