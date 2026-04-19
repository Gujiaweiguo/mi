package handlers

import (
	"context"
	"database/sql"
	"net/http"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/config"
	"github.com/gin-gonic/gin"
)

type HealthHandler struct {
	config *config.Config
	db     *sql.DB
}

func NewHealthHandler(cfg *config.Config, db *sql.DB) *HealthHandler {
	return &HealthHandler{config: cfg, db: db}
}

// Get godoc
//
//	@Summary		Check service health
//	@Description	Verifies database connectivity and returns service health metadata.
//	@Tags			Health
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{status=string,service=string,environment=string}
//	@Failure		500	{object}	swaggerMessageResponse
//	@Failure		503	{object}	swaggerEnvelope{status=string,service=string,environment=string}
//	@Router			/health [get]
func (h *HealthHandler) Get(c *gin.Context) {
	pingCtx, cancel := context.WithTimeout(c.Request.Context(), 2*time.Second)
	defer cancel()

	if err := h.db.PingContext(pingCtx); err != nil {
		c.JSON(http.StatusServiceUnavailable, gin.H{
			"status":      "degraded",
			"service":     h.config.App.Name,
			"environment": h.config.App.Environment,
		})
		return
	}

	c.JSON(http.StatusOK, gin.H{
		"status":      "ok",
		"service":     h.config.App.Name,
		"environment": h.config.App.Environment,
	})
}
