//go:build integration

package workflow_test

import (
	"context"
	"errors"
	"os"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

func TestWorkflowAdminServiceLifecycle(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)
	runtimeService := workflow.NewService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 102,
		Code:            string(workflow.TemplateKeyInvoiceDiscountApproval),
		Name:            "Invoice Discount Approval",
		ProcessClass:    string(workflow.ObjectTypeInvoiceDiscount),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create workflow template: %v", err)
	}
	if created.Template == nil || created.Definition == nil {
		t.Fatalf("expected template draft result, got %#v", created)
	}
	if created.Definition.LifecycleStatus != string(workflow.DefinitionLifecycleStatusDraft) {
		t.Fatalf("expected draft definition, got %#v", created.Definition)
	}

	updated, err := adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "Invoice Discount Approval V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{
				FunctionID:   102,
				RoleID:       103,
				StepOrder:    1,
				Code:         "discount-finance",
				Name:         "Discount Finance Review",
				ProcessClass: string(workflow.ObjectTypeInvoiceDiscount),
				AssignmentRules: []workflow.AssignmentRuleInput{
					{StrategyType: workflow.AssignmentStrategyFixedRole, ConfigJSON: `{"role_id":103}`},
				},
			},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "discount-finance", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft definition: %v", err)
	}
	if len(updated.Nodes) != 1 || len(updated.Nodes[0].AssignmentRules) != 1 {
		t.Fatalf("expected updated draft graph, got %#v", updated)
	}

	validation, err := adminService.ValidateDefinition(ctx, created.Definition.ID, 101)
	if err != nil {
		t.Fatalf("validate definition: %v", err)
	}
	if !validation.Valid {
		t.Fatalf("expected valid definition, got %#v", validation)
	}

	published, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish definition: %v", err)
	}
	if published.LifecycleStatus != string(workflow.DefinitionLifecycleStatusPublished) {
		t.Fatalf("expected published definition, got %#v", published)
	}

	template, err := adminService.GetTemplate(ctx, created.Template.ID)
	if err != nil {
		t.Fatalf("get template after publish: %v", err)
	}
	if template.PublishedDefinitionID == nil || *template.PublishedDefinitionID != published.ID {
		t.Fatalf("expected template to point to published definition %d, got %#v", published.ID, template)
	}

	instance, err := runtimeService.Start(ctx, workflow.StartInput{
		DefinitionCode: string(workflow.TemplateKeyInvoiceDiscountApproval),
		DocumentType:   string(workflow.ObjectTypeInvoiceDiscount),
		DocumentID:     9701,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9701",
		Comment:        "submit discount approval",
	})
	if err != nil {
		t.Fatalf("start published workflow: %v", err)
	}
	if instance.WorkflowTemplateID != created.Template.ID || instance.WorkflowDefinitionID != published.ID {
		t.Fatalf("expected instance binding to new template/definition, got %#v", instance)
	}

	if _, err := adminService.DeactivateTemplate(ctx, workflow.DeactivateTemplateInput{TemplateID: created.Template.ID, ActorUserID: 101}); err != nil {
		t.Fatalf("deactivate template: %v", err)
	}
	if _, err := runtimeService.Start(ctx, workflow.StartInput{
		DefinitionCode: string(workflow.TemplateKeyInvoiceDiscountApproval),
		DocumentType:   string(workflow.ObjectTypeInvoiceDiscount),
		DocumentID:     9702,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9702",
		Comment:        "submit discount approval after deactivate",
	}); !errors.Is(err, workflow.ErrDefinitionNotFound) {
		t.Fatalf("expected definition not found after deactivation, got %v", err)
	}

	rolledBack, err := adminService.RollbackTemplate(ctx, workflow.RollbackTemplateInput{TemplateID: created.Template.ID, DefinitionID: published.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("rollback template: %v", err)
	}
	if rolledBack.LifecycleStatus != string(workflow.DefinitionLifecycleStatusPublished) {
		t.Fatalf("expected rolled back definition to be published, got %#v", rolledBack)
	}

	instance, err = runtimeService.Start(ctx, workflow.StartInput{
		DefinitionCode: string(workflow.TemplateKeyInvoiceDiscountApproval),
		DocumentType:   string(workflow.ObjectTypeInvoiceDiscount),
		DocumentID:     9703,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9703",
		Comment:        "submit discount approval after rollback",
	})
	if err != nil {
		t.Fatalf("start rolled-back workflow: %v", err)
	}
	if instance.WorkflowDefinitionID != published.ID {
		t.Fatalf("expected rollback to restore definition %d, got %#v", published.ID, instance)
	}

	auditTrail, err := adminService.GetTemplateAudit(ctx, created.Template.ID)
	if err != nil {
		t.Fatalf("get template audit: %v", err)
	}
	if len(auditTrail) < 5 {
		t.Fatalf("expected workflow definition audit trail, got %#v", auditTrail)
	}
}

