//go:build integration

package invoice_test

import (
	"context"
	"database/sql"
	"fmt"
	"os"
	"reflect"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

type invoiceRepoTestEnv struct {
	db              *sql.DB
	repository      *invoice.Repository
	workflowService *workflow.Service
	leaseService    *lease.Service
	billingRepo     *billing.Repository
	billingService  *billing.Service
	invoiceService  *invoice.Service
}

func TestInvoiceRepositoryDocumentLifecycleIntegration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	env := newInvoiceRepoTestEnv(t, ctx)

	draft := createDraftDocumentViaRepository(t, ctx, env, "CON-REPO-001", 12000, invoice.DocumentTypeInvoice)
	loaded, err := env.repository.FindByID(ctx, draft.ID)
	if err != nil {
		t.Fatalf("find draft document by id: %v", err)
	}
	if loaded == nil || loaded.Status != invoice.StatusDraft || len(loaded.Lines) != 1 || loaded.Lines[0].Amount != 12000 {
		t.Fatalf("unexpected loaded draft document %#v", loaded)
	}

	tx := beginTx(t, ctx, env.db)
	locked, err := env.repository.FindByIDForUpdate(ctx, tx, draft.ID)
	rollbackTx(t, tx)
	if err != nil {
		t.Fatalf("find draft document for update: %v", err)
	}
	if locked == nil || locked.ID != draft.ID || len(locked.Lines) != 1 {
		t.Fatalf("unexpected locked draft document %#v", locked)
	}

	billDraft := createDraftDocumentViaRepository(t, ctx, env, "CON-REPO-002", 8000, invoice.DocumentTypeBill)
	statusDraft := invoice.StatusDraft
	listed, err := env.repository.List(ctx, invoice.ListFilter{Status: &statusDraft, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list draft documents: %v", err)
	}
	if listed.Total != 2 {
		t.Fatalf("expected two draft documents, got %#v", listed)
	}
	docTypeInvoice := invoice.DocumentTypeInvoice
	invoiceOnly, err := env.repository.List(ctx, invoice.ListFilter{DocumentType: &docTypeInvoice, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list invoice documents: %v", err)
	}
	if invoiceOnly.Total != 1 || invoiceOnly.Items[0].ID != draft.ID {
		t.Fatalf("expected only invoice draft in document type filter, got %#v", invoiceOnly)
	}
	leaseOnly, err := env.repository.List(ctx, invoice.ListFilter{LeaseContractID: &billDraft.LeaseContractID, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list documents by lease: %v", err)
	}
	if leaseOnly.Total != 1 || leaseOnly.Items[0].ID != billDraft.ID {
		t.Fatalf("expected bill draft lease filter result, got %#v", leaseOnly)
	}

	started := startWorkflow(t, ctx, env.workflowService, string(invoice.DocumentTypeInvoice), draft.ID, "repo-doc-start")
	submittedAt := time.Date(2026, 4, 15, 9, 0, 0, 0, time.UTC)
	tx = beginTx(t, ctx, env.db)
	if err := env.repository.AttachWorkflowInstance(ctx, tx, draft.ID, started.ID, 101, submittedAt); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("attach workflow instance: %v", err)
	}
	commitTx(t, tx)
	attached, err := env.repository.FindByID(ctx, draft.ID)
	if err != nil {
		t.Fatalf("reload attached document: %v", err)
	}
	if attached.WorkflowInstanceID == nil || *attached.WorkflowInstanceID != started.ID || attached.Status != invoice.StatusPendingApproval || attached.SubmittedAt == nil || !attached.SubmittedAt.Equal(submittedAt) {
		t.Fatalf("unexpected attached document %#v", attached)
	}

	numberTx := beginTx(t, ctx, env.db)
	number, err := env.repository.AllocateNumber(ctx, numberTx, string(invoice.DocumentTypeInvoice))
	if err != nil {
		rollbackTx(t, numberTx)
		t.Fatalf("allocate invoice number: %v", err)
	}
	approvedAt := time.Date(2026, 4, 16, 10, 0, 0, 0, time.UTC)
	if err := env.repository.UpdateWorkflowState(ctx, numberTx, draft.ID, started.ID, 101, invoice.StatusApproved, &number, &approvedAt); err != nil {
		rollbackTx(t, numberTx)
		t.Fatalf("update workflow state: %v", err)
	}
	commitTx(t, numberTx)
	approved, err := env.repository.FindByID(ctx, draft.ID)
	if err != nil {
		t.Fatalf("reload approved document: %v", err)
	}
	if approved.Status != invoice.StatusApproved || approved.DocumentNo == nil || *approved.DocumentNo != "INV-101" || approved.ApprovedAt == nil || !approved.ApprovedAt.Equal(approvedAt) {
		t.Fatalf("unexpected approved document %#v", approved)
	}

	markTx := beginTx(t, ctx, env.db)
	if err := env.repository.MarkAdjusted(ctx, markTx, draft.ID, 101); err != nil {
		rollbackTx(t, markTx)
		t.Fatalf("mark document adjusted: %v", err)
	}
	commitTx(t, markTx)
	adjusted, err := env.repository.FindByID(ctx, draft.ID)
	if err != nil {
		t.Fatalf("reload adjusted document: %v", err)
	}
	if adjusted.Status != invoice.StatusAdjusted {
		t.Fatalf("expected adjusted document, got %#v", adjusted)
	}

	cancelDraft := createDraftDocumentViaRepository(t, ctx, env, "CON-REPO-003", 6000, invoice.DocumentTypeInvoice)
	cancelledAt := time.Date(2026, 4, 17, 11, 0, 0, 0, time.UTC)
	cancelTx := beginTx(t, ctx, env.db)
	if err := env.repository.Cancel(ctx, cancelTx, cancelDraft.ID, 101, cancelledAt); err != nil {
		rollbackTx(t, cancelTx)
		t.Fatalf("cancel document: %v", err)
	}
	commitTx(t, cancelTx)
	cancelled, err := env.repository.FindByID(ctx, cancelDraft.ID)
	if err != nil {
		t.Fatalf("reload cancelled document: %v", err)
	}
	if cancelled.Status != invoice.StatusCancelled || cancelled.CancelledAt == nil || !cancelled.CancelledAt.Equal(cancelledAt) {
		t.Fatalf("unexpected cancelled document %#v", cancelled)
	}

	reminderDoc := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-REPO-004", 9000, "submit-repo-004", "invoice-repo-004")
	candidates, err := env.repository.ListPaymentReminderCandidates(ctx, time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), 30)
	if err != nil {
		t.Fatalf("list payment reminder candidates: %v", err)
	}
	if len(candidates) == 0 {
		t.Fatal("expected at least one payment reminder candidate")
	}
	found := false
	for _, candidate := range candidates {
		if candidate.DocumentID == reminderDoc.ID {
			found = true
			if candidate.InvoiceNumber != "INV-102" || candidate.CustomerName != reminderDoc.TenantName || candidate.OutstandingAmount != 9000 || candidate.ContactCandidate != "CUST-101" || candidate.DueDate.Format(invoice.DateLayout) != "2026-04-30" {
				t.Fatalf("unexpected payment reminder candidate %#v", candidate)
			}
		}
	}
	if !found {
		t.Fatalf("expected reminder candidate for document %d, got %#v", reminderDoc.ID, candidates)
	}
}

func TestInvoiceARRepositoryIntegration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	env := newInvoiceRepoTestEnv(t, ctx)

	document := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-AR-001", 12000, "submit-ar-001", "invoice-ar-001")
	leaseCtx, err := env.repository.FindReceivableLeaseContext(ctx, document.LeaseContractID)
	if err != nil {
		t.Fatalf("find receivable lease context: %v", err)
	}
	if leaseCtx == nil || reflectedInt64Field(t, leaseCtx, "LeaseContractID") != document.LeaseContractID || reflectedInt64Field(t, leaseCtx, "CustomerID") != 101 || reflectedInt64Field(t, leaseCtx, "DepartmentID") != 101 || reflectedPointerInt64Field(t, leaseCtx, "TradeID") != 102 {
		t.Fatalf("unexpected receivable lease context %#v", leaseCtx)
	}

	tx := beginTx(t, ctx, env.db)
	items, err := env.repository.FindOpenItemsByDocumentIDForUpdate(ctx, tx, document.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find open items for update: %v", err)
	}
	if len(items) != 1 || items[0].OutstandingAmount != 12000 {
		rollbackTx(t, tx)
		t.Fatalf("unexpected initial open items %#v", items)
	}
	count, err := env.repository.CountPaymentEntries(ctx, tx, document.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("count payment entries before insert: %v", err)
	}
	if count != 0 {
		rollbackTx(t, tx)
		t.Fatalf("expected zero payment entries, got %d", count)
	}
	note := "partial payment"
	if err := env.repository.InsertPaymentEntry(ctx, tx, invoice.PaymentEntry{BillingDocumentID: document.ID, LeaseContractID: document.LeaseContractID, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), Amount: 3000, Note: &note, RecordedBy: 101, IdempotencyKey: "ar-payment-001"}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert payment entry: %v", err)
	}
	payment, err := env.repository.FindPaymentEntryByIdempotency(ctx, tx, document.ID, "ar-payment-001")
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find payment entry by idempotency: %v", err)
	}
	if payment == nil || payment.Amount != 3000 || payment.Note == nil || *payment.Note != note {
		rollbackTx(t, tx)
		t.Fatalf("unexpected payment entry %#v", payment)
	}
	count, err = env.repository.CountPaymentEntries(ctx, tx, document.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("count payment entries after insert: %v", err)
	}
	if count != 1 {
		rollbackTx(t, tx)
		t.Fatalf("expected one payment entry, got %d", count)
	}
	if err := env.repository.UpdateOpenItemBalance(ctx, tx, items[0].ID, 9000, nil); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("update open item balance: %v", err)
	}
	commitTx(t, tx)

	summary, err := env.repository.GetReceivableSummary(ctx, document.ID)
	if err != nil {
		t.Fatalf("get receivable summary: %v", err)
	}
	if summary == nil || summary.OutstandingAmount != 9000 || summary.SettlementStatus != invoice.SettlementStatusOutstanding || len(summary.Items) != 1 || len(summary.PaymentHistory) != 1 {
		t.Fatalf("unexpected receivable summary %#v", summary)
	}

	manual := createDraftDocumentViaRepository(t, ctx, env, "CON-AR-002", 7000, invoice.DocumentTypeInvoice)
	leaseCtx, err = env.repository.FindReceivableLeaseContext(ctx, manual.LeaseContractID)
	if err != nil {
		t.Fatalf("find manual receivable lease context: %v", err)
	}
	tx = beginTx(t, ctx, env.db)
	if err := invokeUpsertReceivableOpenItem(t, env.repository, ctx, tx, leaseCtx, manual.ID, map[string]any{
		"BillingDocumentLineID": manual.Lines[0].ID,
		"ChargeType":            manual.Lines[0].ChargeType,
		"ChargeSource":          manual.Lines[0].ChargeSource,
		"OvertimeBillID":        manual.Lines[0].OvertimeBillID,
		"OvertimeFormulaID":     manual.Lines[0].OvertimeFormulaID,
		"OvertimeChargeID":      manual.Lines[0].OvertimeChargeID,
		"DueDate":               manual.Lines[0].PeriodEnd.UTC(),
		"OutstandingAmount":     7000.0,
		"IsDeposit":             false,
	}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("upsert receivable open item: %v", err)
	}
	manualItems, err := env.repository.FindOpenItemsByDocumentIDForUpdate(ctx, tx, manual.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find manual open items for update: %v", err)
	}
	if len(manualItems) != 1 || manualItems[0].OutstandingAmount != 7000 {
		rollbackTx(t, tx)
		t.Fatalf("unexpected manual open items %#v", manualItems)
	}
	settledAt := time.Date(2026, 4, 25, 0, 0, 0, 0, time.UTC)
	if err := env.repository.ZeroOpenItemsForDocument(ctx, tx, manual.ID, settledAt); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("zero open items for document: %v", err)
	}
	manualItems, err = env.repository.FindOpenItemsByDocumentIDForUpdate(ctx, tx, manual.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("reload manual open items for update: %v", err)
	}
	if len(manualItems) != 1 || manualItems[0].OutstandingAmount != 0 || manualItems[0].SettledAt == nil || !manualItems[0].SettledAt.Equal(settledAt) {
		rollbackTx(t, tx)
		t.Fatalf("unexpected zeroed open items %#v", manualItems)
	}
	commitTx(t, tx)

	other := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-AR-003", 5000, "submit-ar-003", "invoice-ar-003")
	listByLease, err := env.repository.ListReceivables(ctx, invoice.ReceivableFilter{LeaseContractID: &document.LeaseContractID, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list receivables by lease: %v", err)
	}
	if listByLease.Total != 1 || len(listByLease.Items) != 1 || listByLease.Items[0].BillingDocumentID != document.ID || listByLease.Items[0].OutstandingAmount != 9000 {
		t.Fatalf("unexpected lease receivable list %#v", listByLease)
	}
	customerID := int64(101)
	listByCustomer, err := env.repository.ListReceivables(ctx, invoice.ReceivableFilter{CustomerID: &customerID, Page: 1, PageSize: 20})
	if err != nil {
		t.Fatalf("list receivables by customer: %v", err)
	}
	if listByCustomer.Total != 2 {
		t.Fatalf("expected two outstanding receivables, got %#v", listByCustomer)
	}
	if other.ID == 0 {
		t.Fatalf("expected second approved invoice, got %#v", other)
	}
}

