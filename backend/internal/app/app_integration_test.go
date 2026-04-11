//go:build integration

package app

import (
	"context"
	"database/sql"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

func TestMySQLWorkflowReminderDistributedLockerHoldsLockUntilFunctionReturns(t *testing.T) {
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
	defer func() { _ = container.Terminate(context.Background()) }()

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

	locker := newMySQLWorkflowReminderDistributedLocker(db)
	lockName := "workflow:reminder:scheduler:test"

	acquired, err := locker.WithLock(ctx, lockName, 0, func(lockCtx context.Context) error {
		secondRan := false
		acquiredAgain, lockErr := locker.WithLock(lockCtx, lockName, 0, func(context.Context) error {
			secondRan = true
			return nil
		})
		if lockErr != nil {
			return lockErr
		}
		if acquiredAgain {
			t.Fatal("expected second lock attempt to be skipped while first lock is held")
		}
		if secondRan {
			t.Fatal("expected second lock callback not to run while first lock is held")
		}
		return nil
	})
	if err != nil {
		t.Fatalf("first lock attempt failed: %v", err)
	}
	if !acquired {
		t.Fatal("expected first lock attempt to acquire lock")
	}

	secondRan := false
	acquiredAfterRelease, err := locker.WithLock(ctx, lockName, 0, func(context.Context) error {
		secondRan = true
		return nil
	})
	if err != nil {
		t.Fatalf("second lock attempt after release failed: %v", err)
	}
	if !acquiredAfterRelease {
		t.Fatal("expected lock acquisition after first callback returned")
	}
	if !secondRan {
		t.Fatal("expected callback to run after lock was released")
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
