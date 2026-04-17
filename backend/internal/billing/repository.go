package billing

import (
	"context"
	"database/sql"
	"fmt"
	"sort"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
)

type Repository struct {
	db *sql.DB
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db}
}

func (r *Repository) CreateRun(ctx context.Context, tx *sql.Tx, run *Run) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO billing_runs (period_start, period_end, status, triggered_by, generated_count, skipped_count)
		VALUES (?, ?, ?, ?, ?, ?)
	`, run.PeriodStart, run.PeriodEnd, run.Status, run.TriggeredBy, run.GeneratedCount, run.SkippedCount)
	if err != nil {
		return fmt.Errorf("insert billing run: %w", err)
	}
	runID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get billing run id: %w", err)
	}
	run.ID = runID
	return nil
}

func (r *Repository) UpdateRunCounts(ctx context.Context, tx *sql.Tx, runID int64, generatedCount, skippedCount int) error {
	if _, err := tx.ExecContext(ctx, `
		UPDATE billing_runs
		SET generated_count = ?, skipped_count = ?, status = ?
		WHERE id = ?
	`, generatedCount, skippedCount, RunStatusCompleted, runID); err != nil {
		return fmt.Errorf("update billing run counts: %w", err)
	}
	return nil
}

func (r *Repository) InsertChargeLine(ctx context.Context, tx *sql.Tx, line *ChargeLine) error {
	result, err := tx.ExecContext(ctx, `
		INSERT INTO billing_charge_lines (
			billing_run_id, lease_contract_id, lease_term_id, charge_type, period_start, period_end,
			quantity_days, unit_amount, amount, currency_type_id, source_effective_version
		) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, line.BillingRunID, line.LeaseContractID, line.LeaseTermID, line.ChargeType, line.PeriodStart, line.PeriodEnd, line.QuantityDays, line.UnitAmount, line.Amount, line.CurrencyTypeID, line.SourceEffectiveVersion)
	if err != nil {
		return err
	}
	lineID, err := result.LastInsertId()
	if err != nil {
		return fmt.Errorf("get billing charge line id: %w", err)
	}
	line.ID = lineID
	return nil
}

func (r *Repository) GetLeaseStateForUpdate(ctx context.Context, tx *sql.Tx, leaseContractID int64) (string, int, bool, error) {
	var status string
	var effectiveVersion int
	var billingEffectiveAt sql.NullTime
	if err := tx.QueryRowContext(ctx, `
		SELECT status, effective_version, billing_effective_at
		FROM lease_contracts
		WHERE id = ?
		FOR UPDATE
	`, leaseContractID).Scan(&status, &effectiveVersion, &billingEffectiveAt); err != nil {
		if err == sql.ErrNoRows {
			return "", 0, false, nil
		}
		return "", 0, false, fmt.Errorf("load lease state for billing candidate: %w", err)
	}
	return status, effectiveVersion, billingEffectiveAt.Valid, nil
}

func (r *Repository) ListChargeCandidates(ctx context.Context, periodStart, periodEnd time.Time) ([]chargeCandidate, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT lc.id, lc.lease_no, lc.tenant_name, lc.status, lc.start_date, lc.end_date,
			lc.billing_effective_at, lc.terminated_at, lc.effective_version,
			lt.id, lt.term_type, lt.billing_cycle, lt.currency_type_id, lt.amount, lt.effective_from, lt.effective_to
		FROM lease_contracts lc
		INNER JOIN lease_contract_terms lt ON lt.lease_contract_id = lc.id
		WHERE lc.status IN ('active', 'terminated')
		  AND lc.billing_effective_at IS NOT NULL
		  AND lt.term_type = 'rent'
		  AND lt.billing_cycle = 'monthly'
		  AND lc.start_date <= ?
		  AND lc.end_date >= ?
		  AND lt.effective_from <= ?
		  AND lt.effective_to >= ?
		ORDER BY lc.id, lt.id
	`, periodEnd, periodStart, periodEnd, periodStart)
	if err != nil {
		return nil, fmt.Errorf("query billing charge candidates: %w", err)
	}
	defer rows.Close()

	candidates := make([]chargeCandidate, 0)
	for rows.Next() {
		var candidate chargeCandidate
		var terminatedAt sql.NullTime
		if err := rows.Scan(&candidate.LeaseContractID, &candidate.LeaseNo, &candidate.TenantName, &candidate.LeaseStatus, &candidate.LeaseStartDate, &candidate.LeaseEndDate, &candidate.BillingEffectiveAt, &terminatedAt, &candidate.EffectiveVersion, &candidate.LeaseTermID, &candidate.TermType, &candidate.BillingCycle, &candidate.CurrencyTypeID, &candidate.UnitAmount, &candidate.TermEffectiveFrom, &candidate.TermEffectiveTo); err != nil {
			return nil, fmt.Errorf("scan billing charge candidate: %w", err)
		}
		candidate.TerminatedAt = nullTimePointer(terminatedAt)
		candidates = append(candidates, candidate)
	}
	return candidates, rows.Err()
}

func (r *Repository) GetRun(ctx context.Context, runID int64) (*Run, error) {
	var run Run
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, period_start, period_end, status, triggered_by, generated_count, skipped_count, created_at, updated_at
		FROM billing_runs WHERE id = ?
	`, runID).Scan(&run.ID, &run.PeriodStart, &run.PeriodEnd, &run.Status, &run.TriggeredBy, &run.GeneratedCount, &run.SkippedCount, &run.CreatedAt, &run.UpdatedAt); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query billing run: %w", err)
	}
	return &run, nil
}

