//go:build integration

package invoice_test

import (
	"context"
	"database/sql"
	"errors"
	"os"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/overtime"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

var invoiceWorkflowNow = func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }

func TestInvoiceServiceCreateApproveAndAudit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I101", 12000), "submit-i101")
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
	if document.Status != invoice.StatusDraft || document.TotalAmount != 12000 || document.LeaseContractID != activeLease.ID {
		t.Fatalf("unexpected created document %#v", document)
	}

	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-101", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	if submitted.WorkflowInstanceID == nil || submitted.Status != invoice.StatusPendingApproval {
		t.Fatalf("expected pending invoice with workflow instance, got %#v", submitted)
	}

	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-101", Comment: "finance approved invoice"})
	if err != nil {
		t.Fatalf("approve invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync invoice workflow state: %v", err)
	}

	approved, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("get approved invoice: %v", err)
	}
	if approved.Status != invoice.StatusApproved || approved.DocumentNo == nil || *approved.DocumentNo != "INV-101" || approved.ApprovedAt == nil {
		t.Fatalf("expected approved invoice with INV-101, got %#v", approved)
	}
	receivable, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get invoice receivable: %v", err)
	}
	if receivable.OutstandingAmount != 12000 || receivable.SettlementStatus != invoice.SettlementStatusOutstanding || len(receivable.Items) != 1 {
		t.Fatalf("expected receivable to be booked from approved invoice, got %#v", receivable)
	}
	if receivable.Items[0].DueDate.Format(invoice.DateLayout) != "2026-04-30" {
		t.Fatalf("expected due date to follow document line period end, got %#v", receivable.Items[0])
	}
	history, err := workflowService.AuditHistory(ctx, *submitted.WorkflowInstanceID)
	if err != nil {
		t.Fatalf("load invoice workflow audit: %v", err)
	}
	if len(history) < 2 {
		t.Fatalf("expected workflow audit entries for invoice, got %#v", history)
	}
}

func TestInvoiceServiceCreatesReceivableFromOvertimeBackedChargeLine(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	overtimeService := overtime.NewService(db, overtime.NewRepository(db), billingRepo, workflowService)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I-OT-101", 12000), "submit-i-ot-101")
	bill, err := overtimeService.CreateBill(ctx, overtime.CreateBillInput{LeaseContractID: activeLease.ID, PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101, Formulas: []overtime.FormulaInput{{ChargeType: "overtime_rent", FormulaType: overtime.FormulaTypeFixed, RateType: overtime.RateTypeDaily, EffectiveFrom: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), EffectiveTo: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), CurrencyTypeID: 101, TotalArea: 10, UnitPrice: 2}}})
	if err != nil {
		t.Fatalf("create overtime bill: %v", err)
	}
	submitted, err := overtimeService.SubmitForApproval(ctx, overtime.SubmitInput{BillID: bill.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-ot-invoice-101", Comment: "submit overtime"})
	if err != nil {
		t.Fatalf("submit overtime bill: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-ot-invoice-101-step1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve overtime step 1: %v", err)
	}
	if err := overtimeService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync overtime step 1: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-ot-invoice-101-step2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve overtime step 2: %v", err)
	}
	if err := overtimeService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync overtime step 2: %v", err)
	}
	if _, err := overtimeService.GenerateCharges(ctx, overtime.GenerateInput{BillID: bill.ID, ActorUserID: 101}); err != nil {
		t.Fatalf("generate overtime charges: %v", err)
	}
	chargeLines, err := billingRepo.ListChargeLines(ctx, billing.ChargeListFilter{LeaseContractID: &activeLease.ID, PeriodStart: &bill.PeriodStart, PeriodEnd: &bill.PeriodEnd, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list overtime downstream billing charge lines: %v", err)
	}
	if len(chargeLines.Items) != 1 {
		t.Fatalf("expected one overtime-backed charge line, got %#v", chargeLines)
	}
	chargeLine := chargeLines.Items[0]
	if chargeLine.ChargeSource != billing.ChargeSourceOvertime || chargeLine.OvertimeBillID == nil || *chargeLine.OvertimeBillID != bill.ID {
		t.Fatalf("expected overtime-backed billing charge line provenance, got %#v", chargeLine)
	}

	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{chargeLine.ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create overtime-backed invoice: %v", err)
	}
	if len(document.Lines) != 1 || document.Lines[0].ChargeSource != billing.ChargeSourceOvertime || document.Lines[0].OvertimeChargeID == nil || *document.Lines[0].OvertimeChargeID != *chargeLine.OvertimeChargeID {
		t.Fatalf("expected overtime provenance on invoice line, got %#v", document)
	}

	invoiceSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-ot-101", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit overtime-backed invoice: %v", err)
	}
	invoiceInstance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *invoiceSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-ot-101", Comment: "finance approved invoice"})
	if err != nil {
		t.Fatalf("approve overtime-backed invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, invoiceInstance, 101); err != nil {
		t.Fatalf("sync overtime-backed invoice workflow: %v", err)
	}
	receivable, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get overtime-backed receivable: %v", err)
	}
	if len(receivable.Items) != 1 || receivable.Items[0].ChargeSource != billing.ChargeSourceOvertime || receivable.Items[0].OvertimeBillID == nil || *receivable.Items[0].OvertimeBillID != bill.ID {
		t.Fatalf("expected overtime provenance on receivable item, got %#v", receivable)
	}
}

