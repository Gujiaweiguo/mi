package handlers

import (
	"context"
	"net/http"
	"strconv"

	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type WorkflowHandler struct {
	service *workflow.Service
	syncer  WorkflowStateSyncer
}

type WorkflowStateSyncer interface {
	SyncWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error
}

func NewWorkflowHandler(service *workflow.Service, syncer WorkflowStateSyncer) *WorkflowHandler {
	return &WorkflowHandler{service: service, syncer: syncer}
}

type startWorkflowRequest struct {
	DefinitionCode string `json:"definition_code" binding:"required"`
	DocumentType   string `json:"document_type" binding:"required"`
	DocumentID     int64  `json:"document_id" binding:"required"`
	Comment        string `json:"comment"`
}

type workflowActionRequest struct {
	Comment        string `json:"comment"`
	IdempotencyKey string `json:"idempotency_key" binding:"required"`
}

func (h *WorkflowHandler) ListDefinitions(c *gin.Context) {
	definitions, err := h.service.ListDefinitions(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow definitions"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"definitions": definitions})
}

func (h *WorkflowHandler) ListInstances(c *gin.Context) {
	filter := workflow.InstanceFilter{}

	if status := c.Query("status"); status != "" {
		statusValue := workflow.InstanceStatus(status)
		filter.Status = &statusValue
	}
	if documentType := c.Query("document_type"); documentType != "" {
		filter.DocumentType = &documentType
	}
	if documentID := c.Query("document_id"); documentID != "" {
		documentIDValue, err := strconv.ParseInt(documentID, 10, 64)
		if err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid document_id"})
			return
		}
		filter.DocumentID = &documentIDValue
	}

	instances, err := h.service.ListInstances(c.Request.Context(), filter)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow instances"})
		return
	}

	c.JSON(http.StatusOK, gin.H{"instances": instances})
}

func (h *WorkflowHandler) Start(c *gin.Context) {
	var request startWorkflowRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow start request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}

	instance, err := h.service.Start(c.Request.Context(), workflow.StartInput{
		DefinitionCode: request.DefinitionCode,
		DocumentType:   request.DocumentType,
		DocumentID:     request.DocumentID,
		ActorUserID:    sessionUser.ID,
		DepartmentID:   sessionUser.DepartmentID,
		IdempotencyKey: "start-" + request.DefinitionCode + "-" + strconv.FormatInt(request.DocumentID, 10),
		Comment:        request.Comment,
	})
	if err != nil {
		status := http.StatusInternalServerError
		if err == workflow.ErrDefinitionNotFound || err == workflow.ErrInvalidState {
			status = http.StatusBadRequest
		}
		c.JSON(status, gin.H{"message": err.Error()})
		return
	}

	c.JSON(http.StatusCreated, gin.H{"instance": instance})
}

func (h *WorkflowHandler) Approve(c *gin.Context) {
	h.transition(c, workflow.ActionApprove)
}

func (h *WorkflowHandler) Reject(c *gin.Context) {
	h.transition(c, workflow.ActionReject)
}

func (h *WorkflowHandler) Resubmit(c *gin.Context) {
	h.transition(c, workflow.ActionResubmit)
}

func (h *WorkflowHandler) transition(c *gin.Context, action workflow.Action) {
	instanceID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow instance id"})
		return
	}

	var request workflowActionRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow action request"})
		return
	}

	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}

	input := workflow.TransitionInput{InstanceID: instanceID, ActorUserID: sessionUser.ID, DepartmentID: sessionUser.DepartmentID, IdempotencyKey: request.IdempotencyKey, Comment: request.Comment}

	var instance *workflow.Instance
	switch action {
	case workflow.ActionApprove:
		instance, err = h.service.Approve(c.Request.Context(), input)
	case workflow.ActionReject:
		instance, err = h.service.Reject(c.Request.Context(), input)
	case workflow.ActionResubmit:
		instance, err = h.service.Resubmit(c.Request.Context(), input)
	}
	if err != nil {
		status := http.StatusInternalServerError
		if err == workflow.ErrInvalidState || err == workflow.ErrDefinitionNotFound {
			status = http.StatusBadRequest
		}
		c.JSON(status, gin.H{"message": err.Error()})
		return
	}
	if h.syncer != nil {
		if err := h.syncer.SyncWorkflowState(c.Request.Context(), instance, sessionUser.ID); err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to sync workflow document state"})
			return
		}
	}

	c.JSON(http.StatusOK, gin.H{"instance": instance})
}

func (h *WorkflowHandler) GetInstance(c *gin.Context) {
	instanceID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow instance id"})
		return
	}
	instance, err := h.service.GetInstance(c.Request.Context(), instanceID)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow instance"})
		return
	}
	if instance == nil {
		c.JSON(http.StatusNotFound, gin.H{"message": "workflow instance not found"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"instance": instance})
}

func (h *WorkflowHandler) AuditHistory(c *gin.Context) {
	instanceID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow instance id"})
		return
	}
	history, err := h.service.AuditHistory(c.Request.Context(), instanceID)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow audit history"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"history": history})
}

func (h *WorkflowHandler) ReminderHistory(c *gin.Context) {
	instanceID, err := strconv.ParseInt(c.Param("id"), 10, 64)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow instance id"})
		return
	}
	reminders, err := h.service.ReminderHistory(c.Request.Context(), instanceID)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow reminder history"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"reminders": reminders})
}
