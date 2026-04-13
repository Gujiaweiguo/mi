# MI Migration Workspace

This repository is the **replacement migration workspace** for the legacy MI.NET system. The target system is a **frontend-backend separated** application built with **Vue 3 + Go + MySQL 8**.

> Important: `legacy_code/` and `legacy_docs/` are **reference-only**. They explain legacy behavior, but they are **not** the target implementation stack.

## What this repository is

- A migration workspace for rebuilding the system on a new architecture
- A planning and execution workspace centered on OpenSpec change artifacts
- A place to extract business rules from the old system and re-implement them in the new stack

## What this repository is not

- Not an ASP.NET WebForms maintenance project
- Not a SQL Server continuation project
- Not a page-by-page clone of the legacy UI
- Not a historical data migration project

## Source of truth

When files disagree, use this order:

1. `openspec/changes/legacy-system-migration/`
2. `AGENTS.md`
3. `legacy_code/` and `legacy_docs/` as legacy reference only

If the repository scan suggests “old .NET app”, treat that as **legacy input**, not the target architecture.

## Target architecture

- **Frontend**: Vue 3 SPA
- **Backend**: Go modular monolith
- **Database**: MySQL 8
- **Development**: local frontend + local backend + existing Docker MySQL 8
- **Production**: Docker Compose

For detailed rules and constraints, read `AGENTS.md`.

## First-release scope

Included:

- All non-membership business capability
- Priority chain: **Lease → Bill / Invoice**
- Approval workflow
- Tax rule/export
- Excel import/export
- Print/document output
- Generalize reports frozen in OpenSpec

Excluded:

- Membership / `Associator`
- Email delivery
- Device/client tax integration
- Historical transaction migration
- Workflow timeout/escalation automation
- WebForms page-by-page cloning

## OpenSpec change to follow

Primary change:

- `openspec/changes/legacy-system-migration/proposal.md` — why this migration exists
- `openspec/changes/legacy-system-migration/design.md` — architecture and major decisions
- `openspec/changes/legacy-system-migration/tasks.md` — ordered execution checklist
- `openspec/changes/legacy-system-migration/specs/` — capability requirements
- `openspec/changes/legacy-system-migration/report-inventory.md` — frozen Generalize report list (`R01-R19`)
- `openspec/changes/legacy-system-migration/report-acceptance-matrix.md` — report acceptance baseline

## How to start work

Recommended order for any developer or coding agent:

1. Read `README.md`
2. Read `AGENTS.md`
3. Read:
   - `openspec/changes/legacy-system-migration/proposal.md`
   - `openspec/changes/legacy-system-migration/design.md`
   - `openspec/changes/legacy-system-migration/tasks.md`
4. Find the relevant capability under `openspec/changes/legacy-system-migration/specs/`
5. Start from the OpenSpec acceptance criteria, not from legacy screens
6. Use `legacy_code/` only to recover business rules, workflow semantics, print/export behavior, and data relationships

## Working style

- Prefer **business-capability slices** over page migration
- Preserve **business outcomes**, not old implementation details
- Keep scope inside first-release boundaries
- Treat workflow, tax, print, Excel, and reports as first-class workstreams
- Use executable verification wherever possible

## Verification mindset

- Work test-first when possible
- Use OpenSpec requirements and task completion criteria as the acceptance contract
- Prefer automated verification for:
  - workflow behavior
  - print/document outputs
  - tax/export outputs
  - Excel import/export
  - Generalize reports (`R01-R19`)

## Repository landmarks

- `legacy_code/` — old ASP.NET WebForms solution, behavior reference only
- `legacy_docs/` — legacy schema/design inputs
- `openspec/` — change artifacts and specifications
- `AGENTS.md` — root migration instructions for agents and contributors

## Quick reminder

If you are unsure whether something belongs in first release, **check OpenSpec before implementing**.