func TestInvoiceDiscountRepositoryIntegration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	env := newInvoiceRepoTestEnv(t, ctx)

	document := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-DIS-001", 12000, "submit-dis-001", "invoice-dis-001")
	receivable, err := env.invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get receivable for discount setup: %v", err)
	}

	discount := &invoice.Discount{
		BillingDocumentID:     document.ID,
		BillingDocumentLineID: document.Lines[0].ID,
		LeaseContractID:       document.LeaseContractID,
		ChargeType:            document.Lines[0].ChargeType,
		RequestedAmount:       1500,
		RequestedRate:         0.125,
		Reason:                "support rebate",
		Status:                invoice.DiscountStatusDraft,
		IdempotencyKey:        "discount-repo-001",
		CreatedBy:             101,
		UpdatedBy:             101,
	}
	tx := beginTx(t, ctx, env.db)
	if err := env.repository.CreateDiscount(ctx, tx, discount); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("create discount: %v", err)
	}
	foundByKey, err := env.repository.FindDiscountByIdempotency(ctx, tx, document.ID, discount.IdempotencyKey)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find discount by idempotency: %v", err)
	}
	if foundByKey == nil || foundByKey.ID != discount.ID || foundByKey.Status != invoice.DiscountStatusDraft {
		rollbackTx(t, tx)
		t.Fatalf("unexpected discount by idempotency %#v", foundByKey)
	}
	locked, err := env.repository.FindDiscountByIDForUpdate(ctx, tx, discount.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find discount for update: %v", err)
	}
	if locked == nil || locked.ID != discount.ID {
		rollbackTx(t, tx)
		t.Fatalf("unexpected locked discount %#v", locked)
	}
	commitTx(t, tx)

	instance := startWorkflow(t, ctx, env.workflowService, string(invoice.DocumentTypeDiscount), discount.ID, "discount-repo-start")
	submittedAt := time.Date(2026, 4, 18, 10, 0, 0, 0, time.UTC)
	tx = beginTx(t, ctx, env.db)
	if err := env.repository.AttachDiscountWorkflowInstance(ctx, tx, discount.ID, instance.ID, 101, submittedAt); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("attach discount workflow: %v", err)
	}
	pendingCount, err := env.repository.CountPendingDiscountsByLine(ctx, tx, document.Lines[0].ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("count pending discounts by line: %v", err)
	}
	if pendingCount != 1 {
		rollbackTx(t, tx)
		t.Fatalf("expected one pending discount, got %d", pendingCount)
	}
	commitTx(t, tx)

	approvedAt := time.Date(2026, 4, 19, 11, 0, 0, 0, time.UTC)
	tx = beginTx(t, ctx, env.db)
	if err := env.repository.UpdateDiscountWorkflowState(ctx, tx, discount.ID, instance.ID, 101, invoice.DiscountStatusApproved, &approvedAt, nil); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("update discount workflow state: %v", err)
	}
	approved, err := env.repository.FindDiscountByIDForUpdate(ctx, tx, discount.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("reload approved discount: %v", err)
	}
	if approved == nil || approved.Status != invoice.DiscountStatusApproved || approved.WorkflowInstanceID == nil || *approved.WorkflowInstanceID != instance.ID || approved.ApprovedAt == nil || !approved.ApprovedAt.Equal(approvedAt) {
		rollbackTx(t, tx)
		t.Fatalf("unexpected approved discount %#v", approved)
	}
	if hasEntry, err := env.repository.HasDiscountEntry(ctx, tx, discount.ID); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("check discount entry before insert: %v", err)
	} else if hasEntry {
		rollbackTx(t, tx)
		t.Fatal("expected no discount entry before insert")
	}
	if err := env.repository.InsertDiscountEntry(ctx, tx, invoice.DiscountEntry{InvoiceDiscountID: discount.ID, BillingDocumentID: document.ID, BillingDocumentLineID: document.Lines[0].ID, AROpenItemID: receivable.Items[0].ID, LeaseContractID: document.LeaseContractID, Amount: 1500, RecordedBy: 101}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert discount entry: %v", err)
	}
	hasEntry, err := env.repository.HasDiscountEntry(ctx, tx, discount.ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("check discount entry after insert: %v", err)
	}
	if !hasEntry {
		rollbackTx(t, tx)
		t.Fatal("expected discount entry after insert")
	}
	commitTx(t, tx)
}

