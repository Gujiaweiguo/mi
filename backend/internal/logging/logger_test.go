package logging

import (
	"strings"
	"testing"
)

func TestNew_DebugLevel(t *testing.T) {
	logger, err := New("debug")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if logger == nil {
		t.Fatal("expected non-nil logger")
	}
}

func TestNew_InfoLevel(t *testing.T) {
	logger, err := New("info")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if logger == nil {
		t.Fatal("expected non-nil logger")
	}
}

func TestNew_WarnLevel(t *testing.T) {
	logger, err := New("warn")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if logger == nil {
		t.Fatal("expected non-nil logger")
	}
}

func TestNew_ErrorLevel(t *testing.T) {
	logger, err := New("error")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if logger == nil {
		t.Fatal("expected non-nil logger")
	}
}

func TestNew_WarningLevel(t *testing.T) {
	logger, err := New("warning")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if logger == nil {
		t.Fatal("expected non-nil logger")
	}
}

func TestNew_CaseInsensitive(t *testing.T) {
	logger, err := New("DEBUG")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if logger == nil {
		t.Fatal("expected non-nil logger")
	}
}

func TestNew_InvalidLevel(t *testing.T) {
	logger, err := New("INVALID")
	if err == nil {
		t.Fatal("expected error for unsupported log level")
	}
	if logger != nil {
		t.Fatal("expected nil logger for unsupported log level")
	}
	if !strings.Contains(err.Error(), "unsupported log level") {
		t.Fatalf("error should mention unsupported log level, got: %v", err)
	}
}

func TestNew_EmptyLevel(t *testing.T) {
	logger, err := New("")
	if err == nil {
		t.Fatal("expected error for empty log level")
	}
	if logger != nil {
		t.Fatal("expected nil logger for empty log level")
	}
	if !strings.Contains(err.Error(), "unsupported log level") {
		t.Fatalf("error should mention unsupported log level, got: %v", err)
	}
}
