package notification

import (
	"context"
	"crypto/tls"
	"errors"
	"fmt"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"go.uber.org/zap"
	mail "gopkg.in/mail.v2"
)

// Sender delivers a rendered notification email.
type Sender interface {
	Send(ctx context.Context, to []string, cc []string, subject, htmlBody string) error
}

// SMTPSender sends mail via an SMTP relay.
type SMTPSender struct {
	config config.EmailConfig
	logger *zap.Logger
}

// NewSMTPSender constructs an SMTP-backed sender.
func NewSMTPSender(cfg config.EmailConfig, logger *zap.Logger) *SMTPSender {
	return &SMTPSender{config: cfg, logger: logger}
}

// Send delivers a single HTML email message.
func (s *SMTPSender) Send(ctx context.Context, to []string, cc []string, subject, htmlBody string) error {
	if err := ctx.Err(); err != nil {
		return err
	}
	if len(to) == 0 {
		return errors.New("send email: no recipients")
	}
	if strings.TrimSpace(s.config.SMTPHost) == "" || s.config.SMTPPort <= 0 {
		return errors.New("send email: incomplete SMTP configuration")
	}
	if strings.TrimSpace(s.config.FromAddress) == "" {
		return errors.New("send email: missing from address")
	}

	message := mail.NewMessage()
	message.SetAddressHeader("From", s.config.FromAddress, s.config.FromName)
	message.SetHeader("To", to...)
	if len(cc) > 0 {
		message.SetHeader("Cc", cc...)
	}
	message.SetHeader("Subject", subject)
	message.SetBody("text/html", htmlBody)

	dialer := mail.NewDialer(s.config.SMTPHost, s.config.SMTPPort, s.config.SMTPUsername, s.config.SMTPPassword)
	dialer.TLSConfig = &tls.Config{MinVersion: tls.VersionTLS12, ServerName: s.config.SMTPHost}
	if deadline, ok := ctx.Deadline(); ok {
		timeout := time.Until(deadline)
		if timeout > 0 {
			dialer.Timeout = timeout
		}
	}
	if s.config.SMTPPort == 465 {
		dialer.SSL = true
		dialer.StartTLSPolicy = mail.NoStartTLS
	} else {
		dialer.SSL = false
		dialer.StartTLSPolicy = mail.MandatoryStartTLS
	}

	if err := dialer.DialAndSend(message); err != nil {
		s.logger.Sugar().Errorw("notification email send failed",
			"smtp_host", s.config.SMTPHost,
			"smtp_port", s.config.SMTPPort,
			"to", to,
			"cc", cc,
			"subject", subject,
			"error", err,
		)
		return fmt.Errorf("send email via smtp: %w", err)
	}

	s.logger.Sugar().Infow("notification email sent",
		"smtp_host", s.config.SMTPHost,
		"smtp_port", s.config.SMTPPort,
		"to", to,
		"cc", cc,
		"subject", subject,
	)
	return nil
}
