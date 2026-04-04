package bootstrap

import (
	"context"
	"database/sql"
)

func OrgSeeds() []Seed {
	return []Seed{
		seedDepartmentTypes(),
		seedDepartments(),
	}
}

func seedDepartmentTypes() Seed {
	return Seed{
		name: "department_types",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO department_types (id, code, name, sort_order)
				VALUES
				  (1, 'child_company', 'Child Company', 1),
				  (2, 'child_sold', 'Child Sold', 2),
				  (3, 'region_hq', 'Region HQ', 3),
				  (4, 'region', 'Region', 4),
				  (5, 'city', 'City', 5),
				  (6, 'mall', 'Mall', 6),
				  (7, 'department', 'Department', 7)
				ON DUPLICATE KEY UPDATE name = VALUES(name), sort_order = VALUES(sort_order)
			`)
			return err
		},
	}
}

func seedDepartments() Seed {
	return Seed{
		name: "departments",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO departments (id, parent_id, type_id, code, name, level, status)
				VALUES
				  (101, NULL, 6, 'ROOT-MALL', 'Root Mall', 1, 'active'),
				  (102, 101, 7, 'OPS', 'Operations', 2, 'active'),
				  (103, 101, 7, 'FIN', 'Finance', 2, 'active')
				ON DUPLICATE KEY UPDATE
				  parent_id = VALUES(parent_id),
				  type_id = VALUES(type_id),
				  name = VALUES(name),
				  level = VALUES(level),
				  status = VALUES(status)
			`)
			return err
		},
	}
}
