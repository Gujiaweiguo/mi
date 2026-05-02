//go:build integration

package workflow_test

import (
	"context"
	"database/sql"
	"os"
	"testing"
	"time"

	platformdb "github.com/Gujiaweiguo/mi/backend/internal/platform/database"
	"github.com/Gujiaweiguo/mi/backend/internal/workflow"
	_ "github.com/go-sql-driver/mysql"
)

func TestWorkflowServiceApproveAndDeduplicate(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := workflow.NewService(db, workflow.NewRepository(db))

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9001,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9001",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	if instance.Status != workflow.InstanceStatusPending || instance.CurrentStepOrder == nil || *instance.CurrentStepOrder != 1 {
		t.Fatalf("expected pending instance at step 1, got %#v", instance)
	}
	if instance.WorkflowTemplateID != 101 || instance.WorkflowDefinitionVersion != 1 {
		t.Fatalf("expected template/version binding 101/v1, got template=%d version=%d", instance.WorkflowTemplateID, instance.WorkflowDefinitionVersion)
	}

	instance, err = service.Approve(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-step-1", Comment: "manager approved"})
	if err != nil {
		t.Fatalf("approve first step: %v", err)
	}
	if instance.CurrentStepOrder == nil || *instance.CurrentStepOrder != 2 || instance.Status != workflow.InstanceStatusPending {
		t.Fatalf("expected pending instance at step 2, got %#v", instance)
	}

	duplicateInstance, err := service.Approve(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-step-1", Comment: "manager approved duplicate"})
	if err != nil {
		t.Fatalf("duplicate approve should be safe: %v", err)
	}
	if duplicateInstance.Version != instance.Version {
		t.Fatalf("expected duplicate approve to preserve version %d, got %d", instance.Version, duplicateInstance.Version)
	}

	history, err := service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history: %v", err)
	}
	if len(history) != 2 {
		t.Fatalf("expected 2 audit entries after duplicate approve, got %d", len(history))
	}

	outbox, err := service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox messages: %v", err)
	}
	if len(outbox) != 2 {
		t.Fatalf("expected 2 outbox messages after duplicate approve, got %d", len(outbox))
	}

	instance, err = service.Approve(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-step-2", Comment: "finance approved"})
	if err != nil {
		t.Fatalf("approve final step: %v", err)
	}
	if instance.Status != workflow.InstanceStatusApproved || instance.CompletedAt == nil {
		t.Fatalf("expected approved completed instance, got %#v", instance)
	}

	history, err = service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history after completion: %v", err)
	}
	if len(history) != 3 {
		t.Fatalf("expected 3 audit entries after completion, got %d", len(history))
	}

	outbox, err = service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox messages after completion: %v", err)
	}
	if len(outbox) != 3 {
		t.Fatalf("expected 3 outbox messages after completion, got %d", len(outbox))
	}
}

func TestWorkflowServiceStartDeduplicatesByIdempotencyKey(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := workflow.NewService(db, workflow.NewRepository(db))

	first, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9050,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9050",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}

	duplicate, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9050,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9050",
		Comment:        "submit lease approval duplicate",
	})
	if err != nil {
		t.Fatalf("duplicate start should be safe: %v", err)
	}
	if duplicate.ID != first.ID {
		t.Fatalf("expected duplicate start to return instance %d, got %d", first.ID, duplicate.ID)
	}
	if duplicate.Version != first.Version {
		t.Fatalf("expected duplicate start to preserve version %d, got %d", first.Version, duplicate.Version)
	}
	if duplicate.WorkflowTemplateID != first.WorkflowTemplateID || duplicate.WorkflowDefinitionVersion != first.WorkflowDefinitionVersion {
		t.Fatalf("expected duplicate start to preserve template/version binding %#v vs %#v", duplicate, first)
	}

	instances, err := service.ListInstances(ctx, workflow.InstanceFilter{})
	if err != nil {
		t.Fatalf("list workflow instances: %v", err)
	}
	if len(instances) != 1 {
		t.Fatalf("expected exactly 1 workflow instance after duplicate start, got %d", len(instances))
	}

	history, err := service.AuditHistory(ctx, first.ID)
	if err != nil {
		t.Fatalf("audit history: %v", err)
	}
	if len(history) != 1 {
		t.Fatalf("expected 1 audit entry after duplicate start, got %d", len(history))
	}

	outbox, err := service.OutboxMessages(ctx, first.ID)
	if err != nil {
		t.Fatalf("outbox messages: %v", err)
	}
	if len(outbox) != 1 {
		t.Fatalf("expected 1 outbox message after duplicate start, got %d", len(outbox))
	}
}

