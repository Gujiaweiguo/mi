package reporting

import (
	"fmt"
	"time"
)

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
	ReportR19 ReportID = "r19"
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

func (r R01Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "department_name": r.DepartmentName, "rent_status": r.RentStatus, "use_area_total": r.UseAreaTotal}
}

func (r R02Row) ToMap() map[string]any {
	return map[string]any{"lease_no": r.LeaseNo, "customer_code": r.CustomerCode, "customer_name": r.CustomerName, "trade_name": r.TradeName, "management_type_name": r.ManagementTypeName, "unit_code": r.UnitCode, "unit_name": r.UnitName, "rent_area": r.RentArea, "brand_name": r.BrandName, "shop_type_name": r.ShopTypeName, "department_name": r.DepartmentName, "store_name": r.StoreName}
}

func (r R03Row) ToMap() map[string]any {
	return map[string]any{"shop_type_name": r.ShopTypeName, "rent_area": r.RentArea, "current_sales": r.CurrentSales, "same_period_sales": r.SamePeriodSales, "comparable_sales": r.ComparableSales, "monthly_rent": r.MonthlyRent}
}

func (r R04Row) ToMap() map[string]any {
	row := map[string]any{"unit_code": r.UnitCode, "unit_name": r.UnitName, "rent_area": r.RentArea, "shop_type": r.ShopType, "total_sales": r.TotalSales}
	for day := 1; day <= 31; day++ {
		row[fmt.Sprintf("day_%02d", day)] = r.DayValue(day)
	}
	return row
}

func (r R05Row) ToMap() map[string]any {
	return map[string]any{"unit_code": r.UnitCode, "floor_area": r.FloorArea, "budget_unit_price": r.BudgetUnitPrice, "current_lease_price": r.CurrentLeasePrice, "potential_customer": r.PotentialCustomer, "prospect_brand": r.ProspectBrand, "prospect_trade": r.ProspectTrade, "average_ticket": r.AverageTicket, "prospect_rent_price": r.ProspectRentPrice, "rent_increment": r.RentIncrement, "prospect_term_months": r.ProspectTermMonths}
}

func (r R06Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "period": r.Period, "period_receivable": r.PeriodReceivable, "period_received": r.PeriodReceived, "monthly_budget": r.MonthlyBudget, "annual_budget": r.AnnualBudget, "ytd_cumulative": r.YTDCumulative}
}

func (r R07Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "brand_name": r.BrandName, "annual_total": r.AnnualTotal, "month_01": r.Month01, "month_02": r.Month02, "month_03": r.Month03, "month_04": r.Month04, "month_05": r.Month05, "month_06": r.Month06, "month_07": r.Month07, "month_08": r.Month08, "month_09": r.Month09, "month_10": r.Month10, "month_11": r.Month11, "month_12": r.Month12}
}

func (b AgingBuckets) ToMap() map[string]any {
	return map[string]any{
		"within_one_month":      b.WithinOneMonth,
		"one_to_two_months":     b.OneToTwoMonths,
		"two_to_three_months":   b.TwoToThreeMonths,
		"three_to_six_months":   b.ThreeToSixMonths,
		"six_to_nine_months":    b.SixToNineMonths,
		"nine_to_twelve_months": b.NineToTwelveMonths,
		"one_to_two_years":      b.OneToTwoYears,
		"two_to_three_years":    b.TwoToThreeYears,
		"over_three_years":      b.OverThreeYears,
		"total":                 b.Total,
	}
}

func (r R08Row) ToMap() map[string]any {
	row := r.AgingBuckets.ToMap()
	row["unit_collection"] = r.UnitCollection
	row["customer_name"] = r.CustomerName
	row["trade_name"] = r.TradeName
	row["department_name"] = r.DepartmentName
	row["lease_no"] = r.LeaseNo
	row["deposit_amount"] = r.DepositAmount
	return row
}

func (r R09Row) ToMap() map[string]any {
	row := r.AgingBuckets.ToMap()
	row["unit_collection"] = r.UnitCollection
	row["customer_name"] = r.CustomerName
	row["trade_name"] = r.TradeName
	row["department_name"] = r.DepartmentName
	row["lease_no"] = r.LeaseNo
	row["deposit_amount"] = r.DepositAmount
	row["charge_type"] = r.ChargeType
	return row
}

func (r R10Row) ToMap() map[string]any {
	row := map[string]any{"store_name": r.StoreName, "year": r.Year, "annual_total": r.MonthlyTotal}
	for month := 1; month <= 12; month++ {
		row[fmt.Sprintf("month_%02d", month)] = r.MonthValue(month)
	}
	return row
}

func (r R11Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "period": r.Period, "leased_area": r.LeasedArea, "total_area": r.TotalArea}
}

func (r R12Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "period": r.Period, "shop_type_name": r.ShopTypeName, "occupancy_status": r.OccupancyStatus, "area_total": r.AreaTotal}
}

func (r R13Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "shop_type_name": r.ShopTypeName, "period": r.Period, "current_sales": r.CurrentSales, "ytd_sales": r.YTDSales, "prev_month_sales": r.PrevMonthSales, "last_year_ytd_sales": r.LastYearYTDSales}
}

func (r R14Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "shop_type_name": r.ShopTypeName, "period": r.Period, "sales_amount": r.SalesAmount, "area_total": r.AreaTotal, "days_in_month": r.DaysInMonth, "efficiency": r.Efficiency}
}

func (r R15Row) ToMap() map[string]any {
	return map[string]any{"store_name": r.StoreName, "shop_type_name": r.ShopTypeName, "period": r.Period, "sales_amount": r.SalesAmount, "rent_income": r.RentIncome}
}

func (r R16Row) ToMap() map[string]any {
	row := r.AgingBuckets.ToMap()
	row["department_name"] = r.DepartmentName
	row["deposit_amount"] = r.DepositAmount
	return row
}

func (r R17Row) ToMap() map[string]any {
	row := r.AgingBuckets.ToMap()
	row["department_name"] = r.DepartmentName
	row["deposit_amount"] = r.DepositAmount
	row["charge_type"] = r.ChargeType
	return row
}

func (r R18Row) ToMap() map[string]any {
	return map[string]any{"customer_name": r.CustomerName, "store_name": r.StoreName, "unit_name": r.UnitName, "brand_name": r.BrandName, "period": r.Period, "rent_area": r.RentArea, "current_sales": r.CurrentSales, "comparable_sales": r.ComparableSales, "same_period_sales": r.SamePeriodSales, "period_receivable": r.PeriodReceivable, "period_received": r.PeriodReceived, "period_arrears": r.PeriodArrears, "cumulative_receivable": r.CumulativeReceivable, "cumulative_arrears": r.CumulativeArrears, "days_in_month": r.DaysInMonth, "efficiency": r.Efficiency}
}

func (u R19Unit) ToMap() map[string]any {
	return map[string]any{"unit_code": u.UnitCode, "unit_name": u.UnitName, "floor_area": u.FloorArea, "rent_area": u.RentArea, "rent_status": u.RentStatus, "brand_name": u.BrandName, "customer_name": u.CustomerName, "shop_type_name": u.ShopTypeName, "pos_x": u.PosX, "pos_y": u.PosY, "color_hex": u.ColorHex}
}
