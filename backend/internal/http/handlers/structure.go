package handlers

import (
	"context"
	"errors"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/structure"
	"github.com/gin-gonic/gin"
)

type StructureHandler struct{ service *structure.Service }

func NewStructureHandler(service *structure.Service) *StructureHandler {
	return &StructureHandler{service: service}
}

type structureStoreRequest struct {
	DepartmentID     int64  `json:"department_id" binding:"required"`
	StoreTypeID      int64  `json:"store_type_id" binding:"required"`
	ManagementTypeID int64  `json:"management_type_id" binding:"required"`
	Code             string `json:"code" binding:"required"`
	Name             string `json:"name" binding:"required"`
	ShortName        string `json:"short_name" binding:"required"`
	Status           string `json:"status"`
}

type structureBuildingRequest struct {
	StoreID int64  `json:"store_id" binding:"required"`
	Code    string `json:"code" binding:"required"`
	Name    string `json:"name" binding:"required"`
	Status  string `json:"status"`
}

type structureFloorRequest struct {
	BuildingID        int64   `json:"building_id" binding:"required"`
	Code              string  `json:"code" binding:"required"`
	Name              string  `json:"name" binding:"required"`
	Status            string  `json:"status"`
	FloorPlanImageURL *string `json:"floor_plan_image_url"`
}

type structureAreaRequest struct {
	StoreID     int64  `json:"store_id" binding:"required"`
	AreaLevelID int64  `json:"area_level_id" binding:"required"`
	Code        string `json:"code" binding:"required"`
	Name        string `json:"name" binding:"required"`
	Status      string `json:"status"`
}

type structureLocationRequest struct {
	StoreID int64  `json:"store_id" binding:"required"`
	FloorID int64  `json:"floor_id" binding:"required"`
	Code    string `json:"code" binding:"required"`
	Name    string `json:"name" binding:"required"`
	Status  string `json:"status"`
}

type structureUnitRequest struct {
	BuildingID int64   `json:"building_id" binding:"required"`
	FloorID    int64   `json:"floor_id" binding:"required"`
	LocationID int64   `json:"location_id" binding:"required"`
	AreaID     int64   `json:"area_id" binding:"required"`
	UnitTypeID int64   `json:"unit_type_id" binding:"required"`
	ShopTypeID *int64  `json:"shop_type_id"`
	Code       string  `json:"code" binding:"required"`
	FloorArea  float64 `json:"floor_area" binding:"required"`
	UseArea    float64 `json:"use_area" binding:"required"`
	RentArea   float64 `json:"rent_area" binding:"required"`
	IsRentable bool    `json:"is_rentable"`
	Status     string  `json:"status"`
}

func (h *StructureHandler) ListStores(c *gin.Context) {
	items, err := h.service.ListStores(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load stores"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"stores": items})
}

func (h *StructureHandler) CreateStore(c *gin.Context) {
	h.createStore(c, h.service.CreateStore)
}

func (h *StructureHandler) UpdateStore(c *gin.Context) {
	id, ok := parsePathID(c, "invalid store id")
	if !ok {
		return
	}
	h.updateStore(c, id, h.service.UpdateStore)
}

func (h *StructureHandler) ListBuildings(c *gin.Context) {
	storeID, ok := parseOptionalInt64(c, "store_id")
	if !ok {
		return
	}
	items, err := h.service.ListBuildings(c.Request.Context(), structure.BuildingFilter{StoreID: storeID})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"buildings": items})
}

func (h *StructureHandler) CreateBuilding(c *gin.Context) {
	var request structureBuildingRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid building request"})
		return
	}
	item, err := h.service.CreateBuilding(c.Request.Context(), structure.BuildingInput{StoreID: request.StoreID, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"building": item})
}

func (h *StructureHandler) UpdateBuilding(c *gin.Context) {
	id, ok := parsePathID(c, "invalid building id")
	if !ok {
		return
	}
	var request structureBuildingRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid building request"})
		return
	}
	item, err := h.service.UpdateBuilding(c.Request.Context(), id, structure.BuildingInput{StoreID: request.StoreID, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"building": item})
}

func (h *StructureHandler) ListFloors(c *gin.Context) {
	buildingID, ok := parseOptionalInt64(c, "building_id")
	if !ok {
		return
	}
	items, err := h.service.ListFloors(c.Request.Context(), structure.FloorFilter{BuildingID: buildingID})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"floors": items})
}

func (h *StructureHandler) CreateFloor(c *gin.Context) {
	var request structureFloorRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid floor request"})
		return
	}
	item, err := h.service.CreateFloor(c.Request.Context(), structure.FloorInput{BuildingID: request.BuildingID, Code: request.Code, Name: request.Name, Status: request.Status, FloorPlanImageURL: request.FloorPlanImageURL})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"floor": item})
}

func (h *StructureHandler) UpdateFloor(c *gin.Context) {
	id, ok := parsePathID(c, "invalid floor id")
	if !ok {
		return
	}
	var request structureFloorRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid floor request"})
		return
	}
	item, err := h.service.UpdateFloor(c.Request.Context(), id, structure.FloorInput{BuildingID: request.BuildingID, Code: request.Code, Name: request.Name, Status: request.Status, FloorPlanImageURL: request.FloorPlanImageURL})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"floor": item})
}

func (h *StructureHandler) ListAreas(c *gin.Context) {
	storeID, ok := parseOptionalInt64(c, "store_id")
	if !ok {
		return
	}
	items, err := h.service.ListAreas(c.Request.Context(), structure.AreaFilter{StoreID: storeID})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"areas": items})
}

