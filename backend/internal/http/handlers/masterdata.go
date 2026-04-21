package handlers

import (
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/masterdata"
	"github.com/gin-gonic/gin"
)

type MasterDataHandler struct{ service *masterdata.Service }

func NewMasterDataHandler(service *masterdata.Service) *MasterDataHandler {
	return &MasterDataHandler{service: service}
}

type createCustomerRequest struct {
	Code         string `json:"code" binding:"required,min=1,max=50"`
	Name         string `json:"name" binding:"required,min=1,max=100"`
	TradeID      *int64 `json:"trade_id" binding:"omitempty,gt=0"`
	DepartmentID *int64 `json:"department_id" binding:"omitempty,gt=0"`
	Status       string `json:"status" binding:"omitempty,oneof=active inactive disabled"`
}

type createBrandRequest struct {
	Code   string `json:"code" binding:"required,min=1,max=50"`
	Name   string `json:"name" binding:"required,min=1,max=100"`
	Status string `json:"status" binding:"omitempty,oneof=active inactive disabled"`
}

type updateCustomerRequest = createCustomerRequest
type updateBrandRequest = createBrandRequest

type upsertUnitRentBudgetRequest struct {
	UnitID      int64   `json:"unit_id" binding:"required,gt=0"`
	FiscalYear  int     `json:"fiscal_year" binding:"required,min=2000,max=2100"`
	BudgetPrice float64 `json:"budget_price" binding:"required,gt=0"`
}

type upsertStoreRentBudgetRequest struct {
	StoreID       int64   `json:"store_id" binding:"required,gt=0"`
	FiscalYear    int     `json:"fiscal_year" binding:"required,min=2000,max=2100"`
	FiscalMonth   int     `json:"fiscal_month" binding:"required,min=1,max=12"`
	MonthlyBudget float64 `json:"monthly_budget" binding:"required,gt=0"`
}

type upsertUnitProspectRequest struct {
	UnitID              int64    `json:"unit_id" binding:"required,gt=0"`
	FiscalYear          int      `json:"fiscal_year" binding:"required,min=2000,max=2100"`
	PotentialCustomerID *int64   `json:"potential_customer_id" binding:"omitempty,gt=0"`
	ProspectBrandID     *int64   `json:"prospect_brand_id" binding:"omitempty,gt=0"`
	ProspectTradeID     *int64   `json:"prospect_trade_id" binding:"omitempty,gt=0"`
	AvgTransaction      *float64 `json:"avg_transaction"`
	ProspectRentPrice   *float64 `json:"prospect_rent_price"`
	RentIncrement       *string  `json:"rent_increment"`
	ProspectTermMonths  *int     `json:"prospect_term_months"`
}

// ListCustomers godoc
//
//	@Summary		List customers
//	@Description	Returns paginated customer master data filtered by a search query.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			query		query		string	false	"Search query"
//	@Param			page		query		int		false	"Page number"
//	@Param			page_size	query		int		false	"Page size"
//	@Success		200			{object}	swaggerEnvelope{customers=[]masterdata.Customer,total=int,page=int,page_size=int}
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/customers [get]
func (h *MasterDataHandler) ListCustomers(c *gin.Context) {
	result, err := h.service.ListCustomers(c.Request.Context(), masterdata.ListFilter{
		Query:    c.Query("query"),
		Page:     parseIntQuery(c, "page", 1),
		PageSize: parseIntQuery(c, "page_size", 10),
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load customers"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"customers": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

// CreateCustomer godoc
//
//	@Summary		Create customer
//	@Description	Creates a new customer master data record.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createCustomerRequest	true	"Customer request"
//	@Success		201		{object}	swaggerEnvelope{customer=masterdata.Customer}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/customers [post]
func (h *MasterDataHandler) CreateCustomer(c *gin.Context) {
	var request createCustomerRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid customer request"})
		return
	}
	item, err := h.service.CreateCustomer(c.Request.Context(), masterdata.CreateCustomerInput{Code: request.Code, Name: request.Name, TradeID: request.TradeID, DepartmentID: request.DepartmentID, Status: request.Status})
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"customer": item})
}

// UpdateCustomer godoc
//
//	@Summary		Update customer
//	@Description	Updates an existing customer master data record.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Customer ID"
//	@Param			request	body		updateCustomerRequest	true	"Customer request"
//	@Success		200		{object}	swaggerEnvelope{customer=masterdata.Customer}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/customers/{id} [put]
func (h *MasterDataHandler) UpdateCustomer(c *gin.Context) {
	id, ok := parseInt64Param(c, "id")
	if !ok {
		return
	}
	var request updateCustomerRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid customer request"})
		return
	}
	item, err := h.service.UpdateCustomer(c.Request.Context(), masterdata.UpdateCustomerInput{ID: id, Code: request.Code, Name: request.Name, TradeID: request.TradeID, DepartmentID: request.DepartmentID, Status: request.Status})
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"customer": item})
}

