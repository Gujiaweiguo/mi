package workflow

import (
	"context"
	"database/sql"
	"encoding/json"
	"errors"
	"fmt"
	"strings"
	"time"
)

var (
	ErrTemplateNotFound           = errors.New("workflow template not found")
	ErrDefinitionValidationFailed = errors.New("workflow definition validation failed")
)

type DefinitionValidationError struct {
	Result DefinitionValidationResult
}

func (e *DefinitionValidationError) Error() string {
	return ErrDefinitionValidationFailed.Error()
}

func (e *DefinitionValidationError) Unwrap() error {
	return ErrDefinitionValidationFailed
}

type AdminService struct {
	repository *Repository
	db         *sql.DB
}

func NewAdminService(db *sql.DB, repository *Repository) *AdminService {
	return &AdminService{db: db, repository: repository}
}

func (s *AdminService) ListTemplates(ctx context.Context) ([]Template, error) {
	return s.repository.ListTemplates(ctx)
}

func (s *AdminService) GetTemplate(ctx context.Context, templateID int64) (*Template, error) {
	template, err := s.repository.FindTemplateByID(ctx, templateID)
	if err != nil {
		return nil, err
	}
	if template == nil {
		return nil, ErrTemplateNotFound
	}
	return template, nil
}

func (s *AdminService) GetTemplateVersions(ctx context.Context, templateID int64) ([]Definition, error) {
	template, err := s.repository.FindTemplateByID(ctx, templateID)
	if err != nil {
		return nil, err
	}
	if template == nil {
		return nil, ErrTemplateNotFound
	}
	return s.repository.ListDefinitionVersions(ctx, templateID)
}

func (s *AdminService) GetDefinition(ctx context.Context, definitionID int64) (*Definition, error) {
	definition, err := s.repository.FindDefinitionByID(ctx, definitionID)
	if err != nil {
		return nil, err
	}
	if definition == nil {
		return nil, ErrDefinitionNotFound
	}
	if err := s.hydrateAssignmentRules(ctx, definition); err != nil {
		return nil, err
	}
	return definition, nil
}

func (s *AdminService) GetTemplateAudit(ctx context.Context, templateID int64) ([]DefinitionAuditRecord, error) {
	template, err := s.repository.FindTemplateByID(ctx, templateID)
	if err != nil {
		return nil, err
	}
	if template == nil {
		return nil, ErrTemplateNotFound
	}
	return s.repository.ListDefinitionAudit(ctx, templateID)
}

func (s *AdminService) CreateTemplate(ctx context.Context, input CreateTemplateInput) (*TemplateDraft, error) {
	if strings.TrimSpace(input.Code) == "" || strings.TrimSpace(input.Name) == "" || !IsValidObjectType(input.ProcessClass) {
		return nil, ErrInvalidState
	}
	if input.BusinessGroupID <= 0 || input.ActorUserID <= 0 {
		return nil, ErrInvalidState
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow template create transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	templateID, err := s.repository.NextTemplateID(ctx, tx)
	if err != nil {
		return nil, err
	}
	definitionID, err := s.repository.NextDefinitionID(ctx, tx)
	if err != nil {
		return nil, err
	}

	template, err := s.repository.CreateTemplate(ctx, tx, Template{
		ID:              templateID,
		BusinessGroupID: input.BusinessGroupID,
		Code:            strings.TrimSpace(input.Code),
		Name:            strings.TrimSpace(input.Name),
		ProcessClass:    input.ProcessClass,
		Status:          "active",
	})
	if err != nil {
		return nil, err
	}
	definition, err := s.repository.CreateDefinition(ctx, tx, Definition{
		ID:                 definitionID,
		BusinessGroupID:    input.BusinessGroupID,
		WorkflowTemplateID: templateID,
		Code:               template.Code,
		Name:               template.Name,
		VoucherType:        normalizeVoucherType(input.VoucherType),
		Status:             "active",
		ProcessClass:       input.ProcessClass,
		VersionNumber:      1,
		LifecycleStatus:    string(DefinitionLifecycleStatusDraft),
		TransitionsEnabled: true,
	})
	if err != nil {
		return nil, err
	}
	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   template.ID,
		WorkflowDefinitionID: &definition.ID,
		Action:               DefinitionAuditActionCreateDraft,
		ActorUserID:          input.ActorUserID,
		Details:              jsonStringPointer(map[string]any{"source": "template_create", "version": 1}),
	}); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow template create transaction: %w", err)
	}
	return &TemplateDraft{Template: template, Definition: definition}, nil
}

