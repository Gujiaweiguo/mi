package database

import (
	"strings"
	"testing"
)

func TestConfigDSN(t *testing.T) {
	dsn := Config{
		Host:     "127.0.0.1",
		Port:     3306,
		Name:     "mi_test",
		User:     "mi_test",
		Password: "secret",
	}.DSN()

	expectedParts := []string{
		"mi_test:secret@tcp(127.0.0.1:3306)/mi_test",
		"charset=utf8mb4",
		"parseTime=True",
		"loc=Local",
	}

	for _, part := range expectedParts {
		if !strings.Contains(dsn, part) {
			t.Fatalf("dsn %q does not contain %q", dsn, part)
		}
	}
}
