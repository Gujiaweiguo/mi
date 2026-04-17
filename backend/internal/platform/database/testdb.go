//go:build integration

package database

import (
	"context"
	"database/sql"
	"io/fs"
	"testing"
	"time"

	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

// NewTestDB starts a MySQL 8.0 testcontainer, runs migrations and bootstrap,
// and returns a ready-to-use *sql.DB. It registers t.Cleanup for both the
// container and the database connection.
//
// The migrationsFS parameter should provide the filesystem containing the
// "migrations" directory. Callers typically pass os.DirFS("../platform/database")
// or the appropriate relative path from their test package to the platform/database
// directory.
func NewTestDB(t *testing.T, ctx context.Context, migrationsFS fs.FS) *sql.DB {
	t.Helper()

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
	t.Cleanup(func() { _ = container.Terminate(context.Background()) })

	host, err := container.Host(ctx)
	if err != nil {
		t.Fatalf("resolve mysql host: %v", err)
	}
	port, err := container.MappedPort(ctx, "3306/tcp")
	if err != nil {
		t.Fatalf("resolve mysql port: %v", err)
	}

	db, err := sql.Open("mysql", Config{
		Host:     host,
		Port:     port.Int(),
		Name:     "mi_integration",
		User:     "mi_user",
		Password: "mi_password",
	}.DSN())
	if err != nil {
		t.Fatalf("open mysql connection: %v", err)
	}
	t.Cleanup(func() { _ = db.Close() })

	if err := WaitForDatabase(ctx, db); err != nil {
		t.Fatalf("wait for mysql: %v", err)
	}

	migrator := NewMigrator(db, migrationsFS, "migrations")
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("apply migrations: %v", err)
	}

	bootstrapRunner := NewBootstrapRunner(db, bootstrap.All()...)
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("run bootstrap seeds: %v", err)
	}

	return db
}

// WaitForDatabase retries PingContext until the database is reachable or the
// 30-second deadline expires.
func WaitForDatabase(ctx context.Context, db *sql.DB) error {
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
