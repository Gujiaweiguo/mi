package handlers

import (
	"context"
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/baseinfo"
	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
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

// ListStoreTypes godoc
//
//	@Summary		List store types
//	@Description	Returns all configured store types.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{store_types=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/store-types [get]
func (h *BaseInfoHandler) ListStoreTypes(c *gin.Context) {
	items, err := h.service.ListStoreTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load store types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"store_types": items})
}

// CreateStoreType godoc
//
//	@Summary		Create store type
//	@Description	Creates a new store type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Store type request"
//	@Success		201		{object}	swaggerEnvelope{store_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/store-types [post]
func (h *BaseInfoHandler) CreateStoreType(c *gin.Context) {
	h.create(c, h.service.CreateStoreType, "store_type")
}

// UpdateStoreType godoc
//
//	@Summary		Update store type
//	@Description	Updates an existing store type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Store type ID"
//	@Param			request	body		baseInfoRequest	true	"Store type request"
//	@Success		200		{object}	swaggerEnvelope{store_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/store-types/{id} [put]
func (h *BaseInfoHandler) UpdateStoreType(c *gin.Context) {
	h.update(c, h.service.UpdateStoreType, "store_type")
}

// ListStoreManagementTypes godoc
//
//	@Summary		List store management types
//	@Description	Returns all configured store management types.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{store_management_types=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/store-management-types [get]
func (h *BaseInfoHandler) ListStoreManagementTypes(c *gin.Context) {
	items, err := h.service.ListStoreManagementTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load store management types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"store_management_types": items})
}

// CreateStoreManagementType godoc
//
//	@Summary		Create store management type
//	@Description	Creates a new store management type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Store management type request"
//	@Success		201		{object}	swaggerEnvelope{store_management_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/store-management-types [post]
func (h *BaseInfoHandler) CreateStoreManagementType(c *gin.Context) {
	h.create(c, h.service.CreateStoreManagementType, "store_management_type")
}

// UpdateStoreManagementType godoc
//
//	@Summary		Update store management type
//	@Description	Updates an existing store management type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Store management type ID"
//	@Param			request	body		baseInfoRequest	true	"Store management type request"
//	@Success		200		{object}	swaggerEnvelope{store_management_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/store-management-types/{id} [put]
func (h *BaseInfoHandler) UpdateStoreManagementType(c *gin.Context) {
	h.update(c, h.service.UpdateStoreManagementType, "store_management_type")
}

// ListAreaLevels godoc
//
//	@Summary		List area levels
//	@Description	Returns all configured area levels.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{area_levels=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/area-levels [get]
func (h *BaseInfoHandler) ListAreaLevels(c *gin.Context) {
	items, err := h.service.ListAreaLevels(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load area levels"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"area_levels": items})
}

// CreateAreaLevel godoc
//
//	@Summary		Create area level
//	@Description	Creates a new area level reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Area level request"
//	@Success		201		{object}	swaggerEnvelope{area_level=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/area-levels [post]
func (h *BaseInfoHandler) CreateAreaLevel(c *gin.Context) {
	h.create(c, h.service.CreateAreaLevel, "area_level")
}

// UpdateAreaLevel godoc
//
//	@Summary		Update area level
//	@Description	Updates an existing area level reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Area level ID"
//	@Param			request	body		baseInfoRequest	true	"Area level request"
//	@Success		200		{object}	swaggerEnvelope{area_level=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/area-levels/{id} [put]
func (h *BaseInfoHandler) UpdateAreaLevel(c *gin.Context) {
	h.update(c, h.service.UpdateAreaLevel, "area_level")
}

// ListUnitTypes godoc
//
//	@Summary		List unit types
//	@Description	Returns all configured unit types.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{unit_types=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/unit-types [get]
func (h *BaseInfoHandler) ListUnitTypes(c *gin.Context) {
	items, err := h.service.ListUnitTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load unit types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit_types": items})
}

// CreateUnitType godoc
//
//	@Summary		Create unit type
//	@Description	Creates a new unit type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Unit type request"
//	@Success		201		{object}	swaggerEnvelope{unit_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/unit-types [post]
func (h *BaseInfoHandler) CreateUnitType(c *gin.Context) {
	h.create(c, h.service.CreateUnitType, "unit_type")
}

// UpdateUnitType godoc
//
//	@Summary		Update unit type
//	@Description	Updates an existing unit type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Unit type ID"
//	@Param			request	body		baseInfoRequest	true	"Unit type request"
//	@Success		200		{object}	swaggerEnvelope{unit_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/unit-types/{id} [put]
func (h *BaseInfoHandler) UpdateUnitType(c *gin.Context) {
	h.update(c, h.service.UpdateUnitType, "unit_type")
}

// ListShopTypes godoc
//
//	@Summary		List shop types
//	@Description	Returns all configured shop types.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{shop_types=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/shop-types [get]
func (h *BaseInfoHandler) ListShopTypes(c *gin.Context) {
	items, err := h.service.ListShopTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load shop types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"shop_types": items})
}

