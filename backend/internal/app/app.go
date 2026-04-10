package app

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"net/http"
	"strings"
	"sync"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	api "github.com/Gujiaweiguo/mi/backend/internal/http"
	"github.com/Gujiaweiguo/mi/backend/internal/logging"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"go.uber.org/zap"
)

type App struct {
	config *config.Config
	logger *zap.Logger
	db     *sql.DB
	server *http.Server
}

type workflowReminderSchedulerService interface {
	RunReminders(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error)
}

type workflowReminderDistributedLocker interface {
	WithLock(ctx context.Context, lockName string, waitSeconds int, fn func(ctx context.Context) error) (bool, error)
}

type mysqlWorkflowReminderDistributedLocker struct {
	db *sql.DB
}

func New() (*App, error) {
	cfg, err := config.Load()
	if err != nil {
		return nil, err
	}

	logger, err := logging.New(cfg.Log.Level)
	if err != nil {
		return nil, err
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
		return nil, fmt.Errorf("open database connection: %w", err)
	}

	router := api.NewRouter(cfg, db)

	server := &http.Server{
		Addr:              fmt.Sprintf("%s:%d", cfg.Server.Host, cfg.Server.Port),
		Handler:           router,
		ReadHeaderTimeout: 5 * time.Second,
		ReadTimeout:       time.Duration(cfg.Server.ReadTimeoutSeconds) * time.Second,
		WriteTimeout:      time.Duration(cfg.Server.WriteTimeoutSeconds) * time.Second,
	}

	return &App{config: cfg, logger: logger, db: db, server: server}, nil
}

func (a *App) Run() error {
	stopReminderScheduler := a.startWorkflowReminderScheduler()
	defer stopReminderScheduler()

	a.logger.Sugar().Infow(
		"starting backend service",
		"environment", a.config.App.Environment,
		"address", a.server.Addr,
	)

	return a.server.ListenAndServe()
}

func (a *App) Logger() *zap.Logger {
	return a.logger
}

func (a *App) startWorkflowReminderScheduler() func() {
	schedulerConfig := normalizeWorkflowReminderSchedulerConfig(a.config.WorkflowReminderScheduler)
	if !schedulerConfig.Enabled {
		a.logger.Sugar().Infow("workflow reminder scheduler disabled")
		return func() {}
	}

	workflowService := workflow.NewService(a.db, workflow.NewRepository(a.db))
	locker := newMySQLWorkflowReminderDistributedLocker(a.db)
	interval := time.Duration(schedulerConfig.IntervalSeconds) * time.Second
	ticker := time.NewTicker(interval)
	stopCh := make(chan struct{})
	var stopOnce sync.Once
	var runMutex sync.Mutex

	a.logger.Sugar().Infow("workflow reminder scheduler enabled",
		"interval_seconds", schedulerConfig.IntervalSeconds,
		"lock_name", schedulerConfig.LockName,
		"lock_wait_seconds", schedulerConfig.LockWaitSeconds,
		"reminder_type", schedulerConfig.ReminderType,
		"min_pending_age_seconds", schedulerConfig.MinPendingAgeSeconds,
		"window_truncation_seconds", schedulerConfig.WindowTruncationSeconds,
	)

	go func() {
		for {
			select {
			case <-ticker.C:
				if !tryLockMutex(&runMutex) {
					a.logger.Sugar().Warnw("skip workflow reminder schedule tick because previous run is still in progress")
					continue
				}

				runAt := time.Now().UTC()
				runContext, cancel := context.WithTimeout(context.Background(), interval)
				err := runWorkflowReminderSchedulerTick(runContext, a.logger, locker, workflowService, runAt, schedulerConfig)
				cancel()
				runMutex.Unlock()
				if err != nil {
					a.logger.Sugar().Errorw("workflow reminder scheduled run failed", "error", err)
				}
			case <-stopCh:
				ticker.Stop()
				return
			}
		}
	}()

	return func() {
		stopOnce.Do(func() { close(stopCh) })
	}
}

func runWorkflowReminderSchedulerOnce(ctx context.Context, logger *zap.Logger, service workflowReminderSchedulerService, now time.Time, schedulerConfig config.WorkflowReminderSchedulerConfig) error {
	reminderConfig := workflow.ReminderConfig{
		ReminderType:     schedulerConfig.ReminderType,
		MinPendingAge:    time.Duration(schedulerConfig.MinPendingAgeSeconds) * time.Second,
		WindowTruncation: time.Duration(schedulerConfig.WindowTruncationSeconds) * time.Second,
	}

	records, err := service.RunReminders(ctx, now, reminderConfig)
	if err != nil {
		return err
	}

	emitted, skipped := summarizeReminderOutcomes(records)
	logger.Sugar().Infow("workflow reminder scheduled run completed",
		"run_at", now,
		"records", len(records),
		"emitted", emitted,
		"skipped", skipped,
	)
	return nil
}

