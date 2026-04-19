package invoice

import (
	"math"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
)

func TestRoundMoney(t *testing.T) {
	tests := []struct {
		name   string
		input  float64
		expect float64
	}{
		{"zero", 0, 0},
		{"positive rounding down", 1.234, 1.23},
		{"positive rounding up", 1.235, 1.24},
		{"negative near zero", -0.0000001, 0},
		{"large value", 1234567.89, 1234567.89},
		{"three decimals rounds", 9.999, 10},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got := roundMoney(tt.input)
			if got != tt.expect {
				t.Fatalf("roundMoney(%v) = %v, want %v", tt.input, got, tt.expect)
			}
		})
	}
}

func TestRoundMoneyNeverReturnsNegativeZero(t *testing.T) {
	got := roundMoney(-0.0000001)
	if got != 0 {
		t.Fatalf("expected 0, got %v", got)
	}
	if math.Signbit(got) {
		t.Fatal("expected positive zero, got negative zero")
	}
}

func TestNormalizeChargeLineIDs(t *testing.T) {
	tests := []struct {
		name   string
		input  []int64
		expect []int64
	}{
		{"nil input", nil, []int64{}},
		{"empty input", []int64{}, []int64{}},
		{"removes zeros", []int64{0, 1, 0, 2}, []int64{1, 2}},
		{"deduplicates", []int64{3, 1, 3, 2, 1}, []int64{1, 2, 3}},
		{"sorts ascending", []int64{5, 3, 1}, []int64{1, 3, 5}},
		{"single value", []int64{42}, []int64{42}},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got := normalizeChargeLineIDs(tt.input)
			if len(got) != len(tt.expect) {
				t.Fatalf("normalizeChargeLineIDs(%v) = %v, want %v", tt.input, got, tt.expect)
			}
			for i := range got {
				if got[i] != tt.expect[i] {
					t.Fatalf("normalizeChargeLineIDs(%v) = %v, want %v", tt.input, got, tt.expect)
				}
			}
		})
	}
}

func TestDocumentFromChargeLinesValidates(t *testing.T) {
	baseTime := time.Date(2026, 1, 1, 0, 0, 0, 0, time.UTC)
	chargeLines := []billing.ChargeLine{
		{ID: 1, BillingRunID: 100, LeaseContractID: 200, TenantName: "Tenant A", PeriodStart: baseTime, PeriodEnd: baseTime.AddDate(0, 1, 0), CurrencyTypeID: 1, ChargeType: "rent", Amount: 500},
		{ID: 2, BillingRunID: 100, LeaseContractID: 200, TenantName: "Tenant A", PeriodStart: baseTime, PeriodEnd: baseTime.AddDate(0, 1, 0), CurrencyTypeID: 1, ChargeType: "management_fee", Amount: 300},
	}

	t.Run("creates document with correct totals", func(t *testing.T) {
		doc, err := documentFromChargeLines(CreateInput{DocumentType: DocumentTypeBill, BillingChargeLineIDs: []int64{1, 2}, ActorUserID: 1}, chargeLines)
		if err != nil {
			t.Fatalf("unexpected error: %v", err)
		}
		if doc.TotalAmount != 800 {
			t.Fatalf("expected total 800, got %v", doc.TotalAmount)
		}
		if doc.Status != StatusDraft {
			t.Fatalf("expected draft status, got %v", doc.Status)
		}
		if len(doc.Lines) != 2 {
			t.Fatalf("expected 2 lines, got %d", len(doc.Lines))
		}
	})

	t.Run("rejects mismatched billing run", func(t *testing.T) {
		mismatched := append([]billing.ChargeLine{}, chargeLines...)
		mismatched[1].BillingRunID = 999
		_, err := documentFromChargeLines(CreateInput{DocumentType: DocumentTypeBill, ActorUserID: 1}, mismatched)
		if err != ErrInvalidDocumentInput {
			t.Fatalf("expected ErrInvalidDocumentInput, got %v", err)
		}
	})

	t.Run("rejects mismatched lease contract", func(t *testing.T) {
		mismatched := append([]billing.ChargeLine{}, chargeLines...)
		mismatched[1].LeaseContractID = 999
		_, err := documentFromChargeLines(CreateInput{DocumentType: DocumentTypeBill, ActorUserID: 1}, mismatched)
		if err != ErrInvalidDocumentInput {
			t.Fatalf("expected ErrInvalidDocumentInput, got %v", err)
		}
	})
}

