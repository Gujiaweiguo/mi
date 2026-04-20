package auth

import (
	"context"
	"database/sql"
	"fmt"
	"strings"
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

func (r *Repository) ListUsers(ctx context.Context) ([]UserSummary, error) {
	const query = `SELECT id, department_id, username, display_name, status FROM users ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("list users: %w", err)
	}
	defer rows.Close()
	users := make([]UserSummary, 0)
	for rows.Next() {
		var u UserSummary
		if err := rows.Scan(&u.ID, &u.DepartmentID, &u.Username, &u.DisplayName, &u.Status); err != nil {
			return nil, fmt.Errorf("scan user: %w", err)
		}
		users = append(users, u)
	}
	return users, rows.Err()
}

func (r *Repository) GetUserByID(ctx context.Context, id int64) (*UserSummary, error) {
	const query = `SELECT id, department_id, username, display_name, status FROM users WHERE id = ?`
	var u UserSummary
	if err := r.db.QueryRowContext(ctx, query, id).Scan(&u.ID, &u.DepartmentID, &u.Username, &u.DisplayName, &u.Status); err != nil {
		if err == sql.ErrNoRows {
			return nil, nil
		}
		return nil, fmt.Errorf("get user: %w", err)
	}
	return &u, nil
}

func (r *Repository) nextUserID(ctx context.Context) (int64, error) {
	var maxID sql.NullInt64
	if err := r.db.QueryRowContext(ctx, `SELECT MAX(id) FROM users`).Scan(&maxID); err != nil {
		return 0, fmt.Errorf("next user id: %w", err)
	}
	if maxID.Valid {
		return maxID.Int64 + 1, nil
	}
	return 1, nil
}

func (r *Repository) CreateUser(ctx context.Context, input CreateUserInput) (int64, error) {
	id, err := r.nextUserID(ctx)
	if err != nil {
		return 0, err
	}
	const query = `INSERT INTO users (id, department_id, username, display_name, password_hash, status) VALUES (?, ?, ?, ?, ?, ?)`
	_, err = r.db.ExecContext(ctx, query, id, input.DepartmentID, input.Username, input.DisplayName, input.PasswordHash, input.Status)
	if err != nil {
		return 0, fmt.Errorf("create user: %w", err)
	}
	return id, nil
}

func (r *Repository) UpdateUser(ctx context.Context, id int64, input UpdateUserInput) error {
	setClauses := []string{}
	args := []interface{}{}
	if input.DepartmentID != nil {
		setClauses = append(setClauses, "department_id = ?")
		args = append(args, *input.DepartmentID)
	}
	if input.DisplayName != nil {
		setClauses = append(setClauses, "display_name = ?")
		args = append(args, *input.DisplayName)
	}
	if input.Status != nil {
		setClauses = append(setClauses, "status = ?")
		args = append(args, *input.Status)
	}
	if len(setClauses) == 0 {
		return nil
	}
	query := "UPDATE users SET " + strings.Join(setClauses, ", ") + " WHERE id = ?"
	args = append(args, id)
	_, err := r.db.ExecContext(ctx, query, args...)
	if err != nil {
		return fmt.Errorf("update user: %w", err)
	}
	return nil
}

func (r *Repository) SetUserPassword(ctx context.Context, id int64, passwordHash string) error {
	const query = `UPDATE users SET password_hash = ? WHERE id = ?`
	_, err := r.db.ExecContext(ctx, query, passwordHash, id)
	if err != nil {
		return fmt.Errorf("set user password: %w", err)
	}
	return nil
}

func (r *Repository) ListRoles(ctx context.Context) ([]Role, error) {
	const query = `SELECT id, code, name, status, is_leader FROM roles ORDER BY id`
	rows, err := r.db.QueryContext(ctx, query)
	if err != nil {
		return nil, fmt.Errorf("list roles: %w", err)
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

func (r *Repository) SetUserRoles(ctx context.Context, userID int64, roleIDs []int64, departmentID int64) error {
	tx, err := r.db.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin tx: %w", err)
	}
	defer tx.Rollback()
	if _, err := tx.ExecContext(ctx, `DELETE FROM user_roles WHERE user_id = ?`, userID); err != nil {
		return fmt.Errorf("delete user roles: %w", err)
	}
	for _, roleID := range roleIDs {
		if _, err := tx.ExecContext(ctx, `INSERT INTO user_roles (user_id, role_id, department_id) VALUES (?, ?, ?)`, userID, roleID, departmentID); err != nil {
			return fmt.Errorf("insert user role: %w", err)
		}
	}
	return tx.Commit()
}

func (r *Repository) GetUserRoleIDs(ctx context.Context, userID int64) ([]int64, error) {
	const query = `SELECT role_id FROM user_roles WHERE user_id = ?`
	rows, err := r.db.QueryContext(ctx, query, userID)
	if err != nil {
		return nil, fmt.Errorf("get user role ids: %w", err)
	}
	defer rows.Close()
	ids := make([]int64, 0)
	for rows.Next() {
		var id int64
		if err := rows.Scan(&id); err != nil {
			return nil, fmt.Errorf("scan role id: %w", err)
		}
		ids = append(ids, id)
	}
	return ids, rows.Err()
}

func (r *Repository) UsernameExists(ctx context.Context, username string, excludeID int64) (bool, error) {
	const query = `SELECT COUNT(*) FROM users WHERE username = ? AND id != ?`
	var count int64
	if err := r.db.QueryRowContext(ctx, query, username, excludeID).Scan(&count); err != nil {
		return false, fmt.Errorf("check username: %w", err)
	}
	return count > 0, nil
}
