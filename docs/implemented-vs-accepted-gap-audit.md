# Implemented vs Accepted Gap Audit

This audit compares current repository implementation anchors with explicit acceptance evidence for the first-release non-membership scope.

## Authority and scope

- Canonical requirements: `openspec/specs/`
- Baseline traceability: `docs/capability-traceability-matrix.md`
- Priority audit order: `Lease -> Billing/Invoice -> Payment/AR -> R01-R19`
- Exclusions remain unchanged: membership / `Associator`, and reports outside `R01-R19`

## Status vocabulary

- `accepted` — explicit acceptance closure exists in repository evidence or acceptance summary docs
- `implemented-not-accepted` — implementation and verification anchors exist, but this audit cannot point to explicit acceptance closure
- `spec-defined-only` — canonical OpenSpec defines the behavior, but this audit does not claim repository implementation closure

Gap disposition:

- `fix-now` — required before treating the current HEAD as release-ready for the audited scope
- `non-go-live-exception` — documented exception is required if not fixed before go-live
- `none` — no blocking audit gap for the governed scope

## Current-head evidence posture

- Current repository HEAD: `006ef508f6a68156972b52657decabdc7de3ec99`
- Current HEAD now has commit-scoped verification evidence under `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/`.
- `scripts/archive-ready.sh` passes on current HEAD (`Archive Ready: YES`).
- The latest validated release-ready summary documented in-repo remains `docs/release-ready-summary-2026-04-19.md` for commit `896212969a988f63309591e8815795b98337aadb`.
- Report-specific historical acceptance closure remains anchored at `b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264` via `docs/report-acceptance-release-summary-2026-04-11.md` and `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/report-acceptance-traceability.md`.

The original current-head evidence drift is now closed for archive-ready validation on the audited scope.

## Priority chain audit

| Audit item | Canonical spec anchor | Implementation anchor | Verification anchor | Acceptance evidence | Audit status | Gap disposition | Follow-up item |
|---|---|---|---|---|---|---|---|
| Lease contract lifecycle | `openspec/specs/lease-contract-management/spec.md` | `backend/internal/lease/`, `backend/internal/http/handlers/lease.go`, `frontend/src/views/LeaseListView.vue`, `LeaseCreateView.vue`, `LeaseDetailView.vue` | `backend/internal/lease/service_test.go`, `backend/internal/lease/service_integration_test.go`, `backend/internal/http/handlers/lease_test.go`, `frontend/e2e/task15-workflow.spec.ts` | `docs/lease-bill-regression-acceptance-2026-04-10.md`, `docs/release-ready-summary-2026-04-11.md`, `docs/release-ready-summary-2026-04-19.md`, `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/` | accepted | none | No blocking gap remains for archive-ready scope on current HEAD. |
| Joint venture / ad-board / area contract variants | `openspec/specs/lease-contract-management/spec.md`, `openspec/specs/billing-and-invoicing/spec.md` | `backend/internal/lease/`, `backend/internal/billing/`, `frontend/src/views/LeaseCreateView.vue`, `LeaseDetailView.vue` | `backend/internal/lease/service_integration_test.go`, `backend/internal/billing/service_integration_test.go`, `frontend/e2e/task15-workflow.spec.ts` | `docs/release-ready-summary-2026-04-19.md` explicitly maps `JV / ad / area contract variants` to the current lease model | accepted | none | None for first-release scope. |
| Formula, pricing, and billing parameter management | `openspec/specs/lease-contract-management/spec.md`, `openspec/specs/billing-and-invoicing/spec.md` | `backend/internal/lease/`, `backend/internal/billing/`, `frontend/src/views/BillingChargesView.vue` | `backend/internal/billing/service_test.go`, `backend/internal/billing/service_integration_test.go`, `backend/internal/http/handlers/billing_test.go` | `docs/lease-bill-regression-acceptance-2026-04-10.md`, `docs/release-ready-summary-2026-04-11.md` | accepted | none | None for governed scope; legacy formula behavior is already absorbed into lease and billing rules. |
| Charge generation, bill, and invoice lifecycle | `openspec/specs/billing-and-invoicing/spec.md` | `backend/internal/billing/`, `backend/internal/invoice/`, `frontend/src/views/BillingChargesView.vue`, `BillingInvoicesView.vue` | `backend/internal/billing/service_test.go`, `backend/internal/billing/service_integration_test.go`, `backend/internal/invoice/service_test.go`, `backend/internal/http/handlers/invoice_test.go` | `docs/lease-bill-regression-acceptance-2026-04-10.md`, `docs/payment-ar-acceptance-summary-2026-04-11.md`, `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/` | accepted | none | No blocking gap remains for archive-ready scope on current HEAD. |
| Payment, collection, and financial operations | `openspec/specs/billing-and-invoicing/spec.md`, `openspec/specs/supporting-domain-management/spec.md` | `backend/internal/invoice/`, `frontend/src/views/ReceivablesView.vue`, `InvoiceDetailView.vue`, `frontend/src/api/invoice.ts` | `backend/internal/invoice/service_integration_test.go`, `backend/internal/http/invoice_receivable_integration_test.go`, `frontend/e2e/task20-payment-ar.spec.ts` | `docs/payment-ar-acceptance-summary-2026-04-11.md`, `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/` | accepted | none | Archive-ready evidence is now present on current HEAD; extra named sub-scenario tests remain optional hardening work. |
| Workflow / approval engine | `openspec/specs/workflow-approvals/spec.md` | `backend/internal/workflow/`, `backend/internal/notification/`, `frontend/src/views/WorkflowAdminView.vue`, `NotificationsView.vue` | `backend/internal/workflow/service_test.go`, `backend/internal/workflow/service_integration_test.go`, `backend/internal/http/workflow_reminder_integration_test.go`, `frontend/e2e/task15-workflow.spec.ts`, `frontend/e2e/task19-workflow-admin.spec.ts` | `docs/workflow-acceptance-summary-2026-04-11.md`, `docs/lease-bill-regression-acceptance-2026-04-10.md`, `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/` | accepted | none | Workflow acceptance is now backed by current-head archive-ready evidence again. |

