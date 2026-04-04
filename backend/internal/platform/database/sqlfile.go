package database

import (
	"database/sql"
	"fmt"
	"io/fs"
	"strings"
)

func ExecuteSQLFile(db *sql.DB, source fs.FS, path string) error {
	contents, err := fs.ReadFile(source, path)
	if err != nil {
		return fmt.Errorf("read sql file %s: %w", path, err)
	}

	parts := splitSQLStatements(string(contents))
	if len(parts) == 0 {
		return nil
	}

	for _, query := range parts {
		if _, err := db.Exec(query); err != nil {
			return fmt.Errorf("execute sql file %s: %w", path, err)
		}
	}

	return nil
}

func splitSQLStatements(content string) []string {
	statement := strings.TrimSpace(content)
	if statement == "" {
		return nil
	}

	parts := strings.Split(statement, ";")
	queries := make([]string, 0, len(parts))
	for _, part := range parts {
		query := strings.TrimSpace(part)
		if query == "" {
			continue
		}
		queries = append(queries, query)
	}

	return queries
}
