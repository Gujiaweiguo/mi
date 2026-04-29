package lease

import (
	"errors"
	"testing"
	"time"
)

var (
	testStart = time.Date(2025, 1, 1, 0, 0, 0, 0, time.UTC)
	testEnd   = time.Date(2025, 12, 31, 0, 0, 0, 0, time.UTC)
)

func validCreateDraftInput() CreateDraftInput {
	return CreateDraftInput{
		LeaseNo:      "L-001",
		Subtype:      ContractSubtypeStandard,
		DepartmentID: 1,
		StoreID:      1,
		TenantName:   "Tenant A",
		StartDate:    testStart,
		EndDate:      testEnd,
		ActorUserID:  1,
		Units: []UnitInput{
			{UnitID: 1, RentArea: 100.5},
		},
		Terms: []TermInput{
			{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         5000,
				EffectiveFrom:  testStart,
				EffectiveTo:    testEnd,
			},
		},
	}
}

func assertFieldPresent(t *testing.T, fields []ValidationField, expectedField string) {
	t.Helper()
	for _, f := range fields {
		if f.Field == expectedField {
			return
		}
	}
	t.Errorf("expected field %q in validation fields, got %+v", expectedField, fields)
}

func assertNoField(t *testing.T, fields []ValidationField, unexpectedField string) {
	t.Helper()
	for _, f := range fields {
		if f.Field == unexpectedField {
			t.Errorf("did not expect field %q but it was present", unexpectedField)
			return
		}
	}
}

func TestValidationNormalizeSubtype(t *testing.T) {
	t.Run("empty returns standard", func(t *testing.T) {
		got := normalizeSubtype("")
		if got != ContractSubtypeStandard {
			t.Errorf("expected %q, got %q", ContractSubtypeStandard, got)
		}
	})

	t.Run("ad_board passes through", func(t *testing.T) {
		got := normalizeSubtype(ContractSubtypeAdBoard)
		if got != ContractSubtypeAdBoard {
			t.Errorf("expected %q, got %q", ContractSubtypeAdBoard, got)
		}
	})

	t.Run("standard passes through", func(t *testing.T) {
		got := normalizeSubtype(ContractSubtypeStandard)
		if got != ContractSubtypeStandard {
			t.Errorf("expected %q, got %q", ContractSubtypeStandard, got)
		}
	})
}

func TestValidationNewValidationError(t *testing.T) {
	t.Run("nil fields returns nil", func(t *testing.T) {
		got := newValidationError(nil)
		if got != nil {
			t.Errorf("expected nil, got %v", got)
		}
	})

	t.Run("empty slice returns nil", func(t *testing.T) {
		got := newValidationError([]ValidationField{})
		if got != nil {
			t.Errorf("expected nil, got %v", got)
		}
	})

	t.Run("with fields returns ValidationError", func(t *testing.T) {
		fields := []ValidationField{
			{Field: "test_field", Message: "test message"},
		}
		got := newValidationError(fields)
		if got == nil {
			t.Fatal("expected non-nil error")
		}
		ve, ok := got.(*ValidationError)
		if !ok {
			t.Fatalf("expected *ValidationError, got %T", got)
		}
		if len(ve.Fields) != 1 {
			t.Fatalf("expected 1 field, got %d", len(ve.Fields))
		}
		if ve.Fields[0].Field != "test_field" {
			t.Errorf("expected field %q, got %q", "test_field", ve.Fields[0].Field)
		}
		if !errors.Is(got, ErrLeaseIncompleteForSubmission) {
			t.Error("expected error to wrap ErrLeaseIncompleteForSubmission")
		}
	})
}

func TestValidationValidateCreateDraftInputValid(t *testing.T) {
	input := validCreateDraftInput()
	fields := validateCreateDraftInput(input)
	if len(fields) != 0 {
		t.Errorf("expected 0 fields for valid input, got %d: %+v", len(fields), fields)
	}
}

func TestValidationValidateCreateDraftInputLeaseNo(t *testing.T) {
	input := validCreateDraftInput()
	input.LeaseNo = ""
	assertFieldPresent(t, validateCreateDraftInput(input), "lease_no")

	input.LeaseNo = "   "
	assertFieldPresent(t, validateCreateDraftInput(input), "lease_no")
}

