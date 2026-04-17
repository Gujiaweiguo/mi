## Purpose

TBD: Canonical platform foundation spec for the replacement MI system.
## Requirements
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

### Requirement: The system SHALL externalize environment configuration and runtime mounts
The change SHALL define environment-specific configuration with file-based defaults and environment-variable overrides. Test and production environments SHALL mount runtime paths for configuration, logs, generated documents, uploads, and MySQL data. Production runtime mounts SHALL also enforce documented hygiene and permission assumptions so rehearsal and go-live validation are not considered valid under contaminated runtime baselines or unsupported container runtime behavior.

#### Scenario: Production runtime paths are configured
- **WHEN** the production Docker Compose configuration is rendered
- **THEN** explicit mounts SHALL exist for configuration, logs, generated documents/uploads, and MySQL data

#### Scenario: Production runtime mount hygiene is validated
- **WHEN** production startup or rehearsal preflight evaluates runtime mount baselines
- **THEN** the workflow SHALL reject runtime baselines that violate documented clean-start and hygiene constraints for supported production validation

#### Scenario: Runtime mount permissions are validated for supported container behavior
- **WHEN** production startup or rehearsal validation checks mounted runtime paths
- **THEN** the workflow SHALL verify required writable paths under supported container runtime assumptions and SHALL fail when those assumptions are not met

### Requirement: The system SHALL establish automated test foundations before feature slices
The change SHALL provide backend unit and integration test harnesses, frontend unit tests, Playwright end-to-end tests, and artifact comparison support for generated outputs before feature slices depend on them. End-to-end verification for first-release non-membership scope SHALL be reproducible under documented clean-checkout bootstrap assumptions so archive-evidence generation remains trustworthy. Unit and integration evidence emitted by the verification scripts SHALL derive reported test counts from actual test results rather than fixed placeholder values. The repository SHALL also provide a supported validation path for frontend typechecking, backend static analysis, and frontend build verification so non-test regressions are caught through the default delivery workflow rather than left to ad hoc local checks. Verification scripts that produce `unit` or `integration` evidence SHALL always emit commit-scoped evidence for the evaluated commit regardless of whether the underlying test command succeeds or fails, and SHALL reflect the real pass/fail outcome in the evidence `status` field rather than hardcoding success semantics. Report row structs SHALL carry their own serialization logic via a `ToMap() map[string]any` method so that struct-to-map conversion is collocated with the type definition rather than centralized in a single service method.

#### Scenario: Report row struct provides ToMap method
- **WHEN** a report row struct (e.g., R01Row, R18Row) is converted to a map via `ToMap()`
- **THEN** the returned map SHALL contain exactly the same keys and values as the previous inline map literal conversion, ensuring identical JSON API output

### Requirement: The system SHALL enforce commit-scoped CI evidence gates
The project SHALL define a CI gate that requires passing `unit` and `integration` evidence for the current commit before a push/PR is considered CI-ready. Required CI evidence SHALL follow the canonical verification schema and SHALL be rejected when schema fields or invariants are violated. The repository SHALL provide architecture documentation that explicitly describes responsibility boundaries between evidence producers, schema-driven structure validation, contextual gate validation, and CI-ready entrypoint orchestration. The repository SHALL also define maintenance policy expectations for verification-stack changes so that CI gate behavior, schema validation, and related documentation remain synchronized. CI-ready execution SHALL additionally require all supported prerequisite validation steps for that path, including typecheck, static analysis, and build verification, to pass before the evidence gate can be treated as satisfied. The CI-ready workflow trigger policy SHALL avoid creating duplicate runs for the same PR update while preserving one authoritative validation result for the evaluated commit.

#### Scenario: Verification architecture responsibilities are documented for CI path
- **WHEN** contributors need to modify CI-related verification behavior
- **THEN** the repository SHALL provide architecture documentation that identifies where producer logic, schema rules, validator contextual checks, and CI entrypoint wiring each belong

