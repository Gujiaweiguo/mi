package baseinfo

import (
	"errors"
	"testing"

	mysql "github.com/go-sql-driver/mysql"
)

func strPtr(s string) *string { return &s }

func intPtr(i int) *int { return &i }
func int64Ptr(i int64) *int64 { return &i }
func boolPtr(b bool) *bool { return &b }

func TestTrimStringPointer_Nil(t *testing.T) {
	if got := trimStringPointer(nil); got != nil {
		t.Fatal("trimStringPointer(nil) should return nil")
	}
}

func TestTrimStringPointer_Whitespace(t *testing.T) {
	got := trimStringPointer(strPtr("  hello  "))
	want := "hello"
	if got == nil || *got != want {
		t.Fatalf("trimStringPointer(strPtr(\"  hello  \")) = %v, want %q", got, want)
	}
}

func TestTrimStringPointer_Empty(t *testing.T) {
	if got := trimStringPointer(strPtr("")); got != nil {
		t.Fatalf("trimStringPointer(strPtr(\"\")) = %v, want nil", got)
	}
}

func TestTrimStringPointer_OnlySpaces(t *testing.T) {
	if got := trimStringPointer(strPtr("   ")); got != nil {
		t.Fatalf("trimStringPointer(strPtr(\"   \")) = %v, want nil", got)
	}
}

func TestTrimStringPointer_NoTrim(t *testing.T) {
	got := trimStringPointer(strPtr("hello"))
	if got == nil || *got != "hello" {
		t.Fatalf("trimStringPointer(strPtr(\"hello\")) = %v, want \"hello\"", got)
	}
}

func TestIsDuplicateEntry_MySQL1062(t *testing.T) {
	err := &mysql.MySQLError{Number: 1062, Message: "Duplicate entry"}
	if !isDuplicateEntry(err) {
		t.Fatal("expected isDuplicateEntry to return true for MySQL 1062 error")
	}
}

func TestIsDuplicateEntry_OtherMySQLError(t *testing.T) {
	err := &mysql.MySQLError{Number: 1045, Message: "Access denied"}
	if isDuplicateEntry(err) {
		t.Fatal("expected isDuplicateEntry to return false for non-1062 MySQL error")
	}
}

func TestIsDuplicateEntry_GenericError(t *testing.T) {
	if isDuplicateEntry(errors.New("some error")) {
		t.Fatal("expected isDuplicateEntry to return false for generic error")
	}
}

func TestIsDuplicateEntry_Nil(t *testing.T) {
	if isDuplicateEntry(nil) {
		t.Fatal("expected isDuplicateEntry to return false for nil")
	}
}

func TestNormalizeInput_BasicTrimming(t *testing.T) {
	input := CatalogInput{
		Code: "  ABC  ",
		Name: "  Test Name  ",
	}
	config := entityConfig{table: "test", orderBy: "id"}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Code != "ABC" {
		t.Fatalf("Code not trimmed: got %q", result.Code)
	}
	if result.Name != "Test Name" {
		t.Fatalf("Name not trimmed: got %q", result.Name)
	}
}

func TestNormalizeInput_EmptyCode(t *testing.T) {
	input := CatalogInput{Code: "", Name: "Test"}
	config := entityConfig{table: "test", orderBy: "id"}
	_, err := normalizeInput(config, input)
	if err != ErrInvalidBaseInfo {
		t.Fatalf("expected ErrInvalidBaseInfo, got %v", err)
	}
}

func TestNormalizeInput_EmptyName(t *testing.T) {
	input := CatalogInput{Code: "ABC", Name: ""}
	config := entityConfig{table: "test", orderBy: "id"}
	_, err := normalizeInput(config, input)
	if err != ErrInvalidBaseInfo {
		t.Fatalf("expected ErrInvalidBaseInfo, got %v", err)
	}
}

func TestNormalizeInput_StatusDefault(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", Status: ""}
	config := entityConfig{table: "test", orderBy: "id", hasStatus: true}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Status != "active" {
		t.Fatalf("default status should be 'active', got %q", result.Status)
	}
}

func TestNormalizeInput_StatusPreserved(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", Status: "inactive"}
	config := entityConfig{table: "test", orderBy: "id", hasStatus: true}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Status != "inactive" {
		t.Fatalf("status should be 'inactive', got %q", result.Status)
	}
}

func TestNormalizeInput_StatusClearedWhenNotHasStatus(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", Status: "active"}
	config := entityConfig{table: "test", orderBy: "id", hasStatus: false}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Status != "" {
		t.Fatalf("status should be empty when hasStatus is false, got %q", result.Status)
	}
}

func TestNormalizeInput_ColorTrimmed(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", ColorHex: strPtr("  #FF0000  ")}
	config := entityConfig{table: "test", orderBy: "id", hasColor: true}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.ColorHex == nil || *result.ColorHex != "#FF0000" {
		t.Fatalf("ColorHex not trimmed: got %v", result.ColorHex)
	}
}

func TestNormalizeInput_ColorClearedWhenNotHasColor(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", ColorHex: strPtr("#FF0000")}
	config := entityConfig{table: "test", orderBy: "id", hasColor: false}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.ColorHex != nil {
		t.Fatalf("ColorHex should be nil when hasColor is false, got %v", result.ColorHex)
	}
}

func TestNormalizeInput_InvalidParentID(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", ParentID: int64Ptr(0)}
	config := entityConfig{table: "test", orderBy: "id", hasParent: true}
	_, err := normalizeInput(config, input)
	if err != ErrInvalidBaseInfo {
		t.Fatalf("expected ErrInvalidBaseInfo for ParentID <= 0, got %v", err)
	}
}

func TestNormalizeInput_NegativeParentID(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", ParentID: int64Ptr(-1)}
	config := entityConfig{table: "test", orderBy: "id", hasParent: true}
	_, err := normalizeInput(config, input)
	if err != ErrInvalidBaseInfo {
		t.Fatalf("expected ErrInvalidBaseInfo for negative ParentID, got %v", err)
	}
}

func TestNormalizeInput_LevelDefault(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B"}
	config := entityConfig{table: "test", orderBy: "id", hasLevel: true}
	result, err := normalizeInput(config, input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if result.Level == nil || *result.Level != 1 {
		t.Fatalf("default level should be 1, got %v", result.Level)
	}
}

func TestNormalizeInput_InvalidLevel(t *testing.T) {
	input := CatalogInput{Code: "A", Name: "B", Level: intPtr(0)}
	config := entityConfig{table: "test", orderBy: "id", hasLevel: true}
	_, err := normalizeInput(config, input)
	if err != ErrInvalidBaseInfo {
		t.Fatalf("expected ErrInvalidBaseInfo for level < 1, got %v", err)
	}
}

func TestNewService_Nil(t *testing.T) {
	svc := NewService(nil)
	if svc == nil {
		t.Fatal("NewService(nil) should return non-nil")
	}
}

func TestNewRepository_Nil(t *testing.T) {
	repo := NewRepository(nil)
	if repo == nil {
		t.Fatal("NewRepository(nil) should return non-nil")
	}
}
