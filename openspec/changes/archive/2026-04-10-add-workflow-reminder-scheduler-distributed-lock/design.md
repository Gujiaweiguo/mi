## Context

Workflow reminder scheduling is already wired in-process in `backend/internal/app/app.go` and currently uses only a local `sync.Mutex` to prevent overlap within one process. In multi-replica deployments this does not prevent concurrent runs across replicas.

## Goals / Non-Goals

**Goals**
- Ensure at most one replica executes scheduled reminder evaluation at a time.
- Preserve existing reminder behavior and API contracts.
- Keep implementation minimal and MySQL-native.

**Non-Goals**
- No changes to reminder business rules (`RunReminders` internals).
- No new external distributed coordination dependency.
- No schema migration for lock tables in this slice.

## Decisions

1. **Use MySQL advisory lock (`GET_LOCK` / `RELEASE_LOCK`)**
   - Implement lock acquisition in scheduler runtime path.
   - Use a dedicated DB connection/session so acquire and release operate on the same MySQL session.

2. **Lock scope = scheduled run only**
   - Apply lock around each periodic scheduler tick execution.
   - Manual trigger endpoint remains unchanged.

3. **Configurable lock parameters**
   - Extend `workflow_reminder_scheduler` config with:
     - `lock_name`
     - `lock_wait_seconds`
   - Provide safe defaults and validation/clamping.

4. **Failure policy**
   - Lock acquisition failure logs and skips the current tick.
   - Scheduler continues to future ticks.
   - Release failure logs as error but does not crash process.

## Risks / Trade-offs

- Advisory lock is DB-session scoped; improper connection handling could break release semantics.
  - Mitigation: hold lock on dedicated `*sql.Conn` and release before close.
- Lock contention causes skipped runs.
  - Mitigation: explicit logs with lock name and reason.

## Verification

- Unit tests for scheduler lock path:
  - lock not acquired => reminder run not executed
  - lock acquired => reminder run executes and release is attempted
- Full `go test ./...`, `go vet ./...`, `go build ./...`
