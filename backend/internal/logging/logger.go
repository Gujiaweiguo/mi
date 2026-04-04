package logging

import (
	"fmt"
	"strings"

	"go.uber.org/zap"
)

func New(level string) (*zap.Logger, error) {
	config := zap.NewProductionConfig()

	switch strings.ToLower(level) {
	case "debug":
		config = zap.NewDevelopmentConfig()
	case "info", "warn", "warning", "error":
		// keep production config
	default:
		return nil, fmt.Errorf("unsupported log level %q", level)
	}

	return config.Build()
}
