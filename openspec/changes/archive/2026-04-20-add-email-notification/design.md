## Context

The backend has no notification capability. When a workflow approval is needed, an invoice approaches its due date, or a lease nears expiration, the only way to find out is by logging into the UI and checking manually. This creates real operational risk: missed approvals delay downstream work, overdue invoices go unpaid, and expiring leases catch teams off guard.

The proposal (`proposal.md`) defines this as the highest-value second-release feature. The scope covers SMTP-based email sending, an async outbox for reliability, three initial HTML templates, a notification history API, and YAML + environment variable configuration.

The backend is a Go modular monolith using Gin, Gorm, Viper, and Zap. Services follow a consistent pattern: a struct holding a repository and a `*sql.DB`, with business logic methods. The workflow package already uses an outbox pattern for domain events, which provides a proven model for the notification outbox. The config struct (`config.Config`) uses nested structs with `mapstructure` tags and the `MI_` env prefix.

Stakeholders:

- Operators who need timely alerts about approvals, invoices, and leases.
- System administrators who will configure SMTP credentials in deployment.
- Developers who will extend the notification set in future iterations.

## Goals / Non-Goals

**Goals:**

- Add a self-contained `notification` package that handles email composition, queuing, and delivery.
- Use a database-backed outbox to ensure emails survive process restarts and can be retried on failure.
- Provide three HTML email templates for the initial notification events: workflow approval request, invoice payment reminder, and lease expiration reminder.
- Integrate with existing services (workflow, invoice, lease) through a narrow interface that keeps coupling low.
- Expose a read-only notification history API for operators and troubleshooting.
- Add SMTP configuration to the existing Viper-based config system.

**Non-Goals:**

- No email template editor UI. Templates are files managed by deployment.
- No marketing or bulk email campaigns.
- No SMS or push notifications.
- No email open or click tracking.
- No unsubscribe management. These are operational notifications, not marketing.
- No third-party email API integrations (SendGrid API, SES API, etc.). Standard SMTP only.
- No frontend changes in this iteration. The notification history API is backend-only for now.

## Decisions

### Decision 1: Use `gopkg.in/mail.v2` for SMTP sending

The email sender will use the `gopkg.in/mail.v2` library rather than the standard library `net/smtp`.

Why:

- `mail.v2` handles SMTP AUTH (PLAIN, LOGIN, CRAM-MD5) automatically, which avoids the boilerplate and footguns of `net/smtp` where you must manually manage the AUTH handshake.
- It supports STARTTLS and implicit TLS out of the box, which covers both cloud SMTP relays (Alibaba DirectMail, SendGrid) and self-hosted Postfix.
- The API is simple: construct a `mail.Message`, set headers and body, dial and send. No need to manage connections or handle SMTP protocol details.
- The library is mature, well-maintained, and widely used in Go projects.

Alternatives considered:

- `net/smtp` (standard library): works but requires manual AUTH handling, connection management, and error recovery. The boilerplate outweighs the benefit of zero dependencies for this use case.
- `github.com/go-gomail/gomail`: the predecessor to `mail.v2`, no longer actively maintained.
- Third-party REST APIs (SendGrid SDK, AWS SES SDK): rejected because the proposal explicitly scopes to SMTP-only to keep the system provider-agnostic.

### Decision 2: Database-backed outbox with polling goroutine

Outbound emails will be inserted into a `notification_outbox` table within the same transaction as the triggering business operation. A background goroutine polls the table at a configurable interval, picks up pending rows, sends them, and marks them as sent or failed.

Why:

- The workflow package already uses this exact pattern (`workflow_outbox` table, `InsertOutbox` within business transactions). Reusing the proven approach keeps the architecture consistent.
- Transactional consistency: the outbox row is committed alongside the business change, so emails are never lost due to a crash between the business operation and the send attempt.
- Retry is natural: failed sends increment `attempt_count`, and a max-attempts threshold moves them to a dead-letter state.
- No external message broker dependency. MySQL handles persistence, and the goroutine is lightweight.

Alternatives considered:

- Immediate synchronous send in the HTTP handler: rejected because SMTP latency (often 1-5 seconds per email) would slow down API responses, and failures would require retry logic in the handler.
- Redis queue: rejected because it adds an infrastructure dependency that does not exist in the current stack. The database is already there and reliable.
- RabbitMQ / NATS: same concern as Redis. Not worth the operational overhead for a monolith sending a few dozen operational emails per day.

The polling goroutine will be started in the application bootstrap (alongside the existing workflow reminder scheduler) and will use `SELECT ... WHERE status = 'pending' AND (next_attempt_at IS NULL OR next_attempt_at <= NOW()) ORDER BY created_at ASC LIMIT ?` with row-level locking (`FOR UPDATE SKIP LOCKED`) to allow safe horizontal scaling if multiple backend instances run later.

### Decision 3: Go `html/template` with on-disk template files

Email templates will be Go `html/template` files stored in a configurable directory (defaulting to `templates/email/`). Each template will have a subject line and an HTML body section.

Why:

