package handlers

import (
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/gin-gonic/gin"
)

type HealthHandler struct {
	config *config.Config
}

func NewHealthHandler(cfg *config.Config) *HealthHandler {
	return &HealthHandler{config: cfg}
}

func (h *HealthHandler) Get(c *gin.Context) {
	c.JSON(http.StatusOK, gin.H{
		"status":      "ok",
		"service":     h.config.App.Name,
		"environment": h.config.App.Environment,
	})
}
