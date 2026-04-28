## ADDED Requirements

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
