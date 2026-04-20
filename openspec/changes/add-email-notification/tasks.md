## 1. Foundation

- [x] 1.1 Add `gopkg.in/mail.v2` dependency to `go.mod`; verify with `go mod tidy` and `go build ./...`
- [x] 1.2 Add `EmailConfig` struct to `config.Config` with `mapstructure` tags for all fields (`smtp_host`, `smtp_port`, `smtp_username`, `smtp_password`, `from_address`, `from_name`, `template_dir`, `enabled`, `max_retry_attempts`, `retry_interval_seconds`, `poll_interval_seconds`, `batch_size`); ensure `Enabled` defaults to `false`
- [x] 1.3 Update `development.yaml` with a commented-out `email` section showing all available fields with disabled-by-default values
- [x] 1.4 Create golang-migrate migration (next sequential number) for `notification_outbox` table with columns: `id`, `event_type`, `aggregate_type`, `aggregate_id`, `recipient_to`, `recipient_cc`, `subject`, `template_name`, `template_data` (JSON), `status` ENUM (`pending`, `sending`, `sent`, `failed`, `dead`), `attempt_count`, `max_attempts`, `next_attempt_at`, `sent_at`, `last_error`, `created_at`, `updated_at`; include indexes on `(status, next_attempt_at)` and `(aggregate_type, aggregate_id)`; include a down migration that drops the table

## 2. Core Package — Models

- [x] 2.1 Create `backend/internal/notification/` package with `model.go`: define `OutboxEntry` struct (Gorm model matching the migration columns), `NotificationEvent` struct (EventType, AggregateType, AggregateID, RecipientTo, RecipientCc, Subject, TemplateName, TemplateData), and status constants (`StatusPending`, `StatusSending`, `StatusSent`, `StatusFailed`, `StatusDead`)
- [x] 2.2 Add typed template data structs in `model.go`: `WorkflowApprovalData` (DocumentType, DocumentNumber, SubmitterName, ApprovalStepName, ApprovalLink), `InvoiceReminderData` (InvoiceNumber, CustomerName, AmountDue, DueDate, DaysOverdue), `LeaseExpirationData` (ContractNumber, TenantName, ExpirationDate, DaysRemaining)

## 3. Core Package — Repository

- [x] 3.1 Create `repository.go` in the notification package: implement `Repository` with methods `InsertOutbox(ctx, tx *sql.Tx, entry *OutboxEntry) error`, `FetchPending(ctx, db *sql.DB, batchSize int) ([]*OutboxEntry, error)` (using `SELECT ... WHERE status='pending' AND (next_attempt_at IS NULL OR next_attempt_at <= NOW()) ORDER BY created_at ASC LIMIT ? FOR UPDATE SKIP LOCKED`), `UpdateStatus(ctx, entry *OutboxEntry) error`, and `ListOutbox(ctx, db *sql.DB, params) ([]*OutboxEntry, int, error)` for paginated history queries

## 4. Core Package — SMTP Sender

- [x] 4.1 Create `sender.go` in the notification package: implement `Sender` interface with `Send(ctx, to []string, cc []string, subject, htmlBody string) error` and a concrete `SMTPSender` struct using `gopkg.in/mail.v2`; support STARTTLS and implicit TLS; catch and return all SMTP errors without panicking; log send outcome via Zap

## 5. Core Package — Template Renderer

- [x] 5.1 Create `renderer.go` in the notification package: implement `Renderer` struct that parses all `.html` files from the configured template directory at startup using `html/template`; cache parsed templates; provide `RenderSubject(templateName string) (string, error)` and `RenderBody(templateName string, data any) (string, error)` methods; return a clear error when template name is not found
- [x] 5.2 Define template file convention: each template file contains both a `{{define "subject"}}...{{end}}` block and a `{{define "body"}}...{{end}}` block; document this convention in a comment in `renderer.go`

## 6. Core Package — Service and Interface

- [x] 6.1 Create `service.go` in the notification package: implement `Service` struct holding `Repository`, `Sender`, `Renderer`, `*sql.DB`, `EmailConfig`, and `*zap.Logger`; implement `Enqueue(ctx context.Context, tx *sql.Tx, event NotificationEvent) error` that builds an `OutboxEntry` from the event, renders subject and body, and inserts via `Repository.InsertOutbox`
- [x] 6.2 Add `ProcessOutbox(ctx context.Context) error` method on `Service`: fetch pending entries via `Repository.FetchPending`, iterate and send each via `Sender.Send`, update status to `sent` on success (set `sent_at`) or `failed` on error (increment `attempt_count`, record `last_error`, compute `next_attempt_at`); move to `dead` status when `attempt_count >= max_attempts`; handle context cancellation for graceful shutdown
- [x] 6.3 Create `notifier.go` in the notification package: define `Notifier` interface with `Enqueue(ctx context.Context, tx *sql.Tx, event NotificationEvent) error`; implement `NoopNotifier` that returns `nil` for all calls; implement adapter that wraps `*Service` to satisfy the `Notifier` interface with a nil-receiver check that silently skips when the underlying service is nil

## 7. Email Templates

- [x] 7.1 Create `templates/email/workflow_approval_request.html` with subject block "Approval Request: {{.DocumentType}} {{.DocumentNumber}}" and body block containing document type, document number, submitter name, approval step name, and a clickable approval link
- [x] 7.2 Create `templates/email/invoice_payment_reminder.html` with subject block "Payment Reminder: Invoice {{.InvoiceNumber}}" and body block containing invoice number, customer name, amount due, due date, and days overdue
- [x] 7.3 Create `templates/email/lease_expiration_reminder.html` with subject block "Lease Expiration Reminder: {{.ContractNumber}}" and body block containing contract number, tenant name, expiration date, and days remaining

