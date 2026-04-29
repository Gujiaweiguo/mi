## 1. Resubmit Action

- [ ] 1.1 Add resubmit button in the instances table actions column, visible only when `row.status === 'rejected'`
- [ ] 1.2 Wire button to `resubmitWorkflow(id, { idempotency_key: crypto.randomUUID() })` with per-row loading state following the existing `instanceActionId`/`instanceAction` pattern
- [ ] 1.3 Add success/error feedback via `ElMessage` and refresh the instances list on success

## 2. Instance Detail Drawer

- [ ] 2.1 Add `el-drawer` component anchored right-side to WorkflowAdminView, triggered by row click or "Details" action button
- [ ] 2.2 Display instance metadata header in the drawer (document type, document ID, status, created date, updated date)
- [ ] 2.3 Fetch audit history via `getWorkflowAuditHistory(id)` on drawer open with loading skeleton
- [ ] 2.4 Render audit entries as `el-timeline` nodes showing action, actor, from/to status, step order, comment, and timestamp
- [ ] 2.5 Handle empty audit history with a "No audit history" placeholder

## 3. Reminder History

- [ ] 3.1 Add collapsible reminder history section in the detail drawer below the audit timeline
- [ ] 3.2 Fetch reminder history via `getReminderHistory(instanceId)` on drawer open (parallel with audit fetch via `Promise.all`)
- [ ] 3.3 Render reminder entries with outcome, reason code, and timestamps
- [ ] 3.4 Handle empty reminder history with a "No reminder history" placeholder

## 4. Manual Reminder Trigger

- [ ] 4.1 Add "Run Reminders" button in the instances table header area
- [ ] 4.2 Add `ElMessageBox.confirm` confirmation dialog on click
- [ ] 4.3 Wire confirmed action to `runReminders()` with button disabled during request and loading indicator
- [ ] 4.4 Display success feedback with result summary or error message on completion

## 5. Internationalization

- [ ] 5.1 Add all new i18n keys to `en-US` locale file (resubmit button, drawer labels, timeline labels, reminder section, trigger button, placeholders, confirmation dialog text, success/error messages)
- [ ] 5.2 Add matching keys to `zh-CN` locale file

## 6. Tests

- [ ] 6.1 Unit test: resubmit button visibility (visible for `rejected`, hidden for `pending`/`approved`)
- [ ] 6.2 Unit test: resubmit action calls `resubmitWorkflow` with correct idempotency key and refreshes list
- [ ] 6.3 Unit test: detail drawer opens and fetches audit history on row click
- [ ] 6.4 Unit test: audit timeline renders entries from API response
- [ ] 6.5 Unit test: reminder history section fetches and renders entries
- [ ] 6.6 Unit test: manual reminder trigger shows confirmation, calls `runReminders`, shows feedback
- [ ] 6.7 Unit test: reminder trigger button is disabled during request

## 7. Verification

- [ ] 7.1 Run typecheck (`vue-tsc --noEmit`) — no errors
- [ ] 7.2 Run build (`vite build`) — succeeds
- [ ] 7.3 Run unit tests — all pass
- [ ] 7.4 Record evidence: `artifacts/verification/<commit-sha>/unit.json`
