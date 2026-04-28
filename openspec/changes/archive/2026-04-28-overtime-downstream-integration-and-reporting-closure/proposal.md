## Why

The newly implemented overtime bill flow is now able to create approved overtime charges, but it is not yet fully closed through the downstream operator chain. Until overtime-generated charges are visible in invoicing, reporting, and any affected output surfaces, the business outcome remains only partially delivered and operators must bridge the gap manually.

## What Changes

- Extend the overtime flow from approved/generated overtime charges into downstream billing and invoicing behavior so operators can complete the financial chain without side-channel handling.
- Make overtime-generated financial output visible in the first-release reporting surfaces that rely on billing and receivable data, so overtime does not disappear from operational and finance review.
- Align any affected print or document output behavior with the new overtime-supported financial chain where current supported document types would otherwise omit relevant overtime data.
- Preserve first-release non-membership scope and avoid widening into unrelated legacy reporting pages or membership workflows.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `billing-and-invoicing`: extend deterministic charge/invoice behavior so approved overtime-generated charges participate in downstream billing, invoicing, receivable, and operator-facing financial review as part of the supported first-release chain.
- `lease-contract-management`: refine the newly added overtime requirement so the approved overtime flow is not only generated and auditable, but also operationally consumable downstream.
- `print-output`: update supported output behavior if any current lease/invoice-facing print surface must expose overtime-derived financial data to remain trustworthy.

## Impact

- Affects backend overtime, billing, invoice, receivable, and reporting queries, plus any related document-output loaders that depend on financial document composition.
- Affects frontend billing/invoice/reporting operator views where overtime-supported downstream visibility must be surfaced and verified.
- Requires new executable verification across unit, integration, and E2E paths for overtime-to-invoice/reporting closure, without changing membership scope, deployment topology, or historical-migration assumptions.