func TestInvoiceServiceBillNumberingAndCancel(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I201", 12000), "submit-i201")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeBill, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create bill document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-bill-201", Comment: "submit bill"})
	if err != nil {
		t.Fatalf("submit bill document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-bill-201", Comment: "finance approved bill"})
	if err != nil {
		t.Fatalf("approve bill workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync bill workflow state: %v", err)
	}
	approved, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("get approved bill: %v", err)
	}
	if approved.DocumentNo == nil || *approved.DocumentNo != "BIL-101" {
		t.Fatalf("expected BIL-101 bill number, got %#v", approved)
	}
	cancelled, err := invoiceService.Cancel(ctx, invoice.CancelInput{DocumentID: document.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("cancel bill document: %v", err)
	}
	if cancelled.Status != invoice.StatusCancelled || cancelled.CancelledAt == nil {
		t.Fatalf("expected cancelled bill document, got %#v", cancelled)
	}
	receivable, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get cancelled bill receivable: %v", err)
	}
	if receivable.OutstandingAmount != 0 || receivable.SettlementStatus != invoice.SettlementStatusSettled {
		t.Fatalf("expected cancelled bill receivable to be settled, got %#v", receivable)
	}
	replayedCancel, err := invoiceService.Cancel(ctx, invoice.CancelInput{DocumentID: document.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("replay cancel bill document: %v", err)
	}
	if replayedCancel.Status != invoice.StatusCancelled || replayedCancel.CancelledAt == nil {
		t.Fatalf("expected replayed cancel to preserve cancelled bill document, got %#v", replayedCancel)
	}
}

func TestInvoiceServiceAdjustmentCreatesNewDraft(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I301", 12000), "submit-i301")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-301", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-301", Comment: "finance approved invoice"})
	if err != nil {
		t.Fatalf("approve invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync invoice workflow state: %v", err)
	}

	adjusted, err := invoiceService.Adjust(ctx, invoice.AdjustInput{DocumentID: document.ID, ActorUserID: 101, Lines: []invoice.AdjustLineInput{{BillingChargeLineID: charges.Lines[0].ID, Amount: 11000}}})
	if err != nil {
		t.Fatalf("adjust invoice document: %v", err)
	}
	if adjusted.Status != invoice.StatusDraft || adjusted.AdjustedFromID == nil || len(adjusted.Lines) != 1 || adjusted.Lines[0].Amount != 11000 {
		t.Fatalf("expected adjusted draft invoice, got %#v", adjusted)
	}
	original, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("get original adjusted invoice: %v", err)
	}
	if original.Status != invoice.StatusAdjusted {
		t.Fatalf("expected original invoice adjusted, got %#v", original)
	}
	originalReceivable, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get original adjusted receivable: %v", err)
	}
	if originalReceivable.OutstandingAmount != 0 {
		t.Fatalf("expected original adjusted receivable to be cleared, got %#v", originalReceivable)
	}
	resubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: adjusted.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-301-adjusted", Comment: "submit adjusted invoice"})
	if err != nil {
		t.Fatalf("submit adjusted invoice: %v", err)
	}
	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *resubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-301-adjusted", Comment: "finance approved adjusted invoice"})
	if err != nil {
		t.Fatalf("approve adjusted invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync adjusted invoice workflow state: %v", err)
	}
	approvedAdjusted, err := invoiceService.GetDocument(ctx, adjusted.ID)
	if err != nil {
		t.Fatalf("get approved adjusted invoice: %v", err)
	}
	if approvedAdjusted.Status != invoice.StatusApproved || approvedAdjusted.DocumentNo == nil || *approvedAdjusted.DocumentNo != "INV-102" {
		t.Fatalf("expected approved adjusted invoice with new number, got %#v", approvedAdjusted)
	}
	adjustedReceivable, err := invoiceService.GetReceivable(ctx, adjusted.ID)
	if err != nil {
		t.Fatalf("get adjusted receivable: %v", err)
	}
	if adjustedReceivable.OutstandingAmount != 11000 {
		t.Fatalf("expected adjusted receivable amount 11000, got %#v", adjustedReceivable)
	}
}

func TestInvoiceServiceRejectResubmitAndPreventChargeReuse(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I401", 12000), "submit-i401")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	if _, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101}); !errors.Is(err, invoice.ErrChargeLineAlreadyDocumented) {
		t.Fatalf("expected charge reuse to fail, got %v", err)
	}

	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-401", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	instance, err := workflowService.Reject(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reject-invoice-401", Comment: "reject invoice"})
	if err != nil {
		t.Fatalf("reject invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync rejected invoice workflow state: %v", err)
	}
	rejected, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("get rejected invoice: %v", err)
	}
	if rejected.Status != invoice.StatusRejected || rejected.DocumentNo != nil {
		t.Fatalf("expected rejected invoice with no number, got %#v", rejected)
	}

	instance, err = workflowService.Resubmit(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "resubmit-invoice-401", Comment: "resubmit invoice"})
	if err != nil {
		t.Fatalf("resubmit invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync resubmitted invoice workflow state: %v", err)
	}
	pending, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("get resubmitted invoice: %v", err)
	}
	if pending.Status != invoice.StatusPendingApproval {
		t.Fatalf("expected pending approval invoice after resubmit, got %#v", pending)
	}

	instance, err = workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-401", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve resubmitted invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync approved resubmitted invoice workflow state: %v", err)
	}
	approved, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("get approved invoice: %v", err)
	}
	if approved.Status != invoice.StatusApproved || approved.DocumentNo == nil || *approved.DocumentNo != "INV-101" {
		t.Fatalf("expected approved invoice with first number after resubmit, got %#v", approved)
	}
	replayedSubmit, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-401-replay", Comment: "replay submit invoice"})
	if err != nil {
		t.Fatalf("expected replayed submit to preserve current state, got %v", err)
	}
	if replayedSubmit.Status != invoice.StatusApproved || replayedSubmit.WorkflowInstanceID == nil || replayedSubmit.DocumentNo == nil || *replayedSubmit.DocumentNo != "INV-101" {
		t.Fatalf("expected replayed submit to preserve approved invoice state, got %#v", replayedSubmit)
	}
}

