package baseinfo

import (
	"context"
	"database/sql"
	"errors"
	"fmt"
	"strings"
	"time"

	mysql "github.com/go-sql-driver/mysql"

	"github.com/Gujiaweiguo/mi/backend/internal/sqlutil"
)

var (
	ErrDuplicateCode     = errors.New("base info code already exists")
	ErrInvalidBaseInfo   = errors.New("invalid base info input")
	ErrReferenceNotFound = errors.New("base info record not found")
)

type ReferenceCatalogItem struct {
	ID        int64     `json:"id"`
	Code      string    `json:"code"`
	Name      string    `json:"name"`
	Status    string    `json:"status,omitempty"`
	ColorHex  *string   `json:"color_hex,omitempty"`
	IsLocal   *bool     `json:"is_local,omitempty"`
	ParentID  *int64    `json:"parent_id,omitempty"`
	Level     *int      `json:"level,omitempty"`
	CreatedAt time.Time `json:"created_at"`
	UpdatedAt time.Time `json:"updated_at"`
}

type CatalogInput struct {
	Code     string
	Name     string
	Status   string
	ColorHex *string
	IsLocal  *bool
	ParentID *int64
	Level    *int
}

type entityConfig struct {
	table     string
	orderBy   string
	hasStatus bool
	hasColor  bool
	hasLocal  bool
	hasParent bool
	hasLevel  bool
}

var (
	storeTypesConfig           = entityConfig{table: "store_types", orderBy: "id"}
	storeManagementTypesConfig = entityConfig{table: "store_management_types", orderBy: "id", hasStatus: true}
	areaLevelsConfig           = entityConfig{table: "area_levels", orderBy: "id"}
	unitTypesConfig            = entityConfig{table: "unit_types", orderBy: "id", hasStatus: true}
	shopTypesConfig            = entityConfig{table: "shop_types", orderBy: "id", hasStatus: true, hasColor: true}
	currencyTypesConfig        = entityConfig{table: "currency_types", orderBy: "id", hasStatus: true, hasLocal: true}
	tradeDefinitionsConfig     = entityConfig{table: "trade_definitions", orderBy: "level, code, id", hasStatus: true, hasParent: true, hasLevel: true}
)

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

func (s *Service) ListStoreTypes(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, storeTypesConfig)
}

func (s *Service) CreateStoreType(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, storeTypesConfig, input)
}

func (s *Service) UpdateStoreType(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, storeTypesConfig, id, input)
}

func (s *Service) ListStoreManagementTypes(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, storeManagementTypesConfig)
}

func (s *Service) CreateStoreManagementType(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, storeManagementTypesConfig, input)
}

func (s *Service) UpdateStoreManagementType(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, storeManagementTypesConfig, id, input)
}

func (s *Service) ListAreaLevels(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, areaLevelsConfig)
}

func (s *Service) CreateAreaLevel(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, areaLevelsConfig, input)
}

func (s *Service) UpdateAreaLevel(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, areaLevelsConfig, id, input)
}

func (s *Service) ListUnitTypes(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, unitTypesConfig)
}

func (s *Service) CreateUnitType(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, unitTypesConfig, input)
}

func (s *Service) UpdateUnitType(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, unitTypesConfig, id, input)
}

func (s *Service) ListShopTypes(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, shopTypesConfig)
}

func (s *Service) CreateShopType(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, shopTypesConfig, input)
}

func (s *Service) UpdateShopType(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, shopTypesConfig, id, input)
}

func (s *Service) ListCurrencyTypes(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, currencyTypesConfig)
}

func (s *Service) CreateCurrencyType(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, currencyTypesConfig, input)
}

func (s *Service) UpdateCurrencyType(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, currencyTypesConfig, id, input)
}

func (s *Service) ListTradeDefinitions(ctx context.Context) ([]ReferenceCatalogItem, error) {
	return s.repository.List(ctx, tradeDefinitionsConfig)
}

