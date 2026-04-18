package masterdata

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"strings"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	mysql "github.com/go-sql-driver/mysql"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

var (
	ErrDuplicateCode      = errors.New("master data code already exists")
	ErrInvalidMasterData  = errors.New("invalid master data input")
	ErrMasterDataNotFound = errors.New("master data record not found")
)

type ListFilter struct {
	Query    string
	Page     int
	PageSize int
}

type Customer struct {
	ID           int64     `json:"id"`
	Code         string    `json:"code"`
	Name         string    `json:"name"`
	TradeID      *int64    `json:"trade_id,omitempty"`
	DepartmentID *int64    `json:"department_id,omitempty"`
	Status       string    `json:"status"`
	CreatedAt    time.Time `json:"created_at"`
	UpdatedAt    time.Time `json:"updated_at"`
}

type Brand struct {
	ID        int64     `json:"id"`
	Code      string    `json:"code"`
	Name      string    `json:"name"`
	Status    string    `json:"status"`
	CreatedAt time.Time `json:"created_at"`
	UpdatedAt time.Time `json:"updated_at"`
}

type CreateCustomerInput struct {
	Code         string
	Name         string
	TradeID      *int64
	DepartmentID *int64
	Status       string
}

type CreateBrandInput struct {
	Code   string
	Name   string
	Status string
}

type UpdateCustomerInput struct {
	ID           int64
	Code         string
	Name         string
	TradeID      *int64
	DepartmentID *int64
	Status       string
}

type UpdateBrandInput struct {
	ID     int64
	Code   string
	Name   string
	Status string
}

type UnitRentBudget struct {
	UnitID      int64     `json:"unit_id"`
	FiscalYear  int       `json:"fiscal_year"`
	BudgetPrice float64   `json:"budget_price"`
	CreatedAt   time.Time `json:"created_at"`
	UpdatedAt   time.Time `json:"updated_at"`
}

type StoreRentBudget struct {
	StoreID       int64     `json:"store_id"`
	FiscalYear    int       `json:"fiscal_year"`
	FiscalMonth   int       `json:"fiscal_month"`
	MonthlyBudget float64   `json:"monthly_budget"`
	CreatedAt     time.Time `json:"created_at"`
	UpdatedAt     time.Time `json:"updated_at"`
}

type UnitProspect struct {
	UnitID              int64     `json:"unit_id"`
	FiscalYear          int       `json:"fiscal_year"`
	PotentialCustomerID *int64    `json:"potential_customer_id,omitempty"`
	ProspectBrandID     *int64    `json:"prospect_brand_id,omitempty"`
	ProspectTradeID     *int64    `json:"prospect_trade_id,omitempty"`
	AvgTransaction      *float64  `json:"avg_transaction,omitempty"`
	ProspectRentPrice   *float64  `json:"prospect_rent_price,omitempty"`
	RentIncrement       *string   `json:"rent_increment,omitempty"`
	ProspectTermMonths  *int      `json:"prospect_term_months,omitempty"`
	CreatedAt           time.Time `json:"created_at"`
	UpdatedAt           time.Time `json:"updated_at"`
}

type UpsertUnitRentBudgetInput struct {
	UnitID      int64
	FiscalYear  int
	BudgetPrice float64
}

type UpsertStoreRentBudgetInput struct {
	StoreID       int64
	FiscalYear    int
	FiscalMonth   int
	MonthlyBudget float64
}

type UpsertUnitProspectInput struct {
	UnitID              int64
	FiscalYear          int
	PotentialCustomerID *int64
	ProspectBrandID     *int64
	ProspectTradeID     *int64
	AvgTransaction      *float64
	ProspectRentPrice   *float64
	RentIncrement       *string
	ProspectTermMonths  *int
}

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

func (s *Service) CreateCustomer(ctx context.Context, input CreateCustomerInput) (*Customer, error) {
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	if input.Code == "" || input.Name == "" {
		return nil, ErrInvalidMasterData
	}
	if strings.TrimSpace(input.Status) == "" {
		input.Status = "active"
	}
	item, err := s.repository.CreateCustomer(ctx, input)
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	return item, err
}

