## 1. Docker Build Configuration

- [x] 1.1 Create `.dockerignore` at repository root excluding `.git`, `.gitignore`, `legacy_code/`, `legacy_docs/`, `openspec/`, `.opencode/`, `.sisyphus/`, `**/node_modules/`, `*.md`, `deploy/`, `.env*`, `*.log` but NOT `backend/` or `frontend/`
- [x] 1.2 Add `apk add --no-cache chromium` to backend Dockerfile runtime stage after `adduser` and before `COPY --from=builder`
- [x] 1.3 Add `USER nginx` directive to frontend Dockerfile after the last `COPY` line in the runtime stage

## 2. Docker Compose Production Hardening

- [x] 2.1 Add `restart: unless-stopped` to all four services in docker-compose.production.yml
- [x] 2.2 Add memory limits and reservations to each service (nginx 128 MB, frontend 64 MB, backend 512 MB, mysql 1024 MB)
- [x] 2.3 Add JSON-file logging driver with `max-size: "10m"` and `max-file: "3"` to all services
- [x] 2.4 Remove `ports` mapping from mysql service so it is only reachable on the internal Docker network
- [x] 2.5 Add `stop_grace_period: 30s` to backend service for graceful shutdown drain time

## 3. Nginx Security Hardening

- [x] 3.1 Add security headers to production.conf server block: `X-Content-Type-Options nosniff`, `X-Frame-Options DENY`, `X-XSS-Protection 1; mode=block`, `Strict-Transport-Security max-age=63072000`, `Content-Security-Policy default-src 'self'`, `Referrer-Policy strict-origin-when-cross-origin`
- [x] 3.2 Enable gzip compression in production.conf with `gzip on`, `gzip_vary on`, `gzip_min_length 256`, and `gzip_types` covering `text/css`, `application/javascript`, `application/json`, `text/xml`, `image/svg+xml`
- [x] 3.3 Add proxy timeouts to `/api/` location block: `proxy_connect_timeout 10s`, `proxy_send_timeout 30s`, `proxy_read_timeout 60s`
- [x] 3.4 Add `client_max_body_size 50m` at server level in production.conf

## 4. Go Backend Graceful Shutdown

- [x] 4.1 Refactor `cmd/server/main.go` to create context via `signal.NotifyContext` for SIGINT and SIGTERM, pass context to `App.Run`
- [x] 4.2 Add `App.Shutdown(ctx)` method that calls `server.Shutdown`, stops the workflow reminder scheduler, and closes `*sql.DB` with a 15-second drain timeout
- [x] 4.3 Refactor `App.Run` to accept context, start HTTP server in a goroutine, block on context cancellation, then call shutdown

## 5. Database Connection Pool

- [x] 5.1 Add `db.SetMaxOpenConns(25)`, `db.SetMaxIdleConns(10)`, `db.SetConnMaxLifetime(5 * time.Minute)`, `db.SetConnMaxIdleTime(1 * time.Minute)` after `sql.Open` in `internal/app/app.go`

## 6. Health Endpoint DB Check

- [x] 6.1 Update `NewHealthHandler` signature to accept `*sql.DB` alongside `*config.Config`, store as field on handler struct
- [x] 6.2 Update `Get` handler to call `db.PingContext` with 2-second timeout, return 503 with `{"status": "degraded"}` on failure, 200 with `{"status": "ok"}` on success
- [x] 6.3 Update `internal/http/router.go` to pass `*sql.DB` to `NewHealthHandler`

## 7. Verification

- [x] 7.1 Run `go build ./...` and `go test ./...` from backend directory to verify no regressions
- [x] 7.2 Verify health endpoint returns 200 when database is reachable and 503 when database is unreachable
- [x] 7.3 Verify Dockerfiles build without errors and chromium is present in backend image

## Final Verification Wave

- [x] F1 Review Dockerfile changes for security best practices (non-root users, minimal packages, no secrets)
- [x] F2 Review docker-compose.production.yml for production readiness (restart policies, limits, logging, no MySQL exposure)
- [x] F3 Review nginx production.conf for security headers completeness (all six headers, gzip types, timeouts, upload size)
- [x] F4 Review Go backend changes for correctness and test coverage (graceful shutdown, pool config, health DB check)
