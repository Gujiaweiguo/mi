package handlers

import (
	"context"
	"errors"
	"net/http"

	"github.com/Gujiaweiguo/mi/backend/internal/http/handlers/errutil"
	"github.com/Gujiaweiguo/mi/backend/internal/http/middleware"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type workflowAdminService interface {
	ListTemplates(ctx context.Context) ([]workflow.Template, error)
	GetTemplate(ctx context.Context, templateID int64) (*workflow.Template, error)
	GetTemplateVersions(ctx context.Context, templateID int64) ([]workflow.Definition, error)
	GetDefinition(ctx context.Context, definitionID int64) (*workflow.Definition, error)
	GetTemplateAudit(ctx context.Context, templateID int64) ([]workflow.DefinitionAuditRecord, error)
	CreateTemplate(ctx context.Context, input workflow.CreateTemplateInput) (*workflow.TemplateDraft, error)
	CreateDraft(ctx context.Context, input workflow.CreateDraftInput) (*workflow.Definition, error)
	UpdateDraftDefinition(ctx context.Context, input workflow.UpdateDraftDefinitionInput) (*workflow.Definition, error)
	ValidateDefinition(ctx context.Context, definitionID int64, actorUserID int64) (*workflow.DefinitionValidationResult, error)
	PublishDefinition(ctx context.Context, input workflow.PublishDefinitionInput) (*workflow.Definition, error)
	DeactivateTemplate(ctx context.Context, input workflow.DeactivateTemplateInput) (*workflow.Template, error)
	RollbackTemplate(ctx context.Context, input workflow.RollbackTemplateInput) (*workflow.Definition, error)
}

type WorkflowAdminHandler struct {
	service workflowAdminService
}

func NewWorkflowAdminHandler(service workflowAdminService) *WorkflowAdminHandler {
	return &WorkflowAdminHandler{service: service}
}

type createWorkflowTemplateRequest struct {
	BusinessGroupID int64  `json:"business_group_id" binding:"required,gt=0"`
	Code            string `json:"code" binding:"required,min=1,max=64"`
	Name            string `json:"name" binding:"required,min=1,max=128"`
	ProcessClass    string `json:"process_class" binding:"required,oneof=lease_contract lease_change invoice overtime_bill invoice_discount"`
	VoucherType     string `json:"voucher_type" binding:"omitempty,max=32"`
}

type createWorkflowDraftRequest struct {
	DefinitionName string `json:"definition_name" binding:"omitempty,max=128"`
	VoucherType    string `json:"voucher_type" binding:"omitempty,max=32"`
}

type updateWorkflowDraftRequest struct {
	Name        string                                 `json:"name" binding:"required,min=1,max=128"`
	VoucherType string                                 `json:"voucher_type" binding:"omitempty,max=32"`
	IsInitial   bool                                   `json:"is_initial"`
	Nodes       []updateWorkflowDraftNodeRequest       `json:"nodes" binding:"required,min=1,dive"`
	Transitions []updateWorkflowDraftTransitionRequest `json:"transitions" binding:"required,min=1,dive"`
}

type updateWorkflowDraftNodeRequest struct {
	FunctionID            int64                                 `json:"function_id" binding:"required,gt=0"`
	RoleID                int64                                 `json:"role_id" binding:"required,gt=0"`
	StepOrder             int                                   `json:"step_order" binding:"required,gt=0"`
	Code                  string                                `json:"code" binding:"required,min=1,max=64"`
	Name                  string                                `json:"name" binding:"required,min=1,max=128"`
	CanSubmitToManager    bool                                  `json:"can_submit_to_manager"`
	ValidatesAfterConfirm bool                                  `json:"validates_after_confirm"`
	PrintsAfterConfirm    bool                                  `json:"prints_after_confirm"`
	ProcessClass          string                                `json:"process_class" binding:"required,oneof=lease_contract lease_change invoice overtime_bill invoice_discount"`
	AssignmentRules       []updateWorkflowAssignmentRuleRequest `json:"assignment_rules"`
}

type updateWorkflowAssignmentRuleRequest struct {
	StrategyType string `json:"strategy_type" binding:"required,oneof=fixed_user fixed_role department_leader submitter_context document_field"`
	ConfigJSON   string `json:"config_json" binding:"required,min=2"`
}

type updateWorkflowDraftTransitionRequest struct {
	FromNodeCode *string `json:"from_node_code" binding:"omitempty,min=1,max=64"`
	ToNodeCode   string  `json:"to_node_code" binding:"required,min=1,max=64"`
	Action       string  `json:"action" binding:"required,oneof=submit approve reject resubmit"`
}

type rollbackWorkflowTemplateRequest struct {
	DefinitionID int64 `json:"definition_id" binding:"required,gt=0"`
}

func (h *WorkflowAdminHandler) ListTemplates(c *gin.Context) {
	templates, err := h.service.ListTemplates(c.Request.Context())
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"message": "failed to load workflow templates"})
		return
	}
	c.JSON(http.StatusOK, gin.H{"templates": templates})
}

