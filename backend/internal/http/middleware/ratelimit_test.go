package middleware

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestGetLimiterReturnsNonNil(t *testing.T) {
	limiter := getLimiter("1.2.3.4")
	if limiter == nil {
		t.Fatal("expected non-nil limiter")
	}
}

func TestGetLimiterReturnsSameForSameIP(t *testing.T) {
	a := getLimiter("10.0.0.1")
	b := getLimiter("10.0.0.1")
	if a != b {
		t.Fatal("expected same limiter instance for the same IP")
	}
}

func TestRateLimitMiddlewareAllowsRequest(t *testing.T) {
	gin.SetMode(gin.TestMode)

	called := false
	router := gin.New()
	router.Use(RateLimitMiddleware())
	router.GET("/ping", func(c *gin.Context) {
		called = true
		c.Status(http.StatusOK)
	})

	w := httptest.NewRecorder()
	req := httptest.NewRequest(http.MethodGet, "/ping", nil)
	router.ServeHTTP(w, req)

	if !called {
		t.Fatal("expected handler to be called")
	}
	if w.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d", w.Code)
	}
}

func TestRateLimitMiddlewareRejectsOnLimitExceeded(t *testing.T) {
	gin.SetMode(gin.TestMode)

	// Use a unique IP to get an isolated limiter, then exhaust it.
	ip := "255.255.255.250"
	limiter := getLimiter(ip)

	// Exhaust the burst (200 tokens) plus one extra to trigger rejection.
	for i := 0; i < 201; i++ {
		limiter.Allow()
	}

	called := false
	router := gin.New()
	router.Use(RateLimitMiddleware())
	router.GET("/ping", func(c *gin.Context) {
		called = true
		c.Status(http.StatusOK)
	})

	w := httptest.NewRecorder()
	req := httptest.NewRequest(http.MethodGet, "/ping", nil)
	// Force ClientIP to return our chosen IP.
	req.Header.Set("X-Forwarded-For", ip)
	router.ServeHTTP(w, req)

	if called {
		t.Fatal("expected handler NOT to be called on rate limit")
	}
	if w.Code != http.StatusTooManyRequests {
		t.Fatalf("expected 429, got %d", w.Code)
	}
}