## Frozen report audit (`R01-R19`)

Report acceptance remains bounded by:

- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`
- `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`
- `openspec/specs/supporting-domain-management/spec.md`
- `openspec/specs/report-output-localization/spec.md`

| Audit item | Canonical spec anchor | Implementation anchor | Verification anchor | Acceptance evidence | Audit status | Gap disposition | Follow-up item |
|---|---|---|---|---|---|---|---|
| `R01-R18` tabular frozen report block | `openspec/specs/supporting-domain-management/spec.md`, `openspec/specs/report-output-localization/spec.md` | `backend/internal/reporting/service.go`, `frontend/src/views/GeneralizeReportsView.vue` | `backend/internal/reporting/service_integration_test.go`, `frontend/e2e/task16-reporting.spec.ts`, `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/report-acceptance-traceability.md` | `docs/report-acceptance-release-summary-2026-04-11.md`, the historical traceability snapshot, and `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/` | accepted | none | No blocking gap remains for archive-ready scope on current HEAD. |
| `R19` visual report block | `openspec/specs/supporting-domain-management/spec.md`, `openspec/specs/report-output-localization/spec.md` | `backend/internal/reporting/service.go`, `frontend/src/views/VisualShopAnalysisView.vue` | `frontend/e2e/task16-r19-visual.spec.ts`, `artifacts/verification/b5089530c8bd79c47670a6c7fdd1fcc9a2fa8264/report-acceptance-traceability.md` | `docs/report-acceptance-release-summary-2026-04-11.md`, `docs/release-ready-summary-2026-04-19.md`, and `artifacts/verification/006ef508f6a68156972b52657decabdc7de3ec99/` | accepted | none | Archive-ready evidence is now current; a manual visual spot-check remains optional release hardening. |

## Gap register

### Fix-now

- None. The previous current-head evidence drift is closed by the new verification artifacts and `Archive Ready: YES` result on `006ef508f6a68156972b52657decabdc7de3ec99`.

### Non-go-live exceptions

- None identified for the governed first-release scope.

## Closure criteria for this audit

This audit can be considered closed when all of the following are true:

1. The priority chain rows remain `accepted` with no spec-defined-only regressions.
2. `R01-R19` remains fully accepted for the frozen report scope.
3. Current HEAD has commit-scoped verification evidence in `artifacts/verification/<current-head>/`.
4. `docs/go-live-checklist.md` can be satisfied using current-commit evidence rather than only historical validated commits.

These closure criteria are now satisfied for archive-ready validation on current HEAD.

## Recommended next action

Already completed on current HEAD `006ef508f6a68156972b52657decabdc7de3ec99`:

```bash
scripts/verification/run-unit.sh
scripts/verification/run-integration.sh
scripts/verification/run-e2e.sh
scripts/archive-ready.sh
```

If release confidence is required beyond archive-ready evidence, follow with the supported cutover rehearsal path documented in `docs/go-live-checklist.md` and `docs/verification-gates.md`.
