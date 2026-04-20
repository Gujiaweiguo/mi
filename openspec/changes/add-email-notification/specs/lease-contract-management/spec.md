## MODIFIED Requirements

### Requirement: The system SHALL support Lease contract lifecycle management
The first release SHALL support creation, submission, approval, activation, amendment, termination, and querying of Lease contracts needed by the operational business flow. Lifecycle commands SHALL validate the current Lease state before mutating the contract, and duplicate submissions or other replayed actions SHALL preserve the existing valid state instead of creating duplicate downstream effects. The system SHALL reject lifecycle mutations that are requested from stale or no-longer-eligible states and SHALL preserve the latest valid Lease state without generating duplicate workflow or billing-trigger side effects. When a scheduled check identifies an active lease approaching its expiration date and email notifications are enabled, the system SHALL enqueue a `lease.expiration_reminder` notification event addressed to the internal operations team. The notification SHALL be enqueued within the same transaction as the lease status evaluation.

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

#### Scenario: Lease approaching expiration triggers reminder notification
- **WHEN** a scheduled check identifies an active lease whose expiration date is within a configured threshold (e.g., 30 days) and email notifications are enabled
- **THEN** the system SHALL enqueue a `lease.expiration_reminder` notification addressed to the configured internal operations team recipients

#### Scenario: Expired or terminated lease does not trigger expiration reminder
- **WHEN** a scheduled check evaluates a lease that is already expired, terminated, or otherwise not active
- **THEN** the system SHALL NOT enqueue an expiration reminder notification for that lease

#### Scenario: Expiration reminder enqueue failure does not block lease operations
- **WHEN** a scheduled check processes leases but the notification enqueue call fails for a specific lease
- **THEN** the system SHALL log the notification error and the lease check SHALL continue processing other leases; the notification failure SHALL NOT block the scheduled check
