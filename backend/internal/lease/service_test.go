package lease

import (
	"errors"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
)

func TestIsValidTerm(t *testing.T) {
	tests := []struct {
		name string
		term TermInput
		want bool
	}{
		{
			name: "valid rent term",
			term: TermInput{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         12000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: true,
		},
		{
			name: "valid deposit term",
			term: TermInput{
				TermType:       TermTypeDeposit,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         5000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: true,
		},
		{
			name: "invalid term type",
			term: TermInput{
				TermType:       "management_fee",
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         1000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: false,
		},
		{
			name: "invalid billing cycle",
			term: TermInput{
				TermType:       TermTypeRent,
				BillingCycle:   "yearly",
				CurrencyTypeID: 1,
				Amount:         120000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: false,
		},
		{
			name: "zero currency type id",
			term: TermInput{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 0,
				Amount:         12000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: false,
		},
		{
			name: "negative amount",
			term: TermInput{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         -100,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: false,
		},
		{
			name: "effective from after to",
			term: TermInput{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         12000,
				EffectiveFrom:  time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			},
			want: false,
		},
		{
			name: "zero amount is valid",
			term: TermInput{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         0,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			},
			want: true,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			if got := isValidTerm(tt.term); got != tt.want {
				t.Errorf("isValidTerm() = %v, want %v", got, tt.want)
			}
		})
	}
}

func TestContractFromCreateInput(t *testing.T) {
	validInput := func() CreateDraftInput {
		return CreateDraftInput{
			LeaseNo:      "L001",
			Subtype:      ContractSubtypeStandard,
			DepartmentID: 1,
			StoreID:      1,
			TenantName:   "ACME Corp",
			StartDate:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:      time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			Units:        []UnitInput{{UnitID: 1, RentArea: 100}},
			Terms: []TermInput{{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         12000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			}},
			ActorUserID: 1,
		}
	}

	t.Run("valid input creates contract", func(t *testing.T) {
		contract, err := contractFromCreateInput(validInput())
		if err != nil {
			t.Fatalf("unexpected error: %v", err)
		}
		if contract.LeaseNo != "L001" {
			t.Errorf("LeaseNo = %q, want %q", contract.LeaseNo, "L001")
		}
		if contract.Status != StatusDraft {
			t.Errorf("Status = %q, want %q", contract.Status, StatusDraft)
		}
		if contract.EffectiveVersion != 1 {
			t.Errorf("EffectiveVersion = %d, want 1", contract.EffectiveVersion)
		}
		if len(contract.Units) != 1 || len(contract.Terms) != 1 {
			t.Errorf("Units=%d, Terms=%d, want 1 each", len(contract.Units), len(contract.Terms))
		}
	})

	t.Run("missing lease number", func(t *testing.T) {
		input := validInput()
		input.LeaseNo = ""
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("missing tenant name", func(t *testing.T) {
		input := validInput()
		input.TenantName = "   "
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("zero department id", func(t *testing.T) {
		input := validInput()
		input.DepartmentID = 0
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("zero store id", func(t *testing.T) {
		input := validInput()
		input.StoreID = 0
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("zero actor user id", func(t *testing.T) {
		input := validInput()
		input.ActorUserID = 0
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("end date before start date", func(t *testing.T) {
		input := validInput()
		input.EndDate = time.Date(2026, 3, 31, 0, 0, 0, 0, time.UTC)
		input.StartDate = time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("no units", func(t *testing.T) {
		input := validInput()
		input.Units = nil
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("no terms", func(t *testing.T) {
		input := validInput()
		input.Terms = nil
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("unit with zero unit id", func(t *testing.T) {
		input := validInput()
		input.Units = []UnitInput{{UnitID: 0, RentArea: 100}}
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("unit with zero rent area", func(t *testing.T) {
		input := validInput()
		input.Units = []UnitInput{{UnitID: 1, RentArea: 0}}
		_, err := contractFromCreateInput(input)
		if !errors.Is(err, ErrLeaseIncompleteForSubmission) {
			t.Errorf("expected ErrLeaseIncompleteForSubmission, got %v", err)
		}
	})

	t.Run("joint operation requires settlement and tax fields", func(t *testing.T) {
		input := validInput()
		input.Subtype = ContractSubtypeJointOperation
		input.JointOperation = &JointOperationFieldsInput{RentInc: "5% yearly"}
		_, err := contractFromCreateInput(input)
		if err == nil {
			t.Fatal("expected validation error")
		}
		var validationErr *ValidationError
		if !errors.As(err, &validationErr) {
			t.Fatalf("expected validation error, got %v", err)
		}
		if len(validationErr.Fields) == 0 {
			t.Fatal("expected field-level diagnostics")
		}
	})

	t.Run("ad board subtype preserves detail rows", func(t *testing.T) {
		input := validInput()
		input.Subtype = ContractSubtypeAdBoard
		input.AdBoards = []AdBoardDetailInput{{
			AdBoardID:    10,
			StartDate:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:      time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			RentArea:     20,
			Airtime:      12,
			Frequency:    AdBoardFrequencyWeek,
			FrequencyMon: true,
		}}
		contract, err := contractFromCreateInput(input)
		if err != nil {
			t.Fatalf("unexpected error: %v", err)
		}
		if contract.Subtype != ContractSubtypeAdBoard || len(contract.AdBoards) != 1 {
			t.Fatalf("expected ad board detail payload, got %#v", contract)
		}
	})
}

func TestIsSubmissionReady(t *testing.T) {
	t.Run("nil contract", func(t *testing.T) {
		if isSubmissionReady(nil) {
			t.Error("expected false for nil contract")
		}
	})

	t.Run("complete contract is ready", func(t *testing.T) {
		contract := &Contract{
			LeaseNo:      "L001",
			Subtype:      ContractSubtypeStandard,
			TenantName:   "ACME Corp",
			DepartmentID: 1,
			StoreID:      1,
			StartDate:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:      time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			Units:        []Unit{{UnitID: 1, RentArea: 100}},
			Terms: []Term{{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         12000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			}},
		}
		if !isSubmissionReady(contract) {
			t.Error("expected complete contract to be submission ready")
		}
	})

	t.Run("joint operation contract missing detail is not ready", func(t *testing.T) {
		contract := &Contract{
			LeaseNo:      "L001",
			Subtype:      ContractSubtypeJointOperation,
			TenantName:   "ACME Corp",
			DepartmentID: 1,
			StoreID:      1,
			StartDate:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:      time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			Units:        []Unit{{UnitID: 1, RentArea: 100}},
			Terms: []Term{{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         12000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			}},
		}
		if isSubmissionReady(contract) {
			t.Fatal("expected incomplete joint operation contract to be invalid")
		}
		fields := validateContractForSubmission(contract)
		if len(fields) == 0 || fields[0].Field != "joint_operation" {
			t.Fatalf("expected joint_operation diagnostics, got %#v", fields)
		}
	})

	t.Run("contract with empty lease no", func(t *testing.T) {
		contract := &Contract{
			LeaseNo:      "",
			TenantName:   "ACME",
			DepartmentID: 1,
			StoreID:      1,
			StartDate:    time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			EndDate:      time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			Units:        []Unit{{UnitID: 1, RentArea: 100}},
			Terms: []Term{{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         12000,
				EffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
				EffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			}},
		}
		if isSubmissionReady(contract) {
			t.Error("expected contract with empty lease no to not be ready")
		}
	})
}

func TestMapWorkflowState(t *testing.T) {
	completedAt := time.Date(2026, 4, 10, 12, 0, 0, 0, time.UTC)

	t.Run("approved instance maps to active", func(t *testing.T) {
		instance := &workflow.Instance{
			Status:      workflow.InstanceStatusApproved,
			CompletedAt: &completedAt,
		}
		status, approvedAt, billingEffectiveAt := mapWorkflowState(instance)
		if status != StatusActive {
			t.Errorf("status = %q, want %q", status, StatusActive)
		}
		if approvedAt == nil {
			t.Fatal("expected approvedAt to be set")
		}
		if !approvedAt.Equal(completedAt) {
			t.Errorf("approvedAt = %v, want %v", approvedAt, completedAt)
		}
		if billingEffectiveAt == nil {
			t.Fatal("expected billingEffectiveAt to be set")
		}
	})

	t.Run("rejected instance maps to rejected", func(t *testing.T) {
		instance := &workflow.Instance{
			Status:      workflow.InstanceStatusRejected,
			CompletedAt: &completedAt,
		}
		status, approvedAt, billingEffectiveAt := mapWorkflowState(instance)
		if status != StatusRejected {
			t.Errorf("status = %q, want %q", status, StatusRejected)
		}
		if approvedAt != nil {
			t.Errorf("expected approvedAt to be nil, got %v", approvedAt)
		}
		if billingEffectiveAt != nil {
			t.Errorf("expected billingEffectiveAt to be nil, got %v", billingEffectiveAt)
		}
	})

	t.Run("pending instance maps to pending_approval", func(t *testing.T) {
		instance := &workflow.Instance{
			Status: workflow.InstanceStatusPending,
		}
		status, approvedAt, billingEffectiveAt := mapWorkflowState(instance)
		if status != StatusPendingApproval {
			t.Errorf("status = %q, want %q", status, StatusPendingApproval)
		}
		if approvedAt != nil {
			t.Errorf("expected approvedAt to be nil, got %v", approvedAt)
		}
		if billingEffectiveAt != nil {
			t.Errorf("expected billingEffectiveAt to be nil, got %v", billingEffectiveAt)
		}
	})
}

func TestContractBillingEligible(t *testing.T) {
	t.Run("active with billing effective date", func(t *testing.T) {
		now := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
		c := Contract{
			Status:             StatusActive,
			BillingEffectiveAt: &now,
		}
		if !c.BillingEligible() {
			t.Error("expected active contract with billing effective date to be eligible")
		}
	})

	t.Run("active without billing effective date", func(t *testing.T) {
		c := Contract{
			Status:             StatusActive,
			BillingEffectiveAt: nil,
		}
		if c.BillingEligible() {
			t.Error("expected active contract without billing effective date to not be eligible")
		}
	})

	t.Run("draft with billing effective date", func(t *testing.T) {
		now := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
		c := Contract{
			Status:             StatusDraft,
			BillingEffectiveAt: &now,
		}
		if c.BillingEligible() {
			t.Error("expected draft contract to not be billing eligible")
		}
	})
}
