## Context

The current first-release financial chain already supports charge generation, bill/invoice creation, workflow submission, approval, cancellation, and adjustment. In `backend/internal/invoice/service.go`, the approved-state transition is centralized in `SyncWorkflowState`, which allocates document numbers and updates document status after workflow approval. However, that lifecycle currently stops at document state mutation: it does not create or update receivable records, and there is no supported payment-recording path.

At the same time, receivable-oriented reporting is already live at the query layer. The `ar_open_items` schema was introduced in `backend/internal/platform/database/migrations/000013_ar_open_item_schema.up.sql`, and aging/arrears reports in `backend/internal/reporting/repository.go` already read from it for outputs such as R08, R09, R16, R17, and R18. Today those receivable rows are not owned by an operational lifecycle and are seeded manually in reporting integration tests, which means the reporting surface is structurally present but not yet fed by the real billing/invoice workflow.

This change is therefore cross-cutting but still bounded: it must extend the existing Go modular-monolith billing/invoice flow so approved financial documents create receivable open items, operators can record payments against outstanding balances, and downstream aging outputs consume trustworthy operational data. The design must stay within first-release non-membership scope and avoid introducing bank integration, external payment gateways, or historical migration logic.

## Goals / Non-Goals

**Goals:**
- Make approved bills and invoices create deterministic receivable open items as part of the existing invoice workflow lifecycle.
- Keep receivable balance as a real operational source of truth so current aging and arrears reports can rely on live workflow-driven data instead of manual seeding.
- Add an auditable operator payment flow that reduces outstanding balance, rejects over-application, and remains replay-safe.
- Preserve current modular-monolith boundaries by extending existing financial workflow surfaces instead of introducing unrelated infrastructure.
- Expose operator-facing receivable query and payment entry behavior needed by the supporting-domain spec.

**Non-Goals:**
- No bank integration, payment gateway, reconciliation import, or external settlement provider.
- No membership-domain financial behavior.
- No redesign of the workflow engine or document approval model.
- No historical receivable migration from the legacy system.
- No widening of reporting scope beyond making existing first-release receivable-dependent outputs trustworthy.

## Decisions

### 1. Keep receivable lifecycle ownership in the existing invoice workflow path

**Decision:** Receivable booking remains owned by the current bill/invoice lifecycle, not by a new top-level financial subsystem. Approval-driven receivable creation will be triggered from the same transactional flow that currently finalizes document approval in `invoice.Service.SyncWorkflowState`, and cancellation/adjustment behavior will be enforced from the existing invoice service methods.

**Why:** The repository already has a single place where a document becomes approved and receives its final document number. That is the safest place to attach receivable side effects while preserving replay safety and current lifecycle invariants. It also keeps the change aligned with first-release scope: receivables are a downstream consequence of approved billing documents, not an independent product surface.

**Alternatives considered:**
- Create a new top-level payment/AR bounded context and move billing approval side effects into cross-module orchestration. Rejected because it adds architectural weight before the repo has a real need for a separate financial subsystem.

### 2. Treat `ar_open_items` as the outstanding-balance source of truth and add a minimal payment ledger for auditability

**Decision:** `ar_open_items` remains the source of truth for current outstanding balances used by aging queries, while a new payment-entry table is added to preserve auditable payment history. The payment ledger records operator-entered payment events and enough linkage to reconstruct who applied what, when, and against which approved financial document/open items.

**Why:** The current schema can express outstanding balances but cannot express payment history, replay detection, or settlement audit. The change requirements explicitly require auditable payment application, while the reports already depend on current outstanding amounts. Keeping one table optimized for current balance and another for immutable payment history avoids overloading `ar_open_items` with event-log responsibilities.

**Alternatives considered:**
- Record only the latest outstanding balance and skip a payment ledger. Rejected because it cannot satisfy auditable payment history requirements.
- Derive outstanding balances entirely from payment events plus original document totals. Rejected because existing reporting already reads `ar_open_items` directly, and rewriting all reporting around event reconstruction would widen scope unnecessarily.

### 3. Create receivable rows at document-line granularity, not one row per document

**Decision:** Approval should create or upsert receivable open items at a granularity that preserves downstream reporting dimensions already present on `ar_open_items`, especially `charge_type`, `trade_id`, due date, customer, and department. In practice, the receivable booking step should derive rows from approved document lines (or a deterministic grouping that preserves those dimensions), not collapse the entire document into a single aggregate receivable row.

**Why:** Bills and invoices can contain multiple lines with different charge types. Aging outputs such as R09 and R17 already group arrears by charge type, and `ar_open_items` already carries `charge_type` and trade-related reporting keys. A single document-level receivable row would erase those distinctions and force later reporting workarounds.

**Alternatives considered:**
- Create one `ar_open_items` row per billing document. Rejected because mixed-charge documents would lose charge-type-level receivable semantics that reporting already expects.

### 4. Payment application must be transactional, deterministic, and replay-safe

**Decision:** Payment recording will run inside a database transaction that locks the affected outstanding receivable rows, validates remaining balance before mutation, records an immutable payment entry, applies the amount across targeted open items using a deterministic ordering rule, and commits both ledger and balance updates together.

The first-release allocation rule will be deterministic and simple: when a payment is recorded against an approved bill or invoice, it is applied only to that document’s still-open receivable rows, ordered consistently by due date, then charge type, then open-item identifier. Replayed requests must not produce duplicate payment entries or duplicate balance reductions.

