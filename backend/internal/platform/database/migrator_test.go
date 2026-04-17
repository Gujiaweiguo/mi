package database

import "testing"

func TestVersionName(t *testing.T) {
	tests := []struct {
		input    string
		expected string
	}{
		{
			input:    "migrations/000001_auth_org_bootstrap_schema.up.sql",
			expected: "000001_auth_org_bootstrap_schema",
		},
		{
			input:    "migrations/000002_some_other_migration.up.sql",
			expected: "000002_some_other_migration",
		},
		{
			input:    "000003_short.up.sql",
			expected: "000003_short",
		},
		{
			input:    "migrations/deeply/nested/000004_nested.up.sql",
			expected: "000004_nested",
		},
	}

	for _, tc := range tests {
		t.Run(tc.input, func(t *testing.T) {
			got := versionName(tc.input)
			if got != tc.expected {
				t.Fatalf("versionName(%q) = %q, want %q", tc.input, got, tc.expected)
			}
		})
	}
}
