## ADDED Requirements

### Requirement: The system SHALL send email via SMTP with configurable host, port, and credentials
The notification subsystem SHALL use `gopkg.in/mail.v2` to connect to an SMTP server identified by configurable host, port, username, password, and from-address fields. The SMTP client SHALL support both STARTTLS and implicit TLS connections. Sending errors SHALL be caught and recorded without crashing the polling goroutine.

#### Scenario: Email sent successfully through SMTP relay
- **WHEN** a pending notification outbox entry is picked up by the polling goroutine and the SMTP server is reachable and accepts the message
- **THEN** the system SHALL send the email via the configured SMTP relay, mark the outbox entry as `sent`, and record the `sent_at` timestamp

#### Scenario: SMTP connection failure records error without crash
- **WHEN** the polling goroutine attempts to send a pending notification but the SMTP server is unreachable or rejects the connection
- **THEN** the system SHALL increment the `attempt_count`, record the error in `last_error`, set `status` to `failed`, schedule `next_attempt_at` for a future retry, and continue processing other pending entries

#### Scenario: TLS negotiation succeeds with cloud SMTP relay
- **WHEN** the SMTP server requires STARTTLS (e.g., Alibaba Cloud DirectMail, SendGrid on port 587)
- **THEN** the system SHALL upgrade the connection to TLS before sending credentials and the email body

### Requirement: The system SHALL enqueue notification events asynchronously using a database-backed outbox
The notification subsystem SHALL insert outgoing email records into a `notification_outbox` table within the same database transaction as the triggering business operation. A background polling goroutine SHALL pick up pending entries and attempt delivery. The outbox row SHALL store event type, aggregate type and ID, recipient addresses, subject, template name, template data as JSON, status, attempt count, and retry scheduling fields.

#### Scenario: Notification enqueued atomically with business operation
- **WHEN** a business service calls `Notifier.Enqueue(ctx, tx, event)` during a workflow transition or invoice operation
- **THEN** the system SHALL insert a `notification_outbox` row with status `pending` within the same transaction and SHALL commit both the business change and the outbox entry together

#### Scenario: Polling goroutine picks up and sends pending notification
- **WHEN** the polling goroutine runs and finds a pending outbox entry whose `next_attempt_at` has elapsed
- **THEN** the system SHALL lock the row using `FOR UPDATE SKIP LOCKED`, set status to `sending`, attempt SMTP delivery, and update to `sent` or `failed` based on the outcome

#### Scenario: Outbox entry exceeds max retry attempts
- **WHEN** a notification outbox entry's `attempt_count` reaches the configured `max_attempts` (default 5) and the last attempt failed
- **THEN** the system SHALL set the status to `dead`, record the final error in `last_error`, and SHALL NOT retry the entry again

#### Scenario: Multiple backend instances share outbox load safely
- **WHEN** two or more backend instances run the polling goroutine against the same database
- **THEN** each pending row SHALL be processed by at most one instance per poll cycle due to `FOR UPDATE SKIP LOCKED` row-level locking

### Requirement: The system SHALL render HTML email templates using Go html/template
The notification subsystem SHALL load email templates from a configurable on-disk directory (defaulting to `templates/email/`). Each template SHALL define a subject line and an HTML body. Template data SHALL be typed per notification event (e.g., `WorkflowApprovalData`, `InvoiceReminderData`, `LeaseExpirationData`) so templates receive structured fields rather than generic maps. Templates SHALL be parsed and cached at startup.

#### Scenario: Workflow approval template renders correctly
- **WHEN** the system renders the `workflow_approval_request` template with `WorkflowApprovalData` containing document type, document number, submitter name, approval step name, and a link to the approval page
- **THEN** the rendered email SHALL contain a subject line identifying the approval request and an HTML body with the document details and approval link

#### Scenario: Invoice reminder template renders correctly
- **WHEN** the system renders the `invoice_payment_reminder` template with `InvoiceReminderData` containing invoice number, customer name, amount due, due date, and days overdue
- **THEN** the rendered email SHALL contain a subject line referencing the overdue invoice and an HTML body with payment details

#### Scenario: Lease expiration template renders correctly
- **WHEN** the system renders the `lease_expiration_reminder` template with `LeaseExpirationData` containing contract number, tenant name, expiration date, and days remaining
- **THEN** the rendered email SHALL contain a subject line referencing the upcoming expiration and an HTML body with contract details

#### Scenario: Missing template returns clear error
- **WHEN** the polling goroutine attempts to render a notification whose `template_name` does not exist in the template directory
- **THEN** the system SHALL record a descriptive error in `last_error`, mark the entry as `failed`, and schedule a retry

### Requirement: The system SHALL trigger notification events from business operations
Workflow submission and transition, invoice overdue checks, and lease expiration checks SHALL call the `Notifier.Enqueue` method to queue email notifications. The `Notifier` interface SHALL accept a `NotificationEvent` struct containing event type, aggregate type, aggregate ID, recipient addresses, template name, and template data. When the notifier is `nil` (email disabled), calls SHALL be silently skipped.