func TestValidationValidateCreateDraftInputTenantName(t *testing.T) {
	input := validCreateDraftInput()
	input.TenantName = ""
	assertFieldPresent(t, validateCreateDraftInput(input), "tenant_name")
}

func TestValidationValidateCreateDraftInputDepartmentID(t *testing.T) {
	input := validCreateDraftInput()
	input.DepartmentID = 0
	assertFieldPresent(t, validateCreateDraftInput(input), "department_id")
}

func TestValidationValidateCreateDraftInputStoreID(t *testing.T) {
	input := validCreateDraftInput()
	input.StoreID = 0
	assertFieldPresent(t, validateCreateDraftInput(input), "store_id")
}

func TestValidationValidateCreateDraftInputActorUserID(t *testing.T) {
	input := validCreateDraftInput()
	input.ActorUserID = 0
	assertFieldPresent(t, validateCreateDraftInput(input), "actor_user_id")
}

func TestValidationValidateCreateDraftInputDateRange(t *testing.T) {
	t.Run("start equals end", func(t *testing.T) {
		input := validCreateDraftInput()
		input.StartDate = testStart
		input.EndDate = testStart
		assertFieldPresent(t, validateCreateDraftInput(input), "date_range")
	})

	t.Run("start after end", func(t *testing.T) {
		input := validCreateDraftInput()
		input.StartDate = testEnd
		input.EndDate = testStart
		assertFieldPresent(t, validateCreateDraftInput(input), "date_range")
	})

	t.Run("start before end is valid", func(t *testing.T) {
		input := validCreateDraftInput()
		input.StartDate = testStart
		input.EndDate = testEnd
		assertNoField(t, validateCreateDraftInput(input), "date_range")
	})
}

func TestValidationValidateCreateDraftInputUnits(t *testing.T) {
	t.Run("no units", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Units = nil
		assertFieldPresent(t, validateCreateDraftInput(input), "units")
	})

	t.Run("unit_id zero", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Units = []UnitInput{{UnitID: 0, RentArea: 50}}
		assertFieldPresent(t, validateCreateDraftInput(input), "units[0].unit_id")
	})

	t.Run("rent_area zero", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Units = []UnitInput{{UnitID: 1, RentArea: 0}}
		assertFieldPresent(t, validateCreateDraftInput(input), "units[0].rent_area")
	})

	t.Run("rent_area negative", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Units = []UnitInput{{UnitID: 1, RentArea: -10}}
		assertFieldPresent(t, validateCreateDraftInput(input), "units[0].rent_area")
	})
}

func TestValidationValidateCreateDraftInputTerms(t *testing.T) {
	t.Run("no terms", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Terms = nil
		assertFieldPresent(t, validateCreateDraftInput(input), "terms")
	})

	t.Run("invalid term_type", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Terms = []TermInput{
			{
				TermType:       "invalid",
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         100,
				EffectiveFrom:  testStart,
				EffectiveTo:    testEnd,
			},
		}
		assertFieldPresent(t, validateCreateDraftInput(input), "terms[0].term_type")
	})

	t.Run("invalid billing_cycle", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Terms = []TermInput{
			{
				TermType:       TermTypeRent,
				BillingCycle:   "yearly",
				CurrencyTypeID: 1,
				Amount:         100,
				EffectiveFrom:  testStart,
				EffectiveTo:    testEnd,
			},
		}
		assertFieldPresent(t, validateCreateDraftInput(input), "terms[0].billing_cycle")
	})

	t.Run("effective_from after effective_to", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Terms = []TermInput{
			{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         100,
				EffectiveFrom:  testEnd,
				EffectiveTo:    testStart,
			},
		}
		assertFieldPresent(t, validateCreateDraftInput(input), "terms[0].effective_range")
	})

	t.Run("currency_type_id zero", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Terms = []TermInput{
			{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 0,
				Amount:         100,
				EffectiveFrom:  testStart,
				EffectiveTo:    testEnd,
			},
		}
		assertFieldPresent(t, validateCreateDraftInput(input), "terms[0].currency_type_id")
	})

	t.Run("negative amount", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Terms = []TermInput{
			{
				TermType:       TermTypeRent,
				BillingCycle:   BillingCycleMonthly,
				CurrencyTypeID: 1,
				Amount:         -50,
				EffectiveFrom:  testStart,
				EffectiveTo:    testEnd,
			},
		}
		assertFieldPresent(t, validateCreateDraftInput(input), "terms[0].amount")
	})
}

