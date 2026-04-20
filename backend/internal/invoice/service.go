package invoice

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"math"
	"sort"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/notification"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
)

var (
	ErrDocumentNotFound            = errors.New("billing document not found")
	ErrInvalidDocumentState        = errors.New("invalid billing document state")
	ErrInvalidDocumentInput        = errors.New("invalid billing document input")
	ErrDocumentAlreadySubmitted    = errors.New("billing document already submitted")
	ErrChargeLineAlreadyDocumented = errors.New("billing charge line already linked to another active document")
	ErrReceivableContextInvalid    = errors.New("receivable context missing required customer data")
	ErrPaymentAmountInvalid        = errors.New("payment amount must be greater than zero")
	ErrPaymentNotAllowed           = errors.New("payment not allowed for the current billing document state")
	ErrPaymentOverApplication      = errors.New("payment amount exceeds outstanding receivable balance")
	ErrDocumentHasRecordedPayments = errors.New("billing document already has recorded payments")
)

type Service struct {
	repository      *Repository
	billingRepo     *billing.Repository
	workflowService *workflow.Service
	db              *sql.DB
	notifier        notification.Notifier
}

func NewService(db *sql.DB, repository *Repository, billingRepo *billing.Repository, workflowService *workflow.Service, notifiers ...notification.Notifier) *Service {
	service := &Service{db: db, repository: repository, billingRepo: billingRepo, workflowService: workflowService}
	if len(notifiers) > 0 {
		service.notifier = notifiers[0]
	}
	return service
}

const paymentReminderLeadDays = 7

func (s *Service) CreateFromCharges(ctx context.Context, input CreateInput) (*Document, error) {
	normalizedChargeLineIDs := normalizeChargeLineIDs(input.BillingChargeLineIDs)
	if input.ActorUserID == 0 || (input.DocumentType != DocumentTypeBill && input.DocumentType != DocumentTypeInvoice) || len(normalizedChargeLineIDs) == 0 {
		return nil, ErrInvalidDocumentInput
	}
	if s.billingRepo == nil {
		return nil, ErrInvalidDocumentInput
	}
	chargeLines, err := s.billingRepo.GetChargeLinesByIDs(ctx, normalizedChargeLineIDs)
	if err != nil {
		return nil, err
	}
	if len(chargeLines) != len(normalizedChargeLineIDs) {
		return nil, ErrInvalidDocumentInput
	}
	document, err := documentFromChargeLines(CreateInput{DocumentType: input.DocumentType, BillingChargeLineIDs: normalizedChargeLineIDs, ActorUserID: input.ActorUserID}, chargeLines)
	if err != nil {
		return nil, err
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin billing document create transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	reservedCount, err := s.repository.CountReservedChargeLines(ctx, tx, normalizedChargeLineIDs)
	if err != nil {
		return nil, err
	}
	if reservedCount > 0 {
		return nil, ErrChargeLineAlreadyDocumented
	}
	if err := s.repository.Create(ctx, tx, document); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit billing document create transaction: %w", err)
	}
	return s.repository.FindByID(ctx, document.ID)
}

func (s *Service) GetDocument(ctx context.Context, documentID int64) (*Document, error) {
	document, err := s.repository.FindByID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	return document, nil
}

func (s *Service) ListDocuments(ctx context.Context, filter ListFilter) (*pagination.ListResult[Document], error) {
	return s.repository.List(ctx, filter)
}

