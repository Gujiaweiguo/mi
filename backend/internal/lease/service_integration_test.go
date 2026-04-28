//go:build integration

package lease_test

import (
	"context"
	"errors"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

func TestLeaseServiceCreateSubmitAndActivate(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	draft, err := leaseService.CreateDraft(ctx, newLeaseCreateInput("CON-101", 101))
	if err != nil {
		t.Fatalf("create draft lease: %v", err)
	}
	if draft.Status != lease.StatusDraft {
		t.Fatalf("expected draft lease, got %#v", draft)
	}
	if draft.BillingEligible() {
		t.Fatal("draft lease should not be billing eligible")
	}

	storeID := int64(101)
	list, err := leaseService.ListLeases(ctx, lease.ListFilter{StoreID: &storeID})
	if err != nil {
		t.Fatalf("list leases: %v", err)
	}
	if list.Total != 1 || len(list.Items) != 1 {
		t.Fatalf("expected one listed lease, got total=%d len=%d", list.Total, len(list.Items))
	}

	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-con-101", Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}
	if submitted.Status != lease.StatusPendingApproval || submitted.WorkflowInstanceID == nil {
		t.Fatalf("expected pending approval lease with workflow instance, got %#v", submitted)
	}

	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-con-101-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve first workflow step: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease after first approve: %v", err)
	}

	pending, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get lease after first approve: %v", err)
	}
	if pending.Status != lease.StatusPendingApproval || pending.BillingEligible() {
		t.Fatalf("expected pending non-billing-effective lease after first approve, got %#v", pending)
	}

	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-con-101-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve second workflow step: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync lease after second approve: %v", err)
	}

	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get active lease: %v", err)
	}
	if active.Status != lease.StatusActive || !active.BillingEligible() || active.ApprovedAt == nil {
		t.Fatalf("expected active billing-effective lease, got %#v", active)
	}
}

func TestLeaseServiceRejectAndResubmit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	draft, err := leaseService.CreateDraft(ctx, newLeaseCreateInput("CON-102", 101))
	if err != nil {
		t.Fatalf("create draft lease: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-con-102", Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease: %v", err)
	}

	instance, err := workflowService.Reject(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reject-con-102", Comment: "reject lease"})
	if err != nil {
		t.Fatalf("reject lease workflow: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync rejected lease: %v", err)
	}

	rejected, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get rejected lease: %v", err)
	}
	if rejected.Status != lease.StatusRejected || rejected.BillingEligible() {
		t.Fatalf("expected rejected non-billing-effective lease, got %#v", rejected)
	}

	instance, err = workflowService.Resubmit(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "resubmit-con-102", Comment: "resubmit lease"})
	if err != nil {
		t.Fatalf("resubmit lease workflow: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync resubmitted lease: %v", err)
	}

	resubmitted, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get resubmitted lease: %v", err)
	}
	if resubmitted.Status != lease.StatusPendingApproval {
		t.Fatalf("expected pending approval lease after resubmit, got %#v", resubmitted)
	}
}

func TestLeaseServiceRejectsDuplicateSubmit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	draft, err := leaseService.CreateDraft(ctx, newLeaseCreateInput("CON-103", 101))
	if err != nil {
		t.Fatalf("create draft lease: %v", err)
	}
	if _, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-con-103-1", Comment: "submit lease"}); err != nil {
		t.Fatalf("submit lease first time: %v", err)
	}
	if _, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-con-103-2", Comment: "submit lease again"}); !errors.Is(err, lease.ErrLeaseAlreadySubmitted) {
		t.Fatalf("expected duplicate submit to fail with ErrLeaseAlreadySubmitted, got %v", err)
	}
}

