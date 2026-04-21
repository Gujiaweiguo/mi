package excelio

import (
	"context"
	"database/sql"
	"fmt"
)

type Repository struct{ db *sql.DB }

func NewRepository(db *sql.DB) *Repository { return &Repository{db: db} }

func (r *Repository) LoadUnitReference(ctx context.Context) (*UnitReference, error) {
	buildings, err := loadReferenceItems(ctx, r.db, `SELECT id, code, name FROM buildings ORDER BY code`)
	if err != nil {
		return nil, err
	}
	floors, err := loadReferenceItems(ctx, r.db, `SELECT id, code, name FROM floors ORDER BY code`)
	if err != nil {
		return nil, err
	}
	locations, err := loadReferenceItems(ctx, r.db, `SELECT id, code, name FROM locations ORDER BY code`)
	if err != nil {
		return nil, err
	}
	areas, err := loadReferenceItems(ctx, r.db, `SELECT id, code, name FROM areas ORDER BY code`)
	if err != nil {
		return nil, err
	}
	unitTypes, err := loadReferenceItems(ctx, r.db, `SELECT id, code, name FROM unit_types ORDER BY code`)
	if err != nil {
		return nil, err
	}
	return &UnitReference{Buildings: buildings, Floors: floors, Locations: locations, Areas: areas, UnitTypes: unitTypes}, nil
}

func (r *Repository) LoadSalesReference(ctx context.Context) (*SalesReference, error) {
	stores, err := loadReferenceItems(ctx, r.db, `SELECT id, code, name FROM stores ORDER BY code`)
	if err != nil {
		return nil, err
	}
	units, err := loadReferenceItems(ctx, r.db, `SELECT id, code, code AS name FROM units ORDER BY code`)
	if err != nil {
		return nil, err
	}
	return &SalesReference{Stores: stores, Units: units}, nil
}

func (r *Repository) UpsertUnits(ctx context.Context, rows []UnitImportRow, refs *UnitReference) error {
	tx, err := r.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin unit import transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	buildingByCode := referenceByCode(refs.Buildings)
	floorByCode := referenceByCode(refs.Floors)
	locationByCode := referenceByCode(refs.Locations)
	areaByCode := referenceByCode(refs.Areas)
	unitTypeByCode := referenceByCode(refs.UnitTypes)
	for _, row := range rows {
		unitID, err := existingUnitID(ctx, tx, row.Code)
		if err != nil {
			return err
		}
		if unitID == 0 {
			result, err := tx.ExecContext(ctx, `
				INSERT INTO units (building_id, floor_id, location_id, area_id, unit_type_id, code, floor_area, use_area, rent_area, is_rentable, status)
				VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
			`, buildingByCode[row.BuildingCode].ID, floorByCode[row.FloorCode].ID, locationByCode[row.LocationCode].ID, areaByCode[row.AreaCode].ID, unitTypeByCode[row.UnitTypeCode].ID, row.Code, row.FloorArea, row.UseArea, row.RentArea, row.IsRentable, row.Status)
			if err != nil {
				return fmt.Errorf("insert unit %s: %w", row.Code, err)
			}
			if _, err := result.LastInsertId(); err != nil {
				return fmt.Errorf("last insert id for unit %s: %w", row.Code, err)
			}
		} else {
			_, err = tx.ExecContext(ctx, `
				UPDATE units SET building_id = ?, floor_id = ?, location_id = ?, area_id = ?, unit_type_id = ?, floor_area = ?, use_area = ?, rent_area = ?, is_rentable = ?, status = ?
				WHERE id = ?
			`, buildingByCode[row.BuildingCode].ID, floorByCode[row.FloorCode].ID, locationByCode[row.LocationCode].ID, areaByCode[row.AreaCode].ID, unitTypeByCode[row.UnitTypeCode].ID, row.FloorArea, row.UseArea, row.RentArea, row.IsRentable, row.Status, unitID)
			if err != nil {
				return fmt.Errorf("update unit %s: %w", row.Code, err)
			}
		}
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit unit import transaction: %w", err)
	}
	return nil
}

