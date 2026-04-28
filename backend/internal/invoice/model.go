package invoice

import (
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
)

type DocumentType string

const (
	DocumentTypeBill       DocumentType = "bill"
	DocumentTypeInvoice    DocumentType = "invoice"
	DocumentTypeDiscount   DocumentType = "invoice_discount"
	ChargeTypeLateInterest              = "late_payment_interest"
	ApprovalDefinitionCode              = "invoice-approval"
	DateLayout                          = "2006-01-02"
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

type DiscountStatus string

const (
	DiscountStatusDraft           DiscountStatus = "draft"
	DiscountStatusPendingApproval DiscountStatus = "pending_approval"
	DiscountStatusApproved        DiscountStatus = "approved"
	DiscountStatusRejected        DiscountStatus = "rejected"
)

type SurplusEntryType string

const (
	SurplusEntryTypeOverpayment SurplusEntryType = "overpayment"
	SurplusEntryTypeApplication SurplusEntryType = "application"
)

type InterestConfigStatus string

const (
	InterestConfigStatusActive InterestConfigStatus = "active"
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
	ChargeSource          billing.ChargeSource `json:"charge_source"`
	OvertimeBillID        *int64     `json:"overtime_bill_id,omitempty"`
	OvertimeFormulaID     *int64     `json:"overtime_formula_id,omitempty"`
	OvertimeChargeID      *int64     `json:"overtime_charge_id,omitempty"`
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

type Discount struct {
	ID                    int64          `json:"id"`
	BillingDocumentID     int64          `json:"billing_document_id"`
	BillingDocumentLineID int64          `json:"billing_document_line_id"`
	LeaseContractID       int64          `json:"lease_contract_id"`
	ChargeType            string         `json:"charge_type"`
	RequestedAmount       float64        `json:"requested_amount"`
	RequestedRate         float64        `json:"requested_rate"`
	Reason                string         `json:"reason"`
	Status                DiscountStatus `json:"status"`
	WorkflowInstanceID    *int64         `json:"workflow_instance_id,omitempty"`
	IdempotencyKey        string         `json:"idempotency_key"`
	SubmittedAt           *time.Time     `json:"submitted_at,omitempty"`
	ApprovedAt            *time.Time     `json:"approved_at,omitempty"`
	RejectedAt            *time.Time     `json:"rejected_at,omitempty"`
	CreatedBy             int64          `json:"created_by"`
	UpdatedBy             int64          `json:"updated_by"`
	CreatedAt             time.Time      `json:"created_at"`
	UpdatedAt             time.Time      `json:"updated_at"`
}

type DiscountEntry struct {
	ID                    int64     `json:"id"`
	InvoiceDiscountID     int64     `json:"invoice_discount_id"`
	BillingDocumentID     int64     `json:"billing_document_id"`
	BillingDocumentLineID int64     `json:"billing_document_line_id"`
	AROpenItemID          int64     `json:"ar_open_item_id"`
	LeaseContractID       int64     `json:"lease_contract_id"`
	Amount                float64   `json:"amount"`
	RecordedBy            int64     `json:"recorded_by"`
	CreatedAt             time.Time `json:"created_at"`
}

type SurplusBalance struct {
	ID              int64      `json:"id"`
	CustomerID      int64      `json:"customer_id"`
	AvailableAmount float64    `json:"available_amount"`
	CreatedBy       int64      `json:"created_by"`
	UpdatedBy       int64      `json:"updated_by"`
	CreatedAt       time.Time  `json:"created_at"`
	UpdatedAt       time.Time  `json:"updated_at"`
	LastAppliedAt   *time.Time `json:"last_applied_at,omitempty"`
}

type SurplusEntry struct {
	ID                int64            `json:"id"`
	SurplusBalanceID  int64            `json:"surplus_balance_id"`
	EntryType         SurplusEntryType `json:"entry_type"`
	CustomerID        int64            `json:"customer_id"`
	BillingDocumentID *int64           `json:"billing_document_id,omitempty"`
	AROpenItemID      *int64           `json:"ar_open_item_id,omitempty"`
	Amount            float64          `json:"amount"`
	Note              *string          `json:"note,omitempty"`
	IdempotencyKey    string           `json:"idempotency_key"`
	RecordedBy        int64            `json:"recorded_by"`
	CreatedAt         time.Time        `json:"created_at"`
}

type InterestRateConfig struct {
	ID               int64                `json:"id"`
	ChargeTypeFilter *string              `json:"charge_type_filter,omitempty"`
	DailyRate        float64              `json:"daily_rate"`
	GraceDays        int                  `json:"grace_days"`
	IsDefault        bool                 `json:"is_default"`
	Status           InterestConfigStatus `json:"status"`
	CreatedBy        int64                `json:"created_by"`
	UpdatedBy        int64                `json:"updated_by"`
	CreatedAt        time.Time            `json:"created_at"`
	UpdatedAt        time.Time            `json:"updated_at"`
}

type InterestEntry struct {
	ID                      int64     `json:"id"`
	SourceAROpenItemID      int64     `json:"source_ar_open_item_id"`
	SourceBillingDocumentID int64     `json:"source_billing_document_id"`
	SourceBillingLineID     int64     `json:"source_billing_document_line_id"`
	GeneratedDocumentID     int64     `json:"generated_billing_document_id"`
	GeneratedLineID         int64     `json:"generated_billing_document_line_id"`
	ChargeType              string    `json:"charge_type"`
	PrincipalAmount         float64   `json:"principal_amount"`
	DailyRate               float64   `json:"daily_rate"`
	GraceDays               int       `json:"grace_days"`
	CoveredStartDate        time.Time `json:"covered_start_date"`
	CoveredEndDate          time.Time `json:"covered_end_date"`
	InterestDays            int       `json:"interest_days"`
	InterestAmount          float64   `json:"interest_amount"`
	IdempotencyKey          string    `json:"idempotency_key"`
	CreatedBy               int64     `json:"created_by"`
	CreatedAt               time.Time `json:"created_at"`
}

type DepositApplication struct {
	ID                          int64     `json:"id"`
	SourceBillingDocumentID     int64     `json:"source_billing_document_id"`
	SourceBillingDocumentLineID int64     `json:"source_billing_document_line_id"`
	SourceAROpenItemID          int64     `json:"source_ar_open_item_id"`
	TargetBillingDocumentID     int64     `json:"target_billing_document_id"`
	TargetBillingDocumentLineID int64     `json:"target_billing_document_line_id"`
	TargetAROpenItemID          int64     `json:"target_ar_open_item_id"`
	LeaseContractID             int64     `json:"lease_contract_id"`
	Amount                      float64   `json:"amount"`
	Note                        *string   `json:"note,omitempty"`
	IdempotencyKey              string    `json:"idempotency_key"`
	AppliedBy                   int64     `json:"applied_by"`
	CreatedAt                   time.Time `json:"created_at"`
}

type DepositRefund struct {
	ID                    int64     `json:"id"`
	BillingDocumentID     int64     `json:"billing_document_id"`
	BillingDocumentLineID int64     `json:"billing_document_line_id"`
	AROpenItemID          int64     `json:"ar_open_item_id"`
	LeaseContractID       int64     `json:"lease_contract_id"`
	Amount                float64   `json:"amount"`
	Reason                string    `json:"reason"`
	IdempotencyKey        string    `json:"idempotency_key"`
	RefundedBy            int64     `json:"refunded_by"`
	CreatedAt             time.Time `json:"created_at"`
}

type ReceivableSummary struct {
	BillingDocumentID         int64                `json:"billing_document_id"`
	DocumentNo                *string              `json:"document_no,omitempty"`
	DocumentType              DocumentType         `json:"document_type"`
	TenantName                string               `json:"tenant_name"`
	LeaseContractID           int64                `json:"lease_contract_id"`
	OutstandingAmount         float64              `json:"outstanding_amount"`
	CustomerSurplus           float64              `json:"customer_surplus_available"`
	SettlementStatus          SettlementStatus     `json:"settlement_status"`
	Items                     []OpenItem           `json:"items"`
	PaymentHistory            []PaymentEntry       `json:"payment_history"`
	DiscountHistory           []Discount           `json:"discount_history"`
	SurplusHistory            []SurplusEntry       `json:"surplus_history"`
	InterestHistory           []InterestEntry      `json:"interest_history"`
	DepositApplicationHistory []DepositApplication `json:"deposit_application_history"`
	DepositRefundHistory      []DepositRefund      `json:"deposit_refund_history"`
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
	ChargeSource        billing.ChargeSource `json:"charge_source"`
	OvertimeBillID      *int64    `json:"overtime_bill_id,omitempty"`
	OvertimeFormulaID   *int64    `json:"overtime_formula_id,omitempty"`
	OvertimeChargeID    *int64    `json:"overtime_charge_id,omitempty"`
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

type ApplyDiscountInput struct {
	DocumentID            int64
	BillingDocumentLineID int64
	Amount                float64
	Reason                string
	ActorUserID           int64
	DepartmentID          int64
	IdempotencyKey        string
}

type ApplySurplusInput struct {
	DocumentID            int64
	BillingDocumentLineID int64
	Amount                float64
	Note                  string
	ActorUserID           int64
	IdempotencyKey        string
}

type GenerateInterestInput struct {
	DocumentID            int64
	BillingDocumentLineID int64
	AsOfDate              time.Time
	ActorUserID           int64
	DepartmentID          int64
	IdempotencyKey        string
}

type ApplyDepositInput struct {
	DocumentID                  int64
	BillingDocumentLineID       int64
	TargetDocumentID            int64
	TargetBillingDocumentLineID int64
	Amount                      float64
	Note                        string
	ActorUserID                 int64
	IdempotencyKey              string
}

type RefundDepositInput struct {
	DocumentID            int64
	BillingDocumentLineID int64
	Amount                float64
	Reason                string
	ActorUserID           int64
	IdempotencyKey        string
}

type ReceivableFilter struct {
	CustomerID   *int64
	DepartmentID *int64
	DueDateStart *time.Time
	DueDateEnd   *time.Time
	Page         int
	PageSize     int
}

type ListFilter struct {
	DocumentType    *DocumentType
	Status          *Status
	LeaseContractID *int64
	BillingRunID    *int64
	Page            int
	PageSize        int
}
