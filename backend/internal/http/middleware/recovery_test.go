package middleware

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
	"go.uber.org/zap/zapcore"
)

func TestStructuredRecoveryReturnsInternalServerErrorOnPanic(t *testing.T) {
	gin.SetMode(gin.TestMode)
	logger, logs := observedLogger(zapcore.DebugLevel)

	router := gin.New()
	router.Use(RequestID(), StructuredRecovery(logger))
	router.GET("/panic", func(c *gin.Context) {
		panic("boom")
	})

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/panic", nil)
	request.Header.Set(RequestIDHeader, "req-panic")

	router.ServeHTTP(recorder, request)

	if recorder.Code != http.StatusInternalServerError {
		t.Fatalf("expected 500, got %d", recorder.Code)
	}
	if body := recorder.Body.String(); body != "{\"message\":\"internal server error\"}" {
		t.Fatalf("expected internal server error body, got %q", body)
	}

	entries := logs.All()
	if len(entries) != 1 {
		t.Fatalf("expected 1 log entry, got %d", len(entries))
	}

	entry := entries[0]
	if entry.Level != zapcore.ErrorLevel {
		t.Fatalf("expected error log level, got %s", entry.Level)
	}

	ctx := entry.ContextMap()
	assertLogField(t, ctx, "method", http.MethodGet)
	assertLogField(t, ctx, "path", "/panic")
	assertLogField(t, ctx, "client_ip", "192.0.2.1")
	assertLogField(t, ctx, "request_id", "req-panic")
	assertLogField(t, ctx, "error", "boom")

	stack, ok := ctx["stack"].(string)
	if !ok {
		t.Fatalf("expected stack string, got %T", ctx["stack"])
	}
	if stack == "" {
		t.Fatal("expected stack trace in log entry")
	}
}
