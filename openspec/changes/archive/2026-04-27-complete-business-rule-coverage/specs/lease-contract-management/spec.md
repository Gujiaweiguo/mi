## ADDED Requirements

### Requirement: Lease contract subtypes SHALL preserve required non-membership business data
The system SHALL explicitly identify the supported lease contract subtype for each contract and SHALL capture subtype-specific fields required for standard leases, joint-operation contracts, ad-board contracts, and area/ground contracts.

#### Scenario: Operator creates a subtype-specific lease contract
- **WHEN** an operator creates a non-membership lease contract with a supported subtype
- **THEN** the system SHALL validate the subtype-specific mandatory fields before the contract can be submitted for approval

#### Scenario: Unsupported subtype data is not silently discarded
- **WHEN** a submitted contract includes subtype data not represented by the current model
- **THEN** the system SHALL reject the submission with field-level diagnostics instead of saving partial subtype data

### Requirement: Overtime billing SHALL generate auditable charges from approved overtime rules
The system SHALL support overtime billing for eligible lease contracts using configured overtime formulas, approval workflow, and duplicate-safe charge generation.

#### Scenario: Approved overtime bill creates charge lines
- **WHEN** an approved overtime bill covers a lease contract and billing period
- **THEN** the system SHALL generate overtime charge lines with formula inputs, calculated amount, approval reference, and audit history

#### Scenario: Duplicate overtime generation is blocked
- **WHEN** overtime charges have already been generated for the same contract, formula, and period
- **THEN** the system SHALL skip duplicate trusted output and report the skipped lines to the operator

#### Scenario: Overtime bill cancellation reverses future billing effect
- **WHEN** an approved overtime bill is cancelled or stopped before charge generation
- **THEN** the system SHALL prevent future overtime charges for the cancelled period without deleting historical audit records
