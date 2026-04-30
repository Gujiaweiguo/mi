## ADDED Requirements

### Requirement: Lease detail SHALL expose downstream financial progression
The frontend lease detail view SHALL expose the downstream business progression linked to the current lease contract so operators can review the lease-to-cash state without manually searching each financial module. The view SHALL show downstream overtime, billing charge, invoice, and receivable state that belongs to the selected lease contract, and SHALL surface quick-entry links to supported downstream detail or review flows.

#### Scenario: Operator reviews a lease and sees downstream chain state
- **WHEN** an operator opens a supported lease contract detail view
- **THEN** the system SHALL display downstream financial records linked to that lease contract, including overtime bills, billing charges, invoices, and receivables

#### Scenario: Lease detail preserves context while linking downstream
- **WHEN** a downstream record shown in the lease detail view supports navigation to a more detailed review surface
- **THEN** the system SHALL provide a quick-entry link that opens the supported downstream destination for that record without requiring the operator to manually re-enter the lease context

#### Scenario: Missing downstream records remain operator-understandable
- **WHEN** a lease contract has not yet produced one or more downstream financial record types
- **THEN** the system SHALL render an explicit empty or not-yet-generated state instead of omitting that downstream section silently
