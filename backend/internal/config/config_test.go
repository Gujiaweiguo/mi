package config

import (
	"os"
	"path/filepath"
	"testing"
)

func TestLoadReadsConfigFile(t *testing.T) {
	t.Setenv(envPrefix+"_CONFIG_FILE", filepath.Join("..", "..", "config", "test.yaml"))

	cfg, err := Load()
	if err != nil {
		t.Fatalf("load config: %v", err)
	}

	if cfg.App.Environment != "test" {
		t.Fatalf("expected test environment, got %q", cfg.App.Environment)
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
}

func TestLoadAppliesEnvironmentOverrides(t *testing.T) {
	t.Setenv(envPrefix+"_CONFIG_FILE", filepath.Join("..", "..", "config", "test.yaml"))
	t.Setenv(envPrefix+"_DATABASE_HOST", "override-db")
	t.Setenv(envPrefix+"_WORKFLOW_REMINDER_SCHEDULER_ENABLED", "true")

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