func TestWorkflowServicePublishedVersionBindingPreservesInFlightInstances(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	service := workflow.NewService(db, repo)
	adminService := workflow.NewAdminService(db, repo)

	legacyInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9901,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9901",
		Comment:        "submit lease approval v1",
	})
	if err != nil {
		t.Fatalf("start legacy workflow: %v", err)
	}
	if legacyInstance.WorkflowDefinitionID != 101 || legacyInstance.WorkflowTemplateID != 101 || legacyInstance.WorkflowDefinitionVersion != 1 {
		t.Fatalf("expected legacy instance bound to template 101 definition 101 version 1, got %#v", legacyInstance)
	}

	_, err = db.ExecContext(ctx, `
		INSERT INTO workflow_definitions (id, business_group_id, workflow_template_id, code, version_number, name, voucher_type, is_initial, status, lifecycle_status, published_at, transitions_enabled, process_class)
		VALUES (1001, 101, 101, 'lease-approval', 2, 'Lease Approval V2', 'application', FALSE, 'active', 'published', CURRENT_TIMESTAMP, TRUE, 'lease_contract')
	`)
	if err != nil {
		t.Fatalf("insert workflow definition v2: %v", err)
	}
	_, err = db.ExecContext(ctx, `
		INSERT INTO workflow_nodes (id, workflow_definition_id, function_id, role_id, step_order, code, name, can_submit_to_manager, validates_after_confirm, prints_after_confirm, process_class)
		VALUES (1001, 1001, 101, 102, 1, 'lease-manager-v2', 'Lease Manager Review V2', TRUE, FALSE, FALSE, 'lease_contract')
	`)
	if err != nil {
		t.Fatalf("insert workflow node v2: %v", err)
	}
	_, err = db.ExecContext(ctx, `
		INSERT INTO workflow_transitions (id, workflow_definition_id, from_node_id, to_node_id, action)
		VALUES (1001, 1001, NULL, 1001, 'submit')
	`)
	if err != nil {
		t.Fatalf("insert workflow transition v2: %v", err)
	}
	_, err = db.ExecContext(ctx, `
		UPDATE workflow_definitions
		SET lifecycle_status = CASE WHEN id = 1001 THEN 'published' ELSE 'superseded' END
		WHERE workflow_template_id = 101
	`)
	if err != nil {
		t.Fatalf("publish v2 and supersede v1: %v", err)
	}
	_, err = db.ExecContext(ctx, `
		UPDATE workflow_templates
		SET published_definition_id = 1001, published_version_number = 2
		WHERE id = 101
	`)
	if err != nil {
		t.Fatalf("update workflow template publication pointers to v2: %v", err)
	}

	newInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9902,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9902",
		Comment:        "submit lease approval v2",
	})
	if err != nil {
		t.Fatalf("start v2 workflow: %v", err)
	}
	if newInstance.WorkflowDefinitionID != 1001 || newInstance.WorkflowTemplateID != 101 || newInstance.WorkflowDefinitionVersion != 2 {
		t.Fatalf("expected new instance bound to definition 1001 version 2, got %#v", newInstance)
	}

	legacyInstance, err = service.Approve(ctx, workflow.TransitionInput{InstanceID: legacyInstance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-9901-step1", Comment: "approve legacy v1"})
	if err != nil {
		t.Fatalf("approve legacy instance after publishing v2: %v", err)
	}
	if legacyInstance.WorkflowDefinitionID != 101 || legacyInstance.WorkflowDefinitionVersion != 1 {
		t.Fatalf("expected legacy instance to preserve v1 binding, got %#v", legacyInstance)
	}
	if legacyInstance.Status != workflow.InstanceStatusPending || legacyInstance.CurrentStepOrder == nil || *legacyInstance.CurrentStepOrder != 2 {
		t.Fatalf("expected legacy instance to continue on original v1 second step, got %#v", legacyInstance)
	}

	rolledBack, err := adminService.RollbackTemplate(ctx, workflow.RollbackTemplateInput{
		TemplateID:   101,
		DefinitionID: 101,
		ActorUserID:  101,
	})
	if err != nil {
		t.Fatalf("rollback to v1: %v", err)
	}
	if rolledBack.ID != 101 || rolledBack.VersionNumber != 1 || rolledBack.LifecycleStatus != string(workflow.DefinitionLifecycleStatusPublished) {
		t.Fatalf("expected v1 rollback to publish definition 101/version 1, got %#v", rolledBack)
	}

	rollbackInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9903,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9903",
		Comment:        "submit lease approval after rollback",
	})
	if err != nil {
		t.Fatalf("start workflow after rollback: %v", err)
	}
	if rollbackInstance.WorkflowDefinitionID != 101 || rollbackInstance.WorkflowTemplateID != 101 || rollbackInstance.WorkflowDefinitionVersion != 1 {
		t.Fatalf("expected rollback instance bound back to definition 101 version 1, got %#v", rollbackInstance)
	}

	newInstanceAfterRollback, err := service.GetInstance(ctx, newInstance.ID)
	if err != nil {
		t.Fatalf("reload v2 instance after rollback: %v", err)
	}
	if newInstanceAfterRollback.WorkflowDefinitionID != 1001 || newInstanceAfterRollback.WorkflowDefinitionVersion != 2 {
		t.Fatalf("expected in-flight v2 instance to preserve v2 binding after rollback, got %#v", newInstanceAfterRollback)
	}

	newInstance, err = service.Approve(ctx, workflow.TransitionInput{InstanceID: newInstance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "approve-9902-step1", Comment: "approve new v2 after rollback"})
	if err != nil {
		t.Fatalf("approve v2 instance after rollback: %v", err)
	}
	if newInstance.WorkflowDefinitionID != 1001 || newInstance.WorkflowDefinitionVersion != 2 {
		t.Fatalf("expected v2 instance to preserve v2 binding through approval after rollback, got %#v", newInstance)
	}
	if newInstance.Status != workflow.InstanceStatusApproved || newInstance.CompletedAt == nil {
		t.Fatalf("expected v2 instance to complete after single-step approval, got %#v", newInstance)
	}

	duplicateActive, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9901,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9901-late",
		Comment:        "attempt duplicate active start after version change",
	})
	if err != nil {
		t.Fatalf("duplicate active start across versions: %v", err)
	}
	if duplicateActive.ID != legacyInstance.ID {
		t.Fatalf("expected active instance dedupe at template level, got instance %d vs %d", duplicateActive.ID, legacyInstance.ID)
	}
}