func (s *Service) CreateTradeDefinition(ctx context.Context, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.create(ctx, tradeDefinitionsConfig, input)
}

func (s *Service) UpdateTradeDefinition(ctx context.Context, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	return s.update(ctx, tradeDefinitionsConfig, id, input)
}

func (s *Service) create(ctx context.Context, config entityConfig, input CatalogInput) (*ReferenceCatalogItem, error) {
	normalized, err := normalizeInput(config, input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.Create(ctx, config, normalized)
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	return item, err
}

func (s *Service) update(ctx context.Context, config entityConfig, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	if id <= 0 {
		return nil, ErrInvalidBaseInfo
	}
	normalized, err := normalizeInput(config, input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.Update(ctx, config, id, normalized)
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	if errors.Is(err, sql.ErrNoRows) {
		return nil, ErrReferenceNotFound
	}
	return item, err
}

func normalizeInput(config entityConfig, input CatalogInput) (CatalogInput, error) {
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	if input.Code == "" || input.Name == "" {
		return CatalogInput{}, ErrInvalidBaseInfo
	}
	if config.hasStatus {
		input.Status = strings.TrimSpace(input.Status)
		if input.Status == "" {
			input.Status = "active"
		}
	} else {
		input.Status = ""
	}
	if config.hasColor {
		input.ColorHex = trimStringPointer(input.ColorHex)
	} else {
		input.ColorHex = nil
	}
	if !config.hasLocal {
		input.IsLocal = nil
	}
	if config.hasParent {
		if input.ParentID != nil && *input.ParentID <= 0 {
			return CatalogInput{}, ErrInvalidBaseInfo
		}
	} else {
		input.ParentID = nil
	}
	if config.hasLevel {
		if input.Level == nil {
			defaultLevel := 1
			input.Level = &defaultLevel
		}
		if *input.Level < 1 {
			return CatalogInput{}, ErrInvalidBaseInfo
		}
	} else {
		input.Level = nil
	}
	return input, nil
}

func trimStringPointer(value *string) *string {
	if value == nil {
		return nil
	}
	trimmed := strings.TrimSpace(*value)
	if trimmed == "" {
		return nil
	}
	return &trimmed
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

func (r *Repository) List(ctx context.Context, config entityConfig) ([]ReferenceCatalogItem, error) {
	query := fmt.Sprintf(`
		SELECT %s
		FROM %s ORDER BY %s
	`, selectColumns(config), config.table, config.orderBy)
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	items := make([]ReferenceCatalogItem, 0)
	for rows.Next() {
		item, err := scanReferenceCatalogItem(rows, config)
		if err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) Create(ctx context.Context, config entityConfig, input CatalogInput) (*ReferenceCatalogItem, error) {
	query, args := buildInsertQuery(config, input)
	result, err := r.db.ExecContext(ctx, query, args...)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.FindByID(ctx, config, id)
}

func (r *Repository) Update(ctx context.Context, config entityConfig, id int64, input CatalogInput) (*ReferenceCatalogItem, error) {
	query, args := buildUpdateQuery(config, id, input)
	result, err := r.db.ExecContext(ctx, query, args...)
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
	return r.FindByID(ctx, config, id)
}

func (r *Repository) FindByID(ctx context.Context, config entityConfig, id int64) (*ReferenceCatalogItem, error) {
	query := fmt.Sprintf(`
		SELECT %s
		FROM %s WHERE id = ?
	`, selectColumns(config), config.table)
	row := r.db.QueryRowContext(ctx, query, id)
	item, err := scanReferenceCatalogItem(row, config)
	if err != nil {
		return nil, err
	}
	return &item, nil
}

func buildInsertQuery(config entityConfig, input CatalogInput) (string, []any) {
	columns := []string{"code", "name"}
	placeholders := []string{"?", "?"}
	args := []any{input.Code, input.Name}
	if config.hasColor {
		columns = append(columns, "color_hex")
		placeholders = append(placeholders, "?")
		args = append(args, sqlutil.StringPointerValue(input.ColorHex))
	}
	if config.hasLocal {
		columns = append(columns, "is_local")
		placeholders = append(placeholders, "?")
		args = append(args, sqlutil.BoolPointerValue(input.IsLocal))
	}
	if config.hasParent {
		columns = append(columns, "parent_id")
		placeholders = append(placeholders, "?")
		args = append(args, sqlutil.Int64PointerValue(input.ParentID))
	}
	if config.hasLevel {
		columns = append(columns, "level")
		placeholders = append(placeholders, "?")
		args = append(args, sqlutil.IntPointerValue(input.Level))
	}
	if config.hasStatus {
		columns = append(columns, "status")
		placeholders = append(placeholders, "?")
		args = append(args, input.Status)
	}
	query := fmt.Sprintf("INSERT INTO %s (%s) VALUES (%s)", config.table, strings.Join(columns, ", "), strings.Join(placeholders, ", "))
	return query, args
}

func buildUpdateQuery(config entityConfig, id int64, input CatalogInput) (string, []any) {
	assignments := []string{"code = ?", "name = ?"}
	args := []any{input.Code, input.Name}
	if config.hasColor {
		assignments = append(assignments, "color_hex = ?")
		args = append(args, sqlutil.StringPointerValue(input.ColorHex))
	}
	if config.hasLocal {
		assignments = append(assignments, "is_local = ?")
		args = append(args, sqlutil.BoolPointerValue(input.IsLocal))
	}
	if config.hasParent {
		assignments = append(assignments, "parent_id = ?")
		args = append(args, sqlutil.Int64PointerValue(input.ParentID))
	}
	if config.hasLevel {
		assignments = append(assignments, "level = ?")
		args = append(args, sqlutil.IntPointerValue(input.Level))
	}
	if config.hasStatus {
		assignments = append(assignments, "status = ?")
		args = append(args, input.Status)
	}
	args = append(args, id)
	query := fmt.Sprintf("UPDATE %s SET %s WHERE id = ?", config.table, strings.Join(assignments, ", "))
	return query, args
}

func selectColumns(config entityConfig) string {
	columns := []string{"id", "code", "name"}
	if config.hasStatus {
		columns = append(columns, "status")
	} else {
		columns = append(columns, "NULL AS status")
	}
	if config.hasColor {
		columns = append(columns, "color_hex")
	} else {
		columns = append(columns, "NULL AS color_hex")
	}
	if config.hasLocal {
		columns = append(columns, "is_local")
	} else {
		columns = append(columns, "NULL AS is_local")
	}
	if config.hasParent {
		columns = append(columns, "parent_id")
	} else {
		columns = append(columns, "NULL AS parent_id")
	}
	if config.hasLevel {
		columns = append(columns, "level")
	} else {
		columns = append(columns, "NULL AS level")
	}
	columns = append(columns, "created_at", "updated_at")
	return strings.Join(columns, ", ")
}

type scanner interface {
	Scan(dest ...any) error
}

func scanReferenceCatalogItem(target scanner, config entityConfig) (ReferenceCatalogItem, error) {
	var item ReferenceCatalogItem
	var status sql.NullString
	var colorHex sql.NullString
	var isLocal sql.NullBool
	var parentID sql.NullInt64
	var level sql.NullInt64
	if err := target.Scan(&item.ID, &item.Code, &item.Name, &status, &colorHex, &isLocal, &parentID, &level, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return ReferenceCatalogItem{}, err
	}
	if config.hasStatus && status.Valid {
		item.Status = status.String
	}
	if config.hasColor && colorHex.Valid {
		item.ColorHex = &colorHex.String
	}
	if config.hasLocal && isLocal.Valid {
		value := isLocal.Bool
		item.IsLocal = &value
	}
	if config.hasParent && parentID.Valid {
		value := parentID.Int64
		item.ParentID = &value
	}
	if config.hasLevel && level.Valid {
		value := int(level.Int64)
		item.Level = &value
	}
	return item, nil
}


