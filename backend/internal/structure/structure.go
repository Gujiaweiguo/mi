package structure

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
	ErrDuplicateCode           = errors.New("structure code already exists")
	ErrInvalidStructure        = errors.New("invalid structure input")
	ErrReferenceNotFound       = errors.New("structure record not found")
	ErrParentReferenceNotFound = errors.New("structure parent reference not found")
)

type Store struct {
	ID               int64     `json:"id"`
	DepartmentID     int64     `json:"department_id"`
	StoreTypeID      int64     `json:"store_type_id"`
	ManagementTypeID int64     `json:"management_type_id"`
	Code             string    `json:"code"`
	Name             string    `json:"name"`
	ShortName        string    `json:"short_name"`
	Status           string    `json:"status"`
	CreatedAt        time.Time `json:"created_at"`
	UpdatedAt        time.Time `json:"updated_at"`
}

type Building struct {
	ID        int64     `json:"id"`
	StoreID   int64     `json:"store_id"`
	Code      string    `json:"code"`
	Name      string    `json:"name"`
	Status    string    `json:"status"`
	CreatedAt time.Time `json:"created_at"`
	UpdatedAt time.Time `json:"updated_at"`
}

type Floor struct {
	ID                int64     `json:"id"`
	BuildingID        int64     `json:"building_id"`
	Code              string    `json:"code"`
	Name              string    `json:"name"`
	Status            string    `json:"status"`
	FloorPlanImageURL *string   `json:"floor_plan_image_url,omitempty"`
	CreatedAt         time.Time `json:"created_at"`
	UpdatedAt         time.Time `json:"updated_at"`
}

type Area struct {
	ID          int64     `json:"id"`
	StoreID     int64     `json:"store_id"`
	AreaLevelID int64     `json:"area_level_id"`
	Code        string    `json:"code"`
	Name        string    `json:"name"`
	Status      string    `json:"status"`
	CreatedAt   time.Time `json:"created_at"`
	UpdatedAt   time.Time `json:"updated_at"`
}

type Location struct {
	ID        int64     `json:"id"`
	StoreID   int64     `json:"store_id"`
	FloorID   int64     `json:"floor_id"`
	Code      string    `json:"code"`
	Name      string    `json:"name"`
	Status    string    `json:"status"`
	CreatedAt time.Time `json:"created_at"`
	UpdatedAt time.Time `json:"updated_at"`
}

type Unit struct {
	ID         int64     `json:"id"`
	BuildingID int64     `json:"building_id"`
	FloorID    int64     `json:"floor_id"`
	LocationID int64     `json:"location_id"`
	AreaID     int64     `json:"area_id"`
	UnitTypeID int64     `json:"unit_type_id"`
	ShopTypeID *int64    `json:"shop_type_id"`
	Code       string    `json:"code"`
	FloorArea  float64   `json:"floor_area"`
	UseArea    float64   `json:"use_area"`
	RentArea   float64   `json:"rent_area"`
	IsRentable bool      `json:"is_rentable"`
	Status     string    `json:"status"`
	CreatedAt  time.Time `json:"created_at"`
	UpdatedAt  time.Time `json:"updated_at"`
}

type StoreInput struct {
	DepartmentID     int64
	StoreTypeID      int64
	ManagementTypeID int64
	Code             string
	Name             string
	ShortName        string
	Status           string
}

type BuildingInput struct {
	StoreID int64
	Code    string
	Name    string
	Status  string
}

type FloorInput struct {
	BuildingID        int64
	Code              string
	Name              string
	Status            string
	FloorPlanImageURL *string
}

type AreaInput struct {
	StoreID     int64
	AreaLevelID int64
	Code        string
	Name        string
	Status      string
}

type LocationInput struct {
	StoreID int64
	FloorID int64
	Code    string
	Name    string
	Status  string
}

type UnitInput struct {
	BuildingID int64
	FloorID    int64
	LocationID int64
	AreaID     int64
	UnitTypeID int64
	ShopTypeID *int64
	Code       string
	FloorArea  float64
	UseArea    float64
	RentArea   float64
	IsRentable bool
	Status     string
}

type BuildingFilter struct {
	StoreID *int64
}

type FloorFilter struct {
	BuildingID *int64
}

type AreaFilter struct {
	StoreID *int64
}

