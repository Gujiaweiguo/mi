# Tasks: Add Workflow Reminder Schedule Runner

## 1. Configuration

- [x] 1.1 Add `workflow_reminder_scheduler` configuration model in `backend/internal/config/config.go` with fields for enable flag, interval, reminder type, min pending age, and window truncation.
- [x] 1.2 Update backend YAML config files (`development.yaml`, `test.yaml`, `production.yaml`) with scheduler defaults (disabled by default, safe interval defaults).
- [x] 1.3 Add/extend config tests to verify scheduler configuration loading and env overrides.

## 2. Scheduler runtime wiring

- [x] 2.1 Wire a periodic scheduler loop in `backend/internal/app/app.go` that initializes workflow service and calls `RunReminders` on each interval when enabled.
- [x] 2.2 Add non-overlap protection and error logging so failed runs do not stop future intervals.
- [x] 2.3 Ensure scheduler uses configured reminder parameters and UTC time source.

## 3. Verification

- [x] 3.1 Add or extend tests covering scheduler tick execution behavior and disabled mode behavior.
- [x] 3.2 Run `go test ./...`, `go vet ./...`, and `go build ./...` with zero new diagnostics.
