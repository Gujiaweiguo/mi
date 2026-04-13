## Context

The first-release migration work is largely closed, but the remaining gaps are unevenly distributed across supporting domains rather than the core Lease → Billing → Invoice chain. Current evidence shows that schema and reporting assumptions already reserve space for budget/prospect behavior, while the operator-facing management surface is still missing. At the same time, customer and brand management exist, but not yet at the same lifecycle depth or operational usability level as the rest of the first-release system.

This makes the change both narrow and important. It does not reopen broad first-release scope questions; instead, it closes a small set of in-scope supporting-domain gaps that currently sit in an ambiguous state between “implemented enough to be referenced” and “fully delivered for operators.” The work touches backend modules, frontend admin flows, and acceptance coverage, but should remain bounded to the first-release supporting-domain contract.

## Goals / Non-Goals

**Goals:**
- Close the missing budget/prospect administration surface that is already implied by schema/reporting readiness and first-release supporting-domain scope.
- Complete customer and brand management so operators can maintain those records through a full supported lifecycle rather than create-only or list-limited administration.
- Ensure supporting-domain closure is reflected in explicit specs and verification rather than inferred from migrations, partial UI surfaces, or report dependencies.
- Keep the change tightly scoped to supporting-domain completion, with only narrow billing/invoicing clarifications where dependent master data assumptions need to be explicit.

**Non-Goals:**
- No new business families beyond first-release supporting domains.
- No expansion into membership, second-release modules, or historical data migration.
- No broad redesign of the billing or invoice lifecycle.
- No general frontend refactor beyond what is required to expose and validate the missing supporting-domain workflows.
- No architecture rewrite of persistence patterns or repository conventions.

## Decisions

### 1. Treat budget/prospect as an in-scope supporting-domain closure item, not an optional follow-up

**Decision:** This change will close budget/prospect as a first-release supporting-domain management surface rather than treating the existing schema/reporting presence as sufficient.

**Why:** A reserved schema plus downstream reporting assumptions imply operational ownership of the data. Leaving the management surface absent would make the domain effectively depend on direct database edits or future undefined work, which violates the first-release replacement intent.

**Alternatives considered:**
- Declare budget/prospect out of scope retroactively. Rejected because that would require reopening the release contract instead of closing an already implied capability.

### 2. Upgrade master data from partial administration to full operator lifecycle

**Decision:** Customer and brand management will be treated as full-lifecycle supporting master data, including update, retirement/deletion rules as appropriate, and list-scale usability such as pagination/filtering where operator workflows require it.

**Why:** Supporting master data is only operationally trustworthy when users can maintain it beyond initial creation. Partial CRUD creates a long-tail support problem and pushes corrections outside the supported application path.

**Alternatives considered:**
- Leave create/list support in place and defer full lifecycle later. Rejected because the rest of the first-release system already assumes these records are living operational data rather than static seed entries.

### 3. Keep billing-and-invoicing changes declarative and dependency-focused

**Decision:** Any billing/invoicing spec updates in this change will be limited to clarifying dependency expectations on maintained supporting master data, not to changing the core financial lifecycle itself.

**Why:** The goal is to close supporting-domain ambiguity, not to reopen a stable business slice. Narrow dependency clarifications are enough to keep acceptance coherent where billing flows rely on maintained upstream records.

**Alternatives considered:**
- Modify billing/invoicing behavior directly in this change. Rejected because it would expand scope and blur the closure target.

### 4. Prefer explicit supported workflows over silent schema-backed capabilities

**Decision:** A domain is considered closed only when the repository provides explicit backend APIs, frontend management surfaces where required, and automated verification for the operator flow. Schema presence or report consumption alone is not sufficient.

**Why:** This aligns with the project’s broader principle that acceptance should be demonstrated through supported workflows and evidence, not inferred from partial implementation artifacts.

**Alternatives considered:**
- Accept schema presence plus report output as evidence of domain delivery. Rejected because it leaves operators without a supported maintenance path.

## Risks / Trade-offs

- **[Risk] Budget/prospect business rules may be less mature than other domains** → **Mitigation:** keep the implementation bounded to the minimum first-release administration and acceptance surface required by existing schema/reporting assumptions.
- **[Risk] Full-lifecycle master-data closure introduces deletion or retirement semantics that could affect downstream documents** → **Mitigation:** define safe lifecycle rules explicitly and prefer non-destructive record handling where downstream references exist.
- **[Risk] The change expands into generic admin UX cleanup** → **Mitigation:** limit frontend work to the surfaces needed for budget/prospect and master-data closure, not broad admin redesign.
- **[Risk] Billing acceptance wording could accidentally widen financial-slice scope** → **Mitigation:** keep billing spec changes narrowly dependency-focused and avoid lifecycle rewrites.

## Migration Plan

1. Audit the current supporting-domain implementation and identify the exact budget/prospect gaps plus the missing customer/brand lifecycle and usability operations.
2. Define the minimal supported backend and frontend workflows required to close those gaps without widening first-release scope.
3. Update supporting-domain specs to make budget/prospect administration and full-lifecycle master-data management explicit.
4. Add narrow billing/invoicing dependency clarifications only where upstream supporting master data must be maintained through supported workflows.
5. Implement and verify the backend, frontend, and test changes needed to satisfy the closure requirements.
6. Re-run supporting-domain acceptance so the change closes ambiguity rather than creating another partial slice.

Rollback is straightforward because this change is additive and scope-tight. If a specific closure path proves incorrect, the repository can revert that path without disturbing the already-stable core financial and workflow slices.

## Open Questions

- What exact operator surface is required for budget/prospect in first release: list/create/update only, or an additional approval/report-preparation workflow?
- Should customer/brand lifecycle closure prefer soft-delete/disable semantics over hard deletion wherever downstream references may already exist?
- Is pagination required for all master-data views in this slice, or only for the domains that are expected to grow beyond single-page administration?
