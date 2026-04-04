package main

import (
	"context"
	"database/sql"
	"errors"
	"flag"
	"fmt"
	"os"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
	if len(os.Args) < 2 {
		fatalf("usage: dbops <migrate|bootstrap|verify> [flags]")
	}

	cfg, err := config.Load()
	if err != nil {
		fatalf("load config: %v", err)
	}

	db, err := sql.Open("mysql", platformdb.Config{
		Host:     cfg.Database.Host,
		Port:     cfg.Database.Port,
		Name:     cfg.Database.Name,
		User:     cfg.Database.User,
		Password: cfg.Database.Password,
		SSLMode:  cfg.Database.SSLMode,
	}.DSN())
	if err != nil {
		fatalf("open database: %v", err)
	}
	defer db.Close()

	ctx := context.Background()

	switch os.Args[1] {
	case "migrate":
		if err := runMigrate(db); err != nil {
			fatalf("migrate: %v", err)
		}
	case "bootstrap":
		if err := runBootstrap(ctx, db, os.Args[2:]); err != nil {
			fatalf("bootstrap: %v", err)
		}
	case "verify":
		if err := runVerify(db, os.Args[2:]); err != nil {
			fatalf("verify: %v", err)
		}
	default:
		fatalf("unsupported command %q", os.Args[1])
	}
}

func runMigrate(db *sql.DB) error {
	migrator := platformdb.NewMigrator(db, os.DirFS("."), "internal/platform/database/migrations")
	return migrator.ApplyUpMigrations()
}

func runBootstrap(ctx context.Context, db *sql.DB, args []string) error {
	flags := flag.NewFlagSet("bootstrap", flag.ContinueOnError)
	flags.SetOutput(os.Stderr)
	seedSet := flags.String("seed-set", "cutover", "seed set: cutover or all")
	if err := flags.Parse(args); err != nil {
		return err
	}

	var bootstrappers []platformdb.Bootstrapper
	switch *seedSet {
	case "cutover":
		bootstrappers = bootstrap.Cutover()
	case "all":
		bootstrappers = bootstrap.All()
	default:
		return fmt.Errorf("unsupported seed set %q", *seedSet)
	}

	runner := platformdb.NewBootstrapRunner(db, bootstrappers...)
	return runner.Run(ctx)
}

func runVerify(db *sql.DB, args []string) error {
	flags := flag.NewFlagSet("verify", flag.ContinueOnError)
	flags.SetOutput(os.Stderr)
	profile := flags.String("profile", "all", "verification profile: migrate, bootstrap, fresh-start, or all")
	if err := flags.Parse(args); err != nil {
		return err
	}

	verifier := platformdb.NewVerifier(db, os.DirFS("."), "internal/platform/database/verify")

	var files []string
	switch *profile {
	case "migrate":
		files = []string{"internal/platform/database/verify/post_migrate.sql"}
	case "bootstrap":
		files = []string{"internal/platform/database/verify/post_bootstrap.sql"}
	case "fresh-start":
		files = []string{"internal/platform/database/verify/fresh_start.sql"}
	case "all":
		files = []string{
			"internal/platform/database/verify/post_migrate.sql",
			"internal/platform/database/verify/post_bootstrap.sql",
			"internal/platform/database/verify/fresh_start.sql",
		}
	default:
		return fmt.Errorf("unsupported verification profile %q", *profile)
	}

	if len(files) == 0 {
		return errors.New("no verification files selected")
	}

	return verifier.RunFiles(files...)
}

func fatalf(format string, args ...any) {
	_, _ = fmt.Fprintf(os.Stderr, format+"\n", args...)
	os.Exit(1)
}
