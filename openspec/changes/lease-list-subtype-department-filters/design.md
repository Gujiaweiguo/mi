## Context

`LeaseListView` currently exposes only `lease_no` and `status` filters, while lease summaries already include both `subtype` and `department_id`. Operators working across standard, joint-operation, ad-board, and area/ground contracts need to isolate queues by subtype and department without opening each contract manually.

The current gap is small but cross-layer:
- frontend view state and filter controls do not expose subtype / department filters
- frontend lease list API params do not model the new filters
- backend lease list handler and repository do not accept or apply these optional constraints

## Goals / Non-Goals

**Goals:**
- let operators filter lease contracts by subtype from `LeaseListView`
- let operators filter lease contracts by department from `LeaseListView`
- keep the lease list contract additive so existing callers continue to work unchanged
- verify the filter wiring at handler, repository integration, frontend API, and frontend view levels

**Non-Goals:**
- query-param persistence or deep-link filter restoration
- changing lease detail, amendment, or downstream billing flows
- adding new lease status semantics or new subtype values
- changing pagination, sorting, or list response shape

## Decisions

### 1. Reuse the existing `/leases` list endpoint

**Decision:** extend the existing lease list filter contract with optional `subtype` and `department_id` instead of creating a new search endpoint.

**Why:** the lease list screen already depends on `/leases`, and the missing behavior is just two extra filter dimensions. A new endpoint would add needless maintenance surface for a simple additive query.

### 2. Keep backend filter handling optional and non-breaking

**Decision:** the new backend filter fields remain optional pointers so the existing queue behavior is unchanged when filters are absent.

**Why:** first-release lease operations already rely on the existing list flow. Additive WHERE clauses are the smallest safe change and avoid side effects for callers that only use `lease_no`, `status`, or `store_id`.

### 3. Use existing subtype labels and department data on the frontend

**Decision:** subtype options are derived from the existing lease subtype translation keys, and department options reuse the current `listDepartments()` preload already performed by `LeaseListView`.

**Why:** this keeps the UI aligned with the rest of the lease module and avoids adding another supporting-data request path.

## Risks / Trade-offs

- **Department preload may fail while lease rows still load** → keep department filter options best-effort; list errors stay non-blocking as they are today
- **Additional backend filter dimensions could miss tests and drift later** → add both handler parse coverage and repository/service integration coverage
- **Subtype filter values come from controlled frontend options but not a new validation layer** → keep scope minimal and rely on the existing subtype enum model plus deterministic equality filtering

## Migration Plan

1. Add OpenSpec delta and task checklist
2. Extend backend lease list filter model, handler parsing, repository SQL, and integration coverage
3. Extend frontend lease API params, view filters, i18n, and unit tests
4. Run focused backend/frontend verification, then broader lint/build/test checks
