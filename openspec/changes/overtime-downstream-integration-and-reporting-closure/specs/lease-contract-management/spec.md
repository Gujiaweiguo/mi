## MODIFIED Requirements

### Requirement: Overtime billing SHALL generate auditable charges from approved overtime rules
The system SHALL support overtime billing for eligible lease contracts using configured overtime formulas, approval workflow, duplicate-safe charge generation, and downstream-consumable charge provenance that can participate in billing and invoice workflows without manual re-entry.

#### Scenario: Approved overtime bill creates downstream-consumable charge lines
- **WHEN** an approved overtime bill covers a lease contract and billing period
- **THEN** the system SHALL generate overtime charge lines with formula inputs, calculated amount, approval reference, audit history, and source metadata required for downstream billing and invoicing

#### Scenario: Duplicate overtime generation is blocked
- **WHEN** overtime charges have already been generated for the same contract, formula, and period
- **THEN** the system SHALL skip duplicate trusted output and report the skipped lines to the operator

#### Scenario: Overtime bill cancellation reverses future billing effect
- **WHEN** an approved overtime bill is cancelled or stopped before charge generation
- **THEN** the system SHALL prevent future overtime charges for the cancelled period without deleting historical audit records
