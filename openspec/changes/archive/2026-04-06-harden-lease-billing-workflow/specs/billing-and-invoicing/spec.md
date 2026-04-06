## MODIFIED Requirements

### Requirement: The system SHALL generate billable charges from approved Lease state
The billing subsystem SHALL convert approved, billing-effective Lease contracts and supported rule definitions into deterministic charge lines for an accounting window. Charge generation SHALL ignore Lease records that are not yet billable under the Lease lifecycle contract, and reruns SHALL remain duplicate-safe for already generated accounting windows.

#### Scenario: Monthly charge generation succeeds
- **WHEN** a valid approved Lease contract is processed for a billing period
- **THEN** the system SHALL generate the expected charge lines once without creating duplicates on rerun

#### Scenario: Non-billable Lease state is excluded
- **WHEN** charge generation evaluates a Lease contract that has not reached the required approved or billing-effective state
- **THEN** the system SHALL skip that contract instead of generating billable charge lines

### Requirement: The system SHALL manage bill and invoice lifecycle states
The first release SHALL support creation, numbering, approval, rejection, cancellation, and allowed adjustments for bill and invoice documents required by the business flow. Document lifecycle actions SHALL validate the current state before mutation, and replayed submit, approve, cancel, or equivalent actions SHALL remain auditable while avoiding duplicate side effects or duplicate document state transitions.

#### Scenario: Approved invoice is numbered and auditable
- **WHEN** a valid invoice is created from generated charges and approved
- **THEN** the system SHALL assign a deterministic document number, persist audit history, and expose the approved document for downstream output generation

#### Scenario: Duplicate invoice submission is safe
- **WHEN** a submit-for-approval action is replayed for an invoice that has already been submitted or approved
- **THEN** the system SHALL preserve the existing document state and SHALL NOT create duplicate workflow or numbering side effects

#### Scenario: Invalid lifecycle action is rejected
- **WHEN** an operator attempts a bill or invoice lifecycle action that is not allowed from the document's current state
- **THEN** the system SHALL reject the action and SHALL preserve the existing document state
