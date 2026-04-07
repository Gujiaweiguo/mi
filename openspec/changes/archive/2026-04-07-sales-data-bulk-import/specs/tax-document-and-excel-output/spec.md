## MODIFIED Requirements

### Requirement: The system SHALL support mandatory Excel import and export flows
The first release SHALL support the Excel import and export flows frozen in the output catalog, with validation errors surfaced before invalid data is committed. This SHALL include downloadable templates and batch import handling for daily shop sales and customer traffic so report-driving operational datasets can be refreshed at business scale.

#### Scenario: Invalid import is rejected
- **WHEN** an Excel import contains duplicate business keys or malformed required values
- **THEN** the system SHALL reject the batch and SHALL provide row-level validation feedback without committing partial data by default

#### Scenario: Sales template download includes import guidance
- **WHEN** an operator downloads the daily sales or customer traffic import template
- **THEN** the workbook SHALL include the required columns and enough reference data or labels to prepare a valid batch without reverse-engineering internal IDs from the database

#### Scenario: Daily sales batch import upserts deterministically
- **WHEN** an operator uploads a valid daily sales workbook containing new rows and rows that already exist for the same store, unit, and sale date
- **THEN** the system SHALL apply the batch deterministically, upsert matching business keys idempotently, and report the final imported row count

#### Scenario: Customer traffic batch import upserts deterministically
- **WHEN** an operator uploads a valid customer traffic workbook containing new rows and rows that already exist for the same store and traffic date
- **THEN** the system SHALL apply the batch deterministically, upsert matching business keys idempotently, and report the final imported row count

#### Scenario: Reference validation fails before trusted write
- **WHEN** a sales or traffic import references an unknown store, unit, or other required reference value
- **THEN** the system SHALL surface row-level diagnostics for the invalid references and SHALL NOT commit the batch as trusted operational data
