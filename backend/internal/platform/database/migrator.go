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

func (m *Migrator) ensureSchemaMigrationsTable() error {
	_, err := m.db.Exec(`
		CREATE TABLE IF NOT EXISTS schema_migrations (
			version VARCHAR(255) PRIMARY KEY,
			applied_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
		)
	`)
	if err != nil {
		return fmt.Errorf("ensure schema_migrations table: %w", err)
	}
	return nil
}

func (m *Migrator) getAppliedVersions() (map[string]bool, error) {
	rows, err := m.db.Query("SELECT version FROM schema_migrations")
	if err != nil {
		return nil, fmt.Errorf("query applied versions: %w", err)
	}
	defer rows.Close()

	applied := make(map[string]bool)
	for rows.Next() {
		var version string
		if err := rows.Scan(&version); err != nil {
			return nil, fmt.Errorf("scan version: %w", err)
		}
		applied[version] = true
	}
	return applied, rows.Err()
}

func versionName(filePath string) string {
	base := filepath.Base(filePath)
	return strings.TrimSuffix(base, ".up.sql")
}

func (m *Migrator) ApplyUpMigrations() error {
	if err := m.ensureSchemaMigrationsTable(); err != nil {
		return err
	}

	applied, err := m.getAppliedVersions()
	if err != nil {
		return err
	}

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
		version := versionName(path)
		if applied[version] {
			continue
		}

		if err := ExecuteSQLFile(m.db, m.migrationsFS, path); err != nil {
			return fmt.Errorf("apply migration %s: %w", path, err)
		}

		_, err := m.db.Exec("INSERT INTO schema_migrations (version) VALUES (?)", version)
		if err != nil {
			return fmt.Errorf("record migration %s: %w", version, err)
		}
	}

	return nil
}
