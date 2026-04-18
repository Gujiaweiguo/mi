package handlers

import (
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/dashboard"
	"github.com/gin-gonic/gin"
)

type DashboardHandler struct {
	dashboardSvc *dashboard.DashboardService
}

func NewDashboardHandler(svc *dashboard.DashboardService) *DashboardHandler {
	return &DashboardHandler{dashboardSvc: svc}
}

func (h *DashboardHandler) Summary(c *gin.Context) {
	result, err := h.dashboardSvc.GetSummary(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, gin.H{"summary": result})
}