- `html/template` is part of the standard library, handles HTML escaping, and is familiar to any Go developer on the team.
- Storing templates on disk means they can be updated per deployment without recompiling. A mount point in Docker Compose overrides the default templates.
- Three templates is a small enough set that a file-based approach is simple and maintainable. Each template gets its own file (e.g., `workflow_approval_request.html`, `invoice_payment_reminder.html`, `lease_expiration_reminder.html`).

Template data structures will be defined per notification type (e.g., `WorkflowApprovalData`, `InvoiceReminderData`, `LeaseExpirationData`) so the template has typed fields rather than a generic map.

Alternatives considered:

- Templates embedded in Go source code as string constants: rejected because changes require recompilation, and the HTML is hard to read and edit as escaped strings.
- Database-stored templates: rejected for the first iteration. It adds complexity (CRUD UI, versioning) that is not needed for three operational templates.
- Third-party template engines (Pongo2, Jet): rejected because the standard library is sufficient and avoids an extra dependency for what amounts to simple variable interpolation in HTML.

### Decision 4: Interface-based integration with a `Notifier` interface

Business services (workflow, invoice, lease) will depend on a `Notifier` interface defined in the `notification` package:

```go
type Notifier interface {
    Enqueue(ctx context.Context, tx *sql.Tx, event NotificationEvent) error
}
```

Each business service that triggers notifications will have a `notifier Notifier` field (which may be `nil`, in which case notifications are silently skipped). The `Enqueue` method inserts a row into `notification_outbox` using the caller's transaction, ensuring atomicity.

Why:

- The interface keeps the `notification` package decoupled from specific business logic. Business services only know about `NotificationEvent`, which is a simple struct with event type, recipient addresses, template name, and template data.
- The `nil` check means notifications can be disabled in environments that do not configure SMTP (development, staging) without any code changes.
- Testing is straightforward: pass a mock or no-op notifier.

The notification event types for the first iteration:

| Event Type | Trigger | Recipients |
|---|---|---|
| `workflow.approval_requested` | Workflow submitted or advanced to a new step | Approvers at the current node (resolved by role/department) |
| `invoice.payment_reminder` | Scheduled check for overdue or near-due invoices | Customer contact email from the lease |
| `lease.expiration_reminder` | Scheduled check for leases expiring within N days | Internal operations team |

Alternatives considered:

- Direct function call to a concrete `notification.Service`: rejected because it creates a hard import dependency from every business package to the notification package.
- Event bus / pub-sub: rejected because it adds infrastructure. The outbox pattern already provides decoupling through the database. A full event bus is overkill for three event types.
- Webhook callbacks: rejected because this is a monolith, not a distributed system. In-process method calls are simpler and faster.

### Decision 5: Config section under `email` with Viper struct tags

A new `EmailConfig` struct will be added to `config.Config`:

```go
type EmailConfig struct {
    SMTPHost        string `mapstructure:"smtp_host"`
    SMTPPort        int    `mapstructure:"smtp_port"`
    SMTPUsername    string `mapstructure:"smtp_username"`
    SMTPPassword    string `mapstructure:"smtp_password"`
    FromAddress     string `mapstructure:"from_address"`
    FromName        string `mapstructure:"from_name"`
    TemplateDir     string `mapstructure:"template_dir"`
    Enabled         bool   `mapstructure:"enabled"`
    MaxRetryAttempts int   `mapstructure:"max_retry_attempts"`
    RetryIntervalSeconds int `mapstructure:"retry_interval_seconds"`
    PollIntervalSeconds  int `mapstructure:"poll_interval_seconds"`
    BatchSize       int    `mapstructure:"batch_size"`
}
```

Environment variables follow the existing `MI_` prefix convention: `MI_EMAIL_SMTP_HOST`, `MI_EMAIL_SMTP_PORT`, etc. The `enabled` flag defaults to `false`, so existing deployments continue to work without any SMTP configuration.

Why:

- This follows the exact pattern used by `WorkflowReminderSchedulerConfig`, `StorageConfig`, and other nested config sections.
- Viper's `AutomaticEnv()` with the `MI_` prefix and `.` to `_` replacer handles the env var mapping without custom code.
- The `enabled` flag provides a clean kill switch. When `false`, the notifier is a no-op and the polling goroutine does not start.

### Decision 6: Database schema with two tables

Two new tables will be added via a golang-migrate migration:

```sql
CREATE TABLE notification_outbox (
    id              BIGINT AUTO_INCREMENT PRIMARY KEY,
    event_type      VARCHAR(100) NOT NULL,
    aggregate_type  VARCHAR(50) NOT NULL,
    aggregate_id    BIGINT NOT NULL,
    recipient_to    TEXT NOT NULL,
    recipient_cc    TEXT,
    subject         VARCHAR(500) NOT NULL,
    template_name   VARCHAR(100) NOT NULL,
    template_data   JSON NOT NULL,
    status          ENUM('pending','sending','sent','failed','dead') NOT NULL DEFAULT 'pending',
    attempt_count   INT NOT NULL DEFAULT 0,
    max_attempts    INT NOT NULL DEFAULT 5,
    next_attempt_at DATETIME NULL,
    sent_at         DATETIME NULL,
    last_error      TEXT NULL,
    created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_outbox_status_next (status, next_attempt_at),
    INDEX idx_outbox_aggregate (aggregate_type, aggregate_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
```

