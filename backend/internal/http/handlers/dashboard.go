package handlers

import (
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/dashboard"
	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/gin-gonic/gin"
)

type DashboardHandler struct {
	dashboardSvc *dashboard.DashboardService
}

func NewDashboardHandler(svc *dashboard.DashboardService) *DashboardHandler {
	return &DashboardHandler{dashboardSvc: svc}
}

// Summary godoc
//
//	@Summary		Get dashboard summary
//	@Description	Returns high-level dashboard metrics for leases, invoices, receivables, and workflows.
//	@Tags			Dashboard
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{summary=dashboard.DashboardSummary}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/dashboard/summary [get]
func (h *DashboardHandler) Summary(c *gin.Context) {
	result, err := h.dashboardSvc.GetSummary(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"summary": result})
}

// Workbench godoc
//
//	@Summary		Get workbench queue aggregation
//	@Description	Returns queue-oriented workbench data for approvals, receivables, overdue receivables, and active leases.
//	@Tags			Dashboard
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{workbench=dashboard.WorkbenchAggregate}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/dashboard/workbench [get]
func (h *DashboardHandler) Workbench(c *gin.Context) {
	result, err := h.dashboardSvc.GetWorkbenchAggregate(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": errutil.SafeMessage(err)})
		return
	}
	c.JSON(http.StatusOK, gin.H{"workbench": result})
}
