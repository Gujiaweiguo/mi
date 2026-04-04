package bootstrap

import (
	"context"
	"database/sql"
)

func CommercialSeeds() []Seed {
	all := append([]Seed{}, CutoverCommercialSeeds()...)
	all = append(all, seedDailySales(), seedCustomerTraffic(), seedUnitRentBudgets(), seedStoreRentBudgets(), seedUnitProspects())
	return all
}

func CutoverCommercialSeeds() []Seed {
	return []Seed{
		seedStoreTypes(),
		seedStoreManagementTypes(),
		seedStores(),
		seedAreaLevels(),
		seedAreas(),
		seedUnitTypes(),
		seedShopTypes(),
		seedBuildings(),
		seedFloors(),
		seedLocations(),
		seedUnits(),
		seedTradeDefinitions(),
		seedCustomers(),
		seedBrands(),
		seedCurrencyTypes(),
		seedUnitLayoutPositions(),
		seedCutoverOutputConfiguration(),
	}
}

func seedStoreTypes() Seed {
	return simpleSeed("store_types", `INSERT INTO store_types (id, code, name) VALUES (101, 'mall', 'Mall') ON DUPLICATE KEY UPDATE name = VALUES(name)`)
}
func seedStoreManagementTypes() Seed {
	return simpleSeed("store_management_types", `INSERT INTO store_management_types (id, code, name, status) VALUES (101, 'self_operated', 'Self Operated', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)`)
}
func seedStores() Seed {
	return simpleSeed("stores", `INSERT INTO stores (id, department_id, store_type_id, management_type_id, code, name, short_name, status) VALUES (101, 101, 101, 101, 'MI-001', 'MI Demo Mall', 'MI Mall', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), short_name = VALUES(short_name), status = VALUES(status)`)
}
func seedAreaLevels() Seed {
	return simpleSeed("area_levels", `INSERT INTO area_levels (id, code, name) VALUES (101, 'A', 'Level A') ON DUPLICATE KEY UPDATE name = VALUES(name)`)
}
func seedAreas() Seed {
	return simpleSeed("areas", `INSERT INTO areas (id, store_id, area_level_id, code, name, status) VALUES (101, 101, 101, 'A01', 'Main Atrium', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)`)
}
func seedUnitTypes() Seed {
	return simpleSeed("unit_types", `INSERT INTO unit_types (id, code, name, status) VALUES (101, 'shop', 'Shop', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)`)
}
func seedBuildings() Seed {
	return simpleSeed("buildings", `INSERT INTO buildings (id, store_id, code, name, status) VALUES (101, 101, 'BLD-A', 'Building A', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)`)
}
func seedFloors() Seed {
	return simpleSeed("floors", `INSERT INTO floors (id, building_id, code, name, status, floor_plan_image_url) VALUES (101, 101, 'F1', 'Floor 1', 'active', 'https://example.com/floor-101.png') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status), floor_plan_image_url = VALUES(floor_plan_image_url)`)
}
func seedLocations() Seed {
	return simpleSeed("locations", `INSERT INTO locations (id, store_id, floor_id, code, name, status) VALUES (101, 101, 101, 'L1', 'North Wing', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)`)
}
func seedUnits() Seed {
	return simpleSeed("units", `INSERT INTO units (id, building_id, floor_id, location_id, area_id, unit_type_id, shop_type_id, code, name, floor_area, use_area, rent_area, is_rentable, status) VALUES (101, 101, 101, 101, 101, 101, 101, 'U-101', 'Unit 101', 120.00, 118.00, 118.00, TRUE, 'active') ON DUPLICATE KEY UPDATE shop_type_id = VALUES(shop_type_id), name = VALUES(name), floor_area = VALUES(floor_area), use_area = VALUES(use_area), rent_area = VALUES(rent_area), is_rentable = VALUES(is_rentable), status = VALUES(status)`)
}
func seedShopTypes() Seed {
	return simpleSeed("shop_types", `INSERT INTO shop_types (id, code, name, color_hex, status) VALUES (101, 'fashion', 'Fashion', '#4CAF50', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), color_hex = VALUES(color_hex), status = VALUES(status)`)
}
func seedTradeDefinitions() Seed {
	return simpleSeed("trade_definitions", `INSERT INTO trade_definitions (id, parent_id, level, code, name, status) VALUES (101, NULL, 1, 'RETAIL', 'Retail', 'active'), (102, 101, 2, 'FASHION', 'Fashion', 'active') ON DUPLICATE KEY UPDATE parent_id = VALUES(parent_id), level = VALUES(level), name = VALUES(name), status = VALUES(status)`)
}
func seedCustomers() Seed {
	return simpleSeed("customers", `INSERT INTO customers (id, code, name, trade_id, department_id, status) VALUES (101, 'CUST-101', 'ACME Retail', 102, 101, 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), trade_id = VALUES(trade_id), department_id = VALUES(department_id), status = VALUES(status)`)
}
func seedBrands() Seed {
	return simpleSeed("brands", `INSERT INTO brands (id, code, name, status) VALUES (101, 'BR-101', 'ACME Fashion', 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)`)
}
func seedCurrencyTypes() Seed {
	return simpleSeed("currency_types", `INSERT INTO currency_types (id, code, name, is_local, status) VALUES (101, 'CNY', 'Chinese Yuan', TRUE, 'active') ON DUPLICATE KEY UPDATE name = VALUES(name), is_local = VALUES(is_local), status = VALUES(status)`)
}
func seedDailySales() Seed {
	return simpleSeed("daily_shop_sales", `INSERT INTO daily_shop_sales (store_id, unit_id, sale_date, sales_amount) VALUES
		(101, 101, '2026-04-01', 5000.00),
		(101, 101, '2026-04-05', 5000.00),
		(101, 101, '2026-04-10', 5000.00),
		(101, 101, '2026-04-15', 5000.00),
		(101, 101, '2026-04-20', 5000.00),
		(101, 101, '2025-04-01', 4500.00),
		(101, 101, '2025-04-05', 4500.00),
		(101, 101, '2025-04-10', 4500.00),
		(101, 101, '2025-04-15', 4500.00),
		(101, 101, '2025-04-20', 4500.00)
	ON DUPLICATE KEY UPDATE sales_amount = VALUES(sales_amount)`)
}
func seedCustomerTraffic() Seed {
	return simpleSeed("customer_traffic", `INSERT INTO customer_traffic (store_id, traffic_date, inbound_count) VALUES
		(101, '2026-01-15', 1000),
		(101, '2026-02-15', 2000),
		(101, '2026-03-15', 3000),
		(101, '2026-04-15', 4000),
		(101, '2026-05-15', 5000),
		(101, '2026-06-15', 6000),
		(101, '2026-07-15', 7000),
		(101, '2026-08-15', 8000),
		(101, '2026-09-15', 9000),
		(101, '2026-10-15', 10000),
		(101, '2026-11-15', 11000),
		(101, '2026-12-15', 12000)
	ON DUPLICATE KEY UPDATE inbound_count = VALUES(inbound_count)`)
}

