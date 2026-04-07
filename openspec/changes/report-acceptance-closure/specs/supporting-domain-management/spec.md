## MODIFIED Requirements

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
