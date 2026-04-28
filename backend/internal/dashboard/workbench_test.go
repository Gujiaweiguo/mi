package dashboard

import (
	"encoding/json"
	"testing"
)

func TestWorkbenchAggregateJSON_StableShape(t *testing.T) {
	aggregate := WorkbenchAggregate{}

	data, err := json.Marshal(aggregate)
	if err != nil {
		t.Fatalf("unexpected marshal error: %v", err)
	}

	var decoded map[string]any
	if err := json.Unmarshal(data, &decoded); err != nil {
		t.Fatalf("unexpected unmarshal error: %v", err)
	}

	for _, field := range []string{"pending_approvals", "receivables", "overdue_receivables", "active_leases"} {
		if _, ok := decoded[field]; !ok {
			t.Fatalf("missing workbench field %q in output: %s", field, string(data))
		}
	}
}

func TestWorkbenchAggregateJSON_PreviewRows(t *testing.T) {
	aggregate := WorkbenchAggregate{
		PendingApprovals: WorkbenchQueueSection{
			Count:       2,
			RouteTarget: "/workflow/admin",
			PreviewRows: []WorkbenchPreviewRow{{
				ID:          11,
				Title:       "Lease LC-001",
				Subtitle:    "Tenant A",
				Status:      "pending_approval",
				Meta:        "2026-04-29 09:30",
				RouteTarget: "/lease/contracts",
			}},
		},
	}

	data, err := json.Marshal(aggregate)
	if err != nil {
		t.Fatalf("unexpected marshal error: %v", err)
	}

	var decoded WorkbenchAggregate
	if err := json.Unmarshal(data, &decoded); err != nil {
		t.Fatalf("unexpected unmarshal error: %v", err)
	}

	if decoded.PendingApprovals.Count != 2 {
		t.Fatalf("PendingApprovals.Count = %d, want 2", decoded.PendingApprovals.Count)
	}
	if len(decoded.PendingApprovals.PreviewRows) != 1 {
		t.Fatalf("preview row count = %d, want 1", len(decoded.PendingApprovals.PreviewRows))
	}
	if decoded.PendingApprovals.PreviewRows[0].RouteTarget != "/lease/contracts" {
		t.Fatalf("preview route target = %q, want /lease/contracts", decoded.PendingApprovals.PreviewRows[0].RouteTarget)
	}
}
