# Design: Add Workflow Reminder Trigger Endpoint

## Endpoint

```
POST /api/workflow/reminders/run
```

One route, one handler method, zero service changes.

## Route Registration

The route registers on the existing `workflowGroup` in `backend/internal/http/router.go`, after the current reminder history line:

```go
workflowGroup.POST("/reminders/run",
    middleware.RequirePermission("workflow.admin", "approve", authService),
    workflowHandler.RunReminders,
)
```

Permission gate is `workflow.admin` with action `approve`, matching approve/reject/resubmit. No new permission codes needed.

## Request Shape

Optional JSON body. When omitted or partially provided, the handler fills in defaults before calling the service.

```go
type runRemindersRequest struct {
    ReminderType     *string `json:"reminder_type"`
    MinPendingAgeSec *int    `json:"min_pending_age_sec"`
    WindowTruncSec   *int    `json:"window_truncation_sec"`
}
```

All fields are pointers so the handler can distinguish "caller sent zero" from "caller omitted the field."

### Defaulting

The handler translates the request into a `workflow.ReminderConfig`, then relies on the service's existing `normalizeReminderConfig` for final defaults. The handler's translation rules:

| Field | If nil (omitted) | If provided |
|---|---|---|
| `ReminderType` | `""` (service normalizes to `"standard"`) | Use caller value |
| `MinPendingAgeSec` | `0` (service treats as no minimum) | Convert to `time.Duration` via `time.Duration(v) * time.Second` |
| `WindowTruncSec` | `0` (service normalizes to `24h`) | Convert to `time.Duration` via `time.Duration(v) * time.Second` |

Negative values for the duration fields are passed through; `normalizeReminderConfig` clamps `MinPendingAge` to zero and resets non-positive `WindowTruncation` to 24 hours.

The handler also supplies `time.Now().UTC()` as the `now` argument to `Service.RunReminders`. The caller does not control the timestamp.

### Validation

The handler rejects the request with `400 Bad Request` if:

- The body is malformed JSON (Gin's `ShouldBindJSON` fails).
- Any duration field is provided but negative. (The service would handle it, but the handler catches it early with a clear message.)

No other validation is required. The service already handles edge cases like empty reminder type and zero truncation.

## Handler Method

```go
func (h *WorkflowHandler) RunReminders(c *gin.Context) { ... }
```

Flow:

1. Bind optional JSON body. If binding fails, return `400`.
2. Build `workflow.ReminderConfig` from request, applying nil defaults.
3. Validate that provided duration seconds are non-negative. If not, return `400`.
4. Call `h.service.RunReminders(c.Request.Context(), time.Now().UTC(), config)`.
5. On error, return `500` with `{"message": "failed to run workflow reminders"}`.
6. On success, return `200` with `{"reminders": records}`.

The handler does not need `middleware.CurrentSessionUser`. The endpoint has no actor-dependent behavior; it runs the same reminder sweep regardless of who triggers it. The permission middleware already validated the caller's identity and rights.

## Response Shape

```json
{
  "reminders": [
    {
      "id": 42,
      "workflow_instance_id": 7,
      "reminder_type": "standard",
      "reminder_key": "reminder:7:standard:2026-04-10T00:00:00Z",
      "reminder_window_start": "2026-04-10T00:00:00Z",
      "outcome": "emitted",
      "reason_code": null,
      "created_at": "2026-04-10T14:32:01Z"
    }
  ]
}
```

This is the existing `ReminderAuditRecord` shape, already serialized with JSON tags in `model.go`. An empty sweep (no pending instances) returns `{"reminders": []}`.

## Error Responses

| Status | Condition |
|---|---|
| `400 Bad Request` | Malformed JSON body or negative duration field |
| `401 Unauthorized` | Missing or invalid auth token (handled by auth middleware) |
| `403 Forbidden` | Authenticated user lacks `workflow.admin` approve permission |
| `500 Internal Server Error` | Service call fails (database error, transaction failure) |

## Why No Service-Layer Changes

`Service.RunReminders` already does everything needed:

- Scans pending instances via `FindPendingInstances`.
- Checks pending age against `MinPendingAge`.
- Applies idempotent audit keys scoped to `(instance, type, window)`.
- Writes `ReminderAuditRecord` per instance with emitted/skipped outcomes.
- Returns the full record set.

The service's `normalizeReminderConfig` handles default values. The repository layer supports all required queries. The model types already carry JSON tags for serialization.

The only gap is that nothing in the HTTP layer calls `RunReminders`. This change adds that single call site.

## Out of Scope

The following are explicitly excluded and belong in separate changes:

- **Scheduler or cron.** No timer-based or recurring invocation. This endpoint is for manual, on-demand triggers only.
- **Reminder business logic changes.** Age thresholds, idempotency key format, outcome codes, and reason codes stay as-is.
- **Frontend UI.** No admin button or workflow panel changes.
- **Notification delivery.** Email, in-app messages, or any downstream action after an "emitted" outcome.

## Files Changed

| File | Change |
|---|---|
| `backend/internal/http/handlers/workflow.go` | Add `runRemindersRequest` struct and `RunReminders` handler method |
| `backend/internal/http/router.go` | Add one line: `workflowGroup.POST("/reminders/run", ...)` |

No changes to `workflow/service.go`, `workflow/model.go`, `workflow/repository.go`, or any other package.

## Test Coverage

- **Unit tests** on the handler: validation of malformed body, negative durations, and the success path using a mocked service.
- **Integration tests** hitting the route end-to-end: verify `200` with audit records on a real database, verify `400` on bad input, verify `403` without permission.
