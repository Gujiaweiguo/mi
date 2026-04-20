package notification

import (
	"context"
	"testing"
)

func TestNoopNotifier_Enqueue(t *testing.T) {
	n := NoopNotifier{}
	err := n.Enqueue(context.Background(), nil, NotificationEvent{})
	if err != nil {
		t.Fatalf("expected nil, got %v", err)
	}
}

func TestServiceNotifier_NilReceiver(t *testing.T) {
	var n *ServiceNotifier
	err := n.Enqueue(context.Background(), nil, NotificationEvent{})
	if err != nil {
		t.Fatalf("expected nil for nil receiver, got %v", err)
	}
}

func TestServiceNotifier_NilService(t *testing.T) {
	n := &ServiceNotifier{service: nil}
	err := n.Enqueue(context.Background(), nil, NotificationEvent{})
	if err != nil {
		t.Fatalf("expected nil for nil service, got %v", err)
	}
}
