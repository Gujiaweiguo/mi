## MODIFIED Requirements

### Requirement: Supporting-domain and frozen-report acceptance SHALL remain evidence-backed at release time
The system SHALL keep supporting-domain first-release acceptance grounded in explicit current-head evidence for frozen outputs and visual analyses, especially where operator-facing acceptance depends on rendered presentation rather than only backend data semantics.

#### Scenario: R19 visual acceptance is backed by UI-level current-head evidence
- **WHEN** `R19` is treated as accepted for release
- **THEN** the evaluated current HEAD SHALL have executable UI-level evidence proving the rendered floor visual, markers, legend semantics, and selected-unit detail behavior

#### Scenario: Frozen outputs are not considered accepted from implementation presence alone
- **WHEN** first-release outputs are reviewed for release closure
- **THEN** the project SHALL require explicit evidence mapping for the frozen outputs rather than inferring acceptance from implementation presence alone