func (h *WorkflowAdminHandler) GetTemplate(c *gin.Context) {
	templateID, ok := parsePathID(c, "invalid workflow template id")
	if !ok {
		return
	}
	template, err := h.service.GetTemplate(c.Request.Context(), templateID)
	if err != nil {
		h.writeAdminError(c, err, "failed to load workflow template")
		return
	}
	c.JSON(http.StatusOK, gin.H{"template": template})
}

func (h *WorkflowAdminHandler) GetTemplateVersions(c *gin.Context) {
	templateID, ok := parsePathID(c, "invalid workflow template id")
	if !ok {
		return
	}
	versions, err := h.service.GetTemplateVersions(c.Request.Context(), templateID)
	if err != nil {
		h.writeAdminError(c, err, "failed to load workflow template versions")
		return
	}
	c.JSON(http.StatusOK, gin.H{"definitions": versions})
}

func (h *WorkflowAdminHandler) GetDefinition(c *gin.Context) {
	definitionID, ok := parsePathID(c, "invalid workflow definition id")
	if !ok {
		return
	}
	definition, err := h.service.GetDefinition(c.Request.Context(), definitionID)
	if err != nil {
		h.writeAdminError(c, err, "failed to load workflow definition")
		return
	}
	c.JSON(http.StatusOK, gin.H{"definition": definition})
}

func (h *WorkflowAdminHandler) GetTemplateAudit(c *gin.Context) {
	templateID, ok := parsePathID(c, "invalid workflow template id")
	if !ok {
		return
	}
	history, err := h.service.GetTemplateAudit(c.Request.Context(), templateID)
	if err != nil {
		h.writeAdminError(c, err, "failed to load workflow definition audit")
		return
	}
	c.JSON(http.StatusOK, gin.H{"history": history})
}

func (h *WorkflowAdminHandler) CreateTemplate(c *gin.Context) {
	var req createWorkflowTemplateRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow template request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	result, err := h.service.CreateTemplate(c.Request.Context(), workflow.CreateTemplateInput{
		BusinessGroupID: req.BusinessGroupID,
		Code:            req.Code,
		Name:            req.Name,
		ProcessClass:    req.ProcessClass,
		VoucherType:     req.VoucherType,
		ActorUserID:     sessionUser.ID,
	})
	if err != nil {
		h.writeAdminError(c, err, "failed to create workflow template")
		return
	}
	c.JSON(http.StatusCreated, gin.H{"template": result.Template, "definition": result.Definition})
}

func (h *WorkflowAdminHandler) CreateDraft(c *gin.Context) {
	templateID, ok := parsePathID(c, "invalid workflow template id")
	if !ok {
		return
	}
	var req createWorkflowDraftRequest
	if c.Request.ContentLength > 0 {
		if err := c.ShouldBindJSON(&req); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow draft request"})
			return
		}
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	definition, err := h.service.CreateDraft(c.Request.Context(), workflow.CreateDraftInput{
		TemplateID:     templateID,
		ActorUserID:    sessionUser.ID,
		VoucherType:    req.VoucherType,
		DefinitionName: req.DefinitionName,
	})
	if err != nil {
		h.writeAdminError(c, err, "failed to create workflow draft")
		return
	}
	c.JSON(http.StatusCreated, gin.H{"definition": definition})
}

func (h *WorkflowAdminHandler) UpdateDraftDefinition(c *gin.Context) {
	definitionID, ok := parsePathID(c, "invalid workflow definition id")
	if !ok {
		return
	}
	var req updateWorkflowDraftRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow draft update request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	definition, err := h.service.UpdateDraftDefinition(c.Request.Context(), workflow.UpdateDraftDefinitionInput{
		DefinitionID: definitionID,
		ActorUserID:  sessionUser.ID,
		Name:         req.Name,
		VoucherType:  req.VoucherType,
		IsInitial:    req.IsInitial,
		Nodes:        mapDraftNodes(req.Nodes),
		Transitions:  mapDraftTransitions(req.Transitions),
	})
	if err != nil {
		h.writeAdminError(c, err, "failed to update workflow draft")
		return
	}
	c.JSON(http.StatusOK, gin.H{"definition": definition})
}

