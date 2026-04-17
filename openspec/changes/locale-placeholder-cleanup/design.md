# Design: locale-placeholder-cleanup

## Decision 1 — Empty string for placeholder image URLs

**Choice:** Replace `https://example.com/…` values with `''` (empty string).

**Rationale:** Floor plan images are optional. The frontend already renders a fallback when no URL is present. An empty string communicates "not provided" without introducing a broken link.

**Alternatives considered:**
- `null` / `undefined` — rejected because TypeScript locale objects use string literals.
- A real placeholder image — rejected; adds unnecessary asset management.

## Decision 2 — "No stub records" text replacement

**Choice:** Replace `"No stub records match the current filters."` with `"No records match the current filters."`.

**Rationale:** The word "stub" was a scaffolding artifact. Users should never see it.

## Impact analysis

- **No API changes.** The floor seed SQL already inserts into `floor_plan_image_url`; we only change the value.
- **No schema changes.** The column remains `VARCHAR … NULL`.
- **No UI logic changes.** Existing fallback rendering for empty floor plan URLs continues to work.
- **E2e fixture alignment.** The test fixture for R19 visual report is updated to match the new seed data convention.
