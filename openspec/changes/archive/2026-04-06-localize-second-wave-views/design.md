## Context

The first UI localization change established the frontend i18n foundation, including `vue-i18n`, locale persistence, runtime switching, Element Plus locale integration, and a first wave of localized shell/common surfaces plus representative routed views. The remaining gap is no longer architectural; it is coverage. A defined set of deferred routed views still contains hard-coded English copy, which breaks the expectation that the application defaults to Simplified Chinese while still allowing runtime switching to English.

This second-wave change therefore needs to expand localization coverage without redesigning the locale system that already exists. The main technical challenge is consistency: the deferred pages span different domains (billing, lease, tax export, print/reporting, and admin consoles), but they should all reuse the same message structure, status-label conventions, formatting helpers, and verification approach introduced by the first-wave change.

## Goals / Non-Goals

**Goals:**
- Localize the deferred second-wave routed views that were explicitly excluded from the first-wave change.
- Reuse the existing locale infrastructure, message loading, locale persistence, and runtime switch behavior with no user-visible regression in the first-wave screens.
- Move hard-coded English strings in the deferred pages to locale-managed message keys for both `zh-CN` and `en-US`.
- Keep shared labels, status text, action text, filter labels, page-section copy, and feedback messages consistent with the first-wave message structure.
- Add verification for the newly localized second-wave pages so the default Simplified Chinese experience and runtime English switching remain test-backed.

**Non-Goals:**
- No redesign of the current i18n bootstrap, locale store, or Element Plus locale integration.
- No backend API, persistence, or auth/session changes.
- No attempt to localize business payload values that originate from backend data rather than frontend-authored labels.
- No requirement that every remaining future screen be localized in this same change if it is outside the explicitly listed deferred set.

## Decisions

### 1. Reuse the first-wave i18n foundation as-is

**Decision:** The second-wave change will build on the existing `vue-i18n` setup, locale store, localStorage persistence, and `LocaleSwitcher` behavior without introducing a new localization pattern.

**Why:** The first-wave implementation already proved the architecture and test strategy. Reworking it would create unnecessary risk and would blur the actual goal of this change, which is coverage expansion.

**Alternatives considered:**
- Reorganize the entire locale system before localizing more pages: rejected because it would turn a coverage change into another foundation rewrite.

### 2. Treat second-wave localization as a page-domain expansion, not a new capability

**Decision:** This change will modify the existing `ui-localization` capability rather than creating a new capability.

**Why:** The behavior contract does not change fundamentally; the app already supports default Chinese and runtime switching. This change extends which views honor that contract.

**Alternatives considered:**
- Introduce a second capability just for admin/reporting localization: rejected because that would artificially split one user-facing language behavior across multiple specs.

### 3. Localize deferred pages in domain batches

**Decision:** Implementation should proceed in small page-domain batches rather than a single broad sweep. The expected grouping is:
- billing and invoice views
- lease detail/create views
- reporting, visual analysis, tax export, and print-preview views
- admin console views (`MasterData`, `Sales`, `BaseInfo`, `Structure`, `RentableArea`, `Workbench`)

**Why:** The deferred views span multiple business domains and interaction patterns. Grouping by domain keeps changes reviewable and verification focused.

**Alternatives considered:**
- Translate every deferred page in one undifferentiated commit: rejected because it increases regression risk and weakens test accountability.

### 4. Preserve message taxonomy consistency with the first wave

**Decision:** The new page copy should extend the existing locale catalog structure rather than introducing ad hoc keys. Shared concerns like common actions, statuses, placeholders, and empty states should continue to live under shared namespaces, while page-specific copy should use page/domain namespaces.

**Why:** The first-wave implementation already established conventions for shell/common/view-level text. Reusing those conventions prevents fragmentation and makes tests more predictable.

**Alternatives considered:**
- Add all second-wave strings as one flat block: rejected because it would make the catalogs harder to maintain.

### 5. Keep verification page-targeted and regression-aware

**Decision:** Verification should extend the existing strategy: keep shared unit coverage for locale resolution and label translation, and add or update Playwright coverage only for second-wave screens whose visible language behavior changes.

**Why:** The biggest risk is not i18n infrastructure failure anymore; it is silently leaving English strings behind or breaking localized UI interactions in specific pages.

**Alternatives considered:**
- Rely on manual checks for second-wave pages: rejected because the first-wave change already established test-backed language behavior.

## Risks / Trade-offs

- **[Risk] The second-wave pages use more domain-specific tables/forms and may hide English strings in conditional states** → **Mitigation:** localize by page-domain batch and verify both steady-state and feedback/error states.
- **[Risk] Existing E2E tests may break because they assert English text or placeholders** → **Mitigation:** update tests toward stable selectors first, then assert language-specific text only where the change intentionally affects visible copy.
- **[Risk] Message catalogs become large and repetitive** → **Mitigation:** continue extracting reusable common labels/actions/statuses into shared namespaces instead of duplicating them per page.
- **[Risk] Deferring too many pages again weakens the “default Chinese” claim** → **Mitigation:** explicitly name the second-wave target set in tasks and treat any further deferrals as a conscious follow-up decision, not accidental omission.

## Migration Plan

1. Inventory the deferred pages and group them by domain batch.
2. Extend locale resources for the second-wave view copy while reusing existing shared namespaces where appropriate.
3. Localize the selected second-wave pages batch by batch, keeping current interaction behavior intact.
4. Update or add Playwright coverage for the newly localized pages and any shared selectors affected by visible copy changes.
5. Run commit-scoped unit, integration, and e2e verification evidence once the second-wave batch is complete.
6. If rollback is needed, revert the second-wave frontend copy changes only; the underlying locale foundation remains intact.

## Open Questions

- Should the second-wave implementation try to finish the entire deferred page list in one change, or should it explicitly stop after the highest-priority business and admin pages if the list proves too large?
- Are report labels such as report-option names in `GeneralizeReportsView.vue` expected to be fully localized in this wave, or is it sufficient to localize the surrounding UI shell while keeping report IDs/codes stable?
