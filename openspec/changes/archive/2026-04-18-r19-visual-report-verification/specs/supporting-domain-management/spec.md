## MODIFIED Requirements

### Requirement: Supporting domain management SHALL include first-release visual report acceptance where specified
The system SHALL support first-release supporting-domain outputs and visual analyses that are explicitly listed in the frozen report inventory and acceptance matrix.

#### Scenario: R19 visual acceptance is backed by executable verification
- **WHEN** `R19` is verified for first release
- **THEN** verification SHALL prove both the visual presentation semantics and the correctness of the mapping between visual objects and underlying shop or unit data

#### Scenario: R19 filters and export are acceptance-covered
- **WHEN** `R19` is verified for first release
- **THEN** verification SHALL cover store/floor/area filtering and the associated structured export path used by operators
