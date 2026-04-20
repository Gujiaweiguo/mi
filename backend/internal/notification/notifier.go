package notification

import (
	"context"
	"database/sql"
)

// Notifier queues a notification event inside an existing transaction.
type Notifier interface {
	Enqueue(ctx context.Context, tx *sql.Tx, event NotificationEvent) error
}

// NoopNotifier silently discards notification enqueue requests.
type NoopNotifier struct{}

// Enqueue implements Notifier with a no-op result.
func (NoopNotifier) Enqueue(_ context.Context, _ *sql.Tx, _ NotificationEvent) error {
	return nil
}

// ServiceNotifier adapts a Service to the Notifier interface.
type ServiceNotifier struct {
	service *Service
}

// NewServiceNotifier wraps a Service as a Notifier.
func NewServiceNotifier(service *Service) *ServiceNotifier {
	return &ServiceNotifier{service: service}
}

// Enqueue delegates to the underlying service when present.
func (n *ServiceNotifier) Enqueue(ctx context.Context, tx *sql.Tx, event NotificationEvent) error {
	if n == nil || n.service == nil {
		return nil
	}
	return n.service.Enqueue(ctx, tx, event)
}
