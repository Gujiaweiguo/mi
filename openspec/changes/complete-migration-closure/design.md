## Context

The replacement system already has a routed authenticated dashboard and a reusable `WorkbenchView.vue` shell, but the Workbench is not reachable through Vue Router or the main navigation. For final non-membership migration closure, operators need a stable entry point for pending work without changing backend APIs or expanding scope.

## Goals / Non-Goals

**Goals:**

- Register the Workbench as an authenticated frontend route.
- Add the Workbench to the standard navigation with localized labels.
- Preserve the existing dashboard as the default authenticated home.
- Add focused frontend verification for route/navigation availability.

**Non-Goals:**

- No membership / `Associator` migration.
- No new backend APIs, database migrations, or workflow engine changes.
- No change to the existing dashboard summary API or go-live evidence policy.

## Decisions

- Keep `/dashboard` as the root redirect target and expose Workbench at `/workbench`. This avoids surprising users who already rely on the dashboard summary as the default landing page while still making pending-work navigation explicit.
- Treat Workbench as a general authenticated surface without a new permission code. The view aggregates work across multiple queues, so gating it behind one business permission would hide useful shell-level access from otherwise valid users.
- Reuse existing frontend i18n and navigation filtering instead of adding a separate menu system. This keeps route visibility consistent with the rest of the Vue shell.

## Risks / Trade-offs

- Workbench data is currently shell/static-prop oriented rather than backed by a dedicated aggregate endpoint → keep this change limited to reachability and navigation, and leave deeper queue aggregation to a later business-rule enhancement if needed.
- A general authenticated Workbench may be visible to users with narrow permissions → the page must only expose summary/navigation content suitable for authenticated users and must not bypass permissions on downstream pages.
