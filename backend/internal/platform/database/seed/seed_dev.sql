SET NAMES utf8mb4;

-- MI development/demo seed data
-- Idempotent by design: INSERT IGNORE only, no destructive statements.
-- Load with:
-- mysql -u mi_dev -pmi_dev mi_dev < backend/internal/platform/database/seed/seed_dev.sql

-- =========================================================
-- Phase 1: Reference / Lookup Data
-- =========================================================

INSERT IGNORE INTO department_types (id, code, name, sort_order) VALUES
  (1, 'company', '公司', 1),
  (2, 'department', '部门', 2),
  (3, 'team', '小组', 3);

INSERT IGNORE INTO business_groups (id, code, name, status) VALUES
  (101, 'contract_mgmt', '合同管理', 'active'),
  (102, 'billing_mgmt', '计费管理', 'active'),
  (103, 'workflow_mgmt', '工作流管理', 'active'),
  (104, 'reporting_mgmt', '报表管理', 'active'),
  (105, 'tax_mgmt', '税务管理', 'active'),
  (106, 'system_mgmt', '系统管理', 'active');

INSERT IGNORE INTO store_types (id, code, name) VALUES
  (101, 'mall', '购物中心'),
  (102, 'retail_street', '商业街'),
  (103, 'office_tower', '写字楼');

INSERT IGNORE INTO store_management_types (id, code, name, status) VALUES
  (101, 'self_operated', '自营', 'active'),
  (102, 'entrusted', '委托管理', 'active'),
  (103, 'joint_operation', '合作运营', 'active');

INSERT IGNORE INTO area_levels (id, code, name) VALUES
  (101, 'floor', '楼层'),
  (102, 'zone', '区域'),
  (103, 'district', '片区');

INSERT IGNORE INTO unit_types (id, code, name, status) VALUES
  (101, 'shop', '商铺', 'active'),
  (102, 'counter', '专柜', 'active'),
  (103, 'kiosk', '中岛', 'active'),
  (104, 'office', '办公室', 'active'),
  (105, 'warehouse', '仓库', 'active');

INSERT IGNORE INTO shop_types (id, code, name, color_hex, status) VALUES
  (101, 'apparel', '服装', '#E57373', 'active'),
  (102, 'dining', '餐饮', '#FFB74D', 'active'),
  (103, 'jewelry', '珠宝', '#FFD54F', 'active'),
  (104, 'digital', '数码', '#64B5F6', 'active'),
  (105, 'beauty', '美妆', '#BA68C8', 'active'),
  (106, 'education', '教育', '#4DB6AC', 'active'),
  (107, 'entertainment', '娱乐', '#81C784', 'active'),
  (108, 'service', '服务', '#90A4AE', 'active');

INSERT IGNORE INTO currency_types (id, code, name, is_local, status) VALUES
  (101, 'CNY', '人民币', TRUE, 'active'),
  (102, 'USD', '美元', FALSE, 'active'),
  (103, 'EUR', '欧元', FALSE, 'active');

INSERT IGNORE INTO trade_definitions (id, parent_id, level, code, name, status) VALUES
  (101, NULL, 1, 'retail', '零售', 'active'),
  (102, NULL, 1, 'food', '餐饮', 'active'),
  (103, NULL, 1, 'services', '服务', 'active');

INSERT IGNORE INTO trade_definitions (id, parent_id, level, code, name, status) VALUES
  (111, 101, 2, 'clothing', '服装', 'active'),
  (112, 101, 2, 'shoes', '鞋履', 'active'),
  (113, 101, 2, 'bags', '箱包', 'active'),
  (121, 102, 2, 'chinese_food', '中餐', 'active'),
  (122, 102, 2, 'western_food', '西餐', 'active'),
  (123, 102, 2, 'japanese_food', '日料', 'active'),
  (131, 103, 2, 'education_service', '教育', 'active'),
  (132, 103, 2, 'fitness_service', '健身', 'active'),
  (133, 103, 2, 'beauty_service', '美容', 'active');

INSERT IGNORE INTO numbering_sequences (code, next_value, prefix) VALUES
  ('lease', 1003, 'LSE'),
  ('bill', 2001, 'BIL'),
  ('invoice', 3003, 'INV');

-- =========================================================
-- Phase 2: Organizational Structure
-- =========================================================