func (s *Service) CreateBrand(ctx context.Context, input CreateBrandInput) (*Brand, error) {
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	if input.Code == "" || input.Name == "" {
		return nil, ErrInvalidMasterData
	}
	if strings.TrimSpace(input.Status) == "" {
		input.Status = "active"
	}
	item, err := s.repository.CreateBrand(ctx, input)
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	return item, err
}

func (s *Service) UpdateCustomer(ctx context.Context, input UpdateCustomerInput) (*Customer, error) {
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.Status = normalizeStatus(input.Status)
	if input.ID <= 0 || input.Code == "" || input.Name == "" {
		return nil, ErrInvalidMasterData
	}
	item, err := s.repository.UpdateCustomer(ctx, input)
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	if errors.Is(err, sql.ErrNoRows) {
		return nil, ErrMasterDataNotFound
	}
	return item, err
}

func (s *Service) UpdateBrand(ctx context.Context, input UpdateBrandInput) (*Brand, error) {
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.Status = normalizeStatus(input.Status)
	if input.ID <= 0 || input.Code == "" || input.Name == "" {
		return nil, ErrInvalidMasterData
	}
	item, err := s.repository.UpdateBrand(ctx, input)
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	if errors.Is(err, sql.ErrNoRows) {
		return nil, ErrMasterDataNotFound
	}
	return item, err
}

func (s *Service) ListCustomers(ctx context.Context, filter ListFilter) (*pagination.ListResult[Customer], error) {
	return s.repository.ListCustomers(ctx, normalizeListFilter(filter))
}
func (s *Service) ListBrands(ctx context.Context, filter ListFilter) (*pagination.ListResult[Brand], error) {
	return s.repository.ListBrands(ctx, normalizeListFilter(filter))
}

func (s *Service) ListUnitRentBudgets(ctx context.Context) ([]UnitRentBudget, error) {
	return s.repository.ListUnitRentBudgets(ctx)
}

func (s *Service) UpsertUnitRentBudget(ctx context.Context, input UpsertUnitRentBudgetInput) (*UnitRentBudget, error) {
	if input.UnitID <= 0 || input.FiscalYear <= 0 || input.BudgetPrice < 0 {
		return nil, ErrInvalidMasterData
	}
	return s.repository.UpsertUnitRentBudget(ctx, input)
}

func (s *Service) ListStoreRentBudgets(ctx context.Context) ([]StoreRentBudget, error) {
	return s.repository.ListStoreRentBudgets(ctx)
}

func (s *Service) UpsertStoreRentBudget(ctx context.Context, input UpsertStoreRentBudgetInput) (*StoreRentBudget, error) {
	if input.StoreID <= 0 || input.FiscalYear <= 0 || input.FiscalMonth < 1 || input.FiscalMonth > 12 || input.MonthlyBudget < 0 {
		return nil, ErrInvalidMasterData
	}
	return s.repository.UpsertStoreRentBudget(ctx, input)
}

func (s *Service) ListUnitProspects(ctx context.Context) ([]UnitProspect, error) {
	return s.repository.ListUnitProspects(ctx)
}

func (s *Service) UpsertUnitProspect(ctx context.Context, input UpsertUnitProspectInput) (*UnitProspect, error) {
	if input.UnitID <= 0 || input.FiscalYear <= 0 {
		return nil, ErrInvalidMasterData
	}
	if input.ProspectTermMonths != nil && *input.ProspectTermMonths <= 0 {
		return nil, ErrInvalidMasterData
	}
	return s.repository.UpsertUnitProspect(ctx, input)
}

func isDuplicateEntry(err error) bool {
	if err == nil {
		return false
	}
	var mysqlErr *mysql.MySQLError
	return errors.As(err, &mysqlErr) && mysqlErr.Number == 1062
}

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

func (r *Repository) CreateCustomer(ctx context.Context, input CreateCustomerInput) (*Customer, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO customers (code, name, trade_id, department_id, status)
		VALUES (?, ?, ?, ?, ?)
	`, input.Code, input.Name, sqlutil.Int64PointerValue(input.TradeID), sqlutil.Int64PointerValue(input.DepartmentID), input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.FindCustomerByID(ctx, id)
}

func (r *Repository) CreateBrand(ctx context.Context, input CreateBrandInput) (*Brand, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO brands (code, name, status)
		VALUES (?, ?, ?)
	`, input.Code, input.Name, input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.FindBrandByID(ctx, id)
}

