package handlers

import (
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/sales"
	"github.com/gin-gonic/gin"
)

const salesDateLayout = "2006-01-02"

type SalesHandler struct{ service *sales.Service }

func NewSalesHandler(service *sales.Service) *SalesHandler {
	return &SalesHandler{service: service}
}

type createDailySaleRequest struct {
	StoreID     int64   `json:"store_id" binding:"required"`
	UnitID      int64   `json:"unit_id" binding:"required"`
	SaleDate    string  `json:"sale_date" binding:"required"`
	SalesAmount float64 `json:"sales_amount" binding:"required"`
}

type createTrafficRequest struct {
	StoreID      int64  `json:"store_id" binding:"required"`
	TrafficDate  string `json:"traffic_date" binding:"required"`
	InboundCount int    `json:"inbound_count" binding:"required"`
}

// ListDailySales godoc
//
//	@Summary		List daily sales
//	@Description	Returns daily sales records filtered by store, unit, and date range.
//	@Tags			Sales
//	@Accept			json
//	@Produce		json
//	@Param			store_id	query		int		false	"Store ID"
//	@Param			unit_id		query		int		false	"Unit ID"
//	@Param			date_from	query		string	false	"Start date (YYYY-MM-DD)"
//	@Param			date_to		query		string	false	"End date (YYYY-MM-DD)"
//	@Success		200			{object}	swaggerEnvelope{daily_sales=[]sales.DailySale}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/sales/daily [get]
func (h *SalesHandler) ListDailySales(c *gin.Context) {
	filter, ok := buildDailySaleFilter(c)
	if !ok {
		return
	}
	items, err := h.service.ListDailySales(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load daily sales"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"daily_sales": items})
}

// CreateDailySale godoc
//
//	@Summary		Create daily sale
//	@Description	Creates or updates a daily sales record for a unit on a given date.
//	@Tags			Sales
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createDailySaleRequest	true	"Daily sale request"
//	@Success		201		{object}	swaggerEnvelope{daily_sale=sales.DailySale}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/sales/daily [post]
func (h *SalesHandler) CreateDailySale(c *gin.Context) {
	var request createDailySaleRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid daily sale request"})
		return
	}
	saleDate, err := time.Parse(salesDateLayout, request.SaleDate)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid sale_date"})
		return
	}
	item, err := h.service.CreateDailySale(c.Request.Context(), sales.CreateDailySaleInput{StoreID: request.StoreID, UnitID: request.UnitID, SaleDate: saleDate, SalesAmount: request.SalesAmount})
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": err.Error()})
		return
	}
	c.JSON(http.StatusCreated, gin.H{"daily_sale": item})
}

// ListTraffic godoc
//
//	@Summary		List customer traffic
//	@Description	Returns customer traffic records filtered by store and date range.
//	@Tags			Sales
//	@Accept			json
//	@Produce		json
//	@Param			store_id	query		int		false	"Store ID"
//	@Param			date_from	query		string	false	"Start date (YYYY-MM-DD)"
//	@Param			date_to		query		string	false	"End date (YYYY-MM-DD)"
//	@Success		200			{object}	swaggerEnvelope{customer_traffic=[]sales.CustomerTraffic}
//	@Failure		400			{object}	swaggerMessageResponse
//	@Failure		401			{object}	swaggerMessageResponse
//	@Failure		500			{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/sales/traffic [get]
func (h *SalesHandler) ListTraffic(c *gin.Context) {
	filter, ok := buildTrafficFilter(c)
	if !ok {
		return
	}
	items, err := h.service.ListTraffic(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load customer traffic"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"customer_traffic": items})
}

// CreateTraffic godoc
//
//	@Summary		Create customer traffic record
//	@Description	Creates or updates a customer traffic record for a store on a given date.
//	@Tags			Sales
//	@Accept			json
//	@Produce		json
//	@Param			request	body		createTrafficRequest	true	"Customer traffic request"
//	@Success		201		{object}	swaggerEnvelope{traffic=sales.CustomerTraffic}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/sales/traffic [post]
func (h *SalesHandler) CreateTraffic(c *gin.Context) {
	var request createTrafficRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid traffic request"})
		return
	}
	trafficDate, err := time.Parse(salesDateLayout, request.TrafficDate)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid traffic_date"})
		return
	}
	item, err := h.service.CreateTraffic(c.Request.Context(), sales.CreateTrafficInput{StoreID: request.StoreID, TrafficDate: trafficDate, InboundCount: request.InboundCount})
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": err.Error()})
		return
	}
	c.JSON(http.StatusCreated, gin.H{"traffic": item})
}

func buildDailySaleFilter(c *gin.Context) (sales.DailySaleFilter, bool) {
	storeID, ok := parseOptionalInt64(c, "store_id")
	if !ok {
		return sales.DailySaleFilter{}, false
	}
	unitID, ok := parseOptionalInt64(c, "unit_id")
	if !ok {
		return sales.DailySaleFilter{}, false
	}
	dateFrom, ok := parseOptionalDate(c, "date_from")
	if !ok {
		return sales.DailySaleFilter{}, false
	}
	dateTo, ok := parseOptionalDate(c, "date_to")
	if !ok {
		return sales.DailySaleFilter{}, false
	}
	return sales.DailySaleFilter{StoreID: storeID, UnitID: unitID, DateFrom: dateFrom, DateTo: dateTo}, true
}

func buildTrafficFilter(c *gin.Context) (sales.TrafficFilter, bool) {
	storeID, ok := parseOptionalInt64(c, "store_id")
	if !ok {
		return sales.TrafficFilter{}, false
	}
	dateFrom, ok := parseOptionalDate(c, "date_from")
	if !ok {
		return sales.TrafficFilter{}, false
	}
	dateTo, ok := parseOptionalDate(c, "date_to")
	if !ok {
		return sales.TrafficFilter{}, false
	}
	return sales.TrafficFilter{StoreID: storeID, DateFrom: dateFrom, DateTo: dateTo}, true
}

func parseOptionalInt64(c *gin.Context, key string) (*int64, bool) {
	value := c.Query(key)
	if value == "" {
		return nil, true
	}
	parsed, err := strconv.ParseInt(value, 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid " + key})
		return nil, false
	}
	return &parsed, true
}

func parseOptionalDate(c *gin.Context, key string) (*time.Time, bool) {
	value := c.Query(key)
	if value == "" {
		return nil, true
	}
	parsed, err := time.Parse(salesDateLayout, value)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid " + key})
		return nil, false
	}
	return &parsed, true
}
