//go:build integration

package overtime_test

import (
	"context"
	"errors"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/overtime"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

var overtimeWorkflowNow = func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }

func TestOvertimeServiceApproveGenerateAndDeduplicate(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, overtimeWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	service := overtime.NewService(db, overtime.NewRepository(db), billingRepo, workflowService)
	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-OT-101", 12000, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)), "submit-ot-101")
	bill, err := service.CreateBill(ctx, overtime.CreateBillInput{LeaseContractID: activeLease.ID, PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), Note: "April overtime", ActorUserID: 101, Formulas: sampleFormulaInputs()})
	if err != nil {
		t.Fatalf("create overtime bill: %v", err)
	}
	if bill.Status != overtime.BillStatusDraft || len(bill.Formulas) != 3 {
		t.Fatalf("unexpected draft overtime bill %#v", bill)
	}
	submitted, err := service.SubmitForApproval(ctx, overtime.SubmitInput{BillID: bill.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-ot-bill-101", Comment: "submit overtime"})
	if err != nil {
		t.Fatalf("submit overtime bill: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-ot-101-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve overtime step 1: %v", err)
	}
	if err := service.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync overtime step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-ot-101-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve overtime step 2: %v", err)
	}
	if err := service.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync overtime step 2: %v", err)
	}
	approved, err := service.GetBill(ctx, bill.ID)
	if err != nil {
		t.Fatalf("get approved overtime bill: %v", err)
	}
	if approved.Status != overtime.BillStatusApproved || approved.ApprovedAt == nil {
		t.Fatalf("expected approved overtime bill, got %#v", approved)
	}
	result, err := service.GenerateCharges(ctx, overtime.GenerateInput{BillID: bill.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate overtime charges: %v", err)
	}
	if result.Totals.Generated != 3 || result.Totals.Skipped != 0 || len(result.Charges) != 3 {
		t.Fatalf("unexpected overtime generation result %#v", result)
	}
	if result.Charges[0].Amount != 600 || result.Charges[1].Amount != 1500 || result.Charges[2].Amount != 800 {
		t.Fatalf("unexpected overtime generated amounts %#v", result.Charges)
	}
	rerun, err := service.GenerateCharges(ctx, overtime.GenerateInput{BillID: bill.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("rerun overtime generation: %v", err)
	}
	if rerun.Totals.Generated != 0 || rerun.Totals.Skipped != 3 {
		t.Fatalf("expected duplicate-safe rerun, got %#v", rerun)
	}
	stored, err := service.GetBill(ctx, bill.ID)
	if err != nil {
		t.Fatalf("get generated overtime bill: %v", err)
	}
	if stored.Status != overtime.BillStatusGenerated || len(stored.GeneratedCharges) != 3 || stored.GeneratedAt == nil {
		t.Fatalf("expected generated overtime bill state, got %#v", stored)
	}
	lines, err := billingRepo.ListChargeLines(ctx, billing.ChargeListFilter{LeaseContractID: &activeLease.ID, PeriodStart: &bill.PeriodStart, PeriodEnd: &bill.PeriodEnd, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list downstream billing charge lines: %v", err)
	}
	if lines.Total != 3 || len(lines.Items) != 3 {
		t.Fatalf("expected 3 downstream billing charge lines, got %#v", lines)
	}
	for _, line := range lines.Items {
		if line.ChargeSource != billing.ChargeSourceOvertime || line.OvertimeBillID == nil || *line.OvertimeBillID != bill.ID || line.OvertimeChargeID == nil {
			t.Fatalf("expected overtime provenance on downstream line, got %#v", line)
		}
		if line.LeaseTermID != nil {
			t.Fatalf("expected overtime downstream line to omit lease term id, got %#v", line)
		}
	}
}

func TestOvertimeServiceStopBlocksFutureGeneration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, overtimeWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	service := overtime.NewService(db, overtime.NewRepository(db), billing.NewRepository(db), workflowService)
	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-OT-201", 12000, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)), "submit-ot-201")
	bill, err := service.CreateBill(ctx, overtime.CreateBillInput{LeaseContractID: activeLease.ID, PeriodStart: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC), ActorUserID: 101, Formulas: sampleFormulaInputs()[:1]})
	if err != nil {
		t.Fatalf("create overtime bill: %v", err)
	}
	submitted, err := service.SubmitForApproval(ctx, overtime.SubmitInput{BillID: bill.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-ot-bill-201", Comment: "submit overtime"})
	if err != nil {
		t.Fatalf("submit overtime bill: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-ot-201-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve overtime step 1: %v", err)
	}
	if err := service.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync overtime step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-ot-201-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve overtime step 2: %v", err)
	}
	if err := service.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync overtime step 2: %v", err)
	}
	stopped, err := service.Stop(ctx, overtime.StopInput{BillID: bill.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("stop overtime bill: %v", err)
	}
	if stopped.Status != overtime.BillStatusStopped || stopped.StoppedAt == nil {
		t.Fatalf("expected stopped overtime bill, got %#v", stopped)
	}
	if _, err := service.GenerateCharges(ctx, overtime.GenerateInput{BillID: bill.ID, ActorUserID: 101}); !errors.Is(err, overtime.ErrOvertimeGenerationBlocked) {
		t.Fatalf("expected stopped bill to block generation, got %v", err)
	}
}

func sampleFormulaInputs() []overtime.FormulaInput {
	return []overtime.FormulaInput{
		{ChargeType: "overtime_rent", FormulaType: overtime.FormulaTypeFixed, RateType: overtime.RateTypeDaily, EffectiveFrom: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), EffectiveTo: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), CurrencyTypeID: 101, TotalArea: 10, UnitPrice: 2},
		{ChargeType: "overtime_misc", FormulaType: overtime.FormulaTypeOneTime, RateType: overtime.RateTypeMonthly, EffectiveFrom: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), EffectiveTo: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), CurrencyTypeID: 101, FixedRental: 1500},
		{ChargeType: "overtime_sales", FormulaType: overtime.FormulaTypePercentage, RateType: overtime.RateTypeMonthly, EffectiveFrom: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), EffectiveTo: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), CurrencyTypeID: 101, BaseAmount: 10000, PercentageOption: "tiered", MinimumOption: "tiered", PercentageTiers: []overtime.PercentTierInput{{SalesTo: 0, Percentage: 0.05}}, MinimumTiers: []overtime.MinimumTierInput{{SalesTo: 0, MinimumSum: 800}}},
	}
}

func newLeaseCreateInput(leaseNo string, amount float64, effectiveFrom, effectiveTo time.Time) lease.CreateDraftInput {
	return lease.CreateDraftInput{
		LeaseNo:      leaseNo,
		DepartmentID: 101,
		StoreID:      101,
		BuildingID:   int64Pointer(101),
		TenantName:   "OT Tenant",
		StartDate:    effectiveFrom,
		EndDate:      effectiveTo,
		Units:        []lease.UnitInput{{UnitID: 101, RentArea: 118}},
		Terms: []lease.TermInput{{
			TermType:       lease.TermTypeRent,
			BillingCycle:   lease.BillingCycleMonthly,
			CurrencyTypeID: 101,
			Amount:         amount,
			EffectiveFrom:  effectiveFrom,
			EffectiveTo:    effectiveTo,
		}},
		ActorUserID: 101,
	}
}

func activateLease(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service, input lease.CreateDraftInput, submitKey string) *lease.Contract {
	t.Helper()
	draft, err := leaseService.CreateDraft(ctx, input)
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey, Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey + "-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: submitKey + "-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease step 2: %v", err)
	}
	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get active lease: %v", err)
	}
	return active
}

func int64Pointer(value int64) *int64 { return &value }
