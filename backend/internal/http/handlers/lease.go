package handlers

import (
	"errors"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/lease"
	"github.com/Gujiaweiguo/mi/backend/internal/pagination"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type LeaseHandler struct {
	service *lease.Service
}

func NewLeaseHandler(service *lease.Service) *LeaseHandler {
	return &LeaseHandler{service: service}
}

type createLeaseRequest struct {
	LeaseNo          string                   `json:"lease_no" binding:"required"`
	DepartmentID     int64                    `json:"department_id" binding:"required"`
	StoreID          int64                    `json:"store_id" binding:"required"`
	BuildingID       *int64                   `json:"building_id"`
	CustomerID       *int64                   `json:"customer_id"`
	BrandID          *int64                   `json:"brand_id"`
	TradeID          *int64                   `json:"trade_id"`
	ManagementTypeID *int64                   `json:"management_type_id"`
	TenantName       string                   `json:"tenant_name" binding:"required"`
	StartDate        string                   `json:"start_date" binding:"required"`
	EndDate          string                   `json:"end_date" binding:"required"`
	Units            []createLeaseUnitRequest `json:"units" binding:"required"`
	Terms            []createLeaseTermRequest `json:"terms" binding:"required"`
}

type createLeaseUnitRequest struct {
	UnitID   int64   `json:"unit_id" binding:"required"`
	RentArea float64 `json:"rent_area" binding:"required"`
}

type createLeaseTermRequest struct {
	TermType       lease.TermType     `json:"term_type" binding:"required"`
	BillingCycle   lease.BillingCycle `json:"billing_cycle" binding:"required"`
	CurrencyTypeID int64              `json:"currency_type_id" binding:"required"`
	Amount         float64            `json:"amount" binding:"required"`
	EffectiveFrom  string             `json:"effective_from" binding:"required"`
	EffectiveTo    string             `json:"effective_to" binding:"required"`
}

type submitLeaseRequest struct {
	Comment        string `json:"comment"`
	IdempotencyKey string `json:"idempotency_key" binding:"required"`
}

type terminateLeaseRequest struct {
	TerminatedAt string `json:"terminated_at" binding:"required"`
}

// Create godoc
//
//	@Summary		Create lease draft
//	@Description	Creates a draft lease contract with units and billing terms.
//	@Tags			Lease
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createLeaseRequest	true	"Lease create request"
//	@Success		201		{object}	swaggerEnvelope{lease=lease.Contract}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/leases [post]
func (h *LeaseHandler) Create(c *gin.Context) {
	var request createLeaseRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease create request"})
		return
	}

	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}

	input, ok := h.bindCreateDraftInput(c, request, sessionUser.ID)
	if !ok {
		return
	}

	contract, err := h.service.CreateDraft(c.Request.Context(), input)
	if err != nil {
		h.renderLeaseError(c, err)
		return
	}

	c.JSON(http.StatusCreated, gin.H{"lease": contract})
}

// Amend godoc
//
//	@Summary		Create lease amendment
//	@Description	Creates an amended lease draft from an existing lease contract.
//	@Tags			Lease
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Lease ID"
//	@Param			request	body		createLeaseRequest	true	"Lease amendment request"
//	@Success		201		{object}	swaggerEnvelope{lease=lease.Contract}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/leases/{id}/amend [post]
func (h *LeaseHandler) Amend(c *gin.Context) {
	leaseID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease id"})
		return
	}

	var request createLeaseRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease amend request"})
		return
	}

	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}

	input, ok := h.bindCreateDraftInput(c, request, sessionUser.ID)
	if !ok {
		return
	}

	contract, err := h.service.CreateAmendment(c.Request.Context(), lease.AmendInput{LeaseID: leaseID, CreateDraftInput: input})
	if err != nil {
		h.renderLeaseError(c, err)
		return
	}

	c.JSON(http.StatusCreated, gin.H{"lease": contract})
}

// Get godoc
//
//	@Summary		Get lease
//	@Description	Returns a lease contract with units and terms by ID.
//	@Tags			Lease
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Lease ID"
//	@Success		200	{object}	swaggerEnvelope{lease=lease.Contract}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/leases/{id} [get]
func (h *LeaseHandler) Get(c *gin.Context) {
	leaseID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease id"})
		return
	}

	contract, err := h.service.GetLease(c.Request.Context(), leaseID)
	if err != nil {
		h.renderLeaseError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"lease": contract})
}

