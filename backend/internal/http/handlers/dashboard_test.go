package handlers

import (
	"testing"
)

func TestNewDashboardHandler(t *testing.T) {
	handler := NewDashboardHandler(nil)
	if handler == nil {
		t.Fatal("expected handler instance")
	}
}
