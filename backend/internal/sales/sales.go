package sales

import (
	"context"
	"database/sql"
	"fmt"
	"time"
)

type DailySale struct {
	ID          int64     `json:"id"`
	StoreID     int64     `json:"store_id"`
	UnitID      int64     `json:"unit_id"`
	SaleDate    time.Time `json:"sale_date"`
	SalesAmount float64   `json:"sales_amount"`
	CreatedAt   time.Time `json:"created_at"`
	UpdatedAt   time.Time `json:"updated_at"`
}

type CustomerTraffic struct {
	ID           int64     `json:"id"`
	StoreID      int64     `json:"store_id"`
	TrafficDate  time.Time `json:"traffic_date"`
	InboundCount int       `json:"inbound_count"`
	CreatedAt    time.Time `json:"created_at"`
	UpdatedAt    time.Time `json:"updated_at"`
}

type CreateDailySaleInput struct {
	StoreID     int64
	UnitID      int64
	SaleDate    time.Time
	SalesAmount float64
}

type CreateTrafficInput struct {
	StoreID      int64
	TrafficDate  time.Time
	InboundCount int
}

type BatchDailySaleInput = CreateDailySaleInput

type BatchTrafficInput = CreateTrafficInput

type DailySaleFilter struct {
	StoreID  *int64
	UnitID   *int64
	DateFrom *time.Time
	DateTo   *time.Time
	Limit    int
	Offset   int
}

type TrafficFilter struct {
	StoreID  *int64
	DateFrom *time.Time
	DateTo   *time.Time
	Limit    int
	Offset   int
}

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

func (r *Repository) ListDailySales(ctx context.Context, filter DailySaleFilter) ([]DailySale, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, store_id, unit_id, sale_date, sales_amount, created_at, updated_at
		FROM daily_shop_sales
		WHERE (? IS NULL OR store_id = ?)
		  AND (? IS NULL OR unit_id = ?)
		  AND (? IS NULL OR sale_date >= ?)
		  AND (? IS NULL OR sale_date <= ?)
		ORDER BY sale_date DESC, id DESC
		LIMIT ? OFFSET ?
	`, int64PointerValue(filter.StoreID), int64PointerValue(filter.StoreID), int64PointerValue(filter.UnitID), int64PointerValue(filter.UnitID), timePointerValue(filter.DateFrom), timePointerValue(filter.DateFrom), timePointerValue(filter.DateTo), timePointerValue(filter.DateTo), normalizeLimit(filter.Limit), normalizeOffset(filter.Offset))
	if err != nil {
		return nil, fmt.Errorf("list daily sales: %w", err)
	}
	defer rows.Close()

	items := make([]DailySale, 0)
	for rows.Next() {
		var item DailySale
		if err := rows.Scan(&item.ID, &item.StoreID, &item.UnitID, &item.SaleDate, &item.SalesAmount, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan daily sale: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate daily sales: %w", err)
	}
	return items, nil
}

func (r *Repository) CreateDailySale(ctx context.Context, input CreateDailySaleInput) (*DailySale, error) {
	saleDate := input.SaleDate.Format("2006-01-02")
	if _, err := r.db.ExecContext(ctx, `
		INSERT INTO daily_shop_sales (store_id, unit_id, sale_date, sales_amount)
		VALUES (?, ?, ?, ?)
		ON DUPLICATE KEY UPDATE sales_amount = VALUES(sales_amount)
	`, input.StoreID, input.UnitID, saleDate, input.SalesAmount); err != nil {
		return nil, fmt.Errorf("upsert daily sale: %w", err)
	}
	return r.findDailySaleByNaturalKey(ctx, input.StoreID, input.UnitID, saleDate)
}

func (r *Repository) BatchUpsertDailySales(ctx context.Context, inputs []BatchDailySaleInput) error {
	if len(inputs) == 0 {
		return nil
	}
	tx, err := r.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin daily sales batch transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	for _, input := range inputs {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO daily_shop_sales (store_id, unit_id, sale_date, sales_amount)
			VALUES (?, ?, ?, ?)
			ON DUPLICATE KEY UPDATE sales_amount = VALUES(sales_amount)
		`, input.StoreID, input.UnitID, input.SaleDate.Format("2006-01-02"), input.SalesAmount); err != nil {
			return fmt.Errorf("batch upsert daily sale for store %d unit %d: %w", input.StoreID, input.UnitID, err)
		}
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit daily sales batch transaction: %w", err)
	}
	return nil
}

