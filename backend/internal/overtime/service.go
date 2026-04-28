package overtime

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
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	mysql "github.com/go-sql-driver/mysql"
)

var (
	ErrOvertimeNotFound           = errors.New("overtime bill not found")
	ErrInvalidOvertimeInput       = errors.New("invalid overtime bill input")
	ErrInvalidOvertimeState       = errors.New("invalid overtime bill state")
	ErrOvertimeAlreadySubmitted   = errors.New("overtime bill already submitted")
	ErrOvertimeWorkflowRequired   = errors.New("overtime workflow service is required")
	ErrOvertimeGenerationLocked   = errors.New("overtime bill already has generated charges")
	ErrOvertimeGenerationBlocked  = errors.New("overtime bill is not eligible for generation")
	ErrOvertimeFormulaRequired    = errors.New("at least one overtime formula is required")
	ErrOvertimeFormulaInvalid     = errors.New("invalid overtime formula configuration")
	ErrOvertimeDuplicateBill      = errors.New("overtime bill already exists for the selected contract and period")
)

type Service struct {
	repository      *Repository
	billingRepo     *billing.Repository
	workflowService *workflow.Service
	db              *sql.DB
}

func NewService(db *sql.DB, repository *Repository, billingRepo *billing.Repository, workflowService *workflow.Service) *Service {
	return &Service{db: db, repository: repository, billingRepo: billingRepo, workflowService: workflowService}
}