func (s *Service) SubmitForApproval(ctx context.Context, input SubmitInput) (*Document, error) {
	if input.ActorUserID == 0 || input.DepartmentID == 0 || input.IdempotencyKey == "" || s.workflowService == nil {
		return nil, ErrInvalidDocumentInput
	}
	document, err := s.repository.FindByID(ctx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	if document.Status != StatusDraft {
		if document.WorkflowInstanceID != nil {
			return document, nil
		}
		return nil, ErrInvalidDocumentState
	}
	leaseCtx, err := s.repository.FindReceivableLeaseContext(ctx, document.LeaseContractID)
	if err != nil {
		return nil, err
	}
	if leaseCtx == nil {
		return nil, ErrReceivableContextInvalid
	}
	instance, err := s.workflowService.Start(ctx, workflow.StartInput{
		DefinitionCode: ApprovalDefinitionCode,
		DocumentType:   string(document.DocumentType),
		DocumentID:     document.ID,
		ActorUserID:    input.ActorUserID,
		DepartmentID:   input.DepartmentID,
		IdempotencyKey: input.IdempotencyKey,
		Comment:        input.Comment,
	})
	if err != nil {
		return nil, err
	}
	now := time.Now().UTC()
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin billing document submit transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	if err := s.repository.AttachWorkflowInstance(ctx, tx, document.ID, instance.ID, input.ActorUserID, now); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit billing document submit transaction: %w", err)
	}
	return s.repository.FindByID(ctx, document.ID)
}

func (s *Service) Cancel(ctx context.Context, input CancelInput) (*Document, error) {
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin billing document cancel transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	document, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	if document.Status == StatusCancelled {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate billing document cancel transaction: %w", err)
		}
		return s.repository.FindByID(ctx, document.ID)
	}
	if document.Status != StatusApproved {
		return nil, ErrInvalidDocumentState
	}
	paymentCount, err := s.repository.CountPaymentEntries(ctx, tx, document.ID)
	if err != nil {
		return nil, err
	}
	if paymentCount > 0 {
		return nil, ErrDocumentHasRecordedPayments
	}
	if err := s.repository.ZeroOpenItemsForDocument(ctx, tx, document.ID, time.Now().UTC()); err != nil {
		return nil, err
	}
	if err := s.repository.Cancel(ctx, tx, document.ID, input.ActorUserID, time.Now().UTC()); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit billing document cancel transaction: %w", err)
	}
	return s.repository.FindByID(ctx, document.ID)
}

func (s *Service) Adjust(ctx context.Context, input AdjustInput) (*Document, error) {
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin billing document adjust transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	original, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if original == nil {
		return nil, ErrDocumentNotFound
	}
	if original.Status != StatusApproved {
		return nil, ErrInvalidDocumentState
	}
	paymentCount, err := s.repository.CountPaymentEntries(ctx, tx, original.ID)
	if err != nil {
		return nil, err
	}
	if paymentCount > 0 {
		return nil, ErrDocumentHasRecordedPayments
	}
	adjusted, err := adjustedDocumentFromExisting(*original, input)
	if err != nil {
		return nil, err
	}
	if err := s.repository.ZeroOpenItemsForDocument(ctx, tx, original.ID, time.Now().UTC()); err != nil {
		return nil, err
	}
	if err := s.repository.MarkAdjusted(ctx, tx, original.ID, input.ActorUserID); err != nil {
		return nil, err
	}
	if err := s.repository.Create(ctx, tx, adjusted); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit billing document adjust transaction: %w", err)
	}
	return s.repository.FindByID(ctx, adjusted.ID)
}

