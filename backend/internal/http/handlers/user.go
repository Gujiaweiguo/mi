package handlers

import (
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/gin-gonic/gin"
	"golang.org/x/crypto/bcrypt"
)

type UserHandler struct {
	repo *auth.Repository
}

func NewUserHandler(repo *auth.Repository) *UserHandler {
	return &UserHandler{repo: repo}
}

type createUserRequest struct {
	Username     string  `json:"username" binding:"required"`
	DisplayName  string  `json:"display_name" binding:"required"`
	Password     string  `json:"password" binding:"required,min=6"`
	DepartmentID int64   `json:"department_id" binding:"required"`
	RoleIDs      []int64 `json:"role_ids"`
}

type updateUserRequest struct {
	DisplayName  *string `json:"display_name"`
	DepartmentID *int64  `json:"department_id"`
	Status       *string `json:"status"`
}

type resetPasswordRequest struct {
	NewPassword string `json:"new_password" binding:"required,min=6"`
}

type setUserRolesRequest struct {
	RoleIDs      []int64 `json:"role_ids" binding:"required"`
	DepartmentID int64   `json:"department_id" binding:"required"`
}

// List returns all users.
func (h *UserHandler) List(c *gin.Context) {
	users, err := h.repo.ListUsers(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"users": users})
}

// Get returns a single user by ID.
func (h *UserHandler) Get(c *gin.Context) {
	id, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid user id"})
		return
	}
	user, err := h.repo.GetUserByID(c.Request.Context(), id)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	if user == nil {
		c.JSON(http.StatusNotFound, gin.H{"message": "user not found"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"user": user})
}

// Create creates a new user.
func (h *UserHandler) Create(c *gin.Context) {
	var req createUserRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid create user request"})
		return
	}

	exists, err := h.repo.UsernameExists(c.Request.Context(), req.Username, 0)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	if exists {
		c.JSON(http.StatusConflict, gin.H{"message": "username already exists"})
		return
	}

	hash, err := bcrypt.GenerateFromPassword([]byte(req.Password), bcrypt.DefaultCost)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to hash password"})
		return
	}

	id, err := h.repo.CreateUser(c.Request.Context(), auth.CreateUserInput{
		DepartmentID: req.DepartmentID,
		Username:     req.Username,
		DisplayName:  req.DisplayName,
		PasswordHash: string(hash),
		Status:       "active",
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}

	if len(req.RoleIDs) > 0 {
		_ = h.repo.SetUserRoles(c.Request.Context(), id, req.RoleIDs, req.DepartmentID)
	}

	user, _ := h.repo.GetUserByID(c.Request.Context(), id)
	c.JSON(http.StatusCreated, gin.H{"user": user})
}

// Update updates user fields.
func (h *UserHandler) Update(c *gin.Context) {
	id, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid user id"})
		return
	}
	var req updateUserRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid update user request"})
		return
	}

	if err := h.repo.UpdateUser(c.Request.Context(), id, auth.UpdateUserInput{
		DepartmentID: req.DepartmentID,
		DisplayName:  req.DisplayName,
		Status:       req.Status,
	}); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}

	user, _ := h.repo.GetUserByID(c.Request.Context(), id)
	c.JSON(http.StatusOK, gin.H{"user": user})
}

// ResetPassword resets a user's password.
func (h *UserHandler) ResetPassword(c *gin.Context) {
	id, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid user id"})
		return
	}
	var req resetPasswordRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid reset password request"})
		return
	}

	hash, err := bcrypt.GenerateFromPassword([]byte(req.NewPassword), bcrypt.DefaultCost)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to hash password"})
		return
	}

	if err := h.repo.SetUserPassword(c.Request.Context(), id, string(hash)); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"message": "password reset successful"})
}

// SetRoles assigns roles to a user.
func (h *UserHandler) SetRoles(c *gin.Context) {
	id, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid user id"})
		return
	}
	var req setUserRolesRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid set roles request"})
		return
	}

	if err := h.repo.SetUserRoles(c.Request.Context(), id, req.RoleIDs, req.DepartmentID); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"message": "roles updated"})
}

// ListRoles returns all roles.
func (h *UserHandler) ListRoles(c *gin.Context) {
	roles, err := h.repo.ListRoles(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"roles": roles})
}
