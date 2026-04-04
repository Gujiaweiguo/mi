package database

import (
	"context"
	"database/sql"
	"fmt"
)

type Bootstrapper interface {
	Name() string
	Seed(context.Context, *sql.Tx) error
}

type BootstrapRunner struct {
	db            *sql.DB
	bootstrappers []Bootstrapper
}

func NewBootstrapRunner(db *sql.DB, bootstrappers ...Bootstrapper) *BootstrapRunner {
	return &BootstrapRunner{db: db, bootstrappers: bootstrappers}
}

func (r *BootstrapRunner) Run(ctx context.Context) error {
	for _, bootstrapper := range r.bootstrappers {
		tx, err := r.db.BeginTx(ctx, nil)
		if err != nil {
			return fmt.Errorf("begin bootstrap transaction for %s: %w", bootstrapper.Name(), err)
		}

		if err := bootstrapper.Seed(ctx, tx); err != nil {
			_ = tx.Rollback()
			return fmt.Errorf("seed %s: %w", bootstrapper.Name(), err)
		}

		if err := tx.Commit(); err != nil {
			return fmt.Errorf("commit bootstrap transaction for %s: %w", bootstrapper.Name(), err)
		}
	}

	return nil
}