func TestInvoiceDepositRepositoryIntegration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	env := newInvoiceRepoTestEnv(t, ctx)

	depositDoc := createApprovedDepositInvoiceForLease(t, ctx, env.db, env.leaseService, env.workflowService, env.billingRepo, env.invoiceService, "CON-DEP-001", 5000, "submit-dep-001", "deposit-dep-001")
	targetDoc := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-DEP-002", 8000, "submit-dep-002", "invoice-dep-002")
	depositSummary, err := env.invoiceService.GetReceivable(ctx, depositDoc.ID)
	if err != nil {
		t.Fatalf("get deposit receivable: %v", err)
	}
	targetSummary, err := env.invoiceService.GetReceivable(ctx, targetDoc.ID)
	if err != nil {
		t.Fatalf("get target receivable: %v", err)
	}

	note := "apply deposit"
	tx := beginTx(t, ctx, env.db)
	if err := env.repository.InsertDepositApplication(ctx, tx, invoice.DepositApplication{
		SourceBillingDocumentID:     depositDoc.ID,
		SourceBillingDocumentLineID: depositDoc.Lines[0].ID,
		SourceAROpenItemID:          depositSummary.Items[0].ID,
		TargetBillingDocumentID:     targetDoc.ID,
		TargetBillingDocumentLineID: targetDoc.Lines[0].ID,
		TargetAROpenItemID:          targetSummary.Items[0].ID,
		LeaseContractID:             depositDoc.LeaseContractID,
		Amount:                      3000,
		Note:                        &note,
		IdempotencyKey:              "deposit-app-repo-001",
		AppliedBy:                   101,
	}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert deposit application: %v", err)
	}
	app, err := env.repository.FindDepositApplicationByIdempotency(ctx, tx, depositDoc.ID, "deposit-app-repo-001")
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find deposit application by idempotency: %v", err)
	}
	if app == nil || app.Amount != 3000 || app.Note == nil || *app.Note != note {
		rollbackTx(t, tx)
		t.Fatalf("unexpected deposit application %#v", app)
	}
	if err := env.repository.InsertDepositRefund(ctx, tx, invoice.DepositRefund{BillingDocumentID: depositDoc.ID, BillingDocumentLineID: depositDoc.Lines[0].ID, AROpenItemID: depositSummary.Items[0].ID, LeaseContractID: depositDoc.LeaseContractID, Amount: 1000, Reason: "partial refund", IdempotencyKey: "deposit-refund-repo-001", RefundedBy: 101}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert deposit refund: %v", err)
	}
	refund, err := env.repository.FindDepositRefundByIdempotency(ctx, tx, depositDoc.ID, "deposit-refund-repo-001")
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find deposit refund by idempotency: %v", err)
	}
	if refund == nil || refund.Amount != 1000 || refund.Reason != "partial refund" {
		rollbackTx(t, tx)
		t.Fatalf("unexpected deposit refund %#v", refund)
	}
	countOutstanding, err := env.repository.CountOutstandingReceivablesForLease(ctx, tx, targetDoc.LeaseContractID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("count outstanding receivables for lease: %v", err)
	}
	if countOutstanding != 1 {
		rollbackTx(t, tx)
		t.Fatalf("expected one outstanding non-deposit receivable, got %d", countOutstanding)
	}
	commitTx(t, tx)

	leaseID := findLeaseByNo(t, env.leaseService, "CON-DEP-002")
	charges, err := env.billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate pending workflow charges: %v", err)
	}
	pendingChargeLineID := findChargeLineIDByLease(charges.Lines, leaseID)
	if pendingChargeLineID == 0 {
		t.Fatalf("expected pending charge line for lease %d, got %#v", leaseID, charges)
	}
	pendingDoc, err := env.invoiceService.CreateFromCharges(ctx, invoice.CreateInput{DocumentType: invoice.DocumentTypeInvoice, BillingChargeLineIDs: []int64{pendingChargeLineID}, ActorUserID: 101})
	if err != nil {
		t.Fatalf("create pending invoice document: %v", err)
	}
	if _, err := env.invoiceService.SubmitForApproval(ctx, invoice.SubmitInput{DocumentID: pendingDoc.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "deposit-pending-submit", Comment: "submit pending"}); err != nil {
		t.Fatalf("submit pending invoice document: %v", err)
	}
	tx = beginTx(t, ctx, env.db)
	pendingWorkflows, err := env.repository.CountPendingFinancialWorkflowsForLease(ctx, tx, targetDoc.LeaseContractID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("count pending financial workflows: %v", err)
	}
	commitTx(t, tx)
	if pendingWorkflows != 1 {
		t.Fatalf("expected one pending financial workflow, got %d", pendingWorkflows)
	}
}

