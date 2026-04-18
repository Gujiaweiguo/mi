## ADDED Requirements

### Requirement: Authenticated users SHALL land on an operational dashboard
After login, authenticated users SHALL be routed to a dashboard page that summarizes their operational workload instead of the health page.

#### Scenario: Dashboard becomes the default authenticated home
- **WHEN** an authenticated user enters the application root path
- **THEN** the application SHALL route them to the dashboard as the first authorized navigation destination

### Requirement: The dashboard SHALL surface key operational counts using existing APIs
The dashboard SHALL display operational summary cards for the highest-value day-one work queues by aggregating existing frontend API responses.

#### Scenario: Summary cards show current operational counts
- **WHEN** the dashboard loads successfully
- **THEN** it SHALL show counts for active leases, pending approvals, receivables, overdue receivables, and pending workflows

### Requirement: The dashboard SHALL provide quick access to priority workflows
The dashboard SHALL include quick actions or direct links into the most common operational flows.

#### Scenario: User can jump directly into priority work
- **WHEN** a user views the dashboard
- **THEN** they SHALL be able to navigate directly to lease, invoice, receivable, and related operational pages from the dashboard
