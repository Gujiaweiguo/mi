## Context

The WorkflowAdminView (Vue 3 + Element Plus) currently renders a table of workflow instances with approve/reject action buttons. The backend already exposes all required APIs — `resubmitWorkflow`, `getWorkflowAuditHistory`, `getReminderHistory`, and `runReminders` — and the frontend API client (`frontend/src/api/workflow.ts`) already wraps these endpoints. The `AuditEntry` and `IdempotencyRequest` types are already defined. No backend or API-layer changes are needed.

The existing approve/reject pattern uses `instanceActionId` and `instanceAction` refs to track per-row loading states. The new resubmit action should follow this same pattern.

## Goals / Non-Goals

**Goals:**
- Enable resubmission of rejected workflow instances from the admin view
- Provide audit history visibility per workflow instance via a detail drawer
- Provide reminder history visibility per workflow instance
- Enable manual reminder evaluation trigger from the admin view
- Add i18n keys for all new UI text in both en-US and zh-CN
- Add unit tests covering all new interactions

**Non-Goals:**
- Backend API changes (all endpoints already exist)
- New routes or new view files (everything stays in WorkflowAdminView)
- New API client functions
- Workflow template configuration UI
- Timeout/escalation automation

## Decisions

### 1. Resubmit button placement — inline in actions column

**Decision**: Add the resubmit button as a third action button in the instances table actions column, visible only when `row.status === 'rejected'`.

**Rationale**: Follows the existing approve/reject pattern. Keeping it inline avoids extra clicks compared to a dropdown or context menu. The button uses the same `instanceActionId`/`instanceAction` loading-state pattern.

**Alternatives considered**:
- Dropdown menu for actions: more scalable but adds an extra click for the common case
- Separate "Resubmit" column: wastes horizontal space for non-rejected rows

### 2. Instance detail drawer with el-drawer

**Decision**: Use `el-drawer` anchored to the right side, triggered by clicking an instance row (or a "Details" button in the actions column).

**Rationale**: Drawers are the established pattern in Element Plus admin interfaces for viewing detail without navigating away. The drawer shows:
- Instance metadata header (document type, document ID, status, created/updated dates)
- Audit history timeline using `el-timeline`
- Reminder history list

**Alternatives considered**:
- Full-page detail route: breaks admin workflow context, requires new route
- `el-dialog`: obscures more of the table; drawer allows simultaneous reference

### 3. Audit history displayed as el-timeline

**Decision**: Render `AuditEntry[]` from `getWorkflowAuditHistory(id)` as an `el-timeline` where each node shows action, actor, from/to status transition, comment, and timestamp.

**Rationale**: Timeline component is semantically correct for ordered audit entries. Each entry maps naturally to a timeline node.

### 4. Manual reminder trigger in table header area

**Decision**: Place a "Run Reminders" button in the instances table header area (above or beside existing filters). Clicking triggers `runReminders()` with an `ElMessageBox.confirm` confirmation dialog, then shows success/error feedback via `ElMessage`.

**Rationale**: Reminder evaluation is a global operation (not per-instance), so it belongs in the header/toolbar area rather than in a row action. Confirmation dialog prevents accidental trigger.

### 5. Reminder history per instance in drawer

**Decision**: Within the instance detail drawer, below the audit timeline, add a collapsible section showing reminder history from `getReminderHistory(instanceId)`.

**Rationale**: Reminder history is instance-scoped, so it naturally belongs in the instance detail drawer. Keeping it collapsible avoids clutter when not needed.

## Risks / Trade-offs

- **[Row click opens drawer] → Mitigation**: Add a "Details" button as well, so touch/click targets are explicit. Row click is a convenience, not the only path.
- **[Resubmit idempotency key generation] → Mitigation**: Use `crypto.randomUUID()` to generate the key, same approach as any future idempotent action. The browser crypto API is available in all target browsers.
- **[Drawer content loading] → Mitigation**: Show loading skeleton/spinner while fetching audit history and reminder history. Use `Promise.all` to load both in parallel.
- **[Reminder run is global and potentially slow] → Mitigation**: Disable the trigger button while the request is in flight. Show a loading indicator. The confirmation dialog sets expectations.