INSERT IGNORE INTO departments (id, parent_id, type_id, code, name, level, status) VALUES
  (101, NULL, 1, 'HQ', '总公司', 1, 'active');

INSERT IGNORE INTO departments (id, parent_id, type_id, code, name, level, status) VALUES
  (111, 101, 2, 'OPS', '运营部', 2, 'active'),
  (112, 101, 2, 'FIN', '财务部', 2, 'active'),
  (113, 101, 2, 'PRP', '物业部', 2, 'active'),
  (114, 101, 2, 'MKT', '市场部', 2, 'active');

INSERT IGNORE INTO departments (id, parent_id, type_id, code, name, level, status) VALUES
  (211, 111, 3, 'OPS-LEASE', '租赁组', 3, 'active'),
  (212, 112, 3, 'FIN-AR', '应收组', 3, 'active');

INSERT IGNORE INTO roles (id, code, name, status, is_leader) VALUES
  (101, 'system_admin', '系统管理员', 'active', TRUE),
  (102, 'lease_manager', '租赁经理', 'active', TRUE),
  (103, 'finance_manager', '财务经理', 'active', TRUE),
  (104, 'property_manager', '物业经理', 'active', TRUE),
  (105, 'store_operator', '门店运营', 'active', FALSE);

INSERT IGNORE INTO functions (id, business_group_id, code, name) VALUES
  (101, 103, 'workflow.admin', '工作流管理'),
  (102, 106, 'notification.admin', '通知管理'),
  (103, 106, 'masterdata.admin', '主数据管理'),
  (104, 104, 'sales.admin', '销售数据管理'),
  (105, 106, 'baseinfo.admin', '基础资料管理'),
  (106, 106, 'structure.admin', '铺位结构管理'),
  (107, 101, 'lease.contract', '租赁合同'),
  (108, 102, 'billing.charge', '计费出账'),
  (109, 102, 'billing.invoice', '账单发票'),
  (110, 105, 'tax.export', '税务导出'),
  (111, 104, 'reporting.generalize', '综合报表'),
  (112, 106, 'excel.io', 'Excel导入导出');

INSERT IGNORE INTO role_permissions (role_id, function_id, permission_level, can_print, can_export) VALUES
  (101, 101, 'approve', TRUE, TRUE),
  (101, 102, 'approve', TRUE, TRUE),
  (101, 103, 'approve', TRUE, TRUE),
  (101, 104, 'approve', TRUE, TRUE),
  (101, 105, 'approve', TRUE, TRUE),
  (101, 106, 'approve', TRUE, TRUE),
  (101, 107, 'approve', TRUE, TRUE),
  (101, 108, 'approve', TRUE, TRUE),
  (101, 109, 'approve', TRUE, TRUE),
  (101, 110, 'approve', TRUE, TRUE),
  (101, 111, 'approve', TRUE, TRUE),
  (101, 112, 'approve', TRUE, TRUE),
  (102, 101, 'view', FALSE, FALSE),
  (102, 107, 'approve', TRUE, TRUE),
  (102, 108, 'edit', FALSE, TRUE),
  (102, 109, 'view', TRUE, TRUE),
  (102, 111, 'view', FALSE, TRUE),
  (102, 112, 'edit', FALSE, TRUE),
  (103, 101, 'view', FALSE, FALSE),
  (103, 108, 'approve', FALSE, TRUE),
  (103, 109, 'approve', TRUE, TRUE),
  (103, 110, 'approve', FALSE, TRUE),
  (103, 111, 'view', FALSE, TRUE),
  (103, 112, 'edit', FALSE, TRUE),
  (104, 103, 'view', FALSE, FALSE),
  (104, 104, 'view', FALSE, TRUE),
  (104, 105, 'edit', FALSE, FALSE),
  (104, 106, 'approve', FALSE, FALSE),
  (104, 111, 'view', FALSE, TRUE),
  (105, 104, 'edit', FALSE, TRUE),
  (105, 107, 'edit', TRUE, FALSE),
  (105, 108, 'view', FALSE, FALSE),
  (105, 109, 'view', TRUE, FALSE),
  (105, 112, 'edit', FALSE, TRUE);

-- =========================================================
-- Phase 3: Users
-- =========================================================

