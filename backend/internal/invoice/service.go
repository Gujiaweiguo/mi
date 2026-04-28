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
	ErrDiscountAmountInvalid       = errors.New("discount amount must be greater than zero")
	ErrDiscountReasonRequired      = errors.New("discount reason is required")
	ErrDiscountNotAllowed          = errors.New("discount not allowed for the current billing document state")
	ErrDiscountOverApplication     = errors.New("discount amount exceeds outstanding receivable balance")
	ErrDiscountPendingApproval     = errors.New("a discount for the selected line is already pending approval")
	ErrSurplusAmountInvalid        = errors.New("surplus amount must be greater than zero")
	ErrSurplusNotAvailable         = errors.New("customer surplus balance is not available")
	ErrSurplusInsufficient         = errors.New("customer surplus balance is insufficient")
	ErrSurplusTargetNotAllowed     = errors.New("surplus application target is not allowed")
	ErrInterestNotConfigured       = errors.New("late-payment interest rate is not configured")
	ErrInterestNotDue              = errors.New("late-payment interest is not due for the selected receivable")
	ErrDepositAmountInvalid        = errors.New("deposit application amount must be greater than zero")
	ErrDepositNotAvailable         = errors.New("deposit is not available for application")
	ErrDepositTargetNotAllowed     = errors.New("deposit application target is not allowed")
	ErrDepositRefundBlocked        = errors.New("deposit refund is blocked by outstanding obligations")
	ErrDepositRefundAmountInvalid  = errors.New("deposit refund amount must be greater than zero")
	ErrDepositRefundReasonRequired = errors.New("deposit refund reason is required")
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

func (s *Service) ApplyDiscount(ctx context.Context, input ApplyDiscountInput) (*ReceivableSummary, error) {
	if input.DocumentID == 0 || input.ActorUserID == 0 || input.DepartmentID == 0 || input.IdempotencyKey == "" || s.workflowService == nil {
		return nil, ErrInvalidDocumentInput
	}
	amount := roundMoney(input.Amount)
	if amount <= 0 {
		return nil, ErrDiscountAmountInvalid
	}
	reason := strings.TrimSpace(input.Reason)
	if reason == "" {
		return nil, ErrDiscountReasonRequired
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin discount transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	existing, err := s.repository.FindDiscountByIdempotency(ctx, tx, input.DocumentID, input.IdempotencyKey)
	if err != nil {
		return nil, err
	}
	if existing != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate discount transaction: %w", err)
		}
		if existing.WorkflowInstanceID == nil {
			if err := s.startDiscountWorkflow(ctx, existing, input); err != nil {
				return nil, err
			}
		}
		return s.GetReceivable(ctx, input.DocumentID)
	}
	document, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	if document.Status != StatusApproved {
		return nil, ErrDiscountNotAllowed
	}
	line := findDocumentLineByID(document.Lines, input.BillingDocumentLineID)
	if line == nil || line.ChargeType == string(lease.TermTypeDeposit) {
		return nil, ErrDiscountNotAllowed
	}
	openItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, input.DocumentID, input.BillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if openItem == nil || openItem.IsDeposit || openItem.OutstandingAmount <= 0 {
		return nil, ErrDiscountNotAllowed
	}
	if amount > roundMoney(openItem.OutstandingAmount) || amount > roundMoney(line.Amount) {
		return nil, ErrDiscountOverApplication
	}
	pendingCount, err := s.repository.CountPendingDiscountsByLine(ctx, tx, input.BillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if pendingCount > 0 {
		return nil, ErrDiscountPendingApproval
	}
	rate := roundRate(amount / line.Amount)
	discount := &Discount{BillingDocumentID: document.ID, BillingDocumentLineID: input.BillingDocumentLineID, LeaseContractID: document.LeaseContractID, ChargeType: line.ChargeType, RequestedAmount: amount, RequestedRate: rate, Reason: reason, Status: DiscountStatusDraft, IdempotencyKey: input.IdempotencyKey, CreatedBy: input.ActorUserID, UpdatedBy: input.ActorUserID}
	if err := s.repository.CreateDiscount(ctx, tx, discount); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit discount create transaction: %w", err)
	}
	if err := s.startDiscountWorkflow(ctx, discount, input); err != nil {
		return nil, err
	}
	return s.GetReceivable(ctx, input.DocumentID)
}