func seedUnitRentBudgets() Seed {
	return simpleSeed("unit_rent_budgets", `INSERT INTO unit_rent_budgets (unit_id, fiscal_year, budget_price) VALUES
		(101, 2026, 95.00)
	ON DUPLICATE KEY UPDATE budget_price = VALUES(budget_price)`)
}

func seedStoreRentBudgets() Seed {
	return simpleSeed("store_rent_budgets", `INSERT INTO store_rent_budgets (store_id, fiscal_year, fiscal_month, monthly_budget) VALUES
		(101, 2026, 1, 10000.00),
		(101, 2026, 2, 10000.00),
		(101, 2026, 3, 10000.00),
		(101, 2026, 4, 10000.00),
		(101, 2026, 5, 10000.00),
		(101, 2026, 6, 10000.00),
		(101, 2026, 7, 10000.00),
		(101, 2026, 8, 10000.00),
		(101, 2026, 9, 10000.00),
		(101, 2026, 10, 10000.00),
		(101, 2026, 11, 10000.00),
		(101, 2026, 12, 10000.00)
	ON DUPLICATE KEY UPDATE monthly_budget = VALUES(monthly_budget)`)
}

func seedUnitProspects() Seed {
	return simpleSeed("unit_prospects", `INSERT INTO unit_prospects (unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id, avg_transaction, prospect_rent_price, rent_increment, prospect_term_months) VALUES
		(101, 2026, 101, 101, 102, 280.00, 110.00, '5% yearly', 36)
	ON DUPLICATE KEY UPDATE potential_customer_id = VALUES(potential_customer_id), prospect_brand_id = VALUES(prospect_brand_id), prospect_trade_id = VALUES(prospect_trade_id), avg_transaction = VALUES(avg_transaction), prospect_rent_price = VALUES(prospect_rent_price), rent_increment = VALUES(rent_increment), prospect_term_months = VALUES(prospect_term_months)`)
}