func TestValidationJointOperationInput(t *testing.T) {
	t.Run("nil returns error", func(t *testing.T) {
		fields := validateJointOperationInput(nil)
		if len(fields) != 1 || fields[0].Field != "joint_operation" {
			t.Errorf("expected single field 'joint_operation', got %+v", fields)
		}
	})

	t.Run("bill_cycle zero", func(t *testing.T) {
		input := &JointOperationFieldsInput{BillCycle: 0}
		assertFieldPresent(t, validateJointOperationInput(input), "joint_operation.bill_cycle")
	})

	t.Run("valid joint operation passes", func(t *testing.T) {
		input := &JointOperationFieldsInput{
			BillCycle:                1,
			RentInc:                  "Y",
			AccountCycle:             1,
			TaxRate:                  0.06,
			TaxType:                  1,
			SettlementCurrencyTypeID: 1,
			InTaxRate:                0.06,
			OutTaxRate:               0.06,
			MonthSettleDays:          0,
			LatePayInterestRate:      0,
			InterestGraceDays:        0,
		}
		fields := validateJointOperationInput(input)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields for valid joint operation, got %d: %+v", len(fields), fields)
		}
	})

	t.Run("all invalid fields", func(t *testing.T) {
		input := &JointOperationFieldsInput{
			BillCycle:                0,
			RentInc:                  "",
			AccountCycle:             0,
			TaxRate:                  0,
			TaxType:                  0,
			SettlementCurrencyTypeID: 0,
			InTaxRate:                0,
			OutTaxRate:               0,
			MonthSettleDays:          -1,
			LatePayInterestRate:      -1,
			InterestGraceDays:        -1,
		}
		fields := validateJointOperationInput(input)
		expected := []string{
			"joint_operation.bill_cycle",
			"joint_operation.rent_inc",
			"joint_operation.account_cycle",
			"joint_operation.tax_rate",
			"joint_operation.tax_type",
			"joint_operation.settlement_currency_type_id",
			"joint_operation.in_tax_rate",
			"joint_operation.out_tax_rate",
			"joint_operation.month_settle_days",
			"joint_operation.late_pay_interest_rate",
			"joint_operation.interest_grace_days",
		}
		for _, ef := range expected {
			assertFieldPresent(t, fields, ef)
		}
		if len(fields) != len(expected) {
			t.Errorf("expected %d fields, got %d", len(expected), len(fields))
		}
	})
}

