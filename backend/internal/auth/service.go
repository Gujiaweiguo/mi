package auth

import (
	"context"
	"errors"
	"fmt"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/golang-jwt/jwt/v5"
	"golang.org/x/crypto/bcrypt"
)

var ErrInvalidCredentials = errors.New("invalid credentials")

type Service struct {
	repository *Repository
	config     config.AuthConfig
}

func NewService(repository *Repository, cfg config.AuthConfig) *Service {
	return &Service{repository: repository, config: cfg}
}

func (s *Service) Login(ctx context.Context, username, password string) (string, *SessionUser, error) {
	user, err := s.repository.FindUserByUsername(ctx, username)
	if err != nil {
		return "", nil, err
	}
	if user == nil || user.Status != "active" {
		return "", nil, ErrInvalidCredentials
	}
	if err := bcrypt.CompareHashAndPassword([]byte(user.PasswordHash), []byte(password)); err != nil {
		return "", nil, ErrInvalidCredentials
	}
	sessionUser, err := s.BuildSessionUser(ctx, user)
	if err != nil {
		return "", nil, err
	}
	token, err := s.issueToken(sessionUser)
	if err != nil {
		return "", nil, err
	}
	return token, sessionUser, nil
}

func (s *Service) BuildSessionUser(ctx context.Context, user *User) (*SessionUser, error) {
	roles, err := s.repository.ListRolesForUser(ctx, user.ID)
	if err != nil {
		return nil, err
	}
	permissions, err := s.repository.ListPermissionsForUser(ctx, user.ID)
	if err != nil {
		return nil, err
	}
	roleCodes := make([]string, 0, len(roles))
	for _, role := range roles {
		roleCodes = append(roleCodes, role.Code)
	}
	return &SessionUser{ID: user.ID, Username: user.Username, DisplayName: user.DisplayName, DepartmentID: user.DepartmentID, Roles: roleCodes, Permissions: permissions}, nil
}

func (s *Service) issueToken(user *SessionUser) (string, error) {
	claims := jwt.MapClaims{
		"sub":           user.ID,
		"username":      user.Username,
		"department_id": user.DepartmentID,
		"roles":         user.Roles,
		"exp":           time.Now().Add(time.Duration(s.config.TokenExpirySeconds) * time.Second).Unix(),
	}
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	signedToken, err := token.SignedString([]byte(s.config.JWTSecret))
	if err != nil {
		return "", fmt.Errorf("sign token: %w", err)
	}
	return signedToken, nil
}

func (s *Service) ParseToken(tokenString string) (jwt.MapClaims, error) {
	token, err := jwt.Parse(tokenString, func(token *jwt.Token) (any, error) {
		if token.Method.Alg() != jwt.SigningMethodHS256.Alg() {
			return nil, fmt.Errorf("unexpected jwt signing method %s", token.Method.Alg())
		}
		return []byte(s.config.JWTSecret), nil
	})
	if err != nil {
		return nil, fmt.Errorf("parse token: %w", err)
	}
	claims, ok := token.Claims.(jwt.MapClaims)
	if !ok || !token.Valid {
		return nil, ErrInvalidCredentials
	}
	return claims, nil
}

func (s *Service) Can(permissions []Permission, functionCode, action string) bool {
	for _, permission := range permissions {
		if permission.FunctionCode != functionCode {
			continue
		}
		switch action {
		case "view":
			return permission.PermissionLevel != ""
		case "edit":
			return permission.PermissionLevel == "edit" || permission.PermissionLevel == "approve"
		case "approve":
			return permission.PermissionLevel == "approve"
		case "print":
			return permission.CanPrint
		case "export":
			return permission.CanExport
		}
	}
	return false
}
