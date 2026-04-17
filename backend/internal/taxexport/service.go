package taxexport

import (
	"context"
	"errors"
	"fmt"
	"math"
	"sort"
	"strings"

	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/xuri/excelize/v2"
)

var (
	ErrRuleSetNotFound     = errors.New("tax voucher rule set not found")
	ErrInvalidRuleSet      = errors.New("invalid tax voucher rule set")
	ErrInvalidExportWindow = errors.New("invalid tax export date range")
	ErrInvalidTaxSetup     = errors.New("invalid tax setup")
)

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

func (s *Service) UpsertRuleSet(ctx context.Context, input UpsertRuleSetInput) (*RuleSet, error) {
	ruleSet, err := ruleSetFromInput(input)
	if err != nil {
		return nil, err
	}
	tx, err := s.repository.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin tax voucher rule set transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	if err := s.repository.UpsertRuleSet(ctx, tx, ruleSet); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit tax voucher rule set transaction: %w", err)
	}
	return s.repository.FindRuleSetByCode(ctx, ruleSet.Code)
}

func (s *Service) ListRuleSets(ctx context.Context, filter ListFilter) (*pagination.ListResult[RuleSet], error) {
	return s.repository.ListRuleSets(ctx, filter)
}

func (s *Service) ExportVoucherWorkbook(ctx context.Context, input ExportInput) (*ExportArtifact, error) {
	if strings.TrimSpace(input.RuleSetCode) == "" || input.ActorUserID == 0 || input.FromDate.IsZero() || input.ToDate.IsZero() || input.FromDate.After(input.ToDate) {
		return nil, ErrInvalidExportWindow
	}
	ruleSet, err := s.repository.FindRuleSetByCode(ctx, strings.TrimSpace(input.RuleSetCode))
	if err != nil {
		return nil, err
	}
	if ruleSet == nil {
		return nil, ErrRuleSetNotFound
	}
	if err := validateRuleSet(*ruleSet); err != nil {
		return nil, err
	}
	documents, err := s.repository.ListApprovedDocumentsForExport(ctx, ruleSet.DocumentType, input.FromDate, input.ToDate)
	if err != nil {
		return nil, err
	}
	entries, err := buildVoucherEntries(*ruleSet, documents)
	if err != nil {
		return nil, err
	}
	bytesValue, err := buildWorkbook(entries)
	if err != nil {
		return nil, err
	}
	return &ExportArtifact{FileName: fmt.Sprintf("tax-voucher-%s-%s.xlsx", input.FromDate.Format("20060102"), input.ToDate.Format("20060102")), ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Bytes: bytesValue, DocumentCount: len(documents), EntryCount: len(entries)}, nil
}

func ruleSetFromInput(input UpsertRuleSetInput) (*RuleSet, error) {
	code := strings.TrimSpace(input.Code)
	name := strings.TrimSpace(input.Name)
	documentType := strings.TrimSpace(input.DocumentType)
	if code == "" || name == "" || input.ActorUserID == 0 || (documentType != "invoice" && documentType != "bill") || len(input.Rules) == 0 {
		return nil, ErrInvalidRuleSet
	}
	rules := make([]Rule, 0, len(input.Rules))
	for _, rule := range input.Rules {
		if rule.SequenceNo <= 0 || (rule.EntrySide != EntrySideDebit && rule.EntrySide != EntrySideCredit) || strings.TrimSpace(rule.AccountNumber) == "" || strings.TrimSpace(rule.AccountName) == "" || strings.TrimSpace(rule.ExplanationTemplate) == "" {
			return nil, ErrInvalidRuleSet
		}
		chargeTypeFilter := strings.TrimSpace(rule.ChargeTypeFilter)
		if chargeTypeFilter == "" {
			chargeTypeFilter = "*"
		}
		rules = append(rules, Rule{SequenceNo: rule.SequenceNo, EntrySide: rule.EntrySide, ChargeTypeFilter: chargeTypeFilter, AccountNumber: strings.TrimSpace(rule.AccountNumber), AccountName: strings.TrimSpace(rule.AccountName), ExplanationTemplate: strings.TrimSpace(rule.ExplanationTemplate), UseTenantName: rule.UseTenantName, IsBalancingEntry: rule.IsBalancingEntry})
	}
	sort.Slice(rules, func(i, j int) bool { return rules[i].SequenceNo < rules[j].SequenceNo })
	return &RuleSet{Code: code, Name: name, DocumentType: documentType, Status: RuleSetStatusActive, CreatedBy: input.ActorUserID, UpdatedBy: input.ActorUserID, Rules: rules}, nil
}