func (s *Service) CreateBill(ctx context.Context, input CreateBillInput) (*Bill, error) {
	if input.LeaseContractID == 0 || input.ActorUserID == 0 || input.PeriodStart.IsZero() || input.PeriodEnd.IsZero() || input.PeriodStart.After(input.PeriodEnd) {
		return nil, ErrInvalidOvertimeInput
	}
	if len(input.Formulas) == 0 {
		return nil, ErrOvertimeFormulaRequired
	}
	formulas, err := normalizeFormulaInputs(input.Formulas, input.PeriodStart, input.PeriodEnd)
	if err != nil {
		return nil, err
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin overtime create transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	leaseCtx, err := s.repository.FindLeaseContextForUpdate(ctx, tx, input.LeaseContractID)
	if err != nil {
		return nil, err
	}
	if leaseCtx == nil || (leaseCtx.Status != string(lease.StatusActive) && leaseCtx.Status != string(lease.StatusTerminated)) {
		return nil, ErrInvalidOvertimeInput
	}
	bill := &Bill{
		LeaseContractID: input.LeaseContractID,
		PeriodStart:     dateOnly(input.PeriodStart),
		PeriodEnd:       dateOnly(input.PeriodEnd),
		Status:          BillStatusDraft,
		Note:            strings.TrimSpace(input.Note),
		CreatedBy:       input.ActorUserID,
		UpdatedBy:       input.ActorUserID,
		Formulas:        formulas,
	}
	if err := s.repository.CreateBill(ctx, tx, bill); err != nil {
		if isDuplicateEntry(err) {
			return nil, ErrOvertimeDuplicateBill
		}
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit overtime create transaction: %w", err)
	}
	return s.GetBill(ctx, bill.ID)
}

func normalizeFormulaInputs(inputs []FormulaInput, billStart, billEnd time.Time) ([]Formula, error) {
	formulas := make([]Formula, 0, len(inputs))
	for index, input := range inputs {
		chargeType := strings.TrimSpace(input.ChargeType)
		if chargeType == "" || input.CurrencyTypeID == 0 {
			return nil, ErrOvertimeFormulaInvalid
		}
		if input.FormulaType != FormulaTypeFixed && input.FormulaType != FormulaTypeOneTime && input.FormulaType != FormulaTypePercentage {
			return nil, ErrOvertimeFormulaInvalid
		}
		if input.RateType != RateTypeDaily && input.RateType != RateTypeMonthly {
			return nil, ErrOvertimeFormulaInvalid
		}
		effectiveFrom := dateOnly(input.EffectiveFrom)
		effectiveTo := dateOnly(input.EffectiveTo)
		if effectiveFrom.IsZero() {
			effectiveFrom = dateOnly(billStart)
		}
		if effectiveTo.IsZero() {
			effectiveTo = dateOnly(billEnd)
		}
		if effectiveFrom.After(effectiveTo) {
			return nil, ErrOvertimeFormulaInvalid
		}
		formula := Formula{
			ChargeType:       chargeType,
			FormulaType:      input.FormulaType,
			RateType:         input.RateType,
			EffectiveFrom:    effectiveFrom,
			EffectiveTo:      effectiveTo,
			CurrencyTypeID:   input.CurrencyTypeID,
			TotalArea:        roundMoney(input.TotalArea),
			UnitPrice:        roundMoney(input.UnitPrice),
			BaseAmount:       roundMoney(input.BaseAmount),
			FixedRental:      roundMoney(input.FixedRental),
			PercentageOption: strings.TrimSpace(input.PercentageOption),
			MinimumOption:    strings.TrimSpace(input.MinimumOption),
			SortOrder:        index + 1,
		}
		formula.PercentageTiers = normalizePercentTiers(input.PercentageTiers)
		formula.MinimumTiers = normalizeMinimumTiers(input.MinimumTiers)
		if formula.FormulaType == FormulaTypePercentage && len(formula.PercentageTiers) == 0 {
			return nil, ErrOvertimeFormulaInvalid
		}
		formulas = append(formulas, formula)
	}
	return formulas, nil
}

func normalizePercentTiers(input []PercentTierInput) []PercentTier {
	out := make([]PercentTier, 0, len(input))
	for index, tier := range input {
		out = append(out, PercentTier{SalesTo: roundMoney(tier.SalesTo), Percentage: roundRate(tier.Percentage), SortOrder: index + 1})
	}
	sort.Slice(out, func(i, j int) bool { return out[i].SalesTo < out[j].SalesTo })
	for index := range out {
		out[index].SortOrder = index + 1
	}
	return out
}

func normalizeMinimumTiers(input []MinimumTierInput) []MinimumTier {
	out := make([]MinimumTier, 0, len(input))
	for index, tier := range input {
		out = append(out, MinimumTier{SalesTo: roundMoney(tier.SalesTo), MinimumSum: roundMoney(tier.MinimumSum), SortOrder: index + 1})
	}
	sort.Slice(out, func(i, j int) bool { return out[i].SalesTo < out[j].SalesTo })
	for index := range out {
		out[index].SortOrder = index + 1
	}
	return out
}

func (s *Service) GetBill(ctx context.Context, billID int64) (*Bill, error) {
	bill, err := s.repository.FindBillByID(ctx, billID)
	if err != nil {
		return nil, err
	}
	if bill == nil {
		return nil, ErrOvertimeNotFound
	}
	return bill, nil
}

func (s *Service) ListBills(ctx context.Context, filter ListFilter) (*pagination.ListResult[Bill], error) {
	return s.repository.ListBills(ctx, filter)
}

func (s *Service) SubmitForApproval(ctx context.Context, input SubmitInput) (*Bill, error) {
	if input.BillID == 0 || input.ActorUserID == 0 || input.DepartmentID == 0 || input.IdempotencyKey == "" || s.workflowService == nil {
		return nil, ErrOvertimeWorkflowRequired
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin overtime submit transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	bill, err := s.repository.FindBillByIDForUpdate(ctx, tx, input.BillID)
	if err != nil {
		return nil, err
	}
	if bill == nil {
		return nil, ErrOvertimeNotFound
	}
	if bill.Status != BillStatusDraft && bill.Status != BillStatusRejected {
		return nil, ErrInvalidOvertimeState
	}
	instance, err := s.workflowService.Start(ctx, workflow.StartInput{
		DefinitionCode: ApprovalDefinitionCode,
		DocumentType:   DocumentTypeBill,
		DocumentID:     bill.ID,
		ActorUserID:    input.ActorUserID,
		DepartmentID:   input.DepartmentID,
		IdempotencyKey: input.IdempotencyKey,
		Comment:        input.Comment,
	})
	if err != nil {
		return nil, err
	}
	if instance == nil {
		return nil, ErrInvalidOvertimeState
	}
	if err := s.repository.AttachWorkflowInstance(ctx, tx, bill.ID, instance.ID, input.ActorUserID, instance.SubmittedAt); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit overtime submit transaction: %w", err)
	}
	return s.GetBill(ctx, bill.ID)
}

func (s *Service) SyncWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error {
	if instance == nil || instance.DocumentType != DocumentTypeBill {
		return nil
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin overtime workflow sync transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	bill, err := s.repository.FindBillByIDForUpdate(ctx, tx, instance.DocumentID)
	if err != nil {
		return err
	}
	if bill == nil {
		return nil
	}
	status, approvedAt, rejectedAt := mapWorkflowState(instance)
	if bill.Status == status && bill.WorkflowInstanceID != nil && *bill.WorkflowInstanceID == instance.ID {
		if err := tx.Commit(); err != nil {
			return fmt.Errorf("commit duplicate overtime workflow sync transaction: %w", err)
		}
		return nil
	}
	if err := s.repository.UpdateWorkflowState(ctx, tx, bill.ID, instance.ID, actorUserID, status, approvedAt, rejectedAt); err != nil {
		return err
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit overtime workflow sync transaction: %w", err)
	}
	return nil
}

func mapWorkflowState(instance *workflow.Instance) (BillStatus, *time.Time, *time.Time) {
	switch instance.Status {
	case workflow.InstanceStatusApproved:
		approvedAt := time.Now().UTC()
		if instance.CompletedAt != nil {
			approvedAt = instance.CompletedAt.UTC()
		}
		return BillStatusApproved, &approvedAt, nil
	case workflow.InstanceStatusRejected:
		rejectedAt := time.Now().UTC()
		if instance.CompletedAt != nil {
			rejectedAt = instance.CompletedAt.UTC()
		}
		return BillStatusRejected, nil, &rejectedAt
	default:
		return BillStatusPendingApproval, nil, nil
	}
}

func (s *Service) Cancel(ctx context.Context, input CancelInput) (*Bill, error) {
	return s.transitionToTerminalState(ctx, input.BillID, input.ActorUserID, BillStatusCancelled)
}

func (s *Service) Stop(ctx context.Context, input StopInput) (*Bill, error) {
	return s.transitionToTerminalState(ctx, input.BillID, input.ActorUserID, BillStatusStopped)
}

func (s *Service) transitionToTerminalState(ctx context.Context, billID, actorUserID int64, target BillStatus) (*Bill, error) {
	if billID == 0 || actorUserID == 0 {
		return nil, ErrInvalidOvertimeInput
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin overtime status transition transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	bill, err := s.repository.FindBillByIDForUpdate(ctx, tx, billID)
	if err != nil {
		return nil, err
	}
	if bill == nil {
		return nil, ErrOvertimeNotFound
	}
	generatedCount, err := s.repository.CountGeneratedChargesByBill(ctx, tx, bill.ID)
	if err != nil {
		return nil, err
	}
	if generatedCount > 0 {
		return nil, ErrOvertimeGenerationLocked
	}
	if target == BillStatusCancelled {
		if bill.Status != BillStatusDraft && bill.Status != BillStatusRejected && bill.Status != BillStatusPendingApproval && bill.Status != BillStatusApproved {
			return nil, ErrInvalidOvertimeState
		}
		if err := s.repository.MarkCancelled(ctx, tx, bill.ID, actorUserID, time.Now().UTC()); err != nil {
			return nil, err
		}
	} else {
		if bill.Status != BillStatusApproved {
			return nil, ErrInvalidOvertimeState
		}
		if err := s.repository.MarkStopped(ctx, tx, bill.ID, actorUserID, time.Now().UTC()); err != nil {
			return nil, err
		}
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit overtime status transition transaction: %w", err)
	}
	return s.GetBill(ctx, bill.ID)
}

func (s *Service) GenerateCharges(ctx context.Context, input GenerateInput) (*GenerateResult, error) {
	if input.BillID == 0 || input.ActorUserID == 0 || s.billingRepo == nil {
		return nil, ErrInvalidOvertimeInput
	}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin overtime generation transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	bill, err := s.repository.FindBillByIDForUpdate(ctx, tx, input.BillID)
	if err != nil {
		return nil, err
	}
	if bill == nil {
		return nil, ErrOvertimeNotFound
	}
	if bill.Status != BillStatusApproved && bill.Status != BillStatusGenerated {
		return nil, ErrOvertimeGenerationBlocked
	}
	run := &billing.Run{PeriodStart: bill.PeriodStart, PeriodEnd: bill.PeriodEnd, Status: billing.RunStatusCompleted, TriggeredBy: input.ActorUserID}
	if err := s.billingRepo.CreateRun(ctx, tx, run); err != nil {
		return nil, err
	}
	charges := make([]GeneratedCharge, 0)
	skipped := make([]SkippedGeneration, 0)
	generatedCount := 0
	skippedCount := 0
	for _, formula := range bill.Formulas {
		charge, skipReason, ok := buildGeneratedCharge(*bill, formula, input.ActorUserID)
		if !ok {
			skipped = append(skipped, SkippedGeneration{FormulaID: formula.ID, Reason: skipReason})
			skippedCount++
			continue
		}
		charge.BillingRunID = run.ID
		charge.WorkflowInstanceID = bill.WorkflowInstanceID
		if err := s.repository.InsertGeneratedCharge(ctx, tx, &charge); err != nil {
			if isDuplicateEntry(err) {
				skipped = append(skipped, SkippedGeneration{FormulaID: formula.ID, Reason: "duplicate_generation"})
				skippedCount++
				continue
			}
			return nil, err
		}
		downstreamLine := buildDownstreamChargeLine(run.ID, *bill, formula, charge)
		if err := s.billingRepo.InsertChargeLine(ctx, tx, downstreamLine); err != nil {
			if isDuplicateEntry(err) {
				skipped = append(skipped, SkippedGeneration{FormulaID: formula.ID, Reason: "duplicate_generation"})
				skippedCount++
				continue
			}
			return nil, fmt.Errorf("insert overtime downstream billing charge line: %w", err)
		}
		generatedCount++
		charges = append(charges, charge)
	}
	if err := s.billingRepo.UpdateRunCounts(ctx, tx, run.ID, generatedCount, skippedCount); err != nil {
		return nil, err
	}
	if generatedCount > 0 || bill.Status == BillStatusGenerated {
		if err := s.repository.MarkGenerated(ctx, tx, bill.ID, input.ActorUserID, time.Now().UTC()); err != nil {
			return nil, err
		}
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit overtime generation transaction: %w", err)
	}
	storedRun, err := s.billingRepo.GetRun(ctx, run.ID)
	if err != nil {
		return nil, err
	}
	if storedRun != nil {
		run = storedRun
	}
	return &GenerateResult{Run: run, Charges: charges, Skipped: skipped, Totals: Totals{Generated: generatedCount, Skipped: skippedCount}}, nil
}

func buildGeneratedCharge(bill Bill, formula Formula, actorUserID int64) (GeneratedCharge, string, bool) {
	periodStart := maxDate(bill.PeriodStart, formula.EffectiveFrom)
	periodEnd := minDate(bill.PeriodEnd, formula.EffectiveTo)
	if periodEnd.Before(periodStart) {
		return GeneratedCharge{}, "outside_formula_window", false
	}
	quantity := 1
	if formula.RateType == RateTypeDaily {
		quantity = inclusiveDays(periodStart, periodEnd)
	}
	unitAmount := resolveUnitAmount(formula)
	amount := unitAmount
	appliedRate := 0.0
	appliedMinimum := 0.0
	switch formula.FormulaType {
	case FormulaTypeFixed:
		amount = unitAmount * float64(quantity)
	case FormulaTypeOneTime:
		amount = unitAmount
		quantity = 1
	case FormulaTypePercentage:
		appliedRate = resolvePercentageRate(formula.PercentageTiers, formula.BaseAmount)
		if appliedRate <= 0 {
			return GeneratedCharge{}, "missing_percentage_rate", false
		}
		unitAmount = roundMoney(formula.BaseAmount * appliedRate)
		amount = unitAmount
		if formula.RateType == RateTypeDaily {
			amount = roundMoney(unitAmount * float64(quantity))
		}
		appliedMinimum = resolveMinimumAmount(formula.MinimumTiers, formula.BaseAmount)
		if appliedMinimum > 0 && amount < appliedMinimum {
			amount = appliedMinimum
		}
	}
	amount = roundMoney(amount)
	unitAmount = roundMoney(unitAmount)
	if amount <= 0 {
		return GeneratedCharge{}, "zero_amount", false
	}
	return GeneratedCharge{
		OvertimeBillID:        bill.ID,
		OvertimeFormulaID:     formula.ID,
		LeaseContractID:       bill.LeaseContractID,
		ChargeType:            formula.ChargeType,
		FormulaType:           formula.FormulaType,
		RateType:              formula.RateType,
		PeriodStart:           periodStart,
		PeriodEnd:             periodEnd,
		Quantity:              quantity,
		TotalArea:             formula.TotalArea,
		UnitPrice:             formula.UnitPrice,
		BaseAmount:            formula.BaseAmount,
		FixedRental:           formula.FixedRental,
		PercentageOption:      formula.PercentageOption,
		MinimumOption:         formula.MinimumOption,
		AppliedPercentageRate: appliedRate,
		AppliedMinimumAmount:  appliedMinimum,
		UnitAmount:            unitAmount,
		Amount:                amount,
		CurrencyTypeID:        formula.CurrencyTypeID,
		GeneratedBy:           actorUserID,
	}, "", true
}

func buildDownstreamChargeLine(runID int64, bill Bill, formula Formula, charge GeneratedCharge) *billing.ChargeLine {
	overtimeBillID := bill.ID
	overtimeFormulaID := formula.ID
	overtimeChargeID := charge.ID
	return &billing.ChargeLine{
		BillingRunID:           runID,
		LeaseContractID:        bill.LeaseContractID,
		LeaseNo:                bill.LeaseNo,
		TenantName:             bill.TenantName,
		ChargeType:             charge.ChargeType,
		ChargeSource:           billing.ChargeSourceOvertime,
		OvertimeBillID:         &overtimeBillID,
		OvertimeFormulaID:      &overtimeFormulaID,
		OvertimeChargeID:       &overtimeChargeID,
		PeriodStart:            charge.PeriodStart,
		PeriodEnd:              charge.PeriodEnd,
		QuantityDays:           charge.Quantity,
		UnitAmount:             charge.UnitAmount,
		Amount:                 charge.Amount,
		CurrencyTypeID:         charge.CurrencyTypeID,
		SourceEffectiveVersion: 1,
	}
}

func resolveUnitAmount(formula Formula) float64 {
	if formula.FixedRental > 0 {
		return roundMoney(formula.FixedRental)
	}
	if formula.TotalArea > 0 && formula.UnitPrice > 0 {
		return roundMoney(formula.TotalArea * formula.UnitPrice)
	}
	return roundMoney(formula.BaseAmount)
}

func resolvePercentageRate(tiers []PercentTier, baseAmount float64) float64 {
	best := 0.0
	for _, tier := range tiers {
		if baseAmount >= tier.SalesTo {
			best = tier.Percentage
		}
	}
	return roundRate(best)
}

func resolveMinimumAmount(tiers []MinimumTier, baseAmount float64) float64 {
	best := 0.0
	for _, tier := range tiers {
		if baseAmount >= tier.SalesTo {
			best = tier.MinimumSum
		}
	}
	return roundMoney(best)
}

func dateOnly(value time.Time) time.Time {
	return time.Date(value.Year(), value.Month(), value.Day(), 0, 0, 0, 0, time.UTC)
}

func inclusiveDays(start, end time.Time) int {
	return int(dateOnly(end).Sub(dateOnly(start)).Hours()/24) + 1
}

func maxDate(values ...time.Time) time.Time {
	max := dateOnly(values[0])
	for _, value := range values[1:] {
		current := dateOnly(value)
		if current.After(max) {
			max = current
		}
	}
	return max
}

func minDate(values ...time.Time) time.Time {
	min := dateOnly(values[0])
	for _, value := range values[1:] {
		current := dateOnly(value)
		if current.Before(min) {
			min = current
		}
	}
	return min
}

func roundMoney(value float64) float64 {
	return math.Round(value*100) / 100
}

func roundRate(value float64) float64 {
	return math.Round(value*10000) / 10000
}

func isDuplicateEntry(err error) bool {
	var mysqlErr *mysql.MySQLError
	return errors.As(err, &mysqlErr) && mysqlErr.Number == 1062
}
