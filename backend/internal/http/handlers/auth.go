package handlers

import (
	"errors"
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/gin-gonic/gin"
)

type AuthHandler struct {
	service *auth.Service
}

func NewAuthHandler(service *auth.Service) *AuthHandler {
	return &AuthHandler{service: service}
}

type loginRequest struct {
	Username string `json:"username" binding:"required,min=2,max=50"`
	Password string `json:"password" binding:"required,min=6,max=100"`
}

// Login godoc
//
//	@Summary		Log in user
//	@Description	Authenticates a user and returns a bearer token plus the current user profile.
//	@Tags			Auth
//	@Accept			json
//	@Produce		json
//	@Param			request	body		loginRequest	true	"Login request"
//	@Success		200		{object}	swaggerEnvelope{token=string,user=auth.User}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Router			/auth/login [post]
func (h *AuthHandler) Login(c *gin.Context) {
	var request loginRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid login request"})
		return
	}

	token, user, err := h.service.Login(c.Request.Context(), request.Username, request.Password)
	if err != nil {
		if errors.Is(err, auth.ErrInvalidCredentials) {
			c.JSON(http.StatusUnauthorized, gin.H{"message": "invalid credentials"})
			return
		}
		c.JSON(http.StatusInternalServerError, gin.H{"message": "login failed"})
		return
	}

	c.JSON(http.StatusOK, gin.H{"token": token, "user": user})
}

// Me godoc
//
//	@Summary		Get current user
//	@Description	Returns the authenticated session user resolved from the bearer token.
//	@Tags			Auth
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{user=auth.SessionUser}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/auth/me [get]
func (h *AuthHandler) Me(c *gin.Context) {
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"user": sessionUser})
}