func validateRuleSet(ruleSet RuleSet) error {
	if ruleSet.Status != RuleSetStatusActive || len(ruleSet.Rules) == 0 {
		return ErrInvalidRuleSet
	}
	hasOperationalRule := false
	for _, rule := range ruleSet.Rules {
		if rule.EntrySide != EntrySideDebit && rule.EntrySide != EntrySideCredit {
			return ErrInvalidRuleSet
		}
		if !rule.IsBalancingEntry {
			hasOperationalRule = true
		}
	}
	if !hasOperationalRule {
		return ErrInvalidRuleSet
	}
	return nil
}

func buildVoucherEntries(ruleSet RuleSet, documents []exportDocument) ([]voucherEntry, error) {
	entries := make([]voucherEntry, 0)
	entryID := 1
	for documentIndex, document := range documents {
		groupEntries := make([]voucherEntry, 0)
		debitTotal := 0.0
		creditTotal := 0.0
		for _, line := range document.Lines {
			matched := false
			for _, rule := range ruleSet.Rules {
				if rule.IsBalancingEntry {
					continue
				}
				if rule.ChargeTypeFilter != "*" && rule.ChargeTypeFilter != line.ChargeType {
					continue
				}
				matched = true
				entry := buildVoucherEntry(documentIndex+1, entryID, document, line, rule, line.Amount)
				entryID++
				groupEntries = append(groupEntries, entry)
				if rule.EntrySide == EntrySideDebit {
					debitTotal += line.Amount
				} else {
					creditTotal += line.Amount
				}
			}
			if !matched {
				return nil, fmt.Errorf("%w: missing tax mapping for charge type %s on document %s", ErrInvalidTaxSetup, line.ChargeType, document.DocumentNo)
			}
		}
		if diff := roundCurrency(debitTotal - creditTotal); diff != 0 {
			balancingRule := findBalancingRule(ruleSet.Rules, diff)
			if balancingRule == nil {
				return nil, fmt.Errorf("%w: unbalanced export group for document %s", ErrInvalidTaxSetup, document.DocumentNo)
			}
			amount := diff
			if amount < 0 {
				amount = -amount
			}
			line := exportLine{ChargeType: "balance", PeriodStart: document.PeriodStart, PeriodEnd: document.PeriodEnd, QuantityDays: 0, UnitAmount: 0, Amount: amount}
			entry := buildVoucherEntry(documentIndex+1, entryID, document, line, *balancingRule, amount)
			entryID++
			groupEntries = append(groupEntries, entry)
			if balancingRule.EntrySide == EntrySideDebit {
				debitTotal += amount
			} else {
				creditTotal += amount
			}
		}
		if roundCurrency(debitTotal) != roundCurrency(creditTotal) {
			return nil, fmt.Errorf("%w: debit/credit totals do not balance for document %s", ErrInvalidTaxSetup, document.DocumentNo)
		}
		entries = append(entries, groupEntries...)
	}
	return entries, nil
}

func buildVoucherEntry(groupNumber, entryID int, document exportDocument, line exportLine, rule Rule, amount float64) voucherEntry {
	explanation := expandExplanation(rule.ExplanationTemplate, document, line)
	handler := document.DocumentNo
	if rule.UseTenantName {
		handler = document.TenantName
	}
	entry := voucherEntry{Date: document.ApprovedAt.Format(DateLayout), Year: fmt.Sprintf("%04d", document.ApprovedAt.Year()), Period: fmt.Sprintf("%d", document.ApprovedAt.Month()), GroupID: document.DocumentNo, Number: groupNumber, AccountNum: documentQuoted(rule.AccountNumber), AccountName: documentQuoted(rule.AccountName), CurrencyNum: documentQuoted("RMB"), CurrencyName: "人民币", AmountFor: amount, PreparerID: documentQuoted("NONE"), CheckerID: documentQuoted("NONE"), ApproveID: documentQuoted("NONE"), CashierID: documentQuoted("NONE"), Handler: handler, SettleTypeID: "*", SettleNo: "", Explanation: explanation, Quantity: "'0", MeasureUnitID: "*", UnitPrice: "'0", Reference: document.DocumentNo, TransDate: document.ApprovedAt.Format(DateLayout), TransNo: document.DocumentNo, Attachments: "'0", SerialNum: "'1", ObjectName: document.DocumentType, Parameter: fmt.Sprintf("document_id=%d", document.DocumentID), ExchangeRate: "'1", EntryID: entryID, Item: line.ChargeType, Posted: 0, InternalInd: "", CashFlow: ""}
	if rule.EntrySide == EntrySideDebit {
		entry.Debit = amount
	} else {
		entry.Credit = amount
	}
	return entry
}

