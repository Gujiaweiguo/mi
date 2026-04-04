package middleware

import (
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

func TestRequirePermissionRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	service := &auth.Service{}

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/org/departments", nil)

	RequirePermission("workflow.admin", "view", service)(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d", recorder.Code)
	}
}

func TestRequirePermissionRejectsForbiddenAction(t *testing.T) {
	gin.SetMode(gin.TestMode)
	service := &auth.Service{}

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/org/departments", nil)
	ctx.Set(sessionUserContextKey, &auth.SessionUser{
		Permissions: []auth.Permission{{
			FunctionCode:    "workflow.admin",
			PermissionLevel: "view",
			CanPrint:        false,
			CanExport:       false,
		}},
	})

	RequirePermission("workflow.admin", "export", service)(ctx)

	if recorder.Code != http.StatusForbidden {
		t.Fatalf("expected 403, got %d", recorder.Code)
	}
}