func TestWorkflowServiceRejectAndResubmit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := workflow.NewService(db, workflow.NewRepository(db))

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "invoice-approval",
		DocumentType:   "invoice",
		DocumentID:     9101,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9101",
		Comment:        "submit invoice approval",
	})
	if err != nil {
		t.Fatalf("start invoice workflow: %v", err)
	}

	instance, err = service.Reject(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reject-9101", Comment: "reject invoice"})
	if err != nil {
		t.Fatalf("reject workflow: %v", err)
	}
	if instance.Status != workflow.InstanceStatusRejected {
		t.Fatalf("expected rejected instance, got %#v", instance)
	}

	instance, err = service.Resubmit(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "resubmit-9101", Comment: "resubmit invoice"})
	if err != nil {
		t.Fatalf("resubmit workflow: %v", err)
	}
	if instance.Status != workflow.InstanceStatusPending || instance.CurrentCycle != 2 || instance.CurrentStepOrder == nil || *instance.CurrentStepOrder != 1 {
		t.Fatalf("expected pending resubmitted instance at cycle 2 step 1, got %#v", instance)
	}

	history, err := service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history after resubmit: %v", err)
	}
	if len(history) != 3 {
		t.Fatalf("expected 3 audit entries after reject/resubmit, got %d", len(history))
	}

	outbox, err := service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox messages after resubmit: %v", err)
	}
	if len(outbox) != 3 {
		t.Fatalf("expected 3 outbox messages after reject/resubmit, got %d", len(outbox))
	}
}

