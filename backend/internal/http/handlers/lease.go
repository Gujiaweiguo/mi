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
	LeaseNo          string                            `json:"lease_no" binding:"required,min=1,max=50"`
	Subtype          lease.ContractSubtype             `json:"subtype" binding:"omitempty,oneof=standard joint_operation ad_board area_ground"`
	DepartmentID     int64                             `json:"department_id" binding:"required,gt=0"`
	StoreID          int64                             `json:"store_id" binding:"required,gt=0"`
	BuildingID       *int64                            `json:"building_id" binding:"omitempty,gt=0"`
	CustomerID       *int64                            `json:"customer_id" binding:"omitempty,gt=0"`
	BrandID          *int64                            `json:"brand_id" binding:"omitempty,gt=0"`
	TradeID          *int64                            `json:"trade_id" binding:"omitempty,gt=0"`
	ManagementTypeID *int64                            `json:"management_type_id" binding:"omitempty,gt=0"`
	TenantName       string                            `json:"tenant_name" binding:"required,min=1,max=100"`
	StartDate        string                            `json:"start_date" binding:"required"`
	EndDate          string                            `json:"end_date" binding:"required"`
	JointOperation   *createLeaseJointOperationRequest `json:"joint_operation,omitempty"`
	AdBoards         []createLeaseAdBoardRequest       `json:"ad_boards,omitempty"`
	AreaGrounds      []createLeaseAreaGroundRequest    `json:"area_grounds,omitempty"`
	Units            []createLeaseUnitRequest          `json:"units" binding:"required,min=1,dive"`
	Terms            []createLeaseTermRequest          `json:"terms" binding:"required,min=1,dive"`
}

type createLeaseJointOperationRequest struct {
	BillCycle                int     `json:"bill_cycle"`
	RentInc                  string  `json:"rent_inc"`
	AccountCycle             int     `json:"account_cycle"`
	TaxRate                  float64 `json:"tax_rate"`
	TaxType                  int     `json:"tax_type"`
	SettlementCurrencyTypeID int64   `json:"settlement_currency_type_id"`
	InTaxRate                float64 `json:"in_tax_rate"`
	OutTaxRate               float64 `json:"out_tax_rate"`
	MonthSettleDays          float64 `json:"month_settle_days,omitempty"`
	LatePayInterestRate      float64 `json:"late_pay_interest_rate,omitempty"`
	InterestGraceDays        int     `json:"interest_grace_days,omitempty"`
}

type createLeaseAdBoardRequest struct {
	AdBoardID     int64                  `json:"ad_board_id"`
	Description   string                 `json:"description,omitempty"`
	Status        int                    `json:"status,omitempty"`
	StartDate     string                 `json:"start_date"`
	EndDate       string                 `json:"end_date"`
	RentArea      float64                `json:"rent_area"`
	Airtime       int                    `json:"airtime"`
	Frequency     lease.AdBoardFrequency `json:"frequency"`
	FrequencyDays int                    `json:"frequency_days,omitempty"`
	FrequencyMon  bool                   `json:"frequency_mon,omitempty"`
	FrequencyTue  bool                   `json:"frequency_tue,omitempty"`
	FrequencyWed  bool                   `json:"frequency_wed,omitempty"`
	FrequencyThu  bool                   `json:"frequency_thu,omitempty"`
	FrequencyFri  bool                   `json:"frequency_fri,omitempty"`
	FrequencySat  bool                   `json:"frequency_sat,omitempty"`
	FrequencySun  bool                   `json:"frequency_sun,omitempty"`
	BetweenFrom   int                    `json:"between_from,omitempty"`
	BetweenTo     int                    `json:"between_to,omitempty"`
	StoreID       *int64                 `json:"store_id,omitempty" binding:"omitempty,gt=0"`
	BuildingID    *int64                 `json:"building_id,omitempty" binding:"omitempty,gt=0"`
}

type createLeaseAreaGroundRequest struct {
	Code        string  `json:"code"`
	Name        string  `json:"name"`
	TypeID      int64   `json:"type_id"`
	Description string  `json:"description,omitempty"`
	Status      int     `json:"status,omitempty"`
	StartDate   string  `json:"start_date"`
	EndDate     string  `json:"end_date"`
	RentArea    float64 `json:"rent_area"`
}

type createLeaseUnitRequest struct {
	UnitID   int64   `json:"unit_id" binding:"required,gt=0"`
	RentArea float64 `json:"rent_area" binding:"required,gt=0"`
}

type createLeaseTermRequest struct {
	TermType       lease.TermType     `json:"term_type" binding:"required,oneof=rent deposit utility other"`
	BillingCycle   lease.BillingCycle `json:"billing_cycle" binding:"required,oneof=monthly quarterly yearly"`
	CurrencyTypeID int64              `json:"currency_type_id" binding:"required,gt=0"`
	Amount         float64            `json:"amount" binding:"required,gt=0"`
	EffectiveFrom  string             `json:"effective_from" binding:"required"`
	EffectiveTo    string             `json:"effective_to" binding:"required"`
}