func TestInvoiceServicePaymentApplicationAndReceivableGuards(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I501", 12000), "submit-i501")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-501", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-501", Comment: "finance approved invoice"})
	if err != nil {
		t.Fatalf("approve invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync invoice workflow state: %v", err)
	}

	receivable, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: document.ID, ActorUserID: 101, Amount: 7000, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-501-a", Note: "partial payment"})
	if err != nil {
		t.Fatalf("record partial payment: %v", err)
	}
	if receivable.OutstandingAmount != 5000 || len(receivable.PaymentHistory) != 1 || receivable.CustomerSurplus != 0 {
		t.Fatalf("expected outstanding amount 5000 after partial payment, got %#v", receivable)
	}
	replayed, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: document.ID, ActorUserID: 101, Amount: 7000, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-501-a", Note: "partial payment"})
	if err != nil {
		t.Fatalf("replay partial payment: %v", err)
	}
	if replayed.OutstandingAmount != 5000 || len(replayed.PaymentHistory) != 1 {
		t.Fatalf("expected replayed payment to be a no-op, got %#v", replayed)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("replay invoice workflow sync after payment: %v", err)
	}
	afterReplay, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get receivable after replayed workflow sync: %v", err)
	}
	if afterReplay.OutstandingAmount != 5000 || len(afterReplay.PaymentHistory) != 1 {
		t.Fatalf("expected replayed workflow sync to preserve payment-reduced balance, got %#v", afterReplay)
	}
	settled, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: document.ID, ActorUserID: 101, Amount: 6000, PaymentDate: time.Date(2026, 4, 25, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-501-b", Note: "overpayment"})
	if err != nil {
		t.Fatalf("record final payment: %v", err)
	}
	if settled.OutstandingAmount != 0 || settled.SettlementStatus != invoice.SettlementStatusSettled || len(settled.PaymentHistory) != 2 || settled.CustomerSurplus != 1000 || len(settled.SurplusHistory) != 1 {
		t.Fatalf("expected settled receivable plus 1000 surplus after cumulative payments, got %#v", settled)
	}
	if overpaymentEntries := countRows(t, ctx, db, `SELECT COUNT(*) FROM ar_surplus_entries WHERE entry_type = 'overpayment' AND idempotency_key = ?`, "payment-501-b"); overpaymentEntries != 1 {
		t.Fatalf("expected one persisted overpayment surplus entry, got %d", overpaymentEntries)
	}
	if _, err := invoiceService.Cancel(ctx, invoice.CancelInput{DocumentID: document.ID, ActorUserID: 101}); !errors.Is(err, invoice.ErrDocumentHasRecordedPayments) {
		t.Fatalf("expected cancel with payments to be rejected, got %v", err)
	}
	if _, err := invoiceService.Adjust(ctx, invoice.AdjustInput{DocumentID: document.ID, ActorUserID: 101, Lines: []invoice.AdjustLineInput{{BillingChargeLineID: charges.Lines[0].ID, Amount: 11000}}}); !errors.Is(err, invoice.ErrDocumentHasRecordedPayments) {
		t.Fatalf("expected adjust with payments to be rejected, got %v", err)
	}
}

func TestInvoiceServiceAppliesCustomerSurplusToOpenReceivable(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	firstLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I701", 12000), "submit-i701")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate first charges: %v", err)
	}
	firstChargeLineID := findChargeLineIDByLease(charges.Lines, firstLease.ID)
	if firstChargeLineID == 0 {
		t.Fatalf("expected charge line for first lease %d, got %#v", firstLease.ID, charges)
	}
	first, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{firstChargeLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create first invoice: %v", err)
	}
	firstSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: first.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-701-a", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit first invoice: %v", err)
	}
	firstInstance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *firstSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-701-a", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve first invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, firstInstance, 101); err != nil {
		t.Fatalf("sync first invoice: %v", err)
	}
	firstReceivable, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: first.ID, ActorUserID: 101, Amount: 13000, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-701-a", Note: "overpay first"})
	if err != nil {
		t.Fatalf("overpay first invoice: %v", err)
	}
	if firstReceivable.CustomerSurplus != 1000 {
		t.Fatalf("expected 1000 customer surplus after first overpayment, got %#v", firstReceivable)
	}

	secondLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I702", 12000), "submit-i702")
	charges, err = billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate second charges: %v", err)
	}
	secondChargeLineID := findChargeLineIDByLease(charges.Lines, secondLease.ID)
	if secondChargeLineID == 0 {
		t.Fatalf("expected charge line for second lease %d, got %#v", secondLease.ID, charges)
	}
	second, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{secondChargeLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create second invoice: %v", err)
	}
	secondSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: second.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-701-b", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit second invoice: %v", err)
	}
	secondInstance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *secondSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-701-b", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve second invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, secondInstance, 101); err != nil {
		t.Fatalf("sync second invoice: %v", err)
	}
	secondDoc, err := invoiceService.GetDocument(ctx, second.ID)
	if err != nil {
		t.Fatalf("load second invoice: %v", err)
	}
	updated, err := invoiceService.ApplySurplus(ctx, invoice.ApplySurplusInput{DocumentID: second.ID, BillingDocumentLineID: secondDoc.Lines[0].ID, Amount: 1000, Note: "apply prior overpayment", ActorUserID: 101, IdempotencyKey: "surplus-701-a"})
	if err != nil {
		t.Fatalf("apply surplus: %v", err)
	}
	if updated.OutstandingAmount != 11000 || updated.CustomerSurplus != 0 || len(updated.SurplusHistory) < 2 {
		t.Fatalf("expected surplus application to reduce receivable and consume surplus, got %#v", updated)
	}
	if applicationEntries := countRows(t, ctx, db, `SELECT COUNT(*) FROM ar_surplus_entries WHERE entry_type = 'application' AND idempotency_key = ?`, "surplus-701-a"); applicationEntries != 1 {
		t.Fatalf("expected one persisted surplus application entry, got %d", applicationEntries)
	}
	replayed, err := invoiceService.ApplySurplus(ctx, invoice.ApplySurplusInput{DocumentID: second.ID, BillingDocumentLineID: secondDoc.Lines[0].ID, Amount: 1000, Note: "apply prior overpayment", ActorUserID: 101, IdempotencyKey: "surplus-701-a"})
	if err != nil {
		t.Fatalf("replay surplus application: %v", err)
	}
	if replayed.OutstandingAmount != 11000 || replayed.CustomerSurplus != 0 {
		t.Fatalf("expected replayed surplus application to be a no-op, got %#v", replayed)
	}
	if _, err := invoiceService.ApplySurplus(ctx, invoice.ApplySurplusInput{DocumentID: second.ID, BillingDocumentLineID: secondDoc.Lines[0].ID, Amount: 1, Note: "excess", ActorUserID: 101, IdempotencyKey: "surplus-701-b"}); !errors.Is(err, invoice.ErrSurplusNotAvailable) {
		t.Fatalf("expected no remaining surplus guard, got %v", err)
	}
}

