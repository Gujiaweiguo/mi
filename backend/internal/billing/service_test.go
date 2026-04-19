package billing

import (
	"testing"
	"time"
)

func TestValidMonthlyWindow(t *testing.T) {
	tests := []struct {
		name    string
		start   time.Time
		end     time.Time
		want    bool
	}{
		{
			name:  "valid full month April 2026",
			start: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
			want:  true,
		},
		{
			name:  "valid full month May 2026 31 days",
			start: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC),
			want:  true,
		},
		{
			name:  "valid February leap year",
			start: time.Date(2024, 2, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2024, 2, 29, 0, 0, 0, 0, time.UTC),
			want:  true,
		},
		{
			name:  "invalid start not first of month",
			start: time.Date(2026, 4, 5, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
			want:  false,
		},
		{
			name:  "invalid end does not match month end",
			start: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 4, 29, 0, 0, 0, 0, time.UTC),
			want:  false,
		},
		{
			name:  "invalid start after end",
			start: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
			want:  false,
		},
		{
			name:  "invalid zero start",
			start: time.Time{},
			end:   time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
			want:  false,
		},
		{
			name:  "invalid zero end",
			start: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Time{},
			want:  false,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			if got := validMonthlyWindow(tt.start, tt.end); got != tt.want {
				t.Errorf("validMonthlyWindow() = %v, want %v", got, tt.want)
			}
		})
	}
}

func TestInclusiveDays(t *testing.T) {
	tests := []struct {
		name  string
		start time.Time
		end   time.Time
		want  int
	}{
		{
			name:  "single day",
			start: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			want:  1,
		},
		{
			name:  "full April",
			start: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC),
			want:  30,
		},
		{
			name:  "full May",
			start: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			end:   time.Date(2026, 5, 31, 0, 0, 0, 0, time.UTC),
			want:  31,
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			if got := inclusiveDays(tt.start, tt.end); got != tt.want {
				t.Errorf("inclusiveDays() = %v, want %v", got, tt.want)
			}
		})
	}
}

func TestRoundCurrency(t *testing.T) {
	tests := []struct {
		name  string
		value float64
		want  float64
	}{
		{name: "exact value", value: 100.0, want: 100.0},
		{name: "round up at midpoint", value: 100.005, want: 100.01},
		{name: "round down below midpoint", value: 100.004, want: 100.0},
		{name: "proration result", value: 12000.0 * 14 / 30, want: 5600.0},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			if got := roundCurrency(tt.value); got != tt.want {
				t.Errorf("roundCurrency() = %v, want %v", got, tt.want)
			}
		})
	}
}