## 8. Integration — Wire Notifier into Business Services

- [x] 8.1 Add `notifier Notifier` field to workflow service struct; in `Start` method, after successful workflow submission, enqueue `workflow.approval_requested` event with approver emails resolved from the current workflow node roles; wrap enqueue in a deferred recovery so notification failure does not roll back the workflow transition (per spec: "Notification enqueue failure does not block workflow transition")
- [x] 8.2 In workflow `transition` method, after an approval advances the workflow to a subsequent step, enqueue `workflow.approval_requested` event addressed to approvers at the new step; same error isolation as task 8.1
- [x] 8.3 Add `notifier Notifier` field to invoice service struct; in the scheduled invoice check logic, when an approved invoice is overdue or near-due with outstanding balance greater than zero, enqueue `invoice.payment_reminder` with the customer contact email from the associated lease; skip fully paid invoices; wrap enqueue errors so the scheduled check continues processing other invoices
- [x] 8.4 Add `notifier Notifier` field to lease service struct; in the scheduled lease check logic, when an active lease is within the configured expiration threshold, enqueue `lease.expiration_reminder` addressed to the configured internal operations team recipients; skip expired, terminated, or non-active leases; wrap enqueue errors so the scheduled check continues processing other leases

## 9. API — Notification History Handler

- [x] 9.1 Create `backend/internal/notification/handler.go`: implement `NotificationHandler` struct with a `Repository` dependency; add `ListNotifications(c *gin.Context)` method that parses query params (`page`, `page_size`, `event_type`, `aggregate_type`, `aggregate_id`, `status`), calls `Repository.ListOutbox`, and returns a paginated JSON response with items, total count, page, and page size
- [x] 9.2 Register `GET /api/notifications` route in `router.go` with the `NotificationHandler`; add the route within the authenticated middleware group following the existing handler registration pattern

## 10. Bootstrap — Polling Goroutine and Graceful Shutdown

- [ ] 10.1 Create `poller.go` in the notification package: implement a `StartPoller(ctx context.Context, svc *Service, interval time.Duration)` function that runs a ticker loop calling `svc.ProcessOutbox(ctx)` at the configured interval; respect context cancellation for clean shutdown; log poll cycle outcomes via Zap
- [x] 10.2 Wire notification bootstrap in the application startup: when `EmailConfig.Enabled` is `true`, construct `Renderer` (parse templates), `SMTPSender`, `Repository`, and `Service`; inject the `Notifier` into workflow, invoice, and lease services; start the polling goroutine via `StartPoller`; when `Enabled` is `false`, inject `NoopNotifier` and skip goroutine startup
- [x] 10.3 Add notification polling goroutine to graceful shutdown: when SIGTERM/SIGINT is received, cancel the poller context so the goroutine finishes its current send attempt and exits before the shutdown timeout; add a log line confirming poller stopped

## 11. Unit Tests

- [ ] 11.1 Write unit tests for `Renderer`: verify each template renders with correct subject and body for valid data; verify missing template name returns a clear error; verify startup fails fast when template directory is empty or missing
- [ ] 11.2 Write unit tests for `Service.Enqueue`: verify outbox entry is built correctly from a `NotificationEvent`; verify it calls `Repository.InsertOutbox` with the right fields
- [ ] 11.3 Write unit tests for `Service.ProcessOutbox`: mock `Sender` and `Repository`; verify happy path (pending to sent), SMTP failure (pending to failed, attempt_count incremented, next_attempt_at set), max retries exceeded (failed to dead), and context cancellation stops processing mid-batch
- [ ] 11.4 Write unit tests for `NoopNotifier`: verify `Enqueue` returns nil without calling any downstream method
- [ ] 11.5 Write unit tests for `NotificationHandler.ListNotifications`: verify pagination params parsed correctly, filter params passed to repository, empty results return valid response with total 0
- [ ] 11.6 Record unit test evidence at `artifacts/verification/<commit-sha>/unit.json`; CI gate requires this evidence to pass

## 12. Integration Tests

- [ ] 12.1 Write integration test for enqueue-within-transaction: using Testcontainers-Go with MySQL, begin a transaction, call `Enqueue` with a `workflow.approval_requested` event, commit, and verify the `notification_outbox` row exists with status `pending` and correct event data
- [ ] 12.2 Write integration test for end-to-end outbox processing: enqueue a notification, run `ProcessOutbox` with a mock SMTP server (or test SMTP), verify the entry transitions to `sent` with `sent_at` populated
- [ ] 12.3 Write integration test for retry and dead-letter: enqueue a notification, run `ProcessOutbox` with SMTP configured to fail, verify the entry moves to `failed`, run again enough times to exceed `max_attempts`, verify status becomes `dead`
- [ ] 12.4 Write integration test for `GET /api/notifications`: seed outbox entries with various statuses, call the endpoint with pagination and filters, verify response shape and filtering correctness
- [ ] 12.5 Record integration test evidence at `artifacts/verification/<commit-sha>/integration.json`; CI gate requires this evidence to pass

## 13. Verification

- [x] 13.1 Run `go build ./...` and confirm zero compilation errors
- [ ] 13.2 Run `go test ./backend/internal/notification/...` and confirm all tests pass
- [x] 13.3 Run `go vet ./backend/internal/notification/...` and confirm no issues
- [ ] 13.4 Confirm evidence files exist: `artifacts/verification/<commit-sha>/unit.json` and `artifacts/verification/<commit-sha>/integration.json` (required for CI gate); `artifacts/verification/<commit-sha>/e2e.json` required for archive gate if e2e tests are added in a future iteration