func TestInvoiceServiceSurplusIdempotencyIsDocumentScoped(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	firstDoc := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-I801", 12000, "submit-i801", "invoice-801")
	firstOverpay, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: firstDoc.ID, ActorUserID: 101, Amount: 13000, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), IdempotencyKey: "shared-overpay", Note: "first overpay"})
	if err != nil {
		t.Fatalf("overpay first invoice: %v", err)
	}
	if firstOverpay.CustomerSurplus != 1000 {
		t.Fatalf("expected first overpayment to create 1000 surplus, got %#v", firstOverpay)
	}

	secondDoc := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-I802", 12000, "submit-i802", "invoice-802")
	secondOverpay, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: secondDoc.ID, ActorUserID: 101, Amount: 13000, PaymentDate: time.Date(2026, 4, 21, 0, 0, 0, 0, time.UTC), IdempotencyKey: "shared-overpay", Note: "second overpay"})
	if err != nil {
		t.Fatalf("overpay second invoice with same key: %v", err)
	}
	if secondOverpay.CustomerSurplus != 2000 {
		t.Fatalf("expected second overpayment with same key on different doc to accumulate to 2000 surplus, got %#v", secondOverpay)
	}

	thirdDoc := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-I803", 12000, "submit-i803", "invoice-803")
	firstApplied, err := invoiceService.ApplySurplus(ctx, invoice.ApplySurplusInput{DocumentID: thirdDoc.ID, BillingDocumentLineID: thirdDoc.Lines[0].ID, Amount: 1000, Note: "apply first", ActorUserID: 101, IdempotencyKey: "shared-apply"})
	if err != nil {
		t.Fatalf("apply first surplus: %v", err)
	}
	if firstApplied.CustomerSurplus != 1000 || firstApplied.OutstandingAmount != 11000 {
		t.Fatalf("expected first surplus application to leave 1000 surplus and 11000 outstanding, got %#v", firstApplied)
	}

	fourthDoc := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-I804", 12000, "submit-i804", "invoice-804")
	secondApplied, err := invoiceService.ApplySurplus(ctx, invoice.ApplySurplusInput{DocumentID: fourthDoc.ID, BillingDocumentLineID: fourthDoc.Lines[0].ID, Amount: 1000, Note: "apply second", ActorUserID: 101, IdempotencyKey: "shared-apply"})
	if err != nil {
		t.Fatalf("apply second surplus with same key on different doc: %v", err)
	}
	if secondApplied.CustomerSurplus != 0 || secondApplied.OutstandingAmount != 11000 {
		t.Fatalf("expected second surplus application with same key on different doc to consume remaining surplus, got %#v", secondApplied)
	}
}

func TestInvoiceServiceDiscountApprovalReducesReceivableAndPreservesInvoiceAmounts(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput("CON-I601", 12000), "submit-i601")
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{charges.Lines[0].ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice document: %v", err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-invoice-601", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice document: %v", err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-invoice-601", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve invoice workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync approved invoice workflow state: %v", err)
	}
	receivable, err := invoiceService.ApplyDiscount(ctx, invoice.ApplyDiscountInput{DocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, Amount: 2000, Reason: "launch support", ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "discount-601"})
	if err != nil {
		t.Fatalf("apply discount request: %v", err)
	}
	if receivable.OutstandingAmount != 12000 || len(receivable.DiscountHistory) != 1 || receivable.DiscountHistory[0].Status != invoice.DiscountStatusPendingApproval {
		t.Fatalf("expected pending discount without balance mutation, got %#v", receivable)
	}
	discountWorkflowID := receivable.DiscountHistory[0].WorkflowInstanceID
	if discountWorkflowID == nil {
		t.Fatalf("expected pending discount workflow instance, got %#v", receivable.DiscountHistory[0])
	}
	discountInstance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *discountWorkflowID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-discount-601", Comment: "approve discount"})
	if err != nil {
		t.Fatalf("approve discount workflow: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, discountInstance, 101); err != nil {
		t.Fatalf("sync approved discount workflow state: %v", err)
	}
	updated, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("load receivable after discount approval: %v", err)
	}
	if updated.OutstandingAmount != 10000 || len(updated.DiscountHistory) != 1 || updated.DiscountHistory[0].Status != invoice.DiscountStatusApproved {
		t.Fatalf("expected approved discount to reduce receivable to 10000, got %#v", updated)
	}
	if discountEntries := countRows(t, ctx, db, `SELECT COUNT(*) FROM ar_discount_entries WHERE invoice_discount_id = ?`, updated.DiscountHistory[0].ID); discountEntries != 1 {
		t.Fatalf("expected one persisted discount audit entry, got %d", discountEntries)
	}
	approvedDocument, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("load invoice after discount approval: %v", err)
	}
	if approvedDocument.TotalAmount != 12000 || approvedDocument.Lines[0].Amount != 12000 {
		t.Fatalf("expected invoice amounts unchanged after discount, got %#v", approvedDocument)
	}
	if err := invoiceService.SyncWorkflowState(ctx, discountInstance, 101); err != nil {
		t.Fatalf("replay approved discount workflow state: %v", err)
	}
	replayed, err := invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("load receivable after replayed discount approval: %v", err)
	}
	if replayed.OutstandingAmount != 10000 {
		t.Fatalf("expected idempotent discount replay to preserve balance, got %#v", replayed)
	}
	if _, err := invoiceService.ApplyDiscount(ctx, invoice.ApplyDiscountInput{DocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, Amount: 10001, Reason: "too much", ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "discount-601-over"}); !errors.Is(err, invoice.ErrDiscountOverApplication) {
		t.Fatalf("expected over-application discount guard, got %v", err)
	}
}

