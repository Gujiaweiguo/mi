## Purpose

TBD: Canonical tax, document, and Excel output spec for the replacement MI system.

## Requirements

### Requirement: The system SHALL calculate and export mandatory tax outputs
The first release SHALL provide rule-based tax calculation inputs and mandatory tax export outputs for supported financial documents without relying on device or desktop-client integration.

#### Scenario: Tax export is generated for approved invoices
- **WHEN** an operator runs the mandatory tax export for a valid date range containing approved invoices
- **THEN** the system SHALL generate the configured export artifact with the required fields, ordering, and totals

### Requirement: The system SHALL generate mandatory print and document outputs
The first release SHALL generate Lease, Bill, Invoice, and other frozen output-catalog documents as print-ready HTML or PDF artifacts.

#### Scenario: Invoice print output is available
- **WHEN** an approved invoice is selected for printing
- **THEN** the system SHALL generate a print-ready document containing the required document fields and totals

### Requirement: The system SHALL support mandatory Excel import and export flows
The first release SHALL support the Excel import and export flows frozen in the output catalog, with validation errors surfaced before invalid data is committed.

#### Scenario: Invalid import is rejected
- **WHEN** an Excel import contains duplicate business keys or malformed required values
- **THEN** the system SHALL reject the batch and SHALL provide row-level validation feedback without committing partial data by default
