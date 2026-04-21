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

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/config"
	api "github.com/Gujiaweiguo/mi/backend/internal/http"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/logging"
	"github.com/Gujiaweiguo/mi/backend/internal/notification"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"go.uber.org/zap"
)

type App struct {
	config              *config.Config
	logger              *zap.Logger
	db                  *sql.DB
	server              *http.Server
	notificationService *notification.Service
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

const reminderSchedulerFailureWarnThreshold = 3

type workflowReminderSchedulerRunOutcome struct {
	LockAcquired bool
	Emitted      int
	Skipped      int
}

type workflowReminderSchedulerSnapshot struct {
	TotalRuns           int64
	SuccessfulRuns      int64
	FailedRuns          int64
	LockSkippedRuns     int64
	ConsecutiveFailures int64
	LastRunAt           time.Time
	LastSuccessAt       time.Time
	LastFailureAt       time.Time
	LastLockSkipAt      time.Time
	LastRunDurationMs   int64
	LastEmitted         int
	LastSkipped         int
	LastError           string
}

type workflowReminderSchedulerObservability struct {
	mu       sync.Mutex
	snapshot workflowReminderSchedulerSnapshot
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

	db.SetMaxOpenConns(25)
	db.SetMaxIdleConns(10)
	db.SetConnMaxLifetime(5 * time.Minute)
	db.SetConnMaxIdleTime(1 * time.Minute)

	router := api.NewRouter(cfg, db, logger)
	var notificationService *notification.Service
	if cfg.Email.Enabled {
		renderer, err := notification.NewRenderer(cfg.Email.TemplateDir)
		if err != nil {
			return nil, err
		}
		notificationService = notification.NewService(
			db,
			notification.NewRepository(db),
			notification.NewSMTPSender(cfg.Email, logger),
			renderer,
			cfg.Email,
			logger,
		)
	}

	server := &http.Server{
		Addr:              fmt.Sprintf("%s:%d", cfg.Server.Host, cfg.Server.Port),
		Handler:           router,
		ReadHeaderTimeout: 5 * time.Second,
		ReadTimeout:       time.Duration(cfg.Server.ReadTimeoutSeconds) * time.Second,
		WriteTimeout:      time.Duration(cfg.Server.WriteTimeoutSeconds) * time.Second,
	}

	return &App{config: cfg, logger: logger, db: db, server: server, notificationService: notificationService}, nil
}

func (a *App) Run(ctx context.Context) error {
	stopReminderScheduler := a.startWorkflowReminderScheduler()
	stopNotificationPoller := a.startNotificationPoller()

	a.logger.Sugar().Infow(
		"starting backend service",
		"environment", a.config.App.Environment,
		"address", a.server.Addr,
	)

	serverErr := make(chan error, 1)
	go func() {
		if err := a.server.ListenAndServe(); err != nil && err != http.ErrServerClosed {
			serverErr <- err
		}
		close(serverErr)
	}()

	select {
	case <-ctx.Done():
		a.logger.Sugar().Infow("received shutdown signal")
	case err := <-serverErr:
		stopReminderScheduler()
		stopNotificationPoller()
		return err
	}

	stopReminderScheduler()
	stopNotificationPoller()

	shutdownCtx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	return a.Shutdown(shutdownCtx)
}

func (a *App) Shutdown(ctx context.Context) error {
	a.logger.Sugar().Infow("shutting down backend service")

	if err := a.server.Shutdown(ctx); err != nil {
		a.logger.Sugar().Errorw("server shutdown error", "error", err)
		return err
	}

	if err := a.db.Close(); err != nil {
		a.logger.Sugar().Errorw("database close error", "error", err)
		return err
	}

	a.logger.Sugar().Infow("backend service stopped")
	return nil
}

func (a *App) Logger() *zap.Logger {
	return a.logger
}

func (a *App) startNotificationPoller() func() {
	if a.notificationService == nil {
		a.logger.Sugar().Infow("notification poller disabled")
		return func() {}
	}
	interval := time.Duration(a.config.Email.PollIntervalSeconds) * time.Second
	if interval <= 0 {
		interval = time.Minute
	}
	notifier := notification.NewServiceNotifier(a.notificationService)
	invoiceService := invoice.NewService(a.db, invoice.NewRepository(a.db), billing.NewRepository(a.db), nil, notifier)
	leaseService := lease.NewService(a.db, lease.NewRepository(a.db), nil, notifier)
	pollerCtx, cancel := context.WithCancel(context.Background())
	stopCh := make(chan struct{})
	doneCh := make(chan struct{})
	var stopOnce sync.Once
	var runMutex sync.Mutex

	a.logger.Sugar().Infow("notification poller enabled", "interval_seconds", int(interval.Seconds()))

	go func() {
		ticker := time.NewTicker(interval)
		defer ticker.Stop()
		defer close(doneCh)
		for {
			select {
			case <-ticker.C:
				if !tryLockMutex(&runMutex) {
					a.logger.Sugar().Warnw("skip notification poll tick because previous run is still in progress")
					continue
				}
				runErr := func() error {
					defer runMutex.Unlock()
					runCtx, cancelRun := context.WithTimeout(context.Background(), interval)
					defer cancelRun()
					if err := invoiceService.CheckPaymentReminders(runCtx); err != nil {
						return err
					}
					if err := leaseService.CheckExpirationReminders(runCtx); err != nil {
						return err
					}
					return a.notificationService.ProcessOutbox(runCtx)
				}()
				if runErr != nil {
					a.logger.Sugar().Errorw("notification poll cycle failed", "error", runErr)
					continue
				}
				a.logger.Sugar().Infow("notification poll cycle completed")
			case <-pollerCtx.Done():
				a.logger.Sugar().Infow("notification poller stopping")
				return
			case <-stopCh:
				return
			}
		}
	}()

	return func() {
		stopOnce.Do(func() {
			cancel()
			close(stopCh)
			<-doneCh
			a.logger.Sugar().Infow("notification poller stopped")
		})
	}
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
	observability := newWorkflowReminderSchedulerObservability()

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
				outcome, err := runWorkflowReminderSchedulerTick(runContext, locker, workflowService, runAt, schedulerConfig)
				duration := time.Since(runAt)
				cancel()
				runMutex.Unlock()
				if err != nil {
					snapshot := observability.recordFailure(runAt, duration, err)
					a.logger.Sugar().Errorw("workflow reminder scheduled run failed",
						"error", err,
						"run_at", snapshot.LastRunAt,
						"duration_ms", snapshot.LastRunDurationMs,
						"total_runs", snapshot.TotalRuns,
						"successful_runs", snapshot.SuccessfulRuns,
						"failed_runs", snapshot.FailedRuns,
						"lock_skipped_runs", snapshot.LockSkippedRuns,
						"consecutive_failures", snapshot.ConsecutiveFailures,
					)
					if snapshot.ConsecutiveFailures >= reminderSchedulerFailureWarnThreshold {
						a.logger.Sugar().Warnw("workflow reminder scheduler consecutive failures threshold reached",
							"threshold", reminderSchedulerFailureWarnThreshold,
							"consecutive_failures", snapshot.ConsecutiveFailures,
							"last_error", snapshot.LastError,
						)
					}
					continue
				}

				if !outcome.LockAcquired {
					snapshot := observability.recordLockSkip(runAt, duration)
					a.logger.Sugar().Infow("workflow reminder scheduled run skipped by distributed lock",
						"lock_name", schedulerConfig.LockName,
						"run_at", snapshot.LastRunAt,
						"duration_ms", snapshot.LastRunDurationMs,
						"total_runs", snapshot.TotalRuns,
						"successful_runs", snapshot.SuccessfulRuns,
						"failed_runs", snapshot.FailedRuns,
						"lock_skipped_runs", snapshot.LockSkippedRuns,
						"consecutive_failures", snapshot.ConsecutiveFailures,
					)
					continue
				}

				snapshot := observability.recordSuccess(runAt, duration, outcome.Emitted, outcome.Skipped)
				a.logger.Sugar().Infow("workflow reminder scheduled run completed",
					"run_at", snapshot.LastRunAt,
					"duration_ms", snapshot.LastRunDurationMs,
					"emitted", snapshot.LastEmitted,
					"skipped", snapshot.LastSkipped,
					"total_runs", snapshot.TotalRuns,
					"successful_runs", snapshot.SuccessfulRuns,
					"failed_runs", snapshot.FailedRuns,
					"lock_skipped_runs", snapshot.LockSkippedRuns,
					"consecutive_failures", snapshot.ConsecutiveFailures,
				)
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

func runWorkflowReminderSchedulerOnce(ctx context.Context, service workflowReminderSchedulerService, now time.Time, schedulerConfig config.WorkflowReminderSchedulerConfig) (int, int, error) {
	reminderConfig := workflow.ReminderConfig{
		ReminderType:     schedulerConfig.ReminderType,
		MinPendingAge:    time.Duration(schedulerConfig.MinPendingAgeSeconds) * time.Second,
		WindowTruncation: time.Duration(schedulerConfig.WindowTruncationSeconds) * time.Second,
	}

	records, err := service.RunReminders(ctx, now, reminderConfig)
	if err != nil {
		return 0, 0, err
	}

	emitted, skipped := summarizeReminderOutcomes(records)
	return emitted, skipped, nil
}

func runWorkflowReminderSchedulerTick(ctx context.Context, locker workflowReminderDistributedLocker, service workflowReminderSchedulerService, now time.Time, schedulerConfig config.WorkflowReminderSchedulerConfig) (workflowReminderSchedulerRunOutcome, error) {
	outcome := workflowReminderSchedulerRunOutcome{LockAcquired: false}
	acquired, err := locker.WithLock(ctx, schedulerConfig.LockName, schedulerConfig.LockWaitSeconds, func(lockContext context.Context) error {
		emitted, skipped, runErr := runWorkflowReminderSchedulerOnce(lockContext, service, now, schedulerConfig)
		if runErr != nil {
			return runErr
		}
		outcome.LockAcquired = true
		outcome.Emitted = emitted
		outcome.Skipped = skipped
		return nil
	})
	if err != nil {
		return workflowReminderSchedulerRunOutcome{}, err
	}
	if !acquired {
		return outcome, nil
	}
	outcome.LockAcquired = true
	return outcome, nil
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
	return mu.TryLock()
}

func newWorkflowReminderSchedulerObservability() *workflowReminderSchedulerObservability {
	return &workflowReminderSchedulerObservability{}
}

func (o *workflowReminderSchedulerObservability) recordSuccess(runAt time.Time, duration time.Duration, emitted int, skipped int) workflowReminderSchedulerSnapshot {
	o.mu.Lock()
	defer o.mu.Unlock()

	o.snapshot.TotalRuns++
	o.snapshot.SuccessfulRuns++
	o.snapshot.ConsecutiveFailures = 0
	o.snapshot.LastRunAt = runAt
	o.snapshot.LastSuccessAt = runAt
	o.snapshot.LastRunDurationMs = durationToMilliseconds(duration)
	o.snapshot.LastEmitted = emitted
	o.snapshot.LastSkipped = skipped
	o.snapshot.LastError = ""
	return o.snapshot
}

func (o *workflowReminderSchedulerObservability) recordFailure(runAt time.Time, duration time.Duration, err error) workflowReminderSchedulerSnapshot {
	o.mu.Lock()
	defer o.mu.Unlock()

	o.snapshot.TotalRuns++
	o.snapshot.FailedRuns++
	o.snapshot.ConsecutiveFailures++
	o.snapshot.LastRunAt = runAt
	o.snapshot.LastFailureAt = runAt
	o.snapshot.LastRunDurationMs = durationToMilliseconds(duration)
	o.snapshot.LastError = err.Error()
	o.snapshot.LastEmitted = 0
	o.snapshot.LastSkipped = 0
	return o.snapshot
}

func (o *workflowReminderSchedulerObservability) recordLockSkip(runAt time.Time, duration time.Duration) workflowReminderSchedulerSnapshot {
	o.mu.Lock()
	defer o.mu.Unlock()

	o.snapshot.TotalRuns++
	o.snapshot.LockSkippedRuns++
	o.snapshot.LastRunAt = runAt
	o.snapshot.LastLockSkipAt = runAt
	o.snapshot.LastRunDurationMs = durationToMilliseconds(duration)
	o.snapshot.LastEmitted = 0
	o.snapshot.LastSkipped = 0
	o.snapshot.LastError = ""
	return o.snapshot
}

func durationToMilliseconds(duration time.Duration) int64 {
	return duration.Milliseconds()
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

	var runErr error
	if fn != nil {
		runErr = fn(ctx)
	}
	releaseErr := releaseMySQLSchedulerLock(ctx, conn, lockName)
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
