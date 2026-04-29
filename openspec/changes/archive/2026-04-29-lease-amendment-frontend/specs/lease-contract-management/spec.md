## ADDED Requirements

### Requirement: Lease amendment SHALL be operator-reachable from the first-release frontend
The system SHALL expose amendment initiation for amendment-eligible Lease contracts from the authenticated frontend and SHALL route the operator into an amendment drafting flow that preserves the existing contract data needed for billing-effective edits.

#### Scenario: Operator starts an amendment from an eligible contract
- **WHEN** an operator opens an amendment-eligible Lease contract in the frontend detail view and selects the amendment action
- **THEN** the system SHALL route the operator into an amendment drafting flow that is bound to the selected source contract

#### Scenario: Amendment draft is prefilled from the source contract
- **WHEN** the amendment drafting flow loads successfully
- **THEN** the system SHALL prefill the amendment form with the source contract fields, including subtype-specific data, units, and terms, before the operator submits changes

### Requirement: Lease amendment submission SHALL reuse the existing amendment contract flow
The frontend SHALL submit amendment drafts through the existing lease amendment API contract and SHALL return the operator to the resulting amended contract after successful creation.

#### Scenario: Amendment draft submits through the amendment endpoint
- **WHEN** an operator submits a valid amendment draft from the frontend
- **THEN** the system SHALL call the lease amendment API for the source contract instead of the new-lease creation endpoint

#### Scenario: Successful amendment opens the amended contract
- **WHEN** the amendment API returns a created amended contract successfully
- **THEN** the frontend SHALL navigate the operator to the resulting amended contract detail page so downstream submission and approval can continue from that new contract
