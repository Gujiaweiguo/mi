# Tasks: Add Workflow Reminder Scheduler Distributed Lock

## 1. Config and scheduler wiring

- [x] 1.1 Extend `workflow_reminder_scheduler` config with distributed lock fields (`lock_name`, `lock_wait_seconds`) and defaults.
- [x] 1.2 Update backend environment config files to include new lock fields.
- [x] 1.3 Add/extend config tests for loading and env overrides of lock fields.

## 2. Distributed lock implementation

- [x] 2.1 Implement MySQL advisory lock wrapper (`GET_LOCK`/`RELEASE_LOCK`) for scheduler usage with connection-scoped correctness.
- [x] 2.2 Apply distributed lock in scheduler tick path so only lock owner executes `RunReminders`.
- [x] 2.3 Preserve current non-overlap and failure-safe scheduler behavior with clear logging.

## 3. Verification

- [x] 3.1 Add scheduler lock-path unit tests (lock-acquired and lock-not-acquired cases).
- [x] 3.2 Run `go test ./...`, `go vet ./...`, and `go build ./...`.
