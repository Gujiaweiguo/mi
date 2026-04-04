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
