package taxexport

import (
	"context"
	"testing"
	"time"
)

func TestRuleSetFromInputValid(t *testing.T) {
	input := UpsertRuleSetInput{
		Code:         "invoice-standard",
		Name:         "Standard Invoice Export",
		DocumentType: "invoice",
		ActorUserID:  1,
		Rules: []UpsertRuleInput{{
			SequenceNo:          1,
			EntrySide:           EntrySideDebit,
			AccountNumber:       "1001",
			AccountName:         "Cash",
			ExplanationTemplate: "ITEMCODE rent for SYYYYMM",
		}},
	}
	ruleSet, err := ruleSetFromInput(input)
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if ruleSet.Code != "invoice-standard" {
		t.Fatalf("expected code invoice-standard, got %s", ruleSet.Code)
	}
	if ruleSet.DocumentType != "invoice" {
		t.Fatalf("expected document_type invoice, got %s", ruleSet.DocumentType)
	}
	if len(ruleSet.Rules) != 1 {
		t.Fatalf("expected 1 rule, got %d", len(ruleSet.Rules))
	}
	if ruleSet.Rules[0].ChargeTypeFilter != "*" {
		t.Fatalf("expected default charge_type_filter *, got %s", ruleSet.Rules[0].ChargeTypeFilter)
	}
}

func TestRuleSetFromInputMissingCode(t *testing.T) {
	input := UpsertRuleSetInput{
		Name:         "No Code",
		DocumentType: "invoice",
		ActorUserID:  1,
		Rules: []UpsertRuleInput{{
			SequenceNo:          1,
			EntrySide:           EntrySideDebit,
			AccountNumber:       "1001",
			AccountName:         "Cash",
			ExplanationTemplate: "test",
		}},
	}
	_, err := ruleSetFromInput(input)
	if err != ErrInvalidRuleSet {
		t.Fatalf("expected ErrInvalidRuleSet, got %v", err)
	}
}

func TestRuleSetFromInputInvalidDocumentType(t *testing.T) {
	input := UpsertRuleSetInput{
		Code:         "bad-doc-type",
		Name:         "Bad Doc Type",
		DocumentType: "receipt",
		ActorUserID:  1,
		Rules: []UpsertRuleInput{{
			SequenceNo:          1,
			EntrySide:           EntrySideDebit,
			AccountNumber:       "1001",
			AccountName:         "Cash",
			ExplanationTemplate: "test",
		}},
	}
	_, err := ruleSetFromInput(input)
	if err != ErrInvalidRuleSet {
		t.Fatalf("expected ErrInvalidRuleSet, got %v", err)
	}
}

func TestRuleSetFromInputNoRules(t *testing.T) {
	input := UpsertRuleSetInput{
		Code:         "no-rules",
		Name:         "No Rules",
		DocumentType: "bill",
		ActorUserID:  1,
		Rules:        []UpsertRuleInput{},
	}
	_, err := ruleSetFromInput(input)
	if err != ErrInvalidRuleSet {
		t.Fatalf("expected ErrInvalidRuleSet, got %v", err)
	}
}

func TestValidateRuleSetInactive(t *testing.T) {
	ruleSet := RuleSet{
		Status: "inactive",
		Rules:  []Rule{{EntrySide: EntrySideDebit}},
	}
	if err := validateRuleSet(ruleSet); err != ErrInvalidRuleSet {
		t.Fatalf("expected ErrInvalidRuleSet for inactive, got %v", err)
	}
}

func TestValidateRuleSetAllBalancing(t *testing.T) {
	ruleSet := RuleSet{
		Status: RuleSetStatusActive,
		Rules:  []Rule{{EntrySide: EntrySideDebit, IsBalancingEntry: true}},
	}
	if err := validateRuleSet(ruleSet); err != ErrInvalidRuleSet {
		t.Fatalf("expected ErrInvalidRuleSet for all-balancing, got %v", err)
	}
}

func TestExpandExplanation(t *testing.T) {
	doc := exportDocument{
		PeriodStart: time.Date(2025, 1, 1, 0, 0, 0, 0, time.UTC),
		PeriodEnd:   time.Date(2025, 1, 31, 0, 0, 0, 0, time.UTC),
	}
	line := exportLine{ChargeType: "rent"}
	result := expandExplanation("ITEMCODE for YYYYMMDD-YYYYMMDD", doc, line)
	if result != "rent for 20250101-20250131" {
		t.Fatalf("unexpected expansion: %s", result)
	}
}

func TestDocumentQuoted(t *testing.T) {
	if v := documentQuoted("1001"); v != "'1001" {
		t.Fatalf("expected '1001, got %s", v)
	}
}

func TestRoundCurrency(t *testing.T) {
	if v := roundCurrency(1.005); v != 1.0 {
		t.Fatalf("expected 1.0, got %f", v)
	}
	if v := roundCurrency(1.006); v != 1.01 {
		t.Fatalf("expected 1.01, got %f", v)
	}
}

func TestExportVoucherWorkbookInvalidWindow(t *testing.T) {
	svc := &Service{repository: nil}
	_, err := svc.ExportVoucherWorkbook(context.TODO(), ExportInput{RuleSetCode: "", ActorUserID: 0})
	if err != ErrInvalidExportWindow {
		t.Fatalf("expected ErrInvalidExportWindow, got %v", err)
	}
}
