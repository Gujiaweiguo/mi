## Why

The production Docker Compose stack cannot run PDF generation (Chromium is absent from the backend image), has no resilience against crashes (no restart policies), exposes MySQL to the host network, runs the frontend container as root, serves traffic without security headers, and has no graceful shutdown or connection pool tuning. These gaps make the stack unfit for a production cutover and represent P0/P1 blockers identified across platform, code-quality, and external-calibration scans.

## What Changes

- **Add Chromium to backend Docker image** so PDF generation works inside containers.
- **Add `.dockerignore`** to prevent legacy code, `.git`, and build artifacts from entering Docker build context.
- **Add restart policies, resource limits, and logging config** to docker-compose.production.yml so services recover from crashes and do not exhaust host memory.
- **Remove MySQL external port exposure** from docker-compose.production.yml; use internal network only.
- **Add non-root user to frontend Dockerfile** so nginx workers do not run as root.
- **Add security headers, gzip, proxy timeouts, and upload size limits** to nginx production.conf.
- **Implement graceful shutdown** in the Go backend (signal capture, in-flight request drain, scheduler goroutine cleanup).
- **Add database connection pool configuration** (max open/idle connections, max lifetime).
- **Add database connectivity check to health endpoint** so the health check reflects real downstream status.

## Capabilities

### New Capabilities

_(none — all changes modify existing capabilities)_

### Modified Capabilities

- `platform-foundation`: add requirements for graceful shutdown, connection pool configuration, and health endpoint DB connectivity check.
- `deployment-and-cutover-operations`: add requirements for Chromium availability in backend image, `.dockerignore`, restart policies, resource limits, logging config, MySQL network isolation, frontend non-root user, and nginx security hardening.

## Impact

- **Dockerfiles**: `deploy/docker/backend.Dockerfile` (add Chromium package), `deploy/docker/frontend.Dockerfile` (add non-root user).
- **Docker Compose**: `deploy/compose/docker-compose.production.yml` (restart, limits, logging, MySQL network).
- **Nginx**: `deploy/nginx/production.conf` (security headers, gzip, timeouts, upload size).
- **Go backend**:
  - `cmd/server/main.go` (signal handling).
  - `internal/app/app.go` (graceful shutdown, connection pool).
  - `internal/http/handlers/health.go` (DB ping).
  - `internal/http/router.go` (shutdown-aware server wiring).
- **Root config**: new `.dockerignore` file.
- **No API changes, no database schema changes, no frontend code changes.**
- **No breaking changes.**
