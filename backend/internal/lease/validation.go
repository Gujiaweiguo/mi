package lease

import (
	"fmt"
	"strings"
)

type ValidationField struct {
	Field   string `json:"field"`
	Message string `json:"message"`
}

type ValidationError struct {
	cause  error
	Fields []ValidationField `json:"fields"`
}

func (e *ValidationError) Error() string {
	if e == nil || e.cause == nil {
		return ErrLeaseIncompleteForSubmission.Error()
	}
	return e.cause.Error()
}

func (e *ValidationError) Unwrap() error {
	if e == nil {
		return nil
	}
	return e.cause
}

func newValidationError(fields []ValidationField) error {
	if len(fields) == 0 {
		return nil
	}
	return &ValidationError{cause: ErrLeaseIncompleteForSubmission, Fields: fields}
}

func validateCreateDraftInput(input CreateDraftInput) []ValidationField {
	fields := make([]ValidationField, 0)
	if strings.TrimSpace(input.LeaseNo) == "" {
		fields = append(fields, ValidationField{Field: "lease_no", Message: "lease number is required"})
	}
	if strings.TrimSpace(input.TenantName) == "" {
		fields = append(fields, ValidationField{Field: "tenant_name", Message: "tenant name is required"})
	}
	if input.DepartmentID == 0 {
		fields = append(fields, ValidationField{Field: "department_id", Message: "department_id is required"})
	}
	if input.StoreID == 0 {
		fields = append(fields, ValidationField{Field: "store_id", Message: "store_id is required"})
	}
	if input.ActorUserID == 0 {
		fields = append(fields, ValidationField{Field: "actor_user_id", Message: "actor_user_id is required"})
	}
	if !input.StartDate.Before(input.EndDate) {
		fields = append(fields, ValidationField{Field: "date_range", Message: "start_date must be before end_date"})
	}
	if len(input.Units) == 0 {
		fields = append(fields, ValidationField{Field: "units", Message: "at least one unit is required"})
	}
	for i, unit := range input.Units {
		if unit.UnitID == 0 {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("units[%d].unit_id", i), Message: "unit_id is required"})
		}
		if unit.RentArea <= 0 {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("units[%d].rent_area", i), Message: "rent_area must be greater than zero"})
		}
	}
	if len(input.Terms) == 0 {
		fields = append(fields, ValidationField{Field: "terms", Message: "at least one term is required"})
	}
	for i, term := range input.Terms {
		_ = isValidTerm(term)
		if term.CurrencyTypeID == 0 {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("terms[%d].currency_type_id", i), Message: "currency_type_id is required"})
		}
		if term.Amount < 0 {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("terms[%d].amount", i), Message: "amount must be zero or greater"})
		}
		if term.TermType != TermTypeRent && term.TermType != TermTypeDeposit {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("terms[%d].term_type", i), Message: "term_type must be rent or deposit"})
		}
		if term.BillingCycle != BillingCycleMonthly {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("terms[%d].billing_cycle", i), Message: "billing_cycle must be monthly"})
		}
		if term.EffectiveFrom.After(term.EffectiveTo) {
			fields = append(fields, ValidationField{Field: fmt.Sprintf("terms[%d].effective_range", i), Message: "effective_from must be on or before effective_to"})
		}
	}
	return append(fields, validateSubtypeInput(input)...)
}

func validateSubtypeInput(input CreateDraftInput) []ValidationField {
	subtype := normalizeSubtype(input.Subtype)
	fields := make([]ValidationField, 0)
	switch subtype {
	case ContractSubtypeStandard:
		fields = append(fields, rejectUnexpectedSubtypeData(subtype, input.JointOperation != nil, len(input.AdBoards) > 0, len(input.AreaGrounds) > 0)...)
	case ContractSubtypeJointOperation:
		fields = append(fields, rejectUnexpectedSubtypeData(subtype, false, len(input.AdBoards) > 0, len(input.AreaGrounds) > 0)...)
		fields = append(fields, validateJointOperationInput(input.JointOperation)...)
	case ContractSubtypeAdBoard:
		fields = append(fields, rejectUnexpectedSubtypeData(subtype, input.JointOperation != nil, false, len(input.AreaGrounds) > 0)...)
		if len(input.AdBoards) == 0 {
			fields = append(fields, ValidationField{Field: "ad_boards", Message: "at least one ad board detail is required"})
		}
		for i, detail := range input.AdBoards {
			fields = append(fields, validateAdBoardDetailInput(i, detail)...)
		}
	case ContractSubtypeAreaGround:
		fields = append(fields, rejectUnexpectedSubtypeData(subtype, input.JointOperation != nil, len(input.AdBoards) > 0, false)...)
		if len(input.AreaGrounds) == 0 {
			fields = append(fields, ValidationField{Field: "area_grounds", Message: "at least one area/ground detail is required"})
		}
		for i, detail := range input.AreaGrounds {
			fields = append(fields, validateAreaGroundDetailInput(i, detail)...)
		}
	default:
		fields = append(fields, ValidationField{Field: "subtype", Message: "unsupported lease subtype"})
	}
	return fields
}