**Why:** The spec requires over-application rejection, full-settlement semantics, and replay safety. A transactional apply path is the only reliable way to prevent double reduction or partial ledger/balance divergence.

**Alternatives considered:**
- Apply payments with best-effort sequential updates and write payment history separately. Rejected because a mid-flight failure would desynchronize audit history from outstanding balances.
- Allow free-form cross-document payment allocation in first release. Rejected because that widens operator workflow and complicates deterministic balance semantics beyond current scope.

### 5. Fully settled items are represented by zero outstanding balance and excluded from outstanding queries

**Decision:** Settlement semantics are defined primarily by `outstanding_amount = 0` for the affected open items, with query logic and operator surfaces treating zero-balance items as settled/non-outstanding. If a settlement timestamp or equivalent helper field is added, it is supportive metadata rather than a second source of truth.

**Why:** The existing reporting contract already depends on outstanding balances, and the delta spec defines settlement in terms of no longer remaining outstanding. Using zero balance as the primary invariant keeps the design simple and backward-compatible with current reporting reads.

**Alternatives considered:**
- Introduce a separate lifecycle status enum for open items and make reports depend on that status. Rejected because it duplicates information already captured by outstanding balance and would require broader reporting changes.

### 6. Reuse the existing billing/invoice permission family and add financial-supporting routes near current surfaces

**Decision:** The first-release API/UI surface for receivable review and payment entry will be added alongside existing financial routes and permission families rather than introducing a brand-new permission namespace. Operator-facing endpoints for recording payments and querying receivables should sit near the current invoice/billing surface and reuse the existing authenticated financial operator model.

**Why:** This change extends the existing financial workflow rather than introducing a separate organizational role model. Keeping the routes and permissions near current billing/invoice behavior minimizes rollout complexity and keeps the operator experience coherent.

**Alternatives considered:**
- Create a new standalone permission family such as `accounts.receivable`. Rejected for first release because the repo currently models this work under the existing billing/invoice financial workflow.

### 7. Reporting should continue reading `ar_open_items`, but paid/received metrics must stop inferring settlement from document approval alone

**Decision:** Aging and arrears outputs continue to use `ar_open_items` as their outstanding-balance substrate. Where report logic currently treats approved billing artifacts as “received” proxies, payment-ledger-backed aggregation should become the source for actual receipt/payment figures when this change is implemented.

**Why:** The proposal explicitly closes the gap between approved documents and real receivable/payment lifecycle. Once payments are first-class events, “approved” is no longer the same thing as “received,” and reporting should not keep conflating those concepts.

**Alternatives considered:**
- Leave received/paid metrics unchanged and only improve outstanding balance. Rejected because it would keep a core financial metric semantically wrong after the payment workflow exists.

## Risks / Trade-offs

- **[Risk] Existing `ar_open_items` schema is too thin for audit-friendly payment behavior** → **Mitigation:** add a minimal immutable payment-entry table and only add helper columns to `ar_open_items` when they materially reduce query ambiguity.
- **[Risk] Mixed-charge documents make payment allocation/reporting ambiguous** → **Mitigation:** book AR at line-preserving granularity and define a deterministic in-document allocation order before implementation.
- **[Risk] Cancel/adjust after payment can create difficult financial edge cases** → **Mitigation:** keep first-release behavior strict; if needed, reject cancel/adjust actions once payment has been applied until explicit reversal/rebalancing logic is implemented.
- **[Risk] Duplicate workflow sync or repeated payment submission can double-book balances** → **Mitigation:** keep both approval booking and payment application idempotent, transaction-protected, and keyed by replay-safe request semantics.
- **[Risk] UI may imply broader financial operations than backend supports** → **Mitigation:** scope the operator flow narrowly to document-linked payment entry and outstanding receivable review only.

## Migration Plan

1. Add the schema changes required for auditable payment tracking and any supporting AR indexing/metadata needed for deterministic lifecycle management.
2. Extend invoice lifecycle persistence so workflow approval books receivable open items transactionally and replay-safely.
3. Extend invoice lifecycle behavior for cancellation/adjustment so AR state remains consistent with allowed document transitions.
4. Add backend payment-recording and receivable-query APIs within the existing financial/operator surface.
5. Add frontend operator views/API bindings for outstanding receivable review and payment entry.
6. Add integration coverage for approval booking, duplicate approval replay safety, payment application, over-application rejection, full settlement, and document-action restrictions after payment.
7. Update report-oriented verification so receivable-dependent outputs remain consistent with workflow-driven AR data.
8. Before any archive step, regenerate commit-scoped unit/integration/e2e evidence for the current HEAD SHA.

Rollback remains manageable because the change is additive to the current billing/invoice flow. If payment entry proves unstable, the new routes and UI controls can be disabled while the existing bill/invoice lifecycle continues operating; however, approval-side AR booking and reporting expectations should be deployed together to avoid partially trustworthy financial outputs.

## Open Questions

- What exact first-release due-date derivation should be used when creating receivable open items from approved documents and lines?
- If an approved document has received a partial payment, should first-release cancellation/adjustment be rejected outright, or should the change also include explicit reversal/rebalancing logic?
- Does the payment ledger need per-open-item application rows in first release, or is one payment entry plus deterministic document-local balance mutation sufficient for current reporting and audit requirements?