func (s *AdminService) CreateDraft(ctx context.Context, input CreateDraftInput) (*Definition, error) {
	template, err := s.repository.FindTemplateByID(ctx, input.TemplateID)
	if err != nil {
		return nil, err
	}
	if template == nil {
		return nil, ErrTemplateNotFound
	}
	if existingDraft, err := s.repository.FindDraftDefinitionByTemplate(ctx, input.TemplateID); err != nil {
		return nil, err
	} else if existingDraft != nil {
		return nil, ErrInvalidState
	}
	versions, err := s.repository.ListDefinitionVersions(ctx, input.TemplateID)
	if err != nil {
		return nil, err
	}
	nextVersion := 1
	var sourceDefinition *Definition
	if len(versions) > 0 {
		sourceDefinition = &versions[0]
		nextVersion = versions[0].VersionNumber + 1
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow draft create transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	definitionID, err := s.repository.NextDefinitionID(ctx, tx)
	if err != nil {
		return nil, err
	}
	name := strings.TrimSpace(input.DefinitionName)
	if name == "" {
		if sourceDefinition != nil {
			name = sourceDefinition.Name
		} else {
			name = template.Name
		}
	}
	voucherType := normalizeVoucherType(input.VoucherType)
	if sourceDefinition != nil && strings.TrimSpace(input.VoucherType) == "" {
		voucherType = normalizeVoucherType(sourceDefinition.VoucherType)
	}
	definition, err := s.repository.CreateDefinition(ctx, tx, Definition{
		ID:                 definitionID,
		BusinessGroupID:    template.BusinessGroupID,
		WorkflowTemplateID: template.ID,
		Code:               template.Code,
		Name:               name,
		VoucherType:        voucherType,
		IsInitial:          sourceDefinition != nil && sourceDefinition.IsInitial,
		Status:             "active",
		ProcessClass:       template.ProcessClass,
		VersionNumber:      nextVersion,
		LifecycleStatus:    string(DefinitionLifecycleStatusDraft),
		TransitionsEnabled: true,
	})
	if err != nil {
		return nil, err
	}
	if sourceDefinition != nil {
		if err := s.cloneDefinitionGraph(ctx, tx, sourceDefinition, definition); err != nil {
			return nil, err
		}
	}
	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   template.ID,
		WorkflowDefinitionID: &definition.ID,
		Action:               DefinitionAuditActionCreateDraft,
		ActorUserID:          input.ActorUserID,
		Details:              jsonStringPointer(map[string]any{"source": "version_clone", "version": definition.VersionNumber}),
	}); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow draft create transaction: %w", err)
	}
	return s.GetDefinition(ctx, definition.ID)
}

