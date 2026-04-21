package auth

type User struct {
	ID           int64  `json:"id"`
	DepartmentID int64  `json:"department_id"`
	Username     string `json:"username"`
	DisplayName  string `json:"display_name"`
	PasswordHash string `json:"-"`
	Status       string `json:"status"`
}

type Role struct {
	ID       int64
	Code     string
	Name     string
	Status   string
	IsLeader bool
}

type Department struct {
	ID       int64  `json:"id"`
	Code     string `json:"code"`
	Name     string `json:"name"`
	Level    int    `json:"level"`
	Status   string `json:"status"`
	ParentID *int64 `json:"parent_id"`
	TypeID   int64  `json:"type_id"`
}

type Store struct {
	ID           int64  `json:"id"`
	DepartmentID int64  `json:"department_id"`
	Code         string `json:"code"`
	Name         string `json:"name"`
	ShortName    string `json:"short_name"`
	Status       string `json:"status"`
}

type Permission struct {
	FunctionCode    string `json:"function_code"`
	PermissionLevel string `json:"permission_level"`
	CanPrint        bool   `json:"can_print"`
	CanExport       bool   `json:"can_export"`
}

type SessionUser struct {
	ID           int64        `json:"id"`
	Username     string       `json:"username"`
	DisplayName  string       `json:"display_name"`
	DepartmentID int64        `json:"department_id"`
	Roles        []string     `json:"roles"`
	Permissions  []Permission `json:"permissions"`
}

type UserSummary struct {
	ID           int64  `json:"id"`
	DepartmentID int64  `json:"department_id"`
	Username     string `json:"username"`
	DisplayName  string `json:"display_name"`
	Status       string `json:"status"`
}

type CreateUserInput struct {
	DepartmentID int64
	Username     string
	DisplayName  string
	PasswordHash string
	Status       string
}

type UpdateUserInput struct {
	DepartmentID *int64
	DisplayName  *string
	Status       *string
}
