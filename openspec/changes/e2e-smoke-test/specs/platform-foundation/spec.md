## ADDED Requirements

### Requirement: The project SHALL provide an E2E smoke test for the Lease→Invoice→Payment business chain
The backend SHALL include an integration test (behind `//go:build integration` tag) that exercises the complete operational chain through the HTTP API: login, create lease, submit for approval, approve workflow (all steps), generate charges, create invoice, submit invoice, approve invoice workflow, record payment, and verify receivable settlement.

#### Scenario: Full chain produces settled receivable
- **WHEN** the E2E smoke test creates a lease, submits it, approves it through the full workflow, generates charges for the lease period, creates an invoice from the charges, submits and approves the invoice, and records full payment
- **THEN** the receivable SHALL show `outstanding_amount` of 0 and `settlement_status` of "settled"

#### Scenario: Each intermediate step produces correct status transitions
- **WHEN** the test progresses through each step
- **THEN** the lease SHALL transition draft → pending_approval → active, and the invoice SHALL transition draft → pending_approval → approved
