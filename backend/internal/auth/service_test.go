package auth

import "testing"

func TestCanAllowsExpectedActions(t *testing.T) {
	service := &Service{}
	permissions := []Permission{{
		FunctionCode:    "workflow.admin",
		PermissionLevel: "approve",
		CanPrint:        true,
		CanExport:       true,
	}}

	assertions := map[string]bool{
		"view":    true,
		"edit":    true,
		"approve": true,
		"print":   true,
		"export":  true,
	}

	for action, expected := range assertions {
		if got := service.Can(permissions, "workflow.admin", action); got != expected {
			t.Fatalf("expected action %s to be %t, got %t", action, expected, got)
		}
	}
}

func TestCanRejectsMissingOrLowerPermissions(t *testing.T) {
	service := &Service{}
	permissions := []Permission{{
		FunctionCode:    "lease.contract",
		PermissionLevel: "view",
		CanPrint:        false,
		CanExport:       false,
	}}

	if service.Can(permissions, "billing.invoice", "view") {
		t.Fatal("expected unrelated function permission to be denied")
	}
	if service.Can(permissions, "lease.contract", "approve") {
		t.Fatal("expected approve permission to be denied")
	}
	if service.Can(permissions, "lease.contract", "export") {
		t.Fatal("expected export permission to be denied")
	}
}