func (s *Service) ApplySurplus(ctx context.Context, input ApplySurplusInput) (*ReceivableSummary, error) {
	if input.DocumentID == 0 || input.BillingDocumentLineID == 0 || input.ActorUserID == 0 || input.IdempotencyKey == "" {
		return nil, ErrInvalidDocumentInput
	}
	amount := roundMoney(input.Amount)
	if amount <= 0 {
		return nil, ErrSurplusAmountInvalid
	}
	note := strings.TrimSpace(input.Note)
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin surplus application transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	existing, err := s.repository.FindSurplusEntryByTypeAndIdempotency(ctx, tx, input.DocumentID, SurplusEntryTypeApplication, input.IdempotencyKey)
	if err != nil {
		return nil, err
	}
	if existing != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate surplus application transaction: %w", err)
		}
		if existing.BillingDocumentID != nil {
			return s.GetReceivable(ctx, *existing.BillingDocumentID)
		}
		return s.GetReceivable(ctx, input.DocumentID)
	}
	document, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	if document.Status != StatusApproved {
		return nil, ErrSurplusTargetNotAllowed
	}
	openItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, input.DocumentID, input.BillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if openItem == nil || openItem.IsDeposit || openItem.OutstandingAmount <= 0 {
		return nil, ErrSurplusTargetNotAllowed
	}
	balance, err := s.repository.FindSurplusBalanceByCustomerForUpdate(ctx, tx, openItem.CustomerID)
	if err != nil {
		return nil, err
	}
	if balance == nil || balance.AvailableAmount <= 0 {
		return nil, ErrSurplusNotAvailable
	}
	if amount > roundMoney(balance.AvailableAmount) {
		return nil, ErrSurplusInsufficient
	}
	if amount > roundMoney(openItem.OutstandingAmount) {
		return nil, ErrSurplusTargetNotAllowed
	}
	appliedAt := time.Now().UTC()
	updatedBalance, err := s.repository.UpsertSurplusBalance(ctx, tx, balance.CustomerID, input.ActorUserID, -amount, &appliedAt)
	if err != nil {
		return nil, err
	}
	newOutstanding := roundMoney(openItem.OutstandingAmount - amount)
	var settledAt *time.Time
	if newOutstanding == 0 {
		settledAt = &appliedAt
	}
	if err := s.repository.UpdateOpenItemBalance(ctx, tx, openItem.ID, newOutstanding, settledAt); err != nil {
		return nil, err
	}
	var notePtr *string
	if note != "" {
		notePtr = &note
	}
	documentID := document.ID
	openItemID := openItem.ID
	if err := s.repository.InsertSurplusEntry(ctx, tx, SurplusEntry{SurplusBalanceID: updatedBalance.ID, EntryType: SurplusEntryTypeApplication, CustomerID: balance.CustomerID, BillingDocumentID: &documentID, AROpenItemID: &openItemID, Amount: amount, Note: notePtr, IdempotencyKey: input.IdempotencyKey, RecordedBy: input.ActorUserID}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit surplus application transaction: %w", err)
	}
	return s.GetReceivable(ctx, input.DocumentID)
}

