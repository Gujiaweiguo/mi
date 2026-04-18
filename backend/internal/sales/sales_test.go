package sales

import "testing"

func TestNormalizeLimit_Zero(t *testing.T) {
	if got := normalizeLimit(0); got != 100 {
		t.Fatalf("normalizeLimit(0) = %d, want 100", got)
	}
}

func TestNormalizeLimit_Negative(t *testing.T) {
	if got := normalizeLimit(-1); got != 100 {
		t.Fatalf("normalizeLimit(-1) = %d, want 100", got)
	}
}

func TestNormalizeLimit_ValidValue(t *testing.T) {
	if got := normalizeLimit(50); got != 50 {
		t.Fatalf("normalizeLimit(50) = %d, want 50", got)
	}
}

func TestNormalizeLimit_ExceedsMax(t *testing.T) {
	if got := normalizeLimit(501); got != 500 {
		t.Fatalf("normalizeLimit(501) = %d, want 500", got)
	}
}

func TestNormalizeLimit_ExactlyMax(t *testing.T) {
	if got := normalizeLimit(500); got != 500 {
		t.Fatalf("normalizeLimit(500) = %d, want 500", got)
	}
}

func TestNormalizeLimit_One(t *testing.T) {
	if got := normalizeLimit(1); got != 1 {
		t.Fatalf("normalizeLimit(1) = %d, want 1", got)
	}
}

func TestNormalizeOffset_Zero(t *testing.T) {
	if got := normalizeOffset(0); got != 0 {
		t.Fatalf("normalizeOffset(0) = %d, want 0", got)
	}
}

func TestNormalizeOffset_Negative(t *testing.T) {
	if got := normalizeOffset(-5); got != 0 {
		t.Fatalf("normalizeOffset(-5) = %d, want 0", got)
	}
}

func TestNormalizeOffset_Positive(t *testing.T) {
	if got := normalizeOffset(60); got != 60 {
		t.Fatalf("normalizeOffset(60) = %d, want 60", got)
	}
}

func TestNewRepository_Nil(t *testing.T) {
	repo := NewRepository(nil)
	if repo == nil {
		t.Fatal("NewRepository(nil) should return non-nil")
	}
}

func TestNewService_Nil(t *testing.T) {
	svc := NewService(nil)
	if svc == nil {
		t.Fatal("NewService(nil) should return non-nil")
	}
}
