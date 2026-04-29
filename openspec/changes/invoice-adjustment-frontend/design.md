## Context

`openspec/specs/billing-and-invoicing/spec.md` already includes allowed invoice adjustments in the first-release lifecycle, and the runtime stack is mostly ready: `backend/internal/invoice/service.go` implements `Adjust`, `backend/internal/http/handlers/invoice.go` exposes `POST /invoices/{id}/adjust`, and `frontend/src/api/invoice.ts` already exports `adjustInvoice()`. The missing layer is the operator-facing frontend: `frontend/src/views/InvoiceDetailView.vue` currently exposes submit, cancel, payment, discount, surplus, interest, deposit, and refund flows, but no adjustment action; `frontend/src/views/BillingInvoicesView.vue` likewise omits adjustment entry and does not understand the `adjusted` status.

This is a focused frontend closure. It should reuse existing invoice detail/list surfaces, avoid backend contract changes, and keep the adjustment workflow aligned with the current approved-document review experience.

## Goals / Non-Goals

**Goals:**

- Add an operator-facing adjustment action for adjustment-eligible approved invoice documents.
- Let operators edit replacement line amounts through a focused frontend adjustment form before submitting the adjustment.
- Submit adjustment drafts through the existing `adjustInvoice()` API and route the operator to the returned replacement draft document.
- Surface `adjusted` status and adjusted-from relationships in supported invoice list/detail review surfaces.
- Add focused frontend tests for adjustment entry, submission, and adjusted status visibility.

**Non-Goals:**

- No backend service, API, workflow, or database changes.
- No changes to payment, discount, surplus, interest, deposit, or cancellation semantics beyond existing adjustment eligibility checks.
- No new standalone invoice-adjustment page if the existing detail/list surfaces can host the flow.
- No historical diff viewer between original and replacement invoice documents.

## Decisions

### Decision: Launch adjustment from `InvoiceDetailView.vue` and keep `BillingInvoicesView.vue` as a lightweight entry surface

- **Chosen:** add the full adjustment form and submit flow in `InvoiceDetailView.vue`, while `BillingInvoicesView.vue` only gains status visibility and a convenient navigation/entry action.
- **Why:** invoice detail already holds document lines, receivable guards, and success/error feedback; that makes it the natural place to collect replacement line amounts without duplicating financial context in the list view.
- **Alternative considered:** implement adjustment directly inside the invoice list row actions.
- **Why not:** line-by-line amount editing needs full document context and would overload the list view with modal-only business logic.

### Decision: Use an inline adjustment panel/dialog instead of a new route or page

- **Chosen:** keep the adjustment draft form inside `InvoiceDetailView.vue` as a focused operator action near existing lifecycle controls.
- **Why:** the operator is already reviewing the approved invoice there, and the adjustment API returns a new replacement document that becomes the new navigation target afterward.
- **Alternative considered:** a separate `InvoiceAdjustView.vue` route.
- **Why not:** it would introduce another page with duplicated invoice loading, line rendering, and lifecycle messaging for a one-step operation.

### Decision: Mirror backend eligibility in the UI, but leave the backend as the final authority

- **Chosen:** only enable adjustment when the document is `approved` and has no recorded payments, matching the current backend restriction, while still surfacing backend errors unchanged.
- **Why:** this avoids offering an obviously invalid action while preserving the backend as the source of truth for lifecycle enforcement.
- **Alternative considered:** always show adjustment and rely entirely on backend rejection.
- **Why not:** that creates noisy operator failures in a financial workflow with clear local eligibility cues.

### Decision: Treat adjusted documents as first-class visible states in list/detail views

- **Chosen:** add `adjusted` status handling and render the `adjusted_from_id` relationship in invoice review surfaces.
- **Why:** the backend marks the original document as `adjusted` and creates a new draft replacement; operators need to understand where the replacement came from and why the original is no longer actionable.
- **Alternative considered:** rely on raw backend values and omit the relationship field.
- **Why not:** that would make adjusted documents appear inconsistent or disappear from filtered review flows.

## Risks / Trade-offs

- **[Adjustment UI duplicates line-edit state already implicit in the backend]** → keep the frontend payload close to `AdjustInvoiceRequest` and limit editable fields to replacement amounts keyed by existing charge line IDs.
- **[List and detail views drift on `adjusted` semantics]** → centralize status handling patterns and add tests for both surfaces.
- **[Operators lose track of the replacement document after success]** → redirect immediately to the returned draft document detail page and expose adjusted-from metadata there.
- **[Eligibility logic changes on the backend later]** → keep frontend gating minimal (`approved` + no payments) and continue surfacing backend validation errors verbatim.

## Migration Plan

1. Add adjusted-status and adjustment-entry visibility to invoice list/detail surfaces.
2. Implement the detail-page adjustment form and submit flow using `adjustInvoice()`.
3. Redirect successful adjustments to the returned replacement draft document.
4. Add/update Vue tests for list/detail adjustment behavior and adjusted status handling.
5. Run focused frontend verification for the touched surfaces.

## Open Questions

- Should the original approved invoice remain directly visible after adjustment success through a backlink from the replacement draft, or is `adjusted_from_id` display on the replacement document sufficient for first release?
- Should the list view expose a direct adjust action button, or is a view-first workflow from the list into detail enough once the status and relationship rendering are improved?
