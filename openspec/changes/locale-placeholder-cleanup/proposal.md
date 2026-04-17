# Proposal: locale-placeholder-cleanup

## Problem

Seed data and locale files contain `https://example.com` placeholder URLs for floor plan images and a "No stub records" user-facing string that was left from scaffolding. These placeholder values should not ship in the first release.

## Scope

- Replace `https://example.com` placeholder URLs with empty strings in seed data and locale files (floor plan images are optional; an empty string correctly indicates "no image").
- Replace the scaffolding-era text `"No stub records match the current filters."` with the proper `"No records match the current filters."`.
- **No backend API changes** — only seed SQL, i18n message files, and one e2e test fixture.

## Why empty strings

Floor plan image URLs are optional. The UI already handles the absence of an image. Using an empty string is consistent with "not provided" semantics and avoids broken-link impressions.

## Affected files

| File | Change |
|------|--------|
| `frontend/src/i18n/messages/en-US.ts` | Remove placeholder URL + fix stub text |
| `frontend/src/i18n/messages/zh-CN.ts` | Remove placeholder URL |
| `backend/internal/platform/database/bootstrap/commercial_seed.go` | Remove placeholder URL in floor seed SQL |
| `frontend/e2e/task16-r19-visual.spec.ts` | Remove placeholder URL in test fixture |

## Out of scope

- Schema changes (the column remains nullable string).
- Removing the floor plan field from any form or API response.
- Any backend API contract change.
