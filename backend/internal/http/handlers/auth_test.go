package handlers

import (
	"bytes"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
	"github.com/golang-jwt/jwt/v5"
)

func TestLoginRejectsInvalidPayload(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewAuthHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/auth/login", bytes.NewBufferString(`{"username":""}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Login(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestAuthMeHandler(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewAuthHandler(nil)

	token, err := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
		"sub":           101,
		"username":      "admin",
		"department_id": 101,
		"roles":         []string{"admin"},
		"exp":           time.Now().Add(time.Hour).Unix(),
	}).SignedString([]byte("test-secret"))
	if err != nil {
		t.Fatalf("sign token: %v", err)
	}

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/auth/me", nil)
	ctx.Request.Header.Set("Authorization", "Bearer "+token)
	ctx.Set("session_user", &auth.SessionUser{
		ID:           101,
		Username:     "admin",
		DisplayName:  "System Administrator",
		DepartmentID: 101,
		Roles:        []string{"admin"},
		Permissions: []auth.Permission{{
			FunctionCode:    "workflow.admin",
			PermissionLevel: "approve",
			CanPrint:        true,
			CanExport:       true,
		}},
	})

	handler.Me(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}

	var resp struct {
		User struct {
			ID           int64             `json:"id"`
			Username     string            `json:"username"`
			DisplayName  string            `json:"display_name"`
			DepartmentID int64             `json:"department_id"`
			Roles        []string          `json:"roles"`
			Permissions  []auth.Permission `json:"permissions"`
		} `json:"user"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if resp.User.Username != "admin" || resp.User.DisplayName != "System Administrator" || resp.User.DepartmentID != 101 {
		t.Fatalf("unexpected user payload: %+v", resp.User)
	}
	if len(resp.User.Roles) != 1 || resp.User.Roles[0] != "admin" {
		t.Fatalf("unexpected roles: %+v", resp.User.Roles)
	}
	if len(resp.User.Permissions) != 1 || resp.User.Permissions[0].FunctionCode != "workflow.admin" {
		t.Fatalf("unexpected permissions: %+v", resp.User.Permissions)
	}
}
