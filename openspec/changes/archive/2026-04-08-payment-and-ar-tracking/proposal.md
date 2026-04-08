## Why

The current first-release financial chain stops at approved bills and invoices: the system can generate charges and approve financial documents, but it does not yet create or reduce accounts-receivable open items through a supported payment workflow. This gap now blocks meaningful aging, receivable, and paid-voucher outputs because `ar_open_items` exists at the schema level but has no end-to-end operational lifecycle.

## What Changes

- Add a first-release payment and accounts-receivable tracking workflow that creates receivable open items from approved billing documents and reduces them when operators record payments.
- Extend billing and invoicing requirements so approved financial documents become receivable-bearing records with auditable balance changes instead of terminal output-only states.
- Add operator-facing payment entry and receivable tracking behavior needed to support aging reports, cumulative arrears analysis, and paid-voucher output.
- Define how receivable balances, due dates, and payment application affect downstream report/query surfaces that currently depend on `ar_open_items`.
- Keep scope bounded to non-membership first-release financial operations; no bank integration, external payment gateway, or automated reminder workflow is introduced.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `billing-and-invoicing`: extend document lifecycle requirements so approved bill/invoice records create receivable open items and support payment application, outstanding balance tracking, and auditable financial state changes.
- `supporting-domain-management`: extend first-release supporting operations so operators can manage payment/receivable activity required by aging, arrears, and paid-voucher outputs.

## Impact

- Affects backend financial lifecycle code around invoice approval, receivable persistence, and any new payment/application service boundaries.
- Affects reporting data integrity for aging-related outputs and any report queries that rely on `ar_open_items` balances.
- Affects frontend operator workflows for recording payments and reviewing receivable status.
- Requires new automated coverage for receivable creation, payment application, outstanding balance updates, and downstream report-facing data consistency.
