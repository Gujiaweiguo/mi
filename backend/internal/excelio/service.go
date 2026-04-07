package excelio

import (
	"context"
	"errors"
	"fmt"
	"io"
	"sort"
	"strconv"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/sales"
	"github.com/xuri/excelize/v2"
)

var (
	ErrInvalidDataset = errors.New("invalid excel dataset")
	ErrInvalidImport  = errors.New("invalid excel import")
)

type SalesImporter interface {
	BatchUpsertDailySales(ctx context.Context, inputs []sales.BatchDailySaleInput) (int, error)
	BatchUpsertTraffic(ctx context.Context, inputs []sales.BatchTrafficInput) (int, error)
}

type Service struct {
	repository    *Repository
	salesImporter SalesImporter
}

func NewService(repository *Repository, salesImporter SalesImporter) *Service {
	return &Service{repository: repository, salesImporter: salesImporter}
}

func (s *Service) DownloadUnitTemplate(ctx context.Context) (*TemplateArtifact, error) {
	refs, err := s.repository.LoadUnitReference(ctx)
	if err != nil {
		return nil, err
	}
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	unitSheet := UnitSheetName
	_ = f.SetSheetName(f.GetSheetName(0), unitSheet)
	headers := []string{"code", "building_code", "floor_code", "location_code", "area_code", "unit_type_code", "floor_area", "use_area", "rent_area", "is_rentable", "status"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(unitSheet, cell, header)
	}
	refSheet := RefSheetName
	_, _ = f.NewSheet(refSheet)
	writeReferenceSection(f, refSheet, 1, "buildings", refs.Buildings)
	writeReferenceSection(f, refSheet, 5, "floors", refs.Floors)
	writeReferenceSection(f, refSheet, 9, "locations", refs.Locations)
	writeReferenceSection(f, refSheet, 13, "areas", refs.Areas)
	writeReferenceSection(f, refSheet, 17, "unit_types", refs.UnitTypes)
	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write unit template workbook: %w", err)
	}
	return &TemplateArtifact{FileName: "unit-data-template.xlsx", ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Body: buffer.Bytes()}, nil
}

func (s *Service) ImportUnits(ctx context.Context, reader io.Reader) (*ImportResult, error) {
	f, err := excelize.OpenReader(reader)
	if err != nil {
		return nil, fmt.Errorf("open unit import workbook: %w", err)
	}
	defer func() { _ = f.Close() }()
	rows, err := f.GetRows(UnitSheetName)
	if err != nil {
		return nil, fmt.Errorf("read unit import sheet: %w", err)
	}
	parsedRows, diagnostics := parseUnitRows(rows)
	refs, err := s.repository.LoadUnitReference(ctx)
	if err != nil {
		return nil, err
	}
	diagnostics = append(diagnostics, validateUnitRows(parsedRows, refs)...)
	sort.Slice(diagnostics, func(i, j int) bool {
		if diagnostics[i].Row == diagnostics[j].Row {
			return diagnostics[i].Field < diagnostics[j].Field
		}
		return diagnostics[i].Row < diagnostics[j].Row
	})
	if len(diagnostics) > 0 {
		return &ImportResult{ImportedCount: 0, Diagnostics: diagnostics}, ErrInvalidImport
	}
	if err := s.repository.UpsertUnits(ctx, parsedRows, refs); err != nil {
		return nil, err
	}
	return &ImportResult{ImportedCount: len(parsedRows), Diagnostics: []Diagnostic{}}, nil
}

func (s *Service) DownloadDailySalesTemplate(ctx context.Context) (*TemplateArtifact, error) {
	refs, err := s.repository.LoadSalesReference(ctx)
	if err != nil {
		return nil, err
	}
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	_ = f.SetSheetName(f.GetSheetName(0), DailySalesSheetName)
	headers := []string{"store_code", "unit_code", "sale_date", "sales_amount"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(DailySalesSheetName, cell, header)
	}
	_, _ = f.NewSheet(RefSheetName)
	writeReferenceSection(f, RefSheetName, 1, "stores", refs.Stores)
	writeReferenceSection(f, RefSheetName, 5, "units", refs.Units)
	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write daily sales template workbook: %w", err)
	}
	return &TemplateArtifact{FileName: "daily-sales-template.xlsx", ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Body: buffer.Bytes()}, nil
}

