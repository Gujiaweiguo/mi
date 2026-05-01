package bootstrap

import (
	"context"
	"database/sql"
)

func AccessSeeds() []Seed {
	return []Seed{
		seedRoles(),
		seedUsers(),
		seedUserRoles(),
		seedFunctions(),
		seedRolePermissions(),
	}
}

func seedRoles() Seed {
	return Seed{
		name: "roles",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO roles (id, code, name, status, is_leader)
				VALUES
				  (101, 'admin', 'Administrator', 'active', TRUE),
				  (102, 'ops_manager', 'Operations Manager', 'active', TRUE),
				  (103, 'finance_manager', 'Finance Manager', 'active', TRUE)
				ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status), is_leader = VALUES(is_leader)
			`)
			return err
		},
	}
}

func seedUsers() Seed {
	return Seed{
		name: "users",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO users (id, department_id, username, display_name, password_hash, status)
				VALUES
				  (101, 101, 'admin', 'System Administrator', '$2a$10$32RDlfSKfGJDcHhJWP3JoOBi8SyorV7r2lWcs8hixdhFA/AtOI1gC', 'active')
				ON DUPLICATE KEY UPDATE display_name = VALUES(display_name), password_hash = VALUES(password_hash), status = VALUES(status)
			`)
			return err
		},
	}
}

func seedUserRoles() Seed {
	return Seed{
		name: "user_roles",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO user_roles (user_id, role_id, department_id)
				VALUES (101, 101, 101)
				ON DUPLICATE KEY UPDATE department_id = VALUES(department_id)
			`)
			return err
		},
	}
}

func seedFunctions() Seed {
	return Seed{
		name: "functions",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
					INSERT INTO functions (id, business_group_id, code, name)
					VALUES
					  (101, 101, 'lease.contract', 'Lease Contract'),
					  (104, 102, 'billing.charge', 'Billing Charge'),
					  (102, 102, 'billing.invoice', 'Billing Invoice'),
					  (105, 102, 'tax.export', 'Tax Export'),
					  (106, 103, 'excel.io', 'Excel Import Export'),
					  (103, 103, 'workflow.admin', 'Workflow Admin'),
					  (113, 103, 'workflow.definition', 'Workflow Definition Admin'),
					  (107, 103, 'reporting.generalize', 'Generalize Reporting'),
					  (108, 103, 'masterdata.admin', 'Master Data Admin'),
					  (109, 103, 'sales.admin', 'Sales Data Admin'),
					  (110, 103, 'baseinfo.admin', 'Base Info Admin'),
					  (111, 103, 'structure.admin', 'Structure Admin')
					ON DUPLICATE KEY UPDATE code = VALUES(code), name = VALUES(name)
				`)
			return err
		},
	}
}

func seedRolePermissions() Seed {
	return Seed{
		name: "role_permissions",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
					INSERT INTO role_permissions (role_id, function_id, permission_level, can_print, can_export)
					VALUES
					  (101, 101, 'approve', TRUE, TRUE),
					  (101, 104, 'approve', TRUE, TRUE),
					  (101, 102, 'approve', TRUE, TRUE),
					  (101, 105, 'approve', TRUE, TRUE),
					  (101, 106, 'approve', TRUE, TRUE),
					  (101, 103, 'approve', TRUE, TRUE),
					  (101, 113, 'approve', FALSE, FALSE),
					  (101, 107, 'approve', FALSE, TRUE),
					  (101, 108, 'approve', FALSE, FALSE),
					  (101, 109, 'approve', FALSE, FALSE),
					  (101, 110, 'approve', FALSE, FALSE),
					  (101, 111, 'approve', FALSE, FALSE)
					ON DUPLICATE KEY UPDATE permission_level = VALUES(permission_level), can_print = VALUES(can_print), can_export = VALUES(can_export)
				`)
			return err
		},
	}
}
