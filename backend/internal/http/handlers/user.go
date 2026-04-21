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
	Username     string  `json:"username" binding:"required,min=2,max=50"`
	DisplayName  string  `json:"display_name" binding:"required,min=1,max=100"`
	Password     string  `json:"password" binding:"required,min=6,max=100"`
	DepartmentID int64   `json:"department_id" binding:"required,gt=0"`
	RoleIDs      []int64 `json:"role_ids"`
}

type updateUserRequest struct {
	DisplayName  *string `json:"display_name" binding:"omitempty,min=1,max=100"`
	DepartmentID *int64  `json:"department_id" binding:"omitempty,gt=0"`
	Status       *string `json:"status" binding:"omitempty,oneof=active inactive disabled"`
}

type resetPasswordRequest struct {
	NewPassword string `json:"new_password" binding:"required,min=6,max=100"`
}

type setUserRolesRequest struct {
	RoleIDs      []int64 `json:"role_ids" binding:"required"`
	DepartmentID int64   `json:"department_id" binding:"required,gt=0"`
}

// List godoc
//
//	@Summary		List users
//	@Description	Returns all registered users.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{users=[]auth.User}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/users [get]
func (h *UserHandler) List(c *gin.Context) {
	users, err := h.repo.ListUsers(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"users": users})
}

// Get godoc
//
//	@Summary		Get user by ID
//	@Description	Returns a single user by their unique identifier.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"User ID"
//	@Success		200	{object}	swaggerEnvelope{user=auth.User}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/users/{id} [get]
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

// Create godoc
//
//	@Summary		Create user
//	@Description	Creates a new user account with the provided credentials and role assignments.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createUserRequest	true	"Create user request"
//	@Success		201		{object}	swaggerEnvelope{user=auth.User}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		409		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/users [post]
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

// Update godoc
//
//	@Summary		Update user
//	@Description	Updates display name, department, or status of an existing user.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"User ID"
//	@Param			request	body		updateUserRequest	true	"Update user request"
//	@Success		200		{object}	swaggerEnvelope{user=auth.User}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/users/{id} [put]
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

// ResetPassword godoc
//
//	@Summary		Reset user password
//	@Description	Resets a user's password to the provided new value.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"User ID"
//	@Param			request	body		resetPasswordRequest	true	"Reset password request"
//	@Success		200		{object}	swaggerMessageResponse
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/users/{id}/reset-password [post]
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

// SetRoles godoc
//
//	@Summary		Set user roles
//	@Description	Assigns roles to a user within a department context.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"User ID"
//	@Param			request	body		setUserRolesRequest	true	"Set roles request"
//	@Success		200		{object}	swaggerMessageResponse
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/users/{id}/roles [put]
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

// ListRoles godoc
//
//	@Summary		List roles
//	@Description	Returns all available roles in the system.
//	@Tags			Users
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{roles=[]auth.Role}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/roles [get]
func (h *UserHandler) ListRoles(c *gin.Context) {
	roles, err := h.repo.ListRoles(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"roles": roles})
}
