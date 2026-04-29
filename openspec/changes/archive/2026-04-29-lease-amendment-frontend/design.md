## Context

`openspec/specs/lease-contract-management/spec.md` already treats amendment as part of the first-release Lease lifecycle, and the runtime stack is mostly ready: `backend/internal/lease/service.go` supports amendment creation, `backend/internal/http/handlers/lease.go` exposes `POST /leases/{id}/amend`, and `frontend/src/api/lease.ts` already defines `amendLease()`. The remaining gap is operator reachability inside the Vue app: `frontend/src/views/LeaseDetailView.vue` exposes submit and terminate actions, but not amendment initiation, while `frontend/src/views/LeaseCreateView.vue` already contains the full subtype-aware form logic we need for amendment drafting.

This change is a focused frontend closure. It should reuse existing backend behavior, avoid a new form surface, and keep the amendment workflow aligned with the existing contract-detail → draft-edit → detail-review navigation model.

## Goals / Non-Goals

**Goals:**

- Add an operator-facing amendment action from lease detail for amendment-eligible contracts.
- Reuse `LeaseCreateView.vue` for amendment drafting so subtype-specific validation and form sections stay in one place.
- Prefill the amendment form from the source contract, while still allowing operators to change billing-relevant fields before submission.
- Submit the amendment through the existing `amendLease()` API and route the operator to the new amended contract after success.
- Add focused frontend tests that cover amendment entry, prefilled fields, and successful submission.

**Non-Goals:**

- No backend service, schema, or API contract changes.
- No invoice adjustment, workflow redesign, or new approval behavior.
- No separate amendment-only view or modal wizard.
- No historical amendment comparison UI beyond what current contract detail already shows.

## Decisions

### Decision: Reuse `LeaseCreateView.vue` in amendment mode instead of building a separate amendment screen

- **Chosen:** extend `LeaseCreateView.vue` so it can initialize from an existing lease contract and submit via `amendLease()` when entered in amendment mode.
- **Why:** the create view already contains the subtype-aware form structure, validation, and reference-data loading needed for amendment drafting. Reusing it avoids duplicating a large, high-risk form surface.
- **Alternative considered:** add a dedicated `LeaseAmendView.vue`.
- **Why not:** it would duplicate the same contract form logic and create a second place to maintain subtype-specific rules.

### Decision: Add a dedicated amendment route that reuses the create component

- **Chosen:** add a route such as `/lease/contracts/:id/amend` that points to `LeaseCreateView.vue` and carries the source lease id in the path.
- **Why:** a dedicated route makes amendment entry bookmarkable, explicit in tests, and easy for `LeaseDetailView.vue` to navigate to without overloading the meaning of the generic create route.
- **Alternative considered:** reuse `/lease/contracts/new` with query parameters.
- **Why not:** it obscures intent, makes route-state validation more brittle, and weakens test readability.

### Decision: Prefill from the source lease by normalizing the existing lease payload into the create-form shape

- **Chosen:** load the source lease through `getLease()`, transform the returned contract into the form model already used by `LeaseCreateView.vue`, and let the user edit that draft before submission.
- **Why:** this preserves one authoritative source of truth for lease data, keeps amendment mode aligned with the backend payload contract, and ensures subtype-specific collections (`joint_operation`, `ad_boards`, `area_grounds`, `units`, `terms`) are preserved.
- **Alternative considered:** preload only a subset of fields and ask the operator to re-enter the rest.
- **Why not:** it increases operator friction and makes amendment workflows error-prone for billing-effective fields.

### Decision: Keep eligibility and error handling frontend-light

- **Chosen:** show the amendment action only for leases in amendment-eligible states, but still rely on backend validation as the final authority and surface API errors through existing page-level error messaging.
- **Why:** this preserves responsive UI behavior while avoiding divergence from the backend lifecycle rules.
- **Alternative considered:** enforce all lifecycle eligibility exclusively in the frontend.
- **Why not:** it would duplicate state rules and still require backend validation for safety.

## Risks / Trade-offs

- **[Form reuse introduces mode-specific branching]** → keep amendment mode narrowly scoped to initialization text, submit handler selection, and success navigation while preserving a shared form model.
- **[Source lease normalization misses subtype fields]** → add tests that cover at least one populated amendment draft and keep the transformation close to the existing `CreateLeaseRequest` shape.
- **[Frontend eligibility diverges from backend lifecycle rules]** → treat frontend gating as a convenience only and continue surfacing backend rejection messages unchanged.
- **[New route confuses create-vs-amend semantics]** → make the page title, submit copy, and success navigation explicit when amendment mode is active.

## Migration Plan

1. Add the amendment route and wire `LeaseDetailView.vue` to navigate there for eligible contracts.
2. Extend `LeaseCreateView.vue` to detect amendment mode, load the source lease, and prefill the form.
3. Submit amendment drafts through `amendLease()` and redirect to the newly created amended contract detail page on success.
4. Add or update Vue tests for detail-view entry and amendment-form submission behavior.
5. Run focused frontend tests plus the relevant typecheck/unit verification commands.

## Open Questions

- Should amendment mode prefill `lease_no` exactly from the source contract and rely on the operator to change it, or should the UI apply a visible draft suffix/prompt before submission?
- Should the detail page surface the amended-from relationship as a link when the fetched contract already has `amended_from_id`?