func TestInvoiceInterestRepositoryIntegration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	env := newInvoiceRepoTestEnv(t, ctx)

	document := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-INT-001", 12000, "submit-int-001", "invoice-int-001")
	receivable, err := env.invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get receivable for interest setup: %v", err)
	}
	if _, err := env.db.ExecContext(ctx, `
		INSERT INTO interest_rate_configs (charge_type_filter, daily_rate, grace_days, is_default, status, created_by, updated_by)
		VALUES (?, ?, ?, ?, ?, ?, ?), (?, ?, ?, ?, ?, ?, ?)
	`, billing.ChargeTypeRent, 0.001, 7, false, invoice.InterestConfigStatusActive, 101, 101, nil, 0.002, 5, true, invoice.InterestConfigStatusActive, 101, 101); err != nil {
		t.Fatalf("seed interest rate configs: %v", err)
	}
	specific, err := env.repository.FindInterestRateConfig(ctx, billing.ChargeTypeRent)
	if err != nil {
		t.Fatalf("find interest rate config by charge type: %v", err)
	}
	if specific == nil || specific.ChargeTypeFilter == nil || *specific.ChargeTypeFilter != billing.ChargeTypeRent || specific.DailyRate != 0.001 || specific.GraceDays != 7 || specific.IsDefault {
		t.Fatalf("unexpected specific interest rate config %#v", specific)
	}
	fallback, err := env.repository.FindInterestRateConfig(ctx, "unknown_charge")
	if err != nil {
		t.Fatalf("find default interest rate config: %v", err)
	}
	if fallback == nil || !fallback.IsDefault || fallback.ChargeTypeFilter != nil {
		t.Fatalf("unexpected default interest rate config %#v", fallback)
	}

	tx := beginTx(t, ctx, env.db)
	first := invoice.InterestEntry{SourceAROpenItemID: receivable.Items[0].ID, SourceBillingDocumentID: document.ID, SourceBillingLineID: document.Lines[0].ID, GeneratedDocumentID: document.ID, GeneratedLineID: document.Lines[0].ID, ChargeType: invoice.ChargeTypeLateInterest, PrincipalAmount: 12000, DailyRate: 0.001, GraceDays: 7, CoveredStartDate: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC), CoveredEndDate: time.Date(2026, 5, 3, 0, 0, 0, 0, time.UTC), InterestDays: 3, InterestAmount: 36, IdempotencyKey: "interest-repo-001", CreatedBy: 101}
	if err := env.repository.InsertInterestEntry(ctx, tx, first); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert first interest entry: %v", err)
	}
	found, err := env.repository.FindInterestEntryBySourceAndIdempotency(ctx, tx, receivable.Items[0].ID, "interest-repo-001")
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find interest entry by source and idempotency: %v", err)
	}
	if found == nil || found.InterestAmount != 36 || found.InterestDays != 3 {
		rollbackTx(t, tx)
		t.Fatalf("unexpected found interest entry %#v", found)
	}
	second := invoice.InterestEntry{SourceAROpenItemID: receivable.Items[0].ID, SourceBillingDocumentID: document.ID, SourceBillingLineID: document.Lines[0].ID, GeneratedDocumentID: document.ID, GeneratedLineID: document.Lines[0].ID, ChargeType: invoice.ChargeTypeLateInterest, PrincipalAmount: 12000, DailyRate: 0.001, GraceDays: 7, CoveredStartDate: time.Date(2026, 5, 4, 0, 0, 0, 0, time.UTC), CoveredEndDate: time.Date(2026, 5, 5, 0, 0, 0, 0, time.UTC), InterestDays: 2, InterestAmount: 24, IdempotencyKey: "interest-repo-002", CreatedBy: 101}
	if err := env.repository.InsertInterestEntry(ctx, tx, second); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert second interest entry: %v", err)
	}
	latest, err := env.repository.FindLatestInterestEntryForSource(ctx, tx, receivable.Items[0].ID)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find latest interest entry for source: %v", err)
	}
	commitTx(t, tx)
	if latest == nil || latest.IdempotencyKey != "interest-repo-002" || latest.InterestAmount != 24 {
		t.Fatalf("unexpected latest interest entry %#v", latest)
	}
	history, err := env.repository.GetInterestHistoryByDocumentID(ctx, document.ID)
	if err != nil {
		t.Fatalf("get interest history by document id: %v", err)
	}
	if len(history) != 2 || history[0].IdempotencyKey != "interest-repo-002" || history[1].IdempotencyKey != "interest-repo-001" {
		t.Fatalf("unexpected interest history %#v", history)
	}
}