INSERT IGNORE INTO users (id, department_id, username, display_name, password_hash, status) VALUES
  (101, 101, 'admin', '系统管理员', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'active'),
  (102, 211, 'manager', '租赁经理', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'active'),
  (103, 212, 'finance', '财务经理', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'active'),
  (104, 113, 'property', '物业经理', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'active'),
  (105, 114, 'operator', '门店运营', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'active');

INSERT IGNORE INTO user_roles (user_id, role_id, department_id) VALUES
  (101, 101, 101),
  (102, 102, 211),
  (103, 103, 212),
  (104, 104, 113),
  (105, 105, 114);

-- =========================================================
-- Phase 4: Property Hierarchy
-- =========================================================

INSERT IGNORE INTO stores (id, department_id, store_type_id, management_type_id, code, name, short_name, status) VALUES
  (101, 111, 101, 101, 'STL-MALL-001', '星光购物中心', '星光广场', 'active');

INSERT IGNORE INTO areas (id, store_id, area_level_id, code, name, status) VALUES
  (101, 101, 103, 'DIST-ATRIUM', '中庭片区', 'active'),
  (102, 101, 103, 'DIST-EAST', '东翼片区', 'active'),
  (103, 101, 103, 'DIST-WEST', '西翼片区', 'active'),
  (104, 101, 102, 'ZONE-OFFICE', '办公配套区', 'active');

INSERT IGNORE INTO buildings (id, store_id, code, name, status) VALUES
  (101, 101, 'BLDG-A', 'A栋', 'active'),
  (102, 101, 'BLDG-B', 'B栋', 'active'),
  (103, 101, 'BLDG-C', 'C栋', 'active');

INSERT IGNORE INTO floors (id, building_id, code, name, status, floor_plan_image_url) VALUES
  (101, 101, '1F', 'A栋1F', 'active', '/demo/floorplans/a-1f.png'),
  (102, 101, '2F', 'A栋2F', 'active', '/demo/floorplans/a-2f.png'),
  (103, 101, '3F', 'A栋3F', 'active', '/demo/floorplans/a-3f.png'),
  (104, 102, '1F', 'B栋1F', 'active', '/demo/floorplans/b-1f.png'),
  (105, 102, '2F', 'B栋2F', 'active', '/demo/floorplans/b-2f.png'),
  (106, 102, '3F', 'B栋3F', 'active', '/demo/floorplans/b-3f.png'),
  (107, 103, '1F', 'C栋1F', 'active', '/demo/floorplans/c-1f.png'),
  (108, 103, '2F', 'C栋2F', 'active', '/demo/floorplans/c-2f.png'),
  (109, 103, '3F', 'C栋3F', 'active', '/demo/floorplans/c-3f.png');

INSERT IGNORE INTO locations (id, store_id, floor_id, code, name, status) VALUES
  (1001, 101, 101, 'A1-E', 'A栋1F东区', 'active'),
  (1002, 101, 101, 'A1-W', 'A栋1F西区', 'active'),
  (1003, 101, 102, 'A2-E', 'A栋2F东区', 'active'),
  (1004, 101, 102, 'A2-W', 'A栋2F西区', 'active'),
  (1005, 101, 103, 'A3-E', 'A栋3F东区', 'active'),
  (1006, 101, 103, 'A3-W', 'A栋3F西区', 'active'),
  (1007, 101, 104, 'B1-E', 'B栋1F东区', 'active'),
  (1008, 101, 104, 'B1-W', 'B栋1F西区', 'active'),
  (1009, 101, 105, 'B2-E', 'B栋2F东区', 'active'),
  (1010, 101, 105, 'B2-W', 'B栋2F西区', 'active'),
  (1011, 101, 106, 'B3-E', 'B栋3F东区', 'active'),
  (1012, 101, 106, 'B3-W', 'B栋3F西区', 'active'),
  (1013, 101, 107, 'C1-E', 'C栋1F东区', 'active'),
  (1014, 101, 107, 'C1-W', 'C栋1F西区', 'active'),
  (1015, 101, 108, 'C2-E', 'C栋2F东区', 'active'),
  (1016, 101, 108, 'C2-W', 'C栋2F西区', 'active'),
  (1017, 101, 109, 'C3-E', 'C栋3F东区', 'active'),
  (1018, 101, 109, 'C3-W', 'C栋3F西区', 'active');

