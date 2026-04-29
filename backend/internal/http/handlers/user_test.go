package handlers

import (
	"bytes"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gin-gonic/gin"
)

// --- Get ---

func TestUserGetRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/users/bad-id", nil)

	handler.Get(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

// --- Create ---

func TestUserCreateRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserCreateRejectsMissingRequiredFields(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users", bytes.NewBufferString(`{}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserCreateRejectsShortUsername(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users", bytes.NewBufferString(`{
		"username":"a",
		"display_name":"Test User",
		"password":"password123",
		"department_id":1
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserCreateRejectsShortPassword(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users", bytes.NewBufferString(`{
		"username":"testuser",
		"display_name":"Test User",
		"password":"abc",
		"department_id":1
	}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Create(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

// --- Update ---

func TestUserUpdateRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/bad-id", bytes.NewBufferString(`{"display_name":"X"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Update(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserUpdateRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/1", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Update(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserUpdateRejectsInvalidStatus(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/1", bytes.NewBufferString(`{"status":"not-a-status"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.Update(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

// --- ResetPassword ---

func TestUserResetPasswordRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users/bad-id/reset-password", bytes.NewBufferString(`{"new_password":"newpass123"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ResetPassword(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserResetPasswordRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users/1/reset-password", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ResetPassword(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserResetPasswordRejectsShortPassword(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/users/1/reset-password", bytes.NewBufferString(`{"new_password":"abc"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.ResetPassword(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

// --- SetRoles ---

func TestUserSetRolesRejectsInvalidID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "bad-id"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/bad-id/roles", bytes.NewBufferString(`{"role_ids":[1],"department_id":1}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SetRoles(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserSetRolesRejectsInvalidJSON(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/1/roles", bytes.NewBufferString(`{invalid}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SetRoles(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserSetRolesRejectsMissingRoleIDs(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/1/roles", bytes.NewBufferString(`{"department_id":1}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SetRoles(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}

func TestUserSetRolesRejectsMissingDepartmentID(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewUserHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "1"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/users/1/roles", bytes.NewBufferString(`{"role_ids":[1]}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.SetRoles(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d", recorder.Code)
	}
}