func TestValidationAdBoardDetailInput(t *testing.T) {
	t.Run("ad_board_id zero", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:  0,
			StartDate:  testStart,
			EndDate:    testEnd,
			RentArea:   50,
			Airtime:    30,
			Frequency:  AdBoardFrequencyDay,
			FrequencyDays: 1,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].ad_board_id")
	})

	t.Run("invalid frequency", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:  1,
			StartDate:  testStart,
			EndDate:    testEnd,
			RentArea:   50,
			Airtime:    30,
			Frequency:  "X",
			FrequencyDays: 1,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].frequency")
	})

	t.Run("valid day frequency", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:     1,
			StartDate:     testStart,
			EndDate:       testEnd,
			RentArea:      50,
			Airtime:       30,
			Frequency:     AdBoardFrequencyDay,
			FrequencyDays: 1,
		}
		fields := validateAdBoardDetailInput(0, detail)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})

	t.Run("valid week frequency with weekday", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:   1,
			StartDate:   testStart,
			EndDate:     testEnd,
			RentArea:    50,
			Airtime:     30,
			Frequency:   AdBoardFrequencyWeek,
			FrequencyMon: true,
		}
		fields := validateAdBoardDetailInput(0, detail)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})

	t.Run("week frequency no weekdays", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID: 1,
			StartDate: testStart,
			EndDate:   testEnd,
			RentArea:  50,
			Airtime:   30,
			Frequency: AdBoardFrequencyWeek,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].frequency_weekdays")
	})

	t.Run("start_date not before end_date", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:     1,
			StartDate:     testEnd,
			EndDate:       testStart,
			RentArea:      50,
			Airtime:       30,
			Frequency:     AdBoardFrequencyDay,
			FrequencyDays: 1,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].date_range")
	})

	t.Run("rent_area zero", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:     1,
			StartDate:     testStart,
			EndDate:       testEnd,
			RentArea:      0,
			Airtime:       30,
			Frequency:     AdBoardFrequencyDay,
			FrequencyDays: 1,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].rent_area")
	})

	t.Run("airtime zero", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:     1,
			StartDate:     testStart,
			EndDate:       testEnd,
			RentArea:      50,
			Airtime:       0,
			Frequency:     AdBoardFrequencyDay,
			FrequencyDays: 1,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].airtime")
	})

	t.Run("day frequency with frequency_days zero", func(t *testing.T) {
		detail := AdBoardDetailInput{
			AdBoardID:     1,
			StartDate:     testStart,
			EndDate:       testEnd,
			RentArea:      50,
			Airtime:       30,
			Frequency:     AdBoardFrequencyDay,
			FrequencyDays: 0,
		}
		assertFieldPresent(t, validateAdBoardDetailInput(0, detail), "ad_boards[0].frequency_days")
	})

	t.Run("index is used in field names", func(t *testing.T) {
		detail := AdBoardDetailInput{AdBoardID: 0, Frequency: "X", StartDate: testStart, EndDate: testEnd, RentArea: 50, Airtime: 30}
		fields := validateAdBoardDetailInput(2, detail)
		assertFieldPresent(t, fields, "ad_boards[2].ad_board_id")
		assertFieldPresent(t, fields, "ad_boards[2].frequency")
	})
}

func TestValidationAreaGroundDetailInput(t *testing.T) {
	t.Run("empty code", func(t *testing.T) {
		detail := AreaGroundDetailInput{
			Code:      "",
			Name:      "Area 1",
			TypeID:    1,
			StartDate: testStart,
			EndDate:   testEnd,
			RentArea:  100,
		}
		assertFieldPresent(t, validateAreaGroundDetailInput(0, detail), "area_grounds[0].code")
	})

	t.Run("empty name", func(t *testing.T) {
		detail := AreaGroundDetailInput{
			Code:      "AG-001",
			Name:      "",
			TypeID:    1,
			StartDate: testStart,
			EndDate:   testEnd,
			RentArea:  100,
		}
		assertFieldPresent(t, validateAreaGroundDetailInput(0, detail), "area_grounds[0].name")
	})

	t.Run("type_id zero", func(t *testing.T) {
		detail := AreaGroundDetailInput{
			Code:      "AG-001",
			Name:      "Area 1",
			TypeID:    0,
			StartDate: testStart,
			EndDate:   testEnd,
			RentArea:  100,
		}
		assertFieldPresent(t, validateAreaGroundDetailInput(0, detail), "area_grounds[0].type_id")
	})

	t.Run("valid area ground", func(t *testing.T) {
		detail := AreaGroundDetailInput{
			Code:      "AG-001",
			Name:      "Area 1",
			TypeID:    1,
			StartDate: testStart,
			EndDate:   testEnd,
			RentArea:  100,
		}
		fields := validateAreaGroundDetailInput(0, detail)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})
}

func TestValidationRejectUnexpectedSubtypeData(t *testing.T) {
	t.Run("standard with joint operation", func(t *testing.T) {
		fields := rejectUnexpectedSubtypeData(ContractSubtypeStandard, true, false, false)
		assertFieldPresent(t, fields, "joint_operation")
		if len(fields) != 1 {
			t.Errorf("expected 1 field, got %d", len(fields))
		}
	})

	t.Run("standard with ad boards", func(t *testing.T) {
		fields := rejectUnexpectedSubtypeData(ContractSubtypeStandard, false, true, false)
		assertFieldPresent(t, fields, "ad_boards")
	})

	t.Run("standard with area grounds", func(t *testing.T) {
		fields := rejectUnexpectedSubtypeData(ContractSubtypeStandard, false, false, true)
		assertFieldPresent(t, fields, "area_grounds")
	})

	t.Run("standard with nothing extra", func(t *testing.T) {
		fields := rejectUnexpectedSubtypeData(ContractSubtypeStandard, false, false, false)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})

	t.Run("all unexpected data", func(t *testing.T) {
		fields := rejectUnexpectedSubtypeData(ContractSubtypeStandard, true, true, true)
		if len(fields) != 3 {
			t.Errorf("expected 3 fields, got %d", len(fields))
		}
	})
}

