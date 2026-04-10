# Tasks: Add Workflow Reminder Trigger Endpoint

## Implementation

- [x] **Add handler method and request struct** in `backend/internal/http/handlers/workflow.go`
  - Define `runRemindersRequest` with pointer fields: `ReminderType *string`, `MinPendingAgeSec *int`, `WindowTruncSec *int`
  - Implement `RunReminders(c *gin.Context)`: bind body, build `workflow.ReminderConfig` with nil defaults, validate non-negative durations, call `h.service.RunReminders(ctx, time.Now().UTC(), config)`, return `200` with `{"reminders": records}` or `400`/`500` on error

- [x] **Register route** in `backend/internal/http/router.go`
  - Add `workflowGroup.POST("/reminders/run", middleware.RequirePermission("workflow.admin", "approve", authService), workflowHandler.RunReminders)` after the existing reminder history route

## Tests

- [x] **Handler unit tests** in `backend/internal/http/handlers/workflow_test.go`
  - Test malformed JSON body returns `400`
  - Test negative `MinPendingAgeSec` returns `400`
  - Test negative `WindowTruncSec` returns `400`
  - Test success path: mocked service returns audit records, handler returns `200` with correct JSON shape
  - Test empty body: handler applies defaults and calls service

- [x] **Integration test** hitting the full route
  - Test `200` with audit records when pending instances exist on a real database
  - Test `400` on bad input
  - Test `403` when caller lacks `workflow.admin` approve permission

## Verification

- [x] **Gate check**: `go test ./...` passes; `go vet ./...` clean; no new diagnostics on changed files
