package masterdata

import (
	"errors"
	"strings"
	"testing"

	mysql "github.com/go-sql-driver/mysql"
)

func TestNormalizeListFilter_ZeroValues(t *testing.T) {
	filter := normalizeListFilter(ListFilter{})
	if filter.Query != "" {
		t.Fatalf("Query should be empty, got %q", filter.Query)
	}
	if filter.Page != 1 {
		t.Fatalf("default Page should be 1, got %d", filter.Page)
	}
	if filter.PageSize != 20 {
		t.Fatalf("default PageSize should be 20, got %d", filter.PageSize)
	}
}

func TestNormalizeListFilter_TrimsQuery(t *testing.T) {
	filter := normalizeListFilter(ListFilter{Query: "  hello  ", Page: 1, PageSize: 10})
	if filter.Query != "hello" {
		t.Fatalf("Query not trimmed: got %q", filter.Query)
	}
}

func TestNormalizeStatus_Empty(t *testing.T) {
	if got := normalizeStatus(""); got != "active" {
		t.Fatalf("normalizeStatus('') = %q, want 'active'", got)
	}
}

func TestNormalizeStatus_Active(t *testing.T) {
	if got := normalizeStatus("active"); got != "active" {
		t.Fatalf("normalizeStatus('active') = %q, want 'active'", got)
	}
}

func TestNormalizeStatus_Inactive(t *testing.T) {
	if got := normalizeStatus("inactive"); got != "inactive" {
		t.Fatalf("normalizeStatus('inactive') = %q, want 'inactive'", got)
	}
}

func TestNormalizeStatus_Whitespace(t *testing.T) {
	if got := normalizeStatus("  inactive  "); got != "inactive" {
		t.Fatalf("normalizeStatus('  inactive  ') = %q, want 'inactive'", got)
	}
}

func TestNormalizeStatus_ArbitraryValue(t *testing.T) {
	if got := normalizeStatus("pending"); got != "active" {
		t.Fatalf("normalizeStatus('pending') = %q, want 'active'", got)
	}
}

func TestBuildSearchClause_Empty(t *testing.T) {
	clause, args := buildSearchClause("")
	if clause != "" {
		t.Fatalf("expected empty clause, got %q", clause)
	}
	if args != nil {
		t.Fatalf("expected nil args, got %v", args)
	}
}

func TestBuildSearchClause_NonEmpty(t *testing.T) {
	clause, args := buildSearchClause("test")
	if !strings.Contains(clause, "WHERE") {
		t.Fatalf("expected clause to contain WHERE, got %q", clause)
	}
	if !strings.Contains(clause, "LIKE") {
		t.Fatalf("expected clause to contain LIKE, got %q", clause)
	}
	if len(args) != 2 {
		t.Fatalf("expected 2 args, got %d", len(args))
	}
	for _, arg := range args {
		s, ok := arg.(string)
		if !ok {
			t.Fatalf("expected string arg, got %T", arg)
		}
		if !strings.Contains(s, "test") {
			t.Fatalf("expected arg to contain 'test', got %q", s)
		}
	}
}

func TestIsDuplicateEntry_MySQL1062(t *testing.T) {
	err := &mysql.MySQLError{Number: 1062, Message: "Duplicate entry"}
	if !isDuplicateEntry(err) {
		t.Fatal("expected true for MySQL 1062")
	}
}

func TestIsDuplicateEntry_OtherError(t *testing.T) {
	if isDuplicateEntry(errors.New("generic")) {
		t.Fatal("expected false for generic error")
	}
}

func TestIsDuplicateEntry_Nil(t *testing.T) {
	if isDuplicateEntry(nil) {
		t.Fatal("expected false for nil")
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