func TestLeaseServicePersistsSubtypeDetails(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	jointDraft, err := leaseService.CreateDraft(ctx, newJointOperationLeaseCreateInput("CON-103A", 101))
	if err != nil {
		t.Fatalf("create joint operation draft: %v", err)
	}
	if jointDraft.Subtype != lease.ContractSubtypeJointOperation || jointDraft.JointOperation == nil {
		t.Fatalf("expected joint operation subtype detail, got %#v", jointDraft)
	}
	if jointDraft.JointOperation.BillCycle != 30 || jointDraft.JointOperation.SettlementCurrencyTypeID != 101 {
		t.Fatalf("expected joint operation settlement fields, got %#v", jointDraft.JointOperation)
	}

	adBoardDraft, err := leaseService.CreateDraft(ctx, newAdBoardLeaseCreateInput("CON-103B", 101))
	if err != nil {
		t.Fatalf("create ad board draft: %v", err)
	}
	if adBoardDraft.Subtype != lease.ContractSubtypeAdBoard || len(adBoardDraft.AdBoards) != 2 {
		t.Fatalf("expected repeated ad board detail rows, got %#v", adBoardDraft)
	}
	if adBoardDraft.AdBoards[0].AdBoardID != 901 || adBoardDraft.AdBoards[1].AdBoardID != 902 {
		t.Fatalf("expected persisted ad board IDs, got %#v", adBoardDraft.AdBoards)
	}

	areaGroundDraft, err := leaseService.CreateDraft(ctx, newAreaGroundLeaseCreateInput("CON-103C", 101))
	if err != nil {
		t.Fatalf("create area ground draft: %v", err)
	}
	if areaGroundDraft.Subtype != lease.ContractSubtypeAreaGround || len(areaGroundDraft.AreaGrounds) != 2 {
		t.Fatalf("expected repeated area/ground detail rows, got %#v", areaGroundDraft)
	}
	if areaGroundDraft.AreaGrounds[0].Code != "AREA-A" || areaGroundDraft.AreaGrounds[1].Code != "AREA-B" {
		t.Fatalf("expected persisted area/ground codes, got %#v", areaGroundDraft.AreaGrounds)
	}
}

func TestLeaseServiceSubmitRejectsMissingSubtypeFields(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	draft, err := leaseService.CreateDraft(ctx, newLeaseCreateInput("CON-103D", 101))
	if err != nil {
		t.Fatalf("create draft lease: %v", err)
	}
	if _, err := db.ExecContext(ctx, `UPDATE lease_contracts SET subtype = 'joint_operation' WHERE id = ?`, draft.ID); err != nil {
		t.Fatalf("promote draft subtype for validation test: %v", err)
	}

	_, err = leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-con-103d", Comment: "submit lease"})
	if err == nil {
		t.Fatal("expected subtype validation failure")
	}
	var validationErr *lease.ValidationError
	if !errors.As(err, &validationErr) {
		t.Fatalf("expected validation error, got %v", err)
	}
	if len(validationErr.Fields) == 0 || validationErr.Fields[0].Field != "joint_operation" {
		t.Fatalf("expected joint_operation field diagnostics, got %#v", validationErr.Fields)
	}
}

func TestLeaseServiceAmendmentUpdatesBillingEffectiveLease(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	activeBase := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-104", 101), "submit-con-104")

	amendedInput := newLeaseCreateInput("CON-104A", 101)
	amendedInput.Terms[0].Amount = 15000
	amendment, err := leaseService.CreateAmendment(ctx, lease.AmendInput{LeaseID: activeBase.ID, CreateDraftInput: amendedInput})
	if err != nil {
		t.Fatalf("create amendment lease: %v", err)
	}
	if amendment.AmendedFromID == nil || *amendment.AmendedFromID != activeBase.ID || amendment.EffectiveVersion != activeBase.EffectiveVersion+1 {
		t.Fatalf("expected amendment to point at base lease and increment version, got %#v", amendment)
	}

	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: amendment.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-con-104a", Comment: "submit amendment"})
	if err != nil {
		t.Fatalf("submit amendment lease: %v", err)
	}

	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-con-104a-step1", Comment: "manager approved amendment"})
	if err != nil {
		t.Fatalf("approve amendment step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync amendment lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-con-104a-step2", Comment: "finance approved amendment"})
	if err != nil {
		t.Fatalf("approve amendment step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync amendment lease step 2: %v", err)
	}

	updatedBase, err := leaseService.GetLease(ctx, activeBase.ID)
	if err != nil {
		t.Fatalf("get updated base lease: %v", err)
	}
	if updatedBase.Status != lease.StatusTerminated || updatedBase.BillingEligible() || updatedBase.TerminatedAt == nil {
		t.Fatalf("expected base lease terminated and not billing-effective after amendment approval, got %#v", updatedBase)
	}

	activeAmendment, err := leaseService.GetLease(ctx, amendment.ID)
	if err != nil {
		t.Fatalf("get active amendment lease: %v", err)
	}
	if activeAmendment.Status != lease.StatusActive || !activeAmendment.BillingEligible() {
		t.Fatalf("expected amendment lease active and billing-effective, got %#v", activeAmendment)
	}
	if len(activeAmendment.Terms) != 1 || activeAmendment.Terms[0].Amount != 15000 {
		t.Fatalf("expected amendment lease terms loaded, got %#v", activeAmendment.Terms)
	}
}

