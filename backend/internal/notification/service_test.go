package notification

import (
	"context"
	"database/sql"
	"encoding/json"
	"errors"
	"os"
	"path/filepath"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"go.uber.org/zap"
)

type mockRepo struct {
	inserted []*OutboxEntry
	pending  []*OutboxEntry
	updated  []*OutboxEntry
	insertFn func(ctx context.Context, tx *sql.Tx, entry *OutboxEntry) error
}

func (m *mockRepo) InsertOutbox(ctx context.Context, tx *sql.Tx, entry *OutboxEntry) error {
	if m.insertFn != nil {
		return m.insertFn(ctx, tx, entry)
	}
	m.inserted = append(m.inserted, entry)
	return nil
}

func (m *mockRepo) FetchPending(ctx context.Context, db *sql.DB, batchSize int) ([]*OutboxEntry, error) {
	return m.pending, nil
}

func (m *mockRepo) UpdateStatus(ctx context.Context, entry *OutboxEntry) error {
	m.updated = append(m.updated, entry)
	return nil
}

type mockSender struct {
	sendErr error
	calls   []struct{ to, subject, body string }
}

func (m *mockSender) Send(_ context.Context, to, cc []string, subject, body string) error {
	m.calls = append(m.calls, struct{ to, subject, body string }{to[0], subject, body})
	return m.sendErr
}

// newTestService constructs Service directly with mockRepo (bypassing NewServiceWithNowFunc
// which requires *Repository, not the serviceRepository interface our mock implements).
func newTestService(t *testing.T, repo *mockRepo, sender Sender) *Service {
	t.Helper()
	dir := t.TempDir()
	os.WriteFile(filepath.Join(dir, "test_tmpl.html"), []byte(
		`{{define "subject"}}Test Subject{{end}}{{define "body"}}<p>Body</p>{{end}}`,
	), 0644)
	renderer, err := NewRenderer(dir)
	if err != nil {
		t.Fatal(err)
	}
	cfg := config.EmailConfig{MaxRetryAttempts: 3, RetryIntervalSeconds: 60}
	logger := zap.NewNop()
	return &Service{
		repository: repo,
		sender:     sender,
		renderer:   renderer,
		db:         nil,
		config:     cfg,
		logger:     logger,
		now: func() time.Time {
			return time.Date(2026, 1, 1, 0, 0, 0, 0, time.UTC)
		},
	}
}

func TestService_Enqueue_NilTx(t *testing.T) {
	repo := &mockRepo{}
	svc := newTestService(t, repo, &mockSender{})

	event := NotificationEvent{
		EventType:     "test.event",
		AggregateType: "order",
		AggregateID:   42,
		RecipientTo:   []string{"user@example.com"},
		TemplateName:  "test_tmpl",
		TemplateData:  map[string]string{"key": "value"},
	}

	err := svc.Enqueue(context.Background(), nil, event)
	if err == nil {
		t.Fatal("expected error for nil transaction")
	}
	if err.Error() != "enqueue notification: nil transaction" {
		t.Fatalf("unexpected error: %v", err)
	}
}

func TestService_Enqueue_NoRecipients_NilTx(t *testing.T) {
	repo := &mockRepo{}
	svc := newTestService(t, repo, &mockSender{})

	err := svc.Enqueue(context.Background(), nil, NotificationEvent{
		TemplateName: "test_tmpl",
	})
	if err == nil {
		t.Fatal("expected error")
	}
}

func TestService_ProcessOutbox_HappyPath(t *testing.T) {
	repo := &mockRepo{
		pending: []*OutboxEntry{
			{
				ID:            1,
				EventType:     "test.event",
				TemplateName:  "test_tmpl",
				RecipientTo:   "user@example.com",
				Status:        StatusSending,
				TemplateData:  json.RawMessage(`{}`),
				MaxAttempts:   3,
				AttemptCount:  0,
			},
		},
	}
	sender := &mockSender{}
	svc := newTestService(t, repo, sender)

	err := svc.ProcessOutbox(context.Background())
	if err != nil {
		t.Fatalf("ProcessOutbox: %v", err)
	}

	if len(sender.calls) != 1 {
		t.Fatalf("expected 1 send call, got %d", len(sender.calls))
	}
	if len(repo.updated) != 1 {
		t.Fatalf("expected 1 update, got %d", len(repo.updated))
	}
	if repo.updated[0].Status != StatusSent {
		t.Fatalf("expected status sent, got %s", repo.updated[0].Status)
	}
	if repo.updated[0].SentAt == nil {
		t.Fatal("expected sent_at to be set")
	}
}

func TestService_ProcessOutbox_SendFailure(t *testing.T) {
	repo := &mockRepo{
		pending: []*OutboxEntry{
			{
				ID:           1,
				TemplateName: "test_tmpl",
				RecipientTo:  "user@example.com",
				Status:       StatusSending,
				TemplateData: json.RawMessage(`{}`),
				MaxAttempts:  3,
				AttemptCount: 0,
			},
		},
	}
	sender := &mockSender{sendErr: errors.New("SMTP down")}
	svc := newTestService(t, repo, sender)

	err := svc.ProcessOutbox(context.Background())
	if err != nil {
		t.Fatalf("ProcessOutbox: %v", err)
	}

	if len(repo.updated) != 1 {
		t.Fatalf("expected 1 update, got %d", len(repo.updated))
	}
	if repo.updated[0].Status != StatusFailed {
		t.Fatalf("expected status failed, got %s", repo.updated[0].Status)
	}
	if repo.updated[0].AttemptCount != 1 {
		t.Fatalf("expected attempt_count 1, got %d", repo.updated[0].AttemptCount)
	}
}

func TestService_ProcessOutbox_DeadAfterMaxRetries(t *testing.T) {
	repo := &mockRepo{
		pending: []*OutboxEntry{
			{
				ID:           1,
				TemplateName: "test_tmpl",
				RecipientTo:  "user@example.com",
				Status:       StatusSending,
				TemplateData: json.RawMessage(`{}`),
				MaxAttempts:  3,
				AttemptCount: 2,
			},
		},
	}
	sender := &mockSender{sendErr: errors.New("SMTP down")}
	svc := newTestService(t, repo, sender)

	err := svc.ProcessOutbox(context.Background())
	if err != nil {
		t.Fatalf("ProcessOutbox: %v", err)
	}

	if repo.updated[0].Status != StatusDead {
		t.Fatalf("expected status dead, got %s", repo.updated[0].Status)
	}
}

func TestService_ProcessOutbox_NilService(t *testing.T) {
	var svc *Service
	err := svc.ProcessOutbox(context.Background())
	if err != nil {
		t.Fatalf("expected nil for nil service, got %v", err)
	}
}

func TestService_ProcessOutbox_Empty(t *testing.T) {
	repo := &mockRepo{pending: nil}
	sender := &mockSender{}
	svc := newTestService(t, repo, sender)

	err := svc.ProcessOutbox(context.Background())
	if err != nil {
		t.Fatalf("ProcessOutbox: %v", err)
	}
	if len(sender.calls) != 0 {
		t.Fatalf("expected 0 send calls, got %d", len(sender.calls))
	}
}