func (r *Repository) UpdateCustomer(ctx context.Context, input UpdateCustomerInput) (*Customer, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE customers
		SET code = ?, name = ?, trade_id = ?, department_id = ?, status = ?, updated_at = CURRENT_TIMESTAMP
		WHERE id = ?
	`, input.Code, input.Name, sqlutil.Int64PointerValue(input.TradeID), sqlutil.Int64PointerValue(input.DepartmentID), input.Status, input.ID)
	if err != nil {
		return nil, err
	}
	rowsAffected, err := result.RowsAffected()
	if err != nil {
		return nil, err
	}
	if rowsAffected == 0 {
		return nil, sql.ErrNoRows
	}
	return r.FindCustomerByID(ctx, input.ID)
}

func (r *Repository) UpdateBrand(ctx context.Context, input UpdateBrandInput) (*Brand, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE brands
		SET code = ?, name = ?, status = ?, updated_at = CURRENT_TIMESTAMP
		WHERE id = ?
	`, input.Code, input.Name, input.Status, input.ID)
	if err != nil {
		return nil, err
	}
	rowsAffected, err := result.RowsAffected()
	if err != nil {
		return nil, err
	}
	if rowsAffected == 0 {
		return nil, sql.ErrNoRows
	}
	return r.FindBrandByID(ctx, input.ID)
}

func (r *Repository) FindCustomerByID(ctx context.Context, id int64) (*Customer, error) {
	var item Customer
	var tradeID sql.NullInt64
	var departmentID sql.NullInt64
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, code, name, trade_id, department_id, status, created_at, updated_at
		FROM customers WHERE id = ?
	`, id).Scan(&item.ID, &item.Code, &item.Name, &tradeID, &departmentID, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	item.TradeID = sqlutil.NullInt64Pointer(tradeID)
	item.DepartmentID = sqlutil.NullInt64Pointer(departmentID)
	return &item, nil
}

func (r *Repository) FindBrandByID(ctx context.Context, id int64) (*Brand, error) {
	var item Brand
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, code, name, status, created_at, updated_at
		FROM brands WHERE id = ?
	`, id).Scan(&item.ID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) ListCustomers(ctx context.Context, filter ListFilter) (*pagination.ListResult[Customer], error) {
	whereClause, args := buildSearchClause(filter.Query)
	countQuery := fmt.Sprintf(`SELECT COUNT(*) FROM customers%s`, whereClause)
	var total int
	if err := r.db.QueryRowContext(ctx, countQuery, args...).Scan(&total); err != nil {
		return nil, err
	}

	listQuery := fmt.Sprintf(`
		SELECT id, code, name, trade_id, department_id, status, created_at, updated_at
		FROM customers%s
		ORDER BY updated_at DESC, id DESC
		LIMIT ? OFFSET ?
	`, whereClause)
	listArgs := append(args, filter.PageSize, (filter.Page-1)*filter.PageSize)
	rows, err := r.db.QueryContext(ctx, listQuery, listArgs...)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	items := make([]Customer, 0)
	for rows.Next() {
		var item Customer
		var tradeID sql.NullInt64
		var departmentID sql.NullInt64
		if err := rows.Scan(&item.ID, &item.Code, &item.Name, &tradeID, &departmentID, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, err
		}
		item.TradeID = sqlutil.NullInt64Pointer(tradeID)
		item.DepartmentID = sqlutil.NullInt64Pointer(departmentID)
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, err
	}
	return &pagination.ListResult[Customer]{Items: items, Total: int64(total), Page: filter.Page, PageSize: filter.PageSize}, nil
}