func (s *AdminService) UpdateDraftDefinition(ctx context.Context, input UpdateDraftDefinitionInput) (*Definition, error) {
	definition, err := s.repository.FindDefinitionByID(ctx, input.DefinitionID)
	if err != nil {
		return nil, err
	}
	if definition == nil {
		return nil, ErrDefinitionNotFound
	}
	if definition.LifecycleStatus != string(DefinitionLifecycleStatusDraft) {
		return nil, ErrInvalidState
	}
	if err := validateDraftGraphInput(input); err != nil {
		return nil, err
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow draft update transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	definition.Name = strings.TrimSpace(input.Name)
	definition.VoucherType = normalizeVoucherType(input.VoucherType)
	definition.IsInitial = input.IsInitial
	definition.TransitionsEnabled = len(input.Transitions) > 0
	if err := s.repository.UpdateDefinitionDraftFields(ctx, tx, *definition); err != nil {
		return nil, err
	}
	if err := s.repository.DeleteTransitionsByDefinition(ctx, tx, definition.ID); err != nil {
		return nil, err
	}
	if err := s.repository.DeleteNodesByDefinition(ctx, tx, definition.ID); err != nil {
		return nil, err
	}
	if err := s.persistDraftGraph(ctx, tx, definition, input.Nodes, input.Transitions); err != nil {
		return nil, err
	}
	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   definition.WorkflowTemplateID,
		WorkflowDefinitionID: &definition.ID,
		Action:               DefinitionAuditActionEditDraft,
		ActorUserID:          input.ActorUserID,
		Details:              jsonStringPointer(map[string]any{"node_count": len(input.Nodes), "transition_count": len(input.Transitions)}),
	}); err != nil {
		return nil, err
	}

	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow draft update transaction: %w", err)
	}
	return s.GetDefinition(ctx, definition.ID)
}

func (s *AdminService) ValidateDefinition(ctx context.Context, definitionID int64, actorUserID int64) (*DefinitionValidationResult, error) {
	definition, err := s.GetDefinition(ctx, definitionID)
	if err != nil {
		return nil, err
	}
	result := validateDefinition(definition)

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow definition validate transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	if result.Valid && definition.LifecycleStatus == string(DefinitionLifecycleStatusDraft) {
		if err := s.repository.UpdateDefinitionLifecycleStatus(ctx, tx, definition.ID, DefinitionLifecycleStatusValidated, nil); err != nil {
			return nil, err
		}
	}

	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   definition.WorkflowTemplateID,
		WorkflowDefinitionID: &definition.ID,
		Action:               DefinitionAuditActionValidate,
		ActorUserID:          actorUserID,
		Details:              jsonStringPointer(result),
	}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow definition validate transaction: %w", err)
	}
	return &result, nil
}

func (s *AdminService) PublishDefinition(ctx context.Context, input PublishDefinitionInput) (*Definition, error) {
	definition, err := s.GetDefinition(ctx, input.DefinitionID)
	if err != nil {
		return nil, err
	}
	if definition.LifecycleStatus != string(DefinitionLifecycleStatusDraft) &&
		definition.LifecycleStatus != string(DefinitionLifecycleStatusValidated) {
		return nil, ErrInvalidState
	}
	validation := validateDefinition(definition)
	if !validation.Valid {
		return nil, &DefinitionValidationError{Result: validation}
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow definition publish transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()

	currentPublished, err := s.repository.FindPublishedDefinitionByTemplate(ctx, definition.WorkflowTemplateID)
	if err != nil {
		return nil, err
	}
	if currentPublished != nil && currentPublished.ID != definition.ID {
		if err := s.repository.UpdateDefinitionLifecycleStatus(ctx, tx, currentPublished.ID, DefinitionLifecycleStatusSuperseded, currentPublished.PublishedAt); err != nil {
			return nil, err
		}
	}
	now := time.Now().UTC()
	if err := s.repository.UpdateDefinitionLifecycleStatus(ctx, tx, definition.ID, DefinitionLifecycleStatusPublished, &now); err != nil {
		return nil, err
	}
	publishedDefinitionID := definition.ID
	publishedVersionNumber := definition.VersionNumber
	if err := s.repository.UpdateTemplatePublishedVersion(ctx, tx, definition.WorkflowTemplateID, &publishedDefinitionID, &publishedVersionNumber); err != nil {
		return nil, err
	}
	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   definition.WorkflowTemplateID,
		WorkflowDefinitionID: &definition.ID,
		Action:               DefinitionAuditActionPublish,
		ActorUserID:          input.ActorUserID,
		Details:              jsonStringPointer(map[string]any{"version": definition.VersionNumber}),
	}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow definition publish transaction: %w", err)
	}
	return s.GetDefinition(ctx, definition.ID)
}

