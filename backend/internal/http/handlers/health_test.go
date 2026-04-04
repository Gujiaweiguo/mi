package handlers

import (
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/gin-gonic/gin"
)

func TestHealthHandlerGet(t *testing.T) {
	gin.SetMode(gin.TestMode)

	handler := NewHealthHandler(&config.Config{
		App: config.AppConfig{
			Name:        "mi-backend",
			Environment: "test",
		},
	})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/health", nil)

	handler.Get(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d", recorder.Code)
	}

	var payload map[string]string
	if err := json.Unmarshal(recorder.Body.Bytes(), &payload); err != nil {
		t.Fatalf("decode response: %v", err)
	}

	if payload["status"] != "ok" {
		t.Fatalf("expected status ok, got %q", payload["status"])
	}

	if payload["environment"] != "test" {
		t.Fatalf("expected environment test, got %q", payload["environment"])
	}
}