func TestWorkflowServiceListInstancesFilters(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	service := workflow.NewService(db, workflow.NewRepository(db))

	leaseInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9201,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9201",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start lease workflow: %v", err)
	}

	invoiceInstance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "invoice-approval",
		DocumentType:   "invoice",
		DocumentID:     9202,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9202",
		Comment:        "submit invoice approval",
	})
	if err != nil {
		t.Fatalf("start invoice workflow: %v", err)
	}

	invoiceInstance, err = service.Reject(ctx, workflow.TransitionInput{InstanceID: invoiceInstance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "reject-9202", Comment: "reject invoice"})
	if err != nil {
		t.Fatalf("reject invoice workflow: %v", err)
	}

	status := workflow.InstanceStatusRejected
	rejectedInstances, err := service.ListInstances(ctx, workflow.InstanceFilter{Status: &status})
	if err != nil {
		t.Fatalf("list rejected workflow instances: %v", err)
	}
	if len(rejectedInstances) != 1 || rejectedInstances[0].ID != invoiceInstance.ID {
		t.Fatalf("expected only rejected invoice instance, got %#v", rejectedInstances)
	}

	documentType := "lease_contract"
	leaseInstances, err := service.ListInstances(ctx, workflow.InstanceFilter{DocumentType: &documentType})
	if err != nil {
		t.Fatalf("list lease workflow instances: %v", err)
	}
	if len(leaseInstances) != 1 || leaseInstances[0].ID != leaseInstance.ID {
		t.Fatalf("expected only lease instance, got %#v", leaseInstances)
	}

	allInstances, err := service.ListInstances(ctx, workflow.InstanceFilter{})
	if err != nil {
		t.Fatalf("list all workflow instances: %v", err)
	}
	if len(allInstances) != 2 {
		t.Fatalf("expected 2 workflow instances, got %d", len(allInstances))
	}
	if allInstances[0].ID != invoiceInstance.ID || allInstances[1].ID != leaseInstance.ID {
		t.Fatalf("expected newest-first ordering, got %#v", allInstances)
	}
	if allInstances[0].DocumentType != "invoice" || allInstances[1].DocumentType != "lease_contract" {
		t.Fatalf("expected stable instance ordering and payloads, got %#v", allInstances)
	}
}

