## MODIFIED Requirements

### Requirement: The system SHALL manage bill and invoice lifecycle states
The first release SHALL support creation, numbering, approval, rejection, cancellation, allowed adjustments, receivable open-item creation, and payment application for bill and invoice documents required by the business flow. Document lifecycle and payment actions SHALL validate the current state before mutation, and replayed submit, approve, cancel, payment, or equivalent actions SHALL remain auditable while avoiding duplicate side effects or duplicate financial state transitions. When lifecycle actions are retried after partial progress or attempted from invalid states, the system SHALL deterministically reject invalid transitions and preserve existing valid receivable and document state. When a scheduled check identifies an invoice that is overdue or approaching its due date and email notifications are enabled, the system SHALL enqueue an `invoice.payment_reminder` notification event addressed to the customer contact email from the associated lease. The notification SHALL be enqueued within the same transaction as the invoice status check update.

#### Scenario: Approved invoice is numbered, auditable, and creates receivable balance
- **WHEN** a valid invoice is created from generated charges and approved
- **THEN** the system SHALL assign a deterministic document number, persist audit history, expose the approved document for downstream output generation, and create or update the corresponding receivable open item with the approved outstanding balance

#### Scenario: Payment application reduces receivable balance deterministically
- **WHEN** an operator records a valid payment against an approved bill or invoice that still has outstanding receivable balance
- **THEN** the system SHALL apply the payment audibly, reduce the open-item outstanding balance by the applied amount, and preserve the remaining balance for aging and arrears reporting

#### Scenario: Fully paid receivable no longer remains outstanding
- **WHEN** cumulative applied payments reach the full outstanding amount of the receivable open item
- **THEN** the system SHALL mark the receivable as fully settled for downstream reporting and SHALL NOT continue exposing it as an outstanding aging item

#### Scenario: Over-application is rejected
- **WHEN** an operator attempts to apply a payment larger than the remaining outstanding receivable balance for a bill or invoice
- **THEN** the system SHALL reject the payment application and SHALL preserve the existing receivable balance

#### Scenario: Duplicate invoice submission is safe
- **WHEN** a submit-for-approval action is replayed for an invoice that has already been submitted or approved
- **THEN** the system SHALL preserve the existing document state and SHALL NOT create duplicate workflow, numbering, or receivable-booking side effects

#### Scenario: Invalid lifecycle action is rejected
- **WHEN** an operator attempts a bill or invoice lifecycle action that is not allowed from the document's current state
- **THEN** the system SHALL reject the action and SHALL preserve the existing document state

#### Scenario: Retry after partial progress does not duplicate receivable mutation
- **WHEN** a lifecycle or payment command is retried after the previous attempt already applied a valid state mutation
- **THEN** the system SHALL preserve the current valid document and receivable state and SHALL NOT apply duplicate financial mutations

#### Scenario: Overdue invoice triggers payment reminder notification
- **WHEN** a scheduled check identifies an approved invoice whose due date has passed and the outstanding balance is greater than zero and email notifications are enabled
- **THEN** the system SHALL enqueue an `invoice.payment_reminder` notification with the customer contact email from the associated lease as the recipient

#### Scenario: Near-due invoice triggers payment reminder notification
- **WHEN** a scheduled check identifies an approved invoice whose due date is within a configured threshold (e.g., 7 days) and the outstanding balance is greater than zero and email notifications are enabled
- **THEN** the system SHALL enqueue an `invoice.payment_reminder` notification with the customer contact email from the associated lease as the recipient

#### Scenario: Fully paid invoice does not trigger payment reminder
- **WHEN** a scheduled check identifies an invoice whose receivable is fully settled
- **THEN** the system SHALL NOT enqueue a payment reminder notification for that invoice

#### Scenario: Payment reminder enqueue failure does not block invoice operations
- **WHEN** a scheduled check processes invoices but the notification enqueue call fails for a specific invoice
- **THEN** the system SHALL log the notification error and the invoice check SHALL continue processing other invoices; the notification failure SHALL NOT block the scheduled check
