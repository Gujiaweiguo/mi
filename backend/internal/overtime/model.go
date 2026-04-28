package overtime

import (
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
)

type BillStatus string

const (
	BillStatusDraft           BillStatus = "draft"
	BillStatusPendingApproval BillStatus = "pending_approval"
	BillStatusApproved        BillStatus = "approved"
	BillStatusRejected        BillStatus = "rejected"
	BillStatusCancelled       BillStatus = "cancelled"
	BillStatusStopped         BillStatus = "stopped"
	BillStatusGenerated       BillStatus = "generated"

	DocumentTypeBill       = "overtime_bill"
	ApprovalDefinitionCode = "overtime-approval"
	DateLayout             = "2006-01-02"
)

type FormulaType string

const (
	FormulaTypeFixed      FormulaType = "fixed"
	FormulaTypeOneTime    FormulaType = "one_time"
	FormulaTypePercentage FormulaType = "percentage"
)

type RateType string

const (
	RateTypeDaily   RateType = "daily"
	RateTypeMonthly RateType = "monthly"
)

type Bill struct {
	ID                 int64             `json:"id"`
	LeaseContractID    int64             `json:"lease_contract_id"`
	LeaseNo            string            `json:"lease_no"`
	TenantName         string            `json:"tenant_name"`
	PeriodStart        time.Time         `json:"period_start"`
	PeriodEnd          time.Time         `json:"period_end"`
	Status             BillStatus        `json:"status"`
	WorkflowInstanceID *int64            `json:"workflow_instance_id,omitempty"`
	Note               string            `json:"note,omitempty"`
	SubmittedAt        *time.Time        `json:"submitted_at,omitempty"`
	ApprovedAt         *time.Time        `json:"approved_at,omitempty"`
	RejectedAt         *time.Time        `json:"rejected_at,omitempty"`
	CancelledAt        *time.Time        `json:"cancelled_at,omitempty"`
	StoppedAt          *time.Time        `json:"stopped_at,omitempty"`
	GeneratedAt        *time.Time        `json:"generated_at,omitempty"`
	CreatedBy          int64             `json:"created_by"`
	UpdatedBy          int64             `json:"updated_by"`
	CreatedAt          time.Time         `json:"created_at"`
	UpdatedAt          time.Time         `json:"updated_at"`
	Formulas           []Formula         `json:"formulas"`
	GeneratedCharges   []GeneratedCharge `json:"generated_charges,omitempty"`
}

type Formula struct {
	ID                int64          `json:"id"`
	OvertimeBillID    int64          `json:"overtime_bill_id"`
	ChargeType        string         `json:"charge_type"`
	FormulaType       FormulaType    `json:"formula_type"`
	RateType          RateType       `json:"rate_type"`
	EffectiveFrom     time.Time      `json:"effective_from"`
	EffectiveTo       time.Time      `json:"effective_to"`
	CurrencyTypeID    int64          `json:"currency_type_id"`
	TotalArea         float64        `json:"total_area"`
	UnitPrice         float64        `json:"unit_price"`
	BaseAmount        float64        `json:"base_amount"`
	FixedRental       float64        `json:"fixed_rental"`
	PercentageOption  string         `json:"percentage_option,omitempty"`
	MinimumOption     string         `json:"minimum_option,omitempty"`
	SortOrder         int            `json:"sort_order"`
	CreatedAt         time.Time      `json:"created_at"`
	UpdatedAt         time.Time      `json:"updated_at"`
	PercentageTiers   []PercentTier  `json:"percentage_tiers,omitempty"`
	MinimumTiers      []MinimumTier  `json:"minimum_tiers,omitempty"`
}

type PercentTier struct {
	ID         int64     `json:"id"`
	FormulaID  int64     `json:"formula_id"`
	SalesTo    float64   `json:"sales_to"`
	Percentage float64   `json:"percentage"`
	SortOrder  int       `json:"sort_order"`
	CreatedAt  time.Time `json:"created_at"`
}