func TestWorkflowReminderRunEmitsAndStaysReadOnly(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	baseTime := time.Date(2026, 4, 10, 9, 0, 0, 0, time.UTC)
	repo := workflow.NewRepositoryWithNowFunc(db, func() time.Time { return baseTime })
	service := workflow.NewService(db, repo)

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "lease-approval",
		DocumentType:   "lease_contract",
		DocumentID:     9301,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9301",
		Comment:        "submit lease approval",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	beforeVersion := instance.Version

	auditBefore, err := service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history before reminders: %v", err)
	}
	outboxBefore, err := service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox before reminders: %v", err)
	}

	runAt := baseTime.Add(25 * time.Hour)
	records, err := service.RunReminders(ctx, runAt, workflow.ReminderConfig{
		ReminderType:     "standard",
		MinPendingAge:    24 * time.Hour,
		WindowTruncation: 24 * time.Hour,
	})
	if err != nil {
		t.Fatalf("run reminders: %v", err)
	}
	if len(records) != 1 {
		t.Fatalf("expected 1 reminder record, got %d", len(records))
	}
	if records[0].Outcome != workflow.ReminderOutcomeEmitted || records[0].ReasonCode != nil {
		t.Fatalf("expected emitted reminder record, got %#v", records[0])
	}

	instanceAfter, err := service.GetInstance(ctx, instance.ID)
	if err != nil {
		t.Fatalf("load instance after reminders: %v", err)
	}
	if instanceAfter.Status != workflow.InstanceStatusPending || instanceAfter.Version != beforeVersion || instanceAfter.CurrentStepOrder == nil || *instanceAfter.CurrentStepOrder != 1 {
		t.Fatalf("expected reminder run to preserve workflow state, got %#v", instanceAfter)
	}

	auditAfter, err := service.AuditHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("audit history after reminders: %v", err)
	}
	outboxAfter, err := service.OutboxMessages(ctx, instance.ID)
	if err != nil {
		t.Fatalf("outbox after reminders: %v", err)
	}
	if len(auditAfter) != len(auditBefore) || len(outboxAfter) != len(outboxBefore) {
		t.Fatalf("expected reminder run to avoid workflow audit/outbox mutation, audit %d->%d outbox %d->%d", len(auditBefore), len(auditAfter), len(outboxBefore), len(outboxAfter))
	}

	history, err := service.ReminderHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("reminder history after first run: %v", err)
	}
	if len(history) != 1 || history[0].Outcome != workflow.ReminderOutcomeEmitted {
		t.Fatalf("expected single emitted reminder history record, got %#v", history)
	}

	replayRecords, err := service.RunReminders(ctx, runAt, workflow.ReminderConfig{
		ReminderType:     "standard",
		MinPendingAge:    24 * time.Hour,
		WindowTruncation: 24 * time.Hour,
	})
	if err != nil {
		t.Fatalf("replay reminders: %v", err)
	}
	if len(replayRecords) != 1 || replayRecords[0].Outcome != workflow.ReminderOutcomeSkipped || replayRecords[0].ReasonCode == nil || *replayRecords[0].ReasonCode != string(workflow.ReminderReasonAlreadyEmitted) {
		t.Fatalf("expected already_emitted replay record, got %#v", replayRecords)
	}

	thirdRecords, err := service.RunReminders(ctx, runAt, workflow.ReminderConfig{
		ReminderType:     "standard",
		MinPendingAge:    24 * time.Hour,
		WindowTruncation: 24 * time.Hour,
	})
	if err != nil {
		t.Fatalf("third reminder replay: %v", err)
	}
	if len(thirdRecords) != 1 || thirdRecords[0].Outcome != workflow.ReminderOutcomeSkipped {
		t.Fatalf("expected skipped replay on third run, got %#v", thirdRecords)
	}

	history, err = service.ReminderHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("reminder history after replays: %v", err)
	}
	if len(history) != 2 {
		t.Fatalf("expected emitted + single replay skip records, got %#v", history)
	}
	if history[1].ReasonCode == nil || *history[1].ReasonCode != string(workflow.ReminderReasonAlreadyEmitted) {
		t.Fatalf("expected second reminder history row to be already_emitted, got %#v", history[1])
	}
}

func TestWorkflowReminderRunSkipsWhenNotDue(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	baseTime := time.Date(2026, 4, 10, 9, 0, 0, 0, time.UTC)
	repo := workflow.NewRepositoryWithNowFunc(db, func() time.Time { return baseTime })
	service := workflow.NewService(db, repo)

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: "invoice-approval",
		DocumentType:   "invoice",
		DocumentID:     9302,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9302",
		Comment:        "submit invoice approval",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}

	records, err := service.RunReminders(ctx, baseTime.Add(2*time.Hour), workflow.ReminderConfig{
		ReminderType:     "standard",
		MinPendingAge:    24 * time.Hour,
		WindowTruncation: 24 * time.Hour,
	})
	if err != nil {
		t.Fatalf("run reminders before due: %v", err)
	}
	if len(records) != 1 || records[0].Outcome != workflow.ReminderOutcomeSkipped || records[0].ReasonCode == nil || *records[0].ReasonCode != string(workflow.ReminderReasonNotDue) {
		t.Fatalf("expected not_due reminder record, got %#v", records)
	}

	history, err := service.ReminderHistory(ctx, instance.ID)
	if err != nil {
		t.Fatalf("reminder history: %v", err)
	}
	if len(history) != 1 || history[0].ReasonCode == nil || *history[0].ReasonCode != string(workflow.ReminderReasonNotDue) {
		t.Fatalf("expected single not_due reminder history row, got %#v", history)
	}

	instanceAfter, err := service.GetInstance(ctx, instance.ID)
	if err != nil {
		t.Fatalf("load instance after not_due run: %v", err)
	}
	if instanceAfter.Status != workflow.InstanceStatusPending || instanceAfter.CurrentStepOrder == nil || *instanceAfter.CurrentStepOrder != 1 {
		t.Fatalf("expected workflow state to remain pending after not_due run, got %#v", instanceAfter)
	}
}

