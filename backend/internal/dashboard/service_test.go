package dashboard

import (
	"encoding/json"
	"testing"
)

func TestNewDashboardService_Nil(t *testing.T) {
	svc := NewDashboardService(nil)
	if svc == nil {
		t.Fatal("NewDashboardService(nil) should return non-nil")
	}
}

func TestDashboardSummaryJSON_AllFields(t *testing.T) {
	s := DashboardSummary{
		ActiveLeases:            10,
		PendingLeaseApprovals:   3,
		PendingInvoiceApprovals: 5,
		OpenReceivables:         7,
		OverdueReceivables:      2,
		PendingWorkflows:        4,
	}

	data, err := json.Marshal(s)
	if err != nil {
		t.Fatalf("unexpected marshal error: %v", err)
	}

	var m map[string]interface{}
	if err := json.Unmarshal(data, &m); err != nil {
		t.Fatalf("unexpected unmarshal error: %v", err)
	}

	expectedFields := []string{
		"active_leases",
		"pending_lease_approvals",
		"pending_invoice_approvals",
		"open_receivables",
		"overdue_receivables",
		"pending_workflows",
	}

	for _, field := range expectedFields {
		if _, ok := m[field]; !ok {
			t.Fatalf("missing JSON field %q in output: %s", field, string(data))
		}
	}
}

func TestDashboardSummaryJSON_Values(t *testing.T) {
	s := DashboardSummary{
		ActiveLeases:            42,
		PendingLeaseApprovals:   13,
		PendingInvoiceApprovals: 7,
		OpenReceivables:         20,
		OverdueReceivables:      5,
		PendingWorkflows:        8,
	}

	data, err := json.Marshal(s)
	if err != nil {
		t.Fatalf("unexpected marshal error: %v", err)
	}

	var decoded DashboardSummary
	if err := json.Unmarshal(data, &decoded); err != nil {
		t.Fatalf("unexpected unmarshal error: %v", err)
	}

	if decoded.ActiveLeases != 42 {
		t.Fatalf("ActiveLeases = %d, want 42", decoded.ActiveLeases)
	}
	if decoded.PendingLeaseApprovals != 13 {
		t.Fatalf("PendingLeaseApprovals = %d, want 13", decoded.PendingLeaseApprovals)
	}
	if decoded.PendingInvoiceApprovals != 7 {
		t.Fatalf("PendingInvoiceApprovals = %d, want 7", decoded.PendingInvoiceApprovals)
	}
	if decoded.OpenReceivables != 20 {
		t.Fatalf("OpenReceivables = %d, want 20", decoded.OpenReceivables)
	}
	if decoded.OverdueReceivables != 5 {
		t.Fatalf("OverdueReceivables = %d, want 5", decoded.OverdueReceivables)
	}
	if decoded.PendingWorkflows != 8 {
		t.Fatalf("PendingWorkflows = %d, want 8", decoded.PendingWorkflows)
	}
}

func TestDashboardSummaryJSON_ZeroValues(t *testing.T) {
	s := DashboardSummary{}

	data, err := json.Marshal(s)
	if err != nil {
		t.Fatalf("unexpected marshal error: %v", err)
	}

	var m map[string]interface{}
	if err := json.Unmarshal(data, &m); err != nil {
		t.Fatalf("unexpected unmarshal error: %v", err)
	}

	for _, field := range []string{
		"active_leases", "pending_lease_approvals", "pending_invoice_approvals",
		"open_receivables", "overdue_receivables", "pending_workflows",
	} {
		val, ok := m[field]
		if !ok {
			t.Fatalf("missing field %q", field)
		}
		if f, ok := val.(float64); !ok || f != 0 {
			t.Fatalf("field %q should be 0, got %v", field, val)
		}
	}
}