func buildWorkbook(entries []voucherEntry) ([]byte, error) {
	f := excelize.NewFile()
	defer func() { _ = f.Close() }()
	sheet := f.GetSheetName(0)
	headers := []string{"FDate", "FYear", "FPeriod", "FGroupID", "FNumber", "FAccountNum", "FAccountName", "FCurrencyNum", "FCurrencyName", "FAmountFor", "FDebit", "FCredit", "FPreparerID", "FCheckerID", "FApproveID", "FCashierID", "FHandler", "FSettleTypeID", "FSettleNo", "FExplanation", "FQuantity", "FMeasureUnitID", "FUnitPrice", "FReference", "FTransDate", "FTransNo", "FAttachments", "FSerialNum", "FObjectName", "FParameter", "FExchangeRate", "FEntryID", "FItem", "FPosted", "FInternalInd", "FCashFlow"}
	for index, header := range headers {
		cell, _ := excelize.CoordinatesToCellName(index+1, 1)
		_ = f.SetCellValue(sheet, cell, header)
	}
	for rowIndex, entry := range entries {
		values := []any{entry.Date, entry.Year, entry.Period, entry.GroupID, entry.Number, entry.AccountNum, entry.AccountName, entry.CurrencyNum, entry.CurrencyName, entry.AmountFor, entry.Debit, entry.Credit, entry.PreparerID, entry.CheckerID, entry.ApproveID, entry.CashierID, entry.Handler, entry.SettleTypeID, entry.SettleNo, entry.Explanation, entry.Quantity, entry.MeasureUnitID, entry.UnitPrice, entry.Reference, entry.TransDate, entry.TransNo, entry.Attachments, entry.SerialNum, entry.ObjectName, entry.Parameter, entry.ExchangeRate, entry.EntryID, entry.Item, entry.Posted, entry.InternalInd, entry.CashFlow}
		for columnIndex, value := range values {
			cell, _ := excelize.CoordinatesToCellName(columnIndex+1, rowIndex+2)
			_ = f.SetCellValue(sheet, cell, value)
		}
	}
	buffer, err := f.WriteToBuffer()
	if err != nil {
		return nil, fmt.Errorf("write tax voucher workbook: %w", err)
	}
	return buffer.Bytes(), nil
}

func findBalancingRule(rules []Rule, diff float64) *Rule {
	neededSide := EntrySideCredit
	if diff < 0 {
		neededSide = EntrySideDebit
	}
	for _, rule := range rules {
		if rule.IsBalancingEntry && rule.EntrySide == neededSide {
			ruleCopy := rule
			return &ruleCopy
		}
	}
	return nil
}

func expandExplanation(template string, document exportDocument, line exportLine) string {
	replacements := map[string]string{
		"YYYYMMDD-YYYYMMDD": fmt.Sprintf("%s-%s", document.PeriodStart.Format("20060102"), document.PeriodEnd.Format("20060102")),
		"SYYYYMMDD":         document.PeriodStart.Format("20060102"),
		"EYYYYMMDD":         document.PeriodEnd.Format("20060102"),
		"SYYYYMM":           document.PeriodStart.Format("200601"),
		"EYYYYMM":           document.PeriodEnd.Format("200601"),
		"ITEMCODE":          line.ChargeType,
	}
	result := template
	for key, value := range replacements {
		result = strings.ReplaceAll(result, key, value)
	}
	return result
}

func documentQuoted(value string) string  { return "'" + value }
func roundCurrency(value float64) float64 { return math.Round(value*100) / 100 }
