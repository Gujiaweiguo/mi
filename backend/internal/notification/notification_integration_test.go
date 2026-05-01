//go:build integration

package notification_test

import (
	"context"
	"database/sql"
	"encoding/json"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/notification"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
)

func TestNotificationRepositoryIntegrationOutboxOperations(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := notification.NewRepository(db)

	t.Run("insert outbox and fetch pending", func(t *testing.T) {
		entry := insertOutboxEntry(t, ctx, db, repo, notification.StatusPending)
		entries, err := repo.FetchPending(ctx, db, 10)
		if err != nil {
			t.Fatalf("fetch pending outbox: %v", err)
		}
		found := findOutboxEntry(entries, entry.ID)
		if found == nil {
			t.Fatalf("expected pending entry %d in fetch result: %+v", entry.ID, entries)
		}
		if found.Status != notification.StatusSending {
			t.Fatalf("expected fetched entry to be claimed as sending, got %+v", found)
		}
	})

	t.Run("update status", func(t *testing.T) {
		entry := insertOutboxEntry(t, ctx, db, repo, notification.StatusPending)
		entries, err := repo.FetchPending(ctx, db, 10)
		if err != nil {
			t.Fatalf("fetch pending before update: %v", err)
		}
		found := findOutboxEntry(entries, entry.ID)
		if found == nil {
			t.Fatalf("expected fetched entry %d for update", entry.ID)
		}
		now := time.Now().UTC()
		found.Status = notification.StatusSent
		found.SentAt = &now
		if err := repo.UpdateStatus(ctx, found); err != nil {
			t.Fatalf("update outbox status: %v", err)
		}
		items, total, err := repo.ListOutbox(ctx, db, notification.ListParams{Page: 1, PageSize: 20, Status: string(notification.StatusSent)})
		if err != nil {
			t.Fatalf("list outbox after update: %v", err)
		}
		if total == 0 {
			t.Fatalf("expected sent entry in outbox history, got total %d", total)
		}
		persisted := findOutboxEntry(items, entry.ID)
		if persisted == nil || persisted.Status != notification.StatusSent || persisted.SentAt == nil {
			t.Fatalf("expected persisted sent entry, got %+v", persisted)
		}
	})

	t.Run("fetch pending skips non pending", func(t *testing.T) {
		insertOutboxEntry(t, ctx, db, repo, notification.StatusSent)
		entries, err := repo.FetchPending(ctx, db, 10)
		if err != nil {
			t.Fatalf("fetch pending after sent insert: %v", err)
		}
		for _, entry := range entries {
			if entry.Status == notification.StatusSent {
				t.Fatalf("expected fetch pending to skip sent entries, got %+v", entries)
			}
		}
	})
}

func insertOutboxEntry(t *testing.T, ctx context.Context, db *sql.DB, repo *notification.Repository, status notification.Status) *notification.OutboxEntry {
	t.Helper()
	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin outbox transaction: %v", err)
	}
	defer func() { _ = tx.Rollback() }()

	templateData, err := json.Marshal(map[string]any{"key": "value"})
	if err != nil {
		t.Fatalf("marshal template data: %v", err)
	}
	entry := &notification.OutboxEntry{
		EventType:     "lease.approved",
		AggregateType: "lease",
		AggregateID:   123,
		RecipientTo:   "ops@example.com",
		RecipientCc:   "finance@example.com",
		Subject:       "Integration Notification",
		TemplateName:  "workflow-approval",
		TemplateData:  templateData,
		Status:        status,
		AttemptCount:  0,
		MaxAttempts:   5,
	}
	if status == notification.StatusSent {
		now := time.Now().UTC()
		entry.SentAt = &now
	}
	if err := repo.InsertOutbox(ctx, tx, entry); err != nil {
		t.Fatalf("insert outbox entry: %v", err)
	}
	if err := tx.Commit(); err != nil {
		t.Fatalf("commit outbox transaction: %v", err)
	}
	return entry
}

func findOutboxEntry(entries []*notification.OutboxEntry, id int64) *notification.OutboxEntry {
	for _, entry := range entries {
		if entry != nil && entry.ID == id {
			return entry
		}
	}
	return nil
}