func validateContractForSubmission(contract *Contract) []ValidationField {
	if contract == nil {
		return []ValidationField{{Field: "lease", Message: "lease contract is required"}}
	}
	input := CreateDraftInput{
		LeaseNo:      contract.LeaseNo,
		Subtype:      contract.Subtype,
		DepartmentID: contract.DepartmentID,
		StoreID:      contract.StoreID,
		TenantName:   contract.TenantName,
		StartDate:    contract.StartDate,
		EndDate:      contract.EndDate,
		ActorUserID:  contract.CreatedBy,
		Units:        make([]UnitInput, 0, len(contract.Units)),
		Terms:        make([]TermInput, 0, len(contract.Terms)),
		AdBoards:     make([]AdBoardDetailInput, 0, len(contract.AdBoards)),
		AreaGrounds:  make([]AreaGroundDetailInput, 0, len(contract.AreaGrounds)),
	}
	if input.ActorUserID == 0 {
		input.ActorUserID = 1
	}
	if contract.JointOperation != nil {
		input.JointOperation = &JointOperationFieldsInput{
			BillCycle:                contract.JointOperation.BillCycle,
			RentInc:                  contract.JointOperation.RentInc,
			AccountCycle:             contract.JointOperation.AccountCycle,
			TaxRate:                  contract.JointOperation.TaxRate,
			TaxType:                  contract.JointOperation.TaxType,
			SettlementCurrencyTypeID: contract.JointOperation.SettlementCurrencyTypeID,
			InTaxRate:                contract.JointOperation.InTaxRate,
			OutTaxRate:               contract.JointOperation.OutTaxRate,
			MonthSettleDays:          contract.JointOperation.MonthSettleDays,
			LatePayInterestRate:      contract.JointOperation.LatePayInterestRate,
			InterestGraceDays:        contract.JointOperation.InterestGraceDays,
		}
	}
	for _, unit := range contract.Units {
		input.Units = append(input.Units, UnitInput{UnitID: unit.UnitID, RentArea: unit.RentArea})
	}
	for _, term := range contract.Terms {
		input.Terms = append(input.Terms, TermInput{TermType: term.TermType, BillingCycle: term.BillingCycle, CurrencyTypeID: term.CurrencyTypeID, Amount: term.Amount, EffectiveFrom: term.EffectiveFrom, EffectiveTo: term.EffectiveTo})
	}
	for _, detail := range contract.AdBoards {
		input.AdBoards = append(input.AdBoards, AdBoardDetailInput{
			AdBoardID:     detail.AdBoardID,
			Description:   detail.Description,
			Status:        detail.Status,
			StartDate:     detail.StartDate,
			EndDate:       detail.EndDate,
			RentArea:      detail.RentArea,
			Airtime:       detail.Airtime,
			Frequency:     detail.Frequency,
			FrequencyDays: detail.FrequencyDays,
			FrequencyMon:  detail.FrequencyMon,
			FrequencyTue:  detail.FrequencyTue,
			FrequencyWed:  detail.FrequencyWed,
			FrequencyThu:  detail.FrequencyThu,
			FrequencyFri:  detail.FrequencyFri,
			FrequencySat:  detail.FrequencySat,
			FrequencySun:  detail.FrequencySun,
			BetweenFrom:   detail.BetweenFrom,
			BetweenTo:     detail.BetweenTo,
			StoreID:       detail.StoreID,
			BuildingID:    detail.BuildingID,
		})
	}
	for _, detail := range contract.AreaGrounds {
		input.AreaGrounds = append(input.AreaGrounds, AreaGroundDetailInput{
			Code:        detail.Code,
			Name:        detail.Name,
			TypeID:      detail.TypeID,
			Description: detail.Description,
			Status:      detail.Status,
			StartDate:   detail.StartDate,
			EndDate:     detail.EndDate,
			RentArea:    detail.RentArea,
		})
	}
	return validateCreateDraftInput(input)
}

func rejectUnexpectedSubtypeData(subtype ContractSubtype, hasJointOperation, hasAdBoards, hasAreaGrounds bool) []ValidationField {
	fields := make([]ValidationField, 0)
	if hasJointOperation {
		fields = append(fields, ValidationField{Field: "joint_operation", Message: fmt.Sprintf("joint_operation data is not allowed for subtype %s", subtype)})
	}
	if hasAdBoards {
		fields = append(fields, ValidationField{Field: "ad_boards", Message: fmt.Sprintf("ad_boards data is not allowed for subtype %s", subtype)})
	}
	if hasAreaGrounds {
		fields = append(fields, ValidationField{Field: "area_grounds", Message: fmt.Sprintf("area_grounds data is not allowed for subtype %s", subtype)})
	}
	return fields
}

