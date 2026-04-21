package excelio

import (
	"context"
	"testing"
)

func TestParsePositiveFloatValid(t *testing.T) {
	v, err := parsePositiveFloat("42.5")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if v != 42.5 {
		t.Fatalf("expected 42.5, got %f", v)
	}
}

func TestParsePositiveFloatZero(t *testing.T) {
	_, err := parsePositiveFloat("0")
	if err == nil {
		t.Fatal("expected error for zero value")
	}
}

func TestParsePositiveFloatNegative(t *testing.T) {
	_, err := parsePositiveFloat("-5.0")
	if err == nil {
		t.Fatal("expected error for negative value")
	}
}

func TestParsePositiveFloatNonNumeric(t *testing.T) {
	_, err := parsePositiveFloat("abc")
	if err == nil {
		t.Fatal("expected error for non-numeric value")
	}
}

func TestParsePositiveIntValid(t *testing.T) {
	v, err := parsePositiveInt("100")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if v != 100 {
		t.Fatalf("expected 100, got %d", v)
	}
}

func TestParsePositiveIntZero(t *testing.T) {
	_, err := parsePositiveInt("0")
	if err == nil {
		t.Fatal("expected error for zero value")
	}
}

func TestParseDateValid(t *testing.T) {
	v, err := parseDate("2025-03-15")
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if v.Year() != 2025 || v.Month() != 3 || v.Day() != 15 {
		t.Fatalf("unexpected date: %v", v)
	}
}

func TestParseDateEmpty(t *testing.T) {
	_, err := parseDate("")
	if err == nil {
		t.Fatal("expected error for empty date")
	}
}

func TestParseDateInvalid(t *testing.T) {
	_, err := parseDate("not-a-date")
	if err == nil {
		t.Fatal("expected error for invalid date")
	}
}

func TestParseBoolTrue(t *testing.T) {
	for _, input := range []string{"true", "True", "TRUE", "yes", "1"} {
		v, err := parseBool(input)
		if err != nil {
			t.Fatalf("unexpected error for %q: %v", input, err)
		}
		if !v {
			t.Fatalf("expected true for %q", input)
		}
	}
}

func TestParseBoolFalse(t *testing.T) {
	for _, input := range []string{"false", "False", "no", "0"} {
		v, err := parseBool(input)
		if err != nil {
			t.Fatalf("unexpected error for %q: %v", input, err)
		}
		if v {
			t.Fatalf("expected false for %q", input)
		}
	}
}

func TestParseBoolInvalid(t *testing.T) {
	_, err := parseBool("maybe")
	if err == nil {
		t.Fatal("expected error for invalid bool")
	}
}

func TestCellValueOutOfBounds(t *testing.T) {
	row := []string{"a", "b"}
	if v := cellValue(row, 5); v != "" {
		t.Fatalf("expected empty string for out-of-bounds, got %q", v)
	}
}

func TestIsEmptyRow(t *testing.T) {
	if !isEmptyRow([]string{"", " ", "  "}) {
		t.Fatal("expected empty row with whitespace-only cells")
	}
	if isEmptyRow([]string{"", "x", ""}) {
		t.Fatal("expected non-empty row")
	}
}

func TestExportOperationalDatasetInvalid(t *testing.T) {
	svc := &Service{repository: nil, salesImporter: nil}
	_, err := svc.ExportOperationalDataset(context.TODO(), ExportInput{Dataset: "nonexistent"})
	if err != ErrInvalidDataset {
		t.Fatalf("expected ErrInvalidDataset, got %v", err)
	}
}

func TestReferenceByCode(t *testing.T) {
	items := []ReferenceItem{
		{ID: 1, Code: "A"},
		{ID: 2, Code: "B"},
	}
	m := referenceByCode(items)
	if m["A"].ID != 1 {
		t.Fatalf("expected ID 1 for code A, got %d", m["A"].ID)
	}
	if m["B"].ID != 2 {
		t.Fatalf("expected ID 2 for code B, got %d", m["B"].ID)
	}
	if _, ok := m["C"]; ok {
		t.Fatal("expected missing key C to not exist")
	}
}
