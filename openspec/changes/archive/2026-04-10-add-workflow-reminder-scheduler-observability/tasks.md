# Tasks: Add Workflow Reminder Scheduler Observability

## 1. Scheduler telemetry model

- [x] 1.1 Add internal scheduler observability state in `backend/internal/app/app.go` for run counters, timestamps, and recent duration.
- [x] 1.2 Add helper methods for outcome-specific state transitions (success, failure, lock-skip).

## 2. Structured observability logging

- [x] 2.1 Emit structured per-tick logs with timing, outcome counts, and cumulative counters.
- [x] 2.2 Emit warning-level signal when consecutive failures exceed threshold.

## 3. Verification

- [x] 3.1 Add/extend unit tests in `backend/internal/app/app_test.go` for observability state transitions.
- [x] 3.2 Run `go test ./...`, `go vet ./...`, and `go build ./...`.
