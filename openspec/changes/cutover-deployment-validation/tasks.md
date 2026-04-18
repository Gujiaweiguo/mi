## 1. Fix Backend Dockerfile

- [ ] 1.1 Update `deploy/docker/backend.Dockerfile` to build both `server` and `dbops` binaries in the build stage, and copy both to the runtime stage

## 2. Add Migrate Init Service

- [ ] 2.1 Add a `migrate` service to `deploy/compose/docker-compose.production.yml` that runs `dbops migrate && dbops bootstrap` using the backend image, depends on mysql being healthy, and the backend service depends on `migrate` completing

## 3. Fix Env Var Names

- [ ] 3.1 Fix `deploy/env/production.env` — rename `MI_DB_*` to `MI_DATABASE_*`, `MI_JWT_SECRET` to `MI_AUTH_JWT_SECRET`, `MI_JWT_TOKEN_EXPIRY_SECONDS` to `MI_AUTH_TOKEN_EXPIRY_SECONDS`, `MI_SCHEDULER_*` to `MI_WORKFLOW_REMINDER_SCHEDULER_*`, `MI_STORAGE_DOCUMENTS` to `MI_STORAGE_GENERATED_DOCUMENTS_PATH`, `MI_STORAGE_UPLOADS` to `MI_STORAGE_UPLOADS_PATH`, `MI_STORAGE_LOGS` to `MI_STORAGE_LOGS_PATH`
- [ ] 3.2 Update the backend service in docker-compose to pass `MI_DATABASE_HOST`, `MI_DATABASE_PORT`, `MI_DATABASE_NAME`, `MI_DATABASE_USER`, `MI_DATABASE_PASSWORD`, and `MI_AUTH_JWT_SECRET` as environment variables (using values from the env file)

## 4. Create Runtime Dir Setup Script

- [ ] 4.1 Create `scripts/setup-runtime-dirs.sh` that creates `deploy/runtime/production/{logs,documents,uploads,mysql}` with uid 10001 ownership

## 5. Verification

- [ ] 5.1 Verify `docker-compose -f deploy/compose/docker-compose.production.yml config` produces valid output (no YAML errors)
- [ ] 5.2 Verify backend Dockerfile builds both binaries correctly
