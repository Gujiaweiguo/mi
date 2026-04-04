package database

import (
	"database/sql"
	"fmt"
	"io/fs"
	"path/filepath"
	"sort"
	"strconv"
	"strings"
)

type Verifier struct {
	db         *sql.DB
	verifyFS   fs.FS
	basePath   string
	fileSuffix string
}

func NewVerifier(db *sql.DB, verifyFS fs.FS, basePath string) *Verifier {
	return &Verifier{db: db, verifyFS: verifyFS, basePath: basePath, fileSuffix: ".sql"}
}

func (v *Verifier) RunAll() error {
	entries, err := fs.ReadDir(v.verifyFS, v.basePath)
	if err != nil {
		return fmt.Errorf("read verify directory: %w", err)
	}

	files := make([]string, 0, len(entries))
	for _, entry := range entries {
		if entry.IsDir() {
			continue
		}

		name := entry.Name()
		if strings.HasSuffix(name, v.fileSuffix) {
			files = append(files, filepath.Join(v.basePath, name))
		}
	}

	sort.Strings(files)
	return v.RunFiles(files...)
}

func (v *Verifier) RunFiles(files ...string) error {
	for _, path := range files {
		contents, err := fs.ReadFile(v.verifyFS, path)
		if err != nil {
			return fmt.Errorf("read verify file %s: %w", path, err)
		}

		statements := splitSQLStatements(string(contents))
		for _, statement := range statements {
			row := v.db.QueryRow(statement)
			var result any
			if err := row.Scan(&result); err != nil {
				return fmt.Errorf("verify statement in %s: %w", path, err)
			}

			passed, err := normalizeVerificationResult(result)
			if err != nil {
				return fmt.Errorf("verify statement in %s: %w", path, err)
			}
			if !passed {
				return fmt.Errorf("verification failed for %s", path)
			}
		}
	}

	return nil
}

func normalizeVerificationResult(value any) (bool, error) {
	switch typed := value.(type) {
	case int64:
		return typed != 0, nil
	case []byte:
		parsed, err := strconv.ParseInt(string(typed), 10, 64)
		if err != nil {
			return false, fmt.Errorf("parse verification result: %w", err)
		}
		return parsed != 0, nil
	case string:
		parsed, err := strconv.ParseInt(typed, 10, 64)
		if err != nil {
			return false, fmt.Errorf("parse verification result: %w", err)
		}
		return parsed != 0, nil
	default:
		return false, fmt.Errorf("unsupported verification result type %T", value)
	}
}
