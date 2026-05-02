package handlers

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/Gujiaweiguo/mi/backend/internal/auth"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	"github.com/gin-gonic/gin"
)

type mockWorkflowAdminService struct {
	listTemplatesFunc         func(ctx context.Context) ([]workflow.Template, error)
	getTemplateFunc           func(ctx context.Context, templateID int64) (*workflow.Template, error)
	getTemplateVersionsFunc   func(ctx context.Context, templateID int64) ([]workflow.Definition, error)
	getDefinitionFunc         func(ctx context.Context, definitionID int64) (*workflow.Definition, error)
	getTemplateAuditFunc      func(ctx context.Context, templateID int64) ([]workflow.DefinitionAuditRecord, error)
	createTemplateFunc        func(ctx context.Context, input workflow.CreateTemplateInput) (*workflow.TemplateDraft, error)
	createDraftFunc           func(ctx context.Context, input workflow.CreateDraftInput) (*workflow.Definition, error)
	updateDraftDefinitionFunc func(ctx context.Context, input workflow.UpdateDraftDefinitionInput) (*workflow.Definition, error)
	validateDefinitionFunc    func(ctx context.Context, definitionID int64, actorUserID int64) (*workflow.DefinitionValidationResult, error)
	publishDefinitionFunc     func(ctx context.Context, input workflow.PublishDefinitionInput) (*workflow.Definition, error)
	deactivateTemplateFunc    func(ctx context.Context, input workflow.DeactivateTemplateInput) (*workflow.Template, error)
	rollbackTemplateFunc      func(ctx context.Context, input workflow.RollbackTemplateInput) (*workflow.Definition, error)
}