func (s *AdminService) DeactivateTemplate(ctx context.Context, input DeactivateTemplateInput) (*Template, error) {
	template, err := s.repository.FindTemplateByID(ctx, input.TemplateID)
	if err != nil {
		return nil, err
	}
	if template == nil {
		return nil, ErrTemplateNotFound
	}
	currentPublished, err := s.repository.FindPublishedDefinitionByTemplate(ctx, input.TemplateID)
	if err != nil {
		return nil, err
	}
	if currentPublished == nil {
		return nil, ErrInvalidState
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow template deactivate transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	if err := s.repository.UpdateDefinitionLifecycleStatus(ctx, tx, currentPublished.ID, DefinitionLifecycleStatusDeactivated, currentPublished.PublishedAt); err != nil {
		return nil, err
	}
	if err := s.repository.UpdateTemplatePublishedVersion(ctx, tx, template.ID, nil, nil); err != nil {
		return nil, err
	}
	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   template.ID,
		WorkflowDefinitionID: &currentPublished.ID,
		Action:               DefinitionAuditActionDeactivate,
		ActorUserID:          input.ActorUserID,
		Details:              jsonStringPointer(map[string]any{"version": currentPublished.VersionNumber}),
	}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow template deactivate transaction: %w", err)
	}
	return s.GetTemplate(ctx, template.ID)
}

func (s *AdminService) RollbackTemplate(ctx context.Context, input RollbackTemplateInput) (*Definition, error) {
	template, err := s.repository.FindTemplateByID(ctx, input.TemplateID)
	if err != nil {
		return nil, err
	}
	if template == nil {
		return nil, ErrTemplateNotFound
	}
	target, err := s.GetDefinition(ctx, input.DefinitionID)
	if err != nil {
		return nil, err
	}
	if target.WorkflowTemplateID != input.TemplateID {
		return nil, ErrInvalidState
	}
	if target.LifecycleStatus != string(DefinitionLifecycleStatusSuperseded) &&
		target.LifecycleStatus != string(DefinitionLifecycleStatusDeactivated) {
		return nil, ErrInvalidState
	}
	validation := validateDefinition(target)
	if !validation.Valid {
		return nil, &DefinitionValidationError{Result: validation}
	}

	tx, err := s.db.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin workflow template rollback transaction: %w", err)
	}
	defer func() { _ = tx.Rollback() }()
	currentPublished, err := s.repository.FindPublishedDefinitionByTemplate(ctx, input.TemplateID)
	if err != nil {
		return nil, err
	}
	if currentPublished != nil && currentPublished.ID != target.ID {
		if err := s.repository.UpdateDefinitionLifecycleStatus(ctx, tx, currentPublished.ID, DefinitionLifecycleStatusSuperseded, currentPublished.PublishedAt); err != nil {
			return nil, err
		}
	}
	now := time.Now().UTC()
	if err := s.repository.UpdateDefinitionLifecycleStatus(ctx, tx, target.ID, DefinitionLifecycleStatusPublished, &now); err != nil {
		return nil, err
	}
	publishedDefinitionID := target.ID
	publishedVersionNumber := target.VersionNumber
	if err := s.repository.UpdateTemplatePublishedVersion(ctx, tx, input.TemplateID, &publishedDefinitionID, &publishedVersionNumber); err != nil {
		return nil, err
	}
	if err := s.insertDefinitionAudit(ctx, tx, DefinitionAuditRecord{
		WorkflowTemplateID:   template.ID,
		WorkflowDefinitionID: &target.ID,
		Action:               DefinitionAuditActionRollback,
		ActorUserID:          input.ActorUserID,
		Details:              jsonStringPointer(map[string]any{"version": target.VersionNumber}),
	}); err != nil {
		return nil, err
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit workflow template rollback transaction: %w", err)
	}
	return s.GetDefinition(ctx, target.ID)
}