func (s *Service) DownloadTrafficTemplate(ctx context.Context) (*TemplateArtifact, error) {
	refs, err := s.repository.LoadSalesReference(ctx)
	if err != nil {
		return nil, err
	}
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	_ = f.SetSheetName(f.GetSheetName(0), TrafficSheetName)
	headers := []string{"store_code", "traffic_date", "inbound_count"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(TrafficSheetName, cell, header)
	}
	_, _ = f.NewSheet(RefSheetName)
	writeReferenceSection(f, RefSheetName, 1, "stores", refs.Stores)
	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write traffic template workbook: %w", err)
	}
	return &TemplateArtifact{FileName: "customer-traffic-template.xlsx", ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Body: buffer.Bytes()}, nil
}

func (s *Service) ImportDailySales(ctx context.Context, reader io.Reader) (*ImportResult, error) {
	if s.salesImporter == nil {
		return nil, fmt.Errorf("sales importer is not configured")
	}
	f, err := excelize.OpenReader(reader)
	if err != nil {
		return nil, fmt.Errorf("open daily sales import workbook: %w", err)
	}
	defer func() { _ = f.Close() }()
	rows, err := f.GetRows(DailySalesSheetName)
	if err != nil {
		return nil, fmt.Errorf("read daily sales import sheet: %w", err)
	}
	parsedRows, diagnostics := parseDailySaleRows(rows)
	refs, err := s.repository.LoadSalesReference(ctx)
	if err != nil {
		return nil, err
	}
	diagnostics = append(diagnostics, validateDailySaleRows(parsedRows, refs)...)
	sortDiagnostics(diagnostics)
	if len(diagnostics) > 0 {
		return &ImportResult{ImportedCount: 0, Diagnostics: diagnostics}, ErrInvalidImport
	}
	inputs := buildDailySaleInputs(parsedRows, refs)
	count, err := s.salesImporter.BatchUpsertDailySales(ctx, inputs)
	if err != nil {
		return nil, err
	}
	return &ImportResult{ImportedCount: count, Diagnostics: []Diagnostic{}}, nil
}

func (s *Service) ImportTraffic(ctx context.Context, reader io.Reader) (*ImportResult, error) {
	if s.salesImporter == nil {
		return nil, fmt.Errorf("sales importer is not configured")
	}
	f, err := excelize.OpenReader(reader)
	if err != nil {
		return nil, fmt.Errorf("open traffic import workbook: %w", err)
	}
	defer func() { _ = f.Close() }()
	rows, err := f.GetRows(TrafficSheetName)
	if err != nil {
		return nil, fmt.Errorf("read traffic import sheet: %w", err)
	}
	parsedRows, diagnostics := parseTrafficRows(rows)
	refs, err := s.repository.LoadSalesReference(ctx)
	if err != nil {
		return nil, err
	}
	diagnostics = append(diagnostics, validateTrafficRows(parsedRows, refs)...)
	sortDiagnostics(diagnostics)
	if len(diagnostics) > 0 {
		return &ImportResult{ImportedCount: 0, Diagnostics: diagnostics}, ErrInvalidImport
	}
	inputs := buildTrafficInputs(parsedRows, refs)
	count, err := s.salesImporter.BatchUpsertTraffic(ctx, inputs)
	if err != nil {
		return nil, err
	}
	return &ImportResult{ImportedCount: count, Diagnostics: []Diagnostic{}}, nil
}

func (s *Service) ExportOperationalDataset(ctx context.Context, input ExportInput) (*ExportArtifact, error) {
	switch strings.TrimSpace(input.Dataset) {
	case "invoices":
		return s.exportInvoices(ctx)
	case "billing_charges":
		return s.exportBillingCharges(ctx)
	default:
		return nil, ErrInvalidDataset
	}
}

