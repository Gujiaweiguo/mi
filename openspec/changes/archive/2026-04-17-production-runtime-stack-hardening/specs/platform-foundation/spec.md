## ADDED Requirements

### Requirement: The backend SHALL perform graceful shutdown on termination signals
The backend SHALL capture SIGTERM and SIGINT signals. When a termination signal is received, the backend SHALL stop accepting new requests, drain in-flight requests, stop workflow scheduler goroutines, and close database connections. The drain period SHALL be bounded by a configurable shutdown timeout. If the timeout is exceeded, the backend SHALL force-exit regardless of remaining in-flight work.

#### Scenario: SIGTERM received triggers graceful drain and clean exit
- **WHEN** the backend receives a SIGTERM or SIGINT signal while serving requests
- **THEN** the backend SHALL stop accepting new connections, SHALL allow in-flight requests to complete up to the configured shutdown timeout, SHALL stop workflow scheduler goroutines, SHALL close database connections, and SHALL exit with code 0

#### Scenario: Shutdown timeout exceeded forces exit
- **WHEN** the backend receives a termination signal and the configured shutdown timeout expires before all in-flight requests finish
- **THEN** the backend SHALL force-exit and SHALL log a warning indicating that the shutdown timeout was exceeded

### Requirement: The backend SHALL configure the database connection pool with tunable parameters
The backend SHALL configure the MySQL connection pool using `SetMaxOpenConns`, `SetMaxIdleConns`, `SetConnMaxLifetime`, and `SetConnMaxIdleTime` with values sourced from environment configuration. Default values SHALL be applied when environment overrides are not provided.

#### Scenario: Connection pool parameters are applied on startup
- **WHEN** the backend initializes the database connection
- **THEN** the backend SHALL apply `SetMaxOpenConns`, `SetMaxIdleConns`, `SetConnMaxLifetime`, and `SetConnMaxIdleTime` to the connection pool using configured or default values

#### Scenario: Custom pool parameters override defaults
- **WHEN** environment configuration supplies connection pool parameter values
- **THEN** the backend SHALL use those values instead of defaults when configuring `SetMaxOpenConns`, `SetMaxIdleConns`, `SetConnMaxLifetime`, and `SetConnMaxIdleTime`

### Requirement: The health endpoint SHALL verify database connectivity and report degraded status
The backend health endpoint SHALL ping the database connection as part of its readiness check. When the database is unreachable, the endpoint SHALL return HTTP 503 with a JSON body containing error detail. When the database is reachable, the endpoint SHALL return HTTP 200 with a JSON body confirming database status.

#### Scenario: Database reachable returns healthy status
- **WHEN** the health endpoint is called and the database connection ping succeeds
- **THEN** the endpoint SHALL return HTTP 200 with a JSON response body that includes database status as healthy

#### Scenario: Database unreachable returns degraded status
- **WHEN** the health endpoint is called and the database connection ping fails
- **THEN** the endpoint SHALL return HTTP 503 with a JSON response body that includes error detail describing the database connectivity failure
