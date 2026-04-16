## Purpose

Canonical spec for localized operator-facing Generalize report column and export-header output.

## Requirements

### Requirement: The system SHALL localize operator-facing Generalize report column output
The system SHALL emit localized operator-facing column labels for the frozen `R01-R19` Generalize reporting inventory so backend-provided report metadata matches first-release Chinese acceptance terminology instead of exposing hard-coded English headers by default.

#### Scenario: Query responses expose localized report column labels
- **WHEN** an operator queries any supported Generalize report in the frozen `R01-R19` inventory
- **THEN** the returned `columns[].label` values SHALL use the accepted operator-facing localized terminology for that report instead of hard-coded English labels

#### Scenario: Shared report label patterns stay terminology-consistent
- **WHEN** multiple Generalize reports reuse shared concepts such as store, department, brand, aging buckets, or month/day headers
- **THEN** the emitted localized labels SHALL remain terminology-consistent across those reports for the same business concept

### Requirement: The system SHALL localize exported Generalize report workbook headers
The system SHALL use the same localized operator-facing terminology for exported workbook header rows as it uses for on-screen Generalize report column output.

#### Scenario: Export headers match localized query headers
- **WHEN** an operator exports a supported Generalize report workbook
- **THEN** the workbook header row SHALL use the localized report column labels defined for that report's operator-facing output

#### Scenario: Visual report tabular export uses localized headers
- **WHEN** an operator exports the tabular artifact associated with `R19`
- **THEN** the exported header labels SHALL use the localized operator-facing terminology for the visual shop analysis output fields
