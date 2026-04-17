package handlers

import (
	"context"
	"database/sql"
	"database/sql/driver"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/gin-gonic/gin"
)

// mockConnector implements driver.Connector for testing.
type mockConnector struct {
	err error
}

func (m *mockConnector) Connect(_ context.Context) (driver.Conn, error) {
	if m.err != nil {
		return nil, m.err
	}
	return &mockConn{}, nil
}

func (m *mockConnector) Driver() driver.Driver {
	return &mockDriver{}
}

type mockDriver struct{}

func (d *mockDriver) Open(_ string) (driver.Conn, error) {
	return &mockConn{}, nil
}

type mockConn struct{}

func (c *mockConn) Prepare(_ string) (driver.Stmt, error) { return nil, nil }
func (c *mockConn) Close() error                          { return nil }
func (c *mockConn) Begin() (driver.Tx, error)             { return nil, nil }
func (c *mockConn) Ping(_ context.Context) error          { return nil }

func TestHealthHandlerGet(t *testing.T) {
	gin.SetMode(gin.TestMode)

	db := sql.OpenDB(&mockConnector{})

	handler := NewHealthHandler(&config.Config{
		App: config.AppConfig{
			Name:        "mi-backend",
			Environment: "test",
		},
	}, db)

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
