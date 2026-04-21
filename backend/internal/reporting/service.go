package reporting

import (
	"context"
	"errors"
	"fmt"
	"strings"
	"time"

	"github.com/xuri/excelize/v2"
)

var (
	ErrUnsupportedReport = errors.New("unsupported generalize report")
	ErrInvalidPeriod     = errors.New("invalid report period")
)

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

func ParsePeriod(value string) (time.Time, time.Time, string, error) {
	trimmed := strings.TrimSpace(value)
	if trimmed == "" {
		return time.Time{}, time.Time{}, "", ErrInvalidPeriod
	}
	periodStart, err := time.Parse(PeriodLayout, trimmed)
	if err != nil {
		return time.Time{}, time.Time{}, "", ErrInvalidPeriod
	}
	periodEnd := periodStart.AddDate(0, 1, 0).Add(-time.Nanosecond)
	return periodStart, periodEnd, periodStart.Format(PeriodLayout), nil
}

func (s *Service) QueryReport(ctx context.Context, input QueryInput) (*Result, error) {
	if input.ReportID == ReportR19 {
		resolvedInput, err := s.resolveR19Input(ctx, input)
		if err != nil {
			return nil, err
		}
		visual, err := s.repository.QueryR19(ctx, resolvedInput)
		if err != nil {
			return nil, err
		}
		columns, rows := flattenR19Visual(visual)
		result := &Result{
			ReportID:    resolvedInput.ReportID,
			Columns:     columns,
			Rows:        rows,
			Visual:      visual,
			GeneratedAt: time.Now().UTC(),
		}
		if err := s.repository.InsertReportAudit(ctx, ReportAuditActionQuery, resolvedInput, len(result.Rows), 0); err != nil {
			return nil, err
		}
		return result, nil
	}
	columns, rows, err := s.runReport(ctx, input)
	if err != nil {
		return nil, err
	}
	result := &Result{
		ReportID:    input.ReportID,
		Columns:     columns,
		Rows:        rows,
		GeneratedAt: time.Now().UTC(),
	}
	if err := s.repository.InsertReportAudit(ctx, ReportAuditActionQuery, input, len(result.Rows), 0); err != nil {
		return nil, err
	}
	return result, nil
}

func (s *Service) ExportReport(ctx context.Context, input QueryInput) (*ExportArtifact, error) {
	if input.ReportID == ReportR19 {
		resolvedInput, err := s.resolveR19Input(ctx, input)
		if err != nil {
			return nil, err
		}
		visual, err := s.repository.QueryR19(ctx, resolvedInput)
		if err != nil {
			return nil, err
		}
		columns, rows := flattenR19Visual(visual)
		artifact, err := exportWorkbook(resolvedInput, columns, rows)
		if err != nil {
			return nil, err
		}
		if err := s.repository.InsertReportAudit(ctx, ReportAuditActionExport, resolvedInput, len(rows), len(artifact.Bytes)); err != nil {
			return nil, err
		}
		return artifact, nil
	}
	columns, rows, err := s.runReport(ctx, input)
	if err != nil {
		return nil, err
	}
	artifact, err := exportWorkbook(input, columns, rows)
	if err != nil {
		return nil, err
	}
	if err := s.repository.InsertReportAudit(ctx, ReportAuditActionExport, input, len(rows), len(artifact.Bytes)); err != nil {
		return nil, err
	}
	return artifact, nil
}

func exportWorkbook(input QueryInput, columns []Column, rows []map[string]any) (*ExportArtifact, error) {
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()

	sheet := f.GetSheetName(0)
	for index, column := range columns {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(sheet, cell, column.Label)
	}
	for rowIndex, row := range rows {
		for columnIndex, column := range columns {
			cell, _ := excelize.CoordinatesToCellName(columnIndex+1, rowIndex+2)
			_ = f.SetCellValue(sheet, cell, row[column.Key])
		}
	}

	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write report workbook: %w", err)
	}

	return &ExportArtifact{
		FileName:    fmt.Sprintf("generalize-%s-%s.xlsx", input.ReportID, input.PeriodLabel),
		ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
		Bytes:       buffer.Bytes(),
	}, nil
}

