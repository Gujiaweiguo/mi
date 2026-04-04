package handlers

import (
	"context"
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/baseinfo"
	"github.com/gin-gonic/gin"
)

type BaseInfoHandler struct{ service *baseinfo.Service }

func NewBaseInfoHandler(service *baseinfo.Service) *BaseInfoHandler {
	return &BaseInfoHandler{service: service}
}

type baseInfoRequest struct {
	Code     string  `json:"code" binding:"required"`
	Name     string  `json:"name" binding:"required"`
	Status   string  `json:"status"`
	ColorHex *string `json:"color_hex"`
	IsLocal  *bool   `json:"is_local"`
	ParentID *int64  `json:"parent_id"`
	Level    *int    `json:"level"`
}

func (h *BaseInfoHandler) ListStoreTypes(c *gin.Context) {
	items, err := h.service.ListStoreTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load store types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"store_types": items})
}

func (h *BaseInfoHandler) CreateStoreType(c *gin.Context) {
	h.create(c, h.service.CreateStoreType, "store_type")
}

func (h *BaseInfoHandler) UpdateStoreType(c *gin.Context) {
	h.update(c, h.service.UpdateStoreType, "store_type")
}

func (h *BaseInfoHandler) ListStoreManagementTypes(c *gin.Context) {
	items, err := h.service.ListStoreManagementTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load store management types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"store_management_types": items})
}

func (h *BaseInfoHandler) CreateStoreManagementType(c *gin.Context) {
	h.create(c, h.service.CreateStoreManagementType, "store_management_type")
}

func (h *BaseInfoHandler) UpdateStoreManagementType(c *gin.Context) {
	h.update(c, h.service.UpdateStoreManagementType, "store_management_type")
}

func (h *BaseInfoHandler) ListAreaLevels(c *gin.Context) {
	items, err := h.service.ListAreaLevels(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load area levels"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"area_levels": items})
}

func (h *BaseInfoHandler) CreateAreaLevel(c *gin.Context) {
	h.create(c, h.service.CreateAreaLevel, "area_level")
}

func (h *BaseInfoHandler) UpdateAreaLevel(c *gin.Context) {
	h.update(c, h.service.UpdateAreaLevel, "area_level")
}

func (h *BaseInfoHandler) ListUnitTypes(c *gin.Context) {
	items, err := h.service.ListUnitTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load unit types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit_types": items})
}

func (h *BaseInfoHandler) CreateUnitType(c *gin.Context) {
	h.create(c, h.service.CreateUnitType, "unit_type")
}

func (h *BaseInfoHandler) UpdateUnitType(c *gin.Context) {
	h.update(c, h.service.UpdateUnitType, "unit_type")
}

func (h *BaseInfoHandler) ListShopTypes(c *gin.Context) {
	items, err := h.service.ListShopTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load shop types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"shop_types": items})
}

func (h *BaseInfoHandler) CreateShopType(c *gin.Context) {
	h.create(c, h.service.CreateShopType, "shop_type")
}

func (h *BaseInfoHandler) UpdateShopType(c *gin.Context) {
	h.update(c, h.service.UpdateShopType, "shop_type")
}

func (h *BaseInfoHandler) ListCurrencyTypes(c *gin.Context) {
	items, err := h.service.ListCurrencyTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load currency types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"currency_types": items})
}

func (h *BaseInfoHandler) CreateCurrencyType(c *gin.Context) {
	h.create(c, h.service.CreateCurrencyType, "currency_type")
}

func (h *BaseInfoHandler) UpdateCurrencyType(c *gin.Context) {
	h.update(c, h.service.UpdateCurrencyType, "currency_type")
}

func (h *BaseInfoHandler) ListTradeDefinitions(c *gin.Context) {
	items, err := h.service.ListTradeDefinitions(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load trade definitions"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"trade_definitions": items})
}

func (h *BaseInfoHandler) CreateTradeDefinition(c *gin.Context) {
	h.create(c, h.service.CreateTradeDefinition, "trade_definition")
}

func (h *BaseInfoHandler) UpdateTradeDefinition(c *gin.Context) {
	h.update(c, h.service.UpdateTradeDefinition, "trade_definition")
}

func (h *BaseInfoHandler) create(c *gin.Context, fn func(context.Context, baseinfo.CatalogInput) (*baseinfo.ReferenceCatalogItem, error), key string) {
	var request baseInfoRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid base info request"})
		return
	}
	item, err := fn(c.Request.Context(), request.toInput())
	if err != nil {
		h.renderBaseInfoError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{key: item})
}

func (h *BaseInfoHandler) update(c *gin.Context, fn func(context.Context, int64, baseinfo.CatalogInput) (*baseinfo.ReferenceCatalogItem, error), key string) {
	id, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid base info id"})
		return
	}
	var request baseInfoRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid base info request"})
		return
	}
	item, err := fn(c.Request.Context(), id, request.toInput())
	if err != nil {
		h.renderBaseInfoError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{key: item})
}

func (r baseInfoRequest) toInput() baseinfo.CatalogInput {
	return baseinfo.CatalogInput{
		Code:     r.Code,
		Name:     r.Name,
		Status:   r.Status,
		ColorHex: r.ColorHex,
		IsLocal:  r.IsLocal,
		ParentID: r.ParentID,
		Level:    r.Level,
	}
}

func (h *BaseInfoHandler) renderBaseInfoError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, baseinfo.ErrInvalidBaseInfo):
		status = http.StatusBadRequest
	case errors.Is(err, baseinfo.ErrReferenceNotFound):
		status = http.StatusNotFound
	case errors.Is(err, baseinfo.ErrDuplicateCode):
		status = http.StatusConflict
	}
	c.JSON(status, gin.H{"message": err.Error()})
}
