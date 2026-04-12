# Deployment Defaults

## Scope

This document records the current default deployment conventions for the MI replacement system. It is the single place to check default ports, paths, environment files, and startup commands before changing deployment behavior.

## Supported environments

The repository now keeps two deployment environments only:

- `development` — local backend + local frontend dev server + local Docker MySQL
- `production` — Docker Compose stack with `nginx + frontend + backend + mysql`

There is no separate deployed `test` environment configuration anymore.

## Development defaults

Primary references:

- `deploy/env/development.env`
- `backend/config/development.yaml`

Default values:

| Item | Default |
|---|---|
| Backend host | `0.0.0.0` |
| Backend port | `5180` |
| Frontend dev port | `5173` |
| Database host | `127.0.0.1` |
| Database port | `3306` |
| Database name | `mi_dev` |
| Database user | `mi_dev` |
| Database password | `mi_dev` |
| Log level | `debug` |
| Generated documents path | `./artifacts/generated-documents` |
| Uploads path | `./artifacts/uploads` |
| Logs path | `./artifacts/logs` |
| Scheduler | `disabled` |

Development startup model:

1. Start local MySQL 8 separately.
2. Run the backend with `backend/config/development.yaml`.
3. Run the frontend with Vite on port `5173`.

## Production defaults

Primary references:

- `deploy/env/production.env`
- `deploy/compose/docker-compose.production.yml`
- `backend/config/production.yaml`
- `deploy/nginx/production.conf`

Default values:

| Item | Default |
|---|---|
| External HTTP port | `80` |
| External MySQL port | `3306` |
| Backend host | `0.0.0.0` |
| Backend port | `5180` |
| Database host | `mysql` |
| Database port | `3306` |
| Database name | `mi_prod` |
| Database user | `mi_prod` |
| Database password | `change-me` |
| Root password | `change-me-root` |
| Log level | `info` |
| Generated documents path | `/app/generated-documents` |
| Uploads path | `/app/uploads` |
| Logs path | `/app/logs` |
| Runtime logs mount | `../runtime/production/logs` |
| Runtime documents mount | `../runtime/production/documents` |
| Runtime uploads mount | `../runtime/production/uploads` |
| Runtime MySQL mount | `../runtime/production/mysql` |
| Config mount | `../../backend/config` |
| Scheduler | `disabled` |

Production reverse-proxy routing:

- `/api/*` → `backend:5180`
- `/*` → `frontend:80`

Production startup command:

```bash
cd deploy/compose
docker compose --env-file ../env/production.env -f docker-compose.production.yml up -d
```

## Files to edit when changing defaults

### Development

- `deploy/env/development.env` — centralized reference values
- `backend/config/development.yaml` — actual backend runtime config

### Production

- `deploy/env/production.env` — external ports, credentials, runtime mount paths
- `backend/config/production.yaml` — backend runtime config
- `deploy/compose/docker-compose.production.yml` — Compose wiring and env interpolation
- `deploy/nginx/production.conf` — reverse-proxy target port/path rules

## Required production overrides before real deployment

The following placeholders must be replaced before production use:

- `MYSQL_PASSWORD=change-me`
- `MYSQL_ROOT_PASSWORD=change-me-root`
- `MI_DB_PASSWORD=change-me`
- `MI_JWT_SECRET=change-me-production-secret`

## Consistency rule

The backend port is intentionally aligned across both supported environments:

- `development`: `5180`
- `production`: `5180`

If this port changes again later, update all of the following together:

- backend YAML config
- deployment env file
- nginx upstream target
- Compose healthcheck/default interpolation
- Dockerfile `EXPOSE` when relevant
