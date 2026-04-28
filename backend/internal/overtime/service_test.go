package overtime

import (
	"testing"
	"time"
)

func TestBuildGeneratedChargeSupportsFormulaShapes(t *testing.T) {
	bill := Bill{ID: 1, LeaseContractID: 10, PeriodStart: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC), PeriodEnd: time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC)}
	tests := []struct {
		name         string
		formula      Formula
		wantQuantity int
		wantUnit     float64
		wantAmount   float64
	}{
		{
			name: "fixed daily area rate",
			formula: Formula{ID: 11, ChargeType: "overtime_rent", FormulaType: FormulaTypeFixed, RateType: RateTypeDaily, EffectiveFrom: bill.PeriodStart, EffectiveTo: bill.PeriodEnd, CurrencyTypeID: 101, TotalArea: 10, UnitPrice: 2},
			wantQuantity: 30,
			wantUnit:     20,
			wantAmount:   600,
		},
		{
			name: "one time fixed rental",
			formula: Formula{ID: 12, ChargeType: "overtime_misc", FormulaType: FormulaTypeOneTime, RateType: RateTypeMonthly, EffectiveFrom: bill.PeriodStart, EffectiveTo: bill.PeriodEnd, CurrencyTypeID: 101, FixedRental: 1500},
			wantQuantity: 1,
			wantUnit:     1500,
			wantAmount:   1500,
		},
		{
			name: "percentage with minimum floor",
			formula: Formula{ID: 13, ChargeType: "overtime_sales", FormulaType: FormulaTypePercentage, RateType: RateTypeMonthly, EffectiveFrom: bill.PeriodStart, EffectiveTo: bill.PeriodEnd, CurrencyTypeID: 101, BaseAmount: 10000, PercentageTiers: []PercentTier{{SalesTo: 0, Percentage: 0.05}}, MinimumTiers: []MinimumTier{{SalesTo: 0, MinimumSum: 800}}},
			wantQuantity: 1,
			wantUnit:     500,
			wantAmount:   800,
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			charge, reason, ok := buildGeneratedCharge(bill, tt.formula, 101)
			if !ok {
				t.Fatalf("expected generated charge, got skipped reason=%s", reason)
			}
			if charge.Quantity != tt.wantQuantity || charge.UnitAmount != tt.wantUnit || charge.Amount != tt.wantAmount {
				t.Fatalf("unexpected generated charge %#v", charge)
			}
		})
	}
}
