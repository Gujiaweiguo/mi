## MODIFIED Requirements

### Requirement: The system SHALL support Lease contract lifecycle management
The first release SHALL support creation, submission, approval, activation, amendment, termination, and querying of Lease contracts needed by the operational business flow. Lifecycle commands SHALL validate the current Lease state before mutating the contract, and duplicate submissions or other replayed actions SHALL preserve the existing valid state instead of creating duplicate downstream effects.

#### Scenario: Contract reaches active state
- **WHEN** an operator creates a Lease contract and the required approvals complete successfully
- **THEN** the contract SHALL enter an active state and SHALL become eligible for downstream billing

#### Scenario: Duplicate submission is safe
- **WHEN** a submit action is replayed for a Lease contract that is already pending approval, approved, or otherwise past the initial submit step
- **THEN** the system SHALL preserve the existing lifecycle state and SHALL NOT create a duplicate workflow side effect

#### Scenario: Invalid termination is blocked
- **WHEN** an operator attempts to terminate a Lease contract from a state that is not allowed to terminate under first-release rules
- **THEN** the system SHALL reject the mutation and SHALL preserve the existing contract state

### Requirement: The system SHALL preserve billing-relevant contract state changes
Amendments, terminations, and other supported Lease lifecycle changes SHALL update the billing-effective contract state used by downstream charge generation and invoice flows. Lease records that have not reached the required approved or billing-effective lifecycle point SHALL NOT be treated as eligible billing inputs.

#### Scenario: Approved amendment changes billing inputs
- **WHEN** an approved contract amendment updates a billing-effective field
- **THEN** future charge generation SHALL use the amended contract values

#### Scenario: Pending approval contract is not billable
- **WHEN** charge generation evaluates a Lease contract that is still pending approval or otherwise not billing-effective
- **THEN** the system SHALL exclude that contract from downstream billing generation
