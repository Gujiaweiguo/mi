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
	DefaultPage            = 1
	DefaultPageSize        = 20
	MaxPageSize            = 100
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

type Contract struct {
	ID                 int64      `json:"id"`
	AmendedFromID      *int64     `json:"amended_from_id,omitempty"`
	LeaseNo            string     `json:"lease_no"`
	DepartmentID       int64      `json:"department_id"`
	StoreID            int64      `json:"store_id"`
	BuildingID         *int64     `json:"building_id,omitempty"`
	CustomerID         *int64     `json:"customer_id,omitempty"`
	BrandID            *int64     `json:"brand_id,omitempty"`
	TradeID            *int64     `json:"trade_id,omitempty"`
	ManagementTypeID   *int64     `json:"management_type_id,omitempty"`
	TenantName         string     `json:"tenant_name"`
	StartDate          time.Time  `json:"start_date"`
	EndDate            time.Time  `json:"end_date"`
	Status             Status     `json:"status"`
	WorkflowInstanceID *int64     `json:"workflow_instance_id,omitempty"`
	EffectiveVersion   int        `json:"effective_version"`
	SubmittedAt        *time.Time `json:"submitted_at,omitempty"`
	ApprovedAt         *time.Time `json:"approved_at,omitempty"`
	BillingEffectiveAt *time.Time `json:"billing_effective_at,omitempty"`
	TerminatedAt       *time.Time `json:"terminated_at,omitempty"`
	CreatedBy          int64      `json:"created_by"`
	UpdatedBy          int64      `json:"updated_by"`
	CreatedAt          time.Time  `json:"created_at"`
	UpdatedAt          time.Time  `json:"updated_at"`
	Units              []Unit     `json:"units"`
	Terms              []Term     `json:"terms"`
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
	ID                 int64      `json:"id"`
	LeaseNo            string     `json:"lease_no"`
	TenantName         string     `json:"tenant_name"`
	DepartmentID       int64      `json:"department_id"`
	StoreID            int64      `json:"store_id"`
	BuildingID         *int64     `json:"building_id,omitempty"`
	CustomerID         *int64     `json:"customer_id,omitempty"`
	BrandID            *int64     `json:"brand_id,omitempty"`
	TradeID            *int64     `json:"trade_id,omitempty"`
	ManagementTypeID   *int64     `json:"management_type_id,omitempty"`
	StartDate          time.Time  `json:"start_date"`
	EndDate            time.Time  `json:"end_date"`
	Status             Status     `json:"status"`
	WorkflowInstanceID *int64     `json:"workflow_instance_id,omitempty"`
	BillingEffectiveAt *time.Time `json:"billing_effective_at,omitempty"`
	UpdatedAt          time.Time  `json:"updated_at"`
}

type ListFilter struct {
	Status   *Status
	StoreID  *int64
	LeaseNo  string
	Page     int
	PageSize int
}

type ListResult struct {
	Items    []Summary `json:"items"`
	Total    int64     `json:"total"`
	Page     int       `json:"page"`
	PageSize int       `json:"page_size"`
}

type CreateDraftInput struct {
	LeaseNo          string
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
	Units            []UnitInput
	Terms            []TermInput
	ActorUserID      int64
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
