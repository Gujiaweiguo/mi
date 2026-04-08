## Purpose

TBD: Canonical supporting-domain management spec for the replacement MI system.

## Requirements

### Requirement: The system SHALL support first-release non-membership supporting domains
The first release SHALL provide the supporting domain capabilities required for operations outside the membership module, including BaseInfo/admin master data, Shop, Sell, RentableArea, workflow administration, payment and receivable operations, and any explicitly frozen Generalize outputs. The Sell-domain operating surface SHALL support both single-record maintenance and operator-facing batch ingestion for daily sales and customer traffic so downstream reports can be refreshed without direct database edits.

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

### Requirement: The system SHALL limit Generalize reporting to documented first-release reports
The first release SHALL include only the `Generalize` reports enumerated in the archived first-release report inventory, which is derived from `阳光商业MI.net系统设计.doc`.

#### Scenario: Non-documented report is out of scope
- **WHEN** a report request is raised for a `Generalize` report not listed in the archived first-release report inventory
- **THEN** that report SHALL be treated as out of first-release scope unless the change artifacts are updated

### Requirement: The system SHALL implement the frozen first-release Generalize report inventory
The first release SHALL implement the Generalize report inventory frozen for this change, including at minimum report IDs `R01` through `R19` as enumerated in the archived first-release report inventory. Report acceptance SHALL use the stable `Report ID` as the canonical identifier even when business-facing labels evolve.

#### Scenario: Frozen report inventory is complete
- **WHEN** first-release reporting scope is reviewed before implementation or acceptance
- **THEN** only the reports listed in the frozen `R01-R19` inventory SHALL be considered required for Generalize delivery

#### Scenario: Report IDs remain the acceptance anchor
- **WHEN** a report label or UI wording differs from the normalized archived name
- **THEN** acceptance SHALL still be evaluated against the matching `Report ID`, business meaning, and frozen matrix contract

### Requirement: The system SHALL satisfy the frozen Generalize report acceptance matrix
Each first-release Generalize report SHALL satisfy the minimum field set, filters, output form, and acceptance checks defined in the frozen report acceptance matrix. Acceptance closure SHALL verify reports by family and SHALL record unresolved gaps as explicit fix-now items or documented non-go-live exceptions instead of leaving report status implicit.

#### Scenario: Report acceptance baseline is enforced
- **WHEN** a Generalize report is implemented or accepted
- **THEN** its delivered behavior SHALL meet the matrix entry for the matching `Report ID`

#### Scenario: Tabular reports provide the frozen minimum output form
- **WHEN** a first-release Generalize report other than `R19` is accepted
- **THEN** it SHALL support on-screen query results and `.xlsx` export at minimum

#### Scenario: Cross-report reconciliation is verified where required
- **WHEN** a report family includes matrix checks that require totals or bucket sums to reconcile across summary and detail views
- **THEN** acceptance SHALL verify those cross-report or cross-column reconciliations explicitly before closure

#### Scenario: Incomplete acceptance is documented explicitly
- **WHEN** a report fails a frozen matrix check during closure review
- **THEN** the system team SHALL classify that gap as a required fix or a documented non-go-live exception before the acceptance-closure change can be considered complete

#### Scenario: Visual report acceptance is output-specific
- **WHEN** `R19` is accepted for first release
- **THEN** acceptance SHALL verify both the visual presentation semantics and the correctness of the mapping between visual objects and underlying shop or unit data

### Requirement: The system SHALL exclude membership from first release scope
The first release SHALL exclude the membership (`Associator`) module and SHALL NOT expose membership routes, menus, or operational flows.

#### Scenario: Membership is not available
- **WHEN** an operator navigates the first-release application
- **THEN** membership capabilities SHALL be absent or explicitly blocked as out of scope
