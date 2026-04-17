# Spec: platform-foundation (locale-placeholder-cleanup delta)

## Requirement

All `https://example.com` placeholder URLs in seed data and i18n locale files **MUST** be replaced with empty strings before first release.

All scaffolding-era text containing the word "stub" in user-facing locale strings **MUST** be replaced with appropriate production wording.

## Acceptance criteria

1. `grep -r 'example.com' frontend/src/i18n/messages/` returns no matches.
2. `grep 'example.com' backend/internal/platform/database/bootstrap/commercial_seed.go` returns no matches.
3. `grep -r 'example.com' frontend/e2e/task16-r19-visual.spec.ts` returns no matches.
4. `grep 'No stub records' frontend/src/i18n/messages/en-US.ts` returns no matches.
5. Backend compiles: `go build ./...` succeeds.
6. Frontend typechecks: `vue-tsc --noEmit` succeeds.
