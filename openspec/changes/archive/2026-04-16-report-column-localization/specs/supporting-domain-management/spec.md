## MODIFIED Requirements

### Requirement: The system SHALL satisfy the frozen Generalize report acceptance matrix
Each first-release Generalize report SHALL satisfy the minimum field set, filters, output form, and acceptance checks defined in the frozen report acceptance matrix. Acceptance closure SHALL verify reports by family and SHALL record unresolved gaps as explicit fix-now items or documented non-go-live exceptions instead of leaving report status implicit. Operator-facing query output and exported headers for the frozen `R01-R19` inventory SHALL use accepted localized terminology rather than hard-coded English report labels.

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

#### Scenario: Operator-facing report headers use accepted localized terminology
- **WHEN** a first-release Generalize report query or export is reviewed against the frozen acceptance surface
- **THEN** the operator-facing column labels and exported headers SHALL use the accepted localized terminology for that report instead of hard-coded English field labels