func (s *Service) GenerateInterest(ctx context.Context, input GenerateInterestInput) (*ReceivableSummary, error) {
	if input.DocumentID == 0 || input.BillingDocumentLineID == 0 || input.ActorUserID == 0 || input.IdempotencyKey == "" || s.billingRepo == nil {
		return nil, ErrInvalidDocumentInput
	}
	asOfDate := input.AsOfDate.UTC()
	if asOfDate.IsZero() {
		asOfDate = time.Now().UTC()
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin interest generation transaction: %w", err)
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
		return nil, ErrInvalidDocumentState
	}
	line := findDocumentLineByID(document.Lines, input.BillingDocumentLineID)
	if line == nil || line.ChargeType == string(lease.TermTypeDeposit) || line.ChargeType == ChargeTypeLateInterest {
		return nil, ErrInterestNotDue
	}
	openItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, input.DocumentID, input.BillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if openItem == nil || openItem.IsDeposit || openItem.OutstandingAmount <= 0 || openItem.ChargeType == ChargeTypeLateInterest {
		return nil, ErrInterestNotDue
	}
	existing, err := s.repository.FindInterestEntryBySourceAndIdempotency(ctx, tx, openItem.ID, input.IdempotencyKey)
	if err != nil {
		return nil, err
	}
	if existing != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate interest generation transaction: %w", err)
		}
		return s.GetReceivable(ctx, input.DocumentID)
	}
	config, err := s.repository.FindInterestRateConfig(ctx, openItem.ChargeType)
	if err != nil {
		return nil, err
	}
	if config == nil {
		return nil, ErrInterestNotConfigured
	}
	coveredStart := openItem.DueDate.UTC().AddDate(0, 0, config.GraceDays+1)
	latest, err := s.repository.FindLatestInterestEntryForSource(ctx, tx, openItem.ID)
	if err != nil {
		return nil, err
	}
	if latest != nil {
		coveredStart = latest.CoveredEndDate.UTC().AddDate(0, 0, 1)
	}
	if asOfDate.Before(coveredStart) {
		return nil, ErrInterestNotDue
	}
	interestDays := int(asOfDate.Sub(coveredStart).Hours()/24) + 1
	if interestDays <= 0 {
		return nil, ErrInterestNotDue
	}
	principalAmount := roundMoney(openItem.OutstandingAmount)
	interestAmount := roundMoney(principalAmount * float64(interestDays) * config.DailyRate)
	if interestAmount <= 0 {
		return nil, ErrInterestNotDue
	}
	sourceChargeLines, err := s.billingRepo.GetChargeLinesByIDs(ctx, []int64{line.BillingChargeLineID})
	if err != nil {
		return nil, err
	}
	if len(sourceChargeLines) != 1 {
		return nil, ErrInvalidDocumentInput
	}
	sourceChargeLine := sourceChargeLines[0]
	run := &billing.Run{PeriodStart: coveredStart, PeriodEnd: asOfDate, Status: billing.RunStatusCompleted, TriggeredBy: input.ActorUserID, GeneratedCount: 1, SkippedCount: 0}
	if err := s.billingRepo.CreateRun(ctx, tx, run); err != nil {
		return nil, err
	}
	unitAmount := roundMoney(principalAmount * config.DailyRate)
	interestChargeLine := &billing.ChargeLine{BillingRunID: run.ID, LeaseContractID: document.LeaseContractID, LeaseTermID: sourceChargeLine.LeaseTermID, ChargeType: ChargeTypeLateInterest, ChargeSource: sourceChargeLine.ChargeSource, OvertimeBillID: sourceChargeLine.OvertimeBillID, OvertimeFormulaID: sourceChargeLine.OvertimeFormulaID, OvertimeChargeID: sourceChargeLine.OvertimeChargeID, PeriodStart: coveredStart, PeriodEnd: asOfDate, QuantityDays: interestDays, UnitAmount: unitAmount, Amount: interestAmount, CurrencyTypeID: sourceChargeLine.CurrencyTypeID, SourceEffectiveVersion: sourceChargeLine.SourceEffectiveVersion}
	if err := s.billingRepo.InsertChargeLine(ctx, tx, interestChargeLine); err != nil {
		return nil, err
	}
	number, err := s.repository.AllocateNumber(ctx, tx, string(DocumentTypeInvoice))
	if err != nil {
		return nil, err
	}
	interestDocument, err := documentFromChargeLines(CreateInput{DocumentType: DocumentTypeInvoice, BillingChargeLineIDs: []int64{interestChargeLine.ID}, ActorUserID: input.ActorUserID}, []billing.ChargeLine{{ID: interestChargeLine.ID, BillingRunID: run.ID, LeaseContractID: document.LeaseContractID, TenantName: document.TenantName, LeaseTermID: sourceChargeLine.LeaseTermID, ChargeType: ChargeTypeLateInterest, ChargeSource: sourceChargeLine.ChargeSource, OvertimeBillID: sourceChargeLine.OvertimeBillID, OvertimeFormulaID: sourceChargeLine.OvertimeFormulaID, OvertimeChargeID: sourceChargeLine.OvertimeChargeID, PeriodStart: coveredStart, PeriodEnd: asOfDate, QuantityDays: interestDays, UnitAmount: unitAmount, Amount: interestAmount, CurrencyTypeID: sourceChargeLine.CurrencyTypeID, SourceEffectiveVersion: sourceChargeLine.SourceEffectiveVersion}})
	if err != nil {
		return nil, err
	}
	now := time.Now().UTC()
	interestDocument.Status = StatusApproved
	interestDocument.DocumentNo = &number
	interestDocument.SubmittedAt = &now
	interestDocument.ApprovedAt = &now
	if err := s.repository.Create(ctx, tx, interestDocument); err != nil {
		return nil, err
	}
	createdDocument, err := s.repository.FindByIDForUpdate(ctx, tx, interestDocument.ID)
	if err != nil {
		return nil, err
	}
	if createdDocument == nil || len(createdDocument.Lines) != 1 {
		return nil, ErrInvalidDocumentInput
	}
	leaseCtx, err := s.repository.FindReceivableLeaseContext(ctx, document.LeaseContractID)
	if err != nil {
		return nil, err
	}
	if leaseCtx == nil {
		return nil, ErrReceivableContextInvalid
	}
		if err := s.repository.UpsertReceivableOpenItem(ctx, tx, leaseCtx, createdDocument.ID, receivableOpenItemRow{BillingDocumentLineID: createdDocument.Lines[0].ID, ChargeType: ChargeTypeLateInterest, ChargeSource: createdDocument.Lines[0].ChargeSource, OvertimeBillID: createdDocument.Lines[0].OvertimeBillID, OvertimeFormulaID: createdDocument.Lines[0].OvertimeFormulaID, OvertimeChargeID: createdDocument.Lines[0].OvertimeChargeID, DueDate: asOfDate, OutstandingAmount: interestAmount, IsDeposit: false}); err != nil {
			return nil, err
		}
	if err := s.repository.InsertInterestEntry(ctx, tx, InterestEntry{SourceAROpenItemID: openItem.ID, SourceBillingDocumentID: document.ID, SourceBillingLineID: input.BillingDocumentLineID, GeneratedDocumentID: createdDocument.ID, GeneratedLineID: createdDocument.Lines[0].ID, ChargeType: ChargeTypeLateInterest, PrincipalAmount: principalAmount, DailyRate: config.DailyRate, GraceDays: config.GraceDays, CoveredStartDate: coveredStart, CoveredEndDate: asOfDate, InterestDays: interestDays, InterestAmount: interestAmount, IdempotencyKey: input.IdempotencyKey, CreatedBy: input.ActorUserID}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit interest generation transaction: %w", err)
	}
	return s.GetReceivable(ctx, input.DocumentID)
}