#### Scenario: Workflow approval requested triggers notification
- **WHEN** a workflow is submitted or advances to a new approval step and email is enabled
- **THEN** the system SHALL enqueue a `workflow.approval_requested` notification with recipient addresses resolved from the approver roles at the current workflow node

#### Scenario: Invoice overdue triggers payment reminder
- **WHEN** a scheduled check identifies an invoice that is overdue or approaching its due date and email is enabled
- **THEN** the system SHALL enqueue an `invoice.payment_reminder` notification with the customer contact email from the associated lease

#### Scenario: Lease approaching expiration triggers reminder
- **WHEN** a scheduled check identifies a lease expiring within a configured number of days and email is enabled
- **THEN** the system SHALL enqueue a `lease.expiration_reminder` notification addressed to the internal operations team

#### Scenario: Disabled notifier silently skips all events
- **WHEN** email configuration has `enabled: false` and a business operation would normally trigger a notification
- **THEN** the system SHALL skip the `Enqueue` call without error and the business operation SHALL complete normally

### Requirement: The system SHALL expose a notification history API with pagination
The backend SHALL provide a `GET /api/notifications` endpoint that returns a paginated list of notification outbox entries. The endpoint SHALL support optional query filters for `event_type`, `aggregate_type`, `aggregate_id`, and `status`. Each entry SHALL include its ID, event type, aggregate references, recipients, subject, status, attempt count, timestamps, and last error (if any).

#### Scenario: Paginated notification history returns entries
- **WHEN** an operator calls `GET /api/notifications?page=1&page_size=20`
- **THEN** the system SHALL return a paginated response containing notification outbox entries ordered by `created_at` descending, with total count, current page, and page size

#### Scenario: Filter by event type narrows results
- **WHEN** an operator calls `GET /api/notifications?event_type=workflow.approval_requested`
- **THEN** the system SHALL return only notification entries matching that event type

#### Scenario: Filter by status returns failed notifications
- **WHEN** an operator calls `GET /api/notifications?status=failed`
- **THEN** the system SHALL return only notification entries with `failed` or `dead` status, useful for troubleshooting

#### Scenario: Empty results return valid paginated response
- **WHEN** an operator queries notification history with filters that match no entries
- **THEN** the system SHALL return a valid paginated response with an empty items list and total of 0

### Requirement: The system SHALL configure email settings via YAML and environment variables
Email configuration SHALL be defined in an `EmailConfig` struct nested under the `email` key in the Viper config. Supported fields SHALL include `smtp_host`, `smtp_port`, `smtp_username`, `smtp_password`, `from_address`, `from_name`, `template_dir`, `enabled`, `max_retry_attempts`, `retry_interval_seconds`, `poll_interval_seconds`, and `batch_size`. Environment variables SHALL follow the `MI_EMAIL_` prefix convention (e.g., `MI_EMAIL_SMTP_HOST`, `MI_EMAIL_SMTP_PORT`).

#### Scenario: YAML config provides email defaults
- **WHEN** the backend starts with `email.enabled: true` and SMTP credentials in the YAML config file
- **THEN** the notification subsystem SHALL initialize with those settings and start the polling goroutine

#### Scenario: Environment variables override YAML config
- **WHEN** `MI_EMAIL_SMTP_HOST` and `MI_EMAIL_SMTP_PASSWORD` are set as environment variables
- **THEN** those values SHALL override the corresponding YAML config entries

#### Scenario: Missing SMTP credentials with enabled flag logs warning
- **WHEN** `email.enabled` is `true` but required SMTP fields (`smtp_host`, `smtp_port`, `from_address`) are empty
- **THEN** the system SHALL log a warning at startup and the polling goroutine SHALL record errors for each send attempt until valid credentials are provided

### Requirement: The system SHALL support an enabled/disabled toggle with no-op behavior
When `email.enabled` is `false` (the default), the notification subsystem SHALL operate as a no-op. The `Notifier` injected into business services SHALL be a `NoopNotifier` that silently discards all `Enqueue` calls. The polling goroutine SHALL NOT start. Business operations SHALL proceed normally without any notification side effects. When `email.enabled` is `true`, the real notifier and polling goroutine SHALL activate.

#### Scenario: Disabled by default requires no SMTP configuration
- **WHEN** the backend starts without any `email` configuration section
- **THEN** the system SHALL use a `NoopNotifier`, the polling goroutine SHALL NOT start, and all business operations SHALL work without email

#### Scenario: Enabling email activates notifier and polling goroutine
- **WHEN** an administrator sets `MI_EMAIL_ENABLED=true` and provides valid SMTP configuration
- **THEN** the system SHALL inject the real notifier into business services, start the polling goroutine, and begin processing pending notification entries

#### Scenario: Toggling from enabled to disabled at runtime requires restart
- **WHEN** email is enabled at startup and an administrator later changes the config to disabled
- **THEN** the change SHALL take effect on the next backend restart, consistent with Viper's config loading behavior