func TestInvoiceServiceGeneratesLatePaymentInterestIdempotently(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	document := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-I901", 12000, "submit-i901", "invoice-901")
	if _, err := invoiceService.GenerateInterest(ctx, invoice.GenerateInterestInput{DocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, AsOfDate: time.Date(2026, 5, 5, 0, 0, 0, 0, time.UTC), ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "interest-901-too-early"}); !errors.Is(err, invoice.ErrInterestNotDue) {
		t.Fatalf("expected no interest before grace period ends, got %v", err)
	}
	updated, err := invoiceService.GenerateInterest(ctx, invoice.GenerateInterestInput{DocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, AsOfDate: time.Date(2026, 5, 10, 0, 0, 0, 0, time.UTC), ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "interest-901-a"})
	if err != nil {
		t.Fatalf("generate first interest invoice: %v", err)
	}
	if len(updated.InterestHistory) != 1 || updated.InterestHistory[0].InterestDays != 3 || updated.InterestHistory[0].InterestAmount != 36 || updated.OutstandingAmount != 12000 {
		t.Fatalf("expected one interest history row without mutating source receivable, got %#v", updated)
	}
	replayed, err := invoiceService.GenerateInterest(ctx, invoice.GenerateInterestInput{DocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, AsOfDate: time.Date(2026, 5, 10, 0, 0, 0, 0, time.UTC), ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "interest-901-a"})
	if err != nil {
		t.Fatalf("replay first interest generation: %v", err)
	}
	if len(replayed.InterestHistory) != 1 {
		t.Fatalf("expected replay to remain idempotent, got %#v", replayed)
	}
	next, err := invoiceService.GenerateInterest(ctx, invoice.GenerateInterestInput{DocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, AsOfDate: time.Date(2026, 5, 12, 0, 0, 0, 0, time.UTC), ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "interest-901-b"})
	if err != nil {
		t.Fatalf("generate second interest invoice: %v", err)
	}
	if len(next.InterestHistory) != 2 || next.InterestHistory[0].InterestDays != 2 || next.InterestHistory[0].InterestAmount != 24 {
		t.Fatalf("expected second interest generation to cover only the new period, got %#v", next)
	}
	interestDocument, err := invoiceService.GetDocument(ctx, next.InterestHistory[0].GeneratedDocumentID)
	if err != nil {
		t.Fatalf("load generated interest invoice: %v", err)
	}
	if interestDocument.DocumentType != invoice.DocumentTypeInvoice || interestDocument.DocumentNo == nil || interestDocument.Lines[0].ChargeType != invoice.ChargeTypeLateInterest {
		t.Fatalf("expected generated interest invoice document, got %#v", interestDocument)
	}
	interestReceivable, err := invoiceService.GetReceivable(ctx, interestDocument.ID)
	if err != nil {
		t.Fatalf("load generated interest receivable: %v", err)
	}
	if interestReceivable.OutstandingAmount != 24 {
		t.Fatalf("expected generated interest invoice receivable 24, got %#v", interestReceivable)
	}
	if interestEntries := countRows(t, ctx, db, `SELECT COUNT(*) FROM invoice_interest_entries WHERE source_billing_document_id = ?`, document.ID); interestEntries != 2 {
		t.Fatalf("expected two persisted interest audit entries, got %d", interestEntries)
	}
}

func newLeaseCreateInput(leaseNo string, amount float64) lease.CreateDraftInput {
	start := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	end := time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)
	buildingID := int64(101)
	customerID := int64(101)
	brandID := int64(101)
	tradeID := int64(102)
	managementTypeID := int64(101)
	return lease.CreateDraftInput{LeaseNo: leaseNo, DepartmentID: 101, StoreID: 101, BuildingID: &buildingID, CustomerID: &customerID, BrandID: &brandID, TradeID: &tradeID, ManagementTypeID: &managementTypeID, TenantName: "ACME Retail", StartDate: start, EndDate: end, Units: []lease.UnitInput{{UnitID: 101, RentArea: 118}}, Terms: []lease.TermInput{{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: amount, EffectiveFrom: start, EffectiveTo: end}}, ActorUserID: 101}
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

