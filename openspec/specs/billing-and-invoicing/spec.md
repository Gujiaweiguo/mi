## Purpose

TBD: Canonical billing and invoicing spec for the replacement MI system.
## Requirements
### Requirement: The system SHALL generate billable charges from approved Lease state
The billing subsystem SHALL convert approved, billing-effective Lease contracts and supported rule definitions into deterministic charge lines for an accounting window. Charge generation SHALL ignore Lease records that are not yet billable under the Lease lifecycle contract, and reruns SHALL remain duplicate-safe for already generated accounting windows. The system SHALL reject stale or invalid generation requests whose source Lease state no longer matches billing-eligible conditions, and SHALL preserve existing generated results without creating duplicate charge lines.

#### Scenario: Monthly charge generation succeeds
- **WHEN** a valid approved Lease contract is processed for a billing period
- **THEN** the system SHALL generate the expected charge lines once without creating duplicates on rerun

#### Scenario: Non-billable Lease state is excluded
- **WHEN** charge generation evaluates a Lease contract that has not reached the required approved or billing-effective state
- **THEN** the system SHALL skip that contract instead of generating billable charge lines

#### Scenario: Stale eligibility request is rejected
- **WHEN** charge generation is triggered from an outdated request context after the Lease state has changed to a non-billable state
- **THEN** the system SHALL reject generation for that request and SHALL NOT create inconsistent or duplicate charge lines

### Requirement: The system SHALL manage bill and invoice lifecycle states
The first release SHALL support creation, numbering, approval, rejection, cancellation, allowed adjustments, receivable open-item creation, and payment application for bill and invoice documents required by the business flow. Document lifecycle and payment actions SHALL validate the current state before mutation, and replayed submit, approve, cancel, payment, or equivalent actions SHALL remain auditable while avoiding duplicate side effects or duplicate financial state transitions. When lifecycle actions are retried after partial progress or attempted from invalid states, the system SHALL deterministically reject invalid transitions and preserve existing valid receivable and document state.

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

### Requirement: The system SHALL rely on maintained supporting master data for billable customer-facing operations
First-release billing and invoicing workflows that depend on customer-facing supporting master data SHALL use records maintained through supported application workflows rather than assuming those records are immutable seed data or manually corrected through direct database edits. When a required supporting record is missing, invalid, or no longer operationally usable, the affected billing or invoicing workflow SHALL fail through supported validation instead of silently proceeding with inconsistent customer-facing data.

#### Scenario: Billing workflow uses maintained supporting customer data
- **WHEN** an operator creates or advances a billing or invoicing workflow that requires customer-facing supporting master data
- **THEN** the workflow SHALL use the maintained record state available through supported master-data administration surfaces

#### Scenario: Invalid supporting data blocks inconsistent financial workflow
- **WHEN** a billing or invoicing operation depends on a required supporting master-data record that is missing or operationally invalid
- **THEN** the system SHALL reject or block that operation through supported validation rather than producing an inconsistent customer-facing financial document
