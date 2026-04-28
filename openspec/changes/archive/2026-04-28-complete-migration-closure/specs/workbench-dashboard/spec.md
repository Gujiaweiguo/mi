## ADDED Requirements

### Requirement: Workbench SHALL be reachable from authenticated navigation
The system SHALL expose the Workbench as a first-class authenticated frontend route and navigation item without replacing the existing dashboard default home.

#### Scenario: Authenticated user opens the Workbench route
- **WHEN** an authenticated user navigates to `/workbench`
- **THEN** the application SHALL render the Workbench page instead of returning a missing route or forbidden page

#### Scenario: Workbench appears in standard navigation
- **WHEN** an authenticated user views the application navigation
- **THEN** the navigation SHALL include a localized Workbench entry that links to `/workbench`

#### Scenario: Dashboard remains the authenticated home
- **WHEN** an authenticated user enters the application root path
- **THEN** the application SHALL continue routing them to `/dashboard` as the default authorized home