type MinimumTier struct {
	ID         int64     `json:"id"`
	FormulaID  int64     `json:"formula_id"`
	SalesTo    float64   `json:"sales_to"`
	MinimumSum float64   `json:"minimum_sum"`
	SortOrder  int       `json:"sort_order"`
	CreatedAt  time.Time `json:"created_at"`
}

type GeneratedCharge struct {
	ID                    int64      `json:"id"`
	BillingRunID          int64      `json:"billing_run_id"`
	OvertimeBillID        int64      `json:"overtime_bill_id"`
	OvertimeFormulaID     int64      `json:"overtime_formula_id"`
	LeaseContractID       int64      `json:"lease_contract_id"`
	WorkflowInstanceID    *int64     `json:"workflow_instance_id,omitempty"`
	ChargeType            string     `json:"charge_type"`
	FormulaType           FormulaType `json:"formula_type"`
	RateType              RateType   `json:"rate_type"`
	PeriodStart           time.Time  `json:"period_start"`
	PeriodEnd             time.Time  `json:"period_end"`
	Quantity              int        `json:"quantity"`
	TotalArea             float64    `json:"total_area"`
	UnitPrice             float64    `json:"unit_price"`
	BaseAmount            float64    `json:"base_amount"`
	FixedRental           float64    `json:"fixed_rental"`
	PercentageOption      string     `json:"percentage_option,omitempty"`
	MinimumOption         string     `json:"minimum_option,omitempty"`
	AppliedPercentageRate float64    `json:"applied_percentage_rate,omitempty"`
	AppliedMinimumAmount  float64    `json:"applied_minimum_amount,omitempty"`
	UnitAmount            float64    `json:"unit_amount"`
	Amount                float64    `json:"amount"`
	CurrencyTypeID        int64      `json:"currency_type_id"`
	GeneratedBy           int64      `json:"generated_by"`
	CreatedAt             time.Time  `json:"created_at"`
}

type CreateBillInput struct {
	LeaseContractID int64
	PeriodStart     time.Time
	PeriodEnd       time.Time
	Note            string
	ActorUserID     int64
	Formulas        []FormulaInput
}

type FormulaInput struct {
	ChargeType       string
	FormulaType      FormulaType
	RateType         RateType
	EffectiveFrom    time.Time
	EffectiveTo      time.Time
	CurrencyTypeID   int64
	TotalArea        float64
	UnitPrice        float64
	BaseAmount       float64
	FixedRental      float64
	PercentageOption string
	MinimumOption    string
	PercentageTiers  []PercentTierInput
	MinimumTiers     []MinimumTierInput
}

type PercentTierInput struct {
	SalesTo    float64
	Percentage float64
}

type MinimumTierInput struct {
	SalesTo    float64
	MinimumSum float64
}

type SubmitInput struct {
	BillID          int64
	ActorUserID     int64
	DepartmentID    int64
	IdempotencyKey  string
	Comment         string
}

type GenerateInput struct {
	BillID      int64
	ActorUserID int64
}

type CancelInput struct {
	BillID      int64
	ActorUserID int64
}

type StopInput struct {
	BillID      int64
	ActorUserID int64
}

type ListFilter struct {
	LeaseContractID *int64
	Status          *BillStatus
	PeriodStart     *time.Time
	PeriodEnd       *time.Time
	Page            int
	PageSize        int
}

type GenerateResult struct {
	Run     *billing.Run        `json:"run"`
	Charges []GeneratedCharge   `json:"charges"`
	Skipped []SkippedGeneration `json:"skipped"`
	Totals  Totals              `json:"totals"`
}

type SkippedGeneration struct {
	FormulaID int64  `json:"formula_id"`
	Reason    string `json:"reason"`
}

type Totals struct {
	Generated int `json:"generated"`
	Skipped   int `json:"skipped"`
}
