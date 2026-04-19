package docoutput

import (
	"testing"
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
	_, err := svc.RenderHTML(nil, RenderInput{TemplateCode: "", ActorUserID: 0})
	if err != ErrInvalidRenderInput {
		t.Fatalf("expected ErrInvalidRenderInput, got %v", err)
	}
}
