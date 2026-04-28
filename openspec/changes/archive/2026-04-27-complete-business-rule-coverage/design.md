## Context

The current system implements the main Lease → Billing/Invoice → AR chain, but the migration scope includes several legacy business outcomes that are not yet explicit in canonical specs or code. These rules are financially sensitive and should be delivered as test-backed slices rather than inferred from generic lease or invoice behavior.

## Goals / Non-Goals

**Goals:**

- Make the remaining non-membership lease and invoice business rules explicit and testable.
- Implement each rule through backend model/service/API changes with frontend operator surfaces where needed.
- Preserve auditability by integrating approval-sensitive adjustments with workflow history and immutable financial audit records.
- Keep generated charges, invoices, receivables, tax/export, and print output consistent after adjustments.

**Non-Goals:**

- No membership / `Associator` migration.
- No import of historical transactions from the legacy SQL Server system.
- No page-for-page WebForms cloning.
- No additional Generalize reports beyond R01-R19.

## Decisions

- Treat overtime billing, discounts, interest, surplus, and deposit application as first-class domain concepts instead of free-form invoice adjustments. This prevents financially significant events from disappearing into generic adjustment notes.
- Add workflow/audit integration for operator-entered financial changes such as overtime bills and discounts. These actions change receivables and therefore require traceable approval behavior.
- Clarify lease subtypes before adding subtype-only fields. Standard and joint-operation contracts may fit the current generic model, while ad-board and area/ground contracts likely require additional typed attributes.
- Implement lease contract print output by loading lease-domain data into a dedicated renderable document shape rather than forcing lease contracts through invoice document loading.

## Risks / Trade-offs

- Financial rules can conflict when several adjustments apply to one receivable → define deterministic ordering in tests: base charge, approved discount, deposit/surplus application, payment, interest.
- Legacy formulas may be broader than first-release operational needs → implement the minimum formula set required by current acceptance scenarios, then add explicit scenarios before expanding.
- Subtype modeling can overfit legacy pages → preserve business outcomes and required data, not old page structures.

## Migration Plan

Implement in independent slices: lease subtype clarification, overtime billing, invoice financial adjustments, lease contract print output, then full regression through unit/integration/e2e evidence. Each slice must include migrations, bootstrap updates, API tests, and frontend coverage where it adds an operator workflow.

## Open Questions

- Which ad-board attributes are mandatory at go-live: board identity, airtime, frequency, weekday schedule, or pricing formula?
- Which interest calculation convention is authoritative: calendar days overdue, grace days, monthly rate conversion, or configured daily rate?
- Can surplus balances be generated only from overpayment, or also from manual opening balance during fresh-start bootstrap?
