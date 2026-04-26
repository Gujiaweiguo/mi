## Context

The legacy system is a single-server ASP.NET WebForms + SQL Server application with business behavior distributed across server-rendered pages, database-centric persistence helpers, workflow tables, Crystal Reports-era printing, and Office/Excel exports. The target state is a frontend-backend separated replacement built on Vue 3, Go, and MySQL 8, with no historical transaction migration, fresh base-data initialization, and first-release coverage for all modules except membership.

The change is cross-cutting: it introduces a new application stack, new runtime topology, a new database engine at go-live, a new authorization model, and new output pipelines for workflow, tax, print, and Excel behavior. It also requires a formal cutover and rollback model because the transition is a one-time release event.

The cutover model is now explicitly a fresh start: no legacy data of any kind will be migrated into the new system, including pending drafts, approvals, or open operational records. The first release also intentionally excludes workflow timeout/escalation behavior, and the `Generalize` reporting scope is bounded to the reports named in `阳光商业MI.net系统设计.doc`.

## Goals / Non-Goals

**Goals:**
- Deliver a modular monolith using Vue 3 + Go + MySQL 8 that supports all first-release non-membership business operations.
- Preserve required business outcomes for Lease, Bill, Invoice, workflow, tax export, Excel import/export, and print/document generation.
- Make development local-first, test environment Compose-based, and production Compose-based with mounted runtime data.
- Establish TDD-first automation and artifact verification from the start.
- Support one-time cutover with explicit rehearsal, release gates, and rollback criteria.

**Non-Goals:**
- Historical transaction data migration.
- Membership (`Associator`) implementation in the first release.
- Device/client tax integration.
- Email delivery in the first release.
- Screen-by-screen WebForms parity.
- Microservices, Kubernetes, or a generic BPM platform.

## Decisions

### 1. Architecture: modular monolith
- **Decision**: Use one Go deployable, one Vue SPA, and one MySQL 8 database.
- **Why**: First-release risk is business-rule recovery, not horizontal scaling. A modular monolith keeps deployment, transactions, and debugging simpler.
- **Alternatives considered**:
  - Microservices: rejected due to operational and consistency overhead.
  - Server-rendered Go UI: rejected because the requested target is frontend-backend separation with Vue.

### 2. Frontend stack
- **Decision**: Use Vue 3 + Vite + Vue Router + Pinia + Element Plus + Axios.
- **Why**: This combination is stable, productive for admin-style systems, and supports rapid delivery of complex form/table workflows.
- **Alternatives considered**:
  - Ant Design Vue / Naive UI: viable, but not selected because Element Plus provides a familiar, broad admin component set.

### 3. Backend stack
- **Decision**: Use Gin + Gorm + Viper + Zap + go-playground/validator.
- **Why**: The system needs pragmatic delivery, clear request handling, schema-backed persistence, environment-aware config, and structured logging.
- **Alternatives considered**:
  - Echo/Fiber: rejected to avoid introducing stylistic variance without clear project benefit.
  - sqlx-first persistence: deferred because first release benefits from faster CRUD/model bootstrapping through Gorm.

### 4. Database and migration model
- **Decision**: Go-live moves to MySQL 8 with versioned `golang-migrate` migrations and deterministic seed/bootstrap data.
- **Why**: MySQL 8 is a hard release requirement, and no historical transaction migration means the focus is on schema fidelity, seed data, and opening-state readiness.
- **Alternatives considered**:
  - Temporary SQL Server compatibility mode: rejected because it violates the go-live requirement.

### 5. Workflow implementation
- **Decision**: Build a configurable approval subsystem with templates, transitions, assignee rules, audit history, and outbox-backed side effects.
- **Why**: Approval capability is mandatory, but exact legacy process parity is not. A bounded configuration model is safer than a generic BPM engine.
- **Alternatives considered**:
  - Generic BPM/workflow engine: rejected for first release because it adds platform complexity and slows domain recovery.
  - Hardcoded per-document approvals: rejected because workflow must remain configurable.

### 5.1 Workflow timeout and escalation boundary
- **Decision**: Timeout/escalation behavior is not included in the first release.
- **Why**: The current release goal is full approval capability for submit/approve/reject/resubmit and auditability, but not legacy parity for every automation edge.
- **Alternatives considered**:
  - Include timeout/escalation now: rejected to keep first-release workflow bounded and delivery-focused.

### 6. Output strategy
- **Decision**: Replace Crystal/Office-bound outputs with server-generated HTML templates, PDF generation through headless Chromium/Playwright, and `excelize`-based Excel handling.
- **Why**: First-release printing/export is mandatory and must be testable in CI/test automation.
- **Alternatives considered**:
  - Keep Crystal Reports/Office Interop: rejected because they are desktop/runtime-bound and hard to automate reliably.