func (s *Service) ApplyDeposit(ctx context.Context, input ApplyDepositInput) (*ReceivableSummary, error) {
	if input.DocumentID == 0 || input.BillingDocumentLineID == 0 || input.TargetDocumentID == 0 || input.TargetBillingDocumentLineID == 0 || input.ActorUserID == 0 || input.IdempotencyKey == "" {
		return nil, ErrInvalidDocumentInput
	}
	amount := roundMoney(input.Amount)
	if amount <= 0 {
		return nil, ErrDepositAmountInvalid
	}
	note := strings.TrimSpace(input.Note)
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin deposit application transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	existing, err := s.repository.FindDepositApplicationByIdempotency(ctx, tx, input.DocumentID, input.IdempotencyKey)
	if err != nil {
		return nil, err
	}
	if existing != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate deposit application transaction: %w", err)
		}
		return s.GetReceivable(ctx, existing.TargetBillingDocumentID)
	}
	sourceDocument, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if sourceDocument == nil {
		return nil, ErrDocumentNotFound
	}
	if sourceDocument.Status != StatusApproved {
		return nil, ErrDepositNotAvailable
	}
	sourceLine := findDocumentLineByID(sourceDocument.Lines, input.BillingDocumentLineID)
	if sourceLine == nil {
		return nil, ErrDepositNotAvailable
	}
	sourceOpenItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, input.DocumentID, input.BillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if sourceOpenItem == nil || !sourceOpenItem.IsDeposit || sourceOpenItem.OutstandingAmount <= 0 {
		return nil, ErrDepositNotAvailable
	}
	if amount > roundMoney(sourceOpenItem.OutstandingAmount) {
		return nil, ErrDepositAmountInvalid
	}
	targetDocument, err := s.repository.FindByIDForUpdate(ctx, tx, input.TargetDocumentID)
	if err != nil {
		return nil, err
	}
	if targetDocument == nil {
		return nil, ErrDepositTargetNotAllowed
	}
	if targetDocument.Status != StatusApproved {
		return nil, ErrDepositTargetNotAllowed
	}
	targetOpenItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, input.TargetDocumentID, input.TargetBillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if targetOpenItem == nil || targetOpenItem.IsDeposit || targetOpenItem.OutstandingAmount <= 0 {
		return nil, ErrDepositTargetNotAllowed
	}
	if amount > roundMoney(targetOpenItem.OutstandingAmount) {
		return nil, ErrDepositTargetNotAllowed
	}
	appliedAt := time.Now().UTC()
	newSourceOutstanding := roundMoney(sourceOpenItem.OutstandingAmount - amount)
	var sourceSettledAt *time.Time
	if newSourceOutstanding == 0 {
		sourceSettledAt = &appliedAt
	}
	if err := s.repository.UpdateOpenItemBalance(ctx, tx, sourceOpenItem.ID, newSourceOutstanding, sourceSettledAt); err != nil {
		return nil, err
	}
	newTargetOutstanding := roundMoney(targetOpenItem.OutstandingAmount - amount)
	var targetSettledAt *time.Time
	if newTargetOutstanding == 0 {
		targetSettledAt = &appliedAt
	}
	if err := s.repository.UpdateOpenItemBalance(ctx, tx, targetOpenItem.ID, newTargetOutstanding, targetSettledAt); err != nil {
		return nil, err
	}
	var notePtr *string
	if note != "" {
		notePtr = &note
	}
	if err := s.repository.InsertDepositApplication(ctx, tx, DepositApplication{
		SourceBillingDocumentID:     sourceDocument.ID,
		SourceBillingDocumentLineID: input.BillingDocumentLineID,
		SourceAROpenItemID:          sourceOpenItem.ID,
		TargetBillingDocumentID:     targetDocument.ID,
		TargetBillingDocumentLineID: input.TargetBillingDocumentLineID,
		TargetAROpenItemID:          targetOpenItem.ID,
		LeaseContractID:             sourceDocument.LeaseContractID,
		Amount:                      amount,
		Note:                        notePtr,
		IdempotencyKey:              input.IdempotencyKey,
		AppliedBy:                   input.ActorUserID,
	}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit deposit application transaction: %w", err)
	}
	return s.GetReceivable(ctx, input.TargetDocumentID)
}

