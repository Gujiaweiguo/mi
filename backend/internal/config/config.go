package config

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"

	"github.com/spf13/viper"
)

const envPrefix = "MI"

type Config struct {
	App                       AppConfig                       `mapstructure:"app"`
	Server                    ServerConfig                    `mapstructure:"server"`
	Database                  DatabaseConfig                  `mapstructure:"database"`
	Auth                      AuthConfig                      `mapstructure:"auth"`
	Log                       LogConfig                       `mapstructure:"log"`
	Storage                   StorageConfig                   `mapstructure:"storage"`
	WorkflowReminderScheduler WorkflowReminderSchedulerConfig `mapstructure:"workflow_reminder_scheduler"`
}

type AppConfig struct {
	Name        string `mapstructure:"name"`
	Environment string `mapstructure:"environment"`
}

type ServerConfig struct {
	Host                string `mapstructure:"host"`
	Port                int    `mapstructure:"port"`
	ReadTimeoutSeconds  int    `mapstructure:"read_timeout_seconds"`
	WriteTimeoutSeconds int    `mapstructure:"write_timeout_seconds"`
}

type DatabaseConfig struct {
	Host     string `mapstructure:"host"`
	Port     int    `mapstructure:"port"`
	Name     string `mapstructure:"name"`
	User     string `mapstructure:"user"`
	Password string `mapstructure:"password"`
	SSLMode  string `mapstructure:"ssl_mode"`
}

type AuthConfig struct {
	JWTSecret          string `mapstructure:"jwt_secret"`
	TokenExpirySeconds int    `mapstructure:"token_expiry_seconds"`
}

type LogConfig struct {
	Level string `mapstructure:"level"`
}

type StorageConfig struct {
	GeneratedDocumentsPath string `mapstructure:"generated_documents_path"`
	UploadsPath            string `mapstructure:"uploads_path"`
	LogsPath               string `mapstructure:"logs_path"`
}

type WorkflowReminderSchedulerConfig struct {
	Enabled                 bool   `mapstructure:"enabled"`
	IntervalSeconds         int    `mapstructure:"interval_seconds"`
	ReminderType            string `mapstructure:"reminder_type"`
	MinPendingAgeSeconds    int    `mapstructure:"min_pending_age_seconds"`
	WindowTruncationSeconds int    `mapstructure:"window_truncation_seconds"`
}

func Load() (*Config, error) {
	configFile := os.Getenv(envPrefix + "_CONFIG_FILE")
	if configFile == "" {
		configFile = filepath.Join("config", "development.yaml")
	}

	v := viper.New()
	v.SetConfigFile(configFile)
	v.SetConfigType("yaml")
	v.SetEnvPrefix(envPrefix)
	v.SetEnvKeyReplacer(strings.NewReplacer(".", "_"))
	v.AutomaticEnv()

	if err := v.ReadInConfig(); err != nil {
		return nil, fmt.Errorf("read config %s: %w", configFile, err)
	}

	var cfg Config
	if err := v.Unmarshal(&cfg); err != nil {
		return nil, fmt.Errorf("unmarshal config: %w", err)
	}

	return &cfg, nil
}