func validateJointOperationInput(input *JointOperationFieldsInput) []ValidationField {
	if input == nil {
		return []ValidationField{{Field: "joint_operation", Message: "joint_operation detail is required"}}
	}
	fields := make([]ValidationField, 0)
	if input.BillCycle <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.bill_cycle", Message: "bill_cycle must be greater than zero"})
	}
	if strings.TrimSpace(input.RentInc) == "" {
		fields = append(fields, ValidationField{Field: "joint_operation.rent_inc", Message: "rent_inc is required"})
	}
	if input.AccountCycle <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.account_cycle", Message: "account_cycle must be greater than zero"})
	}
	if input.TaxRate <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.tax_rate", Message: "tax_rate must be greater than zero"})
	}
	if input.TaxType <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.tax_type", Message: "tax_type must be greater than zero"})
	}
	if input.SettlementCurrencyTypeID <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.settlement_currency_type_id", Message: "settlement_currency_type_id must be greater than zero"})
	}
	if input.InTaxRate <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.in_tax_rate", Message: "in_tax_rate must be greater than zero"})
	}
	if input.OutTaxRate <= 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.out_tax_rate", Message: "out_tax_rate must be greater than zero"})
	}
	if input.MonthSettleDays < 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.month_settle_days", Message: "month_settle_days must be zero or greater"})
	}
	if input.LatePayInterestRate < 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.late_pay_interest_rate", Message: "late_pay_interest_rate must be zero or greater"})
	}
	if input.InterestGraceDays < 0 {
		fields = append(fields, ValidationField{Field: "joint_operation.interest_grace_days", Message: "interest_grace_days must be zero or greater"})
	}
	return fields
}

func validateAdBoardDetailInput(index int, detail AdBoardDetailInput) []ValidationField {
	fieldPrefix := fmt.Sprintf("ad_boards[%d]", index)
	fields := make([]ValidationField, 0)
	if detail.AdBoardID <= 0 {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".ad_board_id", Message: "ad_board_id must be greater than zero"})
	}
	if !detail.StartDate.Before(detail.EndDate) {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".date_range", Message: "start_date must be before end_date"})
	}
	if detail.RentArea <= 0 {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".rent_area", Message: "rent_area must be greater than zero"})
	}
	if detail.Airtime <= 0 {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".airtime", Message: "airtime must be greater than zero"})
	}
	if detail.BetweenFrom < 0 || detail.BetweenTo < 0 || (detail.BetweenTo > 0 && detail.BetweenFrom > detail.BetweenTo) {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".between_range", Message: "between_from must not exceed between_to"})
	}
	switch detail.Frequency {
	case AdBoardFrequencyDay, AdBoardFrequencyMonth:
		if detail.FrequencyDays <= 0 {
			fields = append(fields, ValidationField{Field: fieldPrefix + ".frequency_days", Message: "frequency_days must be greater than zero"})
		}
	case AdBoardFrequencyWeek:
		if !detail.FrequencyMon && !detail.FrequencyTue && !detail.FrequencyWed && !detail.FrequencyThu && !detail.FrequencyFri && !detail.FrequencySat && !detail.FrequencySun {
			fields = append(fields, ValidationField{Field: fieldPrefix + ".frequency_weekdays", Message: "at least one weekday must be selected for weekly frequency"})
		}
	default:
		fields = append(fields, ValidationField{Field: fieldPrefix + ".frequency", Message: "frequency must be one of D, M, or W"})
	}
	return fields
}

func validateAreaGroundDetailInput(index int, detail AreaGroundDetailInput) []ValidationField {
	fieldPrefix := fmt.Sprintf("area_grounds[%d]", index)
	fields := make([]ValidationField, 0)
	if strings.TrimSpace(detail.Code) == "" {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".code", Message: "code is required"})
	}
	if strings.TrimSpace(detail.Name) == "" {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".name", Message: "name is required"})
	}
	if detail.TypeID <= 0 {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".type_id", Message: "type_id must be greater than zero"})
	}
	if !detail.StartDate.Before(detail.EndDate) {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".date_range", Message: "start_date must be before end_date"})
	}
	if detail.RentArea <= 0 {
		fields = append(fields, ValidationField{Field: fieldPrefix + ".rent_area", Message: "rent_area must be greater than zero"})
	}
	return fields
}

func normalizeSubtype(subtype ContractSubtype) ContractSubtype {
	if subtype == "" {
		return ContractSubtypeStandard
	}
	return subtype
}
