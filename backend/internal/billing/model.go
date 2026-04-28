package billing

import "time"

type RunStatus string

type ChargeSource string

const (
	RunStatusCompleted  RunStatus    = "completed"
	ChargeTypeRent      string       = "rent"
	ChargeSourceStandard ChargeSource = "standard"
	ChargeSourceOvertime ChargeSource = "overtime"
	DateLayout          string       = "2006-01-02"
)

type Run struct {
	ID             int64     `json:"id"`
	PeriodStart    time.Time `json:"period_start"`
	PeriodEnd      time.Time `json:"period_end"`
	Status         RunStatus `json:"status"`
	TriggeredBy    int64     `json:"triggered_by"`
	GeneratedCount int       `json:"generated_count"`
	SkippedCount   int       `json:"skipped_count"`
	CreatedAt      time.Time `json:"created_at"`
	UpdatedAt      time.Time `json:"updated_at"`
}

type ChargeLine struct {
	ID                     int64     `json:"id"`
	BillingRunID           int64     `json:"billing_run_id"`
	LeaseContractID        int64     `json:"lease_contract_id"`
	LeaseNo                string    `json:"lease_no"`
	TenantName             string    `json:"tenant_name"`
	LeaseTermID            *int64       `json:"lease_term_id,omitempty"`
	ChargeType             string       `json:"charge_type"`
	ChargeSource           ChargeSource `json:"charge_source"`
	OvertimeBillID         *int64       `json:"overtime_bill_id,omitempty"`
	OvertimeFormulaID      *int64       `json:"overtime_formula_id,omitempty"`
	OvertimeChargeID       *int64       `json:"overtime_charge_id,omitempty"`
	PeriodStart            time.Time `json:"period_start"`
	PeriodEnd              time.Time `json:"period_end"`
	QuantityDays           int       `json:"quantity_days"`
	UnitAmount             float64   `json:"unit_amount"`
	Amount                 float64   `json:"amount"`
	CurrencyTypeID         int64     `json:"currency_type_id"`
	SourceEffectiveVersion int       `json:"source_effective_version"`
	CreatedAt              time.Time `json:"created_at"`
}

type GenerateInput struct {
	PeriodStart time.Time
	PeriodEnd   time.Time
	ActorUserID int64
}

type GenerateResult struct {
	Run    *Run         `json:"run"`
	Lines  []ChargeLine `json:"lines"`
	Totals Totals       `json:"totals"`
}

type Totals struct {
	Generated int `json:"generated"`
	Skipped   int `json:"skipped"`
}

type ChargeListFilter struct {
	LeaseContractID *int64
	PeriodStart     *time.Time
	PeriodEnd       *time.Time
	Page            int
	PageSize        int
}

type chargeCandidate struct {
	LeaseContractID    int64
	LeaseNo            string
	TenantName         string
	LeaseStatus        string
	LeaseStartDate     time.Time
	LeaseEndDate       time.Time
	BillingEffectiveAt time.Time
	TerminatedAt       *time.Time
	EffectiveVersion   int
	LeaseTermID        int64
	TermType           string
	BillingCycle       string
	CurrencyTypeID     int64
	UnitAmount         float64
	TermEffectiveFrom  time.Time
	TermEffectiveTo    time.Time
}
