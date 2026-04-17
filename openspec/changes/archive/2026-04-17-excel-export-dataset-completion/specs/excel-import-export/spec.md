## ADDED Requirements

### Requirement: The system SHALL support mandatory Excel import and export flows
The first release SHALL support the Excel import and export flows frozen in the output catalog, with validation errors surfaced before invalid data is committed. This SHALL include downloadable templates and batch import handling for daily shop sales and customer traffic so report-driving operational datasets can be refreshed at business scale. The export surface SHALL cover all four operational datasets: `invoices`, `billing_charges`, `lease_contracts`, and `unit_data`.

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

#### Scenario: Lease contracts export produces a valid workbook
- **WHEN** an operator exports the `lease_contracts` dataset
- **THEN** the system SHALL generate an Excel workbook with columns `lease_no`, `tenant_name`, `store_code`, `department_code`, `start_date`, `end_date`, `status`, `effective_version` and SHALL resolve foreign-key IDs to codes via JOINs against `stores` and `departments`

#### Scenario: Unit data export produces a valid workbook
- **WHEN** an operator exports the `unit_data` dataset
- **THEN** the system SHALL generate an Excel workbook with columns `code`, `building_code`, `floor_code`, `location_code`, `area_code`, `unit_type_code`, `floor_area`, `use_area`, `rent_area`, `is_rentable`, `status` and SHALL resolve foreign-key IDs to codes via JOINs against `buildings`, `floors`, `locations`, `areas`, and `unit_types`

#### Scenario: Invoices export is visible in the frontend dataset dropdown
- **WHEN** an operator opens the Excel export section of the Excel I/O page
- **THEN** the dataset dropdown SHALL include an `invoices` option alongside `billing_charges`, `lease_contracts`, and `unit_data`, and selecting it SHALL produce a valid invoice export workbook