### 7. Authorization strategy
- **Decision**: Redesign auth/permissions around fresh-init users, roles, org units, shops/buildings, and scoped actions for view/edit/approve/print/export.
- **Why**: The user explicitly allows redesign, and the first release benefits from a clearer permission model than legacy flag sprawl.
- **Alternatives considered**:
  - One-to-one legacy permission cloning: rejected because it preserves complexity without business value.

### 8. Verification strategy
- **Decision**: Use TDD with `go test`, Testcontainers-Go, Vitest, Playwright, and artifact comparison for PDF/CSV/XLSX/TXT outputs.
- **Why**: Manual-only verification is too risky for workflow, tax, and output-heavy migration work.
- **Alternatives considered**:
  - Manual QA first, automation later: rejected because the user explicitly wants TDD plus test foundations.

### 8.1 Staged test gates
- **Decision**: Separate verification into `CI Ready` and `Archive Ready` gates.
- **Why**: Push/PR validation and archive validation serve different purposes and should not be conflated.
- **Rule**:
  - `CI Ready` requires passing unit and integration evidence for the current commit.
  - `Archive Ready` requires passing unit, integration, and e2e evidence for the current commit.
- **Alternatives considered**:
  - Treat CI pass as sufficient for archive: rejected because archive represents a stricter completion state.

### 8.2 Evidence-based gate enforcement
- **Decision**: Test gates are satisfied only by machine-readable evidence files under `artifacts/verification/<commit-sha>/`.
- **Why**: Current-commit evidence is the only reliable way to prevent stale or claimed test success from being used during archive.
- **Alternatives considered**:
  - Trust local command history or verbal confirmation: rejected because it is not reproducible or automatable.

### 9. Cutover data policy
- **Decision**: The first release starts with fully reinitialized data and migrates no legacy records of any kind.
- **Why**: The user explicitly chose not to migrate old-system data, which removes data-conversion complexity and makes freeze-time record handling binary.
- **Alternatives considered**:
  - Migrate open or pending records only: rejected because it violates the chosen fresh-start cutover policy.

### 10. Reporting boundary
- **Decision**: `Generalize` scope in first release is limited to the reports explicitly mentioned in `阳光商业MI.net系统设计.doc`.
- **Why**: This bounds reporting scope to documented business needs and prevents first-release reporting sprawl.
- **Alternatives considered**:
  - Rebuild all legacy reporting: rejected because the exact report inventory is intentionally bounded.
  - Accept an implicit or evolving report scope: rejected because first release needs a frozen, enumerable output set.

### 10.1 First-release Generalize inventory source
- **Decision**: The operational report list for first release is frozen in `openspec/changes/archive/2026-04-04-legacy-system-migration/report-inventory.md`, derived from the extractable report SQL/content in `阳光商业MI.net系统设计.doc`.
- **Why**: The binary `.doc` does not preserve all display titles cleanly in this environment, so the most reliable bounded inventory is the normalized list reconstructed from the document's report queries and structures.
- **Alternatives considered**:
  - Leave report names informal: rejected because implementation and acceptance need a stable inventory.

## Risks / Trade-offs

- **[Hidden legacy business rules]** → Mitigation: require a dedicated legacy behavior inventory and decision log before implementation slices begin.
- **[Scope explosion from “all modules except membership”]** → Mitigation: freeze first-release output catalog and capability inventory before feature implementation.
- **[Workflow semantics underestimated]** → Mitigation: isolate workflow as its own capability, keep it configurable, and document timeout/escalation as an explicit non-goal for first release.
- **[Output mismatch in tax/print/Excel]** → Mitigation: treat tax export, print outputs, and Excel I/O as separate capabilities with golden-sample verification.
- **[Cutover ambiguity]** → Mitigation: require rehearsal, release gates, and explicit rollback criteria before go-live.
- **[Fresh-start data gaps]** → Mitigation: create deterministic seed/bootstrap packs for org, auth, code tables, numbering, and supporting master data.
- **[False green archive readiness]** → Mitigation: require commit-scoped evidence for unit/integration/e2e before archive.

## Migration Plan

1. Freeze proposal-driven capability set and first-release boundaries.
2. Extract legacy rules and output inventory.
3. Scaffold the new repo/runtime/test foundations.
4. Build MySQL schema, seed/bootstrap, auth, workflow, and Lease/Bill/Invoice core.
5. Build tax, document output, and Excel I/O.
6. Build remaining non-membership supporting domains.
7. Finalize Compose environments and operational scripts.
8. Rehearse cutover, evaluate release gates, and either proceed or rollback.

## Open Questions

- None at this planning stage; freeze-time records are not migrated, workflow timeout/escalation is excluded from first release, and reporting scope is frozen in `report-inventory.md` based on `阳光商业MI.net系统设计.doc`.