func TestWorkflowAdminServicePublishRejectsInvalidDraft(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "lease-approval-invalid",
		Name:            "Lease Approval Invalid",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create workflow template: %v", err)
	}

	broken, err := adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "Lease Approval Invalid V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{
				FunctionID:   101,
				RoleID:       102,
				StepOrder:    1,
				Code:         "entry",
				Name:         "Entry Review",
				ProcessClass: string(workflow.ObjectTypeLeaseContract),
			},
			{
				FunctionID:   101,
				RoleID:       103,
				StepOrder:    2,
				Code:         "orphan",
				Name:         "Orphan Review",
				ProcessClass: string(workflow.ObjectTypeLeaseContract),
				AssignmentRules: []workflow.AssignmentRuleInput{{
					StrategyType: workflow.AssignmentStrategyDocumentField,
					ConfigJSON:   `{"field_path":"unsupported_field"}`,
				}},
			},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry", Action: workflow.ActionSubmit}, {FromNodeCode: stringPointer("entry"), ToNodeCode: "entry", Action: workflow.ActionApprove}},
	})
	if err != nil {
		t.Fatalf("update broken draft definition: %v", err)
	}

	validation, err := adminService.ValidateDefinition(ctx, broken.ID, 101)
	if err != nil {
		t.Fatalf("validate broken definition: %v", err)
	}
	if validation.Valid {
		t.Fatalf("expected invalid validation result, got %#v", validation)
	}
	if !hasIssueCode(validation.Issues, "unreachable_node") || !hasIssueCode(validation.Issues, "unsupported_field_reference") || !hasIssueCode(validation.Issues, "missing_terminal_outcome") {
		t.Fatalf("expected multiple validation issue codes, got %#v", validation.Issues)
	}

	_, err = adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: broken.ID, ActorUserID: 101})
	if err == nil {
		t.Fatal("expected publish validation failure")
	}
	var validationErr *workflow.DefinitionValidationError
	if !errors.As(err, &validationErr) {
		t.Fatalf("expected DefinitionValidationError, got %v", err)
	}
	persisted, err := adminService.GetDefinition(ctx, broken.ID)
	if err != nil {
		t.Fatalf("reload draft after failed publish: %v", err)
	}
	if persisted.LifecycleStatus != string(workflow.DefinitionLifecycleStatusDraft) {
		t.Fatalf("expected draft lifecycle after failed publish, got %#v", persisted)
	}
}

func stringPointer(value string) *string {
	return &value
}

func hasIssueCode(issues []workflow.DefinitionValidationIssue, code string) bool {
	for _, issue := range issues {
		if issue.Code == code {
			return true
		}
	}
	return false
}

func TestWorkflowAdminValidationTransitionsToValidated(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 103,
		Code:            "validated-lifecycle-test",
		Name:            "Validated Lifecycle Test",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}

	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "Validated Lifecycle V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 102, StepOrder: 1, Code: "entry", Name: "Entry", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft definition: %v", err)
	}

	validation, err := adminService.ValidateDefinition(ctx, created.Definition.ID, 101)
	if err != nil {
		t.Fatalf("validate definition: %v", err)
	}
	if !validation.Valid {
		t.Fatalf("expected valid definition, got %#v", validation)
	}

	persisted, err := adminService.GetDefinition(ctx, created.Definition.ID)
	if err != nil {
		t.Fatalf("reload after validate: %v", err)
	}
	if persisted.LifecycleStatus != string(workflow.DefinitionLifecycleStatusValidated) {
		t.Fatalf("expected validated lifecycle, got %s", persisted.LifecycleStatus)
	}
}

func TestWorkflowAdminPublishFromValidated(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "publish-from-validated",
		Name:            "Publish From Validated",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}

	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "Publish From Validated V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 102, StepOrder: 1, Code: "entry", Name: "Entry", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft: %v", err)
	}

	if _, err := adminService.ValidateDefinition(ctx, created.Definition.ID, 101); err != nil {
		t.Fatalf("validate: %v", err)
	}

	published, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish from validated: %v", err)
	}
	if published.LifecycleStatus != string(workflow.DefinitionLifecycleStatusPublished) {
		t.Fatalf("expected published, got %s", published.LifecycleStatus)
	}
}