Why:

- `notification_outbox` is the queue table. It stores everything needed to compose and send the email independently of the triggering service.
- `recipient_to` and `recipient_cc` store email addresses as comma-separated strings. This avoids a separate recipients table for the first iteration, where each notification has a small, fixed set of recipients.
- `template_data` is JSON, allowing flexible per-event data without schema changes.
- The status enum covers the full lifecycle: pending, sending (in-flight lock), sent, failed (retryable), dead (exceeded max attempts).
- `next_attempt_at` enables exponential backoff on retries without overloading the SMTP server.

No separate `notification_recipients` table is needed in the first iteration. Recipients are resolved at enqueue time (the business service knows who should receive the notification) and stored directly in the outbox row. This can be normalized later if the recipient model becomes more complex.

### Decision 7: Notification history API endpoint

A single read-only endpoint will expose sent notification history:

```
GET /api/notifications?page=1&page_size=20&event_type=&aggregate_type=&aggregate_id=&status=
```

The endpoint returns a paginated list of outbox entries with their status, timestamps, and error details. It lives under a new `NotificationHandler` in the handlers package, following the same pattern as other handlers (interface dependency, Gin route registration in `router.go`).

Why:

- Operators need visibility into what was sent, what failed, and why.
- The existing handler pattern (interface, constructor, route group) keeps the codebase consistent.
- Read-only means no mutation risk. Failed emails can be manually re-queued in a future iteration.

## Risks / Trade-offs

- **[Risk] SMTP credentials in environment variables may leak in process listings** -> **Mitigation:** use Docker secrets or a secrets manager in production. The `MI_EMAIL_SMTP_PASSWORD` env var follows the same pattern as `MI_DATABASE_PASSWORD`, which already exists. Document the recommended approach in deployment docs.
- **[Risk] Polling goroutine may fall behind under high notification volume** -> **Mitigation:** the system is expected to send a few dozen operational emails per day, not thousands. The `batch_size` and `poll_interval_seconds` config knobs allow tuning. If volume grows, `FOR UPDATE SKIP LOCKED` allows multiple backend instances to share the load.
- **[Risk] Template changes on disk require no restart but could be picked up mid-send** -> **Mitigation:** templates are parsed and cached at startup. A template reload endpoint or signal-based reload can be added later. For the first iteration, a deployment restart is acceptable.
- **[Risk] Resolving approver email addresses at enqueue time means role changes are not reflected** -> **Mitigation:** this is acceptable for operational notifications. If an approver changes, the next notification will pick up the new email. Historical accuracy (who was notified) is preserved because the outbox row stores the resolved addresses.
- **[Trade-off] JSON template_data is flexible but untyped** -> **Mitigation:** each event type defines a Go struct for template data. The struct is marshaled to JSON for storage and unmarshaled by type when rendering. This gives type safety at the code level while keeping the schema generic.

## Migration Plan

1. Add `gopkg.in/mail.v2` dependency to `go.mod`.
2. Create `backend/internal/notification/` package with model types (`NotificationEvent`, `OutboxEntry`), repository, sender (SMTP client), template renderer, and service.
3. Add `EmailConfig` struct to `config.Config` and update `development.yaml` with a disabled-by-default email section.
4. Create golang-migrate migration (next sequential number) for `notification_outbox` table.
5. Create default HTML email templates in `templates/email/` for the three initial event types.
6. Add `Notifier` interface to the notification package and a `NoopNotifier` for when email is disabled.
7. Wire the notification service in `router.go`: construct the service, register the handler, inject the notifier into workflow/invoice/lease services.
8. Add notification enqueue calls in workflow `Start` and `transition` methods (approval requested events), invoice `SyncWorkflowState` (payment reminder candidates), and lease `SyncWorkflowState` (expiration reminder candidates).
9. Start the outbox polling goroutine in the application bootstrap, alongside the existing workflow reminder scheduler.
10. Add `GET /api/notifications` handler with pagination and filtering.
11. Write unit tests for template rendering, outbox processing, and SMTP mocking.
12. Write integration tests for the enqueue-within-transaction flow.

Rollback: remove the `notification` package, the config section, the migration (run down), and the handler registration. The business services gracefully handle a `nil` notifier, so removing the injection has no impact on existing functionality.

## Open Questions

- Should the polling goroutine use exponential backoff for individual failed emails (e.g., 1min, 5min, 15min, 1hr, 4hr), or is a fixed retry interval sufficient for the first iteration?
- Should the invoice payment reminder and lease expiration reminder be triggered by the same polling mechanism as the outbox sender (a scheduled check), or should they be triggered inline during billing/lease operations? The former is more decoupled but requires a separate scheduler; the latter is simpler but ties notification logic into business handlers.