INSERT IGNORE INTO units (id, building_id, floor_id, location_id, area_id, unit_type_id, shop_type_id, code, name, floor_area, use_area, rent_area, is_rentable, status) VALUES
  (2001, 101, 101, 1001, 101, 101, 101, 'A1-101', 'A栋1F-101', 135.00, 130.00, 130.00, TRUE, 'active'),
  (2002, 101, 101, 1001, 101, 101, 102, 'A1-102', 'A栋1F-102', 180.00, 175.00, 175.00, TRUE, 'active'),
  (2003, 101, 101, 1002, 103, 102, 103, 'A1-W01', 'A栋1F-西01', 95.00, 90.00, 90.00, TRUE, 'active'),
  (2004, 101, 101, 1002, 103, 103, 104, 'A1-W02', 'A栋1F-西02', 60.00, 56.00, 56.00, TRUE, 'active'),
  (2005, 101, 102, 1003, 102, 101, 105, 'A2-201', 'A栋2F-201', 120.00, 116.00, 116.00, TRUE, 'active'),
  (2006, 101, 102, 1003, 102, 101, 106, 'A2-202', 'A栋2F-202', 140.00, 135.00, 135.00, TRUE, 'active'),
  (2007, 101, 102, 1004, 103, 101, 108, 'A2-W01', 'A栋2F-西01', 110.00, 105.00, 105.00, TRUE, 'active'),
  (2008, 101, 102, 1004, 103, 102, 107, 'A2-W02', 'A栋2F-西02', 150.00, 145.00, 145.00, TRUE, 'active'),
  (2009, 101, 103, 1005, 104, 104, 108, 'A3-301', 'A栋3F-301', 220.00, 210.00, 210.00, TRUE, 'active'),
  (2010, 101, 103, 1005, 104, 105, 108, 'A3-302', 'A栋3F-302', 180.00, 172.00, 172.00, TRUE, 'active'),
  (2011, 101, 103, 1006, 104, 104, 108, 'A3-W01', 'A栋3F-西01', 205.00, 198.00, 198.00, TRUE, 'active'),
  (2012, 102, 104, 1007, 101, 101, 101, 'B1-101', 'B栋1F-101', 210.00, 205.00, 205.00, TRUE, 'active'),
  (2013, 102, 104, 1007, 101, 101, 101, 'B1-102', 'B栋1F-102', 195.00, 188.00, 188.00, TRUE, 'active'),
  (2014, 102, 104, 1008, 103, 102, 103, 'B1-W01', 'B栋1F-西01', 88.00, 84.00, 84.00, TRUE, 'active'),
  (2015, 102, 104, 1008, 103, 103, 104, 'B1-W02', 'B栋1F-西02', 72.00, 68.00, 68.00, TRUE, 'active'),
  (2016, 102, 105, 1009, 102, 101, 105, 'B2-201', 'B栋2F-201', 128.00, 123.00, 123.00, TRUE, 'active'),
  (2017, 102, 105, 1009, 102, 101, 107, 'B2-202', 'B栋2F-202', 165.00, 160.00, 160.00, TRUE, 'active'),
  (2018, 102, 105, 1010, 103, 101, 106, 'B2-W01', 'B栋2F-西01', 132.00, 127.00, 127.00, TRUE, 'active'),
  (2019, 102, 105, 1010, 103, 101, 108, 'B2-W02', 'B栋2F-西02', 118.00, 112.00, 112.00, TRUE, 'active'),
  (2020, 102, 106, 1011, 104, 104, 108, 'B3-301', 'B栋3F-301', 230.00, 220.00, 220.00, TRUE, 'active'),
  (2021, 102, 106, 1011, 104, 105, 108, 'B3-302', 'B栋3F-302', 175.00, 168.00, 168.00, TRUE, 'active'),
  (2022, 102, 106, 1012, 104, 104, 108, 'B3-W01', 'B栋3F-西01', 215.00, 208.00, 208.00, TRUE, 'active'),
  (2023, 103, 107, 1013, 101, 101, 102, 'C1-101', 'C栋1F-101', 160.00, 154.00, 154.00, TRUE, 'active'),
  (2024, 103, 107, 1013, 101, 101, 103, 'C1-102', 'C栋1F-102', 102.00, 98.00, 98.00, TRUE, 'active'),
  (2025, 103, 107, 1014, 103, 102, 104, 'C1-W01', 'C栋1F-西01', 78.00, 74.00, 74.00, TRUE, 'active'),
  (2026, 103, 107, 1014, 103, 103, 101, 'C1-W02', 'C栋1F-西02', 82.00, 78.00, 78.00, TRUE, 'active'),
  (2027, 103, 108, 1015, 102, 101, 105, 'C2-201', 'C栋2F-201', 136.00, 130.00, 130.00, TRUE, 'active'),
  (2028, 103, 108, 1015, 102, 101, 106, 'C2-202', 'C栋2F-202', 148.00, 142.00, 142.00, TRUE, 'active'),
  (2029, 103, 108, 1016, 103, 101, 107, 'C2-W01', 'C栋2F-西01', 170.00, 165.00, 165.00, TRUE, 'active'),
  (2030, 103, 108, 1016, 103, 101, 108, 'C2-W02', 'C栋2F-西02', 126.00, 120.00, 120.00, TRUE, 'active');