func seedUnitLayoutPositions() Seed {
	return simpleSeed("unit_layout_positions", `INSERT INTO unit_layout_positions (unit_id, pos_x, pos_y) VALUES
		(101, 240, 160)
	ON DUPLICATE KEY UPDATE pos_x = VALUES(pos_x), pos_y = VALUES(pos_y)`)
}

func seedCutoverOutputConfiguration() Seed {
	return Seed{
		name: "cutover_output_configuration",
		run: func(ctx context.Context, tx *sql.Tx) error {
			if _, err := tx.ExecContext(ctx, `
				INSERT INTO print_templates (id, code, name, document_type, output_mode, status, title, subtitle, header_lines, footer_lines, created_by, updated_by)
				VALUES
				  (101, 'invoice-batch-default', 'Invoice Batch Default', 'invoice', 'invoice_batch', 'active', 'Lease Invoice Batch Print', 'Default invoice batch layout', JSON_ARRAY('Sunshine Commercial MI', 'Finance Department'), JSON_ARRAY('Approved invoice output', 'Generated by MI'), 101, 101),
				  (102, 'bill-state-default', 'Bill State Default', 'bill', 'bill_state', 'active', 'Bill Printable State', 'Approved bill state', JSON_ARRAY('Sunshine Commercial MI'), JSON_ARRAY('Bill is printable after approval'), 101, 101),
				  (103, 'invoice-detail-default', 'Invoice Detail Default', 'invoice', 'invoice_detail', 'active', 'Invoice Detail Print', 'Printable invoice detail', JSON_ARRAY('Sunshine Commercial MI'), JSON_ARRAY('Invoice detail output'), 101, 101)
				ON DUPLICATE KEY UPDATE
				  name = VALUES(name),
				  document_type = VALUES(document_type),
				  output_mode = VALUES(output_mode),
				  status = VALUES(status),
				  title = VALUES(title),
				  subtitle = VALUES(subtitle),
				  header_lines = VALUES(header_lines),
				  footer_lines = VALUES(footer_lines),
				  updated_by = VALUES(updated_by)
			`); err != nil {
				return err
			}

			if _, err := tx.ExecContext(ctx, `
				INSERT INTO tax_voucher_rule_sets (id, code, name, document_type, status, created_by, updated_by)
				VALUES (101, 'kingdee-default', 'Kingdee Default', 'invoice', 'active', 101, 101)
				ON DUPLICATE KEY UPDATE
				  name = VALUES(name),
				  document_type = VALUES(document_type),
				  status = VALUES(status),
				  updated_by = VALUES(updated_by)
			`); err != nil {
				return err
			}

			_, err := tx.ExecContext(ctx, `
				INSERT INTO tax_voucher_rules (id, rule_set_id, sequence_no, entry_side, charge_type_filter, account_number, account_name, explanation_template, use_tenant_name)
				VALUES
				  (101, 101, 1, 'debit', 'rent', '1122', '应收账款', 'YYYYMMDD-YYYYMMDD ITEMCODE', TRUE),
				  (102, 101, 2, 'credit', 'rent', '6001', '租金收入', 'SYYYYMM ITEMCODE', FALSE)
				ON DUPLICATE KEY UPDATE
				  sequence_no = VALUES(sequence_no),
				  entry_side = VALUES(entry_side),
				  charge_type_filter = VALUES(charge_type_filter),
				  account_number = VALUES(account_number),
				  account_name = VALUES(account_name),
				  explanation_template = VALUES(explanation_template),
				  use_tenant_name = VALUES(use_tenant_name)
			`)
			return err
		},
	}
}

func simpleSeed(name, query string) Seed {
	return Seed{
		name: name,
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, query)
			return err
		},
	}
}
