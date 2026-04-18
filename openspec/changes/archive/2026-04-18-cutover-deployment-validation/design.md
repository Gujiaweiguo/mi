## Context

The production stack has 4 services (nginx, frontend, backend, mysql) but cannot produce a working system via `docker-compose up` alone. The database needs migrations and seed data before the backend can serve requests. The env file has misleading variable names that don't actually configure the backend.

## Goals / Non-Goals

**Goals:**
- `docker-compose up` with a single `--env-file` flag produces a working system.
- Database is migrated and seeded automatically before the backend starts.
- Env var names match Viper expectations so env vars actually override YAML config.
- Runtime directories are created with correct permissions.

**Non-Goals:**
- TLS termination (requires external LB or cert management).
- Docker secrets / Vault integration (future).
- Production monitoring / alerting.
- Changing the application config loading mechanism.

## Decisions

### Decision 1 — dbops in the same image, migrate init service

**Choice:** Build `dbops` into the backend Docker image alongside `server`. Add a `migrate` service to compose that runs dbops and exits, with the backend depending on it.

**Rationale:** Keeping dbops in the same image ensures version consistency between migrations and the server. The init-service pattern is standard Docker Compose practice.

### Decision 2 — Fix env var names, not YAML keys

**Choice:** Rename env vars in `production.env` to match Viper's expected names.

**Rationale:** Viper's `AutomaticEnv()` with `SetEnvPrefix("MI")` and `SetEnvKeyReplacer(".", "_")` produces specific env var names. The env file should match those names. The existing `db-bootstrap.sh` script already uses the correct names.

### Decision 3 — Entrypoint script for migrate service

**Choice:** Use a shell command in the compose file for the migrate service rather than a custom entrypoint script.

**Rationale:** The migrate service runs once and exits. A simple command in compose is sufficient.
