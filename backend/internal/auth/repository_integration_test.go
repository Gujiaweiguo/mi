//go:build integration

package auth_test

import (
	"context"
	"errors"
	"os"
	"reflect"
	"sort"
	"testing"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/config"
	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"golang.org/x/crypto/bcrypt"
)

func newTestRepository(t *testing.T) (context.Context, *auth.Repository) {
	t.Helper()

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	t.Cleanup(cancel)

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	return ctx, auth.NewRepository(db)
}

func TestRepositoryIntegrationCRUDAndLookups(t *testing.T) {
	ctx, repo := newTestRepository(t)

	admin, err := repo.FindUserByUsername(ctx, "admin")
	if err != nil {
		t.Fatalf("find admin user: %v", err)
	}
	if admin == nil {
		t.Fatal("expected seeded admin user")
	}
	if admin.ID != 101 || admin.DepartmentID != 101 || admin.DisplayName != "System Administrator" || admin.Status != "active" {
		t.Fatalf("unexpected admin user: %+v", admin)
	}
	if err := bcrypt.CompareHashAndPassword([]byte(admin.PasswordHash), []byte("password")); err != nil {
		t.Fatalf("expected seeded admin password hash to match password: %v", err)
	}

	roles, err := repo.ListRolesForUser(ctx, admin.ID)
	if err != nil {
		t.Fatalf("list admin roles: %v", err)
	}
	if len(roles) == 0 || roles[0].Code != "admin" {
		t.Fatalf("expected admin role, got %+v", roles)
	}

	permissions, err := repo.ListPermissionsForUser(ctx, admin.ID)
	if err != nil {
		t.Fatalf("list admin permissions: %v", err)
	}
	if len(permissions) == 0 {
		t.Fatal("expected seeded admin permissions")
	}

	departments, err := repo.ListDepartments(ctx)
	if err != nil {
		t.Fatalf("list departments: %v", err)
	}
	gotDepartmentIDs := make([]int64, 0, len(departments))
	for _, department := range departments {
		gotDepartmentIDs = append(gotDepartmentIDs, department.ID)
	}
	if !reflect.DeepEqual(gotDepartmentIDs, []int64{101, 102, 103}) {
		t.Fatalf("unexpected departments: %+v", departments)
	}

	stores, err := repo.ListStores(ctx)
	if err != nil {
		t.Fatalf("list stores: %v", err)
	}
	if len(stores) == 0 || stores[0].ID != 101 || stores[0].Code != "MI-001" {
		t.Fatalf("expected seeded stores, got %+v", stores)
	}

	users, err := repo.ListUsers(ctx)
	if err != nil {
		t.Fatalf("list users: %v", err)
	}
	if len(users) == 0 || users[0].Username != "admin" {
		t.Fatalf("expected seeded admin in users list, got %+v", users)
	}

	adminSummary, err := repo.GetUserByID(ctx, admin.ID)
	if err != nil {
		t.Fatalf("get admin by id: %v", err)
	}
	if adminSummary == nil {
		t.Fatal("expected admin summary by id")
	}
	if adminSummary.ID != admin.ID || adminSummary.Username != admin.Username || adminSummary.DisplayName != admin.DisplayName || adminSummary.DepartmentID != admin.DepartmentID {
		t.Fatalf("admin summary mismatch: %+v vs %+v", adminSummary, admin)
	}

	passwordHash, err := bcrypt.GenerateFromPassword([]byte("temp-password"), bcrypt.DefaultCost)
	if err != nil {
		t.Fatalf("hash password: %v", err)
	}
	createdUserID, err := repo.CreateUser(ctx, auth.CreateUserInput{
		DepartmentID: 102,
		Username:     "integration-user",
		DisplayName:  "Integration User",
		PasswordHash: string(passwordHash),
		Status:       "active",
	})
	if err != nil {
		t.Fatalf("create user: %v", err)
	}

	exists, err := repo.UsernameExists(ctx, "integration-user", 0)
	if err != nil {
		t.Fatalf("check username exists: %v", err)
	}
	if !exists {
		t.Fatal("expected created username to exist")
	}

	createdUser, err := repo.GetUserByID(ctx, createdUserID)
	if err != nil {
		t.Fatalf("get created user: %v", err)
	}
	if createdUser == nil {
		t.Fatal("expected created user")
	}
	if createdUser.DepartmentID != 102 || createdUser.Username != "integration-user" || createdUser.DisplayName != "Integration User" || createdUser.Status != "active" {
		t.Fatalf("unexpected created user: %+v", createdUser)
	}

	updatedName := "Integration User Updated"
	updatedDepartmentID := int64(103)
	if err := repo.UpdateUser(ctx, createdUserID, auth.UpdateUserInput{DepartmentID: &updatedDepartmentID, DisplayName: &updatedName}); err != nil {
		t.Fatalf("update user: %v", err)
	}
	updatedUser, err := repo.GetUserByID(ctx, createdUserID)
	if err != nil {
		t.Fatalf("get updated user: %v", err)
	}
	if updatedUser == nil || updatedUser.DepartmentID != updatedDepartmentID || updatedUser.DisplayName != updatedName {
		t.Fatalf("user update not persisted: %+v", updatedUser)
	}

	newPasswordHash, err := bcrypt.GenerateFromPassword([]byte("new-password"), bcrypt.DefaultCost)
	if err != nil {
		t.Fatalf("hash new password: %v", err)
	}
	if err := repo.SetUserPassword(ctx, createdUserID, string(newPasswordHash)); err != nil {
		t.Fatalf("set user password: %v", err)
	}
	persistedUser, err := repo.FindUserByUsername(ctx, "integration-user")
	if err != nil {
		t.Fatalf("reload created user: %v", err)
	}
	if persistedUser == nil || persistedUser.PasswordHash != string(newPasswordHash) {
		t.Fatalf("expected persisted password hash, got %+v", persistedUser)
	}

	allRoles, err := repo.ListRoles(ctx)
	if err != nil {
		t.Fatalf("list roles: %v", err)
	}
	if len(allRoles) < 3 {
		t.Fatalf("expected seeded roles, got %+v", allRoles)
	}
	roleIDs := []int64{allRoles[1].ID, allRoles[2].ID}
	if err := repo.SetUserRoles(ctx, createdUserID, roleIDs, updatedDepartmentID); err != nil {
		t.Fatalf("set user roles: %v", err)
	}
	gotRoleIDs, err := repo.GetUserRoleIDs(ctx, createdUserID)
	if err != nil {
		t.Fatalf("get user role ids: %v", err)
	}
	sort.Slice(gotRoleIDs, func(i, j int) bool { return gotRoleIDs[i] < gotRoleIDs[j] })
	sort.Slice(roleIDs, func(i, j int) bool { return roleIDs[i] < roleIDs[j] })
	if !reflect.DeepEqual(gotRoleIDs, roleIDs) {
		t.Fatalf("unexpected role ids: got %v want %v", gotRoleIDs, roleIDs)
	}
	adminExcludedExists, err := repo.UsernameExists(ctx, "integration-user", createdUserID)
	if err != nil {
		t.Fatalf("check username exists with exclusion: %v", err)
	}
	if adminExcludedExists {
		t.Fatal("expected username exclusion to ignore current user")
	}
}

