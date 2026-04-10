# Lease → Bill/Invoice Regression Acceptance (2026-04-10)

## Scope

This acceptance run validates the first-release priority chain from Lease lifecycle to charge generation, bill/invoice lifecycle, and receivable settlement behavior, with focus on replay safety and stale-state rejection.

## Spec coverage baseline

- `openspec/specs/lease-contract-management/spec.md`
- `openspec/specs/billing-and-invoicing/spec.md`
- `openspec/specs/workflow-approvals/spec.md`

## Executed checks

### Backend integration

1. Lease lifecycle acceptance
   - Command:
     - `go test -tags integration ./internal/lease -run "TestLeaseServiceCreateSubmitAndActivate|TestLeaseServiceRejectsTerminateOnAlreadyTerminatedLease|TestLeaseServiceRejectsAmendmentOnTerminatedLease"`
   - Result: PASS

2. Billing generation and boundary acceptance
   - Command:
     - `go test -tags integration ./internal/billing -run "TestBillingServiceGenerateChargesAndDeduplicate|TestBillingServiceExcludesPendingApprovalLease|TestBillingServiceProratesTerminationCutoff"`
   - Result: PASS

3. Invoice/receivable lifecycle acceptance
   - Command:
     - `go test -tags integration ./internal/invoice -run "TestInvoiceServiceCreateApproveAndAudit|TestInvoiceServiceRejectResubmitAndPreventChargeReuse|TestInvoiceServicePaymentApplicationAndReceivableGuards"`
   - Result: PASS

### Frontend E2E (workflow + receivable)

4. Lease-to-billing workflow + payment/AR flow
   - Command:
     - `npm run test:e2e -- --grep "task 15 lease to billing output workflow|payment entry, settlement updates"`
   - Result: PASS (2 passed)

### Verification gates (current HEAD)

5. CI gate
   - Command:
     - `scripts/ci-ready.sh`
   - Result: PASS (`CI Ready: YES`)

6. Archive gate
   - Command:
     - `scripts/archive-ready.sh`
   - Result: PASS (`Archive Ready: YES`)

## Acceptance outcomes

- Lease state transitions remain guarded; invalid stale-state operations are rejected while current valid state is preserved.
- Billing generation remains duplicate-safe and excludes non-billable lease states.
- Replay on invoice/payment-related paths does not duplicate receivable mutations.
- End-to-end workflow and AR payment experience remains operational for first-release chain.

## Traceability notes

- Lease replay/stale-state safeguards are implemented in:
  - `backend/internal/lease/service.go`
  - `backend/internal/lease/service_integration_test.go`
- Billing stale candidate rejection is implemented in:
  - `backend/internal/billing/service.go`
  - `backend/internal/billing/repository.go`
- Invoice/AR replay-safe receivable behavior is enforced in:
  - `backend/internal/invoice/ar_repository.go`
  - `backend/internal/invoice/service_integration_test.go`

## Conclusion

Lease → Bill/Invoice critical chain passes this regression acceptance run for the covered first-release scenarios and remains ready for continued iterative hardening.