func (s *AdminService) cloneDefinitionGraph(ctx context.Context, tx *sql.Tx, source, target *Definition) error {
	nodeIDMap := make(map[int64]int64, len(source.Nodes))
	for _, node := range sortNodes(source.Nodes) {
		nextNodeID, err := s.repository.NextNodeID(ctx, tx)
		if err != nil {
			return err
		}
		createdNode, err := s.repository.CreateNode(ctx, tx, Node{
			ID:                    nextNodeID,
			WorkflowDefinitionID:  target.ID,
			FunctionID:            node.FunctionID,
			RoleID:                node.RoleID,
			StepOrder:             node.StepOrder,
			Code:                  node.Code,
			Name:                  node.Name,
			CanSubmitToManager:    node.CanSubmitToManager,
			ValidatesAfterConfirm: node.ValidatesAfterConfirm,
			PrintsAfterConfirm:    node.PrintsAfterConfirm,
			ProcessClass:          node.ProcessClass,
		})
		if err != nil {
			return err
		}
		nodeIDMap[node.ID] = createdNode.ID
		rules, err := s.repository.ListAssignmentRulesByNode(ctx, node.ID)
		if err != nil {
			return err
		}
		for _, rule := range rules {
			if _, err := s.repository.CreateAssignmentRule(ctx, tx, AssignmentRule{
				WorkflowNodeID: createdNode.ID,
				StrategyType:   rule.StrategyType,
				ConfigJSON:     rule.ConfigJSON,
			}); err != nil {
				return err
			}
		}
	}
	for _, transition := range source.Transitions {
		nextTransitionID, err := s.repository.NextTransitionID(ctx, tx)
		if err != nil {
			return err
		}
		var fromNodeID *int64
		if transition.FromNodeID != nil {
			mapped := nodeIDMap[*transition.FromNodeID]
			fromNodeID = &mapped
		}
		toNodeID := nodeIDMap[transition.ToNodeID]
		if _, err := s.repository.CreateTransition(ctx, tx, Transition{
			ID:                   nextTransitionID,
			WorkflowDefinitionID: target.ID,
			FromNodeID:           fromNodeID,
			ToNodeID:             toNodeID,
			Action:               transition.Action,
		}); err != nil {
			return err
		}
	}
	return nil
}

func (s *AdminService) persistDraftGraph(ctx context.Context, tx *sql.Tx, definition *Definition, nodes []DefinitionNodeInput, transitions []DefinitionTransitionInput) error {
	nodeCodeMap := make(map[string]int64, len(nodes))
	for _, node := range nodes {
		nextNodeID, err := s.repository.NextNodeID(ctx, tx)
		if err != nil {
			return err
		}
		createdNode, err := s.repository.CreateNode(ctx, tx, Node{
			ID:                    nextNodeID,
			WorkflowDefinitionID:  definition.ID,
			FunctionID:            node.FunctionID,
			RoleID:                node.RoleID,
			StepOrder:             node.StepOrder,
			Code:                  strings.TrimSpace(node.Code),
			Name:                  strings.TrimSpace(node.Name),
			CanSubmitToManager:    node.CanSubmitToManager,
			ValidatesAfterConfirm: node.ValidatesAfterConfirm,
			PrintsAfterConfirm:    node.PrintsAfterConfirm,
			ProcessClass:          strings.TrimSpace(node.ProcessClass),
		})
		if err != nil {
			return err
		}
		nodeCodeMap[createdNode.Code] = createdNode.ID
		for _, rule := range node.AssignmentRules {
			if _, err := s.repository.CreateAssignmentRule(ctx, tx, AssignmentRule{
				WorkflowNodeID: createdNode.ID,
				StrategyType:   rule.StrategyType,
				ConfigJSON:     strings.TrimSpace(rule.ConfigJSON),
			}); err != nil {
				return err
			}
		}
	}
	for _, transition := range transitions {
		nextTransitionID, err := s.repository.NextTransitionID(ctx, tx)
		if err != nil {
			return err
		}
		var fromNodeID *int64
		if transition.FromNodeCode != nil {
			mapped := nodeCodeMap[strings.TrimSpace(*transition.FromNodeCode)]
			fromNodeID = &mapped
		}
		toNodeID := nodeCodeMap[strings.TrimSpace(transition.ToNodeCode)]
		if _, err := s.repository.CreateTransition(ctx, tx, Transition{
			ID:                   nextTransitionID,
			WorkflowDefinitionID: definition.ID,
			FromNodeID:           fromNodeID,
			ToNodeID:             toNodeID,
			Action:               transition.Action,
		}); err != nil {
			return err
		}
	}
	return nil
}