-- =========================================================
-- Phase 5: Workflow Definitions
-- =========================================================

INSERT IGNORE INTO workflow_definitions (id, business_group_id, code, name, voucher_type, is_initial, status, transitions_enabled, process_class) VALUES
  (101, 101, 'lease-approval', '租赁合同审批', 'lease_contract', TRUE, 'active', TRUE, 'lease_contract'),
  (102, 102, 'invoice-approval', '发票审批', 'invoice', FALSE, 'active', TRUE, 'invoice');

INSERT IGNORE INTO workflow_nodes (id, workflow_definition_id, function_id, role_id, step_order, code, name, can_submit_to_manager, validates_after_confirm, prints_after_confirm, process_class) VALUES
  (101, 101, 107, 105, 1, 'lease-submit', '提交申请', TRUE, FALSE, FALSE, 'lease_contract'),
  (102, 101, 107, 102, 2, 'lease-manager-approve', '经理审批', FALSE, TRUE, TRUE, 'lease_contract'),
  (103, 102, 109, 105, 1, 'invoice-submit', '提交开票', TRUE, FALSE, FALSE, 'invoice'),
  (104, 102, 109, 103, 2, 'invoice-finance-approve', '财务审批', FALSE, TRUE, TRUE, 'invoice');

INSERT IGNORE INTO workflow_transitions (id, workflow_definition_id, from_node_id, to_node_id, action) VALUES
  (101, 101, NULL, 101, 'submit'),
  (102, 101, 101, 102, 'approve'),
  (103, 102, NULL, 103, 'submit'),
  (104, 102, 103, 104, 'approve');

-- =========================================================
-- Phase 6: Master Data
-- =========================================================

INSERT IGNORE INTO customers (id, code, name, trade_id, department_id, status) VALUES
  (101, 'CUST-101', '万达影城', 103, 111, 'active'),
  (102, 'CUST-102', '星巴克咖啡', 122, 111, 'active'),
  (103, 'CUST-103', '优衣库', 111, 111, 'active'),
  (104, 'CUST-104', '海底捞', 121, 111, 'active'),
  (105, 'CUST-105', '周大福珠宝', 101, 111, 'active');

INSERT IGNORE INTO brands (id, code, name, status) VALUES
  (101, 'BR-101', '万达影城', 'active'),
  (102, 'BR-102', '星巴克', 'active'),
  (103, 'BR-103', '优衣库', 'active'),
  (104, 'BR-104', '海底捞', 'active'),
  (105, 'BR-105', '周大福', 'active');

-- =========================================================
-- Phase 7: Transaction Data
-- =========================================================

INSERT IGNORE INTO lease_contracts (
  id, amended_from_id, lease_no, department_id, store_id, building_id, customer_id, brand_id, trade_id, management_type_id,
  tenant_name, start_date, end_date, status, workflow_instance_id, effective_version, submitted_at, approved_at,
  billing_effective_at, terminated_at, created_by, updated_by
) VALUES
  (10001, NULL, 'LSE-2026-0001', 111, 101, 101, 102, 102, 122, 101, '星巴克咖啡', '2026-01-01', '2028-12-31', 'active', NULL, 1, '2025-12-20 10:00:00', '2025-12-22 15:30:00', '2026-01-01 00:00:00', NULL, 105, 102),
  (10002, NULL, 'LSE-2026-0002', 111, 101, 102, 103, 103, 111, 101, '优衣库', '2026-03-01', '2029-02-28', 'active', NULL, 1, '2026-02-10 09:15:00', '2026-02-12 18:20:00', '2026-03-01 00:00:00', NULL, 105, 102);

