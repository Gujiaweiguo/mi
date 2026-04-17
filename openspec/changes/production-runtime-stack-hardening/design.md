## Context

The production Docker Compose stack is close to functional but has nine gaps that block a production cutover. The backend image runs on Alpine but ships no Chromium binary, so the PDF generation path (`exec.CommandContext` calling `chromeBinary` with `--headless`) will always fail at `resolveChromeBinary()`. The frontend container runs nginx as root. MySQL exposes port 3306 to the host network. The nginx reverse proxy has no security headers, no gzip, no proxy timeouts, and no upload size limit. Docker Compose has no restart policies, no memory limits, and no log rotation. The Go backend has no graceful shutdown; `main.go` calls `app.Run()` which blocks on `server.ListenAndServe()` and ignores SIGTERM. The database connection is opened with `sql.Open("mysql", dsn)` and never tuned for pool size or lifetime. The health endpoint returns static JSON with no check against MySQL. There is no `.dockerignore`, so the Docker build context includes `.git`, `legacy_code/`, `legacy_docs/`, `node_modules/`, and other unnecessary weight.

Current state of the affected files:

- `backend.Dockerfile`: Alpine 3.20 runtime, non-root `appuser` (uid 10001), no Chromium.
- `frontend.Dockerfile`: `nginx:1.27-alpine`, no USER directive, runs as root.
- `docker-compose.production.yml`: four services (nginx, frontend, backend, mysql), health checks present, no restart/limits/logging, MySQL ports mapped to host.
- `production.conf`: minimal reverse proxy, `/api/` to backend:5180, `/` to frontend:80, no headers/gzip/timeouts.
- `app.go`: creates `*sql.DB` via `sql.Open` with no pool config, starts HTTP server via `ListenAndServe`, runs a workflow reminder scheduler goroutine with a `stopCh` channel.
- `main.go`: calls `app.New()` then `application.Run()`, no signal handling.
- `health.go`: `HealthHandler` holds only `*config.Config`, returns static `{"status":"ok"}`.
- `router.go`: wires `HealthHandler` without passing `*sql.DB`, uses `gin.Logger()` and `gin.Recovery()` middleware.
- No `.dockerignore` exists at the repository root.

## Goals / Non-Goals

**Goals:**

- Make PDF generation work inside the backend Docker container by installing Chromium.
- Shrink Docker build context by excluding irrelevant directories via `.dockerignore`.
- Make the stack resilient to crashes with restart policies, and prevent runaway memory with resource limits and log rotation.
- Remove MySQL's host-facing port so it is only reachable on the internal Docker network.
- Run the frontend nginx container as a non-root user.
- Harden the nginx reverse proxy with security headers, gzip compression, proxy timeouts, and an upload body size limit.
- Implement graceful shutdown in the Go backend so SIGTERM drains in-flight requests and stops the scheduler goroutine cleanly.
- Configure the `*sql.DB` connection pool (max open connections, max idle connections, max connection lifetime).
- Make the health endpoint verify MySQL connectivity so the Docker health check reflects real downstream status.

**Non-Goals:**

- Switching from `database/sql` to Gorm (the codebase uses `database/sql` directly).
- Replacing the custom migrator with golang-migrate.
- Adding TLS termination (nginx listens on plain HTTP; TLS is an infrastructure-level concern outside this change).
- Changing the Go request logging from `gin.Logger()` to Zap middleware.
- Modifying any frontend Vue code or any API contract.
- Adding rate limiting, IP allowlisting, or authentication at the nginx level.
- Introducing a container orchestrator beyond Docker Compose (no Kubernetes, no Swarm).

## Decisions

### 1. Chromium package in backend Docker image

**Approach:** Install `chromium` from the Alpine 3.20 community repository in the runtime stage of `backend.Dockerfile`. Add `RUN apk add --no-cache chromium` after the existing `adduser` line, before `COPY --from=builder`.

**Why this package:** Alpine's `chromium` package installs the actual browser binary that `resolveChromeBinary()` already looks for (it checks `chromium`, `chromium-browser`, and `google-chrome` in that order). The Alpine `chromium` package symlinks the binary to `/usr/bin/chromium`, which matches the lookup.