func (s *AdminService) hydrateAssignmentRules(ctx context.Context, definition *Definition) error {
	if definition == nil {
		return nil
	}
	for i := range definition.Nodes {
		rules, err := s.repository.ListAssignmentRulesByNode(ctx, definition.Nodes[i].ID)
		if err != nil {
			return err
		}
		definition.Nodes[i].AssignmentRules = rules
	}
	return nil
}

func (s *AdminService) insertDefinitionAudit(ctx context.Context, tx *sql.Tx, record DefinitionAuditRecord) error {
	_, err := s.repository.InsertDefinitionAudit(ctx, tx, record)
	return err
}

func validateDraftGraphInput(input UpdateDraftDefinitionInput) error {
	if strings.TrimSpace(input.Name) == "" || input.ActorUserID <= 0 || input.DefinitionID <= 0 {
		return ErrInvalidState
	}
	if len(input.Nodes) == 0 {
		return ErrInvalidState
	}
	seenCodes := make(map[string]struct{}, len(input.Nodes))
	for _, node := range input.Nodes {
		code := strings.TrimSpace(node.Code)
		name := strings.TrimSpace(node.Name)
		if code == "" || name == "" || node.FunctionID <= 0 || node.RoleID <= 0 || node.StepOrder <= 0 {
			return ErrInvalidState
		}
		if _, exists := seenCodes[code]; exists {
			return ErrInvalidState
		}
		seenCodes[code] = struct{}{}
	}
	for _, transition := range input.Transitions {
		if transition.Action == "" || strings.TrimSpace(transition.ToNodeCode) == "" {
			return ErrInvalidState
		}
		if _, exists := seenCodes[strings.TrimSpace(transition.ToNodeCode)]; !exists {
			return ErrInvalidState
		}
		if transition.FromNodeCode != nil {
			if _, exists := seenCodes[strings.TrimSpace(*transition.FromNodeCode)]; !exists {
				return ErrInvalidState
			}
		}
	}
	return nil
}

