## ADDED Requirements

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
