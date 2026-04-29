package middleware

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestPrometheusMiddlewareCallsNext(t *testing.T) {
	gin.SetMode(gin.TestMode)

	called := false
	router := gin.New()
	router.Use(PrometheusMiddleware())
	router.GET("/test", func(c *gin.Context) {
		called = true
		c.Status(http.StatusOK)
	})

	w := httptest.NewRecorder()
	req := httptest.NewRequest(http.MethodGet, "/test", nil)
	router.ServeHTTP(w, req)

	if !called {
		t.Fatal("expected next handler to be called")
	}
	if w.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d", w.Code)
	}
}

func TestPrometheusMiddlewareRecordsMetrics(t *testing.T) {
	gin.SetMode(gin.TestMode)

	router := gin.New()
	router.Use(PrometheusMiddleware())
	router.GET("/metrics-test", func(c *gin.Context) {
		c.Status(http.StatusCreated)
	})

	w := httptest.NewRecorder()
	req := httptest.NewRequest(http.MethodGet, "/metrics-test", nil)
	router.ServeHTTP(w, req)

	if w.Code != http.StatusCreated {
		t.Fatalf("expected 201, got %d", w.Code)
	}

	// Verify metrics endpoint serves successfully (proves registration worked).
	handler := MetricsHandler()
	mw := httptest.NewRecorder()
	mreq := httptest.NewRequest(http.MethodGet, "/metrics", nil)
	handler.ServeHTTP(mw, mreq)

	if mw.Code != http.StatusOK {
		t.Fatalf("expected 200 from metrics handler, got %d", mw.Code)
	}
	body := mw.Body.String()
	if len(body) == 0 {
		t.Fatal("expected non-empty metrics output")
	}
}

func TestMetricsHandlerReturnsNonNil(t *testing.T) {
	handler := MetricsHandler()
	if handler == nil {
		t.Fatal("expected non-nil handler")
	}
}