func createApprovedInvoiceForLease(t *testing.T, ctx context.Context, leaseService *lease.Service, workflowService *workflow.Service, billingService *billing.Service, invoiceService *invoice.Service, leaseNo string, amount float64, submitKey string, invoiceKey string) *invoice.Document {
	t.Helper()
	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput(leaseNo, amount), submitKey)
	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges for %s: %v", leaseNo, err)
	}
	chargeLineID := findChargeLineIDByLease(charges.Lines, activeLease.ID)
	if chargeLineID == 0 {
		t.Fatalf("expected charge line for lease %d, got %#v", activeLease.ID, charges)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{chargeLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create invoice for %s: %v", leaseNo, err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-submit", Comment: "submit invoice"})
	if err != nil {
		t.Fatalf("submit invoice for %s: %v", leaseNo, err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-approve", Comment: "approve invoice"})
	if err != nil {
		t.Fatalf("approve invoice for %s: %v", leaseNo, err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync invoice for %s: %v", leaseNo, err)
	}
	approved, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("load approved invoice for %s: %v", leaseNo, err)
	}
	return approved
}

func findChargeLineIDByLease(lines []billing.ChargeLine, leaseID int64) int64 {
	for _, line := range lines {
		if line.LeaseContractID == leaseID {
			return line.ID
		}
	}
	return 0
}

func createApprovedDepositInvoiceForLease(t *testing.T, ctx context.Context, db *sql.DB, leaseService *lease.Service, workflowService *workflow.Service, billingRepo *billing.Repository, invoiceService *invoice.Service, leaseNo string, depositAmount float64, submitKey string, invoiceKey string) *invoice.Document {
	t.Helper()
	activeLease := activateLease(t, ctx, leaseService, workflowService, newLeaseCreateInput(leaseNo, 1000), submitKey)
	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin deposit charge transaction: %v", err)
	}
	run := &billing.Run{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), Status: billing.RunStatusCompleted, TriggeredBy: 101, GeneratedCount: 1, SkippedCount: 0}
	if err := billingRepo.CreateRun(ctx, tx, run); err != nil {
		t.Fatalf("create deposit billing run: %v", err)
	}
	chargeLine := &billing.ChargeLine{BillingRunID: run.ID, LeaseContractID: activeLease.ID, LeaseNo: activeLease.LeaseNo, TenantName: activeLease.TenantName, LeaseTermID: &activeLease.Terms[0].ID, ChargeType: string(lease.TermTypeDeposit), ChargeSource: billing.ChargeSourceStandard, PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), QuantityDays: 30, UnitAmount: depositAmount, Amount: depositAmount, CurrencyTypeID: 101, SourceEffectiveVersion: activeLease.EffectiveVersion}
	if err := billingRepo.InsertChargeLine(ctx, tx, chargeLine); err != nil {
		t.Fatalf("insert deposit charge line: %v", err)
	}
	if err := tx.Commit(); err != nil {
		t.Fatalf("commit deposit charge transaction: %v", err)
	}
	document, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{chargeLine.ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create deposit invoice for %s: %v", leaseNo, err)
	}
	submitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: document.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-submit", Comment: "submit deposit invoice"})
	if err != nil {
		t.Fatalf("submit deposit invoice for %s: %v", leaseNo, err)
	}
	instance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *submitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: invoiceKey + "-approve", Comment: "approve deposit invoice"})
	if err != nil {
		t.Fatalf("approve deposit invoice for %s: %v", leaseNo, err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, instance, 101); err != nil {
		t.Fatalf("sync deposit invoice for %s: %v", leaseNo, err)
	}
	approved, err := invoiceService.GetDocument(ctx, document.ID)
	if err != nil {
		t.Fatalf("load approved deposit invoice for %s: %v", leaseNo, err)
	}
	return approved
}

func TestInvoiceServiceApplyDepositReducesBothReceivableAndDeposit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	depositDoc := createApprovedDepositInvoiceForLease(t, ctx, db, leaseService, workflowService, billingRepo, invoiceService, "CON-D001", 5000, "submit-d001", "deposit-d001")
	depositReceivable, err := invoiceService.GetReceivable(ctx, depositDoc.ID)
	if err != nil {
		t.Fatalf("get deposit receivable: %v", err)
	}
	if len(depositReceivable.Items) != 1 || !depositReceivable.Items[0].IsDeposit || depositReceivable.Items[0].OutstandingAmount != 5000 {
		t.Fatalf("expected deposit open item with 5000 outstanding, got %#v", depositReceivable.Items)
	}

	targetDoc := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-D002", 8000, "submit-d002", "invoice-d002")

	result, err := invoiceService.ApplyDeposit(ctx, invoice.ApplyDepositInput{
		DocumentID:                  depositDoc.ID,
		BillingDocumentLineID:       depositDoc.Lines[0].ID,
		TargetDocumentID:            targetDoc.ID,
		TargetBillingDocumentLineID: targetDoc.Lines[0].ID,
		Amount:                      3000,
		ActorUserID:                 101,
		IdempotencyKey:              "deposit-apply-d001",
	})
	if err != nil {
		t.Fatalf("apply deposit: %v", err)
	}
	if result.OutstandingAmount != 5000 {
		t.Fatalf("expected target outstanding 5000 after deposit application, got %v", result.OutstandingAmount)
	}
	if len(result.DepositApplicationHistory) != 0 {
		t.Fatalf("expected no deposit application history on target document, got %d entries", len(result.DepositApplicationHistory))
	}

	sourceReceivable, err := invoiceService.GetReceivable(ctx, depositDoc.ID)
	if err != nil {
		t.Fatalf("get deposit receivable after apply: %v", err)
	}
	if sourceReceivable.OutstandingAmount != 2000 {
		t.Fatalf("expected deposit outstanding 2000 after apply, got %v", sourceReceivable.OutstandingAmount)
	}
	if len(sourceReceivable.DepositApplicationHistory) != 1 || sourceReceivable.DepositApplicationHistory[0].Amount != 3000 {
		t.Fatalf("expected one deposit application history entry with amount 3000, got %#v", sourceReceivable.DepositApplicationHistory)
	}
	if depositApplications := countRows(t, ctx, db, `SELECT COUNT(*) FROM deposit_applications WHERE source_billing_document_id = ? AND idempotency_key = ?`, depositDoc.ID, "deposit-apply-d001"); depositApplications != 1 {
		t.Fatalf("expected one persisted deposit application entry, got %d", depositApplications)
	}

	replayed, err := invoiceService.ApplyDeposit(ctx, invoice.ApplyDepositInput{
		DocumentID:                  depositDoc.ID,
		BillingDocumentLineID:       depositDoc.Lines[0].ID,
		TargetDocumentID:            targetDoc.ID,
		TargetBillingDocumentLineID: targetDoc.Lines[0].ID,
		Amount:                      3000,
		ActorUserID:                 101,
		IdempotencyKey:              "deposit-apply-d001",
	})
	if err != nil {
		t.Fatalf("replay deposit application: %v", err)
	}
	if replayed.OutstandingAmount != 5000 {
		t.Fatalf("expected replayed deposit application to be no-op, got outstanding %v", replayed.OutstandingAmount)
	}
}

