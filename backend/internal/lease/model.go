package lease

import "time"

type Status string

const (
	StatusDraft           Status = "draft"
	StatusPendingApproval Status = "pending_approval"
	StatusActive          Status = "active"
	StatusRejected        Status = "rejected"
	StatusTerminated      Status = "terminated"
)

const (
	DocumentTypeContract   = "lease_contract"
	ApprovalDefinitionCode = "lease-approval"
	ChangeDefinitionCode   = "lease-change"
	DateLayout             = "2006-01-02"
)

type TermType string

const (
	TermTypeRent    TermType = "rent"
	TermTypeDeposit TermType = "deposit"
)

type BillingCycle string

const (
	BillingCycleMonthly BillingCycle = "monthly"
)

type ContractSubtype string

const (
	ContractSubtypeStandard       ContractSubtype = "standard"
	ContractSubtypeJointOperation ContractSubtype = "joint_operation"
	ContractSubtypeAdBoard        ContractSubtype = "ad_board"
	ContractSubtypeAreaGround     ContractSubtype = "area_ground"
)

type AdBoardFrequency string

const (
	AdBoardFrequencyDay   AdBoardFrequency = "D"
	AdBoardFrequencyMonth AdBoardFrequency = "M"
	AdBoardFrequencyWeek  AdBoardFrequency = "W"
)

type Contract struct {
	ID                 int64                 `json:"id"`
	AmendedFromID      *int64                `json:"amended_from_id,omitempty"`
	LeaseNo            string                `json:"lease_no"`
	Subtype            ContractSubtype       `json:"subtype"`
	DepartmentID       int64                 `json:"department_id"`
	StoreID            int64                 `json:"store_id"`
	BuildingID         *int64                `json:"building_id,omitempty"`
	CustomerID         *int64                `json:"customer_id,omitempty"`
	BrandID            *int64                `json:"brand_id,omitempty"`
	TradeID            *int64                `json:"trade_id,omitempty"`
	ManagementTypeID   *int64                `json:"management_type_id,omitempty"`
	TenantName         string                `json:"tenant_name"`
	StartDate          time.Time             `json:"start_date"`
	EndDate            time.Time             `json:"end_date"`
	Status             Status                `json:"status"`
	WorkflowInstanceID *int64                `json:"workflow_instance_id,omitempty"`
	EffectiveVersion   int                   `json:"effective_version"`
	SubmittedAt        *time.Time            `json:"submitted_at,omitempty"`
	ApprovedAt         *time.Time            `json:"approved_at,omitempty"`
	BillingEffectiveAt *time.Time            `json:"billing_effective_at,omitempty"`
	TerminatedAt       *time.Time            `json:"terminated_at,omitempty"`
	CreatedBy          int64                 `json:"created_by"`
	UpdatedBy          int64                 `json:"updated_by"`
	CreatedAt          time.Time             `json:"created_at"`
	UpdatedAt          time.Time             `json:"updated_at"`
	JointOperation     *JointOperationFields `json:"joint_operation,omitempty"`
	AdBoards           []AdBoardDetail       `json:"ad_boards,omitempty"`
	AreaGrounds        []AreaGroundDetail    `json:"area_grounds,omitempty"`
	Units              []Unit                `json:"units"`
	Terms              []Term                `json:"terms"`
}

type JointOperationFields struct {
	LeaseContractID          int64     `json:"lease_contract_id,omitempty"`
	BillCycle                int       `json:"bill_cycle"`
	RentInc                  string    `json:"rent_inc"`
	AccountCycle             int       `json:"account_cycle"`
	TaxRate                  float64   `json:"tax_rate"`
	TaxType                  int       `json:"tax_type"`
	SettlementCurrencyTypeID int64     `json:"settlement_currency_type_id"`
	InTaxRate                float64   `json:"in_tax_rate"`
	OutTaxRate               float64   `json:"out_tax_rate"`
	MonthSettleDays          float64   `json:"month_settle_days,omitempty"`
	LatePayInterestRate      float64   `json:"late_pay_interest_rate,omitempty"`
	InterestGraceDays        int       `json:"interest_grace_days,omitempty"`
	CreatedAt                time.Time `json:"created_at"`
	UpdatedAt                time.Time `json:"updated_at"`
}

type AdBoardDetail struct {
	ID              int64            `json:"id"`
	LeaseContractID int64            `json:"lease_contract_id,omitempty"`
	AdBoardID       int64            `json:"ad_board_id"`
	Description     string           `json:"description,omitempty"`
	Status          int              `json:"status"`
	StartDate       time.Time        `json:"start_date"`
	EndDate         time.Time        `json:"end_date"`
	RentArea        float64          `json:"rent_area"`
	Airtime         int              `json:"airtime"`
	Frequency       AdBoardFrequency `json:"frequency"`
	FrequencyDays   int              `json:"frequency_days,omitempty"`
	FrequencyMon    bool             `json:"frequency_mon,omitempty"`
	FrequencyTue    bool             `json:"frequency_tue,omitempty"`
	FrequencyWed    bool             `json:"frequency_wed,omitempty"`
	FrequencyThu    bool             `json:"frequency_thu,omitempty"`
	FrequencyFri    bool             `json:"frequency_fri,omitempty"`
	FrequencySat    bool             `json:"frequency_sat,omitempty"`
	FrequencySun    bool             `json:"frequency_sun,omitempty"`
	BetweenFrom     int              `json:"between_from,omitempty"`
	BetweenTo       int              `json:"between_to,omitempty"`
	StoreID         *int64           `json:"store_id,omitempty"`
	BuildingID      *int64           `json:"building_id,omitempty"`
	CreatedAt       time.Time        `json:"created_at"`
	UpdatedAt       time.Time        `json:"updated_at"`
}