func TestAdjustedDocumentFromExisting(t *testing.T) {
	now := time.Now().UTC()
	original := Document{
		ID:              1,
		DocumentType:    DocumentTypeInvoice,
		BillingRunID:    100,
		LeaseContractID: 200,
		TenantName:      "Tenant A",
		PeriodStart:     now,
		PeriodEnd:       now.AddDate(0, 1, 0),
		CurrencyTypeID:  1,
		Status:          StatusApproved,
		Lines: []Line{
			{ID: 10, BillingChargeLineID: 101, ChargeType: "rent", Amount: 500, PeriodStart: now, PeriodEnd: now.AddDate(0, 1, 0)},
			{ID: 11, BillingChargeLineID: 102, ChargeType: "management_fee", Amount: 300, PeriodStart: now, PeriodEnd: now.AddDate(0, 1, 0)},
		},
	}

	t.Run("creates adjusted document with overrides", func(t *testing.T) {
		adjusted, err := adjustedDocumentFromExisting(original, AdjustInput{
			ActorUserID: 99,
			Lines: []AdjustLineInput{
				{BillingChargeLineID: 101, Amount: 600},
			},
		})
		if err != nil {
			t.Fatalf("unexpected error: %v", err)
		}
		if adjusted.TotalAmount != 900 {
			t.Fatalf("expected total 900, got %v", adjusted.TotalAmount)
		}
		if adjusted.Status != StatusDraft {
			t.Fatalf("expected draft status, got %v", adjusted.Status)
		}
		if adjusted.AdjustedFromID == nil || *adjusted.AdjustedFromID != 1 {
			t.Fatal("expected adjusted_from_id to point to original")
		}
		if adjusted.CreatedBy != 99 {
			t.Fatalf("expected created_by 99, got %v", adjusted.CreatedBy)
		}
	})

	t.Run("rejects missing actor", func(t *testing.T) {
		_, err := adjustedDocumentFromExisting(original, AdjustInput{ActorUserID: 0})
		if err != ErrInvalidDocumentInput {
			t.Fatalf("expected ErrInvalidDocumentInput, got %v", err)
		}
	})

	t.Run("rejects negative amounts", func(t *testing.T) {
		_, err := adjustedDocumentFromExisting(original, AdjustInput{
			ActorUserID: 1,
			Lines:       []AdjustLineInput{{BillingChargeLineID: 101, Amount: -10}},
		})
		if err != ErrInvalidDocumentInput {
			t.Fatalf("expected ErrInvalidDocumentInput, got %v", err)
		}
	})

	t.Run("rejects document with no lines", func(t *testing.T) {
		emptyOriginal := original
		emptyOriginal.Lines = nil
		_, err := adjustedDocumentFromExisting(emptyOriginal, AdjustInput{ActorUserID: 1})
		if err != ErrInvalidDocumentInput {
			t.Fatalf("expected ErrInvalidDocumentInput, got %v", err)
		}
	})
}

func TestMapWorkflowState(t *testing.T) {
	now := time.Now().UTC()

	t.Run("approved returns approved status with time", func(t *testing.T) {
		instance := &workflow.Instance{Status: workflow.InstanceStatusApproved, CompletedAt: &now}
		status, approvedAt := mapWorkflowState(instance)
		if status != StatusApproved {
			t.Fatalf("expected approved, got %v", status)
		}
		if approvedAt == nil {
			t.Fatal("expected approved_at to be set")
		}
	})

	t.Run("approved without completed_at uses current time", func(t *testing.T) {
		instance := &workflow.Instance{Status: workflow.InstanceStatusApproved, CompletedAt: nil}
		_, approvedAt := mapWorkflowState(instance)
		if approvedAt == nil {
			t.Fatal("expected approved_at to be set even without completed_at")
		}
	})

	t.Run("rejected returns rejected status", func(t *testing.T) {
		instance := &workflow.Instance{Status: workflow.InstanceStatusRejected}
		status, approvedAt := mapWorkflowState(instance)
		if status != StatusRejected {
			t.Fatalf("expected rejected, got %v", status)
		}
		if approvedAt != nil {
			t.Fatal("expected approved_at to be nil for rejected")
		}
	})

	t.Run("pending returns pending_approval status", func(t *testing.T) {
		instance := &workflow.Instance{Status: workflow.InstanceStatusPending}
		status, approvedAt := mapWorkflowState(instance)
		if status != StatusPendingApproval {
			t.Fatalf("expected pending_approval, got %v", status)
		}
		if approvedAt != nil {
			t.Fatal("expected approved_at to be nil for pending")
		}
	})
}

func TestSettlementStatus(t *testing.T) {
	tests := []struct {
		name   string
		amount float64
		expect SettlementStatus
	}{
		{"zero is settled", 0, SettlementStatusSettled},
		{"negative is settled", -1, SettlementStatusSettled},
		{"positive is outstanding", 0.01, SettlementStatusOutstanding},
		{"large positive is outstanding", 9999.99, SettlementStatusOutstanding},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got := settlementStatus(tt.amount)
			if got != tt.expect {
				t.Fatalf("settlementStatus(%v) = %v, want %v", tt.amount, got, tt.expect)
			}
		})
	}
}