func (r *Repository) ListInvoiceExportRows(ctx context.Context) ([]exportInvoiceRow, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT document_no, document_type, tenant_name, status, period_start, period_end, total_amount
		FROM billing_documents WHERE status = 'approved' ORDER BY id
	`)
	if err != nil {
		return nil, fmt.Errorf("list invoice export rows: %w", err)
	}
	defer rows.Close()
	items := make([]exportInvoiceRow, 0)
	for rows.Next() {
		var item exportInvoiceRow
		if err := rows.Scan(&item.DocumentNo, &item.DocumentType, &item.TenantName, &item.Status, &item.PeriodStart, &item.PeriodEnd, &item.TotalAmount); err != nil {
			return nil, fmt.Errorf("scan invoice export row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate invoice export rows: %w", err)
	}
	return items, nil
}

func (r *Repository) ListChargeExportRows(ctx context.Context) ([]exportChargeRow, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT lc.lease_no, lc.tenant_name, bcl.charge_type, bcl.period_start, bcl.period_end, bcl.quantity_days, bcl.unit_amount, bcl.amount
		FROM billing_charge_lines bcl
		INNER JOIN lease_contracts lc ON lc.id = bcl.lease_contract_id
		ORDER BY bcl.id
	`)
	if err != nil {
		return nil, fmt.Errorf("list charge export rows: %w", err)
	}
	defer rows.Close()
	items := make([]exportChargeRow, 0)
	for rows.Next() {
		var item exportChargeRow
		if err := rows.Scan(&item.LeaseNo, &item.TenantName, &item.ChargeType, &item.PeriodStart, &item.PeriodEnd, &item.QuantityDays, &item.UnitAmount, &item.Amount); err != nil {
			return nil, fmt.Errorf("scan charge export row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate charge export rows: %w", err)
	}
	return items, nil
}

func (r *Repository) ListLeaseContractExportRows(ctx context.Context) ([]exportLeaseContractRow, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT lc.lease_no, lc.tenant_name, s.code AS store_code, d.code AS department_code,
		       lc.start_date, lc.end_date, lc.status, lc.effective_version
		FROM lease_contracts lc
		INNER JOIN stores s ON s.id = lc.store_id
		INNER JOIN departments d ON d.id = lc.department_id
		ORDER BY lc.id
	`)
	if err != nil {
		return nil, fmt.Errorf("list lease contract export rows: %w", err)
	}
	defer rows.Close()
	items := make([]exportLeaseContractRow, 0)
	for rows.Next() {
		var item exportLeaseContractRow
		if err := rows.Scan(&item.LeaseNo, &item.TenantName, &item.StoreCode, &item.DepartmentCode, &item.StartDate, &item.EndDate, &item.Status, &item.EffectiveVersion); err != nil {
			return nil, fmt.Errorf("scan lease contract export row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate lease contract export rows: %w", err)
	}
	return items, nil
}

func (r *Repository) ListUnitDataExportRows(ctx context.Context) ([]exportUnitDataRow, error) {
	rows, err := r.db.QueryContext(ctx, `
		SELECT u.code, b.code AS building_code, f.code AS floor_code, l.code AS location_code,
		       a.code AS area_code, ut.code AS unit_type_code,
		       u.floor_area, u.use_area, u.rent_area, u.is_rentable, u.status
		FROM units u
		INNER JOIN buildings b ON b.id = u.building_id
		INNER JOIN floors f ON f.id = u.floor_id
		INNER JOIN locations l ON l.id = u.location_id
		INNER JOIN areas a ON a.id = u.area_id
		INNER JOIN unit_types ut ON ut.id = u.unit_type_id
		ORDER BY u.id
	`)
	if err != nil {
		return nil, fmt.Errorf("list unit data export rows: %w", err)
	}
	defer rows.Close()
	items := make([]exportUnitDataRow, 0)
	for rows.Next() {
		var item exportUnitDataRow
		if err := rows.Scan(&item.Code, &item.BuildingCode, &item.FloorCode, &item.LocationCode, &item.AreaCode, &item.UnitTypeCode, &item.FloorArea, &item.UseArea, &item.RentArea, &item.IsRentable, &item.Status); err != nil {
			return nil, fmt.Errorf("scan unit data export row: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate unit data export rows: %w", err)
	}
	return items, nil
}

func loadReferenceItems(ctx context.Context, db *sql.DB, query string) ([]ReferenceItem, error) {
	rows, err := db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("query reference items: %w", err)
	}
	defer rows.Close()
	items := make([]ReferenceItem, 0)
	for rows.Next() {
		var item ReferenceItem
		if err := rows.Scan(&item.ID, &item.Code, &item.Name); err != nil {
			return nil, fmt.Errorf("scan reference item: %w", err)
		}
		items = append(items, item)
	}
	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate reference items: %w", err)
	}
	return items, nil
}

func referenceByCode(items []ReferenceItem) map[string]ReferenceItem {
	result := make(map[string]ReferenceItem, len(items))
	for _, item := range items {
		result[item.Code] = item
	}
	return result
}

func existingUnitID(ctx context.Context, tx *sql.Tx, code string) (int64, error) {
	var id int64
	if err := tx.QueryRowContext(ctx, `SELECT id FROM units WHERE code = ?`, code).Scan(&id); err != nil {
		if err == sql.ErrNoRows {
			return 0, nil
		}
		return 0, fmt.Errorf("load existing unit id for %s: %w", code, err)
	}
	return id, nil
}
