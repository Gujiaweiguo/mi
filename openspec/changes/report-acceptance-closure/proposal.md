## Why

The first-release `Generalize` report scope was frozen as `R01-R19`, but the migration change closed Task 16 at an implementation level rather than at a report-by-report acceptance level. We need a dedicated closure change now so each frozen report is verified against the acceptance matrix, any remaining gaps are explicitly classified, and the release contract for reporting is no longer implicit.

## What Changes

- Inventory the delivered state of `R01-R19` against the frozen report inventory and acceptance matrix.
- Define a report-acceptance closure contract for query behavior, filter coverage, export form, cross-report reconciliation, and documented exceptions.
- Add report-family verification tasks and evidence expectations so acceptance can be demonstrated instead of inferred.
- Tighten the supporting-domain reporting spec so first-release `Generalize` delivery means "implemented and matrix-verified," not merely present in the UI.

## Capabilities

### New Capabilities

### Modified Capabilities

- `supporting-domain-management`: Clarify how the frozen `R01-R19` report inventory is accepted, how gaps are classified, and what verification is required before the reporting slice can be considered closed.

## Impact

- Affected specs: `openspec/specs/supporting-domain-management/spec.md`
- Affected acceptance sources: `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md` and `report-acceptance-matrix.md`
- Affected implementation areas: reporting queries, report exports, visual report output, and report-focused verification tests/evidence
- Affected release process: report closure becomes a distinct gate with explicit per-report verification and documented exception handling