func (m *mockWorkflowAdminService) ListTemplates(ctx context.Context) ([]workflow.Template, error) {
	if m.listTemplatesFunc != nil {
		return m.listTemplatesFunc(ctx)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) GetTemplate(ctx context.Context, templateID int64) (*workflow.Template, error) {
	if m.getTemplateFunc != nil {
		return m.getTemplateFunc(ctx, templateID)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) GetTemplateVersions(ctx context.Context, templateID int64) ([]workflow.Definition, error) {
	if m.getTemplateVersionsFunc != nil {
		return m.getTemplateVersionsFunc(ctx, templateID)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) GetDefinition(ctx context.Context, definitionID int64) (*workflow.Definition, error) {
	if m.getDefinitionFunc != nil {
		return m.getDefinitionFunc(ctx, definitionID)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) GetTemplateAudit(ctx context.Context, templateID int64) ([]workflow.DefinitionAuditRecord, error) {
	if m.getTemplateAuditFunc != nil {
		return m.getTemplateAuditFunc(ctx, templateID)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) CreateTemplate(ctx context.Context, input workflow.CreateTemplateInput) (*workflow.TemplateDraft, error) {
	if m.createTemplateFunc != nil {
		return m.createTemplateFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) CreateDraft(ctx context.Context, input workflow.CreateDraftInput) (*workflow.Definition, error) {
	if m.createDraftFunc != nil {
		return m.createDraftFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) UpdateDraftDefinition(ctx context.Context, input workflow.UpdateDraftDefinitionInput) (*workflow.Definition, error) {
	if m.updateDraftDefinitionFunc != nil {
		return m.updateDraftDefinitionFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) ValidateDefinition(ctx context.Context, definitionID int64, actorUserID int64) (*workflow.DefinitionValidationResult, error) {
	if m.validateDefinitionFunc != nil {
		return m.validateDefinitionFunc(ctx, definitionID, actorUserID)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) PublishDefinition(ctx context.Context, input workflow.PublishDefinitionInput) (*workflow.Definition, error) {
	if m.publishDefinitionFunc != nil {
		return m.publishDefinitionFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) DeactivateTemplate(ctx context.Context, input workflow.DeactivateTemplateInput) (*workflow.Template, error) {
	if m.deactivateTemplateFunc != nil {
		return m.deactivateTemplateFunc(ctx, input)
	}
	return nil, nil
}
func (m *mockWorkflowAdminService) RollbackTemplate(ctx context.Context, input workflow.RollbackTemplateInput) (*workflow.Definition, error) {
	if m.rollbackTemplateFunc != nil {
		return m.rollbackTemplateFunc(ctx, input)
	}
	return nil, nil
}

func setWorkflowAdminSessionUser(ctx *gin.Context) {
	ctx.Set("session_user", &auth.SessionUser{ID: 101, Username: "admin", DepartmentID: 101})
}

func TestWorkflowAdminListTemplates(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{listTemplatesFunc: func(ctx context.Context) ([]workflow.Template, error) {
		return []workflow.Template{{ID: 101, Code: "lease-approval", Name: "Lease Approval", ProcessClass: "lease_contract", Status: "active"}}, nil
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/admin/templates", nil)

	handler.ListTemplates(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
	var resp struct {
		Templates []workflow.Template `json:"templates"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if len(resp.Templates) != 1 || resp.Templates[0].Code != "lease-approval" {
		t.Fatalf("unexpected templates: %#v", resp.Templates)
	}
}

func TestWorkflowAdminCreateTemplateRejectsInvalidPayload(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates", bytes.NewBufferString(`{"code":""}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateTemplate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminCreateTemplateRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates", bytes.NewBufferString(`{"business_group_id":101,"code":"lease-approval-v2","name":"Lease Approval V2","process_class":"lease_contract"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")

	handler.CreateTemplate(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminCreateDraftSuccess(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{createDraftFunc: func(ctx context.Context, input workflow.CreateDraftInput) (*workflow.Definition, error) {
		return &workflow.Definition{ID: 201, WorkflowTemplateID: input.TemplateID, VersionNumber: 2, LifecycleStatus: string(workflow.DefinitionLifecycleStatusDraft)}, nil
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "101"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates/101/drafts", bytes.NewBufferString(`{"definition_name":"Lease Approval V2"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")
	setWorkflowAdminSessionUser(ctx)

	handler.CreateDraft(ctx)

	if recorder.Code != http.StatusCreated {
		t.Fatalf("expected 201, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminCreateDraftRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "101"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates/101/drafts", nil)

	handler.CreateDraft(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminPublishDefinitionReturnsValidationDiagnostics(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{publishDefinitionFunc: func(ctx context.Context, input workflow.PublishDefinitionInput) (*workflow.Definition, error) {
		return nil, &workflow.DefinitionValidationError{Result: workflow.DefinitionValidationResult{
			Valid:  false,
			Issues: []workflow.DefinitionValidationIssue{{Code: "missing_nodes", Field: "nodes", Message: "workflow definition must contain at least one node"}},
		}}
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "101"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/definitions/101/publish", nil)
	setWorkflowAdminSessionUser(ctx)

	handler.PublishDefinition(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
	var resp struct {
		Message    string                              `json:"message"`
		Validation workflow.DefinitionValidationResult `json:"validation"`
	}
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if resp.Message != workflow.ErrDefinitionValidationFailed.Error() {
		t.Fatalf("expected validation error message, got %q", resp.Message)
	}
	if resp.Validation.Valid || len(resp.Validation.Issues) != 1 {
		t.Fatalf("expected validation issues, got %#v", resp.Validation)
	}
}

func TestWorkflowAdminPublishDefinitionSuccess(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{publishDefinitionFunc: func(ctx context.Context, input workflow.PublishDefinitionInput) (*workflow.Definition, error) {
		return &workflow.Definition{ID: input.DefinitionID, WorkflowTemplateID: 101, VersionNumber: 2, LifecycleStatus: string(workflow.DefinitionLifecycleStatusPublished)}, nil
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "201"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/definitions/201/publish", nil)
	setWorkflowAdminSessionUser(ctx)

	handler.PublishDefinition(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminPublishDefinitionRejectsMissingSessionUser(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "201"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/definitions/201/publish", nil)

	handler.PublishDefinition(ctx)

	if recorder.Code != http.StatusUnauthorized {
		t.Fatalf("expected 401, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminDeactivateTemplateSuccess(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{deactivateTemplateFunc: func(ctx context.Context, input workflow.DeactivateTemplateInput) (*workflow.Template, error) {
		return &workflow.Template{ID: input.TemplateID, Code: "lease-approval", Name: "Lease Approval", ProcessClass: "lease_contract", Status: "active"}, nil
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "101"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates/101/deactivate", nil)
	setWorkflowAdminSessionUser(ctx)

	handler.DeactivateTemplate(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminValidateDefinitionSuccess(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{validateDefinitionFunc: func(ctx context.Context, definitionID int64, actorUserID int64) (*workflow.DefinitionValidationResult, error) {
		return &workflow.DefinitionValidationResult{Valid: true}, nil
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "301"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/definitions/301/validate", nil)
	setWorkflowAdminSessionUser(ctx)

	handler.ValidateDefinition(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminUpdateDraftDefinitionRejectsInvalidPayload(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(nil)

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "301"}}
	ctx.Request = httptest.NewRequest(http.MethodPut, "/api/workflow/admin/definitions/301", bytes.NewBufferString(`{"name":"Draft without graph"}`))
	ctx.Request.Header.Set("Content-Type", "application/json")
	setWorkflowAdminSessionUser(ctx)

	handler.UpdateDraftDefinition(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminGetTemplateNotFound(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{getTemplateFunc: func(ctx context.Context, templateID int64) (*workflow.Template, error) {
		return nil, workflow.ErrTemplateNotFound
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "999"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/admin/templates/999", nil)

	handler.GetTemplate(ctx)

	if recorder.Code != http.StatusNotFound {
		t.Fatalf("expected 404, got %d body=%s", recorder.Code, recorder.Body.String())
	}
	var resp map[string]any
	if err := json.Unmarshal(recorder.Body.Bytes(), &resp); err != nil {
		t.Fatalf("decode response: %v", err)
	}
	if resp["message"] != workflow.ErrTemplateNotFound.Error() {
		t.Fatalf("expected template not found message, got %#v", resp)
	}
}

func TestWorkflowAdminRollbackTemplateRejectsInvalidBody(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{rollbackTemplateFunc: func(ctx context.Context, input workflow.RollbackTemplateInput) (*workflow.Definition, error) {
		return nil, errors.New("should not be called")
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "101"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates/101/rollback", bytes.NewBufferString(`{"definition_id":0}`))
	ctx.Request.Header.Set("Content-Type", "application/json")
	setWorkflowAdminSessionUser(ctx)

	handler.RollbackTemplate(ctx)

	if recorder.Code != http.StatusBadRequest {
		t.Fatalf("expected 400, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminRollbackTemplateSuccess(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{rollbackTemplateFunc: func(ctx context.Context, input workflow.RollbackTemplateInput) (*workflow.Definition, error) {
		return &workflow.Definition{ID: input.DefinitionID, WorkflowTemplateID: input.TemplateID, LifecycleStatus: string(workflow.DefinitionLifecycleStatusPublished)}, nil
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "101"}}
	ctx.Request = httptest.NewRequest(http.MethodPost, "/api/workflow/admin/templates/101/rollback", bytes.NewBufferString(`{"definition_id":201}`))
	ctx.Request.Header.Set("Content-Type", "application/json")
	setWorkflowAdminSessionUser(ctx)

	handler.RollbackTemplate(ctx)

	if recorder.Code != http.StatusOK {
		t.Fatalf("expected 200, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}

func TestWorkflowAdminGetDefinitionNotFound(t *testing.T) {
	gin.SetMode(gin.TestMode)
	handler := NewWorkflowAdminHandler(&mockWorkflowAdminService{getDefinitionFunc: func(ctx context.Context, definitionID int64) (*workflow.Definition, error) {
		return nil, workflow.ErrDefinitionNotFound
	}})

	recorder := httptest.NewRecorder()
	ctx, _ := gin.CreateTestContext(recorder)
	ctx.Params = gin.Params{{Key: "id", Value: "999"}}
	ctx.Request = httptest.NewRequest(http.MethodGet, "/api/workflow/admin/definitions/999", nil)

	handler.GetDefinition(ctx)

	if recorder.Code != http.StatusNotFound {
		t.Fatalf("expected 404, got %d body=%s", recorder.Code, recorder.Body.String())
	}
}
