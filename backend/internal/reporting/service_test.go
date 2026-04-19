package reporting

import (
	"testing"
	"time"
)

func TestParsePeriodValid(t *testing.T) {
	start, end, label, err := ParsePeriod("2025-03")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if label != "2025-03" {
		t.Fatalf("expected label 2025-03, got %s", label)
	}
	expectedStart := time.Date(2025, 3, 1, 0, 0, 0, 0, time.UTC)
	if !start.Equal(expectedStart) {
		t.Fatalf("expected start %v, got %v", expectedStart, start)
	}
	expectedEnd := time.Date(2025, 3, 31, 23, 59, 59, 999999999, time.UTC)
	if !end.Equal(expectedEnd) {
		t.Fatalf("expected end %v, got %v", expectedEnd, end)
	}
}

func TestParsePeriodEmpty(t *testing.T) {
	_, _, _, err := ParsePeriod("")
	if err != ErrInvalidPeriod {
		t.Fatalf("expected ErrInvalidPeriod, got %v", err)
	}
}

func TestParsePeriodInvalidFormat(t *testing.T) {
	_, _, _, err := ParsePeriod("not-a-period")
	if err != ErrInvalidPeriod {
		t.Fatalf("expected ErrInvalidPeriod, got %v", err)
	}
}

func TestParsePeriodWhitespace(t *testing.T) {
	_, _, _, err := ParsePeriod("   ")
	if err != ErrInvalidPeriod {
		t.Fatalf("expected ErrInvalidPeriod for whitespace, got %v", err)
	}
}

func TestRunReportUnsupportedID(t *testing.T) {
	svc := &Service{repository: nil}
	_, _, err := svc.runReport(nil, QueryInput{ReportID: "r99"})
	if err != ErrUnsupportedReport {
		t.Fatalf("expected ErrUnsupportedReport, got %v", err)
	}
}

func TestR04RowDayValue(t *testing.T) {
	row := R04Row{Day15: 123.45}
	if v := row.DayValue(15); v != 123.45 {
		t.Fatalf("expected 123.45, got %f", v)
	}
	if v := row.DayValue(0); v != 0 {
		t.Fatalf("expected 0 for out-of-range day, got %f", v)
	}
}

func TestR10RowMonthValue(t *testing.T) {
	row := R10Row{Month06: 42}
	if v := row.MonthValue(6); v != 42 {
		t.Fatalf("expected 42, got %d", v)
	}
	if v := row.MonthValue(0); v != 0 {
		t.Fatalf("expected 0 for out-of-range month, got %d", v)
	}
}
