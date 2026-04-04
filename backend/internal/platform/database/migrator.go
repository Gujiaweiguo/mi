package database

import (
	"database/sql"
	"fmt"
	"io/fs"
	"path/filepath"
	"sort"
	"strings"
)

type Migrator struct {
	db           *sql.DB
	migrationsFS fs.FS
	basePath     string
}

func NewMigrator(db *sql.DB, migrationsFS fs.FS, basePath string) *Migrator {
	return &Migrator{db: db, migrationsFS: migrationsFS, basePath: basePath}
}

func (m *Migrator) ApplyUpMigrations() error {
	entries, err := fs.ReadDir(m.migrationsFS, m.basePath)
	if err != nil {
		return fmt.Errorf("read migrations directory: %w", err)
	}

	files := make([]string, 0, len(entries))
	for _, entry := range entries {
		if entry.IsDir() {
			continue
		}
		name := entry.Name()
		if strings.HasSuffix(name, ".up.sql") {
			files = append(files, filepath.Join(m.basePath, name))
		}
	}

	sort.Strings(files)

	for _, path := range files {
		if err := ExecuteSQLFile(m.db, m.migrationsFS, path); err != nil {
			return fmt.Errorf("apply migration %s: %w", path, err)
		}
	}

	return nil
}
