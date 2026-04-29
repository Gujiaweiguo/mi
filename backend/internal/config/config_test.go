package config

import (
	"os"
	"path/filepath"
	"strings"
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

func TestValidate_NonProductionSkipsValidation(t *testing.T) {
	cfg := &Config{
		App:     AppConfig{Environment: "development"},
		Database: DatabaseConfig{Password: "change-me"},
		Auth:    AuthConfig{JWTSecret: "change-me"},
	}
	if err := cfg.Validate(); err != nil {
		t.Fatalf("expected nil error in non-production, got: %v", err)
	}
}

func TestValidate_ProductionWithValidSecrets(t *testing.T) {
	cfg := &Config{
		App:     AppConfig{Environment: "production"},
		Database: DatabaseConfig{Password: "a-real-db-password-9f8e7d"},
		Auth:    AuthConfig{JWTSecret: "a-real-jwt-secret-4a3b2c"},
	}
	if err := cfg.Validate(); err != nil {
		t.Fatalf("expected nil error with valid secrets, got: %v", err)
	}
}

func TestValidate_ProductionBlockedDBPassword(t *testing.T) {
	cfg := &Config{
		App:     AppConfig{Environment: "production"},
		Database: DatabaseConfig{Password: "change-me"},
		Auth:    AuthConfig{JWTSecret: "a-real-jwt-secret"},
	}
	err := cfg.Validate()
	if err == nil {
		t.Fatal("expected error for blocked DB password, got nil")
	}
	if !strings.Contains(err.Error(), "database.password") {
		t.Errorf("error should mention database.password, got: %v", err)
	}
}

func TestValidate_ProductionBlockedJWTSecret(t *testing.T) {
	cfg := &Config{
		App:     AppConfig{Environment: "production"},
		Database: DatabaseConfig{Password: "a-real-db-password"},
		Auth:    AuthConfig{JWTSecret: "change-me-production-secret"},
	}
	err := cfg.Validate()
	if err == nil {
		t.Fatal("expected error for blocked JWT secret, got nil")
	}
	if !strings.Contains(err.Error(), "auth.jwt_secret") {
		t.Errorf("error should mention auth.jwt_secret, got: %v", err)
	}
}

func TestValidate_ProductionDevPasswordBlocked(t *testing.T) {
	cfg := &Config{
		App:     AppConfig{Environment: "production"},
		Database: DatabaseConfig{Password: "devpassword"},
		Auth:    AuthConfig{JWTSecret: "a-real-jwt-secret"},
	}
	err := cfg.Validate()
	if err == nil {
		t.Fatal("expected error for devpassword, got nil")
	}
}

func TestValidate_ProductionJWTSameAsDBPassword(t *testing.T) {
	cfg := &Config{
		App:     AppConfig{Environment: "production"},
		Database: DatabaseConfig{Password: "same-secret-value"},
		Auth:    AuthConfig{JWTSecret: "same-secret-value"},
	}
	err := cfg.Validate()
	if err == nil {
		t.Fatal("expected error when JWT equals DB password, got nil")
	}
	if !strings.Contains(err.Error(), "must differ") {
		t.Errorf("error should mention must differ, got: %v", err)
	}
}

func TestIsBlockedSecret(t *testing.T) {
	tests := []struct {
		value string
		want  bool
	}{
		{"change-me", true},
		{"CHANGE-ME", true},
		{"prefix-change-me-suffix", true},
		{"devpassword", true},
		{"DevPassword", true},
		{"dev-secret-change-in-production", true},
		{"change-me-development-secret", true},
		{"a-real-secret-abc123", false},
		{"", false},
		{"prodpassword", false},
	}
	for _, tt := range tests {
		got := isBlockedSecret(tt.value)
		if got != tt.want {
			t.Errorf("isBlockedSecret(%q) = %v, want %v", tt.value, got, tt.want)
		}
	}
}

func TestIsProduction(t *testing.T) {
	tests := []struct {
		env  string
		want bool
	}{
		{"production", true},
		{"PRODUCTION", true},
		{"Production", true},
		{"prod", true},
		{"PROD", true},
		{"development", false},
		{"staging", false},
		{"", false},
	}
	for _, tt := range tests {
		cfg := &Config{App: AppConfig{Environment: tt.env}}
		got := cfg.isProduction()
		if got != tt.want {
			t.Errorf("isProduction(%q) = %v, want %v", tt.env, got, tt.want)
		}
	}
}