**Alternatives considered:**

- `google-chrome` (Google's official .deb): Not available for Alpine. Would require switching the runtime to a Debian base, which would roughly double the image size and break the existing `adduser`/Alpine patterns.
- `playwright` or `puppeteer` with bundled Chromium: Overkill. The codebase uses `exec.CommandContext` to call the Chrome binary directly, not a Node-based driver. Adding a Node runtime to the Go backend image would be a poor trade.
- Static wkhtmltopdf binary: Would require rewriting the PDF generation code from Chrome flags to wkhtmltopdf flags, a larger and riskier change for no real benefit.

**Implementation details:**

- The `chromium` package on Alpine pulls in several font and dependency packages, increasing the runtime image by roughly 200-250 MB. This is acceptable for a backend service that must render HTML to PDF.
- The `--no-sandbox` flag is already passed in the `exec.CommandContext` call, which is necessary because the container runs as a non-root user (`appuser`, uid 10001). No additional Chrome flags are needed.
- No font packages are added beyond what `chromium` pulls in. If CJK character rendering is needed in PDFs, that can be addressed in a follow-up change by adding `wqy-zenhei` or similar font packages.

### 2. `.dockerignore` file

**Approach:** Create a `.dockerignore` at the repository root that excludes `.git`, `legacy_code/`, `legacy_docs/`, `node_modules/`, `frontend/node_modules/`, `*.md` (except those needed at build time, which is none), `.opencode/`, `.sisyphus/`, `openspec/`, and build artifact directories.

**Why:** The Docker build context is the entire repository root (the `context: ../..` in docker-compose). Without a `.dockerignore`, Docker sends the full repo including `.git` history, legacy .NET code, and documentation to the daemon. This slows builds and increases context transfer time.

**Alternatives considered:**

- Per-Dockerfile `.dockerignore`: Docker only supports one `.dockerignore` per build context. Since both Dockerfiles share the same context root, a single file covers both.
- No `.dockerignore`, rely on `.gitignore`: Unrelated. `.gitignore` controls what enters the repo; `.dockerignore` controls what enters the Docker build context.

**Implementation details:**

- The file should be at `/opt/code/mi/.dockerignore`.
- Include patterns: `.git`, `.gitignore`, `legacy_code/`, `legacy_docs/`, `openspec/`, `.opencode/`, `.sisyphus/`, `**/node_modules/`, `*.md`, `deploy/` (not needed inside the build context for backend or frontend builds), `.env*`, `*.log`.
- Do not exclude `backend/` or `frontend/` since the Dockerfiles COPY from those directories.

### 3. Restart policies, resource limits, and logging in docker-compose

**Approach:** Add `restart: unless-stopped` to all four services. Add `deploy.resources.limits.memory` and `deploy.resources.reservations.memory` to each service. Add a `logging` block with the `json-file` driver and `max-size`/`max-file` constraints.

**Why:** Without restart policies, a crashed container stays down until a human intervenes. Without memory limits, a single leaky container can starve the host and the other services. Without log rotation, Docker logs grow unbounded on disk.

**Alternatives considered:**

- `restart: always` vs `unless-stopped`: `unless-stopped` is preferable because it preserves the ability to stop a service intentionally (`docker compose stop`) without it restarting. `always` would restart even after an explicit stop when the Docker daemon restarts.
- `local` log driver vs `json-file`: `json-file` with size limits is simpler and works with `docker compose logs`. The `local` driver compresses logs, but the operational simplicity of `json-file` is better for a single-host Docker Compose deployment.
- Swarm-mode `deploy` keys: The `deploy.resources` keys are technically Compose Spec (v3) but Docker Compose (the `docker compose` CLI) supports them in non-Swarm mode as of recent versions.

**Implementation details:**

- Memory limits: nginx 128 MB, frontend 64 MB, backend 512 MB, mysql 1024 MB. These are conservative starting points that can be tuned after load testing.
- Log rotation: `max-size: "10m"`, `max-file: "3"` for all services. This caps each service at roughly 30 MB of logs.
- Restart: `unless-stopped` on all services.

### 4. Remove MySQL external port exposure

**Approach:** Delete the `ports` mapping (`"${MI_MYSQL_PORT:-3306}:3306"`) from the `mysql` service in `docker-compose.production.yml`. MySQL remains reachable by other containers on the internal Docker network via the service name `mysql` on port 3306.

**Why:** In production, no external process should connect to MySQL directly. The backend reaches MySQL through the Docker network. Exposing port 3306 to the host is an unnecessary attack surface and violates the principle of least access.

**Alternatives considered:**

- Keep the port mapping but bind to `127.0.0.1`: Better than binding to all interfaces, but still unnecessary in production. Development debugging can use `docker compose exec mysql` or a separate override file.
- Add an `expose` directive: `expose` only documents the port; it doesn't map it to the host. Since MySQL's port 3306 is already implicitly exposed on the internal network by virtue of being a Docker service, no `expose` directive is needed.

**Implementation details:**

- Remove the `ports` block from the `mysql` service entirely.
- If developers need host access to MySQL for debugging, they can use a separate `docker-compose.override.yml` that re-adds the port mapping. This keeps production config clean.

### 5. Non-root user in frontend Dockerfile

**Approach:** Add a `USER nginx` directive to the second stage of `frontend.Dockerfile`, after the `COPY` lines. The `nginx:1.27-alpine` base image already includes the `nginx` user (uid 101), so no `adduser` is needed.

**Why:** Running nginx workers as root means a vulnerability in nginx or a misconfigured path could give an attacker root privileges inside the container. The official nginx Alpine image is designed to work with `USER nginx`.

**Alternatives considered:**

- Creating a custom non-root user (like `appuser` in the backend): Unnecessary. The `nginx` user already exists in the image and is the standard choice.
- `USER 101` (numeric UID): Less readable than `USER nginx`. Both achieve the same thing.

**Implementation details:**

- Add `USER nginx` after the last `COPY` line in the runtime stage.
- Verify that the copied files under `/usr/share/nginx/html` are readable by the `nginx` user. The default permissions from `COPY` (root-owned, world-readable) are sufficient since nginx only needs to read these files.
- The nginx master process normally runs as root to bind to port 80. When `USER nginx` is set in the Dockerfile, nginx cannot bind to privileged ports below 1024. However, the `nginx:1.27-alpine` image's default config already binds to port 80 inside the container, and this works because the image's entrypoint handles the permission. If it doesn't, the listen port can be changed to 8080 and the internal Docker Compose routing adjusted accordingly. Testing will confirm which case applies.

### 6. Nginx security headers, gzip, proxy timeouts, and upload size

**Approach:** Add the following to `production.conf`:

- **Security headers:** `X-Content-Type-Options: nosniff`, `X-Frame-Options: DENY`, `X-XSS-Protection: 1; mode=block`, `Strict-Transport-Security: max-age=63072000` (HSTS, since TLS may be handled upstream), and a `Content-Security-Policy` that restricts to same-origin.
- **Gzip:** Enable `gzip on`, set `gzip_types` to cover `text/css`, `application/javascript`, `application/json`, `text/xml`, `image/svg+xml`.
- **Proxy timeouts:** Add `proxy_connect_timeout 10s`, `proxy_send_timeout 30s`, `proxy_read_timeout 60s` to the `/api/` location block.
- **Upload size:** Add `client_max_body_size 50m` at the server level to accommodate Excel imports and template uploads.

**Why:** Without security headers, the application is vulnerable to clickjacking, MIME sniffing, and other browser-level attacks. Without gzip, large JSON responses and JS bundles transfer uncompressed, wasting bandwidth. Without proxy timeouts, a hung backend connection can stall the nginx worker indefinitely. Without an upload size limit, large uploads hit nginx's default 1 MB limit and fail.

**Alternatives considered:**

- `add_header` in a separate `server` block: Not applicable since there is only one server block.
- Brotli compression: Requires compiling a custom nginx module. Gzip is built-in and sufficient for internal network traffic.
- Per-location upload limits: A single server-level limit is simpler and the application has no location that should reject large uploads.

**Implementation details:**

- Add `add_header` directives inside the `server` block (not inside `location` blocks) so they apply to all responses.
- Gzip should be enabled at the `server` level with `gzip_vary on` and `gzip_min_length 256`.
- The `client_max_body_size 50m` matches the expected upload sizes for Excel files and print templates.
- Proxy timeouts: 10s connect, 30s send, 60s read. The read timeout is generous to accommodate long-running report queries.

### 7. Graceful shutdown in Go backend

**Approach:** Refactor `main.go` to capture `SIGINT` and `SIGTERM` via `signal.NotifyContext`. Pass the resulting context to a new `App.Shutdown(ctx)` method that:

1. Calls `server.Shutdown(ctx)` to drain in-flight HTTP requests.
2. Calls the existing `stopReminderScheduler()` function to stop the scheduler goroutine.
3. Calls `db.Close()` to release MySQL connections.

Change `App.Run()` to accept a context and use `server.ListenAndServe()` in a goroutine, blocking on `<-ctx.Done()` or the server error channel.

**Why:** When Docker stops a container, it sends SIGTERM and waits (default 30 seconds, configurable via `stop_grace_period` in docker-compose) before sending SIGKILL. Without signal handling, the Go process ignores SIGTERM and gets force-killed, potentially interrupting in-flight requests, database transactions, and file writes.

**Alternatives considered:**

- `http.Server.RegisterOnShutdown` callbacks: Useful but not sufficient alone. The main loop needs to know when to exit.
- A separate shutdown goroutine that calls `server.Shutdown()`: More complex than necessary. `signal.NotifyContext` is the idiomatic Go 1.16+ pattern.
- Using `gin.Engine` shutdown hooks: Gin doesn't provide built-in shutdown coordination. The `http.Server` level is the right place.

**Implementation details:**

- In `main.go`: create `ctx, stop := signal.NotifyContext(context.Background(), syscall.SIGINT, syscall.SIGTERM)`, defer `stop()`. Pass `ctx` to a new `App.Run(ctx) error` method.
- In `App.Run(ctx)`: start the HTTP server in a goroutine. Block on `ctx.Done()`. When the context is cancelled, call `a.server.Shutdown(shutdownCtx)` with a fresh context that has a 15-second timeout.
- The `startWorkflowReminderScheduler()` method already returns a `stop()` function and uses a `stopCh` channel. The shutdown sequence calls this function after the HTTP server drain completes.
- Add a `stop_grace_period: 30s` to the backend service in docker-compose to give the Go process time to drain.
- In `router.go`: no changes needed. The `gin.Engine` is wrapped in `*http.Server` which handles the drain.

### 8. Database connection pool configuration

**Approach:** After `sql.Open("mysql", dsn)` in `app.go`, configure the pool:

```go
db.SetMaxOpenConns(25)
db.SetMaxIdleConns(10)
db.SetConnMaxLifetime(5 * time.Minute)
db.SetConnMaxIdleTime(1 * time.Minute)
```

**Why:** The default `database/sql` pool has unlimited open connections and no idle connection recycling. Under load, this can exhaust MySQL's `max_connections` limit. Without `ConnMaxLifetime`, connections stay open indefinitely and may hit MySQL's `wait_timeout` on the server side, causing stale connection errors.

**Alternatives considered:**

- Making pool settings configurable via `DatabaseConfig`: Good practice but adds scope. The config struct doesn't have these fields today, and the hardcoded values are reasonable starting points. Configurability can be added later if needed.
- Using Gorm's connection pool: The codebase doesn't use Gorm. It uses `database/sql` directly. The `SetMaxOpenConns` family is the standard `database/sql` API.
- No pool limits, rely on MySQL defaults: MySQL's default `max_connections` is 151. Without client-side limits, a burst of requests could exhaust this. Setting client-side limits is the right defensive approach.

**Implementation details:**

- `MaxOpenConns(25)`: Limits the total number of concurrent database connections. Combined with the scheduler goroutine's lock-based concurrency control, this leaves headroom for request traffic plus scheduled tasks.
- `MaxIdleConns(10)`: Keeps 10 warm connections ready. This avoids the latency spike of establishing new connections for each request after an idle period.
- `ConnMaxLifetime(5m)`: Recycles connections before MySQL's default `wait_timeout` (typically 8 hours) can stale them. 5 minutes is conservative and ensures connections are fresh.
- `ConnMaxIdleTime(1m)`: Closes idle connections after 1 minute to release resources when traffic drops.
- These settings go in `app.go`'s `New()` function, immediately after `sql.Open` and before the first use of `db`.

### 9. DB connectivity check in health endpoint

**Approach:** Change `HealthHandler` to accept `*sql.DB` in addition to `*config.Config`. In the `Get` handler, call `db.PingContext(c.Request.Context())` with a 2-second timeout. Return `503 Service Unavailable` with `{"status": "degraded"}` if the ping fails, otherwise return the existing `200 OK` with `{"status": "ok"}`.

**Why:** The Docker health check (`wget ... /healthz`) currently reports healthy even when MySQL is down. This means Docker won't restart the backend when the database is unreachable, and nginx won't stop routing traffic to a degraded instance. A real connectivity check makes the health endpoint honest.

**Alternatives considered:**

- A separate `/healthz` for liveness and `/readyz` for readiness: More granular but adds complexity. A single endpoint that checks downstream connectivity is sufficient for Docker Compose's health check model, which doesn't distinguish liveness from readiness.
- Checking migrator status or running a test query: Overkill. `db.PingContext()` verifies the connection is alive without executing application logic.
- Returning detailed error messages in the health response: Security risk. If the endpoint is ever exposed (even through a misconfigured proxy), error details could leak internal state. Return only `{"status": "degraded"}` on failure.

**Implementation details:**

- Update `NewHealthHandler` signature to `NewHealthHandler(cfg *config.Config, db *sql.DB)`.
- In `Get`: create a 2-second context via `context.WithTimeout(c.Request.Context(), 2*time.Second)`, call `db.PingContext(ctx)`, cancel the context.
- If ping fails, return `503` with `{"status": "degraded", "service": ..., "environment": ...}` and log the error at warn level.
- Update `router.go` to pass `db` to `NewHealthHandler`.
- The existing `/health` and `/healthz` routes both point to the same handler, so both endpoints gain the check automatically.

## Risks / Trade-offs

**Chromium image size.** Adding `chromium` to the Alpine runtime adds roughly 200-250 MB to the backend image. This is unavoidable if PDF generation must work inside the container. The trade-off is acceptable because the alternative (switching to a Debian base for Google Chrome) would add even more size, and the alternative of not supporting PDF generation is a hard blocker.

**Nginx non-root port binding.** Setting `USER nginx` may prevent nginx from binding to port 80 (a privileged port). The official `nginx:1.27-alpine` image has internal handling for this in its default entrypoint, but it needs verification during implementation. If it fails, the fix is to listen on port 8080 inside the container and adjust the docker-compose port mapping or the internal routing.

**Hardcoded connection pool values.** The pool settings (25 open, 10 idle, 5 min lifetime) are hardcoded rather than configurable. This is a deliberate trade-off to keep the change small. If the deployment needs tuning under production load, these values can be externalized to the config file in a follow-up change.

**Health endpoint latency.** Adding `db.PingContext` to every `/healthz` call introduces a database round-trip on each health check. With a 10-second health check interval and a 2-second ping timeout, this is at most one query every 10 seconds. MySQL can handle this trivially. The risk is that under extreme database load, the ping might fail and cause Docker to mark the backend as unhealthy, triggering a restart loop. The 2-second timeout and 5 retries in the health check configuration provide enough buffer to avoid this.

**Graceful shutdown timeout race.** If the Go server takes longer than the Docker `stop_grace_period` (30 seconds) to drain, Docker sends SIGKILL and the process dies ungracefully anyway. The 15-second internal shutdown timeout is well within the 30-second grace period, so the server drain should complete first. The remaining 15 seconds is buffer for the scheduler stop and database close.

**No TLS in scope.** This design does not add TLS termination at nginx. If the deployment sits behind an external load balancer or reverse proxy that handles TLS, this is fine. If nginx is the edge, the HSTS header is premature (it will be ignored on plain HTTP). This is a low-risk trade-off since HSTS on HTTP is a no-op, and the header will take effect whenever TLS is eventually added upstream.
