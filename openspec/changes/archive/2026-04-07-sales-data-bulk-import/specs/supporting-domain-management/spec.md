## MODIFIED Requirements

### Requirement: The system SHALL support first-release non-membership supporting domains
The first release SHALL provide the supporting domain capabilities required for operations outside the membership module, including BaseInfo/admin master data, Shop, Sell, RentableArea, workflow administration, and any explicitly frozen Generalize outputs. The Sell-domain operating surface SHALL support both single-record maintenance and operator-facing batch ingestion for daily sales and customer traffic so downstream reports can be refreshed without direct database edits.

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