func TestWorkflowServiceDocumentFieldAssigneeResolutionAndResubmit(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)
	service := workflow.NewService(db, repo).SetDocumentFieldLookup(func(ctx context.Context, documentType string, documentID int64, fieldPath string) (any, bool, error) {
		if documentType == string(workflow.ObjectTypeLeaseContract) && documentID == 9951 && fieldPath == "created_by" {
			return int64(101), true, nil
		}
		return nil, false, nil
	})

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "lease-document-field-resolution",
		Name:            "Lease Document Field Resolution",
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
		Name:         "Lease Document Field Resolution V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{{
			FunctionID:   101,
			RoleID:       102,
			StepOrder:    1,
			Code:         "dept-review",
			Name:         "Department Review",
			ProcessClass: string(workflow.ObjectTypeLeaseContract),
			AssignmentRules: []workflow.AssignmentRuleInput{{
				StrategyType: workflow.AssignmentStrategyDocumentField,
				ConfigJSON:   `{"field_path":"created_by"}`,
			}},
		}},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "dept-review", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft definition: %v", err)
	}
	if _, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101}); err != nil {
		t.Fatalf("publish definition: %v", err)
	}

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeLeaseContract),
		DocumentID:     9951,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9951",
		Comment:        "submit document-field workflow",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	step, err := loadWorkflowStep(ctx, db, instance.ID, 1, 1)
	if err != nil {
		t.Fatalf("load cycle 1 step: %v", err)
	}
	if step.AssigneeDepartmentID != 101 || step.AssigneeRoleID != 102 || step.AssigneeUserID == nil || *step.AssigneeUserID != 101 {
		t.Fatalf("expected document_field created_by resolution to assign user 101 in department 101, got %#v", step)
	}

	instance, err = service.Reject(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 211, IdempotencyKey: "reject-9951", Comment: "reject for resubmit"})
	if err != nil {
		t.Fatalf("reject workflow: %v", err)
	}
	instance, err = service.Resubmit(ctx, workflow.TransitionInput{InstanceID: instance.ID, ActorUserID: 101, DepartmentID: 101, IdempotencyKey: "resubmit-9951", Comment: "resubmit document-field workflow"})
	if err != nil {
		t.Fatalf("resubmit workflow: %v", err)
	}
	step, err = loadWorkflowStep(ctx, db, instance.ID, 2, 1)
	if err != nil {
		t.Fatalf("load cycle 2 step: %v", err)
	}
	if step.AssigneeDepartmentID != 101 || step.AssigneeRoleID != 102 || step.AssigneeUserID == nil || *step.AssigneeUserID != 101 {
		t.Fatalf("expected document_field resolution to persist on resubmit, got %#v", step)
	}
}

func TestWorkflowServiceFixedUserAssigneeResolution(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)
	service := workflow.NewService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "lease-fixed-user-resolution",
		Name:            "Lease Fixed User Resolution",
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
		Name:         "Lease Fixed User Resolution V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{{
			FunctionID:   101,
			RoleID:       102,
			StepOrder:    1,
			Code:         "fixed-user-review",
			Name:         "Fixed User Review",
			ProcessClass: string(workflow.ObjectTypeLeaseContract),
			AssignmentRules: []workflow.AssignmentRuleInput{{
				StrategyType: workflow.AssignmentStrategyFixedUser,
				ConfigJSON:   `{"user_id":101}`,
			}},
		}},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "fixed-user-review", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft definition: %v", err)
	}
	if _, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101}); err != nil {
		t.Fatalf("publish definition: %v", err)
	}

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeLeaseContract),
		DocumentID:     9961,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9961",
		Comment:        "submit fixed-user workflow",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	step, err := loadWorkflowStep(ctx, db, instance.ID, 1, 1)
	if err != nil {
		t.Fatalf("load step: %v", err)
	}
	if step.AssigneeDepartmentID != 101 || step.AssigneeRoleID != 102 || step.AssigneeUserID == nil || *step.AssigneeUserID != 101 {
		t.Fatalf("expected fixed_user resolution to assign user 101 with department 101 and role 102, got %#v", step)
	}
}

