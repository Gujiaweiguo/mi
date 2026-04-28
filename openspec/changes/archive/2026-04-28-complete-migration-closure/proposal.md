## Why

The non-membership migration is structurally complete, but the final release must close operator-facing gaps that can otherwise make the system feel unfinished at go-live. The clearest immediate gap is the existing Workbench view not being reachable as an authenticated navigation entry for pending operational work.

## What Changes

- Expose the Workbench as a first-class authenticated route and navigation item.
- Use the Workbench as the operator landing area for migration closure visibility, including approval/work queues already represented by the frontend shell.
- Keep membership / `Associator` out of scope; this change only closes first-release non-membership migration usability and validation gaps.
- Capture follow-up closure checks for lease subtype behavior, billing edge cases, and print/PDF output verification without widening first-release scope.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `workbench-dashboard`: add an authenticated Workbench route/navigation entry so operators can reach the pending-work surface as part of the first-release shell.

## Impact

- Affects Vue router configuration, navigation permission metadata, i18n navigation labels, and frontend tests/E2E coverage around authenticated navigation.
- Does not change backend APIs, database schema, production topology, or the explicit membership exclusion.