#### Scenario: CI gate blocks when required evidence is missing
- **WHEN** unit or integration evidence for the evaluated commit is missing
- **THEN** CI-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: CI gate blocks stale evidence after commit changes
- **WHEN** evidence files exist but `commit_sha` does not match the currently evaluated commit
- **THEN** CI-ready evaluation SHALL reject the evidence as stale and SHALL fail the gate

#### Scenario: CI-ready path blocks on failed prerequisite validation
- **WHEN** the supported CI-ready path encounters a failing typecheck, static-analysis, or build-validation step for the evaluated commit
- **THEN** the CI-ready path SHALL fail and SHALL not report the commit as ready even if `unit` and `integration` evidence exist

#### Scenario: A pull request update produces one CI-ready run
- **WHEN** a contributor updates a branch that already has an open pull request
- **THEN** the repository SHALL produce one authoritative `ci-ready` workflow run for that PR update instead of duplicate runs with the same validation intent

#### Scenario: Trigger deduplication does not weaken commit-scoped validation
- **WHEN** CI-ready runs for an evaluated commit after trigger deduplication is applied
- **THEN** the run SHALL still validate `unit` and `integration` evidence for the current commit SHA and SHALL fail if required evidence is missing or stale

#### Scenario: Non-PR CI coverage remains intentional
- **WHEN** a branch update occurs outside the PR lifecycle but still matches the repository's intended CI-ready coverage policy
- **THEN** the workflow SHALL either run by design or be explicitly excluded by documented trigger rules, rather than being left ambiguous by overlapping events

### Requirement: The system SHALL enforce stricter archive evidence gates
The project SHALL define an archive gate that requires passing `unit`, `integration`, and `e2e` evidence for the current commit before a change is considered archive-ready. All required archive evidence SHALL follow the same canonical verification schema used by CI evidence, with `e2e` additionally requiring non-empty artifact references. The repository SHALL provide architecture documentation that explains how archive evaluation reuses shared verification components while preserving archive-specific requirements. The repository SHALL also define maintenance policy expectations for archive-path verification changes so regression checks and documentation updates are performed together.

#### Scenario: Verification architecture documents archive-specific concerns
- **WHEN** contributors need to reason about archive-ready behavior
- **THEN** the architecture documentation SHALL explain shared versus archive-specific verification responsibilities without requiring reverse engineering from scripts alone

#### Scenario: Archive gate blocks when required evidence is missing
- **WHEN** any required archive evidence (`unit`, `integration`, or `e2e`) for the evaluated commit is missing
- **THEN** archive-ready evaluation SHALL fail and SHALL not be treated as satisfied

#### Scenario: Archive gate blocks stale evidence after commit changes
- **WHEN** archive evidence exists but references a different commit SHA than the evaluated commit
- **THEN** archive-ready evaluation SHALL reject the stale evidence and SHALL fail the gate

### Requirement: The system SHALL provide frontend locale infrastructure as part of the application foundation
The frontend foundation SHALL include locale infrastructure that can be initialized during app startup, provide locale-managed application messages, and align shared framework locale behavior with the active application locale.

#### Scenario: Frontend startup initializes locale infrastructure
- **WHEN** the frontend application starts in local development or deployed runtime
- **THEN** the application SHALL initialize its locale infrastructure before rendering localized user-facing screens

#### Scenario: Shared framework locale follows application locale
- **WHEN** the active application locale is resolved during startup or changed at runtime
- **THEN** shared frontend framework locale behavior SHALL follow the same active locale instead of remaining fixed in English

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

### Requirement: All placeholder URLs and scaffolding text SHALL be removed before first release
Seed data and i18n locale files SHALL NOT contain `https://example.com` placeholder URLs. User-facing locale strings SHALL NOT contain scaffolding-era text such as the word "stub". Floor plan image URLs SHALL use empty strings to indicate "not provided" since the field is optional and the UI already handles absence with a fallback.

#### Scenario: No placeholder URLs in locale files
- **WHEN** a search is performed across `frontend/src/i18n/messages/` for `example.com`
- **THEN** no matches SHALL be found