func validateDefinition(definition *Definition) DefinitionValidationResult {
	issues := make([]DefinitionValidationIssue, 0)
	if definition == nil {
		issues = append(issues, DefinitionValidationIssue{Code: "definition_missing", Message: "workflow definition not found"})
		return DefinitionValidationResult{Valid: false, Issues: issues}
	}
	if !IsValidObjectType(definition.ProcessClass) {
		issues = append(issues, DefinitionValidationIssue{Code: "invalid_process_class", Field: "process_class", Message: "unsupported workflow object type"})
	}
	if len(definition.Nodes) == 0 {
		issues = append(issues, DefinitionValidationIssue{Code: "missing_nodes", Field: "nodes", Message: "workflow definition must contain at least one node"})
	}
	if len(definition.Transitions) == 0 {
		issues = append(issues, DefinitionValidationIssue{Code: "missing_transitions", Field: "transitions", Message: "workflow definition must contain at least one transition"})
	}
	seenCodes := make(map[string]struct{}, len(definition.Nodes))
	nodeCodeByID := make(map[int64]string, len(definition.Nodes))
	nodeIDs := make(map[int64]struct{}, len(definition.Nodes))
	for _, node := range definition.Nodes {
		if _, exists := seenCodes[node.Code]; exists {
			issues = append(issues, DefinitionValidationIssue{Code: "duplicate_node_code", Field: "nodes.code", Message: "workflow node codes must be unique"})
		}
		seenCodes[node.Code] = struct{}{}
		nodeCodeByID[node.ID] = node.Code
		nodeIDs[node.ID] = struct{}{}
		if strings.TrimSpace(node.ProcessClass) != "" && node.ProcessClass != definition.ProcessClass {
			issues = append(issues, DefinitionValidationIssue{Code: "node_process_class_mismatch", Field: node.Code, Message: "workflow node process class must match definition process class"})
		}
		for _, rule := range node.AssignmentRules {
			if strings.TrimSpace(rule.ConfigJSON) == "" {
				issues = append(issues, DefinitionValidationIssue{Code: "empty_assignment_rule_config", Field: node.Code, Message: "assignment rule config is required"})
				continue
			}
			switch rule.StrategyType {
			case AssignmentStrategyFixedUser, AssignmentStrategyFixedRole, AssignmentStrategyDepartmentLead, AssignmentStrategySubmitter, AssignmentStrategyDocumentField:
			default:
				issues = append(issues, DefinitionValidationIssue{Code: "unsupported_assignment_strategy", Field: node.Code, Message: "assignment rule strategy is unsupported"})
				continue
			}
			if issue := validateAssignmentRuleConfig(definition.ProcessClass, node.Code, rule); issue != nil {
				issues = append(issues, *issue)
			}
		}
	}
	hasSubmitTransition := false
	entryNodeIDs := make([]int64, 0)
	adjacency := make(map[int64][]int64, len(definition.Nodes))
	for _, transition := range definition.Transitions {
		switch transition.Action {
		case ActionSubmit, ActionApprove, ActionReject, ActionResubmit:
		default:
			issues = append(issues, DefinitionValidationIssue{Code: "unsupported_transition_action", Field: "transitions", Message: "workflow transition action is unsupported"})
		}
		if transition.FromNodeID == nil && transition.Action == ActionSubmit {
			hasSubmitTransition = true
			entryNodeIDs = append(entryNodeIDs, transition.ToNodeID)
		} else if transition.FromNodeID == nil && transition.Action != ActionSubmit {
			issues = append(issues, DefinitionValidationIssue{Code: "invalid_transition_source", Field: "transitions", Message: "only submit transitions may omit a source node"})
		}
		if transition.FromNodeID != nil {
			if _, exists := nodeIDs[*transition.FromNodeID]; !exists {
				issues = append(issues, DefinitionValidationIssue{Code: "invalid_transition_from_node", Field: "transitions", Message: "transition source node does not exist"})
			} else {
				adjacency[*transition.FromNodeID] = append(adjacency[*transition.FromNodeID], transition.ToNodeID)
			}
		}
		if _, exists := nodeIDs[transition.ToNodeID]; !exists {
			issues = append(issues, DefinitionValidationIssue{Code: "invalid_transition_to_node", Field: "transitions", Message: "transition target node does not exist"})
		}
	}
	if !hasSubmitTransition {
		issues = append(issues, DefinitionValidationIssue{Code: "missing_submit_transition", Field: "transitions", Message: "workflow definition must contain a submit transition"})
	}
	if len(entryNodeIDs) > 0 {
		visited := make(map[int64]struct{}, len(definition.Nodes))
		stack := append([]int64(nil), entryNodeIDs...)
		for len(stack) > 0 {
			nodeID := stack[len(stack)-1]
			stack = stack[:len(stack)-1]
			if _, seen := visited[nodeID]; seen {
				continue
			}
			visited[nodeID] = struct{}{}
			for _, nextNodeID := range adjacency[nodeID] {
				if _, exists := nodeIDs[nextNodeID]; exists {
					stack = append(stack, nextNodeID)
				}
			}
		}
		for nodeID := range nodeIDs {
			if _, seen := visited[nodeID]; !seen {
				issues = append(issues, DefinitionValidationIssue{Code: "unreachable_node", Field: nodeCodeByID[nodeID], Message: "workflow node is unreachable from submit entry"})
			}
		}
		hasTerminalNode := false
		for nodeID := range visited {
			if len(adjacency[nodeID]) == 0 {
				hasTerminalNode = true
				break
			}
		}
		if !hasTerminalNode {
			issues = append(issues, DefinitionValidationIssue{Code: "missing_terminal_outcome", Field: "transitions", Message: "workflow definition must contain at least one reachable terminal outcome"})
		}
	}
	return DefinitionValidationResult{Valid: len(issues) == 0, Issues: issues}
}

