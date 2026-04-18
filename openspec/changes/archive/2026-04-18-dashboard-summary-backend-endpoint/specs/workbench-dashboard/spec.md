## MODIFIED Requirements

### Requirement: The dashboard SHALL surface key operational counts using existing APIs
The dashboard SHALL display operational summary cards for the highest-value day-one work queues.

#### Scenario: Dashboard summary is available from a single backend endpoint
- **WHEN** an authenticated user requests `GET /api/dashboard/summary`
- **THEN** the backend SHALL return a single JSON object containing all six operational metrics in one response

#### Scenario: Frontend dashboard loads with one API call
- **WHEN** the dashboard view mounts
- **THEN** it SHALL make exactly one API call to fetch all summary metrics