func TestWorkflowAdminPublishRejectsSuperseded(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "reject-superseded-publish",
		Name:            "Reject Superseded Publish",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}

	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 102, StepOrder: 1, Code: "entry", Name: "Entry", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft v1: %v", err)
	}

	v1, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish v1: %v", err)
	}

	draft, err := adminService.CreateDraft(ctx, workflow.CreateDraftInput{TemplateID: created.Template.ID, ActorUserID: 101, VoucherType: "application"})
	if err != nil {
		t.Fatalf("create v2 draft: %v", err)
	}
	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: draft.ID,
		ActorUserID:  101,
		Name:         "V2",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 103, StepOrder: 1, Code: "entry2", Name: "Entry 2", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry2", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft v2: %v", err)
	}
	_, err = adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: draft.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish v2 (supersedes v1): %v", err)
	}

	_, err = adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: v1.ID, ActorUserID: 101})
	if err == nil {
		t.Fatal("expected error when publishing superseded definition")
	}
	if !errors.Is(err, workflow.ErrInvalidState) {
		t.Fatalf("expected ErrInvalidState, got %v", err)
	}
}

func TestWorkflowAdminRollbackRejectsDraft(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "rollback-rejects-draft",
		Name:            "Rollback Rejects Draft",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}

	_, err = adminService.RollbackTemplate(ctx, workflow.RollbackTemplateInput{
		TemplateID:   created.Template.ID,
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
	})
	if err == nil {
		t.Fatal("expected error when rolling back to draft")
	}
	if !errors.Is(err, workflow.ErrInvalidState) {
		t.Fatalf("expected ErrInvalidState, got %v", err)
	}
}

func TestWorkflowAdminRollbackRejectsCurrentlyPublished(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "rollback-rejects-published",
		Name:            "Rollback Rejects Published",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}

	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 102, StepOrder: 1, Code: "entry", Name: "Entry", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft: %v", err)
	}

	published, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish: %v", err)
	}

	_, err = adminService.RollbackTemplate(ctx, workflow.RollbackTemplateInput{
		TemplateID:   created.Template.ID,
		DefinitionID: published.ID,
		ActorUserID:  101,
	})
	if err == nil {
		t.Fatal("expected error when rolling back to currently published version")
	}
	if !errors.Is(err, workflow.ErrInvalidState) {
		t.Fatalf("expected ErrInvalidState, got %v", err)
	}
}

func TestWorkflowAdminMultiVersionPublishSupersedeRollback(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)
	runtimeService := workflow.NewService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "multi-version-supersede",
		Name:            "Multi Version Supersede",
		ProcessClass:    string(workflow.ObjectTypeLeaseContract),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}

	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 102, StepOrder: 1, Code: "entry", Name: "Entry", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft v1: %v", err)
	}

	v1, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish v1: %v", err)
	}

	instance1, err := runtimeService.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeLeaseContract),
		DocumentID:     9801,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9801",
		Comment:        "start on v1",
	})
	if err != nil {
		t.Fatalf("start instance on v1: %v", err)
	}
	if instance1.WorkflowDefinitionID != v1.ID {
		t.Fatalf("expected instance bound to v1 (%d), got %d", v1.ID, instance1.WorkflowDefinitionID)
	}

	draft, err := adminService.CreateDraft(ctx, workflow.CreateDraftInput{TemplateID: created.Template.ID, ActorUserID: 101, VoucherType: "application"})
	if err != nil {
		t.Fatalf("create v2 draft: %v", err)
	}
	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: draft.ID,
		ActorUserID:  101,
		Name:         "V2",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{
			{FunctionID: 101, RoleID: 103, StepOrder: 1, Code: "entry2", Name: "Entry 2", ProcessClass: string(workflow.ObjectTypeLeaseContract)},
		},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "entry2", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft v2: %v", err)
	}
	v2, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: draft.ID, ActorUserID: 101})
	if err != nil {
		t.Fatalf("publish v2: %v", err)
	}

	instance2, err := runtimeService.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeLeaseContract),
		DocumentID:     9802,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9802",
		Comment:        "start on v2",
	})
	if err != nil {
		t.Fatalf("start instance on v2: %v", err)
	}
	if instance2.WorkflowDefinitionID != v2.ID {
		t.Fatalf("expected instance bound to v2 (%d), got %d", v2.ID, instance2.WorkflowDefinitionID)
	}

	v1Reloaded, err := adminService.GetDefinition(ctx, v1.ID)
	if err != nil {
		t.Fatalf("reload v1: %v", err)
	}
	if v1Reloaded.LifecycleStatus != string(workflow.DefinitionLifecycleStatusSuperseded) {
		t.Fatalf("expected v1 superseded, got %s", v1Reloaded.LifecycleStatus)
	}

	rolledBack, err := adminService.RollbackTemplate(ctx, workflow.RollbackTemplateInput{
		TemplateID:   created.Template.ID,
		DefinitionID: v1.ID,
		ActorUserID:  101,
	})
	if err != nil {
		t.Fatalf("rollback to v1: %v", err)
	}
	if rolledBack.LifecycleStatus != string(workflow.DefinitionLifecycleStatusPublished) {
		t.Fatalf("expected rollback published, got %s", rolledBack.LifecycleStatus)
	}
}
