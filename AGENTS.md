# AGENTS.md

## Project Identity

This workspace is a **replacement migration project**, not a maintenance project for the existing ASP.NET system.

- **Legacy reference only**: `legacy_code/`, `legacy_docs/`
- **Target architecture**: Vue 3 frontend + Go modular monolith backend + MySQL 8
- **Deployment target**: local FE/BE + existing Docker MySQL 8 for development; Docker Compose for production

If repository scanning conflicts with these instructions, **prefer this file and the OpenSpec change artifacts over language/file-frequency heuristics**.

## Source of Truth Order

When deciding intent, scope, or architecture, use this order:

1. Current canonical specs in `openspec/specs/`
2. Archived migration baseline in `openspec/changes/archive/2026-04-04-legacy-system-migration/`
3. `AGENTS.md`
4. `legacy_code/` and `legacy_docs/` as legacy behavior reference only

The legacy ASP.NET WebForms + SQL Server codebase is **not** the intended implementation target.

## Migration Target

### Target Stack
- Frontend: Vue 3 + Vite + Vue Router + Pinia + Element Plus + Axios
- Backend: Go modular monolith using Gin + Gorm + Viper + Zap + validator
- Database: MySQL 8
- Migrations: golang-migrate
- Testing: go test + Testcontainers-Go, Vitest, Playwright
- Document output: HTML templates + headless Chromium/Playwright PDF generation
- Excel: excelize

### Architecture Rules
- Use a **modular monolith**, not microservices.
- Frontend and backend are separated.
- Production topology is Docker Compose with nginx + frontend + backend + mysql.
- Config must be externalized and environment-aware.

## Scope Boundaries

### First Release Includes
- All non-membership business capability
- Priority operational chain: Lease → Bill / Invoice
- Approval workflow
- Tax rule/export
- Excel import/export
- Print/document output
- Generalize reports bounded by the current OpenSpec inventory

### First Release Excludes
- Membership / `Associator` — explicit product decision: do not migrate the membership system, member cards, points/bonus, gifts/redeem, member activities, or membership reports
- Email delivery
- Device/client tax integration
- Historical transaction migration
- Workflow timeout/escalation automation
- Page-for-page WebForms cloning

### Legacy Capability Coverage Notes
- Lease scope includes standard leases, union/joint-operation contracts, ad-board contracts, area/ground contracts, amendments, termination, overtime billing, charge generation, formulas, and lease approval flows.
- Bill / Invoice scope includes invoices, charge confirmation, adjustments, discounts, cancellation, deposits/surplus, interest, bank-card payment records, other/union charges, receivable tracking, and accounting/tax/export outputs where covered by active specs.
- Supporting-domain scope includes BaseInfo/admin data, Shop, Sell/POS data ingestion, RentableArea, customer/brand/prospect/budget administration, payment/receivable operations, and frozen Generalize report outputs.
- Generalize reporting remains bounded to the frozen `R01-R19` first-release report inventory. Legacy reports outside that inventory, including extra VisualAnalysis/POS/reporting pages, are not included unless OpenSpec is updated.

## Data and Cutover Rules

- The new system is a **fresh-start cutover**.
- Do **not** plan or assume migration of old business records.
- Old pending drafts, approvals, and open operational records are **not migrated**.
- Base/master data is reinitialized for the new system.

## Legacy-Code Handling Rules

When reading `legacy_code/`:

- Treat it as a **behavior/source reference**, not as a template to port mechanically.
- Extract business rules, report logic, workflow semantics, print/export behavior, and data relationships.
- Do **not** infer that ASP.NET, C#, SQL Server, Crystal Reports, or Office Interop are target technologies.

## Reporting Rules

- Generalize reporting scope is frozen in:
  - `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`
  - `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`
- Treat report IDs `R01-R19` as the authoritative first-release report set.
- Do not add extra reporting scope unless OpenSpec is updated.

## OpenSpec Alignment

Primary change:

- Current canonical specs: `openspec/specs/**`
- Archived migration proposal: `openspec/changes/archive/2026-04-04-legacy-system-migration/proposal.md`
- Archived migration design: `openspec/changes/archive/2026-04-04-legacy-system-migration/design.md`
- Archived migration tasks: `openspec/changes/archive/2026-04-04-legacy-system-migration/tasks.md`

If codebase evidence and OpenSpec disagree, assume:

- old code explains **legacy behavior**
- OpenSpec defines the **new intended system**

## Implementation Guidance for Agents

- Prefer business-capability slices over page-by-page migration.
- Treat workflow, tax export, print output, and Excel I/O as first-class workstreams.
- Keep verification executable and test-backed.
- For Generalize, implement/report against the acceptance matrix, not ad hoc report interpretations.
- Ask before widening scope beyond the frozen first-release boundary.

## Quick Path Reference

- Legacy solution: `legacy_code/MI_net.sln`
- Current specs: `openspec/specs/`
- Archived target migration change: `openspec/changes/archive/2026-04-04-legacy-system-migration/`
- Report inventory: `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`
- Report acceptance matrix: `openspec/changes/archive/2026-04-04-legacy-system-migration/report-acceptance-matrix.md`
