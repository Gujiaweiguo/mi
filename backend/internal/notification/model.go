package notification

import (
	"encoding/json"
	"strings"
	"time"
)

// Status identifies the current state of a notification outbox entry.
type Status string

const (
	// StatusPending indicates the notification is ready for processing.
	StatusPending Status = "pending"
	// StatusSending indicates the notification is currently being processed.
	StatusSending Status = "sending"
	// StatusSent indicates the notification was delivered successfully.
	StatusSent Status = "sent"
	// StatusFailed indicates the notification send failed and may be retried.
	StatusFailed Status = "failed"
	// StatusDead indicates the notification exceeded its retry budget.
	StatusDead Status = "dead"
)

// OutboxEntry models a row in the notification_outbox table.
type OutboxEntry struct {
	ID            int64           `json:"id"`
	EventType     string          `json:"event_type"`
	AggregateType string          `json:"aggregate_type"`
	AggregateID   int64           `json:"aggregate_id"`
	RecipientTo   string          `json:"recipient_to"`
	RecipientCc   string          `json:"recipient_cc"`
	Subject       string          `json:"subject"`
	TemplateName  string          `json:"template_name"`
	TemplateData  json.RawMessage `json:"template_data"`
	Status        Status          `json:"status"`
	AttemptCount  int             `json:"attempt_count"`
	MaxAttempts   int             `json:"max_attempts"`
	NextAttemptAt *time.Time      `json:"next_attempt_at"`
	SentAt        *time.Time      `json:"sent_at"`
	LastError     *string         `json:"last_error"`
	CreatedAt     time.Time       `json:"created_at"`
	UpdatedAt     time.Time       `json:"updated_at"`
}

// ToRecipients returns the parsed primary recipient list.
func (e OutboxEntry) ToRecipients() []string {
	return splitRecipients(e.RecipientTo)
}

// CcRecipients returns the parsed carbon-copy recipient list.
func (e OutboxEntry) CcRecipients() []string {
	return splitRecipients(e.RecipientCc)
}

// NotificationEvent describes a notification enqueue request from a business service.
type NotificationEvent struct {
	EventType     string
	AggregateType string
	AggregateID   int64
	RecipientTo   []string
	RecipientCc   []string
	Subject       string
	TemplateName  string
	TemplateData  any
}

// WorkflowApprovalData is the template data for workflow approval notifications.
type WorkflowApprovalData struct {
	DocumentType     string
	DocumentNumber   string
	SubmitterName    string
	ApprovalStepName string
	ApprovalLink     string
}

// InvoiceReminderData is the template data for invoice reminder notifications.
type InvoiceReminderData struct {
	InvoiceNumber string
	CustomerName  string
	AmountDue     string
	DueDate       string
	DaysOverdue   int
}

// LeaseExpirationData is the template data for lease expiration notifications.
type LeaseExpirationData struct {
	ContractNumber string
	TenantName     string
	ExpirationDate string
	DaysRemaining  int
}

// ListParams defines supported filters for notification history queries.
type ListParams struct {
	Page          int
	PageSize      int
	EventType     string
	AggregateType string
	AggregateID   *int64
	Status        string
}

func splitRecipients(raw string) []string {
	parts := strings.Split(raw, ",")
	result := make([]string, 0, len(parts))
	for _, part := range parts {
		trimmed := strings.TrimSpace(part)
		if trimmed == "" {
			continue
		}
		result = append(result, trimmed)
	}
	return result
}

func joinRecipients(recipients []string) string {
	cleaned := make([]string, 0, len(recipients))
	for _, recipient := range recipients {
		trimmed := strings.TrimSpace(recipient)
		if trimmed == "" {
			continue
		}
		cleaned = append(cleaned, trimmed)
	}
	return strings.Join(cleaned, ",")
}
