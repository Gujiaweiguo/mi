## 1. Migrator Core

- [x] 1.1 Add `ensureSchemaMigrationsTable` method to `Migrator` that creates `schema_migrations` table with `CREATE TABLE IF NOT EXISTS` if it does not already exist
- [x] 1.2 Add `getAppliedVersions` method that queries `schema_migrations` and returns a `map[string]bool` of applied version names
- [x] 1.3 Refactor `ApplyUpMigrations` to call `ensureSchemaMigrationsTable`, then `getAppliedVersions`, then skip already-applied migrations and insert new versions after each successful application
- [x] 1.4 Use the migration filename (without `.up.sql` suffix) as the version identifier

## 2. Testing

- [x] 2.1 Create `backend/internal/platform/database/migrator_test.go` with unit tests
- [x] 2.2 Test: first run applies all migrations and records all versions
- [x] 2.3 Test: second run skips all migrations (idempotent)
- [x] 2.4 Test: partially applied state resumes correctly
- [x] 2.5 Run `go build ./...` and `go test -short ./...` to verify no regressions

## 3. Verification

- [x] 3.1 Run integration tests to verify migrator still works with Testcontainers
- [x] 3.2 Run `scripts/verification/run-unit.sh` to generate evidence

## Final Verification Wave

- [x] F1 Review migrator.go changes for correctness (tracking table, version check, version insert)
- [x] F2 Review test coverage (idempotent, partial resume, fresh database scenarios)
- [x] F3 Verify existing integration tests still pass unchanged