func TestWorkflowServiceDepartmentLeaderAssigneeResolution(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	repo := workflow.NewRepository(db)
	adminService := workflow.NewAdminService(db, repo)
	service := workflow.NewService(db, repo)

	if _, err := db.ExecContext(ctx, `INSERT INTO user_roles (user_id, role_id, department_id) VALUES (101, 102, 102)`); err != nil {
		t.Fatalf("seed department leader assignment: %v", err)
	}

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 101,
		Code:            "lease-department-leader-resolution",
		Name:            "Lease Department Leader Resolution",
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
		Name:         "Lease Department Leader Resolution V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{{
			FunctionID:   101,
			RoleID:       102,
			StepOrder:    1,
			Code:         "department-leader-review",
			Name:         "Department Leader Review",
			ProcessClass: string(workflow.ObjectTypeLeaseContract),
			AssignmentRules: []workflow.AssignmentRuleInput{{
				StrategyType: workflow.AssignmentStrategyDepartmentLead,
				ConfigJSON:   `{}`,
			}},
		}},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "department-leader-review", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft definition: %v", err)
	}
	if _, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101}); err != nil {
		t.Fatalf("publish definition: %v", err)
	}

	instance, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeLeaseContract),
		DocumentID:     9962,
		ActorUserID:    101,
		DepartmentID:   102,
		IdempotencyKey: "start-9962",
		Comment:        "submit department-leader workflow",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	step, err := loadWorkflowStep(ctx, db, instance.ID, 1, 1)
	if err != nil {
		t.Fatalf("load step: %v", err)
	}
	if step.AssigneeDepartmentID != 102 || step.AssigneeRoleID != 102 || step.AssigneeUserID == nil || *step.AssigneeUserID != 101 {
		t.Fatalf("expected department_leader resolution to assign leader user 101 with department 102 and role 102, got %#v", step)
	}
}