func TestBuildChargeLine(t *testing.T) {
	periodStart := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	periodEnd := time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC)

	t.Run("full month charge", func(t *testing.T) {
		candidate := chargeCandidate{
			LeaseContractID:    1,
			LeaseNo:            "L001",
			TenantName:         "Tenant A",
			LeaseStatus:        "active",
			LeaseStartDate:     time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			LeaseEndDate:       time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			BillingEffectiveAt: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			TerminatedAt:       nil,
			EffectiveVersion:   1,
			LeaseTermID:        10,
			TermType:           "rent",
			BillingCycle:       "monthly",
			CurrencyTypeID:     1,
			UnitAmount:         12000,
			TermEffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			TermEffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
		}

		line, ok := buildChargeLine(100, periodStart, periodEnd, candidate)
		if !ok {
			t.Fatal("expected buildChargeLine to succeed")
		}
		if line.BillingRunID != 100 {
			t.Errorf("BillingRunID = %d, want 100", line.BillingRunID)
		}
		if line.LeaseContractID != 1 {
			t.Errorf("LeaseContractID = %d, want 1", line.LeaseContractID)
		}
		if line.QuantityDays != 30 {
			t.Errorf("QuantityDays = %d, want 30", line.QuantityDays)
		}
		if line.Amount != 12000.0 {
			t.Errorf("Amount = %v, want 12000", line.Amount)
		}
		if line.ChargeType != ChargeTypeRent {
			t.Errorf("ChargeType = %s, want %s", line.ChargeType, ChargeTypeRent)
		}
	})

	t.Run("prorated termination charge", func(t *testing.T) {
		terminatedAt := time.Date(2026, 4, 15, 0, 0, 0, 0, time.UTC)
		candidate := chargeCandidate{
			LeaseContractID:    2,
			LeaseNo:            "L002",
			TenantName:         "Tenant B",
			LeaseStatus:        "terminated",
			LeaseStartDate:     time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			LeaseEndDate:       time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
			BillingEffectiveAt: time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			TerminatedAt:       &terminatedAt,
			EffectiveVersion:   1,
			LeaseTermID:        20,
			TermType:           "rent",
			BillingCycle:       "monthly",
			CurrencyTypeID:     1,
			UnitAmount:         12000,
			TermEffectiveFrom:  time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC),
			TermEffectiveTo:    time.Date(2027, 3, 31, 0, 0, 0, 0, time.UTC),
		}

		line, ok := buildChargeLine(101, periodStart, periodEnd, candidate)
		if !ok {
			t.Fatal("expected buildChargeLine to succeed for terminated lease")
		}
		// termination cutoff = terminatedAt - 1 day = April 14
		// chargeStart = April 1, chargeEnd = min(April 30, March 31, April 14) = April 14
		// days = 14, amount = 12000 * 14 / 30 = 5600
		if line.QuantityDays != 14 {
			t.Errorf("QuantityDays = %d, want 14", line.QuantityDays)
		}
		if line.Amount != 5600.0 {
			t.Errorf("Amount = %v, want 5600", line.Amount)
		}
	})

	t.Run("no charge when window is empty", func(t *testing.T) {
		candidate := chargeCandidate{
			LeaseContractID:    3,
			LeaseNo:            "L003",
			TenantName:         "Tenant C",
			LeaseStartDate:     time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			LeaseEndDate:       time.Date(2027, 4, 30, 0, 0, 0, 0, time.UTC),
			BillingEffectiveAt: time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			TerminatedAt:       nil,
			EffectiveVersion:   1,
			LeaseTermID:        30,
			TermType:           "rent",
			BillingCycle:       "monthly",
			CurrencyTypeID:     1,
			UnitAmount:         10000,
			TermEffectiveFrom:  time.Date(2026, 5, 1, 0, 0, 0, 0, time.UTC),
			TermEffectiveTo:    time.Date(2027, 4, 30, 0, 0, 0, 0, time.UTC),
		}

		_, ok := buildChargeLine(102, periodStart, periodEnd, candidate)
		if ok {
			t.Fatal("expected buildChargeLine to return false for out-of-window term")
		}
	})
}

func TestDateOnly(t *testing.T) {
	input := time.Date(2026, 4, 15, 14, 30, 0, 0, time.UTC)
	got := dateOnly(input)
	want := time.Date(2026, 4, 15, 0, 0, 0, 0, time.UTC)
	if !got.Equal(want) {
		t.Errorf("dateOnly() = %v, want %v", got, want)
	}
}

func TestMaxDate(t *testing.T) {
	a := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	b := time.Date(2026, 4, 15, 0, 0, 0, 0, time.UTC)
	c := time.Date(2026, 3, 31, 0, 0, 0, 0, time.UTC)
	got := maxDate(a, b, c)
	if !got.Equal(b) {
		t.Errorf("maxDate() = %v, want %v", got, b)
	}
}

func TestMinDate(t *testing.T) {
	a := time.Date(2026, 4, 1, 0, 0, 0, 0, time.UTC)
	b := time.Date(2026, 4, 30, 0, 0, 0, 0, time.UTC)
	c := time.Date(2026, 4, 15, 0, 0, 0, 0, time.UTC)
	got := minDate(a, b, c)
	if !got.Equal(a) {
		t.Errorf("minDate() = %v, want %v", got, a)
	}
}
