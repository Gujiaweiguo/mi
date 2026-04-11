package reporting

import "time"

const (
	PeriodLayout = "2006-01"
)

type ReportID string

const (
	ReportR02 ReportID = "r02"
	ReportR01 ReportID = "r01"
	ReportR03 ReportID = "r03"
	ReportR04 ReportID = "r04"
	ReportR05 ReportID = "r05"
	ReportR06 ReportID = "r06"
	ReportR07 ReportID = "r07"
	ReportR08 ReportID = "r08"
	ReportR09 ReportID = "r09"
	ReportR10 ReportID = "r10"
	ReportR11 ReportID = "r11"
	ReportR12 ReportID = "r12"
	ReportR13 ReportID = "r13"
	ReportR14 ReportID = "r14"
	ReportR15 ReportID = "r15"
	ReportR16 ReportID = "r16"
	ReportR17 ReportID = "r17"
	ReportR18 ReportID = "r18"
)

type QueryInput struct {
	ReportID         ReportID
	PeriodStart      time.Time
	PeriodEnd        time.Time
	PeriodLabel      string
	StoreID          *int64
	FloorID          *int64
	AreaID           *int64
	UnitID           *int64
	DepartmentID     *int64
	ShopTypeID       *int64
	CustomerID       *int64
	BrandID          *int64
	TradeID          *int64
	ChargeType       *string
	ManagementTypeID *int64
	Status           *string
	RequestedByID    int64
}

type Column struct {
	Key   string `json:"key"`
	Label string `json:"label"`
}

type Result struct {
	ReportID    ReportID         `json:"report_id"`
	Columns     []Column         `json:"columns"`
	Rows        []map[string]any `json:"rows"`
	Visual      *R19Result       `json:"visual,omitempty"`
	GeneratedAt time.Time        `json:"generated_at"`
}

type ExportArtifact struct {
	FileName    string
	ContentType string
	Bytes       []byte
}

type ReportAuditAction string

const (
	ReportAuditActionQuery  ReportAuditAction = "query"
	ReportAuditActionExport ReportAuditAction = "export"
)

type R01Row struct {
	StoreName      string
	DepartmentName string
	RentStatus     string
	UseAreaTotal   float64
}

type R02Row struct {
	LeaseNo            string
	CustomerCode       string
	CustomerName       string
	TradeName          string
	ManagementTypeName string
	UnitCode           string
	UnitName           string
	RentArea           float64
	BrandName          string
	ShopTypeName       string
	DepartmentName     string
	StoreName          string
}

type R11Row struct {
	StoreName  string
	Period     string
	LeasedArea float64
	TotalArea  float64
}

type R03Row struct {
	ShopTypeName    string
	RentArea        float64
	CurrentSales    float64
	SamePeriodSales float64
	ComparableSales float64
	MonthlyRent     float64
}

type R04Row struct {
	UnitCode   string
	UnitName   string
	RentArea   float64
	ShopType   string
	TotalSales float64
	Day01      float64
	Day02      float64
	Day03      float64
	Day04      float64
	Day05      float64
	Day06      float64
	Day07      float64
	Day08      float64
	Day09      float64
	Day10      float64
	Day11      float64
	Day12      float64
	Day13      float64
	Day14      float64
	Day15      float64
	Day16      float64
	Day17      float64
	Day18      float64
	Day19      float64
	Day20      float64
	Day21      float64
	Day22      float64
	Day23      float64
	Day24      float64
	Day25      float64
	Day26      float64
	Day27      float64
	Day28      float64
	Day29      float64
	Day30      float64
	Day31      float64
}

type R07Row struct {
	StoreName   string
	BrandName   string
	AnnualTotal float64
	Month01     float64
	Month02     float64
	Month03     float64
	Month04     float64
	Month05     float64
	Month06     float64
	Month07     float64
	Month08     float64
	Month09     float64
	Month10     float64
	Month11     float64
	Month12     float64
}

