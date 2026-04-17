# Design: Migrator Idempotency Tracking

## Context

The current migrator is 48 lines with zero state tracking. It reads `.up.sql` files from an `fs.FS` interface, sorts them alphabetically, and executes them sequentially via `ExecuteSQLFile`. There is no record of which migrations have run. All 19 existing up-migrations use bare `CREATE TABLE` statements. Running `ApplyUpMigrations()` a second time always fails.

The migrator is invoked through `dbops migrate` in `backend/cmd/dbops/main.go`, which constructs a `Migrator` with `os.DirFS(".")` pointing at `internal/platform/database/migrations`.

## Goals

- Make `ApplyUpMigrations()` idempotent. Calling it multiple times produces the same result as calling it once.
- Record applied migration versions for auditability.
- Zero impact on existing integration tests. Those tests use fresh Testcontainers instances that start with an empty database, so adding a tracking table changes nothing for them.

## Non-Goals

- NOT adding down-migration or rollback support.
- NOT adding advisory locking. The deployment model is single-instance (one backend container in Docker Compose).
- NOT switching to golang-migrate or any other external migration library.
- NOT adding CLI progress output, dry-run mode, or verbose logging.

## Decisions

### Decision 1: Inline `schema_migrations` table creation in the migrator

The tracking table must exist before any migration can be checked for prior application. Putting it in a separate migration file creates a chicken-and-egg problem: the migrator would need the tracking table to decide whether to run the tracking-table migration.

The table is created with:

```sql
CREATE TABLE IF NOT EXISTS schema_migrations (
    version VARCHAR(255) PRIMARY KEY,
    applied_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
)
```

This runs at the start of `ApplyUpMigrations()`, before the migration loop.

**Alternative considered:** Switch to golang-migrate. Rejected because it is a large dependency swap for what amounts to a 30-line change in an existing, working migrator.

### Decision 2: Use migration filename as the version identifier

Filenames like `000001_auth_org_bootstrap_schema.up.sql` are already unique and lexicographically sortable. Strip the `.up.sql` suffix and use the base filename as the version key. No separate version numbering scheme is needed.

### Decision 3: Execute each migration and version insert in a single database transaction

Wrapping both the migration SQL and the version insert in one transaction prevents partial states where a migration ran but the version was not recorded.

Caveat: MySQL implicitly commits on DDL statements (`CREATE TABLE`, `ALTER TABLE`), so for DDL-heavy migrations this is best-effort atomicity. The transaction still provides true atomicity for DML-only migrations. Combined with Decision 1's use of `CREATE TABLE IF NOT EXISTS` in migration files (which should be updated over time), re-attempting a partially tracked DDL migration is safe.

**Alternative considered:** Record the version after migration without a transaction. Simpler code but risks missing the version record if the process crashes between migration completion and insert.

### Decision 4: Skip already-applied migrations silently

If a migration version is already in `schema_migrations`, skip it without logging a warning or returning an error. Idempotent behavior should be invisible to the caller.

## Risks and Trade-offs

**MySQL DDL auto-commits.** `CREATE TABLE` inside a transaction commits implicitly. If the migration succeeds but the version insert fails, the migration will be re-attempted on the next run. This is safe because `CREATE TABLE IF NOT EXISTS` (which migration files should use going forward) is itself idempotent for DDL.

**No locking.** If two instances run migrate simultaneously, they could both try to apply the same migration. Acceptable because Docker Compose runs a single backend instance, and `dbops migrate` is an operational command, not a hot-path.

**Schema table pollution.** Adding `schema_migrations` to the application database. This is standard practice. Every major migration tool (golang-migrate, Flyway, Laravel, Rails) does the same thing.