func (r *Repository) ListTraffic(ctx context.Context, filter TrafficFilter) ([]CustomerTraffic, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, store_id, traffic_date, inbound_count, created_at, updated_at
		FROM customer_traffic
		WHERE (? IS NULL OR store_id = ?)
		  AND (? IS NULL OR traffic_date >= ?)
		  AND (? IS NULL OR traffic_date <= ?)
		ORDER BY traffic_date DESC, id DESC
		LIMIT ? OFFSET ?
	`, int64PointerValue(filter.StoreID), int64PointerValue(filter.StoreID), timePointerValue(filter.DateFrom), timePointerValue(filter.DateFrom), timePointerValue(filter.DateTo), timePointerValue(filter.DateTo), normalizeLimit(filter.Limit), normalizeOffset(filter.Offset))
	if err != nil {
		return nil, fmt.Errorf("list customer traffic: %w", err)
	}
	defer rows.Close()

	items := make([]CustomerTraffic, 0)
	for rows.Next() {
		var item CustomerTraffic
		if err := rows.Scan(&item.ID, &item.StoreID, &item.TrafficDate, &item.InboundCount, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan customer traffic: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate customer traffic: %w", err)
	}
	return items, nil
}

func (r *Repository) CreateTraffic(ctx context.Context, input CreateTrafficInput) (*CustomerTraffic, error) {
	trafficDate := input.TrafficDate.Format("2006-01-02")
	if _, err := r.db.ExecContext(ctx, `
		INSERT INTO customer_traffic (store_id, traffic_date, inbound_count)
		VALUES (?, ?, ?)
		ON DUPLICATE KEY UPDATE inbound_count = VALUES(inbound_count)
	`, input.StoreID, trafficDate, input.InboundCount); err != nil {
		return nil, fmt.Errorf("upsert customer traffic: %w", err)
	}
	return r.findTrafficByNaturalKey(ctx, input.StoreID, trafficDate)
}

func (r *Repository) BatchUpsertTraffic(ctx context.Context, inputs []BatchTrafficInput) error {
	if len(inputs) == 0 {
		return nil
	}
	tx, err := r.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin customer traffic batch transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	for _, input := range inputs {
		if _, err := tx.ExecContext(ctx, `
			INSERT INTO customer_traffic (store_id, traffic_date, inbound_count)
			VALUES (?, ?, ?)
			ON DUPLICATE KEY UPDATE inbound_count = VALUES(inbound_count)
		`, input.StoreID, input.TrafficDate.Format("2006-01-02"), input.InboundCount); err != nil {
			return fmt.Errorf("batch upsert customer traffic for store %d: %w", input.StoreID, err)
		}
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit customer traffic batch transaction: %w", err)
	}
	return nil
}

func (r *Repository) findDailySaleByNaturalKey(ctx context.Context, storeID, unitID int64, saleDate string) (*DailySale, error) {
	var item DailySale
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, store_id, unit_id, sale_date, sales_amount, created_at, updated_at
		FROM daily_shop_sales
		WHERE store_id = ? AND unit_id = ? AND sale_date = ?
	`, storeID, unitID, saleDate).Scan(&item.ID, &item.StoreID, &item.UnitID, &item.SaleDate, &item.SalesAmount, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, fmt.Errorf("find daily sale: %w", err)
	}
	return &item, nil
}

func (r *Repository) findTrafficByNaturalKey(ctx context.Context, storeID int64, trafficDate string) (*CustomerTraffic, error) {
	var item CustomerTraffic
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, store_id, traffic_date, inbound_count, created_at, updated_at
		FROM customer_traffic
		WHERE store_id = ? AND traffic_date = ?
	`, storeID, trafficDate).Scan(&item.ID, &item.StoreID, &item.TrafficDate, &item.InboundCount, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, fmt.Errorf("find customer traffic: %w", err)
	}
	return &item, nil
}

type Service struct{ repo *Repository }

func NewService(repo *Repository) *Service { return &Service{repo: repo} }

func (s *Service) ListDailySales(ctx context.Context, filter DailySaleFilter) ([]DailySale, error) {
	return s.repo.ListDailySales(ctx, filter)
}

func (s *Service) CreateDailySale(ctx context.Context, input CreateDailySaleInput) (*DailySale, error) {
	if input.StoreID == 0 || input.UnitID == 0 || input.SaleDate.IsZero() {
		return nil, fmt.Errorf("invalid daily sale input")
	}
	return s.repo.CreateDailySale(ctx, input)
}

func (s *Service) BatchUpsertDailySales(ctx context.Context, inputs []BatchDailySaleInput) (int, error) {
	for _, input := range inputs {
		if input.StoreID == 0 || input.UnitID == 0 || input.SaleDate.IsZero() {
			return 0, fmt.Errorf("invalid daily sale input")
		}
	}
	if err := s.repo.BatchUpsertDailySales(ctx, inputs); err != nil {
		return 0, err
	}
	return len(inputs), nil
}

func (s *Service) ListTraffic(ctx context.Context, filter TrafficFilter) ([]CustomerTraffic, error) {
	return s.repo.ListTraffic(ctx, filter)
}

func (s *Service) CreateTraffic(ctx context.Context, input CreateTrafficInput) (*CustomerTraffic, error) {
	if input.StoreID == 0 || input.TrafficDate.IsZero() {
		return nil, fmt.Errorf("invalid customer traffic input")
	}
	return s.repo.CreateTraffic(ctx, input)
}

func (s *Service) BatchUpsertTraffic(ctx context.Context, inputs []BatchTrafficInput) (int, error) {
	for _, input := range inputs {
		if input.StoreID == 0 || input.TrafficDate.IsZero() {
			return 0, fmt.Errorf("invalid customer traffic input")
		}
	}
	if err := s.repo.BatchUpsertTraffic(ctx, inputs); err != nil {
		return 0, err
	}
	return len(inputs), nil
}

func normalizeLimit(limit int) int {
	if limit <= 0 {
		return 100
	}
	if limit > 500 {
		return 500
	}
	return limit
}

func normalizeOffset(offset int) int {
	if offset < 0 {
		return 0
	}
	return offset
}

func int64PointerValue(value *int64) any {
	if value == nil {
		return nil
	}
	return *value
}

func timePointerValue(value *time.Time) any {
	if value == nil {
		return nil
	}
	return value.Format("2006-01-02")
}