func (h *StructureHandler) CreateArea(c *gin.Context) {
	var request structureAreaRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid area request"})
		return
	}
	item, err := h.service.CreateArea(c.Request.Context(), structure.AreaInput{StoreID: request.StoreID, AreaLevelID: request.AreaLevelID, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"area": item})
}

func (h *StructureHandler) UpdateArea(c *gin.Context) {
	id, ok := parsePathID(c, "invalid area id")
	if !ok {
		return
	}
	var request structureAreaRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid area request"})
		return
	}
	item, err := h.service.UpdateArea(c.Request.Context(), id, structure.AreaInput{StoreID: request.StoreID, AreaLevelID: request.AreaLevelID, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"area": item})
}

func (h *StructureHandler) ListLocations(c *gin.Context) {
	storeID, ok := parseOptionalInt64(c, "store_id")
	if !ok {
		return
	}
	floorID, ok := parseOptionalInt64(c, "floor_id")
	if !ok {
		return
	}
	items, err := h.service.ListLocations(c.Request.Context(), structure.LocationFilter{StoreID: storeID, FloorID: floorID})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"locations": items})
}

func (h *StructureHandler) CreateLocation(c *gin.Context) {
	var request structureLocationRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid location request"})
		return
	}
	item, err := h.service.CreateLocation(c.Request.Context(), structure.LocationInput{StoreID: request.StoreID, FloorID: request.FloorID, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"location": item})
}

func (h *StructureHandler) UpdateLocation(c *gin.Context) {
	id, ok := parsePathID(c, "invalid location id")
	if !ok {
		return
	}
	var request structureLocationRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid location request"})
		return
	}
	item, err := h.service.UpdateLocation(c.Request.Context(), id, structure.LocationInput{StoreID: request.StoreID, FloorID: request.FloorID, Code: request.Code, Name: request.Name, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"location": item})
}

func (h *StructureHandler) ListUnits(c *gin.Context) {
	buildingID, ok := parseOptionalInt64(c, "building_id")
	if !ok {
		return
	}
	floorID, ok := parseOptionalInt64(c, "floor_id")
	if !ok {
		return
	}
	locationID, ok := parseOptionalInt64(c, "location_id")
	if !ok {
		return
	}
	areaID, ok := parseOptionalInt64(c, "area_id")
	if !ok {
		return
	}
	items, err := h.service.ListUnits(c.Request.Context(), structure.UnitFilter{BuildingID: buildingID, FloorID: floorID, LocationID: locationID, AreaID: areaID})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"units": items})
}

func (h *StructureHandler) CreateUnit(c *gin.Context) {
	var request structureUnitRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid unit request"})
		return
	}
	item, err := h.service.CreateUnit(c.Request.Context(), structure.UnitInput{BuildingID: request.BuildingID, FloorID: request.FloorID, LocationID: request.LocationID, AreaID: request.AreaID, UnitTypeID: request.UnitTypeID, ShopTypeID: request.ShopTypeID, Code: request.Code, FloorArea: request.FloorArea, UseArea: request.UseArea, RentArea: request.RentArea, IsRentable: request.IsRentable, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"unit": item})
}

func (h *StructureHandler) UpdateUnit(c *gin.Context) {
	id, ok := parsePathID(c, "invalid unit id")
	if !ok {
		return
	}
	var request structureUnitRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid unit request"})
		return
	}
	item, err := h.service.UpdateUnit(c.Request.Context(), id, structure.UnitInput{BuildingID: request.BuildingID, FloorID: request.FloorID, LocationID: request.LocationID, AreaID: request.AreaID, UnitTypeID: request.UnitTypeID, ShopTypeID: request.ShopTypeID, Code: request.Code, FloorArea: request.FloorArea, UseArea: request.UseArea, RentArea: request.RentArea, IsRentable: request.IsRentable, Status: request.Status})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"unit": item})
}

func (h *StructureHandler) createStore(c *gin.Context, fn func(context.Context, structure.StoreInput) (*structure.Store, error)) {
	var request structureStoreRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid store request"})
		return
	}
	item, err := fn(c.Request.Context(), structure.StoreInput{
		DepartmentID:     request.DepartmentID,
		StoreTypeID:      request.StoreTypeID,
		ManagementTypeID: request.ManagementTypeID,
		Code:             request.Code,
		Name:             request.Name,
		ShortName:        request.ShortName,
		Status:           request.Status,
	})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusCreated, gin.H{"store": item})
}

func (h *StructureHandler) updateStore(c *gin.Context, id int64, fn func(context.Context, int64, structure.StoreInput) (*structure.Store, error)) {
	var request structureStoreRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid store request"})
		return
	}
	item, err := fn(c.Request.Context(), id, structure.StoreInput{
		DepartmentID:     request.DepartmentID,
		StoreTypeID:      request.StoreTypeID,
		ManagementTypeID: request.ManagementTypeID,
		Code:             request.Code,
		Name:             request.Name,
		ShortName:        request.ShortName,
		Status:           request.Status,
	})
	if err != nil {
		h.renderStructureError(c, err)
		return
	}
	c.JSON(http.StatusOK, gin.H{"store": item})
}

func parsePathID(c *gin.Context, message string) (int64, bool) {
	id, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": message})
		return 0, false
	}
	return id, true
}

func (h *StructureHandler) renderStructureError(c *gin.Context, err error) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, structure.ErrInvalidStructure):
		status = http.StatusBadRequest
	case errors.Is(err, structure.ErrReferenceNotFound), errors.Is(err, structure.ErrParentReferenceNotFound):
		status = http.StatusNotFound
	case errors.Is(err, structure.ErrDuplicateCode):
		status = http.StatusConflict
	}
	c.JSON(status, gin.H{"message": err.Error()})
}
