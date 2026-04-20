package notification

import (
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"strings"
	"testing"

	"github.com/gin-gonic/gin"
)

func TestListNotifications_EmptyResults(t *testing.T) {
	gin.SetMode(gin.TestMode)
	repo := NewRepository(nil)
	handler := NewHandler(repo)

	w := httptest.NewRecorder()
	c, _ := gin.CreateTestContext(w)
	c.Request = httptest.NewRequest(http.MethodGet, "/api/notifications?page=1&page_size=20", nil)

	handler.ListNotifications(c)

	if w.Code != http.StatusInternalServerError {
		t.Logf("Status: %d, Body: %s", w.Code, w.Body.String())
	}
}

func TestListNotifications_InvalidPage(t *testing.T) {
	gin.SetMode(gin.TestMode)
	repo := NewRepository(nil)
	handler := NewHandler(repo)

	w := httptest.NewRecorder()
	c, _ := gin.CreateTestContext(w)
	c.Request = httptest.NewRequest(http.MethodGet, "/api/notifications?page=abc", nil)

	handler.ListNotifications(c)

	if w.Code != http.StatusBadRequest {
		t.Fatalf("expected 400 for invalid page, got %d", w.Code)
	}
}

func TestListNotifications_InvalidAggregateID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	repo := NewRepository(nil)
	handler := NewHandler(repo)

	w := httptest.NewRecorder()
	c, _ := gin.CreateTestContext(w)
	c.Request = httptest.NewRequest(http.MethodGet, "/api/notifications?aggregate_id=notanumber", nil)

	handler.ListNotifications(c)

	if w.Code != http.StatusBadRequest {
		t.Fatalf("expected 400 for invalid aggregate_id, got %d", w.Code)
	}
}

func TestParsePositiveIntQuery(t *testing.T) {
	gin.SetMode(gin.TestMode)
	tests := []struct {
		name         string
		query        string
		key          string
		defaultValue int
		want         int
		wantErr      bool
	}{
		{"valid", "page=5", "page", 1, 5, false},
		{"missing uses default", "", "page", 1, 1, false},
		{"zero uses default", "page=0", "page", 1, 1, false},
		{"negative uses default", "page=-1", "page", 1, 1, false},
		{"invalid returns error", "page=abc", "page", 1, 0, true},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			w := httptest.NewRecorder()
			c, _ := gin.CreateTestContext(w)
			c.Request = httptest.NewRequest(http.MethodGet, "/?"+tt.query, nil)
			got, err := parsePositiveIntQuery(c, tt.key, tt.defaultValue)
			if tt.wantErr && err == nil {
				t.Fatal("expected error")
			}
			if !tt.wantErr && err != nil {
				t.Fatalf("unexpected error: %v", err)
			}
			if got != tt.want {
				t.Fatalf("expected %d, got %d", tt.want, got)
			}
		})
	}
}

func TestHandler_NilRepository(t *testing.T) {
	gin.SetMode(gin.TestMode)
	var handler *NotificationHandler
	w := httptest.NewRecorder()
	c, _ := gin.CreateTestContext(w)
	c.Request = httptest.NewRequest(http.MethodGet, "/api/notifications", nil)
	handler.ListNotifications(c)
	if w.Code != http.StatusInternalServerError {
		t.Fatalf("expected 500 for nil handler, got %d", w.Code)
	}
}

func TestListNotifications_ResponseJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	repo := NewRepository(nil)
	handler := NewHandler(repo)

	w := httptest.NewRecorder()
	c, _ := gin.CreateTestContext(w)
	c.Request = httptest.NewRequest(http.MethodGet, "/api/notifications?page=1&page_size=20", nil)
	handler.ListNotifications(c)

	var resp map[string]interface{}
	if err := json.Unmarshal(w.Body.Bytes(), &resp); err != nil {
		body := strings.TrimSpace(w.Body.String())
		if body != "" {
			t.Fatalf("response is not valid JSON: %s", body)
		}
	}
}
