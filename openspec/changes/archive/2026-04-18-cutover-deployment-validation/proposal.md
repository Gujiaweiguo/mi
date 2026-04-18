## Why

The production Docker Compose stack cannot complete a full deployment without manual intervention. Three issues block `docker-compose up` from producing a working system: (1) database migrations (`dbops`) are not included in the backend image and MySQL's port is not published, so the bootstrap script cannot run; (2) the production.env file uses abbreviated env var names (`MI_DB_*`) that don't match Viper's expected names (`MI_DATABASE_*`), making them dead values; (3) runtime directories (logs, documents, uploads, mysql) are not created by any automated step.

## What Changes

- Build `dbops` alongside `server` in the backend Dockerfile.
- Add a `migrate` service to `docker-compose.production.yml` that runs `dbops migrate && dbops bootstrap` as an init container before the backend starts.
- Fix env var names in `production.env` to match Viper's expected naming (`MI_DB_*` → `MI_DATABASE_*`, `MI_JWT_SECRET` → `MI_AUTH_JWT_SECRET`, etc.).
- Add a `scripts/setup-runtime-dirs.sh` script that creates the required runtime directories with correct permissions.
- No application code changes — only deployment configuration.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `deployment-and-cutover-operations`: fix deployment stack so `docker-compose up` produces a working system with migrated database.

## Impact

- `deploy/docker/backend.Dockerfile`: build both `server` and `dbops`
- `deploy/compose/docker-compose.production.yml`: add `migrate` init service
- `deploy/env/production.env`: fix env var names to match Viper
- `scripts/setup-runtime-dirs.sh`: new file (~15 lines)
- Zero application code changes.
