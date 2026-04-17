package middleware

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"go.uber.org/zap/zaptest/observer"
)

func TestStructuredLoggerLogsSuccessfulRequest(t *testing.T) {
	gin.SetMode(gin.TestMode)
	logger, logs := observedLogger(zapcore.DebugLevel)

	router := gin.New()
	router.Use(RequestID(), StructuredLogger(logger))
	router.GET("/health", func(c *gin.Context) {
		c.Status(http.StatusOK)
	})

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/health", nil)
	request.Header.Set(RequestIDHeader, "req-success")

	router.ServeHTTP(recorder, request)

	entries := logs.All()
	if len(entries) != 1 {
		t.Fatalf("expected 1 log entry, got %d", len(entries))
	}

	entry := entries[0]
	if entry.Level != zapcore.InfoLevel {
		t.Fatalf("expected info log level, got %s", entry.Level)
	}

	ctx := entry.ContextMap()
	assertLogField(t, ctx, "method", http.MethodGet)
	assertLogField(t, ctx, "path", "/health")
	assertLogField(t, ctx, "status", int64(http.StatusOK))
	assertLogField(t, ctx, "client_ip", "192.0.2.1")
	assertLogField(t, ctx, "request_id", "req-success")

	latency, ok := ctx["latency_ms"].(int64)
	if !ok {
		t.Fatalf("expected latency_ms int64, got %T", ctx["latency_ms"])
	}
	if latency < 0 {
		t.Fatalf("expected non-negative latency, got %d", latency)
	}
}

func TestStructuredLoggerLogsWarnForClientErrors(t *testing.T) {
	gin.SetMode(gin.TestMode)
	logger, logs := observedLogger(zapcore.DebugLevel)

	router := gin.New()
	router.Use(RequestID(), StructuredLogger(logger))
	router.GET("/missing", func(c *gin.Context) {
		c.Status(http.StatusNotFound)
	})

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/missing", nil)
	request.Header.Set(RequestIDHeader, "req-warn")

	router.ServeHTTP(recorder, request)

	entries := logs.All()
	if len(entries) != 1 {
		t.Fatalf("expected 1 log entry, got %d", len(entries))
	}
	if entries[0].Level != zapcore.WarnLevel {
		t.Fatalf("expected warn log level, got %s", entries[0].Level)
	}

	ctx := entries[0].ContextMap()
	assertLogField(t, ctx, "status", int64(http.StatusNotFound))
	assertLogField(t, ctx, "request_id", "req-warn")
}

func TestStructuredLoggerIncludesAuthenticatedUserID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	logger, logs := observedLogger(zapcore.DebugLevel)

	router := gin.New()
	router.Use(RequestID(), StructuredLogger(logger))
	router.GET("/me", func(c *gin.Context) {
		c.Set(sessionUserContextKey, &auth.SessionUser{ID: 42})
		c.Status(http.StatusOK)
	})

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/me", nil)
	request.Header.Set(RequestIDHeader, "req-user")

	router.ServeHTTP(recorder, request)

	entries := logs.All()
	if len(entries) != 1 {
		t.Fatalf("expected 1 log entry, got %d", len(entries))
	}

	ctx := entries[0].ContextMap()
	assertLogField(t, ctx, "user_id", int64(42))
	assertLogField(t, ctx, "request_id", "req-user")
}

func observedLogger(level zapcore.Level) (*zap.Logger, *observer.ObservedLogs) {
	core, logs := observer.New(level)
	return zap.New(core), logs
}

func assertLogField(t *testing.T, fields map[string]any, key string, want any) {
	t.Helper()

	got, ok := fields[key]
	if !ok {
		t.Fatalf("expected log field %q", key)
	}
	if got != want {
		t.Fatalf("expected %s=%v, got %v", key, want, got)
	}
}
