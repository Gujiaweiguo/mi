## Why

The non-membership migration still has business-rule depth gaps that are explicitly part of the first-release boundary in `AGENTS.md` but are not fully represented in the current canonical specs or implementation. These gaps affect lease subtype coverage, overtime billing, discounts, surplus/customer balance, late-payment interest, deposit application, and lease contract print output.

## What Changes

- Add explicit requirements for overtime billing tied to lease contracts, including formula configuration and approval/audit behavior.
- Add explicit requirements for invoice discount approval, surplus/customer balance tracking, late-payment interest calculation, and deposit application/refund handling.
- Add explicit requirements for lease contract print output so `lease_contract` is either fully rendered or no longer advertised as a supported document type.
- Clarify lease subtype coverage for standard, joint-operation, ad-board, and area/ground contracts without introducing membership scope.
- Keep historical transaction migration, membership / `Associator`, email delivery, device tax integration, and reports outside R01-R19 excluded.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `lease-contract-management`: clarify subtype-specific contract behavior and add overtime billing requirements.
- `billing-and-invoicing`: add discount, surplus/customer balance, late-payment interest, and deposit application/refund requirements.
- `print-output`: align lease contract print support with the supported document type list.

## Impact

- Affects backend lease, billing, invoice, workflow, print output, database migrations, bootstrap data, frontend views/API clients, and automated tests.
- Requires new executable verification for edge-case billing calculations and workflow/audit behavior.
- Does not affect membership, historical data migration, or deployment topology.