func (h *WorkflowAdminHandler) ValidateDefinition(c *gin.Context) {
	definitionID, ok := parsePathID(c, "invalid workflow definition id")
	if !ok {
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	result, err := h.service.ValidateDefinition(c.Request.Context(), definitionID, sessionUser.ID)
	if err != nil {
		h.writeAdminError(c, err, "failed to validate workflow definition")
		return
	}
	c.JSON(http.StatusOK, gin.H{"validation": result})
}

func (h *WorkflowAdminHandler) PublishDefinition(c *gin.Context) {
	definitionID, ok := parsePathID(c, "invalid workflow definition id")
	if !ok {
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	definition, err := h.service.PublishDefinition(c.Request.Context(), workflow.PublishDefinitionInput{DefinitionID: definitionID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.writeAdminError(c, err, "failed to publish workflow definition")
		return
	}
	c.JSON(http.StatusOK, gin.H{"definition": definition})
}

func (h *WorkflowAdminHandler) DeactivateTemplate(c *gin.Context) {
	templateID, ok := parsePathID(c, "invalid workflow template id")
	if !ok {
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	template, err := h.service.DeactivateTemplate(c.Request.Context(), workflow.DeactivateTemplateInput{TemplateID: templateID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.writeAdminError(c, err, "failed to deactivate workflow template")
		return
	}
	c.JSON(http.StatusOK, gin.H{"template": template})
}

func (h *WorkflowAdminHandler) RollbackTemplate(c *gin.Context) {
	templateID, ok := parsePathID(c, "invalid workflow template id")
	if !ok {
		return
	}
	var req rollbackWorkflowTemplateRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"message": "invalid workflow rollback request"})
		return
	}
	sessionUser, ok := middleware.CurrentSessionUser(c)
	if !ok {
		c.JSON(http.StatusUnauthorized, gin.H{"message": "missing session user"})
		return
	}
	definition, err := h.service.RollbackTemplate(c.Request.Context(), workflow.RollbackTemplateInput{TemplateID: templateID, DefinitionID: req.DefinitionID, ActorUserID: sessionUser.ID})
	if err != nil {
		h.writeAdminError(c, err, "failed to roll back workflow template")
		return
	}
	c.JSON(http.StatusOK, gin.H{"definition": definition})
}

func (h *WorkflowAdminHandler) writeAdminError(c *gin.Context, err error, fallback string) {
	status := http.StatusInternalServerError
	switch {
	case errors.Is(err, workflow.ErrTemplateNotFound), errors.Is(err, workflow.ErrDefinitionNotFound):
		status = http.StatusNotFound
	case errors.Is(err, workflow.ErrInvalidState), errors.Is(err, workflow.ErrDefinitionValidationFailed):
		status = http.StatusBadRequest
	}
	response := gin.H{"message": fallback}
	if status != http.StatusInternalServerError {
		response["message"] = errutil.SafeMessage(err)
	}
	if validationErr := (*workflow.DefinitionValidationError)(nil); errors.As(err, &validationErr) {
		response["validation"] = validationErr.Result
	}
	c.JSON(status, response)
}

func mapDraftNodes(nodes []updateWorkflowDraftNodeRequest) []workflow.DefinitionNodeInput {
	result := make([]workflow.DefinitionNodeInput, 0, len(nodes))
	for _, node := range nodes {
		rules := make([]workflow.AssignmentRuleInput, 0, len(node.AssignmentRules))
		for _, rule := range node.AssignmentRules {
			rules = append(rules, workflow.AssignmentRuleInput{
				StrategyType: workflow.AssignmentStrategyType(rule.StrategyType),
				ConfigJSON:   rule.ConfigJSON,
			})
		}
		result = append(result, workflow.DefinitionNodeInput{
			FunctionID:            node.FunctionID,
			RoleID:                node.RoleID,
			StepOrder:             node.StepOrder,
			Code:                  node.Code,
			Name:                  node.Name,
			CanSubmitToManager:    node.CanSubmitToManager,
			ValidatesAfterConfirm: node.ValidatesAfterConfirm,
			PrintsAfterConfirm:    node.PrintsAfterConfirm,
			ProcessClass:          node.ProcessClass,
			AssignmentRules:       rules,
		})
	}
	return result
}

func mapDraftTransitions(transitions []updateWorkflowDraftTransitionRequest) []workflow.DefinitionTransitionInput {
	result := make([]workflow.DefinitionTransitionInput, 0, len(transitions))
	for _, transition := range transitions {
		result = append(result, workflow.DefinitionTransitionInput{
			FromNodeCode: transition.FromNodeCode,
			ToNodeCode:   transition.ToNodeCode,
			Action:       workflow.Action(transition.Action),
		})
	}
	return result
}
