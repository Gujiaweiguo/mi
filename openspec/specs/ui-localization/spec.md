## Purpose

TBD: Canonical UI localization spec for the replacement MI frontend.

## Requirements

### Requirement: The system SHALL default the frontend UI language to Simplified Chinese
The frontend SHALL render operator-facing application copy in Simplified Chinese (`zh-CN`) by default when no explicit language preference has been chosen in the current browser.

#### Scenario: First-time visitor sees Simplified Chinese
- **WHEN** an operator opens the frontend in a browser that has no stored UI language preference for the application
- **THEN** shared shell copy, login copy, and other localized frontend text SHALL render in Simplified Chinese by default

### Requirement: The system SHALL allow runtime switching between Simplified Chinese and English
The frontend SHALL provide a visible runtime language switch that lets the operator change the active UI language between Simplified Chinese (`zh-CN`) and English (`en-US`) without changing backend behavior or requiring a new login.

#### Scenario: Operator switches language at runtime
- **WHEN** an operator changes the active language from Simplified Chinese to English or from English to Simplified Chinese
- **THEN** the frontend SHALL update localized application copy and locale-managed framework text to the selected language during the active session

### Requirement: The system SHALL persist the selected frontend language in browser-local state
The frontend SHALL persist the selected UI language in browser-local state so the active language is restored on refresh in the same browser.

#### Scenario: Stored language preference is restored
- **WHEN** an operator has previously selected a UI language and later refreshes or reopens the application in the same browser
- **THEN** the frontend SHALL restore the previously selected language instead of reverting to the default language

### Requirement: The system SHALL localize shared shell and common user-facing frontend surfaces
The frontend SHALL move shared shell copy, navigation labels, login text, global alerts, and other common user-facing application text from hard-coded strings to locale-managed message resources. The frontend SHALL also extend that localization coverage to the deferred second-wave routed views, including billing and invoice pages, lease detail/create pages, reporting and visual-analysis pages, tax export and print-preview pages, and the remaining admin-console views in scope for this change.

#### Scenario: Shared shell and login surfaces follow the active locale
- **WHEN** the active locale changes or the application loads with a resolved locale
- **THEN** the shared shell, permission-driven navigation labels, login page, and common frontend user messages SHALL render text from locale-managed message resources for that locale

#### Scenario: Deferred second-wave routed views follow the active locale
- **WHEN** an operator opens a second-wave deferred routed view after this change is implemented and the application locale is `zh-CN` or `en-US`
- **THEN** the page-section copy, form labels, filter labels, table labels, action labels, feedback messages, and other frontend-authored UI text on that view SHALL render from locale-managed message resources for the active locale
