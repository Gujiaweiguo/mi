## MODIFIED Requirements

### Requirement: The system SHALL provide Compose-based test and production operations
The change SHALL provide Docker Compose definitions for test and production that include nginx, frontend, backend, and MySQL 8 with explicit health checks, mounted runtime paths, rendered environment-specific configuration, and executable bring-up validation for the documented runtime assumptions. Production-oriented operational validation SHALL additionally fail fast when runtime mount hygiene, container runtime assumptions, or blocked placeholder production secrets would make rehearsal outcomes unreliable or insecure. The production backend Docker image SHALL include Chromium for PDF generation. The repository SHALL have a `.dockerignore` file that excludes non-runtime paths from Docker build context. All production Compose services SHALL have `restart: unless-stopped` restart policies. Backend and MySQL production services SHALL have memory limits. All production services SHALL have JSON-file logging with `max-size` and `max-file` constraints. MySQL SHALL NOT expose ports to the host in the production Compose configuration. The frontend container SHALL run nginx worker processes as a non-root user. Production nginx SHALL set security headers (X-Content-Type-Options, X-Frame-Options, Content-Security-Policy, Strict-Transport-Security, X-XSS-Protection, Referrer-Policy), SHALL enable gzip for text-based content types, SHALL configure proxy timeouts for the backend, and SHALL set `client_max_body_size`.

#### Scenario: Test environment starts successfully
- **WHEN** the test Docker Compose stack is started from a clean environment
- **THEN** all required services SHALL become healthy and SHALL expose the documented frontend and backend entry points

#### Scenario: Compose configuration is rendered before startup
- **WHEN** an operator prepares the test or production environment for bring-up
- **THEN** the system SHALL render and validate the target Compose configuration before attempting container startup so path, env-file, and syntax issues fail fast

#### Scenario: Runtime mount assumptions are validated
- **WHEN** the test or production stack is started through the supported operational workflow
- **THEN** the system SHALL validate that required runtime directories and mounted paths exist and are writable before declaring the environment ready

#### Scenario: Production runtime hygiene is validated before stack bring-up
- **WHEN** production compose startup is prepared for rehearsal or go-live-oriented validation
- **THEN** the workflow SHALL fail fast if runtime data and mount baselines violate documented clean-start and hygiene assumptions

#### Scenario: Production placeholder secrets block stack bring-up validation
- **WHEN** production compose startup is prepared with an evaluated env file that still contains blocked placeholder values for required production credentials or secrets
- **THEN** the workflow SHALL fail fast before container startup and SHALL report the affected production secret keys

#### Scenario: Backend Docker image includes Chromium for PDF generation
- **WHEN** the production backend Docker image is built
- **THEN** the image SHALL include a Chromium installation sufficient for headless PDF generation

#### Scenario: Dockerignore excludes non-runtime paths from build context
- **WHEN** a Docker build is executed for any service image
- **THEN** the `.dockerignore` file SHALL exclude legacy code, `.git`, build artifacts, and other non-runtime paths from the build context

#### Scenario: Production services have restart policies
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** all services SHALL have `restart: unless-stopped` configured

#### Scenario: Production services have memory limits
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** the backend service and the MySQL service SHALL each have a configured memory limit

#### Scenario: Production services have structured logging with rotation
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** all services SHALL have JSON-file logging driver configured with `max-size` and `max-file` values

#### Scenario: MySQL is not exposed to the host in production
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** the MySQL service SHALL NOT map any ports to the host and SHALL only be reachable through the internal Docker network

#### Scenario: Frontend container runs nginx as non-root
- **WHEN** the production frontend container is running
- **THEN** nginx worker processes SHALL run as a non-root user

#### Scenario: Production nginx sets security headers
- **WHEN** production nginx serves any response
- **THEN** the response SHALL include X-Content-Type-Options, X-Frame-Options, Content-Security-Policy, Strict-Transport-Security, X-XSS-Protection, and Referrer-Policy headers with production-appropriate values

#### Scenario: Production nginx enables gzip compression
- **WHEN** production nginx serves text-based content types
- **THEN** gzip compression SHALL be enabled for those content types

#### Scenario: Production nginx configures proxy timeouts for backend
- **WHEN** production nginx proxies requests to the backend service
- **THEN** proxy connect, read, and send timeouts SHALL be configured with explicit values

#### Scenario: Production nginx sets upload body size limit
- **WHEN** production nginx receives a client request with a body
- **THEN** `client_max_body_size` SHALL be set to a configured maximum upload size
