## Context

The migration workspace already supports subtype-rich lease contracts and an overtime billing flow that can create approved overtime charge lines with audit history. That work closes the upstream rule-capture problem, but it does not yet guarantee that overtime-derived charges participate in the same downstream operator and finance chain as standard lease-generated charges. The remaining gap is cross-cutting: overtime output must be consumable by billing and invoice workflows, visible in receivable-backed financial views, and reflected wherever supported first-release output surfaces derive their trusted financial data.

This change stays inside the replacement-system scope defined by current OpenSpec and the frozen first-release report inventory. It uses the existing Vue 3 frontend, Go modular monolith backend, and MySQL 8 model, and treats legacy code only as behavior reference if a downstream overtime expectation needs clarification.

## Goals / Non-Goals

**Goals:**

- Make approved overtime-generated charges flow through the supported downstream billing and invoicing path without manual side handling.
- Ensure overtime-derived financial state is visible in the operator-facing financial review surfaces that already depend on billing, invoice, and receivable data.
- Preserve auditability and replay safety so overtime integration does not introduce duplicate invoice, receivable, or reporting side effects.
- Clarify whether any currently supported print/document output must include overtime-derived financial data to remain trustworthy.

**Non-Goals:**

- No membership / `Associator` scope.
- No expansion beyond the frozen first-release Generalize report inventory or into unrelated legacy reporting pages.
- No historical transaction migration, topology change, or new external service dependency.
- No redesign of the existing overtime formula model or approval workflow beyond what is required to make downstream consumption deterministic.

## Decisions

- Treat approved overtime charges as standard downstream financial inputs rather than a separate financial document family. This keeps billing, invoicing, receivable, and aging behavior aligned with the current first-release financial chain, avoids parallel lifecycle logic, and reduces operator ambiguity about where overtime charges should be processed.
- Integrate overtime visibility at the charge/query composition layer instead of bolting it on only in frontend screens. Downstream operator views, invoice creation, receivable review, and bounded reporting all depend on trusted backend financial datasets, so the design should make overtime part of the authoritative source rather than a UI-only annotation.
- Keep overtime traceability explicit in downstream records through source metadata, approval linkage, or equivalent provenance fields. Operators and auditors need to distinguish overtime-derived lines from standard lease-derived lines without sacrificing their ability to participate in the same invoice and receivable flow.
- Bound reporting changes to first-release surfaces that already consume billing and receivable state. If a frozen report in the `R01-R19` inventory or an existing supported finance view omits overtime after integration, that surface should be updated; this change does not authorize new reports or ad hoc legacy clones.
- Only extend print/document output where supported document types would otherwise become misleading after overtime enters the downstream chain. If invoice-facing or lease-facing documents already rely on downstream financial composition, overtime must appear through the same trusted loader path instead of a document-specific special case.

## Risks / Trade-offs

- Overtime may currently live behind lease-local queries while downstream finance screens rely on invoice/receivable aggregates → mitigate by converging integration in shared backend charge and document composition paths rather than duplicating overtime joins across multiple handlers.
- Provenance-rich overtime integration can add query and test complexity across billing, invoice, receivable, and reporting modules → mitigate by reusing existing financial document abstractions and adding focused integration coverage around source attribution and duplicate safety.
- Some supported output or reporting surfaces may implicitly assume only standard lease-generated charges → mitigate by auditing bounded first-release consumers and updating only those that derive trusted financial results from the affected datasets.
- Overtime visibility requirements can expand quickly if interpreted as “all legacy reports” → mitigate by anchoring acceptance to current OpenSpec capabilities and the frozen first-release report inventory only.