func (s *Service) exportInvoices(ctx context.Context) (*ExportArtifact, error) {
	rows, err := s.repository.ListInvoiceExportRows(ctx)
	if err != nil {
		return nil, err
	}
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	sheet := f.GetSheetName(0)
	headers := []string{"document_no", "document_type", "tenant_name", "status", "period_start", "period_end", "total_amount"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(sheet, cell, header)
	}
	for rowIndex, row := range rows {
		values := []any{row.DocumentNo, row.DocumentType, row.TenantName, row.Status, row.PeriodStart.Format(DateLayout), row.PeriodEnd.Format(DateLayout), row.TotalAmount}
		for colIndex, value := range values {
			cell, _ := excelize.CoordinatesToCellName(colIndex+1, rowIndex+2)
			_ = f.SetCellValue(sheet, cell, value)
		}
	}
	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write invoice export workbook: %w", err)
	}
	return &ExportArtifact{FileName: "operational-invoices.xlsx", ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Body: buffer.Bytes()}, nil
}

func (s *Service) exportBillingCharges(ctx context.Context) (*ExportArtifact, error) {
	rows, err := s.repository.ListChargeExportRows(ctx)
	if err != nil {
		return nil, err
	}
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	sheet := f.GetSheetName(0)
	headers := []string{"lease_no", "tenant_name", "charge_type", "period_start", "period_end", "quantity_days", "unit_amount", "amount"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(sheet, cell, header)
	}
	for rowIndex, row := range rows {
		values := []any{row.LeaseNo, row.TenantName, row.ChargeType, row.PeriodStart.Format(DateLayout), row.PeriodEnd.Format(DateLayout), row.QuantityDays, row.UnitAmount, row.Amount}
		for colIndex, value := range values {
			cell, _ := excelize.CoordinatesToCellName(colIndex+1, rowIndex+2)
			_ = f.SetCellValue(sheet, cell, value)
		}
	}
	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write charge export workbook: %w", err)
	}
	return &ExportArtifact{FileName: "operational-billing-charges.xlsx", ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Body: buffer.Bytes()}, nil
}

func writeReferenceSection(f *excelize.File, sheet string, startColumn int, title string, items []ReferenceItem) {
	titleCell, _ := excelize.CoordinatesToCellName(startColumn, 1)
	_ = f.SetCellValue(sheet, titleCell, title)
	codeCell, _ := excelize.CoordinatesToCellName(startColumn, 2)
	nameCell, _ := excelize.CoordinatesToCellName(startColumn+1, 2)
	_ = f.SetCellValue(sheet, codeCell, "code")
	_ = f.SetCellValue(sheet, nameCell, "name")
	for index, item := range items {
		codeRow, _ := excelize.CoordinatesToCellName(startColumn, index+3)
		nameRow, _ := excelize.CoordinatesToCellName(startColumn+1, index+3)
		_ = f.SetCellValue(sheet, codeRow, item.Code)
		_ = f.SetCellValue(sheet, nameRow, item.Name)
	}
}

func parseUnitRows(rows [][]string) ([]UnitImportRow, []Diagnostic) {
	if len(rows) == 0 {
		return nil, []Diagnostic{{Row: 1, Field: "sheet", Message: "Units sheet is empty"}}
	}
	parsed := make([]UnitImportRow, 0)
	diagnostics := make([]Diagnostic, 0)
	for rowIndex, row := range rows[1:] {
		excelRow := rowIndex + 2
		if isEmptyRow(row) {
			continue
		}
		mapped := cellValue(row, 0)
		parsedRow := UnitImportRow{Code: mapped, BuildingCode: cellValue(row, 1), FloorCode: cellValue(row, 2), LocationCode: cellValue(row, 3), AreaCode: cellValue(row, 4), UnitTypeCode: cellValue(row, 5), Status: cellValue(row, 10)}
		var err error
		if parsedRow.FloorArea, err = parsePositiveFloat(cellValue(row, 6)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "floor_area", Message: err.Error()})
		}
		if parsedRow.UseArea, err = parsePositiveFloat(cellValue(row, 7)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "use_area", Message: err.Error()})
		}
		if parsedRow.RentArea, err = parsePositiveFloat(cellValue(row, 8)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "rent_area", Message: err.Error()})
		}
		if parsedRow.IsRentable, err = parseBool(cellValue(row, 9)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "is_rentable", Message: err.Error()})
		}
		parsed = append(parsed, parsedRow)
	}
	return parsed, diagnostics
}