// CreateShopType godoc
//
//	@Summary		Create shop type
//	@Description	Creates a new shop type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Shop type request"
//	@Success		201		{object}	swaggerEnvelope{shop_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/shop-types [post]
func (h *BaseInfoHandler) CreateShopType(c *gin.Context) {
	h.create(c, h.service.CreateShopType, "shop_type")
}

// UpdateShopType godoc
//
//	@Summary		Update shop type
//	@Description	Updates an existing shop type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Shop type ID"
//	@Param			request	body		baseInfoRequest	true	"Shop type request"
//	@Success		200		{object}	swaggerEnvelope{shop_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/shop-types/{id} [put]
func (h *BaseInfoHandler) UpdateShopType(c *gin.Context) {
	h.update(c, h.service.UpdateShopType, "shop_type")
}

// ListCurrencyTypes godoc
//
//	@Summary		List currency types
//	@Description	Returns all configured currency types.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{currency_types=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/currency-types [get]
func (h *BaseInfoHandler) ListCurrencyTypes(c *gin.Context) {
	items, err := h.service.ListCurrencyTypes(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load currency types"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"currency_types": items})
}

// CreateCurrencyType godoc
//
//	@Summary		Create currency type
//	@Description	Creates a new currency type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Currency type request"
//	@Success		201		{object}	swaggerEnvelope{currency_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/currency-types [post]
func (h *BaseInfoHandler) CreateCurrencyType(c *gin.Context) {
	h.create(c, h.service.CreateCurrencyType, "currency_type")
}

// UpdateCurrencyType godoc
//
//	@Summary		Update currency type
//	@Description	Updates an existing currency type reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Currency type ID"
//	@Param			request	body		baseInfoRequest	true	"Currency type request"
//	@Success		200		{object}	swaggerEnvelope{currency_type=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/currency-types/{id} [put]
func (h *BaseInfoHandler) UpdateCurrencyType(c *gin.Context) {
	h.update(c, h.service.UpdateCurrencyType, "currency_type")
}

// ListTradeDefinitions godoc
//
//	@Summary		List trade definitions
//	@Description	Returns all configured trade definitions.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{trade_definitions=[]baseinfo.ReferenceCatalogItem}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/trade-definitions [get]
func (h *BaseInfoHandler) ListTradeDefinitions(c *gin.Context) {
	items, err := h.service.ListTradeDefinitions(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load trade definitions"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"trade_definitions": items})
}

// CreateTradeDefinition godoc
//
//	@Summary		Create trade definition
//	@Description	Creates a new trade definition reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			request	body		baseInfoRequest	true	"Trade definition request"
//	@Success		201		{object}	swaggerEnvelope{trade_definition=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/trade-definitions [post]
func (h *BaseInfoHandler) CreateTradeDefinition(c *gin.Context) {
	h.create(c, h.service.CreateTradeDefinition, "trade_definition")
}

// UpdateTradeDefinition godoc
//
//	@Summary		Update trade definition
//	@Description	Updates an existing trade definition reference record.
//	@Tags			BaseInfo
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int				true	"Trade definition ID"
//	@Param			request	body		baseInfoRequest	true	"Trade definition request"
//	@Success		200		{object}	swaggerEnvelope{trade_definition=baseinfo.ReferenceCatalogItem}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/base-info/trade-definitions/{id} [put]
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
	c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
}