// List godoc
//
//	@Summary		List leases
//	@Description	Returns paginated lease summaries filtered by lease number, status, and store.
//	@Tags			Lease
//	@Accept			json
//	@Produce		json
//	@Param			lease_no	query		string	false	"Lease number"
//	@Param			status		query		string	false	"Lease status"
//	@Param			store_id	query		int		false	"Store ID"
//	@Param			page		query		int		false	"Page number"
//	@Param			page_size	query		int		false	"Page size"
//	@Success		200			{object}	swaggerEnvelope{items=[]lease.Summary,total=int,page=int,page_size=int}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/leases [get]
func (h *LeaseHandler) List(c *gin.Context) {
	filter := lease.ListFilter{LeaseNo: c.Query("lease_no")}
	if status := c.Query("status"); status != "" {
		statusValue := lease.Status(status)
		filter.Status = &statusValue
	}
	if storeIDText := c.Query("store_id"); storeIDText != "" {
		storeID, err := strconv.ParseInt(storeIDText, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid store id"})
			return
		}
		filter.StoreID = &storeID
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

	result, err := h.service.ListLeases(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list leases"})
		return
	}
	if result == nil {
		result = &pagination.ListResult[lease.Summary]{Items: []lease.Summary{}, Total: 0, Page: pagination.DefaultPage, PageSize: pagination.DefaultPageSize}
	}
	if result.Items == nil {
		result.Items = []lease.Summary{}
	}
	c.JSON(http.StatusOK, gin.H{"items": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

// Submit godoc
//
//	@Summary		Submit lease for approval
//	@Description	Submits a draft lease contract into the workflow approval process.
//	@Tags			Lease
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Lease ID"
//	@Param			request	body		submitLeaseRequest	true	"Lease submit request"
//	@Success		200		{object}	swaggerEnvelope{lease=lease.Contract}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/leases/{id}/submit [post]
func (h *LeaseHandler) Submit(c *gin.Context) {
	leaseID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease id"})
		return
	}

	var request submitLeaseRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease submit request"})
		return
	}

	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}

	contract, err := h.service.SubmitForApproval(c.Request.Context(), lease.SubmitInput{
		LeaseID:        leaseID,
		ActorUserID:    sessionUser.ID,
		DepartmentID:   sessionUser.DepartmentID,
		IdempotencyKey: request.IdempotencyKey,
		Comment:        request.Comment,
	})
	if err != nil {
		h.renderLeaseError(c, err)
		return
	}

	c.JSON(http.StatusOK, gin.H{"lease": contract})
}

// Terminate godoc
//
//	@Summary		Terminate lease
//	@Description	Terminates an active lease contract on the supplied termination date.
//	@Tags			Lease
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Lease ID"
//	@Param			request	body		terminateLeaseRequest	true	"Lease termination request"
//	@Success		200		{object}	swaggerEnvelope{lease=lease.Contract}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/leases/{id}/terminate [post]
func (h *LeaseHandler) Terminate(c *gin.Context) {
	leaseID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease id"})
		return
	}

	var request terminateLeaseRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease terminate request"})
		return
	}

	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}

	terminatedAt, err := time.Parse(lease.DateLayout, request.TerminatedAt)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease terminated_at"})
		return
	}

	contract, err := h.service.Terminate(c.Request.Context(), lease.TerminateInput{LeaseID: leaseID, ActorUserID: sessionUser.ID, TerminatedAt: terminatedAt})
	if err != nil {
		h.renderLeaseError(c, err)
		return
	}

	c.JSON(http.StatusOK, gin.H{"lease": contract})
}

func (h *LeaseHandler) bindCreateDraftInput(c *gin.Context, request createLeaseRequest, actorUserID int64) (lease.CreateDraftInput, bool) {
	startDate, err := time.Parse(lease.DateLayout, request.StartDate)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease start date"})
		return lease.CreateDraftInput{}, false
	}
	endDate, err := time.Parse(lease.DateLayout, request.EndDate)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease end date"})
		return lease.CreateDraftInput{}, false
	}

	units := make([]lease.UnitInput, 0, len(request.Units))
	for _, unit := range request.Units {
		units = append(units, lease.UnitInput{UnitID: unit.UnitID, RentArea: unit.RentArea})
	}

	terms := make([]lease.TermInput, 0, len(request.Terms))
	for _, term := range request.Terms {
		effectiveFrom, err := time.Parse(lease.DateLayout, term.EffectiveFrom)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease term effective_from"})
			return lease.CreateDraftInput{}, false
		}
		effectiveTo, err := time.Parse(lease.DateLayout, term.EffectiveTo)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid lease term effective_to"})
			return lease.CreateDraftInput{}, false
		}
		terms = append(terms, lease.TermInput{TermType: term.TermType, BillingCycle: term.BillingCycle, CurrencyTypeID: term.CurrencyTypeID, Amount: term.Amount, EffectiveFrom: effectiveFrom, EffectiveTo: effectiveTo})
	}

	return lease.CreateDraftInput{
		LeaseNo:          request.LeaseNo,
		DepartmentID:     request.DepartmentID,
		StoreID:          request.StoreID,
		BuildingID:       request.BuildingID,
		CustomerID:       request.CustomerID,
		BrandID:          request.BrandID,
		TradeID:          request.TradeID,
		ManagementTypeID: request.ManagementTypeID,
		TenantName:       request.TenantName,
		StartDate:        startDate,
		EndDate:          endDate,
		Units:            units,
		Terms:            terms,
		ActorUserID:      actorUserID,
	}, true
}

func (h *LeaseHandler) renderLeaseError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, lease.ErrLeaseNotFound):
		status = http.StatusNotFound
	case errors.Is(err, lease.ErrDuplicateLeaseNo):
		status = http.StatusConflict
	case errors.Is(err, lease.ErrInvalidLeaseState), errors.Is(err, lease.ErrLeaseAlreadySubmitted):
		status = http.StatusConflict
	case errors.Is(err, lease.ErrLeaseIncompleteForSubmission), errors.Is(err, workflow.ErrDefinitionNotFound), errors.Is(err, workflow.ErrInvalidState):
		status = http.StatusBadRequest
	}
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
