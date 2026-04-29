## Purpose

Define the deterministic charge-generation, bill and invoice lifecycle, receivable, and payment behaviors that complete the first-release financial chain from approved Lease state through settlement.

## Requirements
### Requirement: The system SHALL generate billable charges from approved Lease state
The billing subsystem SHALL convert approved, billing-effective Lease contracts, supported rule definitions, and approved overtime-generated charges into deterministic billable charge lines for an accounting window. Charge generation SHALL ignore Lease or overtime records that are not yet billable under the Lease lifecycle and overtime approval contract, reruns SHALL remain duplicate-safe for already generated accounting windows, and downstream consumers SHALL retain enough source attribution to distinguish overtime-derived lines from standard lease-generated lines.

#### Scenario: Monthly standard charge generation succeeds
- **WHEN** a valid approved Lease contract is processed for a billing period
- **THEN** the system SHALL generate the expected charge lines once without creating duplicates on rerun

#### Scenario: Approved overtime charge is included in billable output
- **WHEN** an approved overtime bill has generated charges for the selected accounting window
- **THEN** the system SHALL expose those overtime-derived lines as eligible downstream billing input with source attribution preserved

#### Scenario: Non-billable Lease state is excluded
- **WHEN** charge generation evaluates a Lease contract that has not reached the required approved or billing-effective state
- **THEN** the system SHALL skip that contract instead of generating billable charge lines

#### Scenario: Unapproved or cancelled overtime output is excluded
- **WHEN** overtime source data has not completed approval or has been cancelled before valid downstream generation
- **THEN** the system SHALL exclude that overtime output from billable charge generation

#### Scenario: Stale eligibility request is rejected
- **WHEN** charge generation is triggered from an outdated request context after the Lease state has changed to a non-billable state
- **THEN** the system SHALL reject generation for that request and SHALL NOT create inconsistent or duplicate charge lines

### Requirement: Overtime-derived financial state SHALL remain visible in downstream review and bounded reporting
The system SHALL expose overtime-derived charge, invoice, and receivable state through the supported first-release operator-facing financial review surfaces and bounded reporting outputs that rely on billing and receivable data, without requiring operators to reconcile overtime through side-channel queries.

#### Scenario: Invoice and receivable review preserves overtime attribution
- **WHEN** overtime-derived charges are billed or invoiced through the supported downstream workflow
- **THEN** the system SHALL let operators review those financial records with enough attribution to identify the overtime source and approval lineage

#### Scenario: Bounded first-release reporting includes overtime-backed financial results
- **WHEN** a supported first-release finance or receivable report is generated from datasets that include overtime-derived billed state
- **THEN** the system SHALL include the overtime-backed financial amounts in the report output instead of omitting them from totals or detail rows

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

### Requirement: Invoice discounts SHALL be approved and reflected in receivables
The system SHALL support invoice or invoice-line discounts with configured rate or amount limits, approval audit, and deterministic receivable reduction.

#### Scenario: Approved discount reduces outstanding receivable
- **WHEN** an approved discount is applied to an open invoice line
- **THEN** the system SHALL reduce the outstanding receivable by the approved discount amount and preserve the original charge amount for audit

#### Scenario: Discount beyond allowed amount is rejected
- **WHEN** an operator submits a discount exceeding the configured rate or outstanding amount
- **THEN** the system SHALL reject the discount with a validation error and SHALL NOT mutate receivable balances

### Requirement: Surplus balances SHALL track and apply customer overpayments
The system SHALL track customer surplus balances created by overpayment or approved balance entry and SHALL allow authorized application to open receivables.

#### Scenario: Overpayment creates surplus balance
- **WHEN** a payment exceeds the selected open receivable balance
- **THEN** the system SHALL create a customer surplus balance for the unapplied amount instead of silently rejecting or losing the excess

#### Scenario: Surplus is applied to an open receivable
- **WHEN** an authorized operator applies available surplus to an open receivable
- **THEN** the system SHALL reduce both the surplus balance and receivable balance atomically with audit history

### Requirement: Late-payment interest SHALL be calculated from configured rates
The system SHALL support late-payment interest calculation for overdue receivables using configured grace days, rate, period, and rounding rules.

#### Scenario: Interest is generated for overdue receivable
- **WHEN** an open receivable exceeds its due date plus configured grace days
- **THEN** the system SHALL calculate interest from the configured rule and generate an auditable interest charge or document