func TestValidationSubtypeInputAdBoard(t *testing.T) {
	t.Run("ad_board without ad_boards data", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Subtype = ContractSubtypeAdBoard
		input.AdBoards = nil
		fields := validateSubtypeInput(input)
		assertFieldPresent(t, fields, "ad_boards")
	})

	t.Run("ad_board with valid ad board", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Subtype = ContractSubtypeAdBoard
		input.AdBoards = []AdBoardDetailInput{
			{
				AdBoardID:     1,
				StartDate:     testStart,
				EndDate:       testEnd,
				RentArea:      50,
				Airtime:       30,
				Frequency:     AdBoardFrequencyDay,
				FrequencyDays: 1,
			},
		}
		fields := validateSubtypeInput(input)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})
}

func TestValidationSubtypeInputAreaGround(t *testing.T) {
	t.Run("area_ground without area_grounds data", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Subtype = ContractSubtypeAreaGround
		input.AreaGrounds = nil
		fields := validateSubtypeInput(input)
		assertFieldPresent(t, fields, "area_grounds")
	})

	t.Run("area_ground with valid data", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Subtype = ContractSubtypeAreaGround
		input.AreaGrounds = []AreaGroundDetailInput{
			{
				Code:      "AG-001",
				Name:      "Area 1",
				TypeID:    1,
				StartDate: testStart,
				EndDate:   testEnd,
				RentArea:  100,
			},
		}
		fields := validateSubtypeInput(input)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})
}

func TestValidationSubtypeInputJointOperation(t *testing.T) {
	t.Run("joint_operation without joint operation data", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Subtype = ContractSubtypeJointOperation
		input.JointOperation = nil
		fields := validateSubtypeInput(input)
		assertFieldPresent(t, fields, "joint_operation")
	})

	t.Run("joint_operation with valid data", func(t *testing.T) {
		input := validCreateDraftInput()
		input.Subtype = ContractSubtypeJointOperation
		input.JointOperation = &JointOperationFieldsInput{
			BillCycle:                1,
			RentInc:                  "Y",
			AccountCycle:             1,
			TaxRate:                  0.06,
			TaxType:                  1,
			SettlementCurrencyTypeID: 1,
			InTaxRate:                0.06,
			OutTaxRate:               0.06,
		}
		fields := validateSubtypeInput(input)
		if len(fields) != 0 {
			t.Errorf("expected 0 fields, got %d: %+v", len(fields), fields)
		}
	})
}

func TestValidationSubtypeInputUnknown(t *testing.T) {
	input := validCreateDraftInput()
	input.Subtype = "unknown_subtype"
	fields := validateSubtypeInput(input)
	assertFieldPresent(t, fields, "subtype")
}

func TestValidationErrorError(t *testing.T) {
	t.Run("non-nil with cause returns cause message", func(t *testing.T) {
		ve := &ValidationError{cause: ErrLeaseIncompleteForSubmission, Fields: nil}
		if ve.Error() != ErrLeaseIncompleteForSubmission.Error() {
			t.Errorf("expected %q, got %q", ErrLeaseIncompleteForSubmission.Error(), ve.Error())
		}
	})
}

func TestValidationErrorUnwrap(t *testing.T) {
	t.Run("non-nil returns cause", func(t *testing.T) {
		ve := &ValidationError{cause: ErrLeaseIncompleteForSubmission}
		if !errors.Is(ve, ErrLeaseIncompleteForSubmission) {
			t.Error("expected Unwrap to return ErrLeaseIncompleteForSubmission")
		}
	})
}
