package masterdata

import (
	"context"
	"database/sql"
	"errors"
	"strings"
	"time"

	mysql "github.com/go-sql-driver/mysql"
)

var (
	ErrDuplicateCode     = errors.New("master data code already exists")
	ErrInvalidMasterData = errors.New("invalid master data input")
)

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

func (s *Service) ListCustomers(ctx context.Context) ([]Customer, error) {
	return s.repository.ListCustomers(ctx)
}
func (s *Service) ListBrands(ctx context.Context) ([]Brand, error) {
	return s.repository.ListBrands(ctx)
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
	`, input.Code, input.Name, int64PointerValue(input.TradeID), int64PointerValue(input.DepartmentID), input.Status)
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
	item.TradeID = nullInt64Pointer(tradeID)
	item.DepartmentID = nullInt64Pointer(departmentID)
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

func (r *Repository) ListCustomers(ctx context.Context) ([]Customer, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, code, name, trade_id, department_id, status, created_at, updated_at
		FROM customers ORDER BY id
	`)
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
		item.TradeID = nullInt64Pointer(tradeID)
		item.DepartmentID = nullInt64Pointer(departmentID)
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) ListBrands(ctx context.Context) ([]Brand, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, code, name, status, created_at, updated_at
		FROM brands ORDER BY id
	`)
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
	return items, rows.Err()
}

func nullInt64Pointer(value sql.NullInt64) *int64 {
	if !value.Valid {
		return nil
	}
	v := value.Int64
	return &v
}

func int64PointerValue(value *int64) any {
	if value == nil {
		return nil
	}
	return *value
}