func TestInvoiceSurplusRepositoryIntegration(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	env := newInvoiceRepoTestEnv(t, ctx)

	document := createApprovedInvoiceForLease(t, ctx, env.leaseService, env.workflowService, env.billingService, env.invoiceService, "CON-SUR-001", 12000, "submit-sur-001", "invoice-sur-001")
	receivable, err := env.invoiceService.GetReceivable(ctx, document.ID)
	if err != nil {
		t.Fatalf("get receivable for surplus setup: %v", err)
	}

	firstAppliedAt := time.Date(2026, 4, 20, 12, 0, 0, 0, time.UTC)
	secondAppliedAt := time.Date(2026, 4, 21, 12, 0, 0, 0, time.UTC)
	tx := beginTx(t, ctx, env.db)
	balance, err := env.repository.UpsertSurplusBalance(ctx, tx, 101, 101, 1000, &firstAppliedAt)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert surplus balance: %v", err)
	}
	if balance.AvailableAmount != 1000 || balance.LastAppliedAt == nil || !balance.LastAppliedAt.Equal(firstAppliedAt) {
		rollbackTx(t, tx)
		t.Fatalf("unexpected created surplus balance %#v", balance)
	}
	balance, err = env.repository.UpsertSurplusBalance(ctx, tx, 101, 101, -250, &secondAppliedAt)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("update surplus balance: %v", err)
	}
	if balance.AvailableAmount != 750 || balance.LastAppliedAt == nil || !balance.LastAppliedAt.Equal(secondAppliedAt) {
		rollbackTx(t, tx)
		t.Fatalf("unexpected updated surplus balance %#v", balance)
	}
	locked, err := env.repository.FindSurplusBalanceByCustomerForUpdate(ctx, tx, 101)
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find surplus balance for update: %v", err)
	}
	if locked == nil || locked.AvailableAmount != 750 {
		rollbackTx(t, tx)
		t.Fatalf("unexpected locked surplus balance %#v", locked)
	}
	note := "apply surplus"
	if err := env.repository.InsertSurplusEntry(ctx, tx, invoice.SurplusEntry{SurplusBalanceID: balance.ID, EntryType: invoice.SurplusEntryTypeApplication, CustomerID: 101, BillingDocumentID: &document.ID, AROpenItemID: &receivable.Items[0].ID, Amount: 250, Note: &note, IdempotencyKey: "surplus-entry-001", RecordedBy: 101}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert first surplus entry: %v", err)
	}
	found, err := env.repository.FindSurplusEntryByTypeAndIdempotency(ctx, tx, document.ID, invoice.SurplusEntryTypeApplication, "surplus-entry-001")
	if err != nil {
		rollbackTx(t, tx)
		t.Fatalf("find surplus entry by type and idempotency: %v", err)
	}
	if found == nil || found.Amount != 250 || found.Note == nil || *found.Note != note {
		rollbackTx(t, tx)
		t.Fatalf("unexpected found surplus entry %#v", found)
	}
	if err := env.repository.InsertSurplusEntry(ctx, tx, invoice.SurplusEntry{SurplusBalanceID: balance.ID, EntryType: invoice.SurplusEntryTypeOverpayment, CustomerID: 101, BillingDocumentID: &document.ID, Amount: 500, IdempotencyKey: "surplus-entry-002", RecordedBy: 101}); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("insert second surplus entry: %v", err)
	}
	commitTx(t, tx)

	stored, err := env.repository.FindSurplusBalanceByCustomer(ctx, 101)
	if err != nil {
		t.Fatalf("find surplus balance by customer: %v", err)
	}
	if stored == nil || stored.AvailableAmount != 750 || stored.LastAppliedAt == nil || !stored.LastAppliedAt.Equal(secondAppliedAt) {
		t.Fatalf("unexpected stored surplus balance %#v", stored)
	}
	history, err := env.repository.GetSurplusHistoryByCustomer(ctx, 101)
	if err != nil {
		t.Fatalf("get surplus history by customer: %v", err)
	}
	if len(history) != 2 || history[0].IdempotencyKey != "surplus-entry-002" || history[1].IdempotencyKey != "surplus-entry-001" {
		t.Fatalf("unexpected surplus history %#v", history)
	}
}

