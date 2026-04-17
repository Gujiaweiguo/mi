# Proposal: Migrator Idempotency Tracking

## Why

The custom migrator (`backend/internal/platform/database/migrator.go`) blindly re-executes every `.up.sql` file on each invocation. It has no concept of which migrations have already been applied. The current crop of 19 migration files uses bare `CREATE TABLE` statements without `IF NOT EXISTS`, so running `dbops migrate` a second time crashes with "table already exists".

This makes deployments fragile. Any CI/CD retry, manual re-run, or container restart that re-executes the migrate step will fail. The system should tolerate repeated migrate calls without error.

## What Changes

Add a `schema_migrations` tracking table that records which migrations have been applied. Before executing each `.up.sql` file, check whether its version is already recorded. After successful execution, insert a row. This makes `ApplyUpMigrations()` idempotent.

## Capabilities

Modifies `platform-foundation` by adding an idempotent migration requirement: the database migrator SHALL track applied migrations and skip them on subsequent runs.

## Impact

- `backend/internal/platform/database/migrator.go` — core change, roughly 30 lines of addition/modification
- New file `backend/internal/platform/database/migrator_test.go` — unit tests
- No API changes
- No frontend changes
- No Docker or deployment topology changes