func TestInvoiceServiceApplyDepositGuardsInvalidTargetAndAmount(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	depositDoc := createApprovedDepositInvoiceForLease(t, ctx, db, leaseService, workflowService, billingRepo, invoiceService, "CON-D101", 5000, "submit-d101", "deposit-d101")
	targetDoc := createApprovedInvoiceForLease(t, ctx, leaseService, workflowService, billingService, invoiceService, "CON-D102", 3000, "submit-d102", "invoice-d102")

	if _, err := invoiceService.ApplyDeposit(ctx, invoice.ApplyDepositInput{
		DocumentID:                  depositDoc.ID,
		BillingDocumentLineID:       depositDoc.Lines[0].ID,
		TargetDocumentID:            targetDoc.ID,
		TargetBillingDocumentLineID: targetDoc.Lines[0].ID,
		Amount:                      6000,
		ActorUserID:                 101,
		IdempotencyKey:              "deposit-over",
	}); !errors.Is(err, invoice.ErrDepositAmountInvalid) {
		t.Fatalf("expected deposit over-application guard, got %v", err)
	}

	if _, err := invoiceService.ApplyDeposit(ctx, invoice.ApplyDepositInput{
		DocumentID:                  depositDoc.ID,
		BillingDocumentLineID:       depositDoc.Lines[0].ID,
		TargetDocumentID:            targetDoc.ID,
		TargetBillingDocumentLineID: targetDoc.Lines[0].ID,
		Amount:                      4000,
		ActorUserID:                 101,
		IdempotencyKey:              "deposit-target-over",
	}); !errors.Is(err, invoice.ErrDepositTargetNotAllowed) {
		t.Fatalf("expected deposit target over-application guard, got %v", err)
	}

	if _, err := invoiceService.ApplyDeposit(ctx, invoice.ApplyDepositInput{
		DocumentID:                  targetDoc.ID,
		BillingDocumentLineID:       targetDoc.Lines[0].ID,
		TargetDocumentID:            depositDoc.ID,
		TargetBillingDocumentLineID: depositDoc.Lines[0].ID,
		Amount:                      1000,
		ActorUserID:                 101,
		IdempotencyKey:              "deposit-non-deposit-source",
	}); !errors.Is(err, invoice.ErrDepositNotAvailable) {
		t.Fatalf("expected non-deposit source guard, got %v", err)
	}

	if _, err := invoiceService.ApplyDeposit(ctx, invoice.ApplyDepositInput{
		DocumentID:                  depositDoc.ID,
		BillingDocumentLineID:       depositDoc.Lines[0].ID,
		TargetDocumentID:            depositDoc.ID,
		TargetBillingDocumentLineID: depositDoc.Lines[0].ID,
		Amount:                      1000,
		ActorUserID:                 101,
		IdempotencyKey:              "deposit-target-is-deposit",
	}); !errors.Is(err, invoice.ErrDepositTargetNotAllowed) {
		t.Fatalf("expected deposit-as-target guard, got %v", err)
	}
}

func TestInvoiceServiceRefundDepositBlockedByOutstandingReceivables(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	start := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	end := time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC)
	buildingID := int64(101)
	customerID := int64(101)
	brandID := int64(101)
	tradeID := int64(102)
	managementTypeID := int64(101)
	leaseInput := lease.CreateDraftInput{
		LeaseNo: "CON-D201", DepartmentID: 101, StoreID: 101, BuildingID: &buildingID, CustomerID: &customerID, BrandID: &brandID, TradeID: &tradeID, ManagementTypeID: &managementTypeID, TenantName: "ACME Retail",
		StartDate: start, EndDate: end,
		Units: []lease.UnitInput{{UnitID: 101, RentArea: 118}},
		Terms: []lease.TermInput{
			{TermType: lease.TermTypeRent, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: 8000, EffectiveFrom: start, EffectiveTo: end},
			{TermType: lease.TermTypeDeposit, BillingCycle: lease.BillingCycleMonthly, CurrencyTypeID: 101, Amount: 5000, EffectiveFrom: start, EffectiveTo: end},
		},
		ActorUserID: 101,
	}
	activeLease := activateLease(t, ctx, leaseService, workflowService, leaseInput, "submit-d201")

	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges: %v", err)
	}
	rentChargeLineID := findChargeLineIDByLease(charges.Lines, activeLease.ID)
	if rentChargeLineID == 0 {
		t.Fatalf("expected rent charge line for lease %d, got %#v", activeLease.ID, charges)
	}
	rentDoc, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{rentChargeLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create rent invoice: %v", err)
	}
	rentSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: rentDoc.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-d201-rent", Comment: "submit rent"})
	if err != nil {
		t.Fatalf("submit rent invoice: %v", err)
	}
	rentInstance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *rentSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-d201-rent", Comment: "approve rent"})
	if err != nil {
		t.Fatalf("approve rent invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, rentInstance, 101); err != nil {
		t.Fatalf("sync rent invoice: %v", err)
	}

	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin deposit charge tx: %v", err)
	}
	run := &billing.Run{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), Status: billing.RunStatusCompleted, TriggeredBy: 101, GeneratedCount: 1, SkippedCount: 0}
	if err := billingRepo.CreateRun(ctx, tx, run); err != nil {
		t.Fatalf("create deposit run: %v", err)
	}
	depositChargeLine := &billing.ChargeLine{BillingRunID: run.ID, LeaseContractID: activeLease.ID, LeaseNo: activeLease.LeaseNo, TenantName: activeLease.TenantName, LeaseTermID: &activeLease.Terms[1].ID, ChargeType: string(lease.TermTypeDeposit), ChargeSource: billing.ChargeSourceStandard, PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), QuantityDays: 30, UnitAmount: 5000, Amount: 5000, CurrencyTypeID: 101, SourceEffectiveVersion: activeLease.EffectiveVersion}
	if err := billingRepo.InsertChargeLine(ctx, tx, depositChargeLine); err != nil {
		t.Fatalf("insert deposit charge line: %v", err)
	}
	if err := tx.Commit(); err != nil {
		t.Fatalf("commit deposit charge tx: %v", err)
	}
	depositDoc, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{depositChargeLine.ID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create deposit invoice: %v", err)
	}
	depositSubmitted, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: depositDoc.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-d201-deposit", Comment: "submit deposit"})
	if err != nil {
		t.Fatalf("submit deposit invoice: %v", err)
	}
	depositInstance, err := workflowService.Approve(ctx, workflow.TransitionInput{InstanceID: *depositSubmitted.WorkflowInstanceID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-d201-deposit", Comment: "approve deposit"})
	if err != nil {
		t.Fatalf("approve deposit invoice: %v", err)
	}
	if err := invoiceService.SyncWorkflowState(ctx, depositInstance, 101); err != nil {
		t.Fatalf("sync deposit invoice: %v", err)
	}
	approvedDeposit, err := invoiceService.GetDocument(ctx, depositDoc.ID)
	if err != nil {
		t.Fatalf("load approved deposit invoice: %v", err)
	}

	if _, err := invoiceService.RefundDeposit(ctx, invoice.RefundDepositInput{
		DocumentID:            approvedDeposit.ID,
		BillingDocumentLineID: approvedDeposit.Lines[0].ID,
		Amount:                5000,
		Reason:                "lease ended",
		ActorUserID:           101,
		IdempotencyKey:        "refund-d201-blocked",
	}); !errors.Is(err, invoice.ErrDepositRefundBlocked) {
		t.Fatalf("expected refund blocked by outstanding receivables, got %v", err)
	}
}

func TestInvoiceServiceRefundDepositBlockedByPendingFinancialWorkflow(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	depositDoc := createApprovedDepositInvoiceForLease(t, ctx, db, leaseService, workflowService, billingRepo, invoiceService, "CON-D301", 5000, "submit-d301", "deposit-d301")

	charges, err := billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate pending charges: %v", err)
	}
	if len(charges.Lines) == 0 {
		t.Fatal("expected charges for pending workflow test")
	}
	pendingChargeLineID := findChargeLineIDByLease(charges.Lines, findLeaseByNo(t, leaseService, "CON-D301"))
	if pendingChargeLineID == 0 {
		t.Fatalf("expected charge line for pending workflow lease, got %#v", charges)
	}
	pendingDoc, err := invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{pendingChargeLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create pending invoice: %v", err)
	}
	if _, err := invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: pendingDoc.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "submit-d301-pending", Comment: "submit pending"}); err != nil {
		t.Fatalf("submit pending invoice: %v", err)
	}

	if _, err := invoiceService.RefundDeposit(ctx, invoice.RefundDepositInput{
		DocumentID:            depositDoc.ID,
		BillingDocumentLineID: depositDoc.Lines[0].ID,
		Amount:                5000,
		Reason:                "lease ended",
		ActorUserID:           101,
		IdempotencyKey:        "refund-d301-pending",
	}); !errors.Is(err, invoice.ErrDepositRefundBlocked) {
		t.Fatalf("expected refund blocked by pending financial workflow, got %v", err)
	}
}

