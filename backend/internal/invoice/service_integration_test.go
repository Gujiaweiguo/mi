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
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	bootstrap "github.com/Gujiaweiguo/mi/backend/internal/platform/database/bootstrap"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
	"github.com/testcontainers/testcontainers-go"
	"github.com/testcontainers/testcontainers-go/wait"
)

var invoiceWorkflowNow = func() time.Time { return time.Date(2026, 4, 1, 9, 0, 0, 0, time.UTC) }

func TestInvoiceServiceCreateApproveAndAudit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newInvoiceTestDB(t, ctx)
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

func TestInvoiceServiceBillNumberingAndCancel(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()
	db := newInvoiceTestDB(t, ctx)
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
	db := newInvoiceTestDB(t, ctx)
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
	db := newInvoiceTestDB(t, ctx)
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
	db := newInvoiceTestDB(t, ctx)
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

	if _, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: document.ID, ActorUserID: 101, Amount: 13000, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-501-over", Note: "too much"}); !errors.Is(err, invoice.ErrPaymentOverApplication) {
		t.Fatalf("expected over-application rejection, got %v", err)
	}
	receivable, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: document.ID, ActorUserID: 101, Amount: 7000, PaymentDate: time.Date(2026, 4, 20, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-501-a", Note: "partial payment"})
	if err != nil {
		t.Fatalf("record partial payment: %v", err)
	}
	if receivable.OutstandingAmount != 5000 || len(receivable.PaymentHistory) != 1 {
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
	settled, err := invoiceService.RecordPayment(ctx, invoice.RecordPaymentInput{DocumentID: document.ID, ActorUserID: 101, Amount: 5000, PaymentDate: time.Date(2026, 4, 25, 0, 0, 0, 0, time.UTC), IdempotencyKey: "payment-501-b", Note: "final payment"})
	if err != nil {
		t.Fatalf("record final payment: %v", err)
	}
	if settled.OutstandingAmount != 0 || settled.SettlementStatus != invoice.SettlementStatusSettled || len(settled.PaymentHistory) != 2 {
		t.Fatalf("expected settled receivable after cumulative payments, got %#v", settled)
	}
	if _, err := invoiceService.Cancel(ctx, invoice.CancelInput{DocumentID: document.ID, ActorUserID: 101}); !errors.Is(err, invoice.ErrDocumentHasRecordedPayments) {
		t.Fatalf("expected cancel with payments to be rejected, got %v", err)
	}
	if _, err := invoiceService.Adjust(ctx, invoice.AdjustInput{DocumentID: document.ID, ActorUserID: 101, Lines: []invoice.AdjustLineInput{{BillingChargeLineID: charges.Lines[0].ID, Amount: 11000}}}); !errors.Is(err, invoice.ErrDocumentHasRecordedPayments) {
		t.Fatalf("expected adjust with payments to be rejected, got %v", err)
	}
}

func newInvoiceTestDB(t *testing.T, ctx context.Context) *sql.DB {
	t.Helper()
	container, err := testcontainers.GenericContainer(ctx, testcontainers.GenericContainerRequest{ContainerRequest: testcontainers.ContainerRequest{Image: "mysql:8.0", ExposedPorts: []string{"3306/tcp"}, Env: map[string]string{"MYSQL_DATABASE": "mi_integration", "MYSQL_USER": "mi_user", "MYSQL_PASSWORD": "mi_password", "MYSQL_ROOT_PASSWORD": "mi_root_password"}, WaitingFor: wait.ForListeningPort("3306/tcp").WithStartupTimeout(3 * time.Minute)}, Started: true})
	if err != nil {
		t.Fatalf("start mysql container: %v", err)
	}
	t.Cleanup(func() { _ = container.Terminate(context.Background()) })
	host, err := container.Host(ctx)
	if err != nil {
		t.Fatalf("resolve mysql host: %v", err)
	}
	port, err := container.MappedPort(ctx, "3306/tcp")
	if err != nil {
		t.Fatalf("resolve mysql port: %v", err)
	}
	db, err := sql.Open("mysql", platformdb.Config{Host: host, Port: port.Int(), Name: "mi_integration", User: "mi_user", Password: "mi_password"}.DSN())
	if err != nil {
		t.Fatalf("open mysql connection: %v", err)
	}
	t.Cleanup(func() { _ = db.Close() })
	if err := waitForDatabase(ctx, db); err != nil {
		t.Fatalf("wait for mysql: %v", err)
	}
	migrator := platformdb.NewMigrator(db, os.DirFS("../platform/database"), "migrations")
	if err := migrator.ApplyUpMigrations(); err != nil {
		t.Fatalf("apply migrations: %v", err)
	}
	bootstrapRunner := platformdb.NewBootstrapRunner(db, bootstrap.All()...)
	if err := bootstrapRunner.Run(ctx); err != nil {
		t.Fatalf("run bootstrap seeds: %v", err)
	}
	return db
}

func waitForDatabase(ctx context.Context, db *sql.DB) error {
	deadline := time.Now().Add(30 * time.Second)
	var lastErr error
	for time.Now().Before(deadline) {
		pingCtx, cancel := context.WithTimeout(ctx, 5*time.Second)
		lastErr = db.PingContext(pingCtx)
		cancel()
		if lastErr == nil {
			return nil
		}
		time.Sleep(500 * time.Millisecond)
	}
	return lastErr
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
