## MODIFIED Requirements

### Requirement: The system SHALL provide automated database initialization
The deployment stack SHALL include a migration init service that runs database migrations and seed data before the backend starts. The backend Docker image SHALL include the `dbops` binary for this purpose.

#### Scenario: Fresh deployment initializes database automatically
- **WHEN** `docker-compose up` is run with an empty MySQL data directory
- **THEN** the migrate service SHALL run `dbops migrate` and `dbops bootstrap` before the backend starts accepting connections

## ADDED Requirements

### Requirement: Production env vars SHALL match Viper's expected names
The production environment file SHALL use env var names that match Viper's automatic environment binding (`MI_` prefix + config key path with dots replaced by underscores).

#### Scenario: Env vars override YAML config
- **WHEN** `MI_DATABASE_HOST=mysql` is set in the environment
- **THEN** the backend SHALL use `mysql` as the database host, overriding any value in the YAML config file
