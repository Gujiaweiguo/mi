package handlers

import (
	"context"
	"net/http"
	"strconv"
	"time"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type workflowService interface {
	ListDefinitions(ctx context.Context) ([]workflow.Definition, error)
	ListInstances(ctx context.Context, filter workflow.InstanceFilter) ([]workflow.Instance, error)
	Start(ctx context.Context, input workflow.StartInput) (*workflow.Instance, error)
	Approve(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error)
	Reject(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error)
	Resubmit(ctx context.Context, input workflow.TransitionInput) (*workflow.Instance, error)
	GetInstance(ctx context.Context, instanceID int64) (*workflow.Instance, error)
	AuditHistory(ctx context.Context, instanceID int64) ([]workflow.AuditEntry, error)
	ReminderHistory(ctx context.Context, instanceID int64) ([]workflow.ReminderAuditRecord, error)
	RunReminders(ctx context.Context, now time.Time, config workflow.ReminderConfig) ([]workflow.ReminderAuditRecord, error)
}

type WorkflowHandler struct {
	service workflowService
	syncer  WorkflowStateSyncer
}

type WorkflowStateSyncer interface {
	SyncWorkflowState(ctx context.Context, instance *workflow.Instance, actorUserID int64) error
}

func NewWorkflowHandler(service workflowService, syncer WorkflowStateSyncer) *WorkflowHandler {
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

// ListDefinitions godoc
//
//	@Summary		List workflow definitions
//	@Description	Returns workflow definitions available for approval flows.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Success		200	{object}	swaggerEnvelope{definitions=[]workflow.Definition}
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/definitions [get]
func (h *WorkflowHandler) ListDefinitions(c *gin.Context) {
	definitions, err := h.service.ListDefinitions(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow definitions"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"definitions": definitions})
}

// ListInstances godoc
//
//	@Summary		List workflow instances
//	@Description	Returns workflow instances filtered by status, document type, and document ID.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			status			query		string	false	"Workflow status"
//	@Param			document_type	query		string	false	"Document type"
//	@Param			document_id		query		int		false	"Document ID"
//	@Success		200				{object}	swaggerEnvelope{instances=[]workflow.Instance}
//	@Failure		400				{object}	swaggerMessageResponse
//	@Failure		401				{object}	swaggerMessageResponse
//	@Failure		500				{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances [get]
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

// Start godoc
//
//	@Summary		Start workflow instance
//	@Description	Starts a workflow instance for a document and assigns the first approval step.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			request	body		startWorkflowRequest	true	"Workflow start request"
//	@Success		201		{object}	swaggerEnvelope{instance=workflow.Instance}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances [post]
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
		c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
		return
	}

	c.JSON(http.StatusCreated, gin.H{"instance": instance})
}

// Approve godoc
//
//	@Summary		Approve workflow instance
//	@Description	Approves the current step of a workflow instance.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Workflow instance ID"
//	@Param			request	body		workflowActionRequest	true	"Workflow action request"
//	@Success		200		{object}	swaggerEnvelope{instance=workflow.Instance}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances/{id}/approve [post]
func (h *WorkflowHandler) Approve(c *gin.Context) {
	h.transition(c, workflow.ActionApprove)
}

// Reject godoc
//
//	@Summary		Reject workflow instance
//	@Description	Rejects the current step of a workflow instance.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Workflow instance ID"
//	@Param			request	body		workflowActionRequest	true	"Workflow action request"
//	@Success		200		{object}	swaggerEnvelope{instance=workflow.Instance}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances/{id}/reject [post]
func (h *WorkflowHandler) Reject(c *gin.Context) {
	h.transition(c, workflow.ActionReject)
}

// Resubmit godoc
//
//	@Summary		Resubmit workflow instance
//	@Description	Resubmits a rejected workflow instance for approval.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			id		path		int						true	"Workflow instance ID"
//	@Param			request	body		workflowActionRequest	true	"Workflow action request"
//	@Success		200		{object}	swaggerEnvelope{instance=workflow.Instance}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		404		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances/{id}/resubmit [post]
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
		c.JSON(status, gin.H{"message": errutil.SafeMessage(err)})
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

// GetInstance godoc
//
//	@Summary		Get workflow instance
//	@Description	Returns a workflow instance by ID.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Workflow instance ID"
//	@Success		200	{object}	swaggerEnvelope{instance=workflow.Instance}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances/{id} [get]
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

// AuditHistory godoc
//
//	@Summary		Get workflow audit history
//	@Description	Returns workflow audit entries for a workflow instance.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Workflow instance ID"
//	@Success		200	{object}	swaggerEnvelope{history=[]workflow.AuditEntry}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances/{id}/audit [get]
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

// ReminderHistory godoc
//
//	@Summary		Get workflow reminder history
//	@Description	Returns reminder audit entries for a workflow instance.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			id	path		int	true	"Workflow instance ID"
//	@Success		200	{object}	swaggerEnvelope{reminders=[]workflow.ReminderAuditRecord}
//	@Failure		400	{object}	swaggerMessageResponse
//	@Failure		401	{object}	swaggerMessageResponse
//	@Failure		404	{object}	swaggerMessageResponse
//	@Failure		500	{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/instances/{id}/reminders [get]
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

type runRemindersRequest struct {
	ReminderType     *string `json:"reminder_type"`
	MinPendingAgeSec *int    `json:"min_pending_age_sec"`
	WindowTruncSec   *int    `json:"window_truncation_sec"`
}

// RunReminders godoc
//
//	@Summary		Run workflow reminders
//	@Description	Evaluates pending workflow reminders and records any reminder emissions.
//	@Tags			Workflow
//	@Accept			json
//	@Produce		json
//	@Param			request	body		runRemindersRequest	false	"Reminder execution request"
//	@Success		200		{object}	swaggerEnvelope{reminders=[]workflow.ReminderAuditRecord}
//	@Failure		400		{object}	swaggerMessageResponse
//	@Failure		401		{object}	swaggerMessageResponse
//	@Failure		500		{object}	swaggerMessageResponse
//	@Security		BearerAuth
//	@Router			/workflow/reminders/run [post]
func (h *WorkflowHandler) RunReminders(c *gin.Context) {
	var req runRemindersRequest
	if c.Request.ContentLength > 0 {
		if err := c.ShouldBindJSON(&req); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid request body"})
			return
		}
	}

	config := workflow.ReminderConfig{}
	if req.ReminderType != nil {
		config.ReminderType = *req.ReminderType
	}
	if req.MinPendingAgeSec != nil {
		if *req.MinPendingAgeSec < 0 {
			c.JSON(http.StatusBadRequest, gin.H{"message": "min_pending_age_sec must be non-negative"})
			return
		}
		config.MinPendingAge = time.Duration(*req.MinPendingAgeSec) * time.Second
	}
	if req.WindowTruncSec != nil {
		if *req.WindowTruncSec < 0 {
			c.JSON(http.StatusBadRequest, gin.H{"message": "window_truncation_sec must be non-negative"})
			return
		}
		config.WindowTruncation = time.Duration(*req.WindowTruncSec) * time.Second
	}

	records, err := h.service.RunReminders(c.Request.Context(), time.Now().UTC(), config)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to run workflow reminders"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"reminders": records})
}