func (r *Repository) ListBrands(ctx context.Context, filter ListFilter) (*pagination.ListResult[Brand], error) {
	whereClause, args := buildSearchClause(filter.Query)
	countQuery := fmt.Sprintf(`SELECT COUNT(*) FROM brands%s`, whereClause)
	var total int
	if err := r.db.QueryRowContext(ctx, countQuery, args...).Scan(&total); err != nil {
		return nil, err
	}

	listQuery := fmt.Sprintf(`
		SELECT id, code, name, status, created_at, updated_at
		FROM brands%s
		ORDER BY updated_at DESC, id DESC
		LIMIT ? OFFSET ?
	`, whereClause)
	listArgs := append(args, filter.PageSize, (filter.Page-1)*filter.PageSize)
	rows, err := r.db.QueryContext(ctx, listQuery, listArgs...)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	items := make([]Brand, 0)
	for rows.Next() {
		var item Brand
		if err := rows.Scan(&item.ID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, err
	}
	return &pagination.ListResult[Brand]{Items: items, Total: int64(total), Page: filter.Page, PageSize: filter.PageSize}, nil
}

func (r *Repository) ListUnitRentBudgets(ctx context.Context) ([]UnitRentBudget, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT unit_id, fiscal_year, budget_price, created_at, updated_at
		FROM unit_rent_budgets
		ORDER BY fiscal_year DESC, unit_id ASC
	`)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	items := make([]UnitRentBudget, 0)
	for rows.Next() {
		var item UnitRentBudget
		if err := rows.Scan(&item.UnitID, &item.FiscalYear, &item.BudgetPrice, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) UpsertUnitRentBudget(ctx context.Context, input UpsertUnitRentBudgetInput) (*UnitRentBudget, error) {
	_, err := r.db.ExecContext(ctx, `
		INSERT INTO unit_rent_budgets (unit_id, fiscal_year, budget_price)
		VALUES (?, ?, ?)
		ON DUPLICATE KEY UPDATE budget_price = VALUES(budget_price), updated_at = CURRENT_TIMESTAMP
	`, input.UnitID, input.FiscalYear, input.BudgetPrice)
	if err != nil {
		return nil, err
	}
	return r.FindUnitRentBudget(ctx, input.UnitID, input.FiscalYear)
}

func (r *Repository) FindUnitRentBudget(ctx context.Context, unitID int64, fiscalYear int) (*UnitRentBudget, error) {
	var item UnitRentBudget
	if err := r.db.QueryRowContext(ctx, `
		SELECT unit_id, fiscal_year, budget_price, created_at, updated_at
		FROM unit_rent_budgets WHERE unit_id = ? AND fiscal_year = ?
	`, unitID, fiscalYear).Scan(&item.UnitID, &item.FiscalYear, &item.BudgetPrice, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) ListStoreRentBudgets(ctx context.Context) ([]StoreRentBudget, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT store_id, fiscal_year, fiscal_month, monthly_budget, created_at, updated_at
		FROM store_rent_budgets
		ORDER BY fiscal_year DESC, fiscal_month DESC, store_id ASC
	`)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	items := make([]StoreRentBudget, 0)
	for rows.Next() {
		var item StoreRentBudget
		if err := rows.Scan(&item.StoreID, &item.FiscalYear, &item.FiscalMonth, &item.MonthlyBudget, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) UpsertStoreRentBudget(ctx context.Context, input UpsertStoreRentBudgetInput) (*StoreRentBudget, error) {
	_, err := r.db.ExecContext(ctx, `
		INSERT INTO store_rent_budgets (store_id, fiscal_year, fiscal_month, monthly_budget)
		VALUES (?, ?, ?, ?)
		ON DUPLICATE KEY UPDATE monthly_budget = VALUES(monthly_budget), updated_at = CURRENT_TIMESTAMP
	`, input.StoreID, input.FiscalYear, input.FiscalMonth, input.MonthlyBudget)
	if err != nil {
		return nil, err
	}
	return r.FindStoreRentBudget(ctx, input.StoreID, input.FiscalYear, input.FiscalMonth)
}

func (r *Repository) FindStoreRentBudget(ctx context.Context, storeID int64, fiscalYear int, fiscalMonth int) (*StoreRentBudget, error) {
	var item StoreRentBudget
	if err := r.db.QueryRowContext(ctx, `
		SELECT store_id, fiscal_year, fiscal_month, monthly_budget, created_at, updated_at
		FROM store_rent_budgets WHERE store_id = ? AND fiscal_year = ? AND fiscal_month = ?
	`, storeID, fiscalYear, fiscalMonth).Scan(&item.StoreID, &item.FiscalYear, &item.FiscalMonth, &item.MonthlyBudget, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) ListUnitProspects(ctx context.Context) ([]UnitProspect, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id, avg_transaction, prospect_rent_price, rent_increment, prospect_term_months, created_at, updated_at
		FROM unit_prospects
		ORDER BY fiscal_year DESC, unit_id ASC
	`)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	items := make([]UnitProspect, 0)
	for rows.Next() {
		item, err := scanUnitProspect(rows)
		if err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) UpsertUnitProspect(ctx context.Context, input UpsertUnitProspectInput) (*UnitProspect, error) {
	_, err := r.db.ExecContext(ctx, `
		INSERT INTO unit_prospects (unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id, avg_transaction, prospect_rent_price, rent_increment, prospect_term_months)
		VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
		ON DUPLICATE KEY UPDATE
		  potential_customer_id = VALUES(potential_customer_id),
		  prospect_brand_id = VALUES(prospect_brand_id),
		  prospect_trade_id = VALUES(prospect_trade_id),
		  avg_transaction = VALUES(avg_transaction),
		  prospect_rent_price = VALUES(prospect_rent_price),
		  rent_increment = VALUES(rent_increment),
		  prospect_term_months = VALUES(prospect_term_months),
		  updated_at = CURRENT_TIMESTAMP
	`, input.UnitID, input.FiscalYear, sqlutil.Int64PointerValue(input.PotentialCustomerID), sqlutil.Int64PointerValue(input.ProspectBrandID), sqlutil.Int64PointerValue(input.ProspectTradeID), sqlutil.Float64PointerValue(input.AvgTransaction), sqlutil.Float64PointerValue(input.ProspectRentPrice), sqlutil.StringPointerValue(input.RentIncrement), sqlutil.IntPointerValue(input.ProspectTermMonths))
	if err != nil {
		return nil, err
	}
	return r.FindUnitProspect(ctx, input.UnitID, input.FiscalYear)
}

func (r *Repository) FindUnitProspect(ctx context.Context, unitID int64, fiscalYear int) (*UnitProspect, error) {
	row := r.db.QueryRowContext(ctx, `
		SELECT unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id, avg_transaction, prospect_rent_price, rent_increment, prospect_term_months, created_at, updated_at
		FROM unit_prospects WHERE unit_id = ? AND fiscal_year = ?
	`, unitID, fiscalYear)
	item, err := scanUnitProspect(row)
	if err != nil {
		return nil, err
	}
	return &item, nil
}

type scanner interface{ Scan(dest ...any) error }

func scanUnitProspect(row scanner) (UnitProspect, error) {
	var item UnitProspect
	var potentialCustomerID sql.NullInt64
	var prospectBrandID sql.NullInt64
	var prospectTradeID sql.NullInt64
	var avgTransaction sql.NullFloat64
	var prospectRentPrice sql.NullFloat64
	var rentIncrement sql.NullString
	var prospectTermMonths sql.NullInt64
	err := row.Scan(
		&item.UnitID,
		&item.FiscalYear,
		&potentialCustomerID,
		&prospectBrandID,
		&prospectTradeID,
		&avgTransaction,
		&prospectRentPrice,
		&rentIncrement,
		&prospectTermMonths,
		&item.CreatedAt,
		&item.UpdatedAt,
	)
	if err != nil {
		return UnitProspect{}, err
	}
	item.PotentialCustomerID = sqlutil.NullInt64Pointer(potentialCustomerID)
	item.ProspectBrandID = sqlutil.NullInt64Pointer(prospectBrandID)
	item.ProspectTradeID = sqlutil.NullInt64Pointer(prospectTradeID)
	item.AvgTransaction = sqlutil.NullFloat64Pointer(avgTransaction)
	item.ProspectRentPrice = sqlutil.NullFloat64Pointer(prospectRentPrice)
	item.RentIncrement = sqlutil.NullStringPointer(rentIncrement)
	item.ProspectTermMonths = sqlutil.NullIntPointer(prospectTermMonths)
	return item, nil
}

func normalizeListFilter(filter ListFilter) ListFilter {
	filter.Query = strings.TrimSpace(filter.Query)
	filter.Page, filter.PageSize = pagination.NormalizePage(filter.Page, filter.PageSize)
	return filter
}

func normalizeStatus(status string) string {
	status = strings.TrimSpace(status)
	if status == "inactive" {
		return status
	}
	return "active"
}

func buildSearchClause(query string) (string, []any) {
	if query == "" {
		return "", nil
	}
	likeQuery := "%" + query + "%"
	return " WHERE code LIKE ? OR name LIKE ?", []any{likeQuery, likeQuery}
}


