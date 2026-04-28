package handlers

import (
	"errors"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/overtime"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/gin-gonic/gin"
)

type OvertimeHandler struct{ service *overtime.Service }

func NewOvertimeHandler(service *overtime.Service) *OvertimeHandler {
	return &OvertimeHandler{service: service}
}

type createOvertimeBillRequest struct {
	LeaseContractID int64                          `json:"lease_contract_id" binding:"required,gt=0"`
	PeriodStart     string                         `json:"period_start" binding:"required"`
	PeriodEnd       string                         `json:"period_end" binding:"required"`
	Note            string                         `json:"note" binding:"omitempty,max=255"`
	Formulas        []createOvertimeFormulaRequest `json:"formulas" binding:"required,min=1,dive"`
}

type createOvertimeFormulaRequest struct {
	ChargeType       string                             `json:"charge_type" binding:"required,min=1,max=32"`
	FormulaType      overtime.FormulaType               `json:"formula_type" binding:"required,oneof=fixed one_time percentage"`
	RateType         overtime.RateType                  `json:"rate_type" binding:"required,oneof=daily monthly"`
	EffectiveFrom    string                             `json:"effective_from"`
	EffectiveTo      string                             `json:"effective_to"`
	CurrencyTypeID   int64                              `json:"currency_type_id" binding:"required,gt=0"`
	TotalArea        float64                            `json:"total_area"`
	UnitPrice        float64                            `json:"unit_price"`
	BaseAmount       float64                            `json:"base_amount"`
	FixedRental      float64                            `json:"fixed_rental"`
	PercentageOption string                             `json:"percentage_option" binding:"omitempty,max=32"`
	MinimumOption    string                             `json:"minimum_option" binding:"omitempty,max=32"`
	PercentageTiers  []createOvertimePercentTierRequest `json:"percentage_tiers,omitempty"`
	MinimumTiers     []createOvertimeMinimumTierRequest `json:"minimum_tiers,omitempty"`
}

type createOvertimePercentTierRequest struct {
	SalesTo    float64 `json:"sales_to"`
	Percentage float64 `json:"percentage"`
}

type createOvertimeMinimumTierRequest struct {
	SalesTo    float64 `json:"sales_to"`
	MinimumSum float64 `json:"minimum_sum"`
}

type submitOvertimeBillRequest struct {
	IdempotencyKey string `json:"idempotency_key" binding:"required,min=1,max=100"`
	Comment        string `json:"comment" binding:"omitempty,max=500"`
}

// CreateBill godoc
//
//	@Summary		Create overtime bill draft
//	@Description	Creates a draft overtime bill with staged formula header/detail rows.
//	@Tags			Overtime
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createOvertimeBillRequest	true	"Overtime bill create request"
//	@Success		201		{object}	swaggerEnvelope{bill=overtime.Bill}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/overtime/bills [post]
func (h *OvertimeHandler) CreateBill(c *gin.Context) {
	var request createOvertimeBillRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill create request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	periodStart, err := time.Parse(overtime.DateLayout, request.PeriodStart)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime period_start"})
		return
	}
	periodEnd, err := time.Parse(overtime.DateLayout, request.PeriodEnd)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime period_end"})
		return
	}
	formulas := make([]overtime.FormulaInput, 0, len(request.Formulas))
	for _, formula := range request.Formulas {
		current := overtime.FormulaInput{
			ChargeType:       formula.ChargeType,
			FormulaType:      formula.FormulaType,
			RateType:         formula.RateType,
			CurrencyTypeID:   formula.CurrencyTypeID,
			TotalArea:        formula.TotalArea,
			UnitPrice:        formula.UnitPrice,
			BaseAmount:       formula.BaseAmount,
			FixedRental:      formula.FixedRental,
			PercentageOption: formula.PercentageOption,
			MinimumOption:    formula.MinimumOption,
		}
		if formula.EffectiveFrom != "" {
			value, parseErr := time.Parse(overtime.DateLayout, formula.EffectiveFrom)
			if parseErr != nil {
				c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime formula effective_from"})
				return
			}
			current.EffectiveFrom = value
		}
		if formula.EffectiveTo != "" {
			value, parseErr := time.Parse(overtime.DateLayout, formula.EffectiveTo)
			if parseErr != nil {
				c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime formula effective_to"})
				return
			}
			current.EffectiveTo = value
		}
		for _, tier := range formula.PercentageTiers {
			current.PercentageTiers = append(current.PercentageTiers, overtime.PercentTierInput{SalesTo: tier.SalesTo, Percentage: tier.Percentage})
		}
		for _, tier := range formula.MinimumTiers {
			current.MinimumTiers = append(current.MinimumTiers, overtime.MinimumTierInput{SalesTo: tier.SalesTo, MinimumSum: tier.MinimumSum})
		}
		formulas = append(formulas, current)
	}
	bill, err := h.service.CreateBill(c.Request.Context(), overtime.CreateBillInput{LeaseContractID: request.LeaseContractID, PeriodStart: periodStart, PeriodEnd: periodEnd, Note: request.Note, ActorUserID: sessionUser.ID, Formulas: formulas})
	if err != nil {
		h.renderOvertimeError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"bill": bill})
}