func newInvoiceRepoTestEnv(t *testing.T, ctx context.Context) invoiceRepoTestEnv {
	t.Helper()
	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	workflowService := workflow.NewService(db, workflow.NewRepositoryWithNowFunc(db, invoiceWorkflowNow))
	leaseService := lease.NewService(db, lease.NewRepository(db), workflowService)
	billingRepo := billing.NewRepository(db)
	billingService := billing.NewService(db, billingRepo)
	invoiceRepo := invoice.NewRepository(db)
	invoiceService := invoice.NewService(db, invoiceRepo, billingRepo, workflowService)
	return invoiceRepoTestEnv{
		db:              db,
		repository:      invoiceRepo,
		workflowService: workflowService,
		leaseService:    leaseService,
		billingRepo:     billingRepo,
		billingService:  billingService,
		invoiceService:  invoiceService,
	}
}

func createDraftDocumentViaRepository(t *testing.T, ctx context.Context, env invoiceRepoTestEnv, leaseNo string, amount float64, documentType invoice.DocumentType) *invoice.Document {
	t.Helper()
	chargeLine := generateChargeLineForLease(t, ctx, env, leaseNo, amount)
	document := &invoice.Document{
		DocumentType:    documentType,
		BillingRunID:    chargeLine.BillingRunID,
		LeaseContractID: chargeLine.LeaseContractID,
		TenantName:      chargeLine.TenantName,
		PeriodStart:     chargeLine.PeriodStart,
		PeriodEnd:       chargeLine.PeriodEnd,
		TotalAmount:     chargeLine.Amount,
		CurrencyTypeID:  chargeLine.CurrencyTypeID,
		Status:          invoice.StatusDraft,
		CreatedBy:       101,
		UpdatedBy:       101,
		Lines: []invoice.Line{{
			BillingChargeLineID: chargeLine.ID,
			ChargeType:          chargeLine.ChargeType,
			ChargeSource:        chargeLine.ChargeSource,
			OvertimeBillID:      chargeLine.OvertimeBillID,
			OvertimeFormulaID:   chargeLine.OvertimeFormulaID,
			OvertimeChargeID:    chargeLine.OvertimeChargeID,
			PeriodStart:         chargeLine.PeriodStart,
			PeriodEnd:           chargeLine.PeriodEnd,
			QuantityDays:        chargeLine.QuantityDays,
			UnitAmount:          chargeLine.UnitAmount,
			Amount:              chargeLine.Amount,
		}},
	}
	tx := beginTx(t, ctx, env.db)
	if err := env.repository.Create(ctx, tx, document); err != nil {
		rollbackTx(t, tx)
		t.Fatalf("create repository-backed draft document: %v", err)
	}
	commitTx(t, tx)
	stored, err := env.repository.FindByID(ctx, document.ID)
	if err != nil {
		t.Fatalf("reload repository-backed draft document: %v", err)
	}
	if stored == nil {
		t.Fatal("expected stored draft document")
	}
	return stored
}

