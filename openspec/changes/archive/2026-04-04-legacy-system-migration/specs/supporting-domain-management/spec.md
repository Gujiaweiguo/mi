## ADDED Requirements

### Requirement: The system SHALL support first-release non-membership supporting domains
The first release SHALL provide the supporting domain capabilities required for operations outside the membership module, including BaseInfo/admin master data, Shop, Sell, RentableArea, workflow administration, and any explicitly frozen Generalize outputs.

#### Scenario: Supporting master data can be used in operations
- **WHEN** an operator maintains required shop or rentable-area master data
- **THEN** the updated data SHALL become selectable in downstream Lease and billing operations without direct database edits

### Requirement: The system SHALL limit Generalize reporting to documented first-release reports
The first release SHALL include only the `Generalize` reports enumerated in `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`, which is derived from `阳光商业MI.net系统设计.doc`.

#### Scenario: Non-documented report is out of scope
- **WHEN** a report request is raised for a `Generalize` report not listed in `report-inventory.md`
- **THEN** that report SHALL be treated as out of first-release scope unless the change artifacts are updated

### Requirement: The system SHALL implement the frozen first-release Generalize report inventory
The first release SHALL implement the Generalize report inventory frozen for this change, including at minimum report IDs `R01` through `R19` as enumerated in `report-inventory.md`.

#### Scenario: Frozen report inventory is complete
- **WHEN** first-release reporting scope is reviewed before implementation or acceptance
- **THEN** only the reports listed in `report-inventory.md` SHALL be considered required for Generalize delivery

### Requirement: The system SHALL satisfy the frozen Generalize report acceptance matrix
Each first-release Generalize report SHALL satisfy the minimum field set, filters, output form, and acceptance checks defined in `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`.

#### Scenario: Report acceptance baseline is enforced
- **WHEN** a Generalize report is implemented or accepted
- **THEN** its delivered behavior SHALL meet the matrix entry for the matching `Report ID`

### Requirement: The system SHALL exclude membership from first release scope
The first release SHALL exclude the membership (`Associator`) module and SHALL NOT expose membership routes, menus, or operational flows.

#### Scenario: Membership is not available
- **WHEN** an operator navigates the first-release application
- **THEN** membership capabilities SHALL be absent or explicitly blocked as out of scope
