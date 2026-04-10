# Proposal: Add Workflow Reminder Trigger Endpoint

## Why

The workflow service already has a complete reminder engine. `Service.RunReminders` scans pending instances, checks age thresholds, applies idempotent audit keys, and writes per-instance reminder records with emitted/skipped outcomes and reason codes. The repository layer supports `FindPendingInstances`, `FindReminderAuditByKey`, `InsertReminderAudit`, and `ListReminderHistory`. The HTTP handler layer exposes `ReminderHistory` as a read-only GET endpoint at `/api/workflow/instances/:id/reminders`, protected by `workflow.admin` `view` permission.

What's missing is any operator-accessible way to *trigger* a reminder run. `RunReminders` is fully functional in the service but has no HTTP entry point. Right now the only callers are integration tests. Without a trigger, the reminder system sits idle even though all the business logic is in place.

This change closes that gap with the smallest possible slice: one new POST endpoint that lets a workflow admin manually kick off a reminder sweep.

## What Changes

Add a single `POST /api/workflow/reminders/run` route that calls the existing `Service.RunReminders` method. The endpoint accepts an optional JSON body for reminder configuration (type, minimum pending age, window truncation) with sensible defaults. It returns the list of `ReminderAuditRecord` values produced by the run, matching the existing shape already used by tests and the history endpoint.

Concrete additions:

- New handler method on `WorkflowHandler` (`RunReminders`) in `backend/internal/http/handlers/workflow.go`
- New route registration in `backend/internal/http/router.go` under the existing `workflowGroup`, gated by `workflow.admin` `approve` permission (same gate used for approve/reject/resubmit)
- Unit test for request validation and error paths
- Integration test that exercises the full route end to end

Nothing changes in the service, repository, or model layers. Those are already complete.

## Capabilities Modified

- **workflow-approvals**: the workflow admin action surface gains a reminder trigger. Existing approve, reject, and resubmit actions are unaffected. The reminder history read endpoint is unaffected.

## Scope Boundaries

**In scope for this change:**

- The manual POST trigger endpoint described above
- Request validation, permission gating, and error handling
- Tests proving the endpoint works

**Explicitly out of scope:**

- Automatic scheduling, cron, or any timer-based invocation of `RunReminders`. That work belongs in a separate change if needed later.
- Changes to reminder business logic (age thresholds, idempotency keys, audit outcomes). Those are already implemented and tested.
- Frontend UI for triggering reminders. This change provides the API; a future change can wire it into the admin interface.
- Notification delivery (email, in-app, etc.). The reminder engine marks instances as "emitted" in the audit trail. What happens after emission is a separate concern.

## Impact

- **Risk**: low. The change is a thin HTTP wrapper over an existing, well-tested service method. No new business logic, no schema changes, no data migrations.
- **Backward compatibility**: fully compatible. All existing routes and handlers remain unchanged. The new endpoint is additive.
- **Permissions**: reuses the existing `workflow.admin` function code. No new permissions or roles required.