func (r *Repository) ListChargeLines(ctx context.Context, filter ChargeListFilter) (*pagination.ListResult[ChargeLine], error) {
	page, pageSize := pagination.NormalizePage(filter.Page, filter.PageSize)
	conditions := []string{"1=1"}
	args := make([]any, 0)
	if filter.LeaseContractID != nil {
		conditions = append(conditions, "bcl.lease_contract_id = ?")
		args = append(args, *filter.LeaseContractID)
	}
	if filter.PeriodStart != nil {
		conditions = append(conditions, "bcl.period_start >= ?")
		args = append(args, *filter.PeriodStart)
	}
	if filter.PeriodEnd != nil {
		conditions = append(conditions, "bcl.period_end <= ?")
		args = append(args, *filter.PeriodEnd)
	}
	whereClause := strings.Join(conditions, " AND ")

	var total int64
	if err := r.db.QueryRowContext(ctx, `SELECT COUNT(*) FROM billing_charge_lines bcl WHERE `+whereClause, args...).Scan(&total); err != nil {
		return nil, fmt.Errorf("count billing charge lines: %w", err)
	}

	queryArgs := append(append([]any{}, args...), pageSize, (page-1)*pageSize)
	rows, err := r.db.QueryContext(ctx, `
		SELECT bcl.id, bcl.billing_run_id, bcl.lease_contract_id, lc.lease_no, lc.tenant_name, bcl.lease_term_id,
			bcl.charge_type, bcl.period_start, bcl.period_end, bcl.quantity_days, bcl.unit_amount, bcl.amount,
			bcl.currency_type_id, bcl.source_effective_version, bcl.created_at
		FROM billing_charge_lines bcl
		INNER JOIN lease_contracts lc ON lc.id = bcl.lease_contract_id
		WHERE `+whereClause+`
		ORDER BY bcl.id DESC
		LIMIT ? OFFSET ?
	`, queryArgs...)
	if err != nil {
		return nil, fmt.Errorf("list billing charge lines: %w", err)
	}
	defer rows.Close()

	items := make([]ChargeLine, 0)
	for rows.Next() {
		var line ChargeLine
		if err := rows.Scan(&line.ID, &line.BillingRunID, &line.LeaseContractID, &line.LeaseNo, &line.TenantName, &line.LeaseTermID, &line.ChargeType, &line.PeriodStart, &line.PeriodEnd, &line.QuantityDays, &line.UnitAmount, &line.Amount, &line.CurrencyTypeID, &line.SourceEffectiveVersion, &line.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan billing charge line: %w", err)
		}
		items = append(items, line)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate billing charge lines: %w", err)
	}

	return &pagination.ListResult[ChargeLine]{Items: items, Total: total, Page: page, PageSize: pageSize}, nil
}

func (r *Repository) GetChargeLinesByIDs(ctx context.Context, ids []int64) ([]ChargeLine, error) {
	if len(ids) == 0 {
		return []ChargeLine{}, nil
	}
	placeholders := make([]string, 0, len(ids))
	args := make([]any, 0, len(ids))
	for _, id := range ids {
		placeholders = append(placeholders, "?")
		args = append(args, id)
	}
	rows, err := r.db.QueryContext(ctx, `
		SELECT bcl.id, bcl.billing_run_id, bcl.lease_contract_id, lc.lease_no, lc.tenant_name, bcl.lease_term_id,
			bcl.charge_type, bcl.period_start, bcl.period_end, bcl.quantity_days, bcl.unit_amount, bcl.amount,
			bcl.currency_type_id, bcl.source_effective_version, bcl.created_at
		FROM billing_charge_lines bcl
		INNER JOIN lease_contracts lc ON lc.id = bcl.lease_contract_id
		WHERE bcl.id IN (`+strings.Join(placeholders, ",")+`)
	`, args...)
	if err != nil {
		return nil, fmt.Errorf("get billing charge lines by ids: %w", err)
	}
	defer rows.Close()

	items := make([]ChargeLine, 0, len(ids))
	for rows.Next() {
		var line ChargeLine
		if err := rows.Scan(&line.ID, &line.BillingRunID, &line.LeaseContractID, &line.LeaseNo, &line.TenantName, &line.LeaseTermID, &line.ChargeType, &line.PeriodStart, &line.PeriodEnd, &line.QuantityDays, &line.UnitAmount, &line.Amount, &line.CurrencyTypeID, &line.SourceEffectiveVersion, &line.CreatedAt); err != nil {
			return nil, fmt.Errorf("scan billing charge line by id: %w", err)
		}
		items = append(items, line)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate billing charge lines by ids: %w", err)
	}
	order := make(map[int64]int, len(ids))
	for index, id := range ids {
		order[id] = index
	}
	sort.Slice(items, func(i, j int) bool { return order[items[i].ID] < order[items[j].ID] })
	return items, nil
}

func nullTimePointer(value sql.NullTime) *time.Time {
	if !value.Valid {
		return nil
	}
	v := value.Time
	return &v
}