func TestLeaseServiceTerminateActiveLease(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-105", 101), "submit-con-105")

	terminated, err := leaseService.Terminate(ctx, lease.TerminateInput{LeaseID: activeLease.ID, ActorUserID: 101, TerminatedAt: time.Date(2026, 10, 1, 0, 0, 0, 0, time.UTC)})
	if err != nil {
		t.Fatalf("terminate lease: %v", err)
	}
	if terminated.Status != lease.StatusTerminated || terminated.BillingEligible() || terminated.TerminatedAt == nil {
		t.Fatalf("expected terminated non-billing-effective lease, got %#v", terminated)
	}
}

func TestLeaseServiceRejectsTerminateOnAlreadyTerminatedLease(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-105A", 101), "submit-con-105a")
	terminatedAt := time.Date(2026, 10, 1, 0, 0, 0, 0, time.UTC)
	if _, err := leaseService.Terminate(ctx, lease.TerminateInput{LeaseID: activeLease.ID, ActorUserID: 101, TerminatedAt: terminatedAt}); err != nil {
		t.Fatalf("terminate lease first time: %v", err)
	}

	if _, err := leaseService.Terminate(ctx, lease.TerminateInput{LeaseID: activeLease.ID, ActorUserID: 101, TerminatedAt: terminatedAt}); !errors.Is(err, lease.ErrInvalidLeaseState) {
		t.Fatalf("expected second terminate to fail with ErrInvalidLeaseState, got %v", err)
	}
}

func TestLeaseServiceRejectsAmendmentOnTerminatedLease(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-105B", 101), "submit-con-105b")
	if _, err := leaseService.Terminate(ctx, lease.TerminateInput{LeaseID: activeLease.ID, ActorUserID: 101, TerminatedAt: time.Date(2026, 10, 1, 0, 0, 0, 0, time.UTC)}); err != nil {
		t.Fatalf("terminate lease: %v", err)
	}

	amendedInput := newLeaseCreateInput("CON-105B-A", 101)
	amendedInput.Terms[0].Amount = 13000
	if _, err := leaseService.CreateAmendment(ctx, lease.AmendInput{LeaseID: activeLease.ID, CreateDraftInput: amendedInput}); !errors.Is(err, lease.ErrInvalidLeaseState) {
		t.Fatalf("expected amendment on terminated lease to fail with ErrInvalidLeaseState, got %v", err)
	}
}

func TestLeaseServiceRejectsTerminateWhenBillingDocumentInFlight(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepository(db))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-106", 101), "submit-con-106")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	if len(charges.Lines) != 1 {
		t.Fatalf("expected one charge line, got %#v", charges)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	if _, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-lease-106", Comment: "submit invoice"}); err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}

	if _, err := leaseService.Terminate(ctx, lease.TerminateInput{LeaseID: activeLease.ID, ActorUserID: 101, TerminatedAt: time.Date(2026, 10, 1, 0, 0, 0, 0, time.UTC)}); !errors.Is(err, lease.ErrLeaseHasBillingDocuments) {
		t.Fatalf("expected terminate to fail with ErrLeaseHasBillingDocuments, got %v", err)
	}

	current, err := leaseService.GetLease(ctx, activeLease.ID)
	if err != nil {
		t.Fatalf("get lease after blocked terminate: %v", err)
	}
	if current.Status != lease.StatusActive || !current.BillingEligible() {
		t.Fatalf("expected lease state preserved after blocked terminate, got %#v", current)
	}
}

func newLeaseCreateInput(leaseNo string, actorUserID int64) lease.CreateDraftInput {
	return lease.CreateDraftInput{
		LeaseNo:          leaseNo,
		Subtype:          lease.ContractSubtypeStandard,
		DepartmentID:     101,
		StoreID:          101,
		BuildingID:       int64Pointer(101),
		CustomerID:       int64Pointer(101),
		BrandID:          int64Pointer(101),
		TradeID:          int64Pointer(102),
		ManagementTypeID: int64Pointer(101),
		TenantName:       "ACME Retail",
		StartDate:        time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
		EndDate:          time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
		Units:            []lease.UnitInput{{UnitID: 101, RentArea: 118}},
		Terms: []lease.TermInput{{
			TermType:       lease.TermTypeRent,
			BillingCycle:   lease.BillingCycleMonthly,
			CurrencyTypeID: 101,
			Amount:         12000,
			EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
		}},
		ActorUserID: actorUserID,
	}
}

