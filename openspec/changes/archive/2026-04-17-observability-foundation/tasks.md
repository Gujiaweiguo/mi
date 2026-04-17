## 1. Request ID Middleware

- [x] 1.1 Create `backend/internal/http/middleware/requestid.go` with a `RequestID()` middleware that reads `X-Request-ID` from the incoming header or generates a new UUID v4 using `github.com/google/uuid`, stores it in the Gin context under key `request_id`, and sets the `X-Request-ID` response header
- [x] 1.2 Write unit tests for request ID middleware: verify generation when header absent, pass-through when header present, response header is set

## 2. Structured Request Logging Middleware

- [x] 2.1 Create `backend/internal/http/middleware/logging.go` with a `StructuredLogger(logger *zap.Logger)` middleware that logs method, path, status, latency_ms, client_ip, request_id, and (when available) user_id after each request completes
- [x] 2.2 Write unit tests for structured logging middleware: verify log fields for successful request, failed request, and authenticated request

## 3. Structured Recovery Middleware

- [x] 3.1 Create `backend/internal/http/middleware/recovery.go` with a `StructuredRecovery(logger *zap.Logger)` middleware that recovers panics, logs error-level with request_id, method, path, client_ip, panic value, and stack trace, then returns HTTP 500 with `{"message": "internal server error"}`
- [x] 3.2 Write unit tests for structured recovery middleware: verify panic is caught, log entry is produced, response is 500 with JSON body

## 4. Router Integration

- [x] 4.1 Update `NewRouter` signature in `backend/internal/http/router.go` to accept `logger *zap.Logger` as parameter, replace `gin.Logger()` and `gin.Recovery()` with `middleware.RequestID()`, `middleware.StructuredLogger(logger)`, and `middleware.StructuredRecovery(logger)`
- [x] 4.2 Update `backend/internal/app/app.go` to pass `a.logger` to `api.NewRouter(cfg, db, a.logger)`
- [x] 4.3 Verify `go build ./...` succeeds from `backend/`

## 5. Frontend Global Error Handler

- [x] 5.1 Add `app.config.errorHandler` in `frontend/src/main.ts` that logs unhandled Vue errors via `console.error` with error, component instance, and info

## 6. Verification

- [x] 6.1 Run `go test ./internal/http/middleware/...` — all middleware tests pass
- [x] 6.2 Run `go build ./...` from `backend/` — compiles cleanly
- [x] 6.3 Run `vue-tsc --noEmit` from `frontend/` — typechecks cleanly
