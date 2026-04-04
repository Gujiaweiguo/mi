package taxexport

import "time"

const (
	DateLayout      = "2006-01-02"
	DefaultPage     = 1
	DefaultPageSize = 20
	MaxPageSize     = 100
)

type RuleSetStatus string

const (
	RuleSetStatusActive RuleSetStatus = "active"
)

type EntrySide string

const (
	EntrySideDebit  EntrySide = "debit"
	EntrySideCredit EntrySide = "credit"
)

type RuleSet struct {
	ID           int64         `json:"id"`
	Code         string        `json:"code"`
	Name         string        `json:"name"`
	DocumentType string        `json:"document_type"`
	Status       RuleSetStatus `json:"status"`
	CreatedBy    int64         `json:"created_by"`
	UpdatedBy    int64         `json:"updated_by"`
	CreatedAt    time.Time     `json:"created_at"`
	UpdatedAt    time.Time     `json:"updated_at"`
	Rules        []Rule        `json:"rules"`
}

type Rule struct {
	ID                  int64     `json:"id"`
	RuleSetID           int64     `json:"rule_set_id"`
	SequenceNo          int       `json:"sequence_no"`
	EntrySide           EntrySide `json:"entry_side"`
	ChargeTypeFilter    string    `json:"charge_type_filter"`
	AccountNumber       string    `json:"account_number"`
	AccountName         string    `json:"account_name"`
	ExplanationTemplate string    `json:"explanation_template"`
	UseTenantName       bool      `json:"use_tenant_name"`
	IsBalancingEntry    bool      `json:"is_balancing_entry"`
	CreatedAt           time.Time `json:"created_at"`
	UpdatedAt           time.Time `json:"updated_at"`
}

type UpsertRuleSetInput struct {
	Code         string
	Name         string
	DocumentType string
	Status       RuleSetStatus
	Rules        []UpsertRuleInput
	ActorUserID  int64
}

type UpsertRuleInput struct {
	SequenceNo          int
	EntrySide           EntrySide
	ChargeTypeFilter    string
	AccountNumber       string
	AccountName         string
	ExplanationTemplate string
	UseTenantName       bool
	IsBalancingEntry    bool
}

type ListFilter struct {
	Page     int
	PageSize int
}

type ListResult struct {
	Items    []RuleSet `json:"items"`
	Total    int64     `json:"total"`
	Page     int       `json:"page"`
	PageSize int       `json:"page_size"`
}

type ExportInput struct {
	RuleSetCode string
	FromDate    time.Time
	ToDate      time.Time
	ActorUserID int64
}

type ExportArtifact struct {
	FileName      string `json:"file_name"`
	ContentType   string `json:"content_type"`
	Bytes         []byte `json:"-"`
	DocumentCount int    `json:"document_count"`
	EntryCount    int    `json:"entry_count"`
}

type exportDocument struct {
	DocumentID     int64
	DocumentNo     string
	DocumentType   string
	TenantName     string
	ApprovedAt     time.Time
	PeriodStart    time.Time
	PeriodEnd      time.Time
	TotalAmount    float64
	CurrencyTypeID int64
	Lines          []exportLine
}

type exportLine struct {
	BillingChargeLineID int64
	ChargeType          string
	PeriodStart         time.Time
	PeriodEnd           time.Time
	QuantityDays        int
	UnitAmount          float64
	Amount              float64
}

type voucherEntry struct {
	Date          string
	Year          string
	Period        string
	GroupID       string
	Number        int
	AccountNum    string
	AccountName   string
	CurrencyNum   string
	CurrencyName  string
	AmountFor     float64
	Debit         float64
	Credit        float64
	PreparerID    string
	CheckerID     string
	ApproveID     string
	CashierID     string
	Handler       string
	SettleTypeID  string
	SettleNo      string
	Explanation   string
	Quantity      string
	MeasureUnitID string
	UnitPrice     string
	Reference     string
	TransDate     string
	TransNo       string
	Attachments   string
	SerialNum     string
	ObjectName    string
	Parameter     string
	ExchangeRate  string
	EntryID       int
	Item          string
	Posted        int
	InternalInd   string
	CashFlow      string
}