INSERT IGNORE INTO lease_contract_units (id, lease_contract_id, unit_id, rent_area) VALUES
  (11001, 10001, 2002, 175.00),
  (11002, 10002, 2012, 205.00),
  (11003, 10002, 2013, 188.00);

INSERT IGNORE INTO lease_contract_terms (id, lease_contract_id, term_type, billing_cycle, currency_type_id, amount, effective_from, effective_to) VALUES
  (12001, 10001, 'rent', 'monthly', 101, 28000.00, '2026-01-01', '2028-12-31'),
  (12002, 10002, 'rent', 'monthly', 101, 45000.00, '2026-03-01', '2029-02-28');

INSERT IGNORE INTO billing_runs (id, period_start, period_end, status, triggered_by, generated_count, skipped_count) VALUES
  (13001, '2026-04-01', '2026-04-30', 'completed', 103, 2, 0);

INSERT IGNORE INTO billing_charge_lines (
  id, billing_run_id, lease_contract_id, lease_term_id, charge_type, period_start, period_end,
  quantity_days, unit_amount, amount, currency_type_id, source_effective_version
) VALUES
  (14001, 13001, 10001, 12001, 'rent', '2026-04-01', '2026-04-30', 30, 28000.00, 28000.00, 101, 1),
  (14002, 13001, 10002, 12002, 'rent', '2026-04-01', '2026-04-30', 30, 45000.00, 45000.00, 101, 1);

INSERT IGNORE INTO billing_documents (
  id, document_type, document_no, billing_run_id, lease_contract_id, tenant_name, period_start, period_end,
  total_amount, currency_type_id, status, workflow_instance_id, adjusted_from_id, submitted_at, approved_at,
  cancelled_at, created_by, updated_by
) VALUES
  (15001, 'invoice', 'INV-2026-0001', 13001, 10001, '星巴克咖啡', '2026-04-01', '2026-04-30', 28000.00, 101, 'approved', NULL, NULL, '2026-04-02 09:00:00', '2026-04-03 14:00:00', NULL, 103, 103),
  (15002, 'invoice', 'INV-2026-0002', 13001, 10002, '优衣库', '2026-04-01', '2026-04-30', 45000.00, 101, 'pending_approval', NULL, NULL, '2026-04-04 10:15:00', NULL, NULL, 103, 103);

INSERT IGNORE INTO billing_document_lines (
  id, billing_document_id, billing_charge_line_id, charge_type, period_start, period_end, quantity_days, unit_amount, amount
) VALUES
  (16001, 15001, 14001, 'rent', '2026-04-01', '2026-04-30', 30, 28000.00, 28000.00),
  (16002, 15002, 14002, 'rent', '2026-04-01', '2026-04-30', 30, 45000.00, 45000.00);

INSERT IGNORE INTO ar_open_items (
  id, lease_contract_id, billing_document_id, billing_document_line_id, customer_id, department_id, trade_id,
  charge_type, due_date, outstanding_amount, settled_at, is_deposit
) VALUES
  (17001, 10001, 15001, 16001, 102, 111, 122, 'rent', '2026-04-30', 18000.00, NULL, FALSE),
  (17002, 10001, NULL, NULL, 102, 111, 122, 'deposit', '2026-01-01', 30000.00, NULL, TRUE);

INSERT IGNORE INTO ar_payment_entries (id, billing_document_id, lease_contract_id, payment_date, amount, note, recorded_by, idempotency_key) VALUES
  (18001, 15001, 10001, '2026-04-20', 10000.00, '演示用部分回款', 103, 'seed-payment-15001');

-- =========================================================
-- Phase 8: Optional Demo Data
-- =========================================================

INSERT IGNORE INTO tax_voucher_rule_sets (id, code, name, document_type, status, created_by, updated_by) VALUES
  (101, 'kingdee-default', '金蝶默认凭证规则', 'invoice', 'active', 101, 101);

INSERT IGNORE INTO tax_voucher_rules (
  id, rule_set_id, sequence_no, entry_side, charge_type_filter, account_number, account_name,
  explanation_template, use_tenant_name, is_balancing_entry
) VALUES
  (101, 101, 1, 'debit', 'rent', '1122', '应收账款', 'YYYYMMDD-YYYYMMDD ITEMCODE', TRUE, FALSE),
  (102, 101, 2, 'credit', 'rent', '6001', '租金收入', 'SYYYYMM ITEMCODE', FALSE, FALSE);

