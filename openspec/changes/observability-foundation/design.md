## Context

The backend already integrates Zap (`go.uber.org/zap v1.27.0`) but uses it only in `app.go` for lifecycle and workflow scheduler events. The HTTP layer uses Gin's default `gin.Logger()` (unstructured stdout text) and `gin.Recovery()` (no structured logging). The `NewRouter` function does not receive a logger, so middleware and handlers have no access to structured logging. The frontend has no global Vue error handler — unhandled component errors are silently swallowed.

## Goals / Non-Goals

**Goals:**
- Every HTTP request produces a single structured JSON log line with method, path, status, latency, client IP, request ID, and (when authenticated) user ID.
- Every request carries a unique request ID propagated via `X-Request-ID` header and accessible from any handler via the Gin context.
- Panics in handlers produce structured log entries (not raw stderr output) with request ID and stack trace.
- Frontend unhandled Vue errors are logged to the browser console for developer visibility.

**Non-Goals:**
- Distributed tracing (OpenTelemetry, Jaeger, etc.).
- Metrics endpoints (Prometheus, `/metrics`).
- Log file output or rotation (stdout + Docker json-file driver is sufficient).
- Structured logging in service/repository layers (handlers only for this change).
- External error tracking (Sentry, Bugsnag).

## Decisions

### Decision 1 — Custom Zap middleware over gin.Logger()

**Choice:** Replace `gin.Logger()` with a custom middleware that calls `logger.Info` after each request completes.

**Rationale:** Gin's default logger writes fixed-format text to stdout (`[GIN] 2026/04/17 - 15:30:00 | 200 | 12ms | GET /api/leases`). This is not parseable by structured log aggregators. A custom middleware can emit Zap's JSON format with consistent field names.

**Alternatives considered:**
- `ginzap` library — rejected to avoid a new dependency for 30 lines of middleware.
- Keeping `gin.Logger()` and adding a separate structured logger — rejected; two log lines per request is confusing.

### Decision 2 — UUID v4 for request IDs

**Choice:** Use `github.com/google/uuid` (already in go.sum via testcontainers) to generate UUID v4 request IDs.

**Rationale:** UUID v4 provides sufficient uniqueness without coordination. The middleware checks for an incoming `X-Request-ID` header first (for load-balancer or nginx propagation) and falls back to generating a new one.

**Alternatives considered:**
- `github.com/rs/xid` — slightly shorter IDs but adds a new dependency.
- Random hex string — no standard library support for cryptographically secure short IDs without manual coding.

### Decision 3 — Recovery middleware with Zap

**Choice:** Replace `gin.Recovery()` with a custom middleware that recovers panics, logs via Zap with request context fields, and returns 500.

**Rationale:** `gin.Recovery()` writes stack traces to os.Stderr without structured fields. The custom version adds request ID, method, path, and client IP to the panic log entry, making it correlatable with the request log line.

### Decision 4 — Logger passed to NewRouter, not stored globally

**Choice:** Pass `*zap.Logger` as a parameter to `NewRouter`.

**Rationale:** Dependency injection is the existing pattern in this codebase (all services are constructed in `NewRouter`). A global logger variable would break testability.

### Decision 5 — Frontend: console.error in errorHandler

**Choice:** Add `app.config.errorHandler` that logs to `console.error` with the component instance, error, and error info.

**Rationale:** This is the standard Vue 3 mechanism for catching unhandled errors. No external library needed. Provides visibility in browser devtools. Can be extended later to post to an error tracking endpoint.

## Risks / Trade-offs

- **UUID import from indirect dependency** → `github.com/google/uuid` is already in go.sum (pulled by testcontainers). If it were ever removed from the dependency tree, we would need to add it directly. Low risk.
- **Logger not yet in service layer** → Services still return errors without logging. This is acceptable for now; a future change can propagate the logger downward. The middleware alone provides significant operational value.
- **Frontend console.error is client-side only** → Errors are only visible to users with devtools open. A future change can add remote error reporting.
