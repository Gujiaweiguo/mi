//go:build integration

package database_test

import (
	"context"
	"database/sql"
	"os"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

func TestIntegrationMySQLConnection(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	container, err := testcontainers.GenericContainer(ctx, testcontainers.GenericContainerRequest{
		ContainerRequest: testcontainers.ContainerRequest{
			Image:        "mysql:8.0",
			ExposedPorts: []string{"3306/tcp"},
			Env: map[string]string{
				"MYSQL_DATABASE":      "mi_integration",
				"MYSQL_USER":          "mi_user",
				"MYSQL_PASSWORD":      "mi_password",
				"MYSQL_ROOT_PASSWORD": "mi_root_password",
			},
			WaitingFor: wait.ForListeningPort("3306/tcp").WithStartupTimeout(3 * time.Minute),
		},
		Started: true,
	})
	if err != nil {
		t.Fatalf("start mysql container: %v", err)
	}
	defer func() {
		_ = container.Terminate(context.Background())
	}()

	host, err := container.Host(ctx)
	if err != nil {
		t.Fatalf("resolve mysql host: %v", err)
	}

	port, err := container.MappedPort(ctx, "3306/tcp")
	if err != nil {
		t.Fatalf("resolve mysql port: %v", err)
	}

	db, err := sql.Open("mysql", platformdb.Config{
		Host:     host,
		Port:     port.Int(),
		Name:     "mi_integration",
		User:     "mi_user",
		Password: "mi_password",
	}.DSN())
	if err != nil {
		t.Fatalf("open mysql connection: %v", err)
	}
	defer db.Close()

	if err := waitForDatabase(ctx, db); err != nil {
		t.Fatalf("ping mysql: %v", err)
	}

	var result int
	if err := db.QueryRowContext(ctx, "SELECT 1").Scan(&result); err != nil {
		t.Fatalf("query mysql: %v", err)
	}

	if result != 1 {
		t.Fatalf("expected query result 1, got %d", result)
	}

	migrator := platformdb.NewMigrator(db, os.DirFS("."), "migrations")
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("apply migrations: %v", err)
	}

	verifier := platformdb.NewVerifier(db, os.DirFS("."), "verify")
	if err := verifier.RunFiles("verify/post_migrate.sql"); err != nil {
		t.Fatalf("verify schema after migrations: %v", err)
	}

	bootstrapRunner := platformdb.NewBootstrapRunner(db, toBootstrappers(bootstrap.All())...)
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("run bootstrap seeds: %v", err)
	}
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("rerun bootstrap seeds: %v", err)
	}

	if err := verifier.RunFiles("verify/post_bootstrap.sql"); err != nil {
		t.Fatalf("verify schema after bootstrap: %v", err)
	}

	var userCount int
	if err := db.QueryRowContext(ctx, "SELECT COUNT(*) FROM users").Scan(&userCount); err != nil {
		t.Fatalf("count users: %v", err)
	}
	if userCount < 1 {
		t.Fatalf("expected bootstrap users, got %d", userCount)
	}
	if userCount != 1 {
		t.Fatalf("expected deterministic bootstrap user count 1, got %d", userCount)
	}
}

func TestIntegrationIdempotentMigrations(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	container, err := testcontainers.GenericContainer(ctx, testcontainers.GenericContainerRequest{
		ContainerRequest: testcontainers.ContainerRequest{
			Image:        "mysql:8.0",
			ExposedPorts: []string{"3306/tcp"},
			Env: map[string]string{
				"MYSQL_DATABASE":      "mi_idempotency",
				"MYSQL_USER":          "mi_user",
				"MYSQL_PASSWORD":      "mi_password",
				"MYSQL_ROOT_PASSWORD": "mi_root_password",
			},
			WaitingFor: wait.ForListeningPort("3306/tcp").WithStartupTimeout(3 * time.Minute),
		},
		Started: true,
	})
	if err != nil {
		t.Fatalf("start mysql container: %v", err)
	}
	defer func() {
		_ = container.Terminate(context.Background())
	}()

	host, err := container.Host(ctx)
	if err != nil {
		t.Fatalf("resolve mysql host: %v", err)
	}

	port, err := container.MappedPort(ctx, "3306/tcp")
	if err != nil {
		t.Fatalf("resolve mysql port: %v", err)
	}

	db, err := sql.Open("mysql", platformdb.Config{
		Host:     host,
		Port:     port.Int(),
		Name:     "mi_idempotency",
		User:     "mi_user",
		Password: "mi_password",
	}.DSN())
	if err != nil {
		t.Fatalf("open mysql connection: %v", err)
	}
	defer db.Close()

	if err := waitForDatabase(ctx, db); err != nil {
		t.Fatalf("ping mysql: %v", err)
	}

	migrator := platformdb.NewMigrator(db, os.DirFS("."), "migrations")

	// First application
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("first apply migrations: %v", err)
	}

	// Second application must succeed (idempotent)
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("second apply migrations (idempotent): %v", err)
	}

	// Verify schema_migrations table has entries
	var count int
	if err := db.QueryRowContext(ctx, "SELECT COUNT(*) FROM schema_migrations").Scan(&count); err != nil {
		t.Fatalf("count schema_migrations: %v", err)
	}
	if count == 0 {
		t.Fatal("expected at least one row in schema_migrations")
	}
}

func waitForDatabase(ctx context.Context, db *sql.DB) error {
	deadline := time.Now().Add(30 * time.Second)
	var lastErr error
	for time.Now().Before(deadline) {
		pingCtx, cancel := context.WithTimeout(ctx, 5*time.Second)
		lastErr = db.PingContext(pingCtx)
		cancel()
		if lastErr == nil {
			return nil
		}
		time.Sleep(500 * time.Millisecond)
	}
	return lastErr
}

func toBootstrappers(seeds []bootstrap.Bootstrapper) []platformdb.Bootstrapper {
	out := make([]platformdb.Bootstrapper, len(seeds))
	for i := range seeds {
		out[i] = seeds[i]
	}
	return out
}
