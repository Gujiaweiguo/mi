package invoice

import "time"

type DocumentType string

const (
	DocumentTypeBill       DocumentType = "bill"
	DocumentTypeInvoice    DocumentType = "invoice"
	ApprovalDefinitionCode              = "invoice-approval"
	DateLayout                          = "2006-01-02"
	DefaultPage                         = 1
	DefaultPageSize                     = 20
	MaxPageSize                         = 100
)

type Status string

const (
	StatusDraft           Status = "draft"
	StatusPendingApproval Status = "pending_approval"
	StatusApproved        Status = "approved"
	StatusRejected        Status = "rejected"
	StatusCancelled       Status = "cancelled"
	StatusAdjusted        Status = "adjusted"
)

type SettlementStatus string

const (
	SettlementStatusOutstanding SettlementStatus = "outstanding"
	SettlementStatusSettled     SettlementStatus = "settled"
)

type Document struct {
	ID                 int64        `json:"id"`
	DocumentType       DocumentType `json:"document_type"`
	DocumentNo         *string      `json:"document_no,omitempty"`
	BillingRunID       int64        `json:"billing_run_id"`
	LeaseContractID    int64        `json:"lease_contract_id"`
	TenantName         string       `json:"tenant_name"`
	PeriodStart        time.Time    `json:"period_start"`
	PeriodEnd          time.Time    `json:"period_end"`
	TotalAmount        float64      `json:"total_amount"`
	CurrencyTypeID     int64        `json:"currency_type_id"`
	Status             Status       `json:"status"`
	WorkflowInstanceID *int64       `json:"workflow_instance_id,omitempty"`
	AdjustedFromID     *int64       `json:"adjusted_from_id,omitempty"`
	SubmittedAt        *time.Time   `json:"submitted_at,omitempty"`
	ApprovedAt         *time.Time   `json:"approved_at,omitempty"`
	CancelledAt        *time.Time   `json:"cancelled_at,omitempty"`
	CreatedBy          int64        `json:"created_by"`
	UpdatedBy          int64        `json:"updated_by"`
	CreatedAt          time.Time    `json:"created_at"`
	UpdatedAt          time.Time    `json:"updated_at"`
	Lines              []Line       `json:"lines"`
}

type OpenItem struct {
	ID                    int64      `json:"id"`
	LeaseContractID       int64      `json:"lease_contract_id"`
	BillingDocumentID     int64      `json:"billing_document_id"`
	BillingDocumentLineID *int64     `json:"billing_document_line_id,omitempty"`
	CustomerID            int64      `json:"customer_id"`
	DepartmentID          int64      `json:"department_id"`
	TradeID               *int64     `json:"trade_id,omitempty"`
	ChargeType            string     `json:"charge_type"`
	DueDate               time.Time  `json:"due_date"`
	OutstandingAmount     float64    `json:"outstanding_amount"`
	SettledAt             *time.Time `json:"settled_at,omitempty"`
	IsDeposit             bool       `json:"is_deposit"`
	CreatedAt             time.Time  `json:"created_at"`
	UpdatedAt             time.Time  `json:"updated_at"`
}

type PaymentEntry struct {
	ID                int64     `json:"id"`
	BillingDocumentID int64     `json:"billing_document_id"`
	LeaseContractID   int64     `json:"lease_contract_id"`
	PaymentDate       time.Time `json:"payment_date"`
	Amount            float64   `json:"amount"`
	Note              *string   `json:"note,omitempty"`
	RecordedBy        int64     `json:"recorded_by"`
	IdempotencyKey    string    `json:"idempotency_key"`
	CreatedAt         time.Time `json:"created_at"`
}

type ReceivableSummary struct {
	BillingDocumentID int64            `json:"billing_document_id"`
	DocumentNo        *string          `json:"document_no,omitempty"`
	DocumentType      DocumentType     `json:"document_type"`
	TenantName        string           `json:"tenant_name"`
	LeaseContractID   int64            `json:"lease_contract_id"`
	OutstandingAmount float64          `json:"outstanding_amount"`
	SettlementStatus  SettlementStatus `json:"settlement_status"`
	Items             []OpenItem       `json:"items"`
	PaymentHistory    []PaymentEntry   `json:"payment_history"`
}

type ReceivableListItem struct {
	BillingDocumentID int64            `json:"billing_document_id"`
	DocumentType      DocumentType     `json:"document_type"`
	DocumentNo        *string          `json:"document_no,omitempty"`
	TenantName        string           `json:"tenant_name"`
	DocumentStatus    Status           `json:"document_status"`
	LeaseContractID   int64            `json:"lease_contract_id"`
	CustomerID        int64            `json:"customer_id"`
	DepartmentID      int64            `json:"department_id"`
	TradeID           *int64           `json:"trade_id,omitempty"`
	EarliestDueDate   time.Time        `json:"earliest_due_date"`
	LatestDueDate     time.Time        `json:"latest_due_date"`
	OutstandingAmount float64          `json:"outstanding_amount"`
	SettlementStatus  SettlementStatus `json:"settlement_status"`
}

type Line struct {
	ID                  int64     `json:"id"`
	BillingDocumentID   int64     `json:"billing_document_id"`
	BillingChargeLineID int64     `json:"billing_charge_line_id"`
	ChargeType          string    `json:"charge_type"`
	PeriodStart         time.Time `json:"period_start"`
	PeriodEnd           time.Time `json:"period_end"`
	QuantityDays        int       `json:"quantity_days"`
	UnitAmount          float64   `json:"unit_amount"`
	Amount              float64   `json:"amount"`
	CreatedAt           time.Time `json:"created_at"`
}

type CreateInput struct {
	DocumentType         DocumentType
	BillingChargeLineIDs []int64
	ActorUserID          int64
}

type SubmitInput struct {
	DocumentID     int64
	ActorUserID    int64
	DepartmentID   int64
	IdempotencyKey string
	Comment        string
}

type CancelInput struct {
	DocumentID  int64
	ActorUserID int64
}

type AdjustLineInput struct {
	BillingChargeLineID int64
	Amount              float64
}

type AdjustInput struct {
	DocumentID  int64
	ActorUserID int64
	Lines       []AdjustLineInput
}

type RecordPaymentInput struct {
	DocumentID     int64
	ActorUserID    int64
	Amount         float64
	PaymentDate    time.Time
	IdempotencyKey string
	Note           string
}

type ReceivableFilter struct {
	CustomerID   *int64
	DepartmentID *int64
	DueDateStart *time.Time
	DueDateEnd   *time.Time
	Page         int
	PageSize     int
}

type ReceivableListResult struct {
	Items    []ReceivableListItem `json:"items"`
	Total    int64                `json:"total"`
	Page     int                  `json:"page"`
	PageSize int                  `json:"page_size"`
}

type ListFilter struct {
	DocumentType    *DocumentType
	Status          *Status
	LeaseContractID *int64
	BillingRunID    *int64
	Page            int
	PageSize        int
}

type ListResult struct {
	Items    []Document `json:"items"`
	Total    int64      `json:"total"`
	Page     int        `json:"page"`
	PageSize int        `json:"page_size"`
}
