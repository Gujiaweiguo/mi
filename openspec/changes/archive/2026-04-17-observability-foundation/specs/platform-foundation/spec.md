## ADDED Requirements

### Requirement: The backend SHALL emit structured JSON logs for every HTTP request
The backend SHALL use a custom Gin middleware backed by the existing Zap logger to emit one structured log line per completed HTTP request. Each log line SHALL include the HTTP method, request path, response status code, request latency in milliseconds, client IP, and request ID. When the request is authenticated, the log line SHALL also include the authenticated user ID. The middleware SHALL replace Gin's default `gin.Logger()`.

#### Scenario: Successful request produces structured log
- **WHEN** a GET request to `/api/leases` completes with status 200 in 45ms
- **THEN** the backend SHALL emit a single Zap info-level log line containing `method=GET`, `path=/api/leases`, `status=200`, `latency_ms=45`, `client_ip`, and `request_id` as structured fields

#### Scenario: Authenticated request includes user ID
- **WHEN** an authenticated request completes
- **THEN** the structured log line SHALL include a `user_id` field with the authenticated user's ID

#### Scenario: Failed request produces structured log
- **WHEN** a request completes with status 4xx or 5xx
- **THEN** the backend SHALL emit a Zap warn-level log line with the same structured fields

### Requirement: Every HTTP request SHALL carry a unique request ID
The backend SHALL use a request-ID middleware that checks for an incoming `X-Request-ID` header. If present, the middleware SHALL use that value. If absent, the middleware SHALL generate a new UUID v4. The request ID SHALL be stored in the Gin context and SHALL be set as the `X-Request-ID` response header. The request ID SHALL be included in all structured log entries for the request.

#### Scenario: Request without X-Request-ID header
- **WHEN** an incoming request does not include an `X-Request-ID` header
- **THEN** the middleware SHALL generate a new UUID v4, store it in the Gin context, and set it on the `X-Request-ID` response header

#### Scenario: Request with existing X-Request-ID header
- **WHEN** an incoming request includes an `X-Request-ID` header (e.g., from nginx or a load balancer)
- **THEN** the middleware SHALL use that value, store it in the Gin context, and pass it through on the response header

### Requirement: Handler panics SHALL be logged as structured errors
The backend SHALL use a custom recovery middleware backed by Zap that catches panics in handler functions. The middleware SHALL log the panic value and stack trace as a Zap error-level log line with the request ID, method, path, and client IP as structured fields. The middleware SHALL then return HTTP 500 with a generic JSON error body. The middleware SHALL replace Gin's default `gin.Recovery()`.

#### Scenario: Handler panic produces structured error log
- **WHEN** a handler function panics during request processing
- **THEN** the middleware SHALL log an error-level entry with `request_id`, `method`, `path`, `client_ip`, `error` (panic value), and `stack` (stack trace), and SHALL return HTTP 500 with `{"message": "internal server error"}`

### Requirement: The frontend SHALL capture unhandled Vue errors
The Vue 3 application SHALL register a global error handler via `app.config.errorHandler` that logs unhandled component errors to the browser console. The log output SHALL include the error, the component instance that caused it, and the error info object.

#### Scenario: Unhandled component error is logged
- **WHEN** a Vue component throws an unhandled error during rendering or lifecycle
- **THEN** the global error handler SHALL log the error, component instance, and info to `console.error`
