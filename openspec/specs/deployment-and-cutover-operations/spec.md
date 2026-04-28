## Purpose

Define the Compose-based environments, production runtime safeguards, and fresh-start cutover workflows required to bring up, validate, rehearse, and operate the replacement system reliably.

## Requirements
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

### Requirement: The system SHALL provide a cutover rehearsal and rollback model
The first release SHALL include a repeatable rehearsal flow, release gates, rollback criteria, backup rehearsal, restore rehearsal, and machine-readable GO/NO-GO output for one-time cutover. Production-topology rehearsal for the evaluated commit SHALL remain a mandatory release-confidence checkpoint and SHALL fail fast when prerequisite evidence, production configuration hygiene, or runtime assumptions are invalid.

#### Scenario: Blocking decision prevents go-live
- **WHEN** a cutover rehearsal is run while a blocking release-gate decision remains unresolved
- **THEN** the rehearsal SHALL report a no-go outcome and SHALL NOT mark the release ready for production

#### Scenario: Missing current-commit archive evidence blocks rehearsal
- **WHEN** cutover rehearsal is started without passing `unit`, `integration`, and `e2e` evidence for the current HEAD commit
- **THEN** the rehearsal SHALL fail fast, report a no-go outcome, and SHALL NOT continue to a go-live-ready decision

#### Scenario: Stale current-commit archive evidence blocks rehearsal
- **WHEN** the available archive-ready evidence exists but does not match the current HEAD commit SHA
- **THEN** the rehearsal SHALL treat the evidence as invalid, report a no-go outcome, and SHALL NOT mark the release ready for production

#### Scenario: Placeholder production secrets block rehearsal
- **WHEN** cutover rehearsal is started with an evaluated production env file that still contains blocked placeholder values for required credentials or secrets
- **THEN** the rehearsal SHALL fail fast, report a no-go outcome, and SHALL NOT continue to bootstrap, smoke, backup, or restore steps

#### Scenario: Backup and restore rehearsal are verified together
- **WHEN** a cutover rehearsal is executed for the supported target environment
- **THEN** the workflow SHALL produce a backup bundle, restore it through the supported restore path, and run explicit post-restore validation before the rehearsal can report a go outcome

#### Scenario: Rehearsal emits machine-readable result artifacts
- **WHEN** the cutover rehearsal completes in either success or failure state
- **THEN** the system SHALL write machine-readable result artifacts and logs under `artifacts/rehearsal/<commit-sha>/` that identify the evaluated commit and the binary GO/NO-GO result

#### Scenario: Current-commit release readiness requires production rehearsal GO
- **WHEN** release readiness is evaluated for the current commit
- **THEN** the workflow SHALL require a production-environment rehearsal artifact for that commit with `status` equal to `GO` before treating the commit as go-live ready

### Requirement: The system SHALL start production without migrating legacy records
The first release SHALL start with reinitialized base data and SHALL NOT migrate legacy business records, pending drafts, approvals, or open operational transactions from the old system. Rehearsal and go-live execution SHALL validate bootstrap-only initialization and SHALL reject any path that attempts to import legacy transactional business data.

#### Scenario: Fresh-start cutover is enforced
- **WHEN** the cutover rehearsal or go-live checklist is executed
- **THEN** the system SHALL initialize only the approved fresh-start base data set and SHALL NOT import legacy business data into the new production system

#### Scenario: Legacy transactional import causes no-go
- **WHEN** rehearsal input or operator configuration attempts to include migrated legacy business records, open operational items, or in-flight approvals as starting state
- **THEN** the cutover workflow SHALL reject the attempt, report a no-go outcome, and preserve the repository’s fresh-start cutover rule

### Requirement: Production env vars SHALL match Viper's expected names
The production environment file SHALL use env var names that match Viper's automatic environment binding (`MI_` prefix + config key path with dots replaced by underscores).

#### Scenario: Env vars override YAML config
- **WHEN** `MI_DATABASE_HOST=mysql` is set in the environment
- **THEN** the backend SHALL use `mysql` as the database host, overriding any value in the YAML config file

### Requirement: First-release acceptance closure SHALL produce a current-commit release-readiness decision
The system SHALL evaluate first-release readiness for a specific commit by combining the bounded canonical product scope, current-commit verification evidence, must-fix blocker status, and machine-readable release-decision output. The readiness decision SHALL remain anchored to the non-membership first-release boundary and SHALL NOT silently widen to out-of-scope legacy or membership features.

#### Scenario: Current-commit release readiness is summarized from bounded first-release scope
- **WHEN** operators evaluate release readiness for the current HEAD commit
- **THEN** the workflow SHALL produce a machine-readable summary that identifies the evaluated commit, the bounded first-release scope being judged, and the resulting GO/NO-GO decision

#### Scenario: Missing current-commit acceptance evidence blocks release readiness
- **WHEN** the final readiness workflow is executed without the required current-commit verification evidence
- **THEN** the workflow SHALL report a NO-GO decision and SHALL identify the missing evidence inputs as blockers

#### Scenario: Stale current-commit acceptance evidence blocks release readiness
- **WHEN** verification evidence exists but references a commit SHA different from the current HEAD commit being evaluated
- **THEN** the workflow SHALL treat the evidence as invalid and SHALL report a NO-GO decision for the current commit

#### Scenario: Deferred non-blocking items remain explicit without widening scope
- **WHEN** known gaps are recorded that are post-release, explicitly out of scope, or otherwise not release-blocking under the first-release boundary
- **THEN** the readiness summary SHALL keep those items visible as deferred findings without treating them as evidence that the product scope itself has widened

#### Scenario: Must-fix blockers prevent GO decision
- **WHEN** any bounded first-release acceptance check records a must-fix blocker for the evaluated commit
- **THEN** the workflow SHALL emit a NO-GO decision and SHALL enumerate the blocking findings in machine-readable form
