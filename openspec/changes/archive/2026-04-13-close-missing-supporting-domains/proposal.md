## Why

The archived first-release migration change closed the main business and delivery slices, but current implementation evidence still shows a small set of in-scope supporting-domain gaps. In particular, the backend schema already reserves space for budget/prospect behavior while the management surface is missing, and the master-data slice remains narrower than the operational workflows around it because customer and brand management are not yet fully closed.

We need a dedicated closure change now so the remaining first-release supporting domains are either fully delivered to the same standard as the rest of the system or explicitly removed from the release contract, instead of remaining in an ambiguous partially implemented state.

## What Changes

- Close the missing budget/prospect supporting-domain management surface required by the first-release supporting-domain scope.
- Complete the master-data management contract so customer and brand workflows support the full lifecycle expected by operators, including list-scale usability rather than create-only administration.
- Add or tighten acceptance coverage for these supporting-domain gaps so closure is demonstrated through tests and explicit spec language rather than inferred from schema presence.
- Keep scope bounded to already intended first-release supporting domains; do not expand into new business families or second-release features.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `supporting-domain-management`: clarify and complete first-release supporting-domain expectations for budget/prospect administration and full-lifecycle master-data management.
- `billing-and-invoicing`: may receive acceptance clarifications only where supporting master data is a prerequisite for stable operational use.

## Impact

- Affects supporting-domain specs and acceptance criteria under `openspec/specs/`.
- Affects backend supporting-domain implementation areas such as `internal/masterdata/` and any new budget/prospect module required to satisfy the frozen first-release scope.
- Affects frontend admin/workbench surfaces where operators manage supporting-domain records and navigate these workflows.
- Affects verification coverage for supporting-domain CRUD, pagination/usability, and report or workflow dependencies on those records.