// ListBrands godoc
//
//	@Summary		List brands
//	@Description	Returns paginated brand master data filtered by a search query.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			query		query		string	false	"Search query"
//	@Param			page		query		int		false	"Page number"
//	@Param			page_size	query		int		false	"Page size"
//	@Success		200			{object}	swaggerEnvelope{brands=[]masterdata.Brand,total=int,page=int,page_size=int}
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/brands [get]
func (h *MasterDataHandler) ListBrands(c *gin.Context) {
	result, err := h.service.ListBrands(c.Request.Context(), masterdata.ListFilter{
		Query:    c.Query("query"),
		Page:     parseIntQuery(c, "page", 1),
		PageSize: parseIntQuery(c, "page_size", 10),
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load brands"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"brands": result.Items, "total": result.Total, "page": result.Page, "page_size": result.PageSize})
}

// CreateBrand godoc
//
//	@Summary		Create brand
//	@Description	Creates a new brand master data record.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createBrandRequest	true	"Brand request"
//	@Success		201		{object}	swaggerEnvelope{brand=masterdata.Brand}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/brands [post]
func (h *MasterDataHandler) CreateBrand(c *gin.Context) {
	var request createBrandRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid brand request"})
		return
	}
	item, err := h.service.CreateBrand(c.Request.Context(), masterdata.CreateBrandInput{Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"brand": item})
}

// UpdateBrand godoc
//
//	@Summary		Update brand
//	@Description	Updates an existing brand master data record.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int					true	"Brand ID"
//	@Param			request	body		updateBrandRequest	true	"Brand request"
//	@Success		200		{object}	swaggerEnvelope{brand=masterdata.Brand}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/brands/{id} [put]
func (h *MasterDataHandler) UpdateBrand(c *gin.Context) {
	id, ok := parseInt64Param(c, "id")
	if !ok {
		return
	}
	var request updateBrandRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid brand request"})
		return
	}
	item, err := h.service.UpdateBrand(c.Request.Context(), masterdata.UpdateBrandInput{ID: id, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"brand": item})
}

// ListUnitRentBudgets godoc
//
//	@Summary		List unit rent budgets
//	@Description	Returns all configured unit rent budgets.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{unit_rent_budgets=[]masterdata.UnitRentBudget}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/unit-rent-budgets [get]
func (h *MasterDataHandler) ListUnitRentBudgets(c *gin.Context) {
	items, err := h.service.ListUnitRentBudgets(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load unit rent budgets"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit_rent_budgets": items})
}

// CreateUnitRentBudget godoc
//
//	@Summary		Create unit rent budget
//	@Description	Creates or upserts a unit rent budget for a fiscal year.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			request	body		upsertUnitRentBudgetRequest	true	"Unit rent budget request"
//	@Success		201		{object}	swaggerEnvelope{unit_rent_budget=masterdata.UnitRentBudget}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/unit-rent-budgets [post]
func (h *MasterDataHandler) CreateUnitRentBudget(c *gin.Context) {
	h.upsertUnitRentBudget(c, http.StatusCreated)
}

// UpdateUnitRentBudget godoc
//
//	@Summary		Update unit rent budget
//	@Description	Updates or upserts a unit rent budget for a fiscal year.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			unitId		path		int							true	"Unit ID"
//	@Param			fiscalYear	path		int							true	"Fiscal year"
//	@Param			request		body		upsertUnitRentBudgetRequest	true	"Unit rent budget request"
//	@Success		200			{object}	swaggerEnvelope{unit_rent_budget=masterdata.UnitRentBudget}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		404			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/unit-rent-budgets/{unitId}/{fiscalYear} [put]
func (h *MasterDataHandler) UpdateUnitRentBudget(c *gin.Context) {
	h.upsertUnitRentBudget(c, http.StatusOK)
}

func (h *MasterDataHandler) upsertUnitRentBudget(c *gin.Context, successStatus int) {
	var request upsertUnitRentBudgetRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid unit rent budget request"})
		return
	}
	item, err := h.service.UpsertUnitRentBudget(c.Request.Context(), masterdata.UpsertUnitRentBudgetInput(request))
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(successStatus, gin.H{"unit_rent_budget": item})
}