func (s *Service) RefundDeposit(ctx context.Context, input RefundDepositInput) (*ReceivableSummary, error) {
	if input.DocumentID == 0 || input.BillingDocumentLineID == 0 || input.ActorUserID == 0 || input.IdempotencyKey == "" {
		return nil, ErrInvalidDocumentInput
	}
	amount := roundMoney(input.Amount)
	if amount <= 0 {
		return nil, ErrDepositRefundAmountInvalid
	}
	reason := strings.TrimSpace(input.Reason)
	if reason == "" {
		return nil, ErrDepositRefundReasonRequired
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin deposit refund transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	existing, err := s.repository.FindDepositRefundByIdempotency(ctx, tx, input.DocumentID, input.IdempotencyKey)
	if err != nil {
		return nil, err
	}
	if existing != nil {
		if err := tx.Commit(); err != nil {
			return nil, fmt.Errorf("commit duplicate deposit refund transaction: %w", err)
		}
		return s.GetReceivable(ctx, input.DocumentID)
	}
	document, err := s.repository.FindByIDForUpdate(ctx, tx, input.DocumentID)
	if err != nil {
		return nil, err
	}
	if document == nil {
		return nil, ErrDocumentNotFound
	}
	if document.Status != StatusApproved {
		return nil, ErrDepositNotAvailable
	}
	openItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, input.DocumentID, input.BillingDocumentLineID)
	if err != nil {
		return nil, err
	}
	if openItem == nil || !openItem.IsDeposit || openItem.OutstandingAmount <= 0 {
		return nil, ErrDepositNotAvailable
	}
	if amount > roundMoney(openItem.OutstandingAmount) {
		return nil, ErrDepositRefundAmountInvalid
	}
	outstandingCount, err := s.repository.CountOutstandingReceivablesForLease(ctx, tx, document.LeaseContractID)
	if err != nil {
		return nil, err
	}
	if outstandingCount > 0 {
		return nil, ErrDepositRefundBlocked
	}
	pendingCount, err := s.repository.CountPendingFinancialWorkflowsForLease(ctx, tx, document.LeaseContractID)
	if err != nil {
		return nil, err
	}
	if pendingCount > 0 {
		return nil, ErrDepositRefundBlocked
	}
	newOutstanding := roundMoney(openItem.OutstandingAmount - amount)
	var settledAt *time.Time
	if newOutstanding == 0 {
		now := time.Now().UTC()
		settledAt = &now
	}
	if err := s.repository.UpdateOpenItemBalance(ctx, tx, openItem.ID, newOutstanding, settledAt); err != nil {
		return nil, err
	}
	if err := s.repository.InsertDepositRefund(ctx, tx, DepositRefund{
		BillingDocumentID:     document.ID,
		BillingDocumentLineID: input.BillingDocumentLineID,
		AROpenItemID:          openItem.ID,
		LeaseContractID:       document.LeaseContractID,
		Amount:                amount,
		Reason:                reason,
		IdempotencyKey:        input.IdempotencyKey,
		RefundedBy:            input.ActorUserID,
	}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit deposit refund transaction: %w", err)
	}
	return s.GetReceivable(ctx, input.DocumentID)
}

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
	if instance != nil && instance.DocumentType == string(DocumentTypeDiscount) {
		return s.syncDiscountWorkflowState(ctx, instance, actorUserID)
	}
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
				ChargeSource:          line.ChargeSource,
				OvertimeBillID:        line.OvertimeBillID,
				OvertimeFormulaID:     line.OvertimeFormulaID,
				OvertimeChargeID:      line.OvertimeChargeID,
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
			BillingDocumentID:         document.ID,
			DocumentNo:                document.DocumentNo,
			DocumentType:              document.DocumentType,
			TenantName:                document.TenantName,
			LeaseContractID:           document.LeaseContractID,
			OutstandingAmount:         0,
			CustomerSurplus:           0,
			SettlementStatus:          SettlementStatusSettled,
			Items:                     []OpenItem{},
			PaymentHistory:            []PaymentEntry{},
			DiscountHistory:           []Discount{},
			SurplusHistory:            []SurplusEntry{},
			InterestHistory:           []InterestEntry{},
			DepositApplicationHistory: []DepositApplication{},
			DepositRefundHistory:      []DepositRefund{},
		}, nil
	}
	if summary.Items == nil {
		summary.Items = []OpenItem{}
	}
	if summary.PaymentHistory == nil {
		summary.PaymentHistory = []PaymentEntry{}
	}
	if summary.DiscountHistory == nil {
		summary.DiscountHistory = []Discount{}
	}
	if summary.SurplusHistory == nil {
		summary.SurplusHistory = []SurplusEntry{}
	}
	if summary.InterestHistory == nil {
		summary.InterestHistory = []InterestEntry{}
	}
	if summary.DepositApplicationHistory == nil {
		summary.DepositApplicationHistory = []DepositApplication{}
	}
	if summary.DepositRefundHistory == nil {
		summary.DepositRefundHistory = []DepositRefund{}
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
	if remaining > 0 {
		balance, err := s.repository.UpsertSurplusBalance(ctx, tx, items[0].CustomerID, input.ActorUserID, remaining, nil)
		if err != nil {
			return nil, err
		}
		if err := s.repository.InsertSurplusEntry(ctx, tx, SurplusEntry{SurplusBalanceID: balance.ID, EntryType: SurplusEntryTypeOverpayment, CustomerID: items[0].CustomerID, BillingDocumentID: &document.ID, Amount: remaining, Note: note, IdempotencyKey: input.IdempotencyKey, RecordedBy: input.ActorUserID}); err != nil {
			return nil, err
		}
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

func roundRate(value float64) float64 {
	return math.Round(value*1_000_000) / 1_000_000
}

func documentFromChargeLines(input CreateInput, chargeLines []billing.ChargeLine) (*Document, error) {
	first := chargeLines[0]
	document := &Document{DocumentType: input.DocumentType, BillingRunID: first.BillingRunID, LeaseContractID: first.LeaseContractID, TenantName: first.TenantName, PeriodStart: first.PeriodStart, PeriodEnd: first.PeriodEnd, CurrencyTypeID: first.CurrencyTypeID, Status: StatusDraft, CreatedBy: input.ActorUserID, UpdatedBy: input.ActorUserID, Lines: make([]Line, 0, len(chargeLines))}
	totalAmount := 0.0
	for _, chargeLine := range chargeLines {
		if chargeLine.BillingRunID != document.BillingRunID || chargeLine.LeaseContractID != document.LeaseContractID || !chargeLine.PeriodStart.Equal(document.PeriodStart) || !chargeLine.PeriodEnd.Equal(document.PeriodEnd) || chargeLine.CurrencyTypeID != document.CurrencyTypeID {
			return nil, ErrInvalidDocumentInput
		}
		document.Lines = append(document.Lines, Line{BillingChargeLineID: chargeLine.ID, ChargeType: chargeLine.ChargeType, ChargeSource: chargeLine.ChargeSource, OvertimeBillID: chargeLine.OvertimeBillID, OvertimeFormulaID: chargeLine.OvertimeFormulaID, OvertimeChargeID: chargeLine.OvertimeChargeID, PeriodStart: chargeLine.PeriodStart, PeriodEnd: chargeLine.PeriodEnd, QuantityDays: chargeLine.QuantityDays, UnitAmount: chargeLine.UnitAmount, Amount: chargeLine.Amount})
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
		document.Lines = append(document.Lines, Line{BillingChargeLineID: line.BillingChargeLineID, ChargeType: line.ChargeType, ChargeSource: line.ChargeSource, OvertimeBillID: line.OvertimeBillID, OvertimeFormulaID: line.OvertimeFormulaID, OvertimeChargeID: line.OvertimeChargeID, PeriodStart: line.PeriodStart, PeriodEnd: line.PeriodEnd, QuantityDays: line.QuantityDays, UnitAmount: line.UnitAmount, Amount: amount})
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

func mapDiscountWorkflowState(instance *workflow.Instance) (DiscountStatus, *time.Time, *time.Time) {
	switch instance.Status {
	case workflow.InstanceStatusApproved:
		approvedAt := time.Now().UTC()
		if instance.CompletedAt != nil {
			approvedAt = instance.CompletedAt.UTC()
		}
		return DiscountStatusApproved, &approvedAt, nil
	case workflow.InstanceStatusRejected:
		rejectedAt := time.Now().UTC()
		if instance.CompletedAt != nil {
			rejectedAt = instance.CompletedAt.UTC()
		}
		return DiscountStatusRejected, nil, &rejectedAt
	default:
		return DiscountStatusPendingApproval, nil, nil
	}
}

func findDocumentLineByID(lines []Line, lineID int64) *Line {
	for _, line := range lines {
		if line.ID == lineID {
			copy := line
			return &copy
		}
	}
	return nil
}

func (s *Service) startDiscountWorkflow(ctx context.Context, discount *Discount, input ApplyDiscountInput) error {
	if discount == nil || discount.WorkflowInstanceID != nil {
		return nil
	}
	instance, err := s.workflowService.Start(ctx, workflow.StartInput{DefinitionCode: ApprovalDefinitionCode, DocumentType: string(DocumentTypeDiscount), DocumentID: discount.ID, ActorUserID: input.ActorUserID, DepartmentID: input.DepartmentID, IdempotencyKey: input.IdempotencyKey, Comment: discount.Reason})
	if err != nil {
		return err
	}
	now := time.Now().UTC()
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin discount submit transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	if err := s.repository.AttachDiscountWorkflowInstance(ctx, tx, discount.ID, instance.ID, input.ActorUserID, now); err != nil {
		return err
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit discount submit transaction: %w", err)
	}
	discount.WorkflowInstanceID = &instance.ID
	discount.Status = DiscountStatusPendingApproval
	discount.SubmittedAt = &now
	return nil
}

func (s *Service) syncDiscountWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error {
	if instance == nil || instance.DocumentType != string(DocumentTypeDiscount) {
		return nil
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin invoice discount workflow sync transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	discount, err := s.repository.FindDiscountByIDForUpdate(ctx, tx, instance.DocumentID)
	if err != nil {
		return err
	}
	if discount == nil {
		return nil
	}
	status, approvedAt, rejectedAt := mapDiscountWorkflowState(instance)
	if discount.Status == status && discount.WorkflowInstanceID != nil && *discount.WorkflowInstanceID == instance.ID {
		if err := tx.Commit(); err != nil {
			return fmt.Errorf("commit duplicate invoice discount workflow sync transaction: %w", err)
		}
		return nil
	}
	if status == DiscountStatusApproved {
		document, err := s.repository.FindByIDForUpdate(ctx, tx, discount.BillingDocumentID)
		if err != nil {
			return err
		}
		if document == nil || document.Status != StatusApproved {
			return ErrDiscountNotAllowed
		}
		openItem, err := s.repository.FindOpenItemByDocumentLineIDForUpdate(ctx, tx, discount.BillingDocumentID, discount.BillingDocumentLineID)
		if err != nil {
			return err
		}
		if openItem == nil || openItem.IsDeposit || openItem.OutstandingAmount <= 0 {
			return ErrDiscountNotAllowed
		}
		if discount.RequestedAmount > roundMoney(openItem.OutstandingAmount) {
			return ErrDiscountOverApplication
		}
		exists, err := s.repository.HasDiscountEntry(ctx, tx, discount.ID)
		if err != nil {
			return err
		}
		if !exists {
			newOutstanding := roundMoney(openItem.OutstandingAmount - discount.RequestedAmount)
			var settledAt *time.Time
			if newOutstanding == 0 {
				now := time.Now().UTC()
				settledAt = &now
			}
			if err := s.repository.UpdateOpenItemBalance(ctx, tx, openItem.ID, newOutstanding, settledAt); err != nil {
				return err
			}
			if err := s.repository.InsertDiscountEntry(ctx, tx, DiscountEntry{InvoiceDiscountID: discount.ID, BillingDocumentID: discount.BillingDocumentID, BillingDocumentLineID: discount.BillingDocumentLineID, AROpenItemID: openItem.ID, LeaseContractID: discount.LeaseContractID, Amount: discount.RequestedAmount, RecordedBy: actorUserID}); err != nil {
				return err
			}
		}
	}
	if err := s.repository.UpdateDiscountWorkflowState(ctx, tx, discount.ID, instance.ID, actorUserID, status, approvedAt, rejectedAt); err != nil {
		return err
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit invoice discount workflow sync transaction: %w", err)
	}
	return nil
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
