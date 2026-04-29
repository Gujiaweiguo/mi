## Why

The WorkflowAdminView currently only supports approve/reject actions on pending workflow instances. Three critical workflow lifecycle capabilities are missing from the frontend: resubmission of rejected instances, visibility into the approval audit trail, and manual reminder management. All required backend APIs and frontend API client functions already exist — only frontend UI integration is needed.

## What Changes

- Add a **resubmit action button** for rejected workflow instances, wired to the existing `resubmitWorkflow()` API
- Add an **instance detail drawer/dialog** displaying full audit history timeline via the existing `getWorkflowAuditHistory()` API
- Add a **manual reminder trigger button** wired to the existing `runReminders()` API
- Add **reminder history display** per instance in the detail view via the existing `getReminderHistory()` API
- Add **i18n copy** (en-US, zh-CN) for all new UI elements
- Add **unit tests** for new interactions

## Capabilities

### New Capabilities

_(none — all features extend the existing workflow-approvals capability)_

### Modified Capabilities

- `workflow-approvals`: Adds requirements for resubmit action, audit history viewing, manual reminder trigger, and reminder history display from the admin view

## Impact

- **Frontend**: `WorkflowAdminView.vue` — new action button, detail drawer with audit timeline, reminder controls
- **Frontend i18n**: New keys in en-US and zh-CN locale files
- **Frontend tests**: New unit tests for resubmit, audit history, and reminder interactions
- **No backend changes**: All APIs and types are already implemented
- **No new routes or views**: Everything lives inside the existing WorkflowAdminView