func (s *Service) SyncWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error {
	if instance == nil || (instance.DocumentType != string(DocumentTypeBill) && instance.DocumentType != string(DocumentTypeInvoice)) {
		return nil
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin billing document workflow sync transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	document, err := s.repository.FindByIDForUpdate(ctx, tx, instance.DocumentID)
	if err != nil {
		return err
	}
	if document == nil {
		return nil
	}
	status, approvedAt := mapWorkflowState(instance)
	if document.Status == status && document.WorkflowInstanceID != nil && *document.WorkflowInstanceID == instance.ID {
		if err := tx.Commit(); err != nil {
			return fmt.Errorf("commit duplicate billing document workflow sync transaction: %w", err)
		}
		return nil
	}
	var documentNo *string
	if status == StatusApproved && document.DocumentNo == nil {
		sequenceCode := string(document.DocumentType)
		number, err := s.repository.AllocateNumber(ctx, tx, sequenceCode)
		if err != nil {
			return err
		}
		documentNo = &number
	} else {
		documentNo = document.DocumentNo
	}
	if err := s.repository.UpdateWorkflowState(ctx, tx, document.ID, instance.ID, actorUserID, status, documentNo, approvedAt); err != nil {
		return err
	}
	if status == StatusApproved {
		leaseCtx, err := s.repository.findReceivableLeaseContext(ctx, tx.QueryRowContext(ctx, `
			SELECT id, customer_id, department_id, trade_id
			FROM lease_contracts
			WHERE id = ?
			FOR UPDATE
		`, document.LeaseContractID))
		if err != nil {
			return err
		}
		if leaseCtx == nil {
			return ErrReceivableContextInvalid
		}
		for _, line := range document.Lines {
			if line.Amount <= 0 {
				continue
			}
			if err := s.repository.UpsertReceivableOpenItem(ctx, tx, leaseCtx, document.ID, receivableOpenItemRow{
				BillingDocumentLineID: line.ID,
				ChargeType:            line.ChargeType,
				DueDate:               line.PeriodEnd.UTC(),
				OutstandingAmount:     roundMoney(line.Amount),
				IsDeposit:             line.ChargeType == string(lease.TermTypeDeposit),
			}); err != nil {
				return err
			}
		}
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit billing document workflow sync transaction: %w", err)
	}
	return nil
}

func (s *Service) GetReceivable(ctx context.Context, documentID int64) (*ReceivableSummary, error) {
	document, err := s.repository.FindByID(ctx, documentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	summary, err := s.repository.GetReceivableSummary(ctx, documentID)
	if err != nil {
		return nil, err
	}
	if summary == nil {
		return &ReceivableSummary{
			BillingDocumentID: document.ID,
			DocumentNo:        document.DocumentNo,
			DocumentType:      document.DocumentType,
			TenantName:        document.TenantName,
			LeaseContractID:   document.LeaseContractID,
			OutstandingAmount: 0,
			SettlementStatus:  SettlementStatusSettled,
			Items:             []OpenItem{},
			PaymentHistory:    []PaymentEntry{},
		}, nil
	}
	if summary.Items == nil {
		summary.Items = []OpenItem{}
	}
	if summary.PaymentHistory == nil {
		summary.PaymentHistory = []PaymentEntry{}
	}
	return summary, nil
}

func (s *Service) ListReceivables(ctx context.Context, filter ReceivableFilter) (*pagination.ListResult[ReceivableListItem], error) {
	return s.repository.ListReceivables(ctx, filter)
}

func (s *Service) RecordPayment(ctx context.Context, input RecordPaymentInput) (*ReceivableSummary, error) {
	if input.DocumentID == 0 || input.ActorUserID == 0 || input.IdempotencyKey == "" {
		return nil, ErrInvalidDocumentInput
	}
	if input.Amount <= 0 {
		return nil, ErrPaymentAmountInvalid
	}
	paymentDate := input.PaymentDate.UTC()
	if paymentDate.IsZero() {
		paymentDate = time.Now().UTC()
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin payment transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	document, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	if document.Status != StatusApproved {
		return nil, ErrPaymentNotAllowed
	}
	existingEntry, err := s.repository.FindPaymentEntryByIdempotency(ctx, tx, input.DocumentID, input.IdempotencyKey)
	if err != nil {
		return nil, err
	}
	if existingEntry != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate payment transaction: %w", err)
		}
		return s.GetReceivable(ctx, input.DocumentID)
	}
	items, err := s.repository.FindOpenItemsByDocumentIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if len(items) == 0 {
		return nil, ErrPaymentNotAllowed
	}
	outstandingTotal := 0.0
	for _, item := range items {
		outstandingTotal += item.OutstandingAmount
	}
	outstandingTotal = roundMoney(outstandingTotal)
	if outstandingTotal <= 0 {
		return nil, ErrPaymentNotAllowed
	}
	amount := roundMoney(input.Amount)
	if amount > outstandingTotal {
		return nil, ErrPaymentOverApplication
	}
	var note *string
	if input.Note != "" {
		note = &input.Note
	}
	if err := s.repository.InsertPaymentEntry(ctx, tx, PaymentEntry{
		BillingDocumentID: document.ID,
		LeaseContractID:   document.LeaseContractID,
		PaymentDate:       paymentDate,
		Amount:            amount,
		Note:              note,
		RecordedBy:        input.ActorUserID,
		IdempotencyKey:    input.IdempotencyKey,
	}); err != nil {
		return nil, err
	}
	remaining := amount
	settledAt := time.Now().UTC()
	for _, item := range items {
		if remaining <= 0 {
			break
		}
		applied := math.Min(item.OutstandingAmount, remaining)
		newOutstanding := roundMoney(item.OutstandingAmount - applied)
		var itemSettledAt *time.Time
		if newOutstanding == 0 {
			itemSettledAt = &settledAt
		}
		if err := s.repository.UpdateOpenItemBalance(ctx, tx, item.ID, newOutstanding, itemSettledAt); err != nil {
			return nil, err
		}
		remaining = roundMoney(remaining - applied)
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit payment transaction: %w", err)
	}
	return s.GetReceivable(ctx, input.DocumentID)
}

func (s *Service) CheckPaymentReminders(ctx context.Context) error {
	if s == nil || s.repository == nil || s.notifier == nil {
		return nil
	}
	asOf := time.Now().UTC()
	candidates, err := s.repository.ListPaymentReminderCandidates(ctx, asOf, paymentReminderLeadDays)
	if err != nil {
		return err
	}
	for _, candidate := range candidates {
		if err := s.enqueuePaymentReminder(ctx, asOf, candidate); err != nil {
			continue
		}
	}
	return nil
}

func roundMoney(value float64) float64 {
	rounded := math.Round(value*100) / 100
	if math.Abs(rounded) < 0.000001 {
		return 0
	}
	return rounded
}

func documentFromChargeLines(input CreateInput, chargeLines []billing.ChargeLine) (*Document, error) {
	first := chargeLines[0]
	document := &Document{DocumentType: input.DocumentType, BillingRunID: first.BillingRunID, LeaseContractID: first.LeaseContractID, TenantName: first.TenantName, PeriodStart: first.PeriodStart, PeriodEnd: first.PeriodEnd, CurrencyTypeID: first.CurrencyTypeID, Status: StatusDraft, CreatedBy: input.ActorUserID, UpdatedBy: input.ActorUserID, Lines: make([]Line, 0, len(chargeLines))}
	totalAmount := 0.0
	for _, chargeLine := range chargeLines {
		if chargeLine.BillingRunID != document.BillingRunID || chargeLine.LeaseContractID != document.LeaseContractID || !chargeLine.PeriodStart.Equal(document.PeriodStart) || !chargeLine.PeriodEnd.Equal(document.PeriodEnd) || chargeLine.CurrencyTypeID != document.CurrencyTypeID {
			return nil, ErrInvalidDocumentInput
		}
		document.Lines = append(document.Lines, Line{BillingChargeLineID: chargeLine.ID, ChargeType: chargeLine.ChargeType, PeriodStart: chargeLine.PeriodStart, PeriodEnd: chargeLine.PeriodEnd, QuantityDays: chargeLine.QuantityDays, UnitAmount: chargeLine.UnitAmount, Amount: chargeLine.Amount})
		totalAmount += chargeLine.Amount
	}
	document.TotalAmount = totalAmount
	return document, nil
}

func adjustedDocumentFromExisting(original Document, input AdjustInput) (*Document, error) {
	if input.ActorUserID == 0 || len(original.Lines) == 0 {
		return nil, ErrInvalidDocumentInput
	}
	overrides := make(map[int64]float64, len(input.Lines))
	for _, line := range input.Lines {
		if line.Amount < 0 {
			return nil, ErrInvalidDocumentInput
		}
		overrides[line.BillingChargeLineID] = line.Amount
	}
	adjustedFromID := original.ID
	document := &Document{DocumentType: original.DocumentType, BillingRunID: original.BillingRunID, LeaseContractID: original.LeaseContractID, TenantName: original.TenantName, PeriodStart: original.PeriodStart, PeriodEnd: original.PeriodEnd, CurrencyTypeID: original.CurrencyTypeID, Status: StatusDraft, AdjustedFromID: &adjustedFromID, CreatedBy: input.ActorUserID, UpdatedBy: input.ActorUserID, Lines: make([]Line, 0, len(original.Lines))}
	totalAmount := 0.0
	for _, line := range original.Lines {
		amount := line.Amount
		if override, ok := overrides[line.BillingChargeLineID]; ok {
			amount = override
		}
		document.Lines = append(document.Lines, Line{BillingChargeLineID: line.BillingChargeLineID, ChargeType: line.ChargeType, PeriodStart: line.PeriodStart, PeriodEnd: line.PeriodEnd, QuantityDays: line.QuantityDays, UnitAmount: line.UnitAmount, Amount: amount})
		totalAmount += amount
	}
	document.TotalAmount = totalAmount
	sort.Slice(document.Lines, func(i, j int) bool {
		return document.Lines[i].BillingChargeLineID < document.Lines[j].BillingChargeLineID
	})
	return document, nil
}

func mapWorkflowState(instance *workflow.Instance) (Status, *time.Time) {
	switch instance.Status {
	case workflow.InstanceStatusApproved:
		approvedAt := time.Now().UTC()
		if instance.CompletedAt != nil {
			approvedAt = instance.CompletedAt.UTC()
		}
		return StatusApproved, &approvedAt
	case workflow.InstanceStatusRejected:
		return StatusRejected, nil
	default:
		return StatusPendingApproval, nil
	}
}

func (s *Service) enqueuePaymentReminder(ctx context.Context, asOf time.Time, candidate PaymentReminderCandidate) error {
	recipient := normalizeReminderRecipient(candidate.ContactCandidate)
	if recipient == "" {
		return nil
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin payment reminder transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	alreadyQueued, err := s.repository.HasReminderQueuedSince(ctx, tx, "invoice.payment_reminder", "billing_document", candidate.DocumentID, asOf.Truncate(24*time.Hour))
	if err != nil {
		return err
	}
	if alreadyQueued {
		if err := tx.Commit(); err != nil {
			return fmt.Errorf("commit duplicate payment reminder transaction: %w", err)
		}
		return nil
	}

	daysOverdue := int(asOf.Truncate(24*time.Hour).Sub(candidate.DueDate.UTC()).Hours() / 24)
	if daysOverdue < 0 {
		daysOverdue = 0
	}
	if err := s.notifier.Enqueue(ctx, tx, notification.NotificationEvent{
		EventType:     "invoice.payment_reminder",
		AggregateType: "billing_document",
		AggregateID:   candidate.DocumentID,
		RecipientTo:   []string{recipient},
		TemplateName:  "invoice_payment_reminder",
		TemplateData: notification.InvoiceReminderData{
			InvoiceNumber: candidate.InvoiceNumber,
			CustomerName:  candidate.CustomerName,
			AmountDue:     fmt.Sprintf("%.2f", roundMoney(candidate.OutstandingAmount)),
			DueDate:       candidate.DueDate.UTC().Format(DateLayout),
			DaysOverdue:   daysOverdue,
		},
	}); err != nil {
		if commitErr := tx.Commit(); commitErr != nil {
			return fmt.Errorf("commit failed payment reminder transaction: %w", commitErr)
		}
		return err
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit payment reminder transaction: %w", err)
	}
	return nil
}

func normalizeReminderRecipient(candidate string) string {
	trimmed := strings.TrimSpace(candidate)
	if trimmed == "" || !strings.Contains(trimmed, "@") {
		return ""
	}
	return strings.ToLower(trimmed)
}
