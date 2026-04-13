package config

import (
	"os"
	"path/filepath"
	"testing"
)

func TestWorkflowReminderSchedulerConfigPresentInAllBuiltInConfigs(t *testing.T) {
	configFiles := []struct {
		name string
		path string
	}{
		{name: "development", path: filepath.Join("..", "..", "config", "development.yaml")},
		{name: "production", path: filepath.Join("..", "..", "config", "production.yaml")},
	}

	for _, tc := range configFiles {
		t.Run(tc.name, func(t *testing.T) {
			t.Setenv(envPrefix+"_CONFIG_FILE", tc.path)

			cfg, err := Load()
			if err != nil {
				t.Fatalf("load config: %v", err)
			}

			if cfg.WorkflowReminderScheduler.IntervalSeconds != 3600 {
				t.Fatalf("expected reminder scheduler interval 3600, got %d", cfg.WorkflowReminderScheduler.IntervalSeconds)
			}
			if cfg.WorkflowReminderScheduler.LockName != "workflow:reminder:scheduler" {
				t.Fatalf("expected scheduler lock name workflow:reminder:scheduler, got %q", cfg.WorkflowReminderScheduler.LockName)
			}
			if cfg.WorkflowReminderScheduler.ReminderType != "standard" {
				t.Fatalf("expected scheduler reminder type standard, got %q", cfg.WorkflowReminderScheduler.ReminderType)
			}
			if cfg.WorkflowReminderScheduler.MinPendingAgeSeconds != 86400 {
				t.Fatalf("expected min pending age 86400, got %d", cfg.WorkflowReminderScheduler.MinPendingAgeSeconds)
			}
			if cfg.WorkflowReminderScheduler.WindowTruncationSeconds != 86400 {
				t.Fatalf("expected window truncation 86400, got %d", cfg.WorkflowReminderScheduler.WindowTruncationSeconds)
			}
		})
	}
}

func TestLoadReadsConfigFile(t *testing.T) {
	t.Setenv(envPrefix+"_CONFIG_FILE", filepath.Join("..", "..", "config", "production.yaml"))

	cfg, err := Load()
	if err != nil {
		t.Fatalf("load config: %v", err)
	}

	if cfg.App.Environment != "production" {
		t.Fatalf("expected production environment, got %q", cfg.App.Environment)
	}

	if cfg.Database.Host != "mysql" {
		t.Fatalf("expected mysql host, got %q", cfg.Database.Host)
	}

	if cfg.WorkflowReminderScheduler.Enabled {
		t.Fatal("expected reminder scheduler disabled by default in test config")
	}
	if cfg.WorkflowReminderScheduler.IntervalSeconds != 3600 {
		t.Fatalf("expected reminder scheduler interval 3600, got %d", cfg.WorkflowReminderScheduler.IntervalSeconds)
	}
	if cfg.WorkflowReminderScheduler.LockName != "workflow:reminder:scheduler" {
		t.Fatalf("expected default scheduler lock name, got %q", cfg.WorkflowReminderScheduler.LockName)
	}
	if cfg.WorkflowReminderScheduler.LockWaitSeconds != 0 {
		t.Fatalf("expected scheduler lock wait seconds 0, got %d", cfg.WorkflowReminderScheduler.LockWaitSeconds)
	}
}

func TestLoadAppliesEnvironmentOverrides(t *testing.T) {
	t.Setenv(envPrefix+"_CONFIG_FILE", filepath.Join("..", "..", "config", "production.yaml"))
	t.Setenv(envPrefix+"_DATABASE_HOST", "override-db")
	t.Setenv(envPrefix+"_WORKFLOW_REMINDER_SCHEDULER_ENABLED", "true")
	t.Setenv(envPrefix+"_WORKFLOW_REMINDER_SCHEDULER_LOCK_NAME", "workflow:reminder:scheduler:test")

	cfg, err := Load()
	if err != nil {
		t.Fatalf("load config: %v", err)
	}

	if cfg.Database.Host != "override-db" {
		t.Fatalf("expected env override, got %q", cfg.Database.Host)
	}
	if !cfg.WorkflowReminderScheduler.Enabled {
		t.Fatal("expected scheduler enabled override from environment")
	}
	if cfg.WorkflowReminderScheduler.LockName != "workflow:reminder:scheduler:test" {
		t.Fatalf("expected scheduler lock name override, got %q", cfg.WorkflowReminderScheduler.LockName)
	}
}

func TestLoadFailsForMissingConfig(t *testing.T) {
	t.Setenv(envPrefix+"_CONFIG_FILE", filepath.Join(t.TempDir(), "missing.yaml"))

	_, err := Load()
	if err == nil {
		t.Fatal("expected error for missing config file")
	}

	if _, statErr := os.Stat(filepath.Join(t.TempDir(), "missing.yaml")); !os.IsNotExist(statErr) {
		t.Fatalf("expected temp path to remain absent, got %v", statErr)
	}
}
