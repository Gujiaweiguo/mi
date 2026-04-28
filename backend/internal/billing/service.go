package billing

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"math"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	mysql "github.com/go-sql-driver/mysql"
)

var (
	ErrInvalidBillingWindow = errors.New("invalid billing window")
)

type Service struct {
	repository *Repository
	db         *sql.DB
}

func NewService(db *sql.DB, repository *Repository) *Service {
	return &Service{db: db, repository: repository}
}

func (s *Service) GenerateCharges(ctx context.Context, input GenerateInput) (*GenerateResult, error) {
	if !validMonthlyWindow(input.PeriodStart, input.PeriodEnd) || input.ActorUserID == 0 {
		return nil, ErrInvalidBillingWindow
	}

	candidates, err := s.repository.ListChargeCandidates(ctx, input.PeriodStart, input.PeriodEnd)
	if err != nil {
		return nil, err
	}

	run := &Run{PeriodStart: input.PeriodStart, PeriodEnd: input.PeriodEnd, Status: RunStatusCompleted, TriggeredBy: input.ActorUserID}
	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin billing generation transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	if err := s.repository.CreateRun(ctx, tx, run); err != nil {
		return nil, err
	}

	lines := make([]ChargeLine, 0)
	generatedCount := 0
	skippedCount := 0
	for _, candidate := range candidates {
		status, effectiveVersion, billingEffective, err := s.repository.GetLeaseStateForUpdate(ctx, tx, candidate.LeaseContractID)
		if err != nil {
			return nil, err
		}
		if !billingEffective || (status != string(lease.StatusActive) && status != string(lease.StatusTerminated)) || effectiveVersion != candidate.EffectiveVersion {
			skippedCount++
			continue
		}

		line, ok := buildChargeLine(run.ID, input.PeriodStart, input.PeriodEnd, candidate)
		if !ok {
			continue
		}
		if err := s.repository.InsertChargeLine(ctx, tx, &line); err != nil {
			if isDuplicateEntry(err) {
				skippedCount++
				continue
			}
			return nil, fmt.Errorf("insert billing charge line: %w", err)
		}
		generatedCount++
		lines = append(lines, line)
	}

	if err := s.repository.UpdateRunCounts(ctx, tx, run.ID, generatedCount, skippedCount); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit billing generation transaction: %w", err)
	}

	run.GeneratedCount = generatedCount
	run.SkippedCount = skippedCount
	storedRun, err := s.repository.GetRun(ctx, run.ID)
	if err != nil {
		return nil, err
	}
	if storedRun != nil {
		run = storedRun
	}
	return &GenerateResult{Run: run, Lines: lines, Totals: Totals{Generated: generatedCount, Skipped: skippedCount}}, nil
}

func (s *Service) ListChargeLines(ctx context.Context, filter ChargeListFilter) (*pagination.ListResult[ChargeLine], error) {
	return s.repository.ListChargeLines(ctx, filter)
}

func buildChargeLine(runID int64, periodStart, periodEnd time.Time, candidate chargeCandidate) (ChargeLine, bool) {
	billingEffectiveDate := dateOnly(candidate.BillingEffectiveAt)
	chargeStart := maxDate(periodStart, candidate.LeaseStartDate, candidate.TermEffectiveFrom, billingEffectiveDate)
	chargeEnd := minDate(periodEnd, candidate.LeaseEndDate, candidate.TermEffectiveTo)
	if candidate.TerminatedAt != nil {
		terminationCutoff := dateOnly(candidate.TerminatedAt.AddDate(0, 0, -1))
		chargeEnd = minDate(chargeEnd, terminationCutoff)
	}
	if chargeEnd.Before(chargeStart) {
		return ChargeLine{}, false
	}

	windowDays := inclusiveDays(periodStart, periodEnd)
	chargeDays := inclusiveDays(chargeStart, chargeEnd)
	amount := roundCurrency(candidate.UnitAmount * float64(chargeDays) / float64(windowDays))
	if amount <= 0 {
		return ChargeLine{}, false
	}

	return ChargeLine{
		BillingRunID:           runID,
		LeaseContractID:        candidate.LeaseContractID,
		LeaseNo:                candidate.LeaseNo,
		TenantName:             candidate.TenantName,
		LeaseTermID:            &candidate.LeaseTermID,
		ChargeType:             ChargeTypeRent,
		ChargeSource:           ChargeSourceStandard,
		PeriodStart:            periodStart,
		PeriodEnd:              periodEnd,
		QuantityDays:           chargeDays,
		UnitAmount:             candidate.UnitAmount,
		Amount:                 amount,
		CurrencyTypeID:         candidate.CurrencyTypeID,
		SourceEffectiveVersion: candidate.EffectiveVersion,
	}, true
}

func validMonthlyWindow(start, end time.Time) bool {
	start = dateOnly(start)
	end = dateOnly(end)
	if start.IsZero() || end.IsZero() || start.After(end) {
		return false
	}
	if start.Day() != 1 {
		return false
	}
	monthEnd := time.Date(start.Year(), start.Month()+1, 0, 0, 0, 0, 0, start.Location())
	return end.Equal(monthEnd)
}

func dateOnly(value time.Time) time.Time {
	return time.Date(value.Year(), value.Month(), value.Day(), 0, 0, 0, 0, time.UTC)
}

func inclusiveDays(start, end time.Time) int {
	return int(dateOnly(end).Sub(dateOnly(start)).Hours()/24) + 1
}

func maxDate(values ...time.Time) time.Time {
	max := dateOnly(values[0])
	for _, value := range values[1:] {
		current := dateOnly(value)
		if current.After(max) {
			max = current
		}
	}
	return max
}

func minDate(values ...time.Time) time.Time {
	min := dateOnly(values[0])
	for _, value := range values[1:] {
		current := dateOnly(value)
		if current.Before(min) {
			min = current
		}
	}
	return min
}

func roundCurrency(value float64) float64 {
	return math.Round(value*100) / 100
}

func isDuplicateEntry(err error) bool {
	var mysqlErr *mysql.MySQLError
	return errors.As(err, &mysqlErr) && mysqlErr.Number == 1062
}
