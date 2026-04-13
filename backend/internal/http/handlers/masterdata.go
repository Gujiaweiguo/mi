package handlers

import (
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/masterdata"
	"github.com/gin-gonic/gin"
)

type MasterDataHandler struct{ service *masterdata.Service }

func NewMasterDataHandler(service *masterdata.Service) *MasterDataHandler {
	return &MasterDataHandler{service: service}
}

type createCustomerRequest struct {
	Code         string `json:"code" binding:"required"`
	Name         string `json:"name" binding:"required"`
	TradeID      *int64 `json:"trade_id"`
	DepartmentID *int64 `json:"department_id"`
	Status       string `json:"status"`
}

type createBrandRequest struct {
	Code   string `json:"code" binding:"required"`
	Name   string `json:"name" binding:"required"`
	Status string `json:"status"`
}

type updateCustomerRequest = createCustomerRequest
type updateBrandRequest = createBrandRequest

type upsertUnitRentBudgetRequest struct {
	UnitID      int64   `json:"unit_id" binding:"required"`
	FiscalYear  int     `json:"fiscal_year" binding:"required"`
	BudgetPrice float64 `json:"budget_price" binding:"required"`
}

type upsertStoreRentBudgetRequest struct {
	StoreID       int64   `json:"store_id" binding:"required"`
	FiscalYear    int     `json:"fiscal_year" binding:"required"`
	FiscalMonth   int     `json:"fiscal_month" binding:"required"`
	MonthlyBudget float64 `json:"monthly_budget" binding:"required"`
}

type upsertUnitProspectRequest struct {
	UnitID              int64    `json:"unit_id" binding:"required"`
	FiscalYear          int      `json:"fiscal_year" binding:"required"`
	PotentialCustomerID *int64   `json:"potential_customer_id"`
	ProspectBrandID     *int64   `json:"prospect_brand_id"`
	ProspectTradeID     *int64   `json:"prospect_trade_id"`
	AvgTransaction      *float64 `json:"avg_transaction"`
	ProspectRentPrice   *float64 `json:"prospect_rent_price"`
	RentIncrement       *string  `json:"rent_increment"`
	ProspectTermMonths  *int     `json:"prospect_term_months"`
}

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

func (h *MasterDataHandler) ListUnitRentBudgets(c *gin.Context) {
	items, err := h.service.ListUnitRentBudgets(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load unit rent budgets"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit_rent_budgets": items})
}

func (h *MasterDataHandler) CreateUnitRentBudget(c *gin.Context) {
	h.upsertUnitRentBudget(c, http.StatusCreated)
}

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

func (h *MasterDataHandler) ListStoreRentBudgets(c *gin.Context) {
	items, err := h.service.ListStoreRentBudgets(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load store rent budgets"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"store_rent_budgets": items})
}

func (h *MasterDataHandler) CreateStoreRentBudget(c *gin.Context) {
	h.upsertStoreRentBudget(c, http.StatusCreated)
}

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

func (h *MasterDataHandler) ListUnitProspects(c *gin.Context) {
	items, err := h.service.ListUnitProspects(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load unit prospects"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit_prospects": items})
}

func (h *MasterDataHandler) CreateUnitProspect(c *gin.Context) {
	h.upsertUnitProspect(c, http.StatusCreated)
}

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
	c.JSON(status, gin.H{"message": err.Error()})
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
