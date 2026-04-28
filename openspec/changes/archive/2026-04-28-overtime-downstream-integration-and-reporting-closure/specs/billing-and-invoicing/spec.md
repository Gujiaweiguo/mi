## MODIFIED Requirements

### Requirement: The system SHALL generate billable charges from approved Lease state
The billing subsystem SHALL convert approved, billing-effective Lease contracts, supported rule definitions, and approved overtime-generated charges into deterministic billable charge lines for an accounting window. Charge generation SHALL ignore Lease or overtime records that are not yet billable under the Lease lifecycle and overtime approval contract, reruns SHALL remain duplicate-safe for already generated accounting windows, and downstream consumers SHALL retain enough source attribution to distinguish overtime-derived lines from standard lease-generated lines.

#### Scenario: Monthly standard charge generation succeeds
- **WHEN** a valid approved Lease contract is processed for a billing period
- **THEN** the system SHALL generate the expected standard charge lines once without creating duplicates on rerun

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

## ADDED Requirements

### Requirement: Overtime-derived financial state SHALL remain visible in downstream review and bounded reporting
The system SHALL expose overtime-derived charge, invoice, and receivable state through the supported first-release operator-facing financial review surfaces and bounded reporting outputs that rely on billing and receivable data, without requiring operators to reconcile overtime through side-channel queries.

#### Scenario: Invoice and receivable review preserves overtime attribution
- **WHEN** overtime-derived charges are billed or invoiced through the supported downstream workflow
- **THEN** the system SHALL let operators review those financial records with enough attribution to identify the overtime source and approval lineage

#### Scenario: Bounded first-release reporting includes overtime-backed financial results
- **WHEN** a supported first-release finance or receivable report is generated from datasets that include overtime-derived billed state
- **THEN** the system SHALL include the overtime-backed financial amounts in the report output instead of omitting them from totals or detail rows
