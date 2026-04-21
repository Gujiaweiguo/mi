package handlers

import (
	"errors"
	"net/http"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/reporting"
	"github.com/gin-gonic/gin"
)

type ReportingHandler struct{ service *reporting.Service }

func NewReportingHandler(service *reporting.Service) *ReportingHandler {
	return &ReportingHandler{service: service}
}

type reportRequest struct {
	Period           string  `json:"period"`
	StoreID          *int64  `json:"store_id"`
	FloorID          *int64  `json:"floor_id"`
	AreaID           *int64  `json:"area_id"`
	UnitID           *int64  `json:"unit_id"`
	DepartmentID     *int64  `json:"department_id"`
	ShopTypeID       *int64  `json:"shop_type_id"`
	CustomerID       *int64  `json:"customer_id"`
	BrandID          *int64  `json:"brand_id"`
	TradeID          *int64  `json:"trade_id"`
	ChargeType       *string `json:"charge_type"`
	ManagementTypeID *int64  `json:"management_type_id"`
	Status           *string `json:"status"`
}

// Query godoc
//
//	@Summary		Query report
//	@Description	Runs a generalize report query for the supplied report ID and filters.
//	@Tags			Reports
//	@Accept			json
//	@Produce		json
//	@Param			reportId	path		string			true	"Report ID"
//	@Param			request		body		reportRequest	true	"Report query request"
//	@Success		200			{object}	swaggerEnvelope{report=reporting.Result}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		404			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/reports/{reportId}/query [post]
func (h *ReportingHandler) Query(c *gin.Context) {
	input, ok := h.buildInput(c)
	if !ok {
		return
	}
	result, err := h.service.QueryReport(c.Request.Context(), input)
	if err != nil {
		h.renderReportingError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"report": result})
}

// Export godoc
//
//	@Summary		Export report
//	@Description	Exports a generalize report for the supplied report ID and filters.
//	@Tags			Reports
//	@Accept			json
//	@Produce		application/octet-stream
//	@Param			reportId	path		string			true	"Report ID"
//	@Param			request		body		reportRequest	true	"Report export request"
//	@Success		200			{file}		file			"Report export file"
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		404			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/reports/{reportId}/export [post]
func (h *ReportingHandler) Export(c *gin.Context) {
	input, ok := h.buildInput(c)
	if !ok {
		return
	}
	artifact, err := h.service.ExportReport(c.Request.Context(), input)
	if err != nil {
		h.renderReportingError(c, err)
		return
	}
	c.Header("Content-Disposition", "attachment; filename="+artifact.FileName)
	c.Data(http.StatusOK, artifact.ContentType, artifact.Bytes)
}

func (h *ReportingHandler) buildInput(c *gin.Context) (reporting.QueryInput, bool) {
	var request reportRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid report request"})
		return reporting.QueryInput{}, false
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return reporting.QueryInput{}, false
	}
	reportID := reporting.ReportID(c.Param("reportId"))
	periodStart, periodEnd, periodLabel := time.Time{}, time.Time{}, "visual"
	if reportID != reporting.ReportR19 {
		var err error
		periodStart, periodEnd, periodLabel, err = reporting.ParsePeriod(request.Period)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": err.Error()})
			return reporting.QueryInput{}, false
		}
	}
	return reporting.QueryInput{
		ReportID:         reportID,
		PeriodStart:      periodStart,
		PeriodEnd:        periodEnd,
		PeriodLabel:      periodLabel,
		StoreID:          request.StoreID,
		FloorID:          request.FloorID,
		AreaID:           request.AreaID,
		UnitID:           request.UnitID,
		DepartmentID:     request.DepartmentID,
		ShopTypeID:       request.ShopTypeID,
		CustomerID:       request.CustomerID,
		BrandID:          request.BrandID,
		TradeID:          request.TradeID,
		ChargeType:       request.ChargeType,
		ManagementTypeID: request.ManagementTypeID,
		Status:           request.Status,
		RequestedByID:    sessionUser.ID,
	}, true
}

func (h *ReportingHandler) renderReportingError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	if errors.Is(err, reporting.ErrInvalidPeriod) || errors.Is(err, reporting.ErrUnsupportedReport) {
		status = http.StatusBadRequest
	}
	c.JSON(status, gin.H{"message": err.Error()})
}
