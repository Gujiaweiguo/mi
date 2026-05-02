//go:build integration

package workflow_test

import (
	"context"
	"os"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

func TestWorkflowRepositoryAdminPersistence(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)

	baselineTemplate, err := repo.FindTemplateByCode(ctx, string(workflow.TemplateKeyLeaseApproval))
	if err != nil {
		t.Fatalf("find baseline template: %v", err)
	}
	if baselineTemplate == nil {
		t.Fatal("expected baseline template")
	}
	if baselineTemplate.PublishedDefinitionID == nil || *baselineTemplate.PublishedDefinitionID != 101 {
		t.Fatalf("expected baseline published definition 101, got %#v", baselineTemplate)
	}
	if baselineTemplate.PublishedVersionNumber == nil || *baselineTemplate.PublishedVersionNumber != 1 {
		t.Fatalf("expected baseline published version 1, got %#v", baselineTemplate)
	}

	templates, err := repo.ListTemplates(ctx)
	if err != nil {
		t.Fatalf("list templates: %v", err)
	}
	if len(templates) < 4 {
		t.Fatalf("expected seeded templates, got %d", len(templates))
	}

	tx, err := db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin transaction: %v", err)
	}
	defer tx.Rollback()

	createdTemplate, err := repo.CreateTemplate(ctx, tx, workflow.Template{
		ID:              205,
		BusinessGroupID: 102,
		Code:            string(workflow.TemplateKeyInvoiceDiscountApproval),
		Name:            "Invoice Discount Approval",
		ProcessClass:    string(workflow.ObjectTypeInvoiceDiscount),
		Status:          "active",
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}
	if createdTemplate.ID != 205 {
		t.Fatalf("expected created template id 205, got %d", createdTemplate.ID)
	}

	createdDefinition, err := repo.CreateDefinition(ctx, tx, workflow.Definition{
		ID:                 1205,
		BusinessGroupID:    102,
		WorkflowTemplateID: 205,
		Code:               string(workflow.TemplateKeyInvoiceDiscountApproval),
		Name:               "Invoice Discount Approval V1",
		VoucherType:        "application",
		Status:             "active",
		ProcessClass:       string(workflow.ObjectTypeInvoiceDiscount),
		VersionNumber:      1,
		LifecycleStatus:    string(workflow.DefinitionLifecycleStatusDraft),
		TransitionsEnabled: true,
	})
	if err != nil {
		t.Fatalf("create definition: %v", err)
	}
	if createdDefinition.ID != 1205 {
		t.Fatalf("expected created definition id 1205, got %d", createdDefinition.ID)
	}

	createdNode, err := repo.CreateNode(ctx, tx, workflow.Node{
		ID:                   2205,
		WorkflowDefinitionID: 1205,
		FunctionID:           102,
		RoleID:               103,
		StepOrder:            1,
		Code:                 "invoice-discount-finance",
		Name:                 "Invoice Discount Finance Review",
		ProcessClass:         string(workflow.ObjectTypeInvoiceDiscount),
	})
	if err != nil {
		t.Fatalf("create node: %v", err)
	}

	createdTransition, err := repo.CreateTransition(ctx, tx, workflow.Transition{
		ID:                   3205,
		WorkflowDefinitionID: 1205,
		ToNodeID:             2205,
		Action:               workflow.ActionSubmit,
	})
	if err != nil {
		t.Fatalf("create transition: %v", err)
	}
	if createdTransition.ID != 3205 {
		t.Fatalf("expected created transition id 3205, got %d", createdTransition.ID)
	}

	createdRule, err := repo.CreateAssignmentRule(ctx, tx, workflow.AssignmentRule{
		WorkflowNodeID: createdNode.ID,
		StrategyType:   workflow.AssignmentStrategyFixedRole,
		ConfigJSON:     `{"role_id":103}`,
	})
	if err != nil {
		t.Fatalf("create assignment rule: %v", err)
	}
	if createdRule.ID == 0 {
		t.Fatal("expected assignment rule id")
	}

	publishedAt := time.Now().UTC().Truncate(time.Second)
	if err := repo.UpdateDefinitionLifecycleStatus(ctx, tx, createdDefinition.ID, workflow.DefinitionLifecycleStatusPublished, &publishedAt); err != nil {
		t.Fatalf("publish definition: %v", err)
	}
	publishedDefinitionID := createdDefinition.ID
	publishedVersionNumber := createdDefinition.VersionNumber
	if err := repo.UpdateTemplatePublishedVersion(ctx, tx, createdTemplate.ID, &publishedDefinitionID, &publishedVersionNumber); err != nil {
		t.Fatalf("update template publication: %v", err)
	}
	details := `{"action":"publish","version":1}`
	auditRecord, err := repo.InsertDefinitionAudit(ctx, tx, workflow.DefinitionAuditRecord{
		WorkflowTemplateID:   createdTemplate.ID,
		WorkflowDefinitionID: &publishedDefinitionID,
		Action:               workflow.DefinitionAuditActionPublish,
		ActorUserID:          101,
		Details:              &details,
	})
	if err != nil {
		t.Fatalf("insert definition audit: %v", err)
	}
	if auditRecord.ID == 0 {
		t.Fatal("expected audit record id")
	}

	if err := tx.Commit(); err != nil {
		t.Fatalf("commit transaction: %v", err)
	}

	persistedTemplate, err := repo.FindTemplateByID(ctx, createdTemplate.ID)
	if err != nil {
		t.Fatalf("find created template: %v", err)
	}
	if persistedTemplate == nil {
		t.Fatal("expected persisted template")
	}
	if persistedTemplate.PublishedDefinitionID == nil || *persistedTemplate.PublishedDefinitionID != publishedDefinitionID {
		t.Fatalf("expected template publication to reference definition %d, got %#v", publishedDefinitionID, persistedTemplate)
	}
	if persistedTemplate.PublishedVersionNumber == nil || *persistedTemplate.PublishedVersionNumber != publishedVersionNumber {
		t.Fatalf("expected template publication version %d, got %#v", publishedVersionNumber, persistedTemplate)
	}

	versions, err := repo.ListDefinitionVersions(ctx, createdTemplate.ID)
	if err != nil {
		t.Fatalf("list definition versions: %v", err)
	}
	if len(versions) != 1 {
		t.Fatalf("expected 1 definition version, got %d", len(versions))
	}
	if versions[0].LifecycleStatus != string(workflow.DefinitionLifecycleStatusPublished) {
		t.Fatalf("expected published definition version, got %#v", versions[0])
	}
	if versions[0].PublishedAt == nil {
		t.Fatalf("expected published_at timestamp, got %#v", versions[0])
	}

	publishedDefinition, err := repo.FindPublishedDefinitionByTemplate(ctx, createdTemplate.ID)
	if err != nil {
		t.Fatalf("find published definition by template: %v", err)
	}
	if publishedDefinition == nil || publishedDefinition.ID != createdDefinition.ID {
		t.Fatalf("expected published definition %d, got %#v", createdDefinition.ID, publishedDefinition)
	}

	nodes, err := repo.ListNodesByDefinition(ctx, createdDefinition.ID)
	if err != nil {
		t.Fatalf("list nodes by definition: %v", err)
	}
	if len(nodes) != 1 || nodes[0].ID != createdNode.ID {
		t.Fatalf("expected created node %d, got %#v", createdNode.ID, nodes)
	}

	transitions, err := repo.ListTransitionsByDefinition(ctx, createdDefinition.ID)
	if err != nil {
		t.Fatalf("list transitions by definition: %v", err)
	}
	if len(transitions) != 1 || transitions[0].ID != createdTransition.ID {
		t.Fatalf("expected created transition %d, got %#v", createdTransition.ID, transitions)
	}

	rules, err := repo.ListAssignmentRulesByNode(ctx, createdNode.ID)
	if err != nil {
		t.Fatalf("list assignment rules: %v", err)
	}
	if len(rules) != 1 || rules[0].StrategyType != workflow.AssignmentStrategyFixedRole {
		t.Fatalf("expected fixed-role assignment rule, got %#v", rules)
	}

	auditTrail, err := repo.ListDefinitionAudit(ctx, createdTemplate.ID)
	if err != nil {
		t.Fatalf("list definition audit: %v", err)
	}
	if len(auditTrail) != 1 || auditTrail[0].Action != workflow.DefinitionAuditActionPublish {
		t.Fatalf("expected publish audit trail, got %#v", auditTrail)
	}

	tx, err = db.BeginTx(ctx, nil)
	if err != nil {
		t.Fatalf("begin delete transaction: %v", err)
	}
	defer tx.Rollback()
	if err := repo.DeleteAssignmentRulesByNode(ctx, tx, createdNode.ID); err != nil {
		t.Fatalf("delete assignment rules by node: %v", err)
	}
	if err := tx.Commit(); err != nil {
		t.Fatalf("commit delete transaction: %v", err)
	}

	rules, err = repo.ListAssignmentRulesByNode(ctx, createdNode.ID)
	if err != nil {
		t.Fatalf("list assignment rules after delete: %v", err)
	}
	if len(rules) != 0 {
		t.Fatalf("expected assignment rules to be deleted, got %#v", rules)
	}
}