type R05Row struct {
	UnitCode           string
	FloorArea          float64
	BudgetUnitPrice    *float64
	CurrentLeasePrice  *float64
	PotentialCustomer  string
	ProspectBrand      string
	ProspectTrade      string
	AverageTicket      *float64
	ProspectRentPrice  *float64
	RentIncrement      string
	ProspectTermMonths *int
}

type R06Row struct {
	StoreName        string
	Period           string
	PeriodReceivable float64
	PeriodReceived   float64
	MonthlyBudget    float64
	AnnualBudget     float64
	YTDCumulative    float64
}

type AgingBuckets struct {
	WithinOneMonth     float64
	OneToTwoMonths     float64
	TwoToThreeMonths   float64
	ThreeToSixMonths   float64
	SixToNineMonths    float64
	NineToTwelveMonths float64
	OneToTwoYears      float64
	TwoToThreeYears    float64
	OverThreeYears     float64
	Total              float64
}

type R08Row struct {
	UnitCollection string
	CustomerName   string
	TradeName      string
	DepartmentName string
	LeaseNo        string
	DepositAmount  float64
	AgingBuckets
}

type R09Row struct {
	UnitCollection string
	CustomerName   string
	TradeName      string
	DepartmentName string
	LeaseNo        string
	DepositAmount  float64
	ChargeType     string
	AgingBuckets
}

type R10Row struct {
	StoreName    string
	Year         int
	MonthlyTotal int
	Month01      int
	Month02      int
	Month03      int
	Month04      int
	Month05      int
	Month06      int
	Month07      int
	Month08      int
	Month09      int
	Month10      int
	Month11      int
	Month12      int
}

type R13Row struct {
	StoreName        string
	ShopTypeName     string
	Period           string
	CurrentSales     float64
	YTDSales         float64
	PrevMonthSales   float64
	LastYearYTDSales float64
}

type R14Row struct {
	StoreName    string
	ShopTypeName string
	Period       string
	SalesAmount  float64
	AreaTotal    float64
	DaysInMonth  int
	Efficiency   *float64
}

type R15Row struct {
	StoreName    string
	ShopTypeName string
	Period       string
	SalesAmount  float64
	RentIncome   float64
}

type R16Row struct {
	DepartmentName string
	DepositAmount  float64
	AgingBuckets
}

type R17Row struct {
	DepartmentName string
	ChargeType     string
	DepositAmount  float64
	AgingBuckets
}

type R18Row struct {
	CustomerName         string
	StoreName            string
	UnitName             string
	BrandName            string
	Period               string
	RentArea             float64
	CurrentSales         float64
	ComparableSales      float64
	SamePeriodSales      float64
	PeriodReceivable     float64
	PeriodReceived       float64
	PeriodArrears        float64
	CumulativeReceivable float64
	CumulativeArrears    float64
	DaysInMonth          int
	Efficiency           *float64
}

type R19Floor struct {
	ID                int64   `json:"id"`
	Name              string  `json:"name"`
	FloorPlanImageURL *string `json:"floor_plan_image_url,omitempty"`
}

type R19Unit struct {
	UnitID       int64   `json:"unit_id"`
	UnitCode     string  `json:"unit_code"`
	UnitName     string  `json:"unit_name"`
	FloorArea    float64 `json:"floor_area"`
	RentArea     float64 `json:"rent_area"`
	RentStatus   string  `json:"rent_status"`
	BrandName    string  `json:"brand_name"`
	CustomerName string  `json:"customer_name"`
	ShopTypeName string  `json:"shop_type_name"`
	PosX         int     `json:"pos_x"`
	PosY         int     `json:"pos_y"`
	ColorHex     string  `json:"color_hex"`
}

type R19LegendEntry struct {
	Label    string `json:"label"`
	ColorHex string `json:"color_hex"`
}

type R19Result struct {
	Floor  R19Floor         `json:"floor"`
	Units  []R19Unit        `json:"units"`
	Legend []R19LegendEntry `json:"legend"`
}

type R12Row struct {
	StoreName       string
	Period          string
	ShopTypeName    string
	OccupancyStatus string
	AreaTotal       float64
}
