# Spec: platform-foundation (locale-placeholder-cleanup delta)

## ADDED Requirements

### Requirement: All placeholder URLs and scaffolding text SHALL be removed before first release
Seed data and i18n locale files SHALL NOT contain `https://example.com` placeholder URLs. User-facing locale strings SHALL NOT contain scaffolding-era text such as the word "stub". Floor plan image URLs SHALL use empty strings to indicate "not provided" since the field is optional and the UI already handles absence with a fallback.

#### Scenario: No placeholder URLs in locale files
- **WHEN** a search is performed across `frontend/src/i18n/messages/` for `example.com`
- **THEN** no matches SHALL be found

#### Scenario: No placeholder URLs in seed data
- **WHEN** `commercial_seed.go` is examined for `example.com`
- **THEN** no matches SHALL be found

#### Scenario: No placeholder URLs in e2e fixtures
- **WHEN** `frontend/e2e/task16-r19-visual.spec.ts` is examined for `example.com`
- **THEN** no matches SHALL be found

#### Scenario: No scaffolding-era stub text in locale files
- **WHEN** `en-US.ts` is searched for "No stub records"
- **THEN** no matches SHALL be found

#### Scenario: Backend compiles after seed data change
- **WHEN** `go build ./...` is run from the backend directory
- **THEN** it SHALL succeed

#### Scenario: Frontend typechecks after locale string change
- **WHEN** `vue-tsc --noEmit` is run from the frontend directory
- **THEN** it SHALL succeed
