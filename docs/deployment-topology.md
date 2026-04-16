# Deployment Topology

## Production Services

The production topology uses the same four-service shape:

- nginx
- frontend
- backend
- mysql

Compose manifest:

- `deploy/compose/docker-compose.production.yml`

## Mounted Runtime Paths

The Compose stacks mount the runtime paths required by the platform and cutover specs:

- backend config: `../../backend/config:/app/config:ro`
- backend logs: `../runtime/<env>/logs:/app/logs`
- generated documents: `../runtime/<env>/documents:/app/generated-documents`
- uploads: `../runtime/<env>/uploads:/app/uploads`
- mysql data: `../runtime/<env>/mysql:/var/lib/mysql`

Runtime root is kept under:

- `deploy/runtime/production/`

## Health Endpoints

- backend: `/health` and `/healthz`
- frontend/nginx: root path `/`
- nginx proxy to backend health: `/api/healthz`

## Bring-Up Procedure

Render the production Compose definition before startup:

```bash
scripts/compose-production-config.sh
```

The render command runs the same preflight contract before rendering:

- validate the target compose file exists
- validate the target deploy env file exists
- validate the matching backend config file exists
- validate `deploy/runtime/<env>/{logs,documents,uploads,mysql}` exists and is writable
- fail before startup if `docker compose config` cannot render with the selected env file

Smoke-test the production stack:

```bash
scripts/compose-smoke-test.sh production --build
```

The smoke script starts the stack, waits for `mysql`, `backend`, `frontend`, and `nginx` to report healthy, then verifies backend, frontend, and nginx proxy endpoints. Pass `--keep-running` if the stack should stay up after validation.

If the host already uses the default published ports, override them for validation:

```bash
MI_HTTP_PORT=18080 MI_MYSQL_PORT=13306 scripts/compose-smoke-test.sh production --build
```

## Backup and Restore

Create a production backup bundle:

```bash
scripts/db-backup.sh production
```

Each bundle contains:

- a MySQL logical dump from the running Compose stack
- snapshots of runtime `logs`, `documents`, and `uploads`
- the selected backend config file
- the selected deploy env file

Restore a production backup bundle:

```bash
scripts/db-restore.sh production <backup-archive>
scripts/db-restore.sh production <backup-archive> --restore-runtime-files
```

By default the restore script imports the MySQL dump only. Pass `--restore-runtime-files` to also replace the on-disk runtime directories plus the backed-up config/env snapshots.

## Preflight Checks

Before production bring-up:

1. Confirm the runtime directories for the target environment exist and are writable.
2. Confirm `backend/config/<env>.yaml` matches the target environment.
3. Confirm `deploy/env/<env>.env` contains the intended MySQL credentials and non-placeholder production secrets.
4. Render the Compose file to catch path or syntax errors before startup.

For the supported production workflow, `scripts/compose-preflight.sh production` now rejects the documented placeholder values for `MYSQL_PASSWORD`, `MYSQL_ROOT_PASSWORD`, `MI_DB_PASSWORD`, and `MI_JWT_SECRET` before container startup.

## Troubleshooting

- If `mysql` never becomes healthy, verify the mounted MySQL data directory is writable and the configured credentials are correct.
- If `backend` never becomes healthy, verify `MI_CONFIG_FILE` points at the expected config file and that `/app/config` is mounted read-only.
- If `nginx` is healthy but `/api/healthz` fails, verify the backend container is healthy and the nginx config under `deploy/nginx/*.conf` still proxies `/api/` to `backend:5180`.
- If document generation or uploads fail at runtime, verify the `documents` and `uploads` directories under `deploy/runtime/<env>/` are writable by the container runtime.