func (s *Service) runReport(ctx context.Context, input QueryInput) ([]Column, []map[string]any, error) {
	switch input.ReportID {
	case ReportR01:
		items, err := s.repository.QueryR01(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR01Columns(), rows, nil
	case ReportR02:
		items, err := s.repository.QueryR02(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR02Columns(), rows, nil
	case ReportR03:
		items, err := s.repository.QueryR03(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR03Columns(), rows, nil
	case ReportR04:
		items, err := s.repository.QueryR04(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR04Columns(), rows, nil
	case ReportR05:
		items, err := s.repository.QueryR05(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR05Columns(), rows, nil
	case ReportR06:
		items, err := s.repository.QueryR06(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR06Columns(), rows, nil
	case ReportR07:
		items, err := s.repository.QueryR07(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR07Columns(), rows, nil
	case ReportR08:
		items, err := s.repository.QueryR08(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return agingCustomerColumns(false), rows, nil
	case ReportR09:
		items, err := s.repository.QueryR09(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return agingCustomerColumns(true), rows, nil
	case ReportR10:
		items, err := s.repository.QueryR10(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR10Columns(), rows, nil
	case ReportR11:
		items, err := s.repository.QueryR11(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR11Columns(), rows, nil
	case ReportR12:
		items, err := s.repository.QueryR12(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR12Columns(), rows, nil
	case ReportR13:
		items, err := s.repository.QueryR13(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR13Columns(), rows, nil
	case ReportR14:
		items, err := s.repository.QueryR14(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR14Columns(), rows, nil
	case ReportR15:
		items, err := s.repository.QueryR15(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR15Columns(), rows, nil
	case ReportR16:
		items, err := s.repository.QueryR16(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return agingDepartmentColumns(false), rows, nil
	case ReportR17:
		items, err := s.repository.QueryR17(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return agingDepartmentColumns(true), rows, nil
	case ReportR18:
		items, err := s.repository.QueryR18(ctx, input)
		if err != nil {
			return nil, nil, err
		}
		rows := make([]map[string]any, 0, len(items))
		for _, item := range items {
			rows = append(rows, item.ToMap())
		}
		return reportR18Columns(), rows, nil
	default:
		return nil, nil, ErrUnsupportedReport
	}
}

func flattenR19Visual(visual *R19Result) ([]Column, []map[string]any) {
	columns := reportR19Columns()
	rows := make([]map[string]any, 0, len(visual.Units))
	for _, item := range visual.Units {
		rows = append(rows, item.ToMap())
	}
	return columns, rows
}

func (s *Service) resolveR19Input(ctx context.Context, input QueryInput) (QueryInput, error) {
	resolved := input
	if resolved.PeriodLabel == "" {
		resolved.PeriodLabel = "visual"
	}
	if resolved.FloorID != nil {
		return resolved, nil
	}
	floorID, err := s.repository.ResolveR19FloorID(ctx, resolved)
	if err != nil {
		return QueryInput{}, err
	}
	resolved.FloorID = floorID
	return resolved, nil
}

func (r R04Row) DayValue(day int) float64 {
	switch day {
	case 1:
		return r.Day01
	case 2:
		return r.Day02
	case 3:
		return r.Day03
	case 4:
		return r.Day04
	case 5:
		return r.Day05
	case 6:
		return r.Day06
	case 7:
		return r.Day07
	case 8:
		return r.Day08
	case 9:
		return r.Day09
	case 10:
		return r.Day10
	case 11:
		return r.Day11
	case 12:
		return r.Day12
	case 13:
		return r.Day13
	case 14:
		return r.Day14
	case 15:
		return r.Day15
	case 16:
		return r.Day16
	case 17:
		return r.Day17
	case 18:
		return r.Day18
	case 19:
		return r.Day19
	case 20:
		return r.Day20
	case 21:
		return r.Day21
	case 22:
		return r.Day22
	case 23:
		return r.Day23
	case 24:
		return r.Day24
	case 25:
		return r.Day25
	case 26:
		return r.Day26
	case 27:
		return r.Day27
	case 28:
		return r.Day28
	case 29:
		return r.Day29
	case 30:
		return r.Day30
	case 31:
		return r.Day31
	default:
		return 0
	}
}

func (r R10Row) MonthValue(month int) int {
	switch month {
	case 1:
		return r.Month01
	case 2:
		return r.Month02
	case 3:
		return r.Month03
	case 4:
		return r.Month04
	case 5:
		return r.Month05
	case 6:
		return r.Month06
	case 7:
		return r.Month07
	case 8:
		return r.Month08
	case 9:
		return r.Month09
	case 10:
		return r.Month10
	case 11:
		return r.Month11
	case 12:
		return r.Month12
	default:
		return 0
	}
}
