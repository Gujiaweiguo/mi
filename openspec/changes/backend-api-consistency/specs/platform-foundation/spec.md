## ADDED Requirements

### Requirement: API response format SHALL be consistent across all handlers
All API handlers SHALL return JSON responses in a consistent shape. Business data SHALL be wrapped in a keyed object (e.g., `{"report": ...}`) rather than returned as raw top-level values.

#### Scenario: Reporting handler wraps response
- **WHEN** the reporting Query endpoint returns data
- **THEN** the response SHALL be `{"report": <data>}`, matching the keyed-object pattern used by all other handlers

### Requirement: IN-clause placeholder generation SHALL use a shared helper
All code that builds SQL IN-clause placeholder strings SHALL use `sqlutil.InPlaceholders(n)` instead of manual loop construction.

#### Scenario: IN-clause helper exists in sqlutil
- **WHEN** a repository needs to build an IN-clause with N placeholders
- **THEN** it SHALL call `sqlutil.InPlaceholders(n)` to get the placeholder string
