package handlers

import (
	"errors"
	"net/http"

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

func (h *MasterDataHandler) ListCustomers(c *gin.Context) {
	items, err := h.service.ListCustomers(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load customers"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"customers": items})
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

func (h *MasterDataHandler) ListBrands(c *gin.Context) {
	items, err := h.service.ListBrands(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load brands"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"brands": items})
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

func (h *MasterDataHandler) renderMasterDataError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	if errors.Is(err, masterdata.ErrDuplicateCode) {
		status = http.StatusConflict
	}
	if errors.Is(err, masterdata.ErrInvalidMasterData) {
		status = http.StatusBadRequest
	}
	c.JSON(status, gin.H{"message": err.Error()})
}