func parseDailySaleRows(rows [][]string) ([]DailySaleImportRow, []Diagnostic) {
	if len(rows) == 0 {
		return nil, []Diagnostic{{Row: 1, Field: "sheet", Message: "DailySales sheet is empty"}}
	}
	parsed := make([]DailySaleImportRow, 0)
	diagnostics := make([]Diagnostic, 0)
	for rowIndex, row := range rows[1:] {
		excelRow := rowIndex + 2
		if isEmptyRow(row) {
			continue
		}
		parsedRow := DailySaleImportRow{StoreCode: cellValue(row, 0), UnitCode: cellValue(row, 1)}
		var err error
		if parsedRow.SaleDate, err = parseDate(cellValue(row, 2)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "sale_date", Message: err.Error()})
		}
		if parsedRow.SalesAmount, err = parsePositiveFloat(cellValue(row, 3)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "sales_amount", Message: err.Error()})
		}
		parsed = append(parsed, parsedRow)
	}
	return parsed, diagnostics
}

func parseTrafficRows(rows [][]string) ([]TrafficImportRow, []Diagnostic) {
	if len(rows) == 0 {
		return nil, []Diagnostic{{Row: 1, Field: "sheet", Message: "CustomerTraffic sheet is empty"}}
	}
	parsed := make([]TrafficImportRow, 0)
	diagnostics := make([]Diagnostic, 0)
	for rowIndex, row := range rows[1:] {
		excelRow := rowIndex + 2
		if isEmptyRow(row) {
			continue
		}
		parsedRow := TrafficImportRow{StoreCode: cellValue(row, 0)}
		var err error
		if parsedRow.TrafficDate, err = parseDate(cellValue(row, 1)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "traffic_date", Message: err.Error()})
		}
		if parsedRow.InboundCount, err = parsePositiveInt(cellValue(row, 2)); err != nil {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "inbound_count", Message: err.Error()})
		}
		parsed = append(parsed, parsedRow)
	}
	return parsed, diagnostics
}

func validateUnitRows(rows []UnitImportRow, refs *UnitReference) []Diagnostic {
	diagnostics := make([]Diagnostic, 0)
	seenCodes := make(map[string]int)
	buildingByCode := referenceByCode(refs.Buildings)
	floorByCode := referenceByCode(refs.Floors)
	locationByCode := referenceByCode(refs.Locations)
	areaByCode := referenceByCode(refs.Areas)
	unitTypeByCode := referenceByCode(refs.UnitTypes)
	for index, row := range rows {
		excelRow := index + 2
		if row.Code == "" {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "code", Message: "code is required"})
		}
		if row.BuildingCode == "" || buildingByCode[row.BuildingCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "building_code", Message: "unknown building_code"})
		}
		if row.FloorCode == "" || floorByCode[row.FloorCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "floor_code", Message: "unknown floor_code"})
		}
		if row.LocationCode == "" || locationByCode[row.LocationCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "location_code", Message: "unknown location_code"})
		}
		if row.AreaCode == "" || areaByCode[row.AreaCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "area_code", Message: "unknown area_code"})
		}
		if row.UnitTypeCode == "" || unitTypeByCode[row.UnitTypeCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "unit_type_code", Message: "unknown unit_type_code"})
		}
		if row.Status == "" {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "status", Message: "status is required"})
		}
		if previousRow, ok := seenCodes[row.Code]; ok && row.Code != "" {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "code", Message: fmt.Sprintf("duplicate code also appears on row %d", previousRow)})
		} else if row.Code != "" {
			seenCodes[row.Code] = excelRow
		}
	}
	return diagnostics
}

func validateDailySaleRows(rows []DailySaleImportRow, refs *SalesReference) []Diagnostic {
	diagnostics := make([]Diagnostic, 0)
	storeByCode := referenceByCode(refs.Stores)
	unitByCode := referenceByCode(refs.Units)
	seenKeys := make(map[string]int)
	for index, row := range rows {
		excelRow := index + 2
		if row.StoreCode == "" || storeByCode[row.StoreCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "store_code", Message: "unknown store_code"})
		}
		if row.UnitCode == "" || unitByCode[row.UnitCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "unit_code", Message: "unknown unit_code"})
		}
		if row.SaleDate.IsZero() {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "sale_date", Message: "sale_date is required"})
		}
		key := fmt.Sprintf("%s|%s|%s", row.StoreCode, row.UnitCode, row.SaleDate.Format(DateLayout))
		if previousRow, ok := seenKeys[key]; ok && row.StoreCode != "" && row.UnitCode != "" && !row.SaleDate.IsZero() {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "store_code", Message: fmt.Sprintf("duplicate business key also appears on row %d", previousRow)})
		} else if row.StoreCode != "" && row.UnitCode != "" && !row.SaleDate.IsZero() {
			seenKeys[key] = excelRow
		}
	}
	return diagnostics
}

