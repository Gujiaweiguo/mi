package handlers

import (
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
)

func TestNewOrgHandler(t *testing.T) {
	repository := auth.NewRepository(nil)
	handler := NewOrgHandler(repository)
	if handler == nil {
		t.Fatal("expected org handler instance")
	}
}