func TestServiceIntegrationLoginAndSessionUser(t *testing.T) {
	ctx, repo := newTestRepository(t)
	service := auth.NewService(repo, config.AuthConfig{JWTSecret: "test-secret-key-for-integration-testing", TokenExpirySeconds: 24 * 60 * 60})

	token, sessionUser, err := service.Login(ctx, "admin", "password")
	if err != nil {
		t.Fatalf("login admin: %v", err)
	}
	if token == "" {
		t.Fatal("expected login token")
	}
	if sessionUser == nil {
		t.Fatal("expected session user")
	}
	if sessionUser.ID != 101 || sessionUser.Username != "admin" || sessionUser.DisplayName != "System Administrator" || sessionUser.DepartmentID != 101 {
		t.Fatalf("unexpected session user: %+v", sessionUser)
	}
	if len(sessionUser.Roles) == 0 || sessionUser.Roles[0] != "admin" {
		t.Fatalf("expected admin role in session user, got %+v", sessionUser.Roles)
	}
	if len(sessionUser.Permissions) == 0 {
		t.Fatal("expected permissions in session user")
	}

	claims, err := service.ParseToken(token)
	if err != nil {
		t.Fatalf("parse login token: %v", err)
	}
	if claims["username"] != "admin" {
		t.Fatalf("expected token username admin, got %v", claims["username"])
	}

	_, _, err = service.Login(ctx, "admin", "wrong-password")
	if !errors.Is(err, auth.ErrInvalidCredentials) {
		t.Fatalf("expected invalid credentials for wrong password, got %v", err)
	}

	admin, err := repo.FindUserByUsername(ctx, "admin")
	if err != nil {
		t.Fatalf("reload admin user: %v", err)
	}
	builtSessionUser, err := service.BuildSessionUser(ctx, admin)
	if err != nil {
		t.Fatalf("build session user: %v", err)
	}
	if builtSessionUser == nil || builtSessionUser.ID != admin.ID || builtSessionUser.Username != admin.Username || builtSessionUser.DisplayName != admin.DisplayName || builtSessionUser.DepartmentID != admin.DepartmentID {
		t.Fatalf("unexpected built session user: %+v", builtSessionUser)
	}
	if len(builtSessionUser.Roles) == 0 || len(builtSessionUser.Permissions) == 0 {
		t.Fatalf("expected populated roles and permissions, got %+v", builtSessionUser)
	}
}
