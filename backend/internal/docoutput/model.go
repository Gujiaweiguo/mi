package docoutput

import "time"

const (
	DateLayout = "2006-01-02"
)

type TemplateStatus string

const (
	TemplateStatusActive TemplateStatus = "active"
)

type OutputMode string

const (
	OutputModeInvoiceBatch  OutputMode = "invoice_batch"
	OutputModeInvoiceDetail OutputMode = "invoice_detail"
	OutputModeBillState     OutputMode = "bill_state"
	OutputModeLeaseContract OutputMode = "lease_contract"
)

type Template struct {
	ID           int64          `json:"id"`
	Code         string         `json:"code"`
	Name         string         `json:"name"`
	DocumentType string         `json:"document_type"`
	OutputMode   OutputMode     `json:"output_mode"`
	Status       TemplateStatus `json:"status"`
	Title        string         `json:"title"`
	Subtitle     string         `json:"subtitle,omitempty"`
	HeaderLines  []string       `json:"header_lines"`
	FooterLines  []string       `json:"footer_lines"`
	CreatedBy    int64          `json:"created_by"`
	UpdatedBy    int64          `json:"updated_by"`
	CreatedAt    time.Time      `json:"created_at"`
	UpdatedAt    time.Time      `json:"updated_at"`
}

type UpsertTemplateInput struct {
	Code         string
	Name         string
	DocumentType string
	OutputMode   OutputMode
	Title        string
	Subtitle     string
	HeaderLines  []string
	FooterLines  []string
	ActorUserID  int64
}

type ListFilter struct {
	Page     int
	PageSize int
}

type RenderInput struct {
	TemplateCode string
	DocumentIDs  []int64
	ActorUserID  int64
}

type Artifact struct {
	FileName    string `json:"file_name"`
	ContentType string `json:"content_type"`
	Body        []byte `json:"-"`
}