#### Scenario: No placeholder URLs in seed data
- **WHEN** `commercial_seed.go` is examined for `example.com`
- **THEN** no matches SHALL be found

#### Scenario: No placeholder URLs in e2e fixtures
- **WHEN** `frontend/e2e/task16-r19-visual.spec.ts` is examined for `example.com`
- **THEN** no matches SHALL be found

#### Scenario: No scaffolding-era stub text in locale files
- **WHEN** `en-US.ts` is searched for "No stub records"
- **THEN** no matches SHALL be found

#### Scenario: Backend compiles after seed data change
- **WHEN** `go build ./...` is run from the backend directory
- **THEN** it SHALL succeed

#### Scenario: Frontend typechecks after locale string change
- **WHEN** `vue-tsc --noEmit` is run from the frontend directory
- **THEN** it SHALL succeed

### Requirement: The backend SHALL emit structured JSON logs for every HTTP request
The backend SHALL use a custom Gin middleware backed by the existing Zap logger to emit one structured log line per completed HTTP request. Each log line SHALL include the HTTP method, request path, response status code, request latency in milliseconds, client IP, and request ID. When the request is authenticated, the log line SHALL also include the authenticated user ID. The middleware SHALL replace Gin's default `gin.Logger()`.

#### Scenario: Successful request produces structured log
- **WHEN** a GET request to `/api/leases` completes with status 200 in 45ms
- **THEN** the backend SHALL emit a single Zap info-level log line containing `method=GET`, `path=/api/leases`, `status=200`, `latency_ms=45`, `client_ip`, and `request_id` as structured fields

#### Scenario: Authenticated request includes user ID
- **WHEN** an authenticated request completes
- **THEN** the structured log line SHALL include a `user_id` field with the authenticated user's ID

#### Scenario: Failed request produces structured log
- **WHEN** a request completes with status 4xx or 5xx
- **THEN** the backend SHALL emit a Zap warn-level log line with the same structured fields

### Requirement: Every HTTP request SHALL carry a unique request ID
The backend SHALL use a request-ID middleware that checks for an incoming `X-Request-ID` header. If present, the middleware SHALL use that value. If absent, the middleware SHALL generate a new UUID v4. The request ID SHALL be stored in the Gin context and SHALL be set as the `X-Request-ID` response header. The request ID SHALL be included in all structured log entries for the request.

#### Scenario: Request without X-Request-ID header
- **WHEN** an incoming request does not include an `X-Request-ID` header
- **THEN** the middleware SHALL generate a new UUID v4, store it in the Gin context, and set it on the `X-Request-ID` response header

#### Scenario: Request with existing X-Request-ID header
- **WHEN** an incoming request includes an `X-Request-ID` header (e.g., from nginx or a load balancer)
- **THEN** the middleware SHALL use that value, store it in the Gin context, and pass it through on the response header

### Requirement: Handler panics SHALL be logged as structured errors
The backend SHALL use a custom recovery middleware backed by Zap that catches panics in handler functions. The middleware SHALL log the panic value and stack trace as a Zap error-level log line with the request ID, method, path, and client IP as structured fields. The middleware SHALL then return HTTP 500 with a generic JSON error body. The middleware SHALL replace Gin's default `gin.Recovery()`.

#### Scenario: Handler panic produces structured error log
- **WHEN** a handler function panics during request processing
- **THEN** the middleware SHALL log an error-level entry with `request_id`, `method`, `path`, `client_ip`, `error` (panic value), and `stack` (stack trace), and SHALL return HTTP 500 with `{"message": "internal server error"}`

### Requirement: The frontend SHALL capture unhandled Vue errors
The Vue 3 application SHALL register a global error handler via `app.config.errorHandler` that logs unhandled component errors to the browser console. The log output SHALL include the error, the component instance that caused it, and the error info object.

#### Scenario: Unhandled component error is logged
- **WHEN** a Vue component throws an unhandled error during rendering or lifecycle
- **THEN** the global error handler SHALL log the error, component instance, and info to `console.error`