func validateAssignmentRuleConfig(processClass, nodeCode string, rule AssignmentRule) *DefinitionValidationIssue {
	config := make(map[string]any)
	if err := json.Unmarshal([]byte(rule.ConfigJSON), &config); err != nil {
		return &DefinitionValidationIssue{Code: "invalid_assignment_rule_config", Field: nodeCode, Message: "assignment rule config must be valid JSON"}
	}
	switch rule.StrategyType {
	case AssignmentStrategyFixedUser:
		if !configHasPositiveNumber(config, "user_id") {
			return &DefinitionValidationIssue{Code: "invalid_assignment_rule_config", Field: nodeCode, Message: "fixed-user assignment rule requires a positive user_id"}
		}
	case AssignmentStrategyFixedRole:
		if !configHasPositiveNumber(config, "role_id") {
			return &DefinitionValidationIssue{Code: "invalid_assignment_rule_config", Field: nodeCode, Message: "fixed-role assignment rule requires a positive role_id"}
		}
	case AssignmentStrategyDocumentField:
		fieldPath, ok := config["field_path"].(string)
		if !ok || strings.TrimSpace(fieldPath) == "" {
			return &DefinitionValidationIssue{Code: "missing_field_path_in_document_field_rule", Field: nodeCode, Message: "document-field assignment rule requires a non-empty field_path"}
		}
		if !isSupportedDocumentFieldPath(processClass, fieldPath) {
			return &DefinitionValidationIssue{Code: "unsupported_field_reference", Field: nodeCode, Message: "document-field assignment rule references an unsupported field_path"}
		}
	}
	return nil
}

func configHasPositiveNumber(config map[string]any, key string) bool {
	value, exists := config[key]
	if !exists {
		return false
	}
	switch number := value.(type) {
	case float64:
		return number > 0
	case int:
		return number > 0
	case int64:
		return number > 0
	default:
		return false
	}
}

func isSupportedDocumentFieldPath(processClass, fieldPath string) bool {
	supportedPaths := supportedDocumentFieldPathsByObjectType[ObjectType(processClass)]
	_, ok := supportedPaths[strings.TrimSpace(fieldPath)]
	return ok
}

var supportedDocumentFieldPathsByObjectType = map[ObjectType]map[string]struct{}{
	ObjectTypeLeaseContract: {
		"department_id": {},
		"created_by":    {},
		"updated_by":    {},
	},
	ObjectTypeLeaseChange: {
		"department_id": {},
		"created_by":    {},
		"updated_by":    {},
	},
	ObjectTypeInvoice: {
		"created_by": {},
		"updated_by": {},
	},
	ObjectTypeOvertimeBill: {
		"created_by": {},
		"updated_by": {},
	},
	ObjectTypeInvoiceDiscount: {
		"created_by": {},
		"updated_by": {},
	},
}

func jsonStringPointer(value any) *string {
	encoded, err := json.Marshal(value)
	if err != nil {
		fallback := `{"message":"marshal_failed"}`
		return &fallback
	}
	text := string(encoded)
	return &text
}
