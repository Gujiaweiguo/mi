package notification

import (
	"net/http"
	"strconv"
	"strings"

	"github.com/gin-gonic/gin"
)

// NotificationHandler serves notification history endpoints.
type NotificationHandler struct {
	repository *Repository
}

// NewHandler constructs a notification history handler.
func NewHandler(repository *Repository) *NotificationHandler {
	return &NotificationHandler{repository: repository}
}

// ListNotifications returns paginated notification history.
func (h *NotificationHandler) ListNotifications(c *gin.Context) {
	if h == nil || h.repository == nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "notification repository unavailable"})
		return
	}

	page, err := parsePositiveIntQuery(c, "page", 1)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page"})
		return
	}
	pageSize, err := parsePositiveIntQuery(c, "page_size", 20)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid page size"})
		return
	}

	params := ListParams{
		Page:          page,
		PageSize:      pageSize,
		EventType:     strings.TrimSpace(c.Query("event_type")),
		AggregateType: strings.TrimSpace(c.Query("aggregate_type")),
		Status:        strings.TrimSpace(c.Query("status")),
	}
	if aggregateIDText := strings.TrimSpace(c.Query("aggregate_id")); aggregateIDText != "" {
		aggregateID, parseErr := strconv.ParseInt(aggregateIDText, 10, 64)
		if parseErr != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid aggregate_id"})
			return
		}
		params.AggregateID = &aggregateID
	}

	items, total, err := h.repository.ListOutbox(c.Request.Context(), nil, params)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to list notifications"})
		return
	}
	if items == nil {
		items = []*OutboxEntry{}
	}

	c.JSON(http.StatusOK, gin.H{"items": items, "total": total, "page": page, "page_size": pageSize})
}

func parsePositiveIntQuery(c *gin.Context, key string, defaultValue int) (int, error) {
	value := strings.TrimSpace(c.DefaultQuery(key, strconv.Itoa(defaultValue)))
	parsed, err := strconv.Atoi(value)
	if err != nil {
		return 0, err
	}
	if parsed <= 0 {
		return defaultValue, nil
	}
	return parsed, nil
}
