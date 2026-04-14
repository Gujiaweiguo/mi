## MODIFIED Requirements

### Requirement: The system SHALL support first-release non-membership supporting domains
The first release SHALL provide the supporting domain capabilities required for operations outside the membership module, including BaseInfo/admin master data, Shop, Sell, RentableArea, workflow administration, payment and receivable operations, budget/prospect administration, and any explicitly frozen Generalize outputs. The Sell-domain operating surface SHALL support both single-record maintenance and operator-facing batch ingestion for daily sales and customer traffic so downstream reports can be refreshed without direct database edits. Supporting master-data administration SHALL provide supported lifecycle maintenance for customer and brand records so operators can create, update, and manage those records at operational scale without direct database edits. Frontend refactors to the supporting-domain administration surface SHALL preserve these existing behaviors and SHALL NOT change the underlying business capabilities or API contracts merely to improve maintainability.

#### Scenario: Supporting master data can be used in operations
- **WHEN** an operator maintains required shop or rentable-area master data
- **THEN** the updated data SHALL become selectable in downstream Lease and billing operations without direct database edits

#### Scenario: Operators can ingest daily sales in batches
- **WHEN** an operator prepares a valid daily sales import workbook for first-release stores and units
- **THEN** the Sell-domain workflow SHALL accept the batch through the supported UI and persist the imported sales records for downstream reporting use

#### Scenario: Operators can ingest customer traffic in batches
- **WHEN** an operator prepares a valid customer traffic import workbook for first-release stores
- **THEN** the Sell-domain workflow SHALL accept the batch through the supported UI and persist the imported traffic records for downstream reporting use

#### Scenario: Batch ingestion does not remove single-record fallback
- **WHEN** an operator needs to correct or enter an isolated sales or traffic record outside a prepared workbook
- **THEN** the system SHALL continue to support single-record Sell-domain maintenance alongside the batch import path

#### Scenario: Operators can record payments against approved financial documents
- **WHEN** an operator selects an approved bill or invoice with outstanding receivable balance and enters a valid payment amount
- **THEN** the supporting-domain workflow SHALL record the payment, update the displayed outstanding balance, and preserve an auditable payment history for the affected receivable

#### Scenario: Operators can review outstanding receivable status
- **WHEN** an operator queries financial operations for a customer, department, or due-date window
- **THEN** the system SHALL expose the current outstanding receivable balance, due-date context, and settlement status needed to support aging and arrears follow-up

#### Scenario: Operators can maintain customer and brand records through a supported lifecycle
- **WHEN** an operator creates, edits, or retires a customer or brand record through the supported administration surface
- **THEN** the system SHALL persist the change through supported application workflows and SHALL not require direct database edits to keep master data operationally correct

#### Scenario: Supporting master-data administration remains usable at list scale
- **WHEN** customer or brand records grow beyond a single-page manual review set
- **THEN** the supported administration surface SHALL provide list navigation and retrieval behavior that allows operators to locate and maintain records without loading the entire dataset into one unbounded view

#### Scenario: Frontend maintainability refactor preserves master-data behavior
- **WHEN** the master-data administration UI is refactored for maintainability
- **THEN** existing customer, brand, budget, and prospect workflows SHALL remain behaviorally equivalent for operators and SHALL continue using the same supported backend contracts
