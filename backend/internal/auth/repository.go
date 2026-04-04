package auth

import (
	"context"
	"database/sql"
	"fmt"
)

type Repository struct {
	db *sql.DB
}

func NewRepository(db *sql.DB) *Repository {
	return &Repository{db: db}
}

func (r *Repository) FindUserByUsername(ctx context.Context, username string) (*User, error) {
	const query = `SELECT id, department_id, username, display_name, password_hash, status FROM users WHERE username = ?`
	var user User
	if err := r.db.QueryRowContext(ctx, query, username).Scan(&user.ID, &user.DepartmentID, &user.Username, &user.DisplayName, &user.PasswordHash, &user.Status); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("query user: %w", err)
	}
	return &user, nil
}

func (r *Repository) ListRolesForUser(ctx context.Context, userID int64) ([]Role, error) {
	const query = `SELECT r.id, r.code, r.name, r.status, r.is_leader FROM roles r INNER JOIN user_roles ur ON ur.role_id = r.id WHERE ur.user_id = ? ORDER BY r.id`
	rows, err := r.db.QueryContext(ctx, query, userID)
	if err != nil {
		return nil, fmt.Errorf("query roles: %w", err)
	}
	defer rows.Close()

	roles := make([]Role, 0)
	for rows.Next() {
		var role Role
		if err := rows.Scan(&role.ID, &role.Code, &role.Name, &role.Status, &role.IsLeader); err != nil {
			return nil, fmt.Errorf("scan role: %w", err)
		}
		roles = append(roles, role)
	}
	return roles, rows.Err()
}

func (r *Repository) ListPermissionsForUser(ctx context.Context, userID int64) ([]Permission, error) {
	const query = `SELECT DISTINCT f.code, rp.permission_level, rp.can_print, rp.can_export FROM role_permissions rp INNER JOIN functions f ON f.id = rp.function_id INNER JOIN user_roles ur ON ur.role_id = rp.role_id WHERE ur.user_id = ? ORDER BY f.code`
	rows, err := r.db.QueryContext(ctx, query, userID)
	if err != nil {
		return nil, fmt.Errorf("query permissions: %w", err)
	}
	defer rows.Close()

	permissions := make([]Permission, 0)
	for rows.Next() {
		var permission Permission
		if err := rows.Scan(&permission.FunctionCode, &permission.PermissionLevel, &permission.CanPrint, &permission.CanExport); err != nil {
			return nil, fmt.Errorf("scan permission: %w", err)
		}
		permissions = append(permissions, permission)
	}
	return permissions, rows.Err()
}

func (r *Repository) ListDepartments(ctx context.Context) ([]Department, error) {
	const query = `SELECT id, code, name, level, status, parent_id, type_id FROM departments ORDER BY level, id`
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("query departments: %w", err)
	}
	defer rows.Close()

	departments := make([]Department, 0)
	for rows.Next() {
		var department Department
		if err := rows.Scan(&department.ID, &department.Code, &department.Name, &department.Level, &department.Status, &department.ParentID, &department.TypeID); err != nil {
			return nil, fmt.Errorf("scan department: %w", err)
		}
		departments = append(departments, department)
	}
	return departments, rows.Err()
}

func (r *Repository) ListStores(ctx context.Context) ([]Store, error) {
	const query = `SELECT id, department_id, code, name, short_name, status FROM stores ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("query stores: %w", err)
	}
	defer rows.Close()

	stores := make([]Store, 0)
	for rows.Next() {
		var store Store
		if err := rows.Scan(&store.ID, &store.DepartmentID, &store.Code, &store.Name, &store.ShortName, &store.Status); err != nil {
			return nil, fmt.Errorf("scan store: %w", err)
		}
		stores = append(stores, store)
	}
	return stores, rows.Err()
}
