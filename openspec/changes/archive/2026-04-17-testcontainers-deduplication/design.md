## Context

Every backend integration test follows the same container lifecycle: start a MySQL 8.0 testcontainer, connect, wait for readiness, migrate, bootstrap. This sequence is currently duplicated as file-local helpers (`newBillingTestDB`, `newLeaseTestDB`, etc.) or as inline setup code in test bodies. The `waitForDatabase` function is also duplicated, identically, in every file that has it.

The `platform/database` package already owns `Config`, `NewMigrator`, and `NewBootstrapRunner`. Adding a test-only helper to the same package is the natural home, since every consumer already imports `platformdb`.

## Goals / Non-Goals

**Goals:**
- Define `NewTestDB` and `WaitForDatabase` once, in the `platform/database` package.
- Replace all inline copies across the 14 integration test files.
- Preserve identical container configuration, credentials, migration logic, and bootstrap behavior.
- Use `t.Cleanup()` for teardown, matching the existing convention in the extracted helper functions.

**Non-Goals:**
- No changes to production code, test assertions, or test logic.
- No new test cases.
- No changes to the container image, environment variables, or bootstrap set.
- No test helpers that accept custom container configs. The shared helper encodes the one canonical integration test setup.

## Decisions

### Decision: New file `backend/internal/platform/database/testdb.go` with build tag `//go:build integration`

The helper file lives in the same package that already owns `Config`, `NewMigrator`, and `NewBootstrapRunner`. The `//go:build integration` tag ensures the file is only compiled during integration test runs, matching the convention used by every existing integration test file.

**Alternative considered:** create a separate `testhelpers` or `testdb` sub-package. Rejected because it would require a new package directory, a new import path, and a new go module dependency, all for a single file. Keeping it in the existing package is simpler and consistent with the Go standard library pattern (e.g., `net/http/httptest` living alongside `net/http`).

### Decision: `NewTestDB(t *testing.T, ctx context.Context) *sql.DB` encapsulates the full setup

The function signature takes a `*testing.T` and a `context.Context`, and returns a ready-to-use `*sql.DB`. Internally it: starts the testcontainer, resolves host/port, opens the connection, waits for readiness, applies migrations, runs bootstrap, and registers cleanup via `t.Cleanup()`. This matches the exact sequence that every existing `new*TestDB` helper performs.

**Alternative considered:** expose a `ContainerConfig` struct so callers can customize image, credentials, etc. Rejected because all 14 files use identical configuration and there is no foreseeable need for variation. Adding configurability now would be premature.

### Decision: `WaitForDatabase(ctx context.Context, db *sql.DB) error` exported for custom setups

Some integration test files (e.g., `app_integration_test.go`, `router_integration_test.go`) construct their database setup inline rather than through a `newTestDB` helper. Exporting `WaitForDatabase` lets those files replace their inline poll loop without forcing them into the full `NewTestDB` path if they have legitimate reasons for custom container handling.

The implementation matches the existing pattern exactly: 30-second deadline, 500ms poll interval, 5-second ping timeout.

**Alternative considered:** keep `WaitForDatabase` unexported and force all consumers through `NewTestDB`. Rejected because some tests (notably the `database_integration_test.go` tests that verify migrator behavior) need to control migration and bootstrap themselves, and only need the wait-and-connect portion.

### Decision: Migration path resolved relative to the `platform/database` directory

Since `testdb.go` lives in `backend/internal/platform/database/`, the migration filesystem uses `os.DirFS(".")` with `"migrations"` as the subdirectory. This is correct because the file's working directory during `go test` is the package directory (`backend/internal/platform/database/`).

The existing `new*TestDB` helpers in other packages use paths like `os.DirFS("../platform/database")` because they sit in sibling directories. By placing the shared helper in the `platform/database` package itself, the relative path simplifies to `"."`.

**Alternative considered:** accept a migration path parameter. Rejected because all consumers want the same migrations directory and the path is now trivially correct from the package that owns it.

### Decision: Bootstrap set `"all"` is hardcoded

Every existing helper calls `bootstrap.All()`. The shared helper does the same. No configurability needed.

## Risks / Trade-offs

- **Relative path assumption** → The helper assumes `go test` runs with the package directory as working directory, which is standard Go behavior. Any CI setup that changes `cwd` before running tests would break this, but that would also break the existing inline helpers, so no new risk is introduced.
- **One container per test function** → Each call to `NewTestDB` starts a fresh MySQL container. This is the existing behavior and is unchanged. Test-level isolation is valued over container reuse speed.
- **No test-only module** → The `testcontainers-go` dependency remains a test-scoped import in each consumer file's import block, not in the `platform/database` package's production imports. The `//go:build integration` tag ensures the dependency is only resolved during integration builds.

## Migration Plan

1. Create `testdb.go` with `NewTestDB` and `WaitForDatabase`.
2. Replace inline helpers in the 8 service-level integration test files (`billing`, `lease`, `invoice`, `workflow`, `docoutput`, `taxexport`, `excelio`, `reporting`).
3. Replace inline setup in the HTTP-level integration test files (`router_integration_test.go`, `invoice_receivable_integration_test.go`, `workflow_reminder_integration_test.go`, `masterdata_closure_integration_test.go`).
4. Replace inline helpers in `app_integration_test.go` and `database_integration_test.go` where applicable.
5. Run integration tests to confirm all tests pass with the shared helper.

Rollback is trivial: each modified test file can be reverted independently since no production code is touched.
