# Payment / Accounts Receivable Acceptance Summary (2026-04-11)

## Scope

This acceptance summary covers the payment and accounts receivable slice for `legacy-system-migration`, with focus on invoice-to-receivable booking, payment application with over-application guards, cumulative settlement tracking, cancel/adjustment guards, charge reuse prevention, bill/invoice numbering, adjustment lifecycle, and current-head verification evidence.

## Included commits

No new commits were required for this acceptance closure. The payment/AR capability was implemented in prior changes and is verified against current HEAD.

## Spec baseline

- `openspec/specs/workflow-approvals/spec.md` (idempotency and audit requirements)
- Invoice/billing document flow implicitly covered by `openspec/changes/archive/2026-04-04-legacy-system-migration/specs/`

## Acceptance evidence (current HEAD)

Current HEAD:

- `49e141e62d2f7400373edfe73f4f45043690a922`

Verification evidence:

- `artifacts/verification/49e141e62d2f7400373edfe73f4f45043690a922/unit.json` — PASS (50/50)
- `artifacts/verification/49e141e62d2f7400373edfe73f4f45043690a922/integration.json` — PASS (84/84)
- `artifacts/verification/49e141e62d2f7400373edfe73f4f45043690a922/e2e.json` — PASS (41/41)

## Executed checks

### Invoice service integration tests

1. `TestInvoiceServiceCreateApproveAndAudit`
   - Command:
     - `go test -tags=integration ./internal/invoice -run TestInvoiceServiceCreateApproveAndAudit -count=1`
   - Result: PASS
   - Full lifecycle: activate lease → generate charges → create invoice → submit for approval → approve → verify INV-101 numbering → verify receivable booked with outstanding amount 12000 and due date from period end → verify workflow audit entries

2. `TestInvoiceServiceBillNumberingAndCancel`
   - Command:
     - `go test -tags=integration ./internal/invoice -run TestInvoiceServiceBillNumberingAndCancel -count=1`
   - Result: PASS
   - Bill numbering: create bill → submit → approve → verify BIL-101 numbering → cancel → verify settled receivable → replay cancel → verify idempotent

3. `TestInvoiceServiceAdjustmentCreatesNewDraft`
   - Command:
     - `go test -tags=integration ./internal/invoice -run TestInvoiceServiceAdjustmentCreatesNewDraft -count=1`
   - Result: PASS
   - Adjustment: create invoice → approve → adjust amount → verify original marked adjusted with cleared receivable → submit adjusted → approve → verify new INV-102 number and new receivable with adjusted amount

4. `TestInvoiceServiceRejectResubmitAndPreventChargeReuse`
   - Command:
     - `go test -tags=integration ./internal/invoice -run TestInvoiceServiceRejectResubmitAndPreventChargeReuse -count=1`
   - Result: PASS
   - Charge reuse prevention: create invoice → attempt reuse → verify error → submit → reject → resubmit → approve → verify INV-101 after resubmit → replay submit → verify approved state preserved

5. `TestInvoiceServicePaymentApplicationAndReceivableGuards`
   - Command:
     - `go test -tags=integration ./internal/invoice -run TestInvoiceServicePaymentApplicationAndReceivableGuards -count=1`
   - Result: PASS
   - Payment guards: approve invoice → over-application rejected → partial payment (7000) → verify outstanding 5000 → replay payment (idempotent) → verify no duplicate → final payment (5000) → verify settled → cancel rejected (has payments) → adjust rejected (has payments)

### HTTP route integration tests

6. `TestIntegrationInvoiceReceivableRoutes`
   - Command:
     - `go test -tags=integration ./internal/http -run TestIntegrationInvoiceReceivableRoutes -count=1`
   - Result: PASS
   - HTTP receivable detail endpoint → receivable list with filters → payment endpoint → over-application returns 409

### E2E tests

7. `task20-payment-ar.spec.ts`
   - Command:
     - `npx playwright test e2e/task20-payment-ar.spec.ts`
   - Result: PASS
   - Receivable list view → receivable detail → partial payment (7000) → verify outstanding 5000 → over-application feedback → final payment (5000) → verify settled tag → verify settled receivable no longer in list

### Full verification gates (current HEAD)

8. Unit verification
   - Command:
     - `./scripts/verification/run-unit.sh 49e141e62d2f7400373edfe73f4f45043690a922`
   - Result: PASS

9. Integration verification
   - Command:
     - `./scripts/verification/run-integration.sh 49e141e62d2f7400373edfe73f4f45043690a922`
   - Result: PASS (84/84)

10. E2E verification
    - Command:
      - `./scripts/verification/run-e2e.sh 49e141e62d2f7400373edfe73f4f45043690a922`
    - Result: PASS (41/41)

11. CI gate
    - Command:
      - `./scripts/ci-ready.sh`
    - Result: PASS (`CI Ready: YES`)

12. Archive gate
    - Command:
      - `./scripts/archive-ready.sh`
    - Result: PASS (`Archive Ready: YES`)

## Acceptance outcomes

- Approved invoices automatically book receivable items with due dates derived from billing period end.
- Payment application reduces outstanding balance and tracks payment history with idempotency keys.
- Over-application is rejected: payment amounts exceeding outstanding balance return errors (409 at HTTP layer).
- Cumulative payments settle receivable: partial payments reduce outstanding, final payment marks settled.
- Payment replay is idempotent: duplicate payment requests with the same idempotency key are safe no-ops.
- Cancel and adjust guards: documents with recorded payments cannot be cancelled or adjusted.
- Bill/invoice numbering is sequential and deterministic upon approval (INV-101, BIL-101).
- Adjustment creates a new draft linked to the original; the original is marked adjusted with its receivable cleared.
- Charge reuse prevention: the same charge line cannot be used in multiple invoice documents.
- Cancel is idempotent: replayed cancel preserves the cancelled state.
- Rejected invoices can be resubmitted through workflow and approved with correct numbering.
- Frontend receivable list supports filtering; invoice detail shows receivable summary and payment form.
- Current HEAD is both `CI Ready` and `Archive Ready`.

## Traceability notes

- Invoice service (including payment/receivable): `backend/internal/invoice/service.go`
- Invoice AR repository: `backend/internal/invoice/ar_repository.go`
- Invoice data access: `backend/internal/invoice/repository.go`
- Invoice domain models: `backend/internal/invoice/model.go`
- Invoice integration tests: `backend/internal/invoice/service_integration_test.go`
- Invoice receivable HTTP routes: `backend/internal/http/router.go`
- Invoice receivable route integration tests: `backend/internal/http/invoice_receivable_integration_test.go`
- Invoice HTTP handler: `backend/internal/http/handlers/invoice.go`
- Frontend invoice API client: `frontend/src/api/invoice.ts`
- Payment/AR E2E: `frontend/e2e/task20-payment-ar.spec.ts`

## Conclusion

The payment / accounts receivable acceptance slice is accepted for the covered first-release scope. Invoice-to-receivable booking, payment application with over-application guards, cumulative settlement, cancel/adjustment guards, charge reuse prevention, bill/invoice numbering, and adjustment lifecycle all pass on HEAD `49e141e62d2f7400373edfe73f4f45043690a922`.
