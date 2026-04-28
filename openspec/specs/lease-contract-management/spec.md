## Purpose

Define the first-release Lease contract lifecycle, billing-effective state rules, and replay-safe mutations that drive downstream workflow and billing behavior.

## Requirements
### Requirement: The system SHALL support Lease contract lifecycle management
The first release SHALL support creation, submission, approval, activation, amendment, termination, and querying of Lease contracts needed by the operational business flow. Lifecycle commands SHALL validate the current Lease state before mutating the contract, and duplicate submissions or other replayed actions SHALL preserve the existing valid state instead of creating duplicate downstream effects. The system SHALL reject lifecycle mutations that are requested from stale or no-longer-eligible states and SHALL preserve the latest valid Lease state without generating duplicate workflow or billing-trigger side effects.

#### Scenario: Contract reaches active state
- **WHEN** an operator creates a Lease contract and the required approvals complete successfully
- **THEN** the contract SHALL enter an active state and SHALL become eligible for downstream billing

#### Scenario: Duplicate submission is safe
- **WHEN** a submit action is replayed for a Lease contract that is already pending approval, approved, or otherwise past the initial submit step
- **THEN** the system SHALL preserve the existing lifecycle state and SHALL NOT create a duplicate workflow side effect

#### Scenario: Invalid termination is blocked
- **WHEN** an operator attempts to terminate a Lease contract from a state that is not allowed to terminate under first-release rules
- **THEN** the system SHALL reject the mutation and SHALL preserve the existing contract state

#### Scenario: Replay after approval does not create duplicate billing trigger
- **WHEN** a previously processed lifecycle command is replayed after the contract already reached a billing-eligible state
- **THEN** the system SHALL preserve the current valid state and SHALL NOT create duplicate downstream billing-trigger side effects

### Requirement: The system SHALL preserve billing-relevant contract state changes
Amendments, terminations, and other supported Lease lifecycle changes SHALL update the billing-effective contract state used by downstream charge generation and invoice flows. Lease records that have not reached the required approved or billing-effective lifecycle point SHALL NOT be treated as eligible billing inputs. If a Lease state changes between request creation and billing evaluation, the system SHALL use the latest persisted valid lifecycle state and SHALL reject stale transitions instead of generating inconsistent billing inputs.

#### Scenario: Approved amendment changes billing inputs
- **WHEN** an approved contract amendment updates a billing-effective field
- **THEN** future charge generation SHALL use the amended contract values

#### Scenario: Pending approval contract is not billable
- **WHEN** charge generation evaluates a Lease contract that is still pending approval or otherwise not billing-effective
- **THEN** the system SHALL exclude that contract from downstream billing generation

#### Scenario: Stale lifecycle transition request is rejected
- **WHEN** a lifecycle mutation is requested using an outdated source state after a newer valid state has already been persisted
- **THEN** the system SHALL reject the stale mutation and SHALL preserve the latest valid billing-effective Lease state

### Requirement: Lease contract subtypes SHALL preserve required non-membership business data
The system SHALL explicitly identify the supported lease contract subtype for each contract and SHALL capture subtype-specific fields required for standard leases, joint-operation contracts, ad-board contracts, and area/ground contracts.

#### Scenario: Operator creates a subtype-specific lease contract
- **WHEN** an operator creates a non-membership lease contract with a supported subtype
- **THEN** the system SHALL validate the subtype-specific mandatory fields before the contract can be submitted for approval

#### Scenario: Unsupported subtype data is not silently discarded
- **WHEN** a submitted contract includes subtype data not represented by the current model
- **THEN** the system SHALL reject the submission with field-level diagnostics instead of saving partial subtype data

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