// GetBill godoc
//
//	@Summary		Get overtime bill
//	@Description	Returns an overtime bill with formulas and generation history by ID.
//	@Tags			Overtime
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Overtime bill ID"
//	@Success		200	{object}	swaggerEnvelope{bill=overtime.Bill}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/overtime/bills/{id} [get]
func (h *OvertimeHandler) GetBill(c *gin.Context) {
	billID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill id"})
		return
	}
	bill, err := h.service.GetBill(c.Request.Context(), billID)
	if err != nil {
		h.renderOvertimeError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"bill": bill})
}

// ListBills godoc
//
//	@Summary		List overtime bills
//	@Description	Returns paginated overtime bills filtered by lease, status, and billing period.
//	@Tags			Overtime
//	@Accept			json
//	@Produce		json
//	@Param			lease_contract_id	query		int		false	"Lease contract ID"
//	@Param			status				query		string	false	"Overtime bill status"
//	@Param			period_start		query		string	false	"Period start (YYYY-MM-DD)"
//	@Param			period_end			query		string	false	"Period end (YYYY-MM-DD)"
//	@Param			page				query		int		false	"Page number"
//	@Param			page_size			query		int		false	"Page size"
//	@Success		200				{object}	swaggerEnvelope{items=[]overtime.Bill,total=int,page=int,page_size=int}
//	@Failure		400				{object}	swaggerMessageResponse
//	@Failure		401				{object}	swaggerMessageResponse
//	@Failure		500				{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/overtime/bills [get]
func (h *OvertimeHandler) ListBills(c *gin.Context) {
	filter := overtime.ListFilter{}
	if leaseIDText := c.Query("lease_contract_id"); leaseIDText != "" {
		leaseID, err := strconv.ParseInt(leaseIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease contract id"})
			return
		}
		filter.LeaseContractID = &leaseID
	}
	if status := c.Query("status"); status != "" {
		value := overtime.BillStatus(status)
		filter.Status = &value
	}
	if periodStartText := c.Query("period_start"); periodStartText != "" {
		value, err := time.Parse(overtime.DateLayout, periodStartText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime period_start"})
			return
		}
		filter.PeriodStart = &value
	}
	if periodEndText := c.Query("period_end"); periodEndText != "" {
		value, err := time.Parse(overtime.DateLayout, periodEndText)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime period_end"})
			return
		}
		filter.PeriodEnd = &value
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
	result, err := h.service.ListBills(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list overtime bills"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[overtime.Bill]{Items: []overtime.Bill{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []overtime.Bill{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

func (h *OvertimeHandler) SubmitBill(c *gin.Context) {
	billID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill id"})
		return
	}
	var request submitOvertimeBillRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill submit request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	bill, err := h.service.SubmitForApproval(c.Request.Context(), overtime.SubmitInput{BillID: billID, ActorUserID: sessionUser.ID, DepartmentID: sessionUser.DepartmentID, IdempotencyKey: request.IdempotencyKey, Comment: request.Comment})
	if err != nil {
		h.renderOvertimeError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"bill": bill})
}

func (h *OvertimeHandler) CancelBill(c *gin.Context) {
	billID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill id"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	bill, err := h.service.Cancel(c.Request.Context(), overtime.CancelInput{BillID: billID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderOvertimeError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"bill": bill})
}

func (h *OvertimeHandler) StopBill(c *gin.Context) {
	billID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill id"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	bill, err := h.service.Stop(c.Request.Context(), overtime.StopInput{BillID: billID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderOvertimeError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"bill": bill})
}

func (h *OvertimeHandler) GenerateCharges(c *gin.Context) {
	billID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid overtime bill id"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	result, err := h.service.GenerateCharges(c.Request.Context(), overtime.GenerateInput{BillID: billID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.renderOvertimeError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"run": result.Run, "charges": result.Charges, "skipped": result.Skipped, "totals": result.Totals})
}

func (h *OvertimeHandler) renderOvertimeError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	if errors.Is(err, overtime.ErrInvalidOvertimeInput) || errors.Is(err, overtime.ErrInvalidOvertimeState) || errors.Is(err, overtime.ErrOvertimeAlreadySubmitted) || errors.Is(err, overtime.ErrOvertimeFormulaRequired) || errors.Is(err, overtime.ErrOvertimeFormulaInvalid) || errors.Is(err, overtime.ErrOvertimeDuplicateBill) || errors.Is(err, overtime.ErrOvertimeGenerationBlocked) || errors.Is(err, overtime.ErrOvertimeGenerationLocked) || errors.Is(err, overtime.ErrOvertimeWorkflowRequired) {
		status = http.StatusBadRequest
	} else if errors.Is(err, overtime.ErrOvertimeNotFound) {
		status = http.StatusNotFound
	}
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
