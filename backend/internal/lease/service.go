package lease

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	mysql "github.com/go-sql-driver/mysql"
)

var (
	ErrLeaseNotFound                = errors.New("lease contract not found")
	ErrInvalidLeaseState            = errors.New("invalid lease contract state")
	ErrLeaseAlreadySubmitted        = errors.New("lease contract already submitted")
	ErrLeaseHasBillingDocuments     = errors.New("lease contract has in-flight billing documents")
	ErrDuplicateLeaseNo             = errors.New("duplicate lease contract number")
	ErrLeaseIncompleteForSubmission = errors.New("lease contract is incomplete for submission")
)

type Service struct {
	repository      *Repository
	db              *sql.DB
	workflowService *workflow.Service
}

func NewService(db *sql.DB, repository *Repository, workflowService *workflow.Service) *Service {
	return &Service{db: db, repository: repository, workflowService: workflowService}
}

func (s *Service) CreateDraft(ctx context.Context, input CreateDraftInput) (*Contract, error) {
	contract, err := contractFromCreateInput(input)
	if err != nil {
		return nil, err
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin lease create transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	if err := s.repository.Create(ctx, tx, contract); err != nil {
		if isDuplicateEntry(err) {
			return nil, ErrDuplicateLeaseNo
		}
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit lease create transaction: %w", err)
	}

	return s.repository.FindByID(ctx, contract.ID)
}

func (s *Service) GetLease(ctx context.Context, leaseID int64) (*Contract, error) {
	contract, err := s.repository.FindByID(ctx, leaseID)
	if err != nil {
		return nil, err
	}
	if contract == nil {
		return nil, ErrLeaseNotFound
	}
	return contract, nil
}

func (s *Service) CreateAmendment(ctx context.Context, input AmendInput) (*Contract, error) {
	baseContract, err := s.repository.FindByID(ctx, input.LeaseID)
	if err != nil {
		return nil, err
	}
	if baseContract == nil {
		return nil, ErrLeaseNotFound
	}
	if baseContract.Status != StatusActive {
		return nil, ErrInvalidLeaseState
	}

	contract, err := contractFromCreateInput(input.CreateDraftInput)
	if err != nil {
		return nil, err
	}
	contract.AmendedFromID = &baseContract.ID
	contract.EffectiveVersion = baseContract.EffectiveVersion + 1

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin lease amendment create transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	if err := s.repository.Create(ctx, tx, contract); err != nil {
		if isDuplicateEntry(err) {
			return nil, ErrDuplicateLeaseNo
		}
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit lease amendment create transaction: %w", err)
	}

	return s.repository.FindByID(ctx, contract.ID)
}

func (s *Service) ListLeases(ctx context.Context, filter ListFilter) (*ListResult, error) {
	return s.repository.List(ctx, filter)
}

func (s *Service) SubmitForApproval(ctx context.Context, input SubmitInput) (*Contract, error) {
	if strings.TrimSpace(input.IdempotencyKey) == "" {
		return nil, ErrLeaseIncompleteForSubmission
	}
	if s.workflowService == nil {
		return nil, fmt.Errorf("workflow service unavailable")
	}

	contract, err := s.repository.FindByID(ctx, input.LeaseID)
	if err != nil {
		return nil, err
	}
	if contract == nil {
		return nil, ErrLeaseNotFound
	}
	if contract.Status != StatusDraft {
		if contract.WorkflowInstanceID != nil {
			return nil, ErrLeaseAlreadySubmitted
		}
		return nil, ErrInvalidLeaseState
	}
	if !isSubmissionReady(contract) {
		return nil, ErrLeaseIncompleteForSubmission
	}
	definitionCode := ApprovalDefinitionCode
	if contract.AmendedFromID != nil {
		definitionCode = ChangeDefinitionCode
	}

	instance, err := s.workflowService.Start(ctx, workflow.StartInput{
		DefinitionCode: definitionCode,
		DocumentType:   DocumentTypeContract,
		DocumentID:     contract.ID,
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
		return nil, fmt.Errorf("begin lease submit transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	if err := s.repository.AttachWorkflowInstance(ctx, tx, contract.ID, instance.ID, input.ActorUserID, now); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit lease submit transaction: %w", err)
	}

	return s.repository.FindByID(ctx, contract.ID)
}

func (s *Service) Terminate(ctx context.Context, input TerminateInput) (*Contract, error) {
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin lease terminate transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	contract, err := s.repository.FindByIDForUpdate(ctx, tx, input.LeaseID)
	if err != nil {
		return nil, err
	}
	if contract == nil {
		return nil, ErrLeaseNotFound
	}
	if contract.Status != StatusActive {
		return nil, ErrInvalidLeaseState
	}
	blockingDocuments, err := s.repository.CountBlockingBillingDocuments(ctx, tx, contract.ID)
	if err != nil {
		return nil, err
	}
	if blockingDocuments > 0 {
		return nil, ErrLeaseHasBillingDocuments
	}

	if err := s.repository.Terminate(ctx, tx, contract.ID, input.ActorUserID, input.TerminatedAt); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit lease terminate transaction: %w", err)
	}

	return s.repository.FindByID(ctx, contract.ID)
}

func (s *Service) SyncWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error {
	if instance == nil || instance.DocumentType != DocumentTypeContract {
		return nil
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin lease workflow sync transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	contract, err := s.repository.FindByIDForUpdate(ctx, tx, instance.DocumentID)
	if err != nil {
		return err
	}
	if contract == nil {
		return nil
	}

	status, approvedAt, billingEffectiveAt := mapWorkflowState(instance)
	if contract.Status == status && contract.WorkflowInstanceID != nil && *contract.WorkflowInstanceID == instance.ID {
		if err := tx.Commit(); err != nil {
			return fmt.Errorf("commit duplicate lease workflow sync transaction: %w", err)
		}
		return nil
	}

	if err := s.repository.UpdateWorkflowState(ctx, tx, contract.ID, instance.ID, actorUserID, status, approvedAt, billingEffectiveAt, nil); err != nil {
		return err
	}
	if status == StatusActive && contract.AmendedFromID != nil && approvedAt != nil {
		if err := s.repository.Terminate(ctx, tx, *contract.AmendedFromID, actorUserID, *approvedAt); err != nil {
			return err
		}
	}

	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit lease workflow sync transaction: %w", err)
	}
	return nil
}

func contractFromCreateInput(input CreateDraftInput) (*Contract, error) {
	leaseNo := strings.TrimSpace(input.LeaseNo)
	tenantName := strings.TrimSpace(input.TenantName)
	if leaseNo == "" || tenantName == "" || input.DepartmentID == 0 || input.StoreID == 0 || input.ActorUserID == 0 {
		return nil, ErrLeaseIncompleteForSubmission
	}
	if !input.StartDate.Before(input.EndDate) {
		return nil, ErrLeaseIncompleteForSubmission
	}
	if len(input.Units) == 0 || len(input.Terms) == 0 {
		return nil, ErrLeaseIncompleteForSubmission
	}

	units := make([]Unit, 0, len(input.Units))
	for _, unit := range input.Units {
		if unit.UnitID == 0 || unit.RentArea <= 0 {
			return nil, ErrLeaseIncompleteForSubmission
		}
		units = append(units, Unit{UnitID: unit.UnitID, RentArea: unit.RentArea})
	}

	terms := make([]Term, 0, len(input.Terms))
	for _, term := range input.Terms {
		if !isValidTerm(term) {
			return nil, ErrLeaseIncompleteForSubmission
		}
		terms = append(terms, Term{
			TermType:       term.TermType,
			BillingCycle:   term.BillingCycle,
			CurrencyTypeID: term.CurrencyTypeID,
			Amount:         term.Amount,
			EffectiveFrom:  term.EffectiveFrom,
			EffectiveTo:    term.EffectiveTo,
		})
	}

	return &Contract{
		LeaseNo:          leaseNo,
		DepartmentID:     input.DepartmentID,
		StoreID:          input.StoreID,
		BuildingID:       input.BuildingID,
		CustomerID:       input.CustomerID,
		BrandID:          input.BrandID,
		TradeID:          input.TradeID,
		ManagementTypeID: input.ManagementTypeID,
		TenantName:       tenantName,
		StartDate:        input.StartDate,
		EndDate:          input.EndDate,
		Status:           StatusDraft,
		EffectiveVersion: 1,
		CreatedBy:        input.ActorUserID,
		UpdatedBy:        input.ActorUserID,
		Units:            units,
		Terms:            terms,
	}, nil
}

func isSubmissionReady(contract *Contract) bool {
	if contract == nil {
		return false
	}
	if strings.TrimSpace(contract.LeaseNo) == "" || strings.TrimSpace(contract.TenantName) == "" {
		return false
	}
	if contract.DepartmentID == 0 || contract.StoreID == 0 || !contract.StartDate.Before(contract.EndDate) {
		return false
	}
	if len(contract.Units) == 0 || len(contract.Terms) == 0 {
		return false
	}
	for _, unit := range contract.Units {
		if unit.UnitID == 0 || unit.RentArea <= 0 {
			return false
		}
	}
	for _, term := range contract.Terms {
		if !isValidTerm(TermInput{TermType: term.TermType, BillingCycle: term.BillingCycle, CurrencyTypeID: term.CurrencyTypeID, Amount: term.Amount, EffectiveFrom: term.EffectiveFrom, EffectiveTo: term.EffectiveTo}) {
			return false
		}
	}
	return true
}

func isValidTerm(term TermInput) bool {
	if term.CurrencyTypeID == 0 || term.Amount < 0 {
		return false
	}
	if term.TermType != TermTypeRent && term.TermType != TermTypeDeposit {
		return false
	}
	if term.BillingCycle != BillingCycleMonthly {
		return false
	}
	return !term.EffectiveFrom.After(term.EffectiveTo)
}

func mapWorkflowState(instance *workflow.Instance) (Status, *time.Time, *time.Time) {
	switch instance.Status {
	case workflow.InstanceStatusApproved:
		approvedAt := time.Now().UTC()
		if instance.CompletedAt != nil {
			approvedAt = instance.CompletedAt.UTC()
		}
		return StatusActive, &approvedAt, &approvedAt
	case workflow.InstanceStatusRejected:
		return StatusRejected, nil, nil
	default:
		return StatusPendingApproval, nil, nil
	}
}

func isDuplicateEntry(err error) bool {
	var mysqlErr *mysql.MySQLError
	return errors.As(err, &mysqlErr) && mysqlErr.Number == 1062
}
