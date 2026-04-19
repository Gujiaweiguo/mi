package workflow

import (
	"testing"
	"time"
)

func TestSortNodes(t *testing.T) {
	nodes := []Node{
		{ID: 3, StepOrder: 2, Name: "Third"},
		{ID: 1, StepOrder: 0, Name: "First"},
		{ID: 2, StepOrder: 1, Name: "Second"},
	}
	sorted := sortNodes(nodes)
	if sorted[0].Name != "First" {
		t.Fatalf("expected first node, got %s", sorted[0].Name)
	}
	if sorted[1].Name != "Second" {
		t.Fatalf("expected second node, got %s", sorted[1].Name)
	}
	if sorted[2].Name != "Third" {
		t.Fatalf("expected third node, got %s", sorted[2].Name)
	}
}

func TestSortNodesDoesNotMutateOriginal(t *testing.T) {
	nodes := []Node{
		{ID: 3, StepOrder: 2},
		{ID: 1, StepOrder: 0},
	}
	sortNodes(nodes)
	if nodes[0].ID != 3 {
		t.Fatal("sortNodes should not mutate the original slice")
	}
}

func TestSortNodesBreaksTiesByID(t *testing.T) {
	nodes := []Node{
		{ID: 30, StepOrder: 1},
		{ID: 10, StepOrder: 1},
		{ID: 20, StepOrder: 1},
	}
	sorted := sortNodes(nodes)
	if sorted[0].ID != 10 || sorted[1].ID != 20 || sorted[2].ID != 30 {
		t.Fatalf("expected IDs ordered 10,20,30 for same step_order")
	}
}

func TestIndexNodes(t *testing.T) {
	nodes := []Node{
		{ID: 1, Name: "A"},
		{ID: 2, Name: "B"},
		{ID: 3, Name: "C"},
	}
	indexed := indexNodes(nodes)
	if len(indexed) != 3 {
		t.Fatalf("expected 3 nodes, got %d", len(indexed))
	}
	if indexed[1].Name != "A" {
		t.Fatalf("expected node 1 = A, got %s", indexed[1].Name)
	}
	if indexed[3].Name != "C" {
		t.Fatalf("expected node 3 = C, got %s", indexed[3].Name)
	}
}

func TestNextTransition(t *testing.T) {
	fromNode1 := int64(1)
	fromNode2 := int64(2)
	transitions := []Transition{
		{ID: 1, FromNodeID: &fromNode1, ToNodeID: 2, Action: ActionApprove},
		{ID: 2, FromNodeID: &fromNode2, ToNodeID: 3, Action: ActionApprove},
		{ID: 3, FromNodeID: &fromNode1, ToNodeID: 99, Action: ActionReject},
	}

	t.Run("finds approve transition from node 1", func(t *testing.T) {
		tr := nextTransition(transitions, 1, ActionApprove)
		if tr == nil {
			t.Fatal("expected transition, got nil")
		}
		if tr.ToNodeID != 2 {
			t.Fatalf("expected to_node_id 2, got %d", tr.ToNodeID)
		}
	})

	t.Run("finds reject transition from node 1", func(t *testing.T) {
		tr := nextTransition(transitions, 1, ActionReject)
		if tr == nil {
			t.Fatal("expected transition, got nil")
		}
		if tr.ToNodeID != 99 {
			t.Fatalf("expected to_node_id 99, got %d", tr.ToNodeID)
		}
	})

	t.Run("returns nil for unknown node", func(t *testing.T) {
		tr := nextTransition(transitions, 999, ActionApprove)
		if tr != nil {
			t.Fatal("expected nil for unknown node")
		}
	})

	t.Run("returns nil for unknown action", func(t *testing.T) {
		tr := nextTransition(transitions, 1, ActionSubmit)
		if tr != nil {
			t.Fatal("expected nil for unknown action")
		}
	})
}

