## ADDED Requirements

### Requirement: Lease-linked financial records SHALL be queryable from the originating lease context
The system SHALL support querying billing charges, invoice documents, and receivable summaries by `lease_contract_id` so lease-detail and other lease-context review surfaces can present downstream financial state deterministically from the originating contract. This filter SHALL be optional and SHALL preserve the existing behavior of broader list views when it is not provided.

#### Scenario: Charges and invoices are queried for a specific lease
- **WHEN** a supported frontend or backend consumer requests billing charges or invoice documents with a `lease_contract_id`
- **THEN** the system SHALL return only records linked to that lease contract while preserving normal pagination and response shape

#### Scenario: Receivables are queried for a specific lease
- **WHEN** a supported frontend or backend consumer requests receivable summaries with a `lease_contract_id`
- **THEN** the system SHALL return only receivable records linked to that lease contract while preserving normal pagination and response shape

#### Scenario: Existing receivable list behavior is preserved without lease filter
- **WHEN** the receivable list API is called without a `lease_contract_id`
- **THEN** the system SHALL preserve the pre-existing customer, department, and due-date filtering behavior for broader receivable review flows

### Requirement: Lease detail SHALL surface downstream billing and receivable review state
Supported first-release lease review surfaces SHALL expose enough downstream billing, invoice, and receivable state for operators to understand whether a lease has produced billable charges, invoice documents, and open receivable obligations.

#### Scenario: Lease detail shows downstream billing and receivable progression
- **WHEN** a lease contract has produced downstream billing or receivable records
- **THEN** the frontend SHALL present that downstream state from the lease detail context with enough identifiers, statuses, and dates for operator review

#### Scenario: Lease detail remains usable when downstream retrieval partially fails
- **WHEN** one downstream financial record type cannot be loaded for the lease detail view
- **THEN** the system SHALL preserve the rest of the lease detail view and SHALL show localized error feedback only for the affected downstream section
