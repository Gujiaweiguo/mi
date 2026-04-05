## Why

The current frontend is effectively English-only: shared shell copy, navigation labels, login text, and view-level user messages are hard-coded in English and the repo has no locale infrastructure. This makes the delivered system mismatch the expected operator experience, where Simplified Chinese should be the default UI language while still allowing English for mixed-language teams and rollout validation.

## What Changes

- Add frontend localization infrastructure for application-authored UI text, with Simplified Chinese as the default language.
- Add runtime Chinese/English language switching for shared shell surfaces and user-facing frontend screens.
- Replace hard-coded English copy in shared layout, login, navigation, and representative operational/admin views with locale-managed message keys.
- Align frontend test coverage with the new language behavior so default locale selection and language switching are verified instead of relying on manual checks.

## Capabilities

### New Capabilities
- `ui-localization`: Covers frontend locale resources, default Simplified Chinese behavior, and runtime switching between Simplified Chinese and English for user-facing UI copy.

### Modified Capabilities
- `platform-foundation`: The frontend foundation requirements change so the shared Vue application shell includes locale infrastructure and a default language behavior instead of assuming hard-coded English UI text.

## Impact

- Affected code: `frontend/src/main.ts`, `frontend/src/App.vue`, `frontend/src/auth/permissions.ts`, shared platform components, login view, and additional user-facing views that currently embed English copy.
- Affected dependencies: frontend will likely need a localization library and locale resources for both application copy and UI framework integration.
- Affected tests: frontend unit tests and end-to-end coverage will need assertions for default language behavior and runtime language switching.
- Affected UX: operators will see Simplified Chinese by default, with an explicit option to switch to English without changing business behavior or backend APIs.
