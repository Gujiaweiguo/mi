package docoutput

import (
	"context"
	"strings"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/invoice"
)

func TestTemplateFromInputValid(t *testing.T) {
	input := UpsertTemplateInput{
		Code:         "invoice-a4",
		Name:         "Invoice A4",
		DocumentType: "invoice",
		OutputMode:   OutputModeInvoiceBatch,
		Title:        "Invoice Print",
		ActorUserID:  1,
		HeaderLines:  []string{"Header 1"},
		FooterLines:  []string{"Footer 1"},
	}
	tmpl, err := templateFromInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if tmpl.Code != "invoice-a4" {
		t.Fatalf("expected code invoice-a4, got %s", tmpl.Code)
	}
	if tmpl.Status != TemplateStatusActive {
		t.Fatalf("expected active status, got %s", tmpl.Status)
	}
	if tmpl.OutputMode != OutputModeInvoiceBatch {
		t.Fatalf("expected invoice_batch output mode, got %s", tmpl.OutputMode)
	}
}

func TestTemplateFromInputValidLeaseContract(t *testing.T) {
	input := UpsertTemplateInput{
		Code:         "lease-contract-a4",
		Name:         "Lease Contract A4",
		DocumentType: "lease_contract",
		OutputMode:   OutputModeLeaseContract,
		Title:        "Lease Contract Print",
		ActorUserID:  1,
	}
	tmpl, err := templateFromInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if tmpl.DocumentType != "lease_contract" || tmpl.OutputMode != OutputModeLeaseContract {
		t.Fatalf("expected lease contract template, got documentType=%s outputMode=%s", tmpl.DocumentType, tmpl.OutputMode)
	}
}

func TestTemplateFromInputRejectsMismatchedDocumentTypeAndOutputMode(t *testing.T) {
	input := UpsertTemplateInput{
		Code:         "lease-contract-bad-mode",
		Name:         "Lease Contract Bad Mode",
		DocumentType: "lease_contract",
		OutputMode:   OutputModeInvoiceDetail,
		Title:        "Lease Contract Print",
		ActorUserID:  1,
	}
	_, err := templateFromInput(input)
	if err != ErrInvalidTemplate {
		t.Fatalf("expected ErrInvalidTemplate, got %v", err)
	}
}

func TestTemplateFromInputMissingCode(t *testing.T) {
	input := UpsertTemplateInput{
		Name:         "No Code",
		DocumentType: "invoice",
		OutputMode:   OutputModeInvoiceBatch,
		Title:        "Title",
		ActorUserID:  1,
	}
	_, err := templateFromInput(input)
	if err != ErrInvalidTemplate {
		t.Fatalf("expected ErrInvalidTemplate, got %v", err)
	}
}

func TestTemplateFromInputMissingTitle(t *testing.T) {
	input := UpsertTemplateInput{
		Code:         "no-title",
		Name:         "No Title",
		DocumentType: "invoice",
		OutputMode:   OutputModeInvoiceBatch,
		ActorUserID:  1,
	}
	_, err := templateFromInput(input)
	if err != ErrInvalidTemplate {
		t.Fatalf("expected ErrInvalidTemplate, got %v", err)
	}
}

func TestTemplateFromInputInvalidDocumentType(t *testing.T) {
	input := UpsertTemplateInput{
		Code:         "bad-type",
		Name:         "Bad Type",
		DocumentType: "receipt",
		OutputMode:   OutputModeInvoiceBatch,
		Title:        "Title",
		ActorUserID:  1,
	}
	_, err := templateFromInput(input)
	if err != ErrInvalidTemplate {
		t.Fatalf("expected ErrInvalidTemplate, got %v", err)
	}
}

func TestTemplateFromInputInvalidOutputMode(t *testing.T) {
	input := UpsertTemplateInput{
		Code:         "bad-mode",
		Name:         "Bad Mode",
		DocumentType: "invoice",
		OutputMode:   "invalid_mode",
		Title:        "Title",
		ActorUserID:  1,
	}
	_, err := templateFromInput(input)
	if err != ErrInvalidTemplate {
		t.Fatalf("expected ErrInvalidTemplate, got %v", err)
	}
}

func TestNormalizeDocumentIDs(t *testing.T) {
	ids := []int64{3, 0, 1, 3, 2, 1}
	result := normalizeDocumentIDs(ids)
	if len(result) != 3 {
		t.Fatalf("expected 3 unique non-zero IDs, got %d: %v", len(result), result)
	}
	expected := []int64{3, 1, 2}
	for i, v := range expected {
		if result[i] != v {
			t.Fatalf("at index %d expected %d, got %d", i, v, result[i])
		}
	}
}

func TestNormalizeDocumentIDsEmpty(t *testing.T) {
	result := normalizeDocumentIDs(nil)
	if len(result) != 0 {
		t.Fatalf("expected empty result for nil input, got %v", result)
	}
	result = normalizeDocumentIDs([]int64{0})
	if len(result) != 0 {
		t.Fatalf("expected empty result for all-zero input, got %v", result)
	}
}

func TestRenderHTMLInvalidInput(t *testing.T) {
	svc := &Service{repository: nil, invoiceService: nil}
	_, err := svc.RenderHTML(context.TODO(), RenderInput{TemplateCode: "", ActorUserID: 0})
	if err != ErrInvalidRenderInput {
		t.Fatalf("expected ErrInvalidRenderInput, got %v", err)
	}
}

func TestPrintableFromInvoiceCarriesChargeSource(t *testing.T) {
	doc := invoice.Document{
		DocumentType: invoice.DocumentTypeInvoice,
		Status:       invoice.StatusApproved,
		TenantName:   "Harbor Foods",
		PeriodStart:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
		PeriodEnd:    time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
		TotalAmount:  600,
		Lines: []invoice.Line{{
			ChargeType:   "overtime_rent",
			ChargeSource: billing.ChargeSourceOvertime,
			PeriodStart:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			PeriodEnd:    time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
			QuantityDays: 30,
			UnitAmount:   20,
			Amount:       600,
		}},
	}

	printable := printableFromInvoice(doc)
	if len(printable.Lines) != 1 || printable.Lines[0].ChargeSource != "overtime" {
		t.Fatalf("expected overtime charge source on printable line, got %#v", printable)
	}
	htmlBytes, err := renderHTML(&Template{Title: "Invoice Print", HeaderLines: []string{"Header"}, FooterLines: []string{"Footer"}}, []printableDocument{printable})
	if err != nil {
		t.Fatalf("render html: %v", err)
	}
	html := string(htmlBytes)
	if !strings.Contains(html, "Source") || !strings.Contains(html, "overtime") {
		t.Fatalf("expected rendered html to include source column and overtime label, got %s", html)
	}
}