func runWorkflowReminderSchedulerTick(ctx context.Context, logger *zap.Logger, locker workflowReminderDistributedLocker, service workflowReminderSchedulerService, now time.Time, schedulerConfig config.WorkflowReminderSchedulerConfig) error {
	acquired, err := locker.WithLock(ctx, schedulerConfig.LockName, schedulerConfig.LockWaitSeconds, func(lockContext context.Context) error {
		return runWorkflowReminderSchedulerOnce(lockContext, logger, service, now, schedulerConfig)
	})
	if err != nil {
		return err
	}
	if !acquired {
		logger.Sugar().Infow("skip workflow reminder schedule tick because distributed lock not acquired",
			"lock_name", schedulerConfig.LockName,
		)
	}
	return nil
}

func normalizeWorkflowReminderSchedulerConfig(schedulerConfig config.WorkflowReminderSchedulerConfig) config.WorkflowReminderSchedulerConfig {
	if schedulerConfig.IntervalSeconds <= 0 {
		schedulerConfig.IntervalSeconds = 3600
	}
	if strings.TrimSpace(schedulerConfig.LockName) == "" {
		schedulerConfig.LockName = "workflow:reminder:scheduler"
	}
	if schedulerConfig.LockWaitSeconds < 0 {
		schedulerConfig.LockWaitSeconds = 0
	}
	if strings.TrimSpace(schedulerConfig.ReminderType) == "" {
		schedulerConfig.ReminderType = "standard"
	}
	if schedulerConfig.MinPendingAgeSeconds < 0 {
		schedulerConfig.MinPendingAgeSeconds = 0
	}
	if schedulerConfig.WindowTruncationSeconds <= 0 {
		schedulerConfig.WindowTruncationSeconds = 86400
	}
	return schedulerConfig
}

func summarizeReminderOutcomes(records []workflow.ReminderAuditRecord) (int, int) {
	emitted := 0
	skipped := 0
	for _, record := range records {
		if record.Outcome == workflow.ReminderOutcomeEmitted {
			emitted++
			continue
		}
		if record.Outcome == workflow.ReminderOutcomeSkipped {
			skipped++
		}
	}
	return emitted, skipped
}

func tryLockMutex(mu *sync.Mutex) bool {
	if !mu.TryLock() {
		return false
	}
	return true
}

func newMySQLWorkflowReminderDistributedLocker(db *sql.DB) workflowReminderDistributedLocker {
	return &mysqlWorkflowReminderDistributedLocker{db: db}
}

func (l *mysqlWorkflowReminderDistributedLocker) WithLock(ctx context.Context, lockName string, waitSeconds int, fn func(ctx context.Context) error) (bool, error) {
	conn, err := l.db.Conn(ctx)
	if err != nil {
		return false, fmt.Errorf("open db connection for scheduler lock: %w", err)
	}
	defer conn.Close()

	var acquired sql.NullInt64
	if err := conn.QueryRowContext(ctx, "SELECT GET_LOCK(?, ?)", lockName, waitSeconds).Scan(&acquired); err != nil {
		return false, fmt.Errorf("acquire scheduler lock %q: %w", lockName, err)
	}
	if !acquired.Valid || acquired.Int64 != 1 {
		return false, nil
	}

	releaseErr := releaseMySQLSchedulerLock(ctx, conn, lockName)
	if fn == nil {
		return true, releaseErr
	}

	runErr := fn(ctx)
	if runErr != nil || releaseErr != nil {
		return true, errors.Join(runErr, releaseErr)
	}
	return true, nil
}

func releaseMySQLSchedulerLock(ctx context.Context, conn *sql.Conn, lockName string) error {
	var released sql.NullInt64
	if err := conn.QueryRowContext(ctx, "SELECT RELEASE_LOCK(?)", lockName).Scan(&released); err != nil {
		return fmt.Errorf("release scheduler lock %q: %w", lockName, err)
	}
	if !released.Valid {
		return fmt.Errorf("release scheduler lock %q returned NULL", lockName)
	}
	if released.Int64 != 1 {
		return fmt.Errorf("release scheduler lock %q returned unexpected status %d", lockName, released.Int64)
	}
	return nil
}