type LocationFilter struct {
	StoreID *int64
	FloorID *int64
}

type UnitFilter struct {
	BuildingID *int64
	FloorID    *int64
	LocationID *int64
	AreaID     *int64
}

type Service struct{ repository *Repository }

func NewService(repository *Repository) *Service { return &Service{repository: repository} }

func (s *Service) ListStores(ctx context.Context) ([]Store, error) {
	return s.repository.ListStores(ctx)
}

func (s *Service) CreateStore(ctx context.Context, input StoreInput) (*Store, error) {
	normalized, err := normalizeStoreInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.CreateStore(ctx, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) UpdateStore(ctx context.Context, id int64, input StoreInput) (*Store, error) {
	if id <= 0 {
		return nil, ErrInvalidStructure
	}
	normalized, err := normalizeStoreInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.UpdateStore(ctx, id, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) ListBuildings(ctx context.Context, filter BuildingFilter) ([]Building, error) {
	if filter.StoreID != nil && *filter.StoreID <= 0 {
		return nil, ErrInvalidStructure
	}
	return s.repository.ListBuildings(ctx, filter)
}

func (s *Service) CreateBuilding(ctx context.Context, input BuildingInput) (*Building, error) {
	normalized, err := normalizeBuildingInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.CreateBuilding(ctx, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) UpdateBuilding(ctx context.Context, id int64, input BuildingInput) (*Building, error) {
	if id <= 0 {
		return nil, ErrInvalidStructure
	}
	normalized, err := normalizeBuildingInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.UpdateBuilding(ctx, id, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) ListFloors(ctx context.Context, filter FloorFilter) ([]Floor, error) {
	if filter.BuildingID != nil && *filter.BuildingID <= 0 {
		return nil, ErrInvalidStructure
	}
	return s.repository.ListFloors(ctx, filter)
}

func (s *Service) CreateFloor(ctx context.Context, input FloorInput) (*Floor, error) {
	normalized, err := normalizeFloorInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.CreateFloor(ctx, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) UpdateFloor(ctx context.Context, id int64, input FloorInput) (*Floor, error) {
	if id <= 0 {
		return nil, ErrInvalidStructure
	}
	normalized, err := normalizeFloorInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.UpdateFloor(ctx, id, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) ListAreas(ctx context.Context, filter AreaFilter) ([]Area, error) {
	if filter.StoreID != nil && *filter.StoreID <= 0 {
		return nil, ErrInvalidStructure
	}
	return s.repository.ListAreas(ctx, filter)
}

func (s *Service) CreateArea(ctx context.Context, input AreaInput) (*Area, error) {
	normalized, err := normalizeAreaInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.CreateArea(ctx, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) UpdateArea(ctx context.Context, id int64, input AreaInput) (*Area, error) {
	if id <= 0 {
		return nil, ErrInvalidStructure
	}
	normalized, err := normalizeAreaInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.UpdateArea(ctx, id, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) ListLocations(ctx context.Context, filter LocationFilter) ([]Location, error) {
	if filter.StoreID != nil && *filter.StoreID <= 0 {
		return nil, ErrInvalidStructure
	}
	if filter.FloorID != nil && *filter.FloorID <= 0 {
		return nil, ErrInvalidStructure
	}
	return s.repository.ListLocations(ctx, filter)
}

func (s *Service) CreateLocation(ctx context.Context, input LocationInput) (*Location, error) {
	normalized, err := normalizeLocationInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.CreateLocation(ctx, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) UpdateLocation(ctx context.Context, id int64, input LocationInput) (*Location, error) {
	if id <= 0 {
		return nil, ErrInvalidStructure
	}
	normalized, err := normalizeLocationInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.UpdateLocation(ctx, id, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) ListUnits(ctx context.Context, filter UnitFilter) ([]Unit, error) {
	for _, value := range []*int64{filter.BuildingID, filter.FloorID, filter.LocationID, filter.AreaID} {
		if value != nil && *value <= 0 {
			return nil, ErrInvalidStructure
		}
	}
	return s.repository.ListUnits(ctx, filter)
}

func (s *Service) CreateUnit(ctx context.Context, input UnitInput) (*Unit, error) {
	normalized, err := normalizeUnitInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.CreateUnit(ctx, normalized)
	return mapRepositoryError(item, err)
}

func (s *Service) UpdateUnit(ctx context.Context, id int64, input UnitInput) (*Unit, error) {
	if id <= 0 {
		return nil, ErrInvalidStructure
	}
	normalized, err := normalizeUnitInput(input)
	if err != nil {
		return nil, err
	}
	item, err := s.repository.UpdateUnit(ctx, id, normalized)
	return mapRepositoryError(item, err)
}

func normalizeStoreInput(input StoreInput) (StoreInput, error) {
	if input.DepartmentID <= 0 || input.StoreTypeID <= 0 || input.ManagementTypeID <= 0 {
		return StoreInput{}, ErrInvalidStructure
	}
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.ShortName = strings.TrimSpace(input.ShortName)
	input.Status = normalizeStatus(input.Status)
	if input.Code == "" || input.Name == "" || input.ShortName == "" {
		return StoreInput{}, ErrInvalidStructure
	}
	return input, nil
}

func normalizeBuildingInput(input BuildingInput) (BuildingInput, error) {
	if input.StoreID <= 0 {
		return BuildingInput{}, ErrInvalidStructure
	}
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.Status = normalizeStatus(input.Status)
	if input.Code == "" || input.Name == "" {
		return BuildingInput{}, ErrInvalidStructure
	}
	return input, nil
}

func normalizeFloorInput(input FloorInput) (FloorInput, error) {
	if input.BuildingID <= 0 {
		return FloorInput{}, ErrInvalidStructure
	}
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.Status = normalizeStatus(input.Status)
	input.FloorPlanImageURL = trimStringPointer(input.FloorPlanImageURL)
	if input.Code == "" || input.Name == "" {
		return FloorInput{}, ErrInvalidStructure
	}
	return input, nil
}

func normalizeAreaInput(input AreaInput) (AreaInput, error) {
	if input.StoreID <= 0 || input.AreaLevelID <= 0 {
		return AreaInput{}, ErrInvalidStructure
	}
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.Status = normalizeStatus(input.Status)
	if input.Code == "" || input.Name == "" {
		return AreaInput{}, ErrInvalidStructure
	}
	return input, nil
}

func normalizeLocationInput(input LocationInput) (LocationInput, error) {
	if input.StoreID <= 0 || input.FloorID <= 0 {
		return LocationInput{}, ErrInvalidStructure
	}
	input.Code = strings.TrimSpace(input.Code)
	input.Name = strings.TrimSpace(input.Name)
	input.Status = normalizeStatus(input.Status)
	if input.Code == "" || input.Name == "" {
		return LocationInput{}, ErrInvalidStructure
	}
	return input, nil
}

func normalizeUnitInput(input UnitInput) (UnitInput, error) {
	if input.BuildingID <= 0 || input.FloorID <= 0 || input.LocationID <= 0 || input.AreaID <= 0 || input.UnitTypeID <= 0 {
		return UnitInput{}, ErrInvalidStructure
	}
	input.Code = strings.TrimSpace(input.Code)
	input.Status = normalizeStatus(input.Status)
	if input.Code == "" || input.FloorArea < 0 || input.UseArea < 0 || input.RentArea < 0 {
		return UnitInput{}, ErrInvalidStructure
	}
	return input, nil
}

func normalizeStatus(status string) string {
	status = strings.TrimSpace(status)
	if status == "" {
		return "active"
	}
	return status
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

func mapRepositoryError[T any](item *T, err error) (*T, error) {
	if err == nil {
		return item, nil
	}
	if errors.Is(err, sql.ErrNoRows) {
		return nil, ErrReferenceNotFound
	}
	if isDuplicateEntry(err) {
		return nil, ErrDuplicateCode
	}
	if isForeignKeyViolation(err) {
		return nil, ErrParentReferenceNotFound
	}
	return nil, err
}

func isDuplicateEntry(err error) bool {
	var mysqlErr *mysql.MySQLError
	return errors.As(err, &mysqlErr) && mysqlErr.Number == 1062
}

func isForeignKeyViolation(err error) bool {
	var mysqlErr *mysql.MySQLError
	return errors.As(err, &mysqlErr) && mysqlErr.Number == 1452
}

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

func (r *Repository) ListStores(ctx context.Context) ([]Store, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, department_id, store_type_id, management_type_id, code, name, short_name, status, created_at, updated_at
		FROM stores ORDER BY id
	`)
	if err != nil {
		return nil, fmt.Errorf("list stores: %w", err)
	}
	defer rows.Close()

	items := make([]Store, 0)
	for rows.Next() {
		var item Store
		if err := rows.Scan(&item.ID, &item.DepartmentID, &item.StoreTypeID, &item.ManagementTypeID, &item.Code, &item.Name, &item.ShortName, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan store: %w", err)
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) CreateStore(ctx context.Context, input StoreInput) (*Store, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO stores (department_id, store_type_id, management_type_id, code, name, short_name, status)
		VALUES (?, ?, ?, ?, ?, ?, ?)
	`, input.DepartmentID, input.StoreTypeID, input.ManagementTypeID, input.Code, input.Name, input.ShortName, input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.findStoreByID(ctx, id)
}

func (r *Repository) UpdateStore(ctx context.Context, id int64, input StoreInput) (*Store, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE stores
		SET department_id = ?, store_type_id = ?, management_type_id = ?, code = ?, name = ?, short_name = ?, status = ?
		WHERE id = ?
	`, input.DepartmentID, input.StoreTypeID, input.ManagementTypeID, input.Code, input.Name, input.ShortName, input.Status, id)
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
	return r.findStoreByID(ctx, id)
}

func (r *Repository) ListBuildings(ctx context.Context, filter BuildingFilter) ([]Building, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, store_id, code, name, status, created_at, updated_at
		FROM buildings
		WHERE (? IS NULL OR store_id = ?)
		ORDER BY store_id, id
	`, sqlutil.Int64PointerValue(filter.StoreID), sqlutil.Int64PointerValue(filter.StoreID))
	if err != nil {
		return nil, fmt.Errorf("list buildings: %w", err)
	}
	defer rows.Close()

	items := make([]Building, 0)
	for rows.Next() {
		var item Building
		if err := rows.Scan(&item.ID, &item.StoreID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan building: %w", err)
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) CreateBuilding(ctx context.Context, input BuildingInput) (*Building, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO buildings (store_id, code, name, status)
		VALUES (?, ?, ?, ?)
	`, input.StoreID, input.Code, input.Name, input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.findBuildingByID(ctx, id)
}

func (r *Repository) UpdateBuilding(ctx context.Context, id int64, input BuildingInput) (*Building, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE buildings SET store_id = ?, code = ?, name = ?, status = ? WHERE id = ?
	`, input.StoreID, input.Code, input.Name, input.Status, id)
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
	return r.findBuildingByID(ctx, id)
}

func (r *Repository) ListFloors(ctx context.Context, filter FloorFilter) ([]Floor, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, building_id, code, name, status, floor_plan_image_url, created_at, updated_at
		FROM floors
		WHERE (? IS NULL OR building_id = ?)
		ORDER BY building_id, id
	`, sqlutil.Int64PointerValue(filter.BuildingID), sqlutil.Int64PointerValue(filter.BuildingID))
	if err != nil {
		return nil, fmt.Errorf("list floors: %w", err)
	}
	defer rows.Close()

	items := make([]Floor, 0)
	for rows.Next() {
		item, err := scanFloor(rows)
		if err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) CreateFloor(ctx context.Context, input FloorInput) (*Floor, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO floors (building_id, code, name, status, floor_plan_image_url)
		VALUES (?, ?, ?, ?, ?)
	`, input.BuildingID, input.Code, input.Name, input.Status, sqlutil.StringPointerValue(input.FloorPlanImageURL))
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.findFloorByID(ctx, id)
}

func (r *Repository) UpdateFloor(ctx context.Context, id int64, input FloorInput) (*Floor, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE floors SET building_id = ?, code = ?, name = ?, status = ?, floor_plan_image_url = ? WHERE id = ?
	`, input.BuildingID, input.Code, input.Name, input.Status, sqlutil.StringPointerValue(input.FloorPlanImageURL), id)
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
	return r.findFloorByID(ctx, id)
}

func (r *Repository) findStoreByID(ctx context.Context, id int64) (*Store, error) {
	var item Store
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, department_id, store_type_id, management_type_id, code, name, short_name, status, created_at, updated_at
		FROM stores WHERE id = ?
	`, id).Scan(&item.ID, &item.DepartmentID, &item.StoreTypeID, &item.ManagementTypeID, &item.Code, &item.Name, &item.ShortName, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) findBuildingByID(ctx context.Context, id int64) (*Building, error) {
	var item Building
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, store_id, code, name, status, created_at, updated_at
		FROM buildings WHERE id = ?
	`, id).Scan(&item.ID, &item.StoreID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) findFloorByID(ctx context.Context, id int64) (*Floor, error) {
	row := r.db.QueryRowContext(ctx, `
		SELECT id, building_id, code, name, status, floor_plan_image_url, created_at, updated_at
		FROM floors WHERE id = ?
	`, id)
	item, err := scanFloor(row)
	if err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) ListAreas(ctx context.Context, filter AreaFilter) ([]Area, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, store_id, area_level_id, code, name, status, created_at, updated_at
		FROM areas
		WHERE (? IS NULL OR store_id = ?)
		ORDER BY store_id, id
	`, sqlutil.Int64PointerValue(filter.StoreID), sqlutil.Int64PointerValue(filter.StoreID))
	if err != nil {
		return nil, fmt.Errorf("list areas: %w", err)
	}
	defer rows.Close()

	items := make([]Area, 0)
	for rows.Next() {
		var item Area
		if err := rows.Scan(&item.ID, &item.StoreID, &item.AreaLevelID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan area: %w", err)
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) CreateArea(ctx context.Context, input AreaInput) (*Area, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO areas (store_id, area_level_id, code, name, status)
		VALUES (?, ?, ?, ?, ?)
	`, input.StoreID, input.AreaLevelID, input.Code, input.Name, input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.findAreaByID(ctx, id)
}

func (r *Repository) UpdateArea(ctx context.Context, id int64, input AreaInput) (*Area, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE areas SET store_id = ?, area_level_id = ?, code = ?, name = ?, status = ? WHERE id = ?
	`, input.StoreID, input.AreaLevelID, input.Code, input.Name, input.Status, id)
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
	return r.findAreaByID(ctx, id)
}

func (r *Repository) ListLocations(ctx context.Context, filter LocationFilter) ([]Location, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, store_id, floor_id, code, name, status, created_at, updated_at
		FROM locations
		WHERE (? IS NULL OR store_id = ?)
		  AND (? IS NULL OR floor_id = ?)
		ORDER BY store_id, floor_id, id
	`,
		sqlutil.Int64PointerValue(filter.StoreID), sqlutil.Int64PointerValue(filter.StoreID),
		sqlutil.Int64PointerValue(filter.FloorID), sqlutil.Int64PointerValue(filter.FloorID),
	)
	if err != nil {
		return nil, fmt.Errorf("list locations: %w", err)
	}
	defer rows.Close()

	items := make([]Location, 0)
	for rows.Next() {
		var item Location
		if err := rows.Scan(&item.ID, &item.StoreID, &item.FloorID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan location: %w", err)
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) CreateLocation(ctx context.Context, input LocationInput) (*Location, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO locations (store_id, floor_id, code, name, status)
		VALUES (?, ?, ?, ?, ?)
	`, input.StoreID, input.FloorID, input.Code, input.Name, input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.findLocationByID(ctx, id)
}

func (r *Repository) UpdateLocation(ctx context.Context, id int64, input LocationInput) (*Location, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE locations SET store_id = ?, floor_id = ?, code = ?, name = ?, status = ? WHERE id = ?
	`, input.StoreID, input.FloorID, input.Code, input.Name, input.Status, id)
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
	return r.findLocationByID(ctx, id)
}

func (r *Repository) ListUnits(ctx context.Context, filter UnitFilter) ([]Unit, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT id, building_id, floor_id, location_id, area_id, unit_type_id, shop_type_id, code, floor_area, use_area, rent_area, is_rentable, status, created_at, updated_at
		FROM units
		WHERE (? IS NULL OR building_id = ?)
		  AND (? IS NULL OR floor_id = ?)
		  AND (? IS NULL OR location_id = ?)
		  AND (? IS NULL OR area_id = ?)
		ORDER BY building_id, floor_id, id
	`,
		sqlutil.Int64PointerValue(filter.BuildingID), sqlutil.Int64PointerValue(filter.BuildingID),
		sqlutil.Int64PointerValue(filter.FloorID), sqlutil.Int64PointerValue(filter.FloorID),
		sqlutil.Int64PointerValue(filter.LocationID), sqlutil.Int64PointerValue(filter.LocationID),
		sqlutil.Int64PointerValue(filter.AreaID), sqlutil.Int64PointerValue(filter.AreaID),
	)
	if err != nil {
		return nil, fmt.Errorf("list units: %w", err)
	}
	defer rows.Close()

	items := make([]Unit, 0)
	for rows.Next() {
		var item Unit
		if err := rows.Scan(&item.ID, &item.BuildingID, &item.FloorID, &item.LocationID, &item.AreaID, &item.UnitTypeID, &item.ShopTypeID, &item.Code, &item.FloorArea, &item.UseArea, &item.RentArea, &item.IsRentable, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
			return nil, fmt.Errorf("scan unit: %w", err)
		}
		items = append(items, item)
	}
	return items, rows.Err()
}

func (r *Repository) CreateUnit(ctx context.Context, input UnitInput) (*Unit, error) {
	result, err := r.db.ExecContext(ctx, `
		INSERT INTO units (building_id, floor_id, location_id, area_id, unit_type_id, shop_type_id, code, floor_area, use_area, rent_area, is_rentable, status)
		VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
	`, input.BuildingID, input.FloorID, input.LocationID, input.AreaID, input.UnitTypeID, sqlutil.Int64PointerValue(input.ShopTypeID), input.Code, input.FloorArea, input.UseArea, input.RentArea, input.IsRentable, input.Status)
	if err != nil {
		return nil, err
	}
	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}
	return r.findUnitByID(ctx, id)
}

func (r *Repository) UpdateUnit(ctx context.Context, id int64, input UnitInput) (*Unit, error) {
	result, err := r.db.ExecContext(ctx, `
		UPDATE units
		SET building_id = ?, floor_id = ?, location_id = ?, area_id = ?, unit_type_id = ?, shop_type_id = ?, code = ?, floor_area = ?, use_area = ?, rent_area = ?, is_rentable = ?, status = ?
		WHERE id = ?
	`, input.BuildingID, input.FloorID, input.LocationID, input.AreaID, input.UnitTypeID, sqlutil.Int64PointerValue(input.ShopTypeID), input.Code, input.FloorArea, input.UseArea, input.RentArea, input.IsRentable, input.Status, id)
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
	return r.findUnitByID(ctx, id)
}

func (r *Repository) findAreaByID(ctx context.Context, id int64) (*Area, error) {
	var item Area
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, store_id, area_level_id, code, name, status, created_at, updated_at
		FROM areas WHERE id = ?
	`, id).Scan(&item.ID, &item.StoreID, &item.AreaLevelID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) findLocationByID(ctx context.Context, id int64) (*Location, error) {
	var item Location
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, store_id, floor_id, code, name, status, created_at, updated_at
		FROM locations WHERE id = ?
	`, id).Scan(&item.ID, &item.StoreID, &item.FloorID, &item.Code, &item.Name, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

func (r *Repository) findUnitByID(ctx context.Context, id int64) (*Unit, error) {
	var item Unit
	if err := r.db.QueryRowContext(ctx, `
		SELECT id, building_id, floor_id, location_id, area_id, unit_type_id, shop_type_id, code, floor_area, use_area, rent_area, is_rentable, status, created_at, updated_at
		FROM units WHERE id = ?
	`, id).Scan(&item.ID, &item.BuildingID, &item.FloorID, &item.LocationID, &item.AreaID, &item.UnitTypeID, &item.ShopTypeID, &item.Code, &item.FloorArea, &item.UseArea, &item.RentArea, &item.IsRentable, &item.Status, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return nil, err
	}
	return &item, nil
}

type scanner interface {
	Scan(dest ...any) error
}

func scanFloor(row scanner) (Floor, error) {
	var item Floor
	var floorPlanImageURL sql.NullString
	if err := row.Scan(&item.ID, &item.BuildingID, &item.Code, &item.Name, &item.Status, &floorPlanImageURL, &item.CreatedAt, &item.UpdatedAt); err != nil {
		return Floor{}, err
	}
	item.FloorPlanImageURL = sqlutil.NullStringPointer(floorPlanImageURL)
	return item, nil
}