func generateChargeLineForLease(t *testing.T, ctx context.Context, env invoiceRepoTestEnv, leaseNo string, amount float64) billing.ChargeLine {
	t.Helper()
	activeLease := activateLease(t, ctx, env.leaseService, env.workflowService, newLeaseCreateInput(leaseNo, amount), leaseNo+"-submit")
	charges, err := env.billingService.GenerateCharges(ctx, billing.GenerateInput{PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC), ActorUserID: 101})
	if err != nil {
		t.Fatalf("generate charges for %s: %v", leaseNo, err)
	}
	for _, line := range charges.Lines {
		if line.LeaseContractID == activeLease.ID {
			return line
		}
	}
	t.Fatalf("charge line for lease %s not found in %#v", leaseNo, charges)
	return billing.ChargeLine{}
}

func startWorkflow(t *testing.T, ctx context.Context, workflowService *workflow.Service, documentType string, documentID int64, key string) *workflow.Instance {
	t.Helper()
	instance, err := workflowService.Start(ctx, workflow.StartInput{DefinitionCode: invoice.ApprovalDefinitionCode, DocumentType: documentType, DocumentID: documentID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: key, Comment: "start workflow"})
	if err != nil {
		t.Fatalf("start workflow for %s %d: %v", documentType, documentID, err)
	}
	return instance
}