#### Scenario: Interest generation is idempotent
- **WHEN** interest has already been generated for the same receivable and calculation period
- **THEN** the system SHALL not generate duplicate interest charges

### Requirement: Deposit application and refund SHALL be explicit financial events
The system SHALL support deposit application to receivables and deposit refund/release with authorization, audit history, and receivable impact.

#### Scenario: Deposit is applied to outstanding balance
- **WHEN** an authorized operator applies an available deposit to an open receivable
- **THEN** the system SHALL reduce deposit availability and receivable balance atomically

#### Scenario: Deposit refund is blocked when obligations remain
- **WHEN** an operator requests a deposit refund while the contract has unresolved receivables or pending financial workflows
- **THEN** the system SHALL reject the refund and explain the blocking obligations

### Requirement: Invoice adjustment SHALL be operator-reachable from the first-release frontend
The system SHALL expose invoice adjustment for adjustment-eligible approved billing documents from the authenticated frontend and SHALL let operators prepare replacement line amounts before submitting the adjustment request.

#### Scenario: Operator starts an adjustment from an eligible invoice document
- **WHEN** an operator opens an adjustment-eligible approved invoice document in the supported frontend review surfaces and selects the adjustment action
- **THEN** the system SHALL present an invoice adjustment drafting flow bound to the selected source document

#### Scenario: Adjustment draft preserves editable line context
- **WHEN** the adjustment drafting flow is displayed
- **THEN** the system SHALL show the source document lines with enough context for the operator to enter replacement amounts tied to the original billing charge lines

### Requirement: Invoice adjustment submission SHALL reuse the existing replacement-document flow
The frontend SHALL submit invoice adjustments through the existing invoice adjustment API contract and SHALL route the operator to the replacement draft document returned by the backend.

#### Scenario: Adjustment draft submits through the adjustment endpoint
- **WHEN** an operator submits a valid invoice adjustment draft from the frontend
- **THEN** the system SHALL call the invoice adjustment API for the selected source document instead of mutating the original document locally

#### Scenario: Successful adjustment opens the replacement draft
- **WHEN** the invoice adjustment API returns a replacement draft document successfully
- **THEN** the frontend SHALL navigate the operator to the returned replacement draft document so downstream review and resubmission can continue there

### Requirement: Adjusted invoice documents SHALL remain visible and understandable in review surfaces
Supported first-release invoice list and detail surfaces SHALL render adjusted document state and the relationship between an original document and its replacement draft.

#### Scenario: Adjusted invoice status remains visible in list and detail views
- **WHEN** an invoice document has entered the adjusted state
- **THEN** the frontend SHALL render that adjusted status with the same level of status visibility provided for other supported invoice lifecycle states

#### Scenario: Replacement draft shows its source document relationship
- **WHEN** a replacement draft document is viewed after a successful adjustment
- **THEN** the frontend SHALL display the originating adjusted document reference so the operator can understand the adjustment lineage

### Requirement: Operators SHALL create bill or invoice documents from selected charge lines
The billing charges view SHALL allow operators to select one or more charge lines from the charges table and create a bill or invoice document from those selected lines. The system SHALL present a document-type chooser before submission, call the existing invoice creation API, and provide success or validation feedback.

#### Scenario: Operator selects charge lines and creates a document
- **WHEN** the operator selects one or more charge lines in the billing charges table and activates the create-document action
- **THEN** the system SHALL present a dialog requiring the operator to choose between bill and invoice document types

#### Scenario: Operator confirms document creation
- **WHEN** the operator selects a document type in the dialog and confirms
- **THEN** the system SHALL call the invoice creation API with the selected document type and charge line IDs, set the action to a loading state, and prevent duplicate submissions

#### Scenario: Successful document creation
- **WHEN** the API returns a created document
- **THEN** the system SHALL display success feedback including the document ID and SHALL provide a navigation link to the created document detail view

#### Scenario: Ineligible charge lines produce validation feedback
- **WHEN** the API rejects the creation request because one or more selected charge lines are not eligible for invoicing
- **THEN** the system SHALL display error feedback with the API-provided validation message and SHALL NOT navigate away from the billing charges view

#### Scenario: Create action disabled when no rows selected
- **WHEN** no charge lines are selected in the table
- **THEN** the create-document action SHALL be disabled and non-interactive

#### Scenario: Selection cleared after successful creation
- **WHEN** a document is created successfully from selected charge lines
- **THEN** the system SHALL clear the table selection and refresh the charge line list to reflect updated state
