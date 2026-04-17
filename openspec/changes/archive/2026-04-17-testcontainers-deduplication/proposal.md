## Why

Fourteen integration test files across the backend contain roughly 10 copies of `new*TestDB` helper functions and 12 copies of `waitForDatabase`, totaling over 500 lines of identical boilerplate. Each copy follows the same six-step sequence: start a MySQL 8.0 testcontainer, resolve host and mapped port, open a `sql.DB` connection, poll until the database accepts connections, run migrations, and bootstrap seed data. Every new integration test requires copy-pasting this entire setup from an existing test file and adjusting the helper name.

This duplication means a one-line change to container credentials, startup timeout, or migration path must be replicated across up to 14 files. It also raises the barrier to writing new integration tests, since the setup code is longer than most test bodies.

## What Changes

- Extract a shared `NewTestDB(t *testing.T, ctx context.Context) *sql.DB` helper into `backend/internal/platform/database/testdb.go`, behind the `//go:build integration` build tag.
- Export `WaitForDatabase(ctx context.Context, db *sql.DB) error` alongside it for tests that need custom setup.
- Replace all inline `new*TestDB` and `waitForDatabase` copies in the 14 integration test files with a single import of the shared helper.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: add a shared integration test database helper so that test infrastructure setup is defined once and reused across all integration test packages.

## Impact

- `backend/internal/platform/database/testdb.go`: new file (~60 lines) containing `NewTestDB` and `WaitForDatabase`.
- 14 integration test files: remove inline `new*TestDB` and `waitForDatabase` functions, import the shared helper instead.
- Zero production code changes.
