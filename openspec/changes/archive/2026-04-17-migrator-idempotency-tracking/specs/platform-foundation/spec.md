## MODIFIED Requirements

### Requirement: The system SHALL provide a modular monolith foundation for the new stack
The change SHALL introduce a Vue 3 frontend, a Go modular monolith backend, and a MySQL 8 database as the first-release runtime foundation. The foundation SHALL support local development for frontend/backend with an existing Docker MySQL 8 instance and SHALL support Docker Compose-based test and production topologies.

#### Scenario: Local development foundation is available
- **WHEN** a developer starts the frontend and backend locally with the documented development configuration
- **THEN** the application SHALL connect to the configured MySQL 8 instance and expose working frontend and backend health endpoints

#### Scenario: Database migrations are idempotent
- **WHEN** the migration tool is executed against a database that has already been migrated
- **THEN** the tool SHALL detect previously applied migrations via a tracking table and SHALL skip them without error

#### Scenario: Migration tracking table is maintained
- **WHEN** a database migration is successfully applied
- **THEN** the tool SHALL record the migration version and timestamp in a `schema_migrations` tracking table

#### Scenario: Fresh database receives all migrations
- **WHEN** the migration tool is executed against an empty database
- **THEN** the tool SHALL apply all pending migrations in order and SHALL record each in the tracking table
