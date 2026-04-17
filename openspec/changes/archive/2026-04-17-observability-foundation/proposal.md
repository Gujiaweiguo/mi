## Why

The backend uses Zap structured logging in `app.go` for lifecycle events and the workflow reminder scheduler, but HTTP request logging relies on Gin's default `gin.Logger()` middleware which outputs unstructured text to stdout. There is no request ID propagation, no structured error logging in handlers, and no frontend global error handler. These gaps make production debugging and log aggregation impractical.

## What Changes

- Replace `gin.Logger()` with a custom Zap-backed structured request logging middleware that emits method, path, status, latency, client IP, request ID, and authenticated user ID as structured JSON fields.
- Add a request-ID middleware that reads `X-Request-ID` from incoming headers or generates a UUID, stores it in the Gin context, and sets it on the response header.
- Replace `gin.Recovery()` with a custom recovery middleware that logs panics via Zap with structured fields (including request ID and stack trace) before returning 500.
- Propagate the Zap logger through `NewRouter` so that the middleware layer can access it.
- Add `app.config.errorHandler` in the Vue 3 frontend `main.ts` to catch and log unhandled component errors to the console, enabling browser devtools and future error tracking integration.
- No new dependencies — all changes use existing packages (`zap`, `gin`, `google/uuid` already in go.sum from testcontainers).

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `platform-foundation`: add structured HTTP request logging, request ID propagation, and frontend global error handling as foundational observability for the first release.

## Impact

- `backend/internal/http/router.go`: accept `*zap.Logger` parameter, wire new middleware.
- `backend/internal/http/middleware/logging.go`: new file — Zap-based request logging middleware.
- `backend/internal/http/middleware/requestid.go`: new file — request ID middleware.
- `backend/internal/http/middleware/recovery.go`: new file — Zap-based recovery middleware.
- `backend/internal/app/app.go`: pass logger to `NewRouter`.
- `frontend/src/main.ts`: add `app.config.errorHandler`.
- Zero API contract changes. Zero schema changes.
