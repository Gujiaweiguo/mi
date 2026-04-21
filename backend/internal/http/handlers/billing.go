package handlers

import (
	"errors"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/billing"
	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/gin-gonic/gin"
)

type BillingHandler struct {
	service *billing.Service
}

func NewBillingHandler(service *billing.Service) *BillingHandler {
	return &BillingHandler{service: service}
}

type generateBillingRequest struct {
	PeriodStart string `json:"period_start" binding:"required"`
	PeriodEnd   string `json:"period_end" binding:"required"`
}

// GenerateCharges godoc
//
//	@Summary		Generate billing charges
//	@Description	Generates billing charge lines for the requested billing window.
//	@Tags			Billing
//	@Accept			json
//	@Produce		json
//	@Param			request	body		generateBillingRequest	true	"Billing generation request"
//	@Success		201		{object}	billing.GenerateResult
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/billing/charges/generate [post]
func (h *BillingHandler) GenerateCharges(c *gin.Context) {
	var request generateBillingRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing generation request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	periodStart, err := time.Parse(billing.DateLayout, request.PeriodStart)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing period_start"})
		return
	}
	periodEnd, err := time.Parse(billing.DateLayout, request.PeriodEnd)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing period_end"})
		return
	}

	result, err := h.service.GenerateCharges(c.Request.Context(), billing.GenerateInput{PeriodStart: periodStart, PeriodEnd: periodEnd, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderBillingError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"run": result.Run, "lines": result.Lines, "totals": result.Totals})
}

// ListCharges godoc
//
//	@Summary		List billing charges
//	@Description	Returns paginated billing charge lines filtered by lease and billing period.
//	@Tags			Billing
//	@Accept			json
//	@Produce		json
//	@Param			lease_contract_id	query		int		false	"Lease contract ID"
//	@Param			period_start		query		string	false	"Billing period start (YYYY-MM-DD)"
//	@Param			period_end			query		string	false	"Billing period end (YYYY-MM-DD)"
//	@Param			page				query		int		false	"Page number"
//	@Param			page_size			query		int		false	"Page size"
//	@Success		200					{object}	swaggerEnvelope{items=[]billing.ChargeLine,total=int,page=int,page_size=int}
//	@Failure		400					{object}	swaggerMessageResponse
//	@Failure		401					{object}	swaggerMessageResponse
//	@Failure		500					{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/billing/charges [get]
func (h *BillingHandler) ListCharges(c *gin.Context) {
	filter := billing.ChargeListFilter{}
	if leaseIDText := c.Query("lease_contract_id"); leaseIDText != "" {
		leaseID, err := strconv.ParseInt(leaseIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease contract id"})
			return
		}
		filter.LeaseContractID = &leaseID
	}
	if periodStartText := c.Query("period_start"); periodStartText != "" {
		periodStart, err := time.Parse(billing.DateLayout, periodStartText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing period_start"})
			return
		}
		filter.PeriodStart = &periodStart
	}
	if periodEndText := c.Query("period_end"); periodEndText != "" {
		periodEnd, err := time.Parse(billing.DateLayout, periodEndText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid billing period_end"})
			return
		}
		filter.PeriodEnd = &periodEnd
	}
	if pageText := c.DefaultQuery("page", strconv.Itoa(pagination.DefaultPage)); pageText != "" {
		page, err := strconv.Atoi(pageText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page"})
			return
		}
		filter.Page = page
	}
	if pageSizeText := c.DefaultQuery("page_size", strconv.Itoa(pagination.DefaultPageSize)); pageSizeText != "" {
		pageSize, err := strconv.Atoi(pageSizeText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page size"})
			return
		}
		filter.PageSize = pageSize
	}

	result, err := h.service.ListChargeLines(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list billing charges"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[billing.ChargeLine]{Items: []billing.ChargeLine{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []billing.ChargeLine{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

func (h *BillingHandler) renderBillingError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	if errors.Is(err, billing.ErrInvalidBillingWindow) {
		status = http.StatusBadRequest
	}
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