func beginTx(t *testing.T, ctx context.Context, db *sql.DB) *sql.Tx {
	t.Helper()
	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin transaction: %v", err)
	}
	return tx
}

func commitTx(t *testing.T, tx *sql.Tx) {
	t.Helper()
	if err := tx.Commit(); err != nil {
		t.Fatalf("commit transaction: %v", err)
	}
}

func rollbackTx(t *testing.T, tx *sql.Tx) {
	t.Helper()
	if err := tx.Rollback(); err != nil && err != sql.ErrTxDone {
		t.Fatalf("rollback transaction: %v", err)
	}
}

func invokeUpsertReceivableOpenItem(t *testing.T, repo *invoice.Repository, ctx context.Context, tx *sql.Tx, leaseCtx any, documentID int64, fields map[string]any) error {
	t.Helper()
	method := reflect.ValueOf(repo).MethodByName("UpsertReceivableOpenItem")
	if !method.IsValid() {
		t.Fatal("UpsertReceivableOpenItem method not found")
	}
	rowType := method.Type().In(4)
	row := reflect.New(rowType).Elem()
	for name, value := range fields {
		field := row.FieldByName(name)
		if !field.IsValid() {
			t.Fatalf("receivable open item row field %s not found", name)
		}
		if value == nil {
			field.Set(reflect.Zero(field.Type()))
			continue
		}
		fieldValue := reflect.ValueOf(value)
		if !fieldValue.Type().AssignableTo(field.Type()) {
			fieldValue = fieldValue.Convert(field.Type())
		}
		field.Set(fieldValue)
	}
	results := method.Call([]reflect.Value{reflect.ValueOf(ctx), reflect.ValueOf(tx), reflect.ValueOf(leaseCtx), reflect.ValueOf(documentID), row})
	if len(results) != 1 || results[0].IsNil() {
		return nil
	}
	err, ok := results[0].Interface().(error)
	if !ok {
		return fmt.Errorf("unexpected non-error result from UpsertReceivableOpenItem: %T", results[0].Interface())
	}
	return err
}

func reflectedInt64Field(t *testing.T, value any, fieldName string) int64 {
	t.Helper()
	v := reflect.Indirect(reflect.ValueOf(value))
	field := v.FieldByName(fieldName)
	if !field.IsValid() {
		t.Fatalf("field %s not found on %#v", fieldName, value)
	}
	return field.Int()
}

func reflectedPointerInt64Field(t *testing.T, value any, fieldName string) int64 {
	t.Helper()
	v := reflect.Indirect(reflect.ValueOf(value))
	field := v.FieldByName(fieldName)
	if !field.IsValid() {
		t.Fatalf("field %s not found on %#v", fieldName, value)
	}
	if field.IsNil() {
		t.Fatalf("field %s unexpectedly nil on %#v", fieldName, value)
	}
	return field.Elem().Int()
}
