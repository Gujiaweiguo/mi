package excelio

import "time"

const (
	DateLayout          = "2006-01-02"
	DefaultPage         = 1
	DefaultPageSize     = 20
	MaxPageSize         = 100
	UnitSheetName       = "Units"
	DailySalesSheetName = "DailySales"
	TrafficSheetName    = "CustomerTraffic"
	RefSheetName        = "ReferenceData"
)

type TemplateArtifact struct {
	FileName    string `json:"file_name"`
	ContentType string `json:"content_type"`
	Body        []byte `json:"-"`
}

type ExportInput struct {
	Dataset string
}

type ExportArtifact struct {
	FileName    string `json:"file_name"`
	ContentType string `json:"content_type"`
	Body        []byte `json:"-"`
}

type ImportResult struct {
	ImportedCount int          `json:"imported_count"`
	Diagnostics   []Diagnostic `json:"diagnostics"`
}

type Diagnostic struct {
	Row     int    `json:"row"`
	Field   string `json:"field"`
	Message string `json:"message"`
}

type UnitImportRow struct {
	Code         string
	BuildingCode string
	FloorCode    string
	LocationCode string
	AreaCode     string
	UnitTypeCode string
	FloorArea    float64
	UseArea      float64
	RentArea     float64
	IsRentable   bool
	Status       string
}

type UnitReference struct {
	Buildings []ReferenceItem
	Floors    []ReferenceItem
	Locations []ReferenceItem
	Areas     []ReferenceItem
	UnitTypes []ReferenceItem
}

type DailySaleImportRow struct {
	StoreCode   string
	UnitCode    string
	SaleDate    time.Time
	SalesAmount float64
}

type TrafficImportRow struct {
	StoreCode    string
	TrafficDate  time.Time
	InboundCount int
}

type SalesReference struct {
	Stores []ReferenceItem
	Units  []ReferenceItem
}

type ReferenceItem struct {
	ID   int64
	Code string
	Name string
}

type exportInvoiceRow struct {
	DocumentNo   string
	DocumentType string
	TenantName   string
	Status       string
	PeriodStart  time.Time
	PeriodEnd    time.Time
	TotalAmount  float64
}

type exportChargeRow struct {
	LeaseNo      string
	TenantName   string
	ChargeType   string
	PeriodStart  time.Time
	PeriodEnd    time.Time
	QuantityDays int
	UnitAmount   float64
	Amount       float64
}

type exportLeaseContractRow struct {
	LeaseNo          string
	TenantName       string
	StoreCode        string
	DepartmentCode   string
	StartDate        time.Time
	EndDate          time.Time
	Status           string
	EffectiveVersion int
}

type exportUnitDataRow struct {
	Code         string
	BuildingCode string
	FloorCode    string
	LocationCode string
	AreaCode     string
	UnitTypeCode string
	FloorArea    float64
	UseArea      float64
	RentArea     float64
	IsRentable   bool
	Status       string
}
