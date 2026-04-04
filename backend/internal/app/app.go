package app

import (
	"database/sql"
	"fmt"
	"net/http"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	api "github.com/Gujiaweiguo/mi/backend/internal/http"
	"github.com/Gujiaweiguo/mi/backend/internal/logging"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	_ "github.com/go-sql-driver/mysql"
	"go.uber.org/zap"
)

type App struct {
	config *config.Config
	logger *zap.Logger
	db     *sql.DB
	server *http.Server
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