// ListStoreRentBudgets godoc
//
//	@Summary		List store rent budgets
//	@Description	Returns all configured store rent budgets.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{store_rent_budgets=[]masterdata.StoreRentBudget}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/store-rent-budgets [get]
func (h *MasterDataHandler) ListStoreRentBudgets(c *gin.Context) {
	items, err := h.service.ListStoreRentBudgets(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load store rent budgets"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"store_rent_budgets": items})
}

// CreateStoreRentBudget godoc
//
//	@Summary		Create store rent budget
//	@Description	Creates or upserts a store rent budget for a fiscal month.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			request	body		upsertStoreRentBudgetRequest	true	"Store rent budget request"
//	@Success		201		{object}	swaggerEnvelope{store_rent_budget=masterdata.StoreRentBudget}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/store-rent-budgets [post]
func (h *MasterDataHandler) CreateStoreRentBudget(c *gin.Context) {
	h.upsertStoreRentBudget(c, http.StatusCreated)
}

// UpdateStoreRentBudget godoc
//
//	@Summary		Update store rent budget
//	@Description	Updates or upserts a store rent budget for a fiscal month.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			storeId		path		int								true	"Store ID"
//	@Param			fiscalYear	path		int								true	"Fiscal year"
//	@Param			fiscalMonth	path		int								true	"Fiscal month"
//	@Param			request		body		upsertStoreRentBudgetRequest	true	"Store rent budget request"
//	@Success		200			{object}	swaggerEnvelope{store_rent_budget=masterdata.StoreRentBudget}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		404			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/store-rent-budgets/{storeId}/{fiscalYear}/{fiscalMonth} [put]
func (h *MasterDataHandler) UpdateStoreRentBudget(c *gin.Context) {
	h.upsertStoreRentBudget(c, http.StatusOK)
}

func (h *MasterDataHandler) upsertStoreRentBudget(c *gin.Context, successStatus int) {
	var request upsertStoreRentBudgetRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid store rent budget request"})
		return
	}
	item, err := h.service.UpsertStoreRentBudget(c.Request.Context(), masterdata.UpsertStoreRentBudgetInput(request))
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(successStatus, gin.H{"store_rent_budget": item})
}

// ListUnitProspects godoc
//
//	@Summary		List unit prospects
//	@Description	Returns all configured unit prospect planning records.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{unit_prospects=[]masterdata.UnitProspect}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/unit-prospects [get]
func (h *MasterDataHandler) ListUnitProspects(c *gin.Context) {
	items, err := h.service.ListUnitProspects(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load unit prospects"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit_prospects": items})
}

// CreateUnitProspect godoc
//
//	@Summary		Create unit prospect
//	@Description	Creates or upserts a unit prospect planning record.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			request	body		upsertUnitProspectRequest	true	"Unit prospect request"
//	@Success		201		{object}	swaggerEnvelope{unit_prospect=masterdata.UnitProspect}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/unit-prospects [post]
func (h *MasterDataHandler) CreateUnitProspect(c *gin.Context) {
	h.upsertUnitProspect(c, http.StatusCreated)
}

// UpdateUnitProspect godoc
//
//	@Summary		Update unit prospect
//	@Description	Updates or upserts a unit prospect planning record.
//	@Tags			MasterData
//	@Accept			json
//	@Produce		json
//	@Param			unitId		path		int							true	"Unit ID"
//	@Param			fiscalYear	path		int							true	"Fiscal year"
//	@Param			request		body		upsertUnitProspectRequest	true	"Unit prospect request"
//	@Success		200			{object}	swaggerEnvelope{unit_prospect=masterdata.UnitProspect}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		404			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/master-data/unit-prospects/{unitId}/{fiscalYear} [put]
func (h *MasterDataHandler) UpdateUnitProspect(c *gin.Context) {
	h.upsertUnitProspect(c, http.StatusOK)
}

func (h *MasterDataHandler) upsertUnitProspect(c *gin.Context, successStatus int) {
	var request upsertUnitProspectRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid unit prospect request"})
		return
	}
	item, err := h.service.UpsertUnitProspect(c.Request.Context(), masterdata.UpsertUnitProspectInput(request))
	if err != nil {
		h.renderMasterDataError(c, err)
		return
	}
	c.JSON(successStatus, gin.H{"unit_prospect": item})
}

func (h *MasterDataHandler) renderMasterDataError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	if errors.Is(err, masterdata.ErrDuplicateCode) {
		status = http.StatusConflict
	}
	if errors.Is(err, masterdata.ErrInvalidMasterData) {
		status = http.StatusBadRequest
	}
	if errors.Is(err, masterdata.ErrMasterDataNotFound) {
		status = http.StatusNotFound
	}
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}

func parseIntQuery(c *gin.Context, key string, fallback int) int {
	value := c.Query(key)
	if value == "" {
		return fallback
	}
	parsed, err := strconv.Atoi(value)
	if err != nil || parsed <= 0 {
		return fallback
	}
	return parsed
}

func parseInt64Param(c *gin.Context, key string) (int64, bool) {
	value := c.Param(key)
	parsed, err := strconv.ParseInt(value, 10, 64)
	if err != nil || parsed <= 0 {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid path parameter"})
		return 0, false
	}
	return parsed, true
}