func validateTrafficRows(rows []TrafficImportRow, refs *SalesReference) []Diagnostic {
	diagnostics := make([]Diagnostic, 0)
	storeByCode := referenceByCode(refs.Stores)
	seenKeys := make(map[string]int)
	for index, row := range rows {
		excelRow := index + 2
		if row.StoreCode == "" || storeByCode[row.StoreCode].ID == 0 {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "store_code", Message: "unknown store_code"})
		}
		if row.TrafficDate.IsZero() {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "traffic_date", Message: "traffic_date is required"})
		}
		key := fmt.Sprintf("%s|%s", row.StoreCode, row.TrafficDate.Format(DateLayout))
		if previousRow, ok := seenKeys[key]; ok && row.StoreCode != "" && !row.TrafficDate.IsZero() {
			diagnostics = append(diagnostics, Diagnostic{Row: excelRow, Field: "store_code", Message: fmt.Sprintf("duplicate business key also appears on row %d", previousRow)})
		} else if row.StoreCode != "" && !row.TrafficDate.IsZero() {
			seenKeys[key] = excelRow
		}
	}
	return diagnostics
}

func buildDailySaleInputs(rows []DailySaleImportRow, refs *SalesReference) []sales.BatchDailySaleInput {
	storeByCode := referenceByCode(refs.Stores)
	unitByCode := referenceByCode(refs.Units)
	inputs := make([]sales.BatchDailySaleInput, 0, len(rows))
	for _, row := range rows {
		inputs = append(inputs, sales.BatchDailySaleInput{StoreID: storeByCode[row.StoreCode].ID, UnitID: unitByCode[row.UnitCode].ID, SaleDate: row.SaleDate, SalesAmount: row.SalesAmount})
	}
	return inputs
}

func buildTrafficInputs(rows []TrafficImportRow, refs *SalesReference) []sales.BatchTrafficInput {
	storeByCode := referenceByCode(refs.Stores)
	inputs := make([]sales.BatchTrafficInput, 0, len(rows))
	for _, row := range rows {
		inputs = append(inputs, sales.BatchTrafficInput{StoreID: storeByCode[row.StoreCode].ID, TrafficDate: row.TrafficDate, InboundCount: row.InboundCount})
	}
	return inputs
}

func sortDiagnostics(diagnostics []Diagnostic) {
	sort.Slice(diagnostics, func(i, j int) bool {
		if diagnostics[i].Row == diagnostics[j].Row {
			return diagnostics[i].Field < diagnostics[j].Field
		}
		return diagnostics[i].Row < diagnostics[j].Row
	})
}

func cellValue(row []string, index int) string {
	if index >= len(row) {
		return ""
	}
	return strings.TrimSpace(row[index])
}

func isEmptyRow(row []string) bool {
	for _, cell := range row {
		if strings.TrimSpace(cell) != "" {
			return false
		}
	}
	return true
}

func parsePositiveFloat(value string) (float64, error) {
	v, err := strconv.ParseFloat(strings.TrimSpace(value), 64)
	if err != nil || v <= 0 {
		return 0, errors.New("must be a positive number")
	}
	return v, nil
}

func parsePositiveInt(value string) (int, error) {
	v, err := strconv.Atoi(strings.TrimSpace(value))
	if err != nil || v <= 0 {
		return 0, errors.New("must be a positive integer")
	}
	return v, nil
}

func parseDate(value string) (time.Time, error) {
	trimmed := strings.TrimSpace(value)
	if trimmed == "" {
		return time.Time{}, errors.New("must be a valid date")
	}
	parsed, err := time.Parse(DateLayout, trimmed)
	if err != nil {
		return time.Time{}, errors.New("must be a valid date")
	}
	return parsed, nil
}

func parseBool(value string) (bool, error) {
	switch strings.ToLower(strings.TrimSpace(value)) {
	case "true", "yes", "1":
		return true, nil
	case "false", "no", "0":
		return false, nil
	default:
		return false, errors.New("must be true/false")
	}
}