INSERT IGNORE INTO print_templates (
  id, code, name, document_type, output_mode, status, title, subtitle, header_lines, footer_lines, created_by, updated_by
) VALUES
  (101, 'invoice-detail-default', '默认发票明细模板', 'invoice', 'invoice_detail', 'active', '星光购物中心发票明细', '开发环境演示模板', '["星光商业管理有限公司","财务中心"]', '["此模板仅用于开发演示","Generated by MI"]', 101, 101);

INSERT IGNORE INTO daily_shop_sales (id, store_id, unit_id, sale_date, sales_amount) VALUES
  (19001, 101, 2002, '2026-04-10', 36800.00),
  (19002, 101, 2012, '2026-04-10', 128000.00),
  (19003, 101, 2023, '2026-04-10', 45200.00),
  (19004, 101, 2002, '2026-04-11', 37250.00),
  (19005, 101, 2012, '2026-04-11', 130400.00),
  (19006, 101, 2023, '2026-04-11', 44700.00);

INSERT IGNORE INTO customer_traffic (id, store_id, traffic_date, inbound_count) VALUES
  (20001, 101, '2026-04-01', 18200),
  (20002, 101, '2026-04-02', 17650),
  (20003, 101, '2026-04-03', 18840),
  (20004, 101, '2026-04-04', 20510);

INSERT IGNORE INTO unit_rent_budgets (id, unit_id, fiscal_year, budget_price) VALUES
  (21001, 2005, 2026, 230.00),
  (21002, 2016, 2026, 260.00),
  (21003, 2017, 2026, 280.00),
  (21004, 2027, 2026, 240.00),
  (21005, 2028, 2026, 235.00),
  (21006, 2030, 2026, 215.00);

INSERT IGNORE INTO store_rent_budgets (id, store_id, fiscal_year, fiscal_month, monthly_budget) VALUES
  (22001, 101, 2026, 1, 920000.00),
  (22002, 101, 2026, 2, 935000.00),
  (22003, 101, 2026, 3, 948000.00),
  (22004, 101, 2026, 4, 965000.00),
  (22005, 101, 2026, 5, 978000.00),
  (22006, 101, 2026, 6, 990000.00),
  (22007, 101, 2026, 7, 1005000.00),
  (22008, 101, 2026, 8, 1012000.00),
  (22009, 101, 2026, 9, 1008000.00),
  (22010, 101, 2026, 10, 1035000.00),
  (22011, 101, 2026, 11, 1060000.00),
  (22012, 101, 2026, 12, 1120000.00);

INSERT IGNORE INTO unit_prospects (
  id, unit_id, fiscal_year, potential_customer_id, prospect_brand_id, prospect_trade_id,
  avg_transaction, prospect_rent_price, rent_increment, prospect_term_months
) VALUES
  (23001, 2027, 2026, 105, 105, 101, 1450.00, 255.00, '3% yearly', 36),
  (23002, 2028, 2026, 101, 101, 103, 980.00, 238.00, '5% every 2 years', 60),
  (23003, 2030, 2026, 104, 104, 121, 320.00, 220.00, '4% yearly', 48);

INSERT IGNORE INTO unit_layout_positions (unit_id, pos_x, pos_y) VALUES
  (2001, 80, 60),
  (2002, 180, 60),
  (2003, 300, 60),
  (2004, 380, 60),
  (2005, 80, 140),
  (2006, 190, 140),
  (2007, 310, 140),
  (2008, 410, 140),
  (2009, 90, 220),
  (2010, 230, 220),
  (2011, 390, 220),
  (2012, 80, 320),
  (2013, 200, 320),
  (2014, 330, 320),
  (2015, 410, 320),
  (2016, 80, 400),
  (2017, 200, 400),
  (2018, 330, 400),
  (2019, 430, 400),
  (2020, 90, 480),
  (2021, 240, 480),
  (2022, 390, 480),
  (2023, 80, 580),
  (2024, 200, 580),
  (2025, 330, 580),
  (2026, 410, 580),
  (2027, 80, 660),
  (2028, 200, 660),
  (2029, 330, 660),
  (2030, 430, 660);
