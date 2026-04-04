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

func (r *Repository) UpsertUnits(ctx context.Context, rows []UnitImportRow, refs *UnitReference) error {
	tx, err := r.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin unit import transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	nextID, err := nextUnitID(ctx, tx)
	if err != nil {
		return err
	}
	buildingByCode := referenceByCode(refs.Buildings)
	floorByCode := referenceByCode(refs.Floors)
	locationByCode := referenceByCode(refs.Locations)
	areaByCode := referenceByCode(refs.Areas)
	unitTypeByCode := referenceByCode(refs.UnitTypes)
	for index, row := range rows {
		unitID, err := existingUnitID(ctx, tx, row.Code)
		if err != nil {
			return err
		}
		if unitID == 0 {
			unitID = nextID + int64(index)
		}
		_, err = tx.ExecContext(ctx, `
			INSERT INTO units (id, building_id, floor_id, location_id, area_id, unit_type_id, code, floor_area, use_area, rent_area, is_rentable, status)
			VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
			ON DUPLICATE KEY UPDATE building_id = VALUES(building_id), floor_id = VALUES(floor_id), location_id = VALUES(location_id), area_id = VALUES(area_id), unit_type_id = VALUES(unit_type_id), floor_area = VALUES(floor_area), use_area = VALUES(use_area), rent_area = VALUES(rent_area), is_rentable = VALUES(is_rentable), status = VALUES(status)
		`, unitID, buildingByCode[row.BuildingCode].ID, floorByCode[row.FloorCode].ID, locationByCode[row.LocationCode].ID, areaByCode[row.AreaCode].ID, unitTypeByCode[row.UnitTypeCode].ID, row.Code, row.FloorArea, row.UseArea, row.RentArea, row.IsRentable, row.Status)
		if err != nil {
			return fmt.Errorf("upsert unit %s: %w", row.Code, err)
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

func nextUnitID(ctx context.Context, tx *sql.Tx) (int64, error) {
	var id int64
	if err := tx.QueryRowContext(ctx, `SELECT COALESCE(MAX(id), 0) + 1 FROM units`).Scan(&id); err != nil {
		return 0, fmt.Errorf("load next unit id: %w", err)
	}
	return id, nil
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
