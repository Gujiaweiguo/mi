## Context

The replacement system already exposes `/workbench` as an authenticated route, but `frontend/src/views/WorkbenchView.vue` still renders shell-style tags, local filtering, and static row props instead of loading real operator queues. On the backend, `backend/internal/dashboard/service.go` already aggregates six top-level dashboard metrics, which proves the system can collect lease, receivable, and workflow counts in one place, but there is no queue-oriented aggregate tailored to the Workbench.

This change needs a small vertical slice across backend and frontend: a single aggregate endpoint for Workbench data, a concrete queue payload for first-release operational focus areas, and tests that keep the experience stable for a solo developer without introducing new infrastructure or widening the release boundary.

## Goals / Non-Goals

**Goals:**

- Add a single authenticated backend endpoint that returns Workbench queue data in one response.
- Surface real queue-focused sections for pending approvals, receivables, overdue receivables, and active lease workload.
- Keep the Workbench useful even when one queue is empty by returning a stable response shape the frontend can render directly.
- Reuse existing lease, billing/receivable, and workflow tables instead of introducing new persistence models.
- Add focused backend and frontend automated coverage for populated and empty Workbench states.

**Non-Goals:**

- No membership / `Associator` work.
- No email delivery, notifications channel redesign, or reminder transport changes.
- No historical data migration or data backfill project.
- No replacement of the existing `/dashboard` summary API or default authenticated home.
- No new permissions model, queue assignment engine, or workflow timeout automation.

## Decisions

### Decision: Add a dedicated Workbench aggregate endpoint instead of stretching the dashboard summary response

- **Chosen:** introduce a Workbench-specific authenticated endpoint that returns queue-oriented data.
- **Why:** the existing dashboard summary is intentionally count-only and optimized for cards. Reusing it for queue rows would either overload its response contract or force the frontend to make several follow-up calls.
- **Alternative considered:** extend `GET /api/dashboard/summary` with queue lists.
- **Why not:** it would mix two different UI concerns in one payload and make the dashboard endpoint heavier than its current day-one contract.

### Decision: Return one stable response with multiple queue sections

- **Chosen:** return one payload containing queue sections for approvals, receivables, overdue receivables, and active leases, with each section carrying a count and a bounded preview list.
- **Why:** the Workbench should load with one request and render predictably for empty or partially sparse data without requiring per-section orchestration in the frontend.
- **Alternative considered:** one endpoint per queue section.
- **Why not:** that would increase latency, complicate loading state, and create more moving pieces for a solo-maintained surface.

### Decision: Use bounded preview rows with downstream navigation links, not a full task engine

- **Chosen:** each queue section shows a small preview set plus a target route for deeper work.
- **Why:** this keeps the Workbench focused on triage and navigation while reusing existing detail/list pages for full workflows.
- **Alternative considered:** make the Workbench a complete operational editor with inline actions.
- **Why not:** that would expand scope into queue assignment, mutation handling, and new authorization edge cases.

### Decision: Reuse existing domain sources and permissions behavior

- **Chosen:** aggregate from existing lease, receivable, invoice, and workflow tables and keep the Workbench available to authenticated users as a general operational surface.
- **Why:** the prior Workbench route change intentionally avoided a new permission code. This change should preserve that shell-level access model while relying on downstream pages to enforce deeper permissions.
- **Alternative considered:** add a dedicated Workbench permission.
- **Why not:** it would create an artificial gate around summary data and add admin/setup overhead without clear first-release value.

## Risks / Trade-offs

- **[Queue payload grows too large]** → keep each queue section to bounded preview rows and direct the user to existing list/detail pages for full data exploration.
- **[Cross-module aggregation becomes brittle]** → keep the first version to count + preview queries from existing tables only, and verify behavior with backend tests that cover empty and populated fixtures.
- **[Workbench exposes information too broadly]** → limit the response to operationally safe summaries and route metadata; do not embed privileged detail that bypasses downstream page permissions.
- **[Frontend becomes tightly coupled to one payload shape]** → use explicit queue section contracts and test empty/populated states so later additions remain additive.

## Migration Plan

1. Add backend Workbench aggregate service logic and the authenticated endpoint.
2. Add backend tests that validate queue counts, bounded previews, and empty-state payloads.
3. Update the frontend Workbench view and API client to load the aggregate once and render the queue sections.
4. Add frontend tests for loading, populated, and empty queue rendering.
5. Verify with existing test suites; no database migration or rollout sequencing is required beyond normal deployment.

## Open Questions

- Should the first version expose only preview rows, or also section-level links/filters encoded in the payload for the frontend to use directly?
- Should pending approvals remain grouped by business type inside one approvals section, or split into separate lease and invoice sections in the UI while still sharing one endpoint?
