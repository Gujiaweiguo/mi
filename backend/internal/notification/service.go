package notification

import (
	"context"
	"database/sql"
	"encoding/json"
	"errors"
	"fmt"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"go.uber.org/zap"
)

type serviceRepository interface {
	InsertOutbox(ctx context.Context, tx *sql.Tx, entry *OutboxEntry) error
	FetchPending(ctx context.Context, db *sql.DB, batchSize int) ([]*OutboxEntry, error)
	UpdateStatus(ctx context.Context, entry *OutboxEntry) error
}

// Service coordinates notification enqueueing and delivery.
type Service struct {
	repository serviceRepository
	sender     Sender
	renderer   *Renderer
	db         *sql.DB
	config     config.EmailConfig
	logger     *zap.Logger
	now        func() time.Time
}

// NewService constructs a notification service.
func NewService(db *sql.DB, repository *Repository, sender Sender, renderer *Renderer, cfg config.EmailConfig, logger *zap.Logger) *Service {
	return &Service{
		repository: repository,
		sender:     sender,
		renderer:   renderer,
		db:         db,
		config:     cfg,
		logger:     logger,
		now:        time.Now().UTC,
	}
}

// NewServiceWithNowFunc constructs a notification service with a custom clock.
func NewServiceWithNowFunc(db *sql.DB, repository *Repository, sender Sender, renderer *Renderer, cfg config.EmailConfig, logger *zap.Logger, now func() time.Time) *Service {
	svc := NewService(db, repository, sender, renderer, cfg, logger)
	if now != nil {
		svc.now = now
	}
	return svc
}

// Enqueue validates and inserts a notification outbox entry inside the caller transaction.
func (s *Service) Enqueue(ctx context.Context, tx *sql.Tx, event NotificationEvent) error {
	if tx == nil {
		return errors.New("enqueue notification: nil transaction")
	}
	if s == nil {
		return nil
	}
	if s.repository == nil {
		return errors.New("enqueue notification: repository is required")
	}
	if s.renderer == nil {
		return errors.New("enqueue notification: renderer is required")
	}
	if len(event.RecipientTo) == 0 {
		return errors.New("enqueue notification: at least one recipient is required")
	}

	subject, err := s.renderer.RenderSubjectWithData(event.TemplateName, event.TemplateData)
	if err != nil {
		return err
	}
	if strings.TrimSpace(event.Subject) != "" {
		subject = strings.TrimSpace(event.Subject)
	}
	if _, err := s.renderer.RenderBody(event.TemplateName, event.TemplateData); err != nil {
		return err
	}

	templateData, err := json.Marshal(event.TemplateData)
	if err != nil {
		return fmt.Errorf("marshal notification template data: %w", err)
	}

	entry := &OutboxEntry{
		EventType:     strings.TrimSpace(event.EventType),
		AggregateType: strings.TrimSpace(event.AggregateType),
		AggregateID:   event.AggregateID,
		RecipientTo:   joinRecipients(event.RecipientTo),
		RecipientCc:   joinRecipients(event.RecipientCc),
		Subject:       subject,
		TemplateName:  strings.TrimSpace(event.TemplateName),
		TemplateData:  templateData,
		Status:        StatusPending,
		AttemptCount:  0,
		MaxAttempts:   maxRetryAttempts(s.config),
	}
	return s.repository.InsertOutbox(ctx, tx, entry)
}

// ProcessOutbox claims pending notifications, sends them, and persists delivery status.
func (s *Service) ProcessOutbox(ctx context.Context) error {
	if s == nil {
		return nil
	}
	if err := ctx.Err(); err != nil {
		return err
	}
	if s.repository == nil {
		return errors.New("process notification outbox: repository is required")
	}
	if s.sender == nil {
		return errors.New("process notification outbox: sender is required")
	}
	if s.renderer == nil {
		return errors.New("process notification outbox: renderer is required")
	}

	entries, err := s.repository.FetchPending(ctx, s.db, batchSize(s.config))
	if err != nil {
		return err
	}

	for _, entry := range entries {
		if err := ctx.Err(); err != nil {
			return err
		}
		if processErr := s.processEntry(ctx, entry); processErr != nil {
			s.logger.Sugar().Errorw("notification outbox entry processing failed", "entry_id", entry.ID, "error", processErr)
		}
	}
	return nil
}

func (s *Service) processEntry(ctx context.Context, entry *OutboxEntry) error {
	var data any
	if len(entry.TemplateData) > 0 {
		if err := json.Unmarshal(entry.TemplateData, &data); err != nil {
			return s.markFailed(ctx, entry, fmt.Errorf("unmarshal notification template data: %w", err))
		}
	}

	body, err := s.renderer.RenderBody(entry.TemplateName, data)
	if err != nil {
		return s.markFailed(ctx, entry, err)
	}
	if strings.TrimSpace(entry.Subject) == "" {
		subject, renderErr := s.renderer.RenderSubjectWithData(entry.TemplateName, data)
		if renderErr != nil {
			return s.markFailed(ctx, entry, renderErr)
		}
		entry.Subject = subject
	}

	if err := s.sender.Send(ctx, entry.ToRecipients(), entry.CcRecipients(), entry.Subject, body); err != nil {
		return s.markFailed(ctx, entry, err)
	}

	now := s.now()
	entry.Status = StatusSent
	entry.SentAt = &now
	entry.NextAttemptAt = nil
	entry.LastError = nil
	if err := s.repository.UpdateStatus(ctx, entry); err != nil {
		return fmt.Errorf("mark notification outbox sent: %w", err)
	}
	s.logger.Sugar().Infow("notification outbox entry sent", "entry_id", entry.ID, "event_type", entry.EventType)
	return nil
}

func (s *Service) markFailed(ctx context.Context, entry *OutboxEntry, sendErr error) error {
	entry.AttemptCount++
	errText := sendErr.Error()
	entry.LastError = &errText
	entry.SentAt = nil
	if entry.MaxAttempts <= 0 {
		entry.MaxAttempts = maxRetryAttempts(s.config)
	}
	if entry.AttemptCount >= entry.MaxAttempts {
		entry.Status = StatusDead
		entry.NextAttemptAt = nil
	} else {
		entry.Status = StatusFailed
		nextAttempt := s.now().Add(retryInterval(s.config))
		entry.NextAttemptAt = &nextAttempt
	}
	if err := s.repository.UpdateStatus(ctx, entry); err != nil {
		return fmt.Errorf("mark notification outbox failed: %w", err)
	}
	return sendErr
}

func maxRetryAttempts(cfg config.EmailConfig) int {
	if cfg.MaxRetryAttempts <= 0 {
		return 5
	}
	return cfg.MaxRetryAttempts
}

func retryInterval(cfg config.EmailConfig) time.Duration {
	if cfg.RetryIntervalSeconds <= 0 {
		return 5 * time.Minute
	}
	return time.Duration(cfg.RetryIntervalSeconds) * time.Second
}

func batchSize(cfg config.EmailConfig) int {
	if cfg.BatchSize <= 0 {
		return 20
	}
	return cfg.BatchSize
}
