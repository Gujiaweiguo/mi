## MODIFIED Requirements

### Requirement: The system SHALL localize shared shell and common user-facing frontend surfaces
The frontend SHALL move shared shell copy, navigation labels, login text, global alerts, and other common user-facing application text from hard-coded strings to locale-managed message resources. The frontend SHALL also extend that localization coverage to the deferred second-wave routed views, including billing and invoice pages, lease detail/create pages, reporting and visual-analysis pages, tax export and print-preview pages, and the remaining admin-console views in scope for this change.

#### Scenario: Shared shell and login surfaces follow the active locale
- **WHEN** the active locale changes or the application loads with a resolved locale
- **THEN** the shared shell, permission-driven navigation labels, login page, and common frontend user messages SHALL render text from locale-managed message resources for that locale

#### Scenario: Deferred second-wave routed views follow the active locale
- **WHEN** an operator opens a second-wave deferred routed view after this change is implemented and the application locale is `zh-CN` or `en-US`
- **THEN** the page-section copy, form labels, filter labels, table labels, action labels, feedback messages, and other frontend-authored UI text on that view SHALL render from locale-managed message resources for the active locale