func TestInvoiceServiceRefundDepositSucceedsWhenSettled(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	invoiceService := invoice.NewService(db, invoice.NewRepository(db), billingRepo, workflowService)

	depositDoc := createApprovedDepositInvoiceForLease(t, ctx, db, leaseService, workflowService, billingRepo, invoiceService, "CON-D401", 5000, "submit-d401", "deposit-d401")

	result, err := invoiceService.RefundDeposit(ctx, invoice.RefundDepositInput{
		DocumentID:            depositDoc.ID,
		BillingDocumentLineID: depositDoc.Lines[0].ID,
		Amount:                5000,
		Reason:                "lease ended, full refund",
		ActorUserID:           101,
		IdempotencyKey:        "refund-d401",
	})
	if err != nil {
		t.Fatalf("refund deposit: %v", err)
	}
	if result.OutstandingAmount != 0 || result.SettlementStatus != invoice.SettlementStatusSettled {
		t.Fatalf("expected settled deposit after refund, got %#v", result)
	}
	if len(result.DepositRefundHistory) != 1 || result.DepositRefundHistory[0].Amount != 5000 {
		t.Fatalf("expected one refund history entry with amount 5000, got %#v", result.DepositRefundHistory)
	}
	if depositRefunds := countRows(t, ctx, db, `SELECT COUNT(*) FROM deposit_refunds WHERE billing_document_id = ? AND idempotency_key = ?`, depositDoc.ID, "refund-d401"); depositRefunds != 1 {
		t.Fatalf("expected one persisted deposit refund entry, got %d", depositRefunds)
	}

	replayed, err := invoiceService.RefundDeposit(ctx, invoice.RefundDepositInput{
		DocumentID:            depositDoc.ID,
		BillingDocumentLineID: depositDoc.Lines[0].ID,
		Amount:                5000,
		Reason:                "lease ended, full refund",
		ActorUserID:           101,
		IdempotencyKey:        "refund-d401",
	})
	if err != nil {
		t.Fatalf("replay deposit refund: %v", err)
	}
	if replayed.OutstandingAmount != 0 {
		t.Fatalf("expected replayed refund to be no-op, got outstanding %v", replayed.OutstandingAmount)
	}
	if len(replayed.DepositRefundHistory) != 1 {
		t.Fatalf("expected replayed refund to not add duplicate history, got %d entries", len(replayed.DepositRefundHistory))
	}
}

func findLeaseByNo(t *testing.T, leaseService *lease.Service, leaseNo string) int64 {
	t.Helper()
	ctx := context.Background()
	contracts, err := leaseService.ListLeases(ctx, lease.ListFilter{Page: 1, PageSize: 1000})
	if err != nil {
		t.Fatalf("list leases: %v", err)
	}
	for _, c := range contracts.Items {
		if c.LeaseNo == leaseNo {
			return c.ID
		}
	}
	t.Fatalf("lease %s not found", leaseNo)
	return 0
}

func countRows(t *testing.T, ctx context.Context, db *sql.DB, query string, args ...any) int {
	t.Helper()
	var count int
	if err := db.QueryRowContext(ctx, query, args...).Scan(&count); err != nil {
		t.Fatalf("count rows with query %q: %v", query, err)
	}
	return count
}
