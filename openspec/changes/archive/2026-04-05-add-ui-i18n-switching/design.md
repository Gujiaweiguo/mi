## Context

The current frontend mounts Vue, Pinia, Router, and Element Plus directly in `frontend/src/main.ts` with no locale infrastructure or translation plugin. Shared shell copy in `frontend/src/App.vue`, navigation labels in `frontend/src/auth/permissions.ts`, and view-level strings such as `frontend/src/views/LoginView.vue` are hard-coded in English, so the delivered UI does not match the expected operator-facing default of Simplified Chinese.

This change is cross-cutting because language behavior affects the shared application shell, permission-driven navigation labels, common component copy, and representative operational/admin screens. It also affects frontend test strategy: default locale selection and runtime switching must become explicit, verifiable behavior rather than incidental English text snapshots.

## Goals / Non-Goals

**Goals:**
- Introduce a frontend i18n foundation that supports Simplified Chinese (`zh-CN`) and English (`en-US`).
- Make Simplified Chinese the default UI language for the application shell and user-facing frontend copy.
- Allow runtime switching between Chinese and English without changing backend behavior or APIs.
- Persist the selected language in browser-local state so refreshes do not reset the operator's choice.
- Localize shared shell/navigation/common component text first, then the highest-traffic user-facing views as part of the same change scope.
- Ensure both app-authored copy and Element Plus locale text follow the active locale.

**Non-Goals:**
- No backend-side translation, locale negotiation, or API contract changes.
- No per-user server-side language preference storage in this change.
- No translation of business data values coming from the backend (for example codes, seeded names, or report payload contents) unless they are currently app-authored UI labels.
- No attempt to localize every future screen generically without explicitly wiring it into the frontend message catalog.

## Decisions

### 1. Use `vue-i18n` as the frontend localization layer

**Decision:** Add `vue-i18n` to the frontend and mount it in `frontend/src/main.ts` alongside Pinia, Router, and Element Plus.

**Why:** The application already uses standard Vue 3 composition patterns, so `vue-i18n` fits naturally into the existing app setup and allows both component text and shared configuration-driven labels to move to message keys.

**Alternatives considered:**
- **Hand-rolled translation map in a Pinia store**: smaller dependency surface, but quickly becomes brittle for nested messages, interpolation, and testability.
- **Keep English strings in components and only patch selected pages**: cheaper initially, but it would not create a reusable foundation and would leave the app in a mixed-language state.

### 2. Default to `zh-CN`, but persist an explicit client-side choice

**Decision:** The app will default to `zh-CN` on first load, then use a browser-local persisted preference on subsequent loads.

**Why:** The requested operator experience is “默认简体中文”, but switching languages should not be lost on refresh. Browser-local persistence keeps the behavior deterministic without introducing backend profile changes.

**Alternatives considered:**
- **Use browser language detection first**: conflicts with the stated requirement that Simplified Chinese should be the default.
- **Store locale in backend profile/session**: useful later, but it expands scope into backend persistence and auth/session behavior that this change does not need.

### 3. Centralize locale state in frontend application state instead of scattering per-view logic

**Decision:** Add locale state to the shared frontend app layer and expose a small, reusable switch API that any shell-level control can invoke.

**Why:** The shell in `frontend/src/App.vue` already owns global layout and session controls, making it the correct place for a language switch entry point. Centralized locale state also keeps navigation labels and global alerts consistent.

**Alternatives considered:**
- **Per-component locale toggles**: would fragment behavior and make the active language hard to reason about.
- **Route-specific locale setup**: does not fit a shared shell application.

### 4. Localize configuration-driven labels, not only template literals

**Decision:** Move navigation labels and other shared configuration text out of hard-coded arrays such as `frontend/src/auth/permissions.ts` and into locale-managed message keys.

**Why:** A large part of the visible English UI is not in templates but in shared configuration objects. If those labels stay as plain strings, the shell will remain partially English even after component templates are localized.

**Alternatives considered:**
- **Only translate component templates**: incomplete; it would miss menus, route-adjacent labels, and common metadata.

### 5. Integrate Element Plus locale with the same active app locale

**Decision:** The active app locale will also drive Element Plus locale configuration so built-in widget text stays aligned with application copy.

**Why:** The UI uses Element Plus broadly across forms, dialogs, tables, tags, validation, and date inputs. If the framework locale remains English while app copy becomes Chinese, the user experience will still look broken.

**Alternatives considered:**
- **Translate only app-authored text**: leaves framework-generated copy inconsistent.

### 6. Deliver localization in an intentionally scoped first wave

**Decision:** The first implementation wave for this change will explicitly cover:
- shared shell (`App.vue`)
- login/auth-facing screens
- navigation labels
- common platform components
- representative operational/admin views already relied on for first-release workflows

**Why:** The codebase has many user-facing pages, and the goal of this change is to create a reliable bilingual foundation plus an operator-visible language switch, not to silently promise complete translation parity everywhere without task-level accounting.

**Alternatives considered:**
- **Translate every page in one undifferentiated pass**: risks scope blur and weak verification.
- **Translate only the shell**: too narrow to satisfy the user-visible requirement for meaningful Chinese default behavior.

### 7. Verify locale behavior with explicit unit and end-to-end checks

**Decision:** Treat locale selection and language switching as testable behavior. Unit coverage should verify default locale resolution and locale persistence; end-to-end coverage should verify visible language changes on key screens.

**Why:** This repo already treats frontend behavior as test-backed. Locale regressions are easy to miss visually, so the design should require explicit coverage rather than manual confidence.

**Alternatives considered:**
- **Manual verification only**: too fragile for a cross-cutting UX change.

## Risks / Trade-offs

- **[Risk] Partial localization leaves the app in a mixed-language state** → **Mitigation:** Define the first-wave surface explicitly and make remaining screens visible follow-up work in tasks.
- **[Risk] Navigation/config labels are missed because they are not in templates** → **Mitigation:** Treat shared configuration files as first-class localization surfaces in tasks and review.
- **[Risk] Element Plus built-in text diverges from app locale** → **Mitigation:** Wire framework locale from the same central locale state used by `vue-i18n`.
- **[Risk] Translation keys become inconsistent across screens** → **Mitigation:** Use a structured message namespace split by shell/common/view domains instead of ad hoc flat keys.
- **[Risk] Browser-local persistence does not travel across devices or users** → **Mitigation:** Accept this as a deliberate trade-off for the first change; server-side preference storage remains out of scope.

## Migration Plan

1. Add the locale infrastructure and dependency wiring in the frontend foundation.
2. Introduce locale resources for `zh-CN` and `en-US` plus a central locale resolver/persistence path.
3. Add the shell-level language switch and make Element Plus follow the active locale.
4. Migrate shared shell, navigation, login, and common component text to message keys.
5. Migrate the first-wave representative views and add unit/e2e verification for default language and switching behavior.
6. Roll back, if needed, by reverting the frontend-only locale changes; no backend schema or API rollback is required.

## Open Questions

- Should the first wave include every currently implemented operational/admin screen, or should a smaller named subset be treated as the required acceptance surface for this change?
- Should the language switch be visible only after login in the shared shell, or also on the public login page before authentication?