func newJointOperationLeaseCreateInput(leaseNo string, actorUserID int64) lease.CreateDraftInput {
	input := newLeaseCreateInput(leaseNo, actorUserID)
	input.Subtype = lease.ContractSubtypeJointOperation
	input.JointOperation = &lease.JointOperationFieldsInput{
		BillCycle:                30,
		RentInc:                  "5% yearly",
		AccountCycle:             30,
		TaxRate:                  0.09,
		TaxType:                  1,
		SettlementCurrencyTypeID: 101,
		InTaxRate:                0.03,
		OutTaxRate:               0.06,
		MonthSettleDays:          25,
		LatePayInterestRate:      0.01,
		InterestGraceDays:        5,
	}
	return input
}

func newAdBoardLeaseCreateInput(leaseNo string, actorUserID int64) lease.CreateDraftInput {
	input := newLeaseCreateInput(leaseNo, actorUserID)
	input.Subtype = lease.ContractSubtypeAdBoard
	input.AdBoards = []lease.AdBoardDetailInput{
		{
			AdBoardID:    901,
			Description:  "North atrium screen",
			Status:       1,
			StartDate:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:      time.Date(2026, 6, 30, 0, 0, 0, 0, time.UTC),
			RentArea:     16,
			Airtime:      20,
			Frequency:    lease.AdBoardFrequencyWeek,
			FrequencyMon: true,
			FrequencyWed: true,
			StoreID:      int64Pointer(101),
			BuildingID:   int64Pointer(101),
		},
		{
			AdBoardID:     902,
			Description:   "South atrium screen",
			Status:        1,
			StartDate:     time.Date(2026, 4, 15, 0, 0, 0, 0, time.UTC),
			EndDate:       time.Date(2026, 7, 31, 0, 0, 0, 0, time.UTC),
			RentArea:      18,
			Airtime:       10,
			Frequency:     lease.AdBoardFrequencyDay,
			FrequencyDays: 3,
			BetweenFrom:   1,
			BetweenTo:     5,
			StoreID:       int64Pointer(101),
			BuildingID:    int64Pointer(101),
		},
	}
	return input
}

func newAreaGroundLeaseCreateInput(leaseNo string, actorUserID int64) lease.CreateDraftInput {
	input := newLeaseCreateInput(leaseNo, actorUserID)
	input.Subtype = lease.ContractSubtypeAreaGround
	input.AreaGrounds = []lease.AreaGroundDetailInput{
		{
			Code:        "AREA-A",
			Name:        "Festival Plaza",
			TypeID:      1,
			Description: "Main event area",
			Status:      1,
			StartDate:   time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:     time.Date(2026, 5, 15, 0, 0, 0, 0, time.UTC),
			RentArea:    120,
		},
		{
			Code:        "AREA-B",
			Name:        "East Courtyard",
			TypeID:      2,
			Description: "Outdoor activation space",
			Status:      1,
			StartDate:   time.Date(2026, 6, 1, 0, 0, 0, 0, time.UTC),
			EndDate:     time.Date(2026, 8, 31, 0, 0, 0, 0, time.UTC),
			RentArea:    80,
		},
	}
	return input
}

func activateLease(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service, input lease.CreateDraftInput, submitKey string) *lease.Contract {
	t.Helper()

	draft, err := leaseService.CreateDraft(ctx, input)
	if err != nil {
		t.Fatalf("create lease draft: %v", err)
	}
	submitted, err := leaseService.SubmitForApproval(ctx, lease.SubmitInput{LeaseID: draft.ID, ActorUserID: input.ActorUserID, DepartmentID: input.DepartmentID, IdempotencyKey: submitKey, Comment: "submit lease"})
	if err != nil {
		t.Fatalf("submit lease draft: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: input.ActorUserID, DepartmentID: input.DepartmentID, IdempotencyKey: submitKey + "-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve lease step 1: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, input.ActorUserID); err != nil {
		t.Fatalf("sync lease step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: input.ActorUserID, DepartmentID: input.DepartmentID, IdempotencyKey: submitKey + "-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve lease step 2: %v", err)
	}
	if err := leaseService.SyncWorkflowState(ctx, instance, input.ActorUserID); err != nil {
		t.Fatalf("sync lease step 2: %v", err)
	}
	active, err := leaseService.GetLease(ctx, draft.ID)
	if err != nil {
		t.Fatalf("get active lease: %v", err)
	}
	return active
}

func int64Pointer(value int64) *int64 {
	return &value
}
