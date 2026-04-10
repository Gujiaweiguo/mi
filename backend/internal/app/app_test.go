package app

import (
	"context"
	"errors"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"go.uber.org/zap"
)

type mockReminderSchedulerService struct {
	runRemindersFunc func(ctx context.Context, now time.Time, cfg workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error)
}

func (m *mockReminderSchedulerService) RunReminders(ctx context.Context, now time.Time, cfg workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
	return m.runRemindersFunc(ctx, now, cfg)
}

func TestNormalizeWorkflowReminderSchedulerConfig(t *testing.T) {
	normalized := normalizeWorkflowReminderSchedulerConfig(config.WorkflowReminderSchedulerConfig{})
	if normalized.IntervalSeconds != 3600 {
		t.Fatalf("expected default interval 3600, got %d", normalized.IntervalSeconds)
	}
	if normalized.ReminderType != "standard" {
		t.Fatalf("expected default reminder type standard, got %q", normalized.ReminderType)
	}
	if normalized.MinPendingAgeSeconds != 0 {
		t.Fatalf("expected min pending age clamped to 0, got %d", normalized.MinPendingAgeSeconds)
	}
	if normalized.WindowTruncationSeconds != 86400 {
		t.Fatalf("expected default window truncation 86400, got %d", normalized.WindowTruncationSeconds)
	}

	custom := normalizeWorkflowReminderSchedulerConfig(config.WorkflowReminderSchedulerConfig{
		Enabled:                 true,
		IntervalSeconds:         120,
		ReminderType:            "ops",
		MinPendingAgeSeconds:    600,
		WindowTruncationSeconds: 900,
	})
	if custom.IntervalSeconds != 120 || custom.ReminderType != "ops" || custom.MinPendingAgeSeconds != 600 || custom.WindowTruncationSeconds != 900 {
		t.Fatalf("expected custom values preserved, got %#v", custom)
	}
}

func TestRunWorkflowReminderSchedulerOncePassesConfig(t *testing.T) {
	now := time.Date(2026, 4, 10, 20, 0, 0, 0, time.UTC)
	called := false

	mock := &mockReminderSchedulerService{
		runRemindersFunc: func(ctx context.Context, runAt time.Time, cfg workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			called = true
			if !runAt.Equal(now) {
				t.Fatalf("expected runAt %v, got %v", now, runAt)
			}
			if cfg.ReminderType != "standard" {
				t.Fatalf("expected reminder type standard, got %q", cfg.ReminderType)
			}
			if cfg.MinPendingAge != 24*time.Hour {
				t.Fatalf("expected min pending age 24h, got %v", cfg.MinPendingAge)
			}
			if cfg.WindowTruncation != 24*time.Hour {
				t.Fatalf("expected truncation 24h, got %v", cfg.WindowTruncation)
			}
			return []workflow.ReminderAuditRecord{{Outcome: workflow.ReminderOutcomeEmitted}}, nil
		},
	}

	err := runWorkflowReminderSchedulerOnce(context.Background(), zap.NewNop(), mock, now, config.WorkflowReminderSchedulerConfig{
		ReminderType:            "standard",
		MinPendingAgeSeconds:    86400,
		WindowTruncationSeconds: 86400,
	})
	if err != nil {
		t.Fatalf("expected nil error, got %v", err)
	}
	if !called {
		t.Fatal("expected RunReminders to be called")
	}
}

func TestRunWorkflowReminderSchedulerOnceReturnsServiceError(t *testing.T) {
	expectedErr := errors.New("boom")
	mock := &mockReminderSchedulerService{
		runRemindersFunc: func(ctx context.Context, now time.Time, cfg workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			return nil, expectedErr
		},
	}

	err := runWorkflowReminderSchedulerOnce(context.Background(), zap.NewNop(), mock, time.Now().UTC(), config.WorkflowReminderSchedulerConfig{})
	if !errors.Is(err, expectedErr) {
		t.Fatalf("expected %v, got %v", expectedErr, err)
	}
}

func TestStartWorkflowReminderSchedulerDisabledMode(t *testing.T) {
	application := &App{
		config: &config.Config{
			WorkflowReminderScheduler: config.WorkflowReminderSchedulerConfig{Enabled: false},
		},
		logger: zap.NewNop(),
	}

	stop := application.startWorkflowReminderScheduler()
	if stop == nil {
		t.Fatal("expected non-nil stop function")
	}
	stop()
}
