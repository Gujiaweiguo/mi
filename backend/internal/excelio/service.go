package excelio

import (
	"context"
	"errors"
	"fmt"
	"io"
	"sort"
	"strconv"
	"strings"

	"github.com/xuri/excelize/v2"
)

var (
	ErrInvalidDataset = errors.New("invalid excel dataset")
	ErrInvalidImport  = errors.New("invalid excel import")
)

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

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
