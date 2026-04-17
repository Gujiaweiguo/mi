# Proposal: locale-placeholder-cleanup

## Why

Seed data and locale files contain `https://example.com` placeholder URLs for floor plan images and a "No stub records" user-facing string left over from scaffolding. These placeholder values would appear in the production first release if not cleaned up, presenting broken links and unprofessional wording to end users.

## What Changes

- Replace all `https://example.com` placeholder URLs with empty strings in seed data, i18n locale files, and the affected e2e test fixture.
- Replace the scaffolding-era text `"No stub records match the current filters."` with the proper production wording `"No records match the current filters."`.
- No backend API changes, no schema changes, and no UI logic changes — only string literal values in seed SQL, i18n message files, and one test fixture.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: ensure all placeholder URLs and scaffolding-era text are removed from seed data, locale files, and test fixtures before first release.

## Impact

- `frontend/src/i18n/messages/en-US.ts`: remove placeholder URL and fix stub text.
- `frontend/src/i18n/messages/zh-CN.ts`: remove placeholder URL.
- `backend/internal/platform/database/bootstrap/commercial_seed.go`: remove placeholder URL in floor seed SQL.
- `frontend/e2e/task16-r19-visual.spec.ts`: remove placeholder URL in test fixture.
- Zero API contract changes. Zero schema changes. Zero logic changes.