type submitLeaseRequest struct {
	Comment        string `json:"comment" binding:"omitempty,max=500"`
	IdempotencyKey string `json:"idempotency_key" binding:"required,min=1,max=100"`
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

	var jointOperation *lease.JointOperationFieldsInput
	if request.JointOperation != nil {
		jointOperation = &lease.JointOperationFieldsInput{
			BillCycle:                request.JointOperation.BillCycle,
			RentInc:                  request.JointOperation.RentInc,
			AccountCycle:             request.JointOperation.AccountCycle,
			TaxRate:                  request.JointOperation.TaxRate,
			TaxType:                  request.JointOperation.TaxType,
			SettlementCurrencyTypeID: request.JointOperation.SettlementCurrencyTypeID,
			InTaxRate:                request.JointOperation.InTaxRate,
			OutTaxRate:               request.JointOperation.OutTaxRate,
			MonthSettleDays:          request.JointOperation.MonthSettleDays,
			LatePayInterestRate:      request.JointOperation.LatePayInterestRate,
			InterestGraceDays:        request.JointOperation.InterestGraceDays,
		}
	}

	adBoards := make([]lease.AdBoardDetailInput, 0, len(request.AdBoards))
	for _, detail := range request.AdBoards {
		adBoardStartDate, err := time.Parse(lease.DateLayout, detail.StartDate)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid ad board start_date"})
			return lease.CreateDraftInput{}, false
		}
		adBoardEndDate, err := time.Parse(lease.DateLayout, detail.EndDate)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid ad board end_date"})
			return lease.CreateDraftInput{}, false
		}
		adBoards = append(adBoards, lease.AdBoardDetailInput{
			AdBoardID:     detail.AdBoardID,
			Description:   detail.Description,
			Status:        detail.Status,
			StartDate:     adBoardStartDate,
			EndDate:       adBoardEndDate,
			RentArea:      detail.RentArea,
			Airtime:       detail.Airtime,
			Frequency:     detail.Frequency,
			FrequencyDays: detail.FrequencyDays,
			FrequencyMon:  detail.FrequencyMon,
			FrequencyTue:  detail.FrequencyTue,
			FrequencyWed:  detail.FrequencyWed,
			FrequencyThu:  detail.FrequencyThu,
			FrequencyFri:  detail.FrequencyFri,
			FrequencySat:  detail.FrequencySat,
			FrequencySun:  detail.FrequencySun,
			BetweenFrom:   detail.BetweenFrom,
			BetweenTo:     detail.BetweenTo,
			StoreID:       detail.StoreID,
			BuildingID:    detail.BuildingID,
		})
	}

	areaGrounds := make([]lease.AreaGroundDetailInput, 0, len(request.AreaGrounds))
	for _, detail := range request.AreaGrounds {
		areaGroundStartDate, err := time.Parse(lease.DateLayout, detail.StartDate)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid area ground start_date"})
			return lease.CreateDraftInput{}, false
		}
		areaGroundEndDate, err := time.Parse(lease.DateLayout, detail.EndDate)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid area ground end_date"})
			return lease.CreateDraftInput{}, false
		}
		areaGrounds = append(areaGrounds, lease.AreaGroundDetailInput{
			Code:        detail.Code,
			Name:        detail.Name,
			TypeID:      detail.TypeID,
			Description: detail.Description,
			Status:      detail.Status,
			StartDate:   areaGroundStartDate,
			EndDate:     areaGroundEndDate,
			RentArea:    detail.RentArea,
		})
	}

	return lease.CreateDraftInput{
		LeaseNo:          request.LeaseNo,
		Subtype:          request.Subtype,
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
		JointOperation:   jointOperation,
		AdBoards:         adBoards,
		AreaGrounds:      areaGrounds,
		Units:            units,
		Terms:            terms,
		ActorUserID:      actorUserID,
	}, true
}

func (h *LeaseHandler) renderLeaseError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	response := gin.H{"message": errutil.SafeMessage(err)}
	var validationErr *lease.ValidationError
	switch {
	case errors.As(err, &validationErr):
		status = http.StatusBadRequest
	case errors.Is(err, lease.ErrLeaseNotFound):
		status = http.StatusNotFound
	case errors.Is(err, lease.ErrDuplicateLeaseNo):
		status = http.StatusConflict
	case errors.Is(err, lease.ErrInvalidLeaseState), errors.Is(err, lease.ErrLeaseAlreadySubmitted):
		status = http.StatusConflict
	case errors.Is(err, lease.ErrLeaseIncompleteForSubmission), errors.Is(err, workflow.ErrDefinitionNotFound), errors.Is(err, workflow.ErrInvalidState):
		status = http.StatusBadRequest
	}
	if errors.As(err, &validationErr) {
		response["message"] = validationErr.Error()
		response["fields"] = validationErr.Fields
	}
	c.JSON(status, response)
}