### Requirement: The frontend SHALL provide a reusable pagination composable for list views
The frontend SHALL expose a `usePagination` composable that manages page, pageSize, and total refs. The composable SHALL provide a `paginationParams` computed property returning `{ page, page_size }` suitable for spreading into API call parameters. The composable SHALL provide a `resetPage()` function that sets page to 1.

#### Scenario: Composable initializes with default page size
- **WHEN** `usePagination()` is called without arguments
- **THEN** it SHALL return `page` ref initialized to 1, `pageSize` ref initialized to 20, `total` ref initialized to 0

#### Scenario: Pagination params are computed for API calls
- **WHEN** a view spreads `paginationParams` into an API call
- **THEN** the API call SHALL receive `page` and `page_size` query parameters with the current values

#### Scenario: Page resets when filters change
- **WHEN** `resetPage()` is called
- **THEN** `page` SHALL be set to 1

### Requirement: Business list views SHALL pass pagination parameters to paginated API endpoints
LeaseListView, BillingInvoicesView, BillingChargesView, and ReceivablesView SHALL pass `page` and `page_size` parameters to their respective list API calls. When the API returns a `PaginatedResponse`, the view SHALL update the composable's total from `response.data.total`.

#### Scenario: Lease list view passes pagination params
- **WHEN** LeaseListView calls `listLeases()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

#### Scenario: Invoice list view passes pagination params
- **WHEN** BillingInvoicesView calls `listInvoices()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

#### Scenario: Charges list view passes pagination params
- **WHEN** BillingChargesView calls `listCharges()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

#### Scenario: Receivables list view passes pagination params
- **WHEN** ReceivablesView calls `listReceivables()`
- **THEN** it SHALL include `page` and `page_size` from the pagination composable

### Requirement: Business list views SHALL render pagination controls
LeaseListView, BillingInvoicesView, BillingChargesView, and ReceivablesView SHALL render an `<el-pagination>` component below their data table. The pagination component SHALL display page numbers, support page size selection, and show the total count. Clicking a page number SHALL reload data for that page.

#### Scenario: User navigates to page 2
- **WHEN** the user clicks page 2 in the pagination control
- **THEN** the view SHALL set page to 2 and reload data with the updated pagination parameters

#### Scenario: User changes page size
- **WHEN** the user selects a different page size from the pagination control
- **THEN** the view SHALL reset page to 1, update page_size, and reload data

### Requirement: The project SHALL provide an E2E smoke test for the Lease→Invoice→Payment business chain
The backend SHALL include an integration test (behind `//go:build integration` tag) that exercises the complete operational chain through the HTTP API: login, create lease, submit for approval, approve workflow (all steps), generate charges, create invoice, submit invoice, approve invoice workflow, record payment, and verify receivable settlement.

#### Scenario: Full chain produces settled receivable
- **WHEN** the E2E smoke test creates a lease, submits it, approves it through the full workflow, generates charges for the lease period, creates an invoice from the charges, submits and approves the invoice, and records full payment
- **THEN** the receivable SHALL show `outstanding_amount` of 0 and `settlement_status` of "settled"

#### Scenario: Each intermediate step produces correct status transitions
- **WHEN** the test progresses through each step
- **THEN** the lease SHALL transition draft → pending_approval → active, and the invoice SHALL transition draft → pending_approval → approved

### Requirement: Pagination types SHALL be consolidated into a shared package
All backend modules that support paginated list endpoints SHALL use a shared `pagination` package for the `ListResult` type, `NormalizePage` function, and pagination constants instead of defining their own local duplicates. The shared package SHALL use `int64` for total count and `20` for default page size.

#### Scenario: Shared pagination package exists
- **WHEN** a module needs a paginated result type
- **THEN** it SHALL use `pagination.ListResult[T]` instead of defining its own struct

#### Scenario: Shared normalize function exists
- **WHEN** a repository needs to clamp page/pageSize values
- **THEN** it SHALL call `pagination.NormalizePage()` instead of a local duplicate

