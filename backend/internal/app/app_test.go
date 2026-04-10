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

type mockWorkflowReminderDistributedLocker struct {
	withLockFunc func(ctx context.Context, lockName string, waitSeconds int, fn func(ctx context.Context) error) (bool, error)
}

func (m *mockWorkflowReminderDistributedLocker) WithLock(ctx context.Context, lockName string, waitSeconds int, fn func(ctx context.Context) error) (bool, error) {
	return m.withLockFunc(ctx, lockName, waitSeconds, fn)
}

func TestNormalizeWorkflowReminderSchedulerConfig(t *testing.T) {
	normalized := normalizeWorkflowReminderSchedulerConfig(config.WorkflowReminderSchedulerConfig{})
	if normalized.IntervalSeconds != 3600 {
		t.Fatalf("expected default interval 3600, got %d", normalized.IntervalSeconds)
	}
	if normalized.LockName != "workflow:reminder:scheduler" {
		t.Fatalf("expected default lock name workflow:reminder:scheduler, got %q", normalized.LockName)
	}
	if normalized.LockWaitSeconds != 0 {
		t.Fatalf("expected default lock wait 0, got %d", normalized.LockWaitSeconds)
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
		LockName:                "workflow:reminder:scheduler:test",
		LockWaitSeconds:         5,
		ReminderType:            "ops",
		MinPendingAgeSeconds:    600,
		WindowTruncationSeconds: 900,
	})
	if custom.IntervalSeconds != 120 || custom.LockName != "workflow:reminder:scheduler:test" || custom.LockWaitSeconds != 5 || custom.ReminderType != "ops" || custom.MinPendingAgeSeconds != 600 || custom.WindowTruncationSeconds != 900 {
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

	emitted, skipped, err := runWorkflowReminderSchedulerOnce(context.Background(), mock, now, config.WorkflowReminderSchedulerConfig{
		ReminderType:            "standard",
		MinPendingAgeSeconds:    86400,
		WindowTruncationSeconds: 86400,
	})
	if err != nil {
		t.Fatalf("expected nil error, got %v", err)
	}
	if emitted != 1 || skipped != 0 {
		t.Fatalf("expected emitted=1 skipped=0, got emitted=%d skipped=%d", emitted, skipped)
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

	_, _, err := runWorkflowReminderSchedulerOnce(context.Background(), mock, time.Now().UTC(), config.WorkflowReminderSchedulerConfig{})
	if !errors.Is(err, expectedErr) {
		t.Fatalf("expected %v, got %v", expectedErr, err)
	}
}

func TestRunWorkflowReminderSchedulerTickSkipsWhenLockNotAcquired(t *testing.T) {
	called := false
	locker := &mockWorkflowReminderDistributedLocker{
		withLockFunc: func(ctx context.Context, lockName string, waitSeconds int, fn func(ctx context.Context) error) (bool, error) {
			if lockName != "workflow:reminder:scheduler" {
				t.Fatalf("unexpected lock name %q", lockName)
			}
			if waitSeconds != 0 {
				t.Fatalf("unexpected lock wait seconds %d", waitSeconds)
			}
			return false, nil
		},
	}
	service := &mockReminderSchedulerService{
		runRemindersFunc: func(ctx context.Context, now time.Time, cfg workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			called = true
			return nil, nil
		},
	}

	outcome, err := runWorkflowReminderSchedulerTick(context.Background(), locker, service, time.Now().UTC(), config.WorkflowReminderSchedulerConfig{LockName: "workflow:reminder:scheduler"})
	if err != nil {
		t.Fatalf("expected nil error, got %v", err)
	}
	if outcome.LockAcquired {
		t.Fatal("expected lock not acquired outcome")
	}
	if called {
		t.Fatal("expected scheduler run to be skipped when lock is not acquired")
	}
}

func TestRunWorkflowReminderSchedulerTickRunsWhenLockAcquired(t *testing.T) {
	called := false
	locker := &mockWorkflowReminderDistributedLocker{
		withLockFunc: func(ctx context.Context, lockName string, waitSeconds int, fn func(ctx context.Context) error) (bool, error) {
			if err := fn(ctx); err != nil {
				return true, err
			}
			return true, nil
		},
	}
	service := &mockReminderSchedulerService{
		runRemindersFunc: func(ctx context.Context, now time.Time, cfg workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error) {
			called = true
			return []workflow.ReminderAuditRecord{}, nil
		},
	}

	outcome, err := runWorkflowReminderSchedulerTick(context.Background(), locker, service, time.Now().UTC(), config.WorkflowReminderSchedulerConfig{LockName: "workflow:reminder:scheduler"})
	if err != nil {
		t.Fatalf("expected nil error, got %v", err)
	}
	if !outcome.LockAcquired {
		t.Fatal("expected lock acquired outcome")
	}
	if !called {
		t.Fatal("expected scheduler run when lock is acquired")
	}
}

func TestWorkflowReminderSchedulerObservabilityTransitions(t *testing.T) {
	obs := newWorkflowReminderSchedulerObservability()
	now := time.Date(2026, 4, 10, 21, 0, 0, 0, time.UTC)

	snap := obs.recordFailure(now, 250*time.Millisecond, errors.New("db timeout"))
	if snap.TotalRuns != 1 || snap.FailedRuns != 1 || snap.ConsecutiveFailures != 1 {
		t.Fatalf("unexpected failure snapshot %#v", snap)
	}

	snap = obs.recordLockSkip(now.Add(time.Minute), 100*time.Millisecond)
	if snap.TotalRuns != 2 || snap.LockSkippedRuns != 1 || snap.ConsecutiveFailures != 1 {
		t.Fatalf("unexpected lock-skip snapshot %#v", snap)
	}

	snap = obs.recordSuccess(now.Add(2*time.Minute), 300*time.Millisecond, 2, 3)
	if snap.TotalRuns != 3 || snap.SuccessfulRuns != 1 || snap.ConsecutiveFailures != 0 {
		t.Fatalf("unexpected success snapshot %#v", snap)
	}
	if snap.LastEmitted != 2 || snap.LastSkipped != 3 || snap.LastRunDurationMs != 300 {
		t.Fatalf("unexpected success counters %#v", snap)
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