type AreaGroundDetail struct {
	ID              int64     `json:"id"`
	LeaseContractID int64     `json:"lease_contract_id,omitempty"`
	Code            string    `json:"code"`
	Name            string    `json:"name"`
	TypeID          int64     `json:"type_id"`
	Description     string    `json:"description,omitempty"`
	Status          int       `json:"status"`
	StartDate       time.Time `json:"start_date"`
	EndDate         time.Time `json:"end_date"`
	RentArea        float64   `json:"rent_area"`
	CreatedAt       time.Time `json:"created_at"`
	UpdatedAt       time.Time `json:"updated_at"`
}

type Unit struct {
	ID              int64     `json:"id"`
	LeaseContractID int64     `json:"lease_contract_id"`
	UnitID          int64     `json:"unit_id"`
	RentArea        float64   `json:"rent_area"`
	CreatedAt       time.Time `json:"created_at"`
	UpdatedAt       time.Time `json:"updated_at"`
}

type Term struct {
	ID              int64        `json:"id"`
	LeaseContractID int64        `json:"lease_contract_id"`
	TermType        TermType     `json:"term_type"`
	BillingCycle    BillingCycle `json:"billing_cycle"`
	CurrencyTypeID  int64        `json:"currency_type_id"`
	Amount          float64      `json:"amount"`
	EffectiveFrom   time.Time    `json:"effective_from"`
	EffectiveTo     time.Time    `json:"effective_to"`
	CreatedAt       time.Time    `json:"created_at"`
	UpdatedAt       time.Time    `json:"updated_at"`
}

type Summary struct {
	ID                 int64           `json:"id"`
	LeaseNo            string          `json:"lease_no"`
	TenantName         string          `json:"tenant_name"`
	Subtype            ContractSubtype `json:"subtype"`
	DepartmentID       int64           `json:"department_id"`
	StoreID            int64           `json:"store_id"`
	BuildingID         *int64          `json:"building_id,omitempty"`
	CustomerID         *int64          `json:"customer_id,omitempty"`
	BrandID            *int64          `json:"brand_id,omitempty"`
	TradeID            *int64          `json:"trade_id,omitempty"`
	ManagementTypeID   *int64          `json:"management_type_id,omitempty"`
	StartDate          time.Time       `json:"start_date"`
	EndDate            time.Time       `json:"end_date"`
	Status             Status          `json:"status"`
	WorkflowInstanceID *int64          `json:"workflow_instance_id,omitempty"`
	BillingEffectiveAt *time.Time      `json:"billing_effective_at,omitempty"`
	UpdatedAt          time.Time       `json:"updated_at"`
}

type ListFilter struct {
	Status   *Status
	StoreID  *int64
	LeaseNo  string
	Page     int
	PageSize int
}

type CreateDraftInput struct {
	LeaseNo          string
	Subtype          ContractSubtype
	DepartmentID     int64
	StoreID          int64
	BuildingID       *int64
	CustomerID       *int64
	BrandID          *int64
	TradeID          *int64
	ManagementTypeID *int64
	TenantName       string
	StartDate        time.Time
	EndDate          time.Time
	JointOperation   *JointOperationFieldsInput
	AdBoards         []AdBoardDetailInput
	AreaGrounds      []AreaGroundDetailInput
	Units            []UnitInput
	Terms            []TermInput
	ActorUserID      int64
}

type JointOperationFieldsInput struct {
	BillCycle                int
	RentInc                  string
	AccountCycle             int
	TaxRate                  float64
	TaxType                  int
	SettlementCurrencyTypeID int64
	InTaxRate                float64
	OutTaxRate               float64
	MonthSettleDays          float64
	LatePayInterestRate      float64
	InterestGraceDays        int
}

type AdBoardDetailInput struct {
	AdBoardID     int64
	Description   string
	Status        int
	StartDate     time.Time
	EndDate       time.Time
	RentArea      float64
	Airtime       int
	Frequency     AdBoardFrequency
	FrequencyDays int
	FrequencyMon  bool
	FrequencyTue  bool
	FrequencyWed  bool
	FrequencyThu  bool
	FrequencyFri  bool
	FrequencySat  bool
	FrequencySun  bool
	BetweenFrom   int
	BetweenTo     int
	StoreID       *int64
	BuildingID    *int64
}

type AreaGroundDetailInput struct {
	Code        string
	Name        string
	TypeID      int64
	Description string
	Status      int
	StartDate   time.Time
	EndDate     time.Time
	RentArea    float64
}

type UnitInput struct {
	UnitID   int64
	RentArea float64
}

type TermInput struct {
	TermType       TermType
	BillingCycle   BillingCycle
	CurrencyTypeID int64
	Amount         float64
	EffectiveFrom  time.Time
	EffectiveTo    time.Time
}

type SubmitInput struct {
	LeaseID        int64
	ActorUserID    int64
	DepartmentID   int64
	IdempotencyKey string
	Comment        string
}

type AmendInput struct {
	LeaseID int64
	CreateDraftInput
}

type TerminateInput struct {
	LeaseID      int64
	ActorUserID  int64
	TerminatedAt time.Time
}

func (c Contract) BillingEligible() bool {
	return c.Status == StatusActive && c.BillingEffectiveAt != nil
}
