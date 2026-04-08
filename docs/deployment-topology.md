# Deployment Topology

## Test and Production Services

Both test and production topologies use the same four-service shape:

- nginx
- frontend
- backend
- mysql

Compose manifests:

- `deploy/compose/docker-compose.test.yml`
- `deploy/compose/docker-compose.production.yml`

## Mounted Runtime Paths

The Compose stacks mount the runtime paths required by the platform and cutover specs:

- backend config: `../../backend/config:/app/config:ro`
- backend logs: `../runtime/<env>/logs:/app/logs`
- generated documents: `../runtime/<env>/documents:/app/generated-documents`
- uploads: `../runtime/<env>/uploads:/app/uploads`
- mysql data: `../runtime/<env>/mysql:/var/lib/mysql`

Runtime roots are kept under:

- `deploy/runtime/test/`
- `deploy/runtime/production/`

## Health Endpoints

- backend: `/health` and `/healthz`
- frontend/nginx: root path `/`
- nginx proxy to backend health: `/api/healthz`

## Bring-Up Procedure

Render the Compose definitions before starting either environment:

```bash
scripts/compose-test-config.sh
scripts/compose-production-config.sh
```

Both commands now run the same preflight contract before rendering:

- validate the target compose file exists
- validate the target deploy env file exists
- validate the matching backend config file exists
- validate `deploy/runtime/<env>/{logs,documents,uploads,mysql}` exists and is writable
- fail before startup if `docker compose config` cannot render with the selected env file

Smoke-test the full stack:

```bash
scripts/compose-smoke-test.sh test --build
scripts/compose-smoke-test.sh production --build
```

The smoke script starts the stack, waits for `mysql`, `backend`, `frontend`, and `nginx` to report healthy, then verifies backend, frontend, and nginx proxy endpoints. Pass `--keep-running` if the stack should stay up after validation.

If the host already uses the default published ports, override them for validation:

```bash
MI_HTTP_PORT=18080 MI_MYSQL_PORT=13306 scripts/compose-smoke-test.sh production --build
```

## Backup and Restore

Create an environment backup bundle:

```bash
scripts/db-backup.sh test
scripts/db-backup.sh production
```

Each bundle contains:

- a MySQL logical dump from the running Compose stack
- snapshots of runtime `logs`, `documents`, and `uploads`
- the selected backend config file
- the selected deploy env file

Restore a backup bundle:

```bash
scripts/db-restore.sh test <backup-archive>
scripts/db-restore.sh production <backup-archive>
scripts/db-restore.sh test <backup-archive> --restore-runtime-files
```

By default the restore script imports the MySQL dump only. Pass `--restore-runtime-files` to also replace the on-disk runtime directories plus the backed-up config/env snapshots.

## Preflight Checks

Before test or production bring-up:

1. Confirm the runtime directories for the target environment exist and are writable.
2. Confirm `backend/config/<env>.yaml` matches the target environment.
3. Confirm `deploy/env/<env>.env` contains the intended MySQL credentials.
4. Render the Compose file to catch path or syntax errors before startup.

## Troubleshooting

- If `mysql` never becomes healthy, verify the mounted MySQL data directory is writable and the configured credentials are correct.
- If `backend` never becomes healthy, verify `MI_CONFIG_FILE` points at the expected config file and that `/app/config` is mounted read-only.
- If `nginx` is healthy but `/api/healthz` fails, verify the backend container is healthy and the nginx config under `deploy/nginx/*.conf` still proxies `/api/` to `backend:8080`.
- If document generation or uploads fail at runtime, verify the `documents` and `uploads` directories under `deploy/runtime/<env>/` are writable by the container runtime.