func TestStringPointer(t *testing.T) {
	t.Run("returns nil for empty string", func(t *testing.T) {
		if p := stringPointer(""); p != nil {
			t.Fatalf("expected nil, got %v", *p)
		}
	})

	t.Run("returns nil for whitespace-only string", func(t *testing.T) {
		if p := stringPointer("   "); p != nil {
			t.Fatalf("expected nil, got %v", *p)
		}
	})

	t.Run("returns trimmed pointer for valid string", func(t *testing.T) {
		p := stringPointer("  hello  ")
		if p == nil {
			t.Fatal("expected non-nil pointer")
		}
		if *p != "hello" {
			t.Fatalf("expected 'hello', got '%s'", *p)
		}
	})
}

func TestNormalizeReminderConfig(t *testing.T) {
	t.Run("fills defaults for empty config", func(t *testing.T) {
		cfg := normalizeReminderConfig(ReminderConfig{})
		if cfg.ReminderType != "standard" {
			t.Fatalf("expected 'standard', got '%s'", cfg.ReminderType)
		}
		if cfg.WindowTruncation != 24*time.Hour {
			t.Fatalf("expected 24h, got %v", cfg.WindowTruncation)
		}
		if cfg.MinPendingAge != 0 {
			t.Fatalf("expected 0, got %v", cfg.MinPendingAge)
		}
	})

	t.Run("preserves valid values", func(t *testing.T) {
		cfg := normalizeReminderConfig(ReminderConfig{
			ReminderType:     "urgent",
			MinPendingAge:    2 * time.Hour,
			WindowTruncation: time.Hour,
		})
		if cfg.ReminderType != "urgent" {
			t.Fatalf("expected 'urgent', got '%s'", cfg.ReminderType)
		}
		if cfg.MinPendingAge != 2*time.Hour {
			t.Fatalf("expected 2h, got %v", cfg.MinPendingAge)
		}
	})

	t.Run("clamps negative min pending age to zero", func(t *testing.T) {
		cfg := normalizeReminderConfig(ReminderConfig{MinPendingAge: -1 * time.Hour})
		if cfg.MinPendingAge != 0 {
			t.Fatalf("expected 0, got %v", cfg.MinPendingAge)
		}
	})

	t.Run("defaults non-positive window truncation", func(t *testing.T) {
		cfg := normalizeReminderConfig(ReminderConfig{WindowTruncation: -5 * time.Second})
		if cfg.WindowTruncation != 24*time.Hour {
			t.Fatalf("expected default 24h, got %v", cfg.WindowTruncation)
		}
	})
}

func TestReminderAuditKey(t *testing.T) {
	ts := time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC)
	key := reminderAuditKey(42, "standard", ts)
	expected := "reminder:42:standard:2026-04-01T09:00:00Z"
	if key != expected {
		t.Fatalf("expected %s, got %s", expected, key)
	}
}

func TestOutboxKey(t *testing.T) {
	key := outboxKey(123, ActionApprove, "abc-456")
	expected := "workflow:123:approve:abc-456"
	if key != expected {
		t.Fatalf("expected %s, got %s", expected, key)
	}
}

func TestReminderReasonPointer(t *testing.T) {
	p := reminderReasonPointer(ReminderReasonNotDue)
	if p == nil {
		t.Fatal("expected non-nil pointer")
	}
	if *p != "not_due" {
		t.Fatalf("expected 'not_due', got '%s'", *p)
	}
}

func TestMarshalOutboxPayload(t *testing.T) {
	instance := &Instance{
		ID:           1,
		DocumentType: "bill",
		DocumentID:   100,
		Status:       InstanceStatusPending,
	}

	t.Run("marshals without comment", func(t *testing.T) {
		payload, err := MarshalOutboxPayload(instance, ActionSubmit, "")
		if err != nil {
			t.Fatalf("unexpected error: %v", err)
		}
		if payload == "" {
			t.Fatal("expected non-empty payload")
		}
	})

	t.Run("marshals with comment", func(t *testing.T) {
		payload, err := MarshalOutboxPayload(instance, ActionApprove, "looks good")
		if err != nil {
			t.Fatalf("unexpected error: %v", err)
		}
		if payload == "" {
			t.Fatal("expected non-empty payload")
		}
	})
}
