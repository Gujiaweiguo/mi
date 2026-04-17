package middleware

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

func TestRequestIDGeneratesUUIDWhenHeaderMissing(t *testing.T) {
	gin.SetMode(gin.TestMode)

	router := gin.New()
	router.Use(RequestID())
	router.GET("/health", func(c *gin.Context) {
		value, exists := c.Get(RequestIDKey)
		if !exists {
			t.Fatal("expected request id in context")
		}

		rid, ok := value.(string)
		if !ok {
			t.Fatalf("expected request id string, got %T", value)
		}

		parsed, err := uuid.Parse(rid)
		if err != nil {
			t.Fatalf("expected valid uuid, got %q: %v", rid, err)
		}
		if parsed.Version() != 4 {
			t.Fatalf("expected uuid v4, got v%d", parsed.Version())
		}

		c.Status(http.StatusNoContent)
	})

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/health", nil)

	router.ServeHTTP(recorder, request)

	if recorder.Code != http.StatusNoContent {
		t.Fatalf("expected 204, got %d", recorder.Code)
	}

	responseID := recorder.Header().Get(RequestIDHeader)
	if responseID == "" {
		t.Fatal("expected response request id header")
	}

	parsed, err := uuid.Parse(responseID)
	if err != nil {
		t.Fatalf("expected valid uuid in response header, got %q: %v", responseID, err)
	}
	if parsed.Version() != 4 {
		t.Fatalf("expected response uuid v4, got v%d", parsed.Version())
	}
}

func TestRequestIDPreservesIncomingHeader(t *testing.T) {
	gin.SetMode(gin.TestMode)

	const requestID = "req-123"

	router := gin.New()
	router.Use(RequestID())
	router.GET("/health", func(c *gin.Context) {
		value, exists := c.Get(RequestIDKey)
		if !exists {
			t.Fatal("expected request id in context")
		}

		rid, ok := value.(string)
		if !ok {
			t.Fatalf("expected request id string, got %T", value)
		}
		if rid != requestID {
			t.Fatalf("expected context request id %q, got %q", requestID, rid)
		}

		c.Status(http.StatusNoContent)
	})

	recorder := httptest.NewRecorder()
	request := httptest.NewRequest(http.MethodGet, "/health", nil)
	request.Header.Set(RequestIDHeader, requestID)

	router.ServeHTTP(recorder, request)

	if recorder.Code != http.StatusNoContent {
		t.Fatalf("expected 204, got %d", recorder.Code)
	}
	if got := recorder.Header().Get(RequestIDHeader); got != requestID {
		t.Fatalf("expected response request id %q, got %q", requestID, got)
	}
}
