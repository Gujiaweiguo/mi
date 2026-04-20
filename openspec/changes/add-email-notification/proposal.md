## Why

The first release explicitly excluded email delivery from scope. The system currently has no way to notify operators or stakeholders about time-sensitive events such as workflow approval requests, invoice due dates, or lease expirations. Users must log in and check the UI manually, which creates risk of missed approvals and delayed payments.

Email notification is the highest-value second-release feature because it directly improves operational responsiveness without adding new business modules.

## What Changes

- Add an email delivery subsystem to the Go backend using SMTP (compatible with standard mail servers and cloud email services like Alibaba Cloud DirectMail, SendGrid, or Amazon SES).
- Define a set of notification events triggered by existing business operations (workflow approval needed, invoice overdue, lease approaching expiration, payment received).
- Add email template management with Go `html/template` for HTML emails, stored on disk and overridable per deployment.
- Send emails asynchronously via a persistent queue (database-backed outbox pattern) to ensure reliability and retry on failure.
- Track delivery status and expose a basic notification history in the backend API.
- Add email configuration to the backend config file and environment variables (SMTP host, port, credentials, from address).

## Capabilities

### New Capabilities
- `email-notification`: Covers SMTP integration, async sending via outbox, template management, delivery tracking, and notification event triggers.

### Modified Capabilities
- `workflow`: Workflow state transitions (submit, approve, reject) will trigger notification events.
- `billing.invoice`: Invoice lifecycle events (created, overdue approaching) will trigger notification events.
- `lease`: Lease lifecycle events (expiration approaching) will trigger notification events.
- `platform-foundation`: Backend config gains SMTP section; database gains notification outbox tables.

## Impact

- Affected code: new `backend/internal/notification` package; modifications to workflow, invoice, and lease services to emit notification events.
- Affected dependencies: Go standard library `net/smtp` or `gopkg.in/mail.v2` for SMTP; no external SaaS dependencies required.
- Affected database: new migration adding `notification_outbox` and `notification_recipients` tables.
- Affected config: `backend/config/*.yaml` gains `email` section; env vars for SMTP credentials.
- Affected tests: new unit tests for template rendering, outbox processing, and SMTP client mocking.
- Affected deployment: production env must include SMTP credentials.

## Scope Boundaries

### In Scope (First Iteration)
- SMTP-based email sending (works with any standard mail server or cloud SMTP relay)
- Async outbox pattern with retry
- HTML email templates for: workflow approval request, invoice payment reminder, lease expiration reminder
- Notification history API (list sent notifications)
- Configuration via YAML + env vars

### Out of Scope
- Email template editor UI (templates are files managed by deployment)
- Marketing/bulk email campaigns
- SMS or push notifications
- Email open/click tracking
- Unsubscribe management (these are operational notifications, not marketing)
- Third-party email API integrations (use standard SMTP)
