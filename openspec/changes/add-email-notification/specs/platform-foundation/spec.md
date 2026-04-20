## MODIFIED Requirements

### Requirement: The system SHALL externalize environment configuration and runtime mounts
The change SHALL define environment-specific configuration with file-based defaults and environment-variable overrides. Test and production environments SHALL mount runtime paths for configuration, logs, generated documents, uploads, and MySQL data. Production runtime mounts SHALL also enforce documented hygiene and permission assumptions so rehearsal and go-live validation are not considered valid under contaminated runtime baselines or unsupported container runtime behavior. The backend config SHALL include an `email` configuration section with fields for SMTP host, port, username, password, from address, from name, template directory, enabled flag, max retry attempts, retry interval, poll interval, and batch size. Environment variables for email SHALL follow the `MI_EMAIL_` prefix convention.

#### Scenario: Production runtime paths are configured
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** explicit mounts SHALL exist for configuration, logs, generated documents/uploads, and MySQL data

#### Scenario: Production runtime mount hygiene is validated
- **WHEN** production startup or rehearsal preflight evaluates runtime mount baselines
- **THEN** the workflow SHALL reject runtime baselines that violate documented clean-start and hygiene constraints for supported production validation

#### Scenario: Runtime mount permissions are validated for supported container behavior
- **WHEN** production startup or rehearsal validation checks mounted runtime paths
- **THEN** the workflow SHALL verify required writable paths under supported container runtime assumptions and SHALL fail when those assumptions are not met

#### Scenario: Email config section is loaded from YAML
- **WHEN** the backend config YAML contains an `email` section with `smtp_host`, `smtp_port`, `from_address`, and other email fields
- **THEN** the Viper config SHALL populate the `EmailConfig` struct with those values

#### Scenario: Email config is overridden by environment variables
- **WHEN** environment variables like `MI_EMAIL_SMTP_HOST` and `MI_EMAIL_SMTP_PORT` are set
- **THEN** those values SHALL override the corresponding YAML config entries for email settings

#### Scenario: Email config defaults to disabled
- **WHEN** no `email` section exists in the YAML config and no `MI_EMAIL_` environment variables are set
- **THEN** the `EmailConfig.Enabled` field SHALL default to `false` and no SMTP connection SHALL be attempted

### Requirement: The system SHALL provide a modular monolith foundation for the new stack
The change SHALL introduce a Vue 3 frontend, a Go modular monolith backend, and a MySQL 8 database as the first-release runtime foundation. The foundation SHALL support local development for frontend/backend with an existing Docker MySQL 8 instance and SHALL support Docker Compose-based test and production topologies. The database migration sequence SHALL include a migration that creates the `notification_outbox` table with columns for event type, aggregate type, aggregate ID, recipient addresses, subject, template name, template data as JSON, status enum, attempt count, max attempts, next attempt timestamp, sent timestamp, last error, and created/updated timestamps.

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

#### Scenario: Notification outbox migration creates required table
- **WHEN** the migration sequence is applied to a database that does not yet have the `notification_outbox` table
- **THEN** the migration SHALL create the table with the required columns, status enum (`pending`, `sending`, `sent`, `failed`, `dead`), and indexes on `(status, next_attempt_at)` and `(aggregate_type, aggregate_id)`

#### Scenario: Notification outbox migration is idempotent on re-run
- **WHEN** the migration tool runs against a database where the `notification_outbox` table already exists
- **THEN** the migration SHALL be skipped as already applied and the existing table SHALL remain unchanged

### Requirement: The backend SHALL perform graceful shutdown on termination signals
The backend SHALL capture SIGTERM and SIGINT signals. When a termination signal is received, the backend SHALL stop accepting new requests, drain in-flight requests, stop workflow scheduler goroutines, stop the notification outbox polling goroutine, and close database connections. The drain period SHALL be bounded by a configurable shutdown timeout. If the timeout is exceeded, the backend SHALL force-exit regardless of remaining in-flight work.

#### Scenario: SIGTERM received triggers graceful drain and clean exit
- **WHEN** the backend receives a SIGTERM or SIGINT signal while serving requests
- **THEN** the backend SHALL stop accepting new connections, SHALL allow in-flight requests to complete up to the configured shutdown timeout, SHALL stop workflow scheduler goroutines, SHALL stop the notification outbox polling goroutine, SHALL close database connections, and SHALL exit with code 0

#### Scenario: Shutdown timeout exceeded forces exit
- **WHEN** the backend receives a termination signal and the configured shutdown timeout expires before all in-flight requests finish
- **THEN** the backend SHALL force-exit and SHALL log a warning indicating that the shutdown timeout was exceeded

#### Scenario: Notification polling goroutine stops cleanly on shutdown
- **WHEN** the backend receives a termination signal and the notification outbox polling goroutine is running
- **THEN** the backend SHALL signal the polling goroutine to stop, the goroutine SHALL finish its current send attempt, and the goroutine SHALL exit before the shutdown timeout
