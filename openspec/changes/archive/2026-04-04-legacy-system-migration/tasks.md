## 1. Discovery and release contract freeze

- [x] 1.1 Task 1 — Freeze legacy behavior inventory and decision log; done when `docs/legacy-inventory.md` lists all first-release capabilities by business outcome, `docs/decision-log.md` marks each legacy behavior as preserve/redesign/drop, membership exclusions are explicit, and inventory validation passes.
- [x] 1.2 Task 2 — Define cutover policy, output inventory, and bootstrap contract; done when `docs/cutover-runbook.md` and `docs/output-catalog.md` define base-data bootstrap, numbering initialization, mandatory prints/exports/imports/tax outputs, and the fresh-start cutover policy explicitly states that no legacy records are migrated.

## 2. Platform, testing, and data foundations

- [x] 2.1 Task 3 — Scaffold the target repo, runtime topology, and configuration system; done when Vue/Go/Compose structure exists, config is environment-aware, and test/production Compose definitions render successfully.
- [x] 2.2 Task 4 — Establish the TDD harness, staged test gates, and artifact verification foundation; done when `go test ./...`, frontend unit tests, Playwright, and output comparison tooling run from a clean checkout, `artifacts/verification/<commit-sha>/unit.json|integration.json|e2e.json` is the documented evidence convention, push/PR is defined to require unit+integration evidence, and archive is defined to require unit+integration+e2e evidence.
- [x] 2.3 Task 5 — Build MySQL 8 schema, migrations, and bootstrap seed packs; done when migrations apply to an empty MySQL 8 instance, verification SQL passes, and deterministic bootstrap data exists for auth/org/shop/building/workflow/code tables.
- [x] 2.4 Task 6 — Implement auth, organization, and redesigned permission foundation; done when fresh-init users/roles/departments/shops/buildings exist and scoped permissions for view/edit/approve/print/export are enforced by automated tests.

## 3. Core business workflow and financial engine

- [x] 3.1 Task 7 — Implement workflow core with audit and idempotent side-effect handling; done when templates/transitions/assignees/audit history work, outbox-backed side effects are idempotent, and first-release workflow explicitly excludes timeout/escalation automation.
- [x] 3.2 Task 8 — Implement Lease domain model and contract lifecycle services; done when create/submit/approve/amend/terminate/query flows work and downstream billing-effective state is validated in tests.
- [x] 3.3 Task 9 — Implement billing rule model and charge generation; done when approved Lease state generates deterministic charge lines, amendment/termination effects are handled, and duplicate generation protection passes tests.
- [x] 3.4 Task 10 — Implement Invoice/Bill lifecycle and financial transaction chain; done when generated charges produce numbered bill/invoice documents with approval, cancellation/adjustment handling, audit history, and passing end-to-end integration tests.

## 4. Mandatory outputs and operator-facing delivery

- [x] 4.1 Task 11 — Implement the tax rule and export subsystem; done when tax rules are configurable, mandatory export files match expected shape/content, and invalid tax setup fails fast without partial trusted output.
- [x] 4.2 Task 12 — Implement document, print, and operational output generation; done when mandatory Lease/Bill/Invoice outputs are generated as print-ready HTML/PDF artifacts and golden-sample regression checks pass.
- [x] 4.3 Task 13 — Implement the Excel import/export subsystem; done when mandatory templates can be downloaded, valid imports commit deterministically, invalid imports produce row-level diagnostics, and exports match expected structure.
- [x] 4.4 Task 14 — Build the Vue application shell and shared frontend platform; done when authenticated routing, role-aware navigation, shared form/table patterns, and common API/error handling are covered by unit and E2E tests.
- [x] 4.5 Task 15 — Implement Lease/Bill/Invoice frontend workflows end to end; done when operators can complete the full Lease → approval → charge → invoice/bill → print/export/tax flow in Playwright using stable `data-testid` selectors.

## 5. Remaining domains and operational readiness

- [x] 5.1 Task 16 — Implement remaining non-membership domain slices; done when BaseInfo/admin, Shop, Sell, RentableArea, workflow administration, and only the `Generalize` reports `R01-R19` frozen in `report-inventory.md` are implemented according to `report-acceptance-matrix.md` or explicitly marked non-go-live in the decision log, while membership remains excluded.
- [x] 5.2 Task 17 — Deliver test/production Compose stacks and operational mounts; done when nginx/frontend/backend/mysql run healthily in test and production Compose configurations with mounted config, logs, generated documents/uploads, and MySQL data plus documented backup/restore scripts.
- [x] 5.3 Task 18 — Execute cutover rehearsal, rollback plan, and go-live checklist; done when a full rehearsal can bootstrap the environment, generate mandatory artifacts, validate fresh-start go-live without legacy data import, and produce a binary go/no-go result.
