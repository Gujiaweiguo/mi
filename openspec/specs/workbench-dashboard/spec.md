## Purpose

Define the authenticated workbench dashboard behavior for the replacement MI system.

## Requirements

### Requirement: Authenticated users SHALL land on an operational dashboard
After login, authenticated users SHALL be routed to a dashboard page that summarizes their operational workload instead of the health page.

#### Scenario: Dashboard becomes the default authenticated home
- **WHEN** an authenticated user enters the application root path
- **THEN** the application SHALL route them to the dashboard as the first authorized navigation destination

### Requirement: The dashboard SHALL surface key operational counts using existing APIs
The dashboard SHALL display operational summary cards for the highest-value day-one work queues.

#### Scenario: Dashboard summary is available from a single backend endpoint
- **WHEN** an authenticated user requests `GET /api/dashboard/summary`
- **THEN** the backend SHALL return a single JSON object containing all six operational metrics in one response

#### Scenario: Frontend dashboard loads with one API call
- **WHEN** the dashboard view mounts
- **THEN** it SHALL make exactly one API call to fetch all summary metrics

#### Scenario: Summary cards show current operational counts
- **WHEN** the dashboard loads successfully
- **THEN** it SHALL show counts for active leases, pending approvals, receivables, overdue receivables, and pending workflows

### Requirement: The dashboard SHALL provide quick access to priority workflows
The dashboard SHALL include quick actions or direct links into the most common operational flows.

#### Scenario: User can jump directly into priority work
- **WHEN** a user views the dashboard
- **THEN** they SHALL be able to navigate directly to lease, invoice, receivable, and related operational pages from the dashboard

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

### Requirement: Workbench SHALL load queue-oriented operational data with one backend request
The system SHALL provide a single authenticated backend response for the Workbench that aggregates the first-release operator queues needed for daily triage.

#### Scenario: Workbench data is available from one aggregate endpoint
- **WHEN** an authenticated user requests the Workbench data API
- **THEN** the backend SHALL return one JSON response containing all Workbench queue sections required by the frontend

#### Scenario: Workbench response covers the first-release queue scope
- **WHEN** the Workbench aggregate response is returned successfully
- **THEN** it SHALL include queue-oriented data for pending approvals, receivables, overdue receivables, and active lease workload

### Requirement: Workbench SHALL present stable queue sections for empty and populated states
The frontend SHALL render the Workbench from the aggregate response without requiring static placeholder rows, and each queue section SHALL remain visible with a stable empty state when no items are available.

#### Scenario: Workbench renders populated queues
- **WHEN** the Workbench aggregate response contains queue rows
- **THEN** the Workbench SHALL show queue counts and preview rows for each populated section

#### Scenario: Workbench renders empty queues without breaking layout
- **WHEN** one or more Workbench queue sections contain no rows
- **THEN** the Workbench SHALL render those sections with zero counts and explicit empty-state messaging instead of omitting the section or failing to render the page

### Requirement: Workbench SHALL remain a triage surface that routes users to deeper operational pages
The Workbench SHALL help users identify priority work and navigate into the existing business pages that own full detail and mutation flows.

#### Scenario: User can navigate from queue section to downstream work page
- **WHEN** a user selects a queue section or preview item from the Workbench
- **THEN** the Workbench SHALL route the user to the existing downstream page that handles the related operational workflow

#### Scenario: Workbench does not become an inline task engine
- **WHEN** a user views the Workbench
- **THEN** the page SHALL surface summary and preview navigation only, and SHALL rely on existing downstream pages for business mutations and approvals
