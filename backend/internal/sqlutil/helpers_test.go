package sqlutil

import (
	"database/sql"
	"testing"
	"time"
)

// --- Null scanning helpers ---

func TestNullInt64Pointer_Valid(t *testing.T) {
	v := sql.NullInt64{Int64: 42, Valid: true}
	got := NullInt64Pointer(v)
	if got == nil {
		t.Fatal("expected non-nil pointer")
	}
	if *got != 42 {
		t.Errorf("got %d, want 42", *got)
	}
}

func TestNullInt64Pointer_Invalid(t *testing.T) {
	v := sql.NullInt64{Valid: false}
	got := NullInt64Pointer(v)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestNullIntPointer_Valid(t *testing.T) {
	v := sql.NullInt64{Int64: 7, Valid: true}
	got := NullIntPointer(v)
	if got == nil {
		t.Fatal("expected non-nil pointer")
	}
	if *got != 7 {
		t.Errorf("got %d, want 7", *got)
	}
}

func TestNullIntPointer_Invalid(t *testing.T) {
	v := sql.NullInt64{Valid: false}
	got := NullIntPointer(v)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestNullFloat64Pointer_Valid(t *testing.T) {
	v := sql.NullFloat64{Float64: 3.14, Valid: true}
	got := NullFloat64Pointer(v)
	if got == nil {
		t.Fatal("expected non-nil pointer")
	}
	if *got != 3.14 {
		t.Errorf("got %f, want 3.14", *got)
	}
}

func TestNullFloat64Pointer_Invalid(t *testing.T) {
	v := sql.NullFloat64{Valid: false}
	got := NullFloat64Pointer(v)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestNullStringPointer_Valid(t *testing.T) {
	v := sql.NullString{String: "hello", Valid: true}
	got := NullStringPointer(v)
	if got == nil {
		t.Fatal("expected non-nil pointer")
	}
	if *got != "hello" {
		t.Errorf("got %q, want %q", *got, "hello")
	}
}

func TestNullStringPointer_Invalid(t *testing.T) {
	v := sql.NullString{Valid: false}
	got := NullStringPointer(v)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestNullTimePointer_Valid(t *testing.T) {
	now := time.Date(2025, 6, 15, 10, 30, 0, 0, time.UTC)
	v := sql.NullTime{Time: now, Valid: true}
	got := NullTimePointer(v)
	if got == nil {
		t.Fatal("expected non-nil pointer")
	}
	if !got.Equal(now) {
		t.Errorf("got %v, want %v", *got, now)
	}
}

func TestNullTimePointer_Invalid(t *testing.T) {
	v := sql.NullTime{Valid: false}
	got := NullTimePointer(v)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

// --- Pointer value helpers ---

func TestInt64PointerValue_Nil(t *testing.T) {
	got := Int64PointerValue(nil)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestInt64PointerValue_NonNil(t *testing.T) {
	v := int64(99)
	got := Int64PointerValue(&v)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if got.(int64) != 99 {
		t.Errorf("got %v, want 99", got)
	}
}

func TestStringPointerValue_Nil(t *testing.T) {
	got := StringPointerValue(nil)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestStringPointerValue_NonNil(t *testing.T) {
	v := "test"
	got := StringPointerValue(&v)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if got.(string) != "test" {
		t.Errorf("got %v, want %q", got, "test")
	}
}

func TestBoolPointerValue_Nil(t *testing.T) {
	got := BoolPointerValue(nil)
	if got != false {
		t.Errorf("expected false for nil, got %v", got)
	}
}

func TestBoolPointerValue_NonNil(t *testing.T) {
	val := true
	got := BoolPointerValue(&val)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if got.(bool) != true {
		t.Errorf("got %v, want true", got)
	}
}

func TestFloat64PointerValue_Nil(t *testing.T) {
	got := Float64PointerValue(nil)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestFloat64PointerValue_NonNil(t *testing.T) {
	v := 2.718
	got := Float64PointerValue(&v)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if got.(float64) != 2.718 {
		t.Errorf("got %v, want 2.718", got)
	}
}

func TestIntPointerValue_Nil(t *testing.T) {
	got := IntPointerValue(nil)
	if got != 0 {
		t.Errorf("expected 0 for nil, got %v", got)
	}
}

func TestIntPointerValue_NonNil(t *testing.T) {
	v := 55
	got := IntPointerValue(&v)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if got.(int) != 55 {
		t.Errorf("got %v, want 55", got)
	}
}

func TestTimePointerValue_Nil(t *testing.T) {
	got := TimePointerValue(nil)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestTimePointerValue_NonNil(t *testing.T) {
	now := time.Date(2025, 1, 1, 0, 0, 0, 0, time.UTC)
	got := TimePointerValue(&now)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if !got.(time.Time).Equal(now) {
		t.Errorf("got %v, want %v", got, now)
	}
}

func TestTimePointerDateString_Nil(t *testing.T) {
	got := TimePointerDateString(nil)
	if got != nil {
		t.Errorf("expected nil, got %v", got)
	}
}

func TestTimePointerDateString_NonNil(t *testing.T) {
	dt := time.Date(2025, 3, 14, 15, 9, 26, 0, time.UTC)
	got := TimePointerDateString(&dt)
	if got == nil {
		t.Fatal("expected non-nil")
	}
	if got.(string) != "2025-03-14" {
		t.Errorf("got %q, want %q", got, "2025-03-14")
	}
}

// --- InPlaceholders ---

func TestInPlaceholders(t *testing.T) {
	tests := []struct {
		name string
		n    int
		want string
	}{
		{"zero", 0, ""},
		{"negative", -1, ""},
		{"one", 1, "?"},
		{"three", 3, "?, ?, ?"},
		{"five", 5, "?, ?, ?, ?, ?"},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got := InPlaceholders(tt.n)
			if got != tt.want {
				t.Errorf("InPlaceholders(%d) = %q, want %q", tt.n, got, tt.want)
			}
		})
	}
}