func TestWorkflowServiceDeduplicateAndReminderPreservedWithAssignmentRules(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Minute)
	defer cancel()

	db := platformdb.NewTestDB(t, ctx, os.DirFS("../platform/database"))
	baseTime := time.Date(2026, 4, 10, 9, 0, 0, 0, time.UTC)
	repo := workflow.NewRepositoryWithNowFunc(db, func() time.Time { return baseTime })
	adminService := workflow.NewAdminService(db, repo)
	service := workflow.NewService(db, repo)

	created, err := adminService.CreateTemplate(ctx, workflow.CreateTemplateInput{
		BusinessGroupID: 102,
		Code:            "invoice-submitter-context-runtime",
		Name:            "Invoice Submitter Context Runtime",
		ProcessClass:    string(workflow.ObjectTypeInvoice),
		VoucherType:     "application",
		ActorUserID:     101,
	})
	if err != nil {
		t.Fatalf("create template: %v", err)
	}
	_, err = adminService.UpdateDraftDefinition(ctx, workflow.UpdateDraftDefinitionInput{
		DefinitionID: created.Definition.ID,
		ActorUserID:  101,
		Name:         "Invoice Submitter Context Runtime V1",
		VoucherType:  "application",
		Nodes: []workflow.DefinitionNodeInput{{
			FunctionID:   102,
			RoleID:       103,
			StepOrder:    1,
			Code:         "submitter-review",
			Name:         "Submitter Review",
			ProcessClass: string(workflow.ObjectTypeInvoice),
			AssignmentRules: []workflow.AssignmentRuleInput{{
				StrategyType: workflow.AssignmentStrategySubmitter,
				ConfigJSON:   `{}`,
			}},
		}},
		Transitions: []workflow.DefinitionTransitionInput{{ToNodeCode: "submitter-review", Action: workflow.ActionSubmit}},
	})
	if err != nil {
		t.Fatalf("update draft definition: %v", err)
	}
	if _, err := adminService.PublishDefinition(ctx, workflow.PublishDefinitionInput{DefinitionID: created.Definition.ID, ActorUserID: 101}); err != nil {
		t.Fatalf("publish definition: %v", err)
	}

	first, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeInvoice),
		DocumentID:     9952,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9952",
		Comment:        "submit with assignment rules",
	})
	if err != nil {
		t.Fatalf("start workflow: %v", err)
	}
	duplicate, err := service.Start(ctx, workflow.StartInput{
		DefinitionCode: created.Template.Code,
		DocumentType:   string(workflow.ObjectTypeInvoice),
		DocumentID:     9952,
		ActorUserID:    101,
		DepartmentID:   101,
		IdempotencyKey: "start-9952",
		Comment:        "duplicate submit with assignment rules",
	})
	if err != nil {
		t.Fatalf("duplicate start: %v", err)
	}
	if duplicate.ID != first.ID {
		t.Fatalf("expected duplicate start to return same instance, got %d vs %d", duplicate.ID, first.ID)
	}
	steps, err := countWorkflowSteps(ctx, db, first.ID)
	if err != nil {
		t.Fatalf("count workflow steps: %v", err)
	}
	if steps != 1 {
		t.Fatalf("expected 1 workflow step after duplicate start, got %d", steps)
	}

	beforeVersion := first.Version
	runAt := baseTime.Add(25 * time.Hour)
	records, err := service.RunReminders(ctx, runAt, workflow.ReminderConfig{ReminderType: "standard", MinPendingAge: 24 * time.Hour, WindowTruncation: 24 * time.Hour})
	if err != nil {
		t.Fatalf("run reminders: %v", err)
	}
	if len(records) != 1 || records[0].Outcome != workflow.ReminderOutcomeEmitted {
		t.Fatalf("expected emitted reminder record, got %#v", records)
	}
	after, err := service.GetInstance(ctx, first.ID)
	if err != nil {
		t.Fatalf("load instance after reminder: %v", err)
	}
	if after.Version != beforeVersion || after.Status != workflow.InstanceStatusPending {
		t.Fatalf("expected reminder run to preserve instance state, got %#v", after)
	}
	step, err := loadWorkflowStep(ctx, db, first.ID, 1, 1)
	if err != nil {
		t.Fatalf("load reminder step: %v", err)
	}
	if step.AssigneeUserID == nil || *step.AssigneeUserID != 101 {
		t.Fatalf("expected submitter-context assignee to remain intact, got %#v", step)
	}
}

func loadWorkflowStep(ctx context.Context, db *sql.DB, instanceID int64, cycle int, stepOrder int) (*workflow.Step, error) {
	row := db.QueryRowContext(ctx, `
		SELECT id, workflow_instance_id, workflow_node_id, step_order, cycle, assignee_role_id, assignee_department_id, assignee_user_id, status, action_comment, acted_by, acted_at
		FROM workflow_instance_steps
		WHERE workflow_instance_id = ? AND cycle = ? AND step_order = ?
		ORDER BY id
		LIMIT 1
	`, instanceID, cycle, stepOrder)
	var step workflow.Step
	if err := row.Scan(&step.ID, &step.WorkflowInstanceID, &step.WorkflowNodeID, &step.StepOrder, &step.Cycle, &step.AssigneeRoleID, &step.AssigneeDepartmentID, &step.AssigneeUserID, &step.Status, &step.ActionComment, &step.ActedBy, &step.ActedAt); err != nil {
		return nil, err
	}
	return &step, nil
}

func countWorkflowSteps(ctx context.Context, db *sql.DB, instanceID int64) (int, error) {
	var count int
	if err := db.QueryRowContext(ctx, `SELECT COUNT(*) FROM workflow_instance_steps WHERE workflow_instance_id = ?`, instanceID).Scan(&count); err != nil {
		return 0, err
	}
	return count, nil
}
