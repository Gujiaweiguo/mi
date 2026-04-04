package middleware

import (
	"net/http"
	"strings"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/gin-gonic/gin"
)

const sessionUserContextKey = "session_user"

func RequireAuth(service *auth.Service, repository *auth.Repository) gin.HandlerFunc {
	return func(c *gin.Context) {
		authorization := c.GetHeader("Authorization")
		if !strings.HasPrefix(authorization, "Bearer ") {
			c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"message": "missing bearer token"})
			return
		}

		token := strings.TrimPrefix(authorization, "Bearer ")
		claims, err := service.ParseToken(token)
		if err != nil {
			c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"message": "invalid token"})
			return
		}

		username, ok := claims["username"].(string)
		if !ok || username == "" {
			c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"message": "invalid token username"})
			return
		}

		user, err := repository.FindUserByUsername(c.Request.Context(), username)
		if err != nil || user == nil {
			c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"message": "user not found"})
			return
		}

		sessionUser, err := service.BuildSessionUser(c.Request.Context(), user)
		if err != nil {
			c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"message": "failed to build session user"})
			return
		}

		c.Set(sessionUserContextKey, sessionUser)
		c.Next()
	}
}

func RequirePermission(functionCode, action string, service *auth.Service) gin.HandlerFunc {
	return func(c *gin.Context) {
		value, exists := c.Get(sessionUserContextKey)
		if !exists {
			c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
			return
		}

		sessionUser, ok := value.(*auth.SessionUser)
		if !ok || !service.Can(sessionUser.Permissions, functionCode, action) {
			c.AbortWithStatusJSON(http.StatusForbidden, gin.H{"message": "forbidden"})
			return
		}

		c.Next()
	}
}

func CurrentSessionUser(c *gin.Context) (*auth.SessionUser, bool) {
	value, exists := c.Get(sessionUserContextKey)
	if !exists {
		return nil, false
	}
	sessionUser, ok := value.(*auth.SessionUser)
	return sessionUser, ok
}
