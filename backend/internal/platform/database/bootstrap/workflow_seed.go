package bootstrap

import (
	"context"
	"database/sql"
)

func WorkflowSeeds() []Seed {
	return append([]Seed{seedBusinessGroups(), seedNumberingSequences()}, workflowDefinitionSeeds()...)
}

func workflowDefinitionSeeds() []Seed {
	return []Seed{
		seedWorkflowTemplates(),
		seedWorkflowDefinitions(),
		seedWorkflowTemplatePublications(),
		seedWorkflowNodes(),
		seedWorkflowTransitions(),
	}
}

func seedWorkflowTemplates() Seed {
	return Seed{
		name: "workflow_templates",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO workflow_templates (id, business_group_id, code, name, process_class, status)
				VALUES
				  (101, 101, 'lease-approval', 'Lease Approval', 'lease_contract', 'active'),
				  (102, 101, 'lease-change', 'Lease Change Approval', 'lease_change', 'active'),
				  (103, 102, 'invoice-approval', 'Invoice Approval', 'invoice', 'active'),
				  (104, 102, 'overtime-approval', 'Overtime Approval', 'overtime_bill', 'active')
				ON DUPLICATE KEY UPDATE name = VALUES(name), process_class = VALUES(process_class), status = VALUES(status)
			`)
			return err
		},
	}
}

func seedWorkflowTemplatePublications() Seed {
	return Seed{
		name: "workflow_template_publications",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				UPDATE workflow_templates wt
				INNER JOIN workflow_definitions wd ON wd.workflow_template_id = wt.id
				SET wt.published_definition_id = wd.id,
				    wt.published_version_number = wd.version_number
				WHERE wd.status = 'active' AND wd.lifecycle_status = 'published'
			`)
			return err
		},
	}
}

func seedBusinessGroups() Seed {
	return Seed{
		name: "business_groups",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO business_groups (id, code, name, status)
				VALUES
				  (101, 'lease', 'Lease', 'active'),
				  (102, 'billing', 'Billing', 'active'),
				  (103, 'workflow', 'Workflow', 'active')
				ON DUPLICATE KEY UPDATE name = VALUES(name), status = VALUES(status)
			`)
			return err
		},
	}
}

func seedWorkflowDefinitions() Seed {
	return Seed{
		name: "workflow_definitions",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO workflow_definitions (id, business_group_id, workflow_template_id, code, version_number, name, voucher_type, is_initial, status, lifecycle_status, published_at, transitions_enabled, process_class)
				VALUES
				  (101, 101, 101, 'lease-approval', 1, 'Lease Approval', 'application', TRUE, 'active', 'published', CURRENT_TIMESTAMP, TRUE, 'lease_contract'),
				  (102, 101, 102, 'lease-change', 1, 'Lease Change Approval', 'change_request', FALSE, 'active', 'published', CURRENT_TIMESTAMP, TRUE, 'lease_change'),
				  (103, 102, 103, 'invoice-approval', 1, 'Invoice Approval', 'application', FALSE, 'active', 'published', CURRENT_TIMESTAMP, TRUE, 'invoice'),
				  (104, 102, 104, 'overtime-approval', 1, 'Overtime Approval', 'application', FALSE, 'active', 'published', CURRENT_TIMESTAMP, TRUE, 'overtime_bill')
				ON DUPLICATE KEY UPDATE workflow_template_id = VALUES(workflow_template_id), version_number = VALUES(version_number), name = VALUES(name), voucher_type = VALUES(voucher_type), status = VALUES(status), lifecycle_status = VALUES(lifecycle_status), published_at = VALUES(published_at), transitions_enabled = VALUES(transitions_enabled), process_class = VALUES(process_class)
			`)
			return err
		},
	}
}

func seedWorkflowNodes() Seed {
	return Seed{
		name: "workflow_nodes",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
					INSERT INTO workflow_nodes (id, workflow_definition_id, function_id, role_id, step_order, code, name, can_submit_to_manager, validates_after_confirm, prints_after_confirm, process_class)
					VALUES
					  (101, 101, 101, 102, 1, 'lease-manager', 'Lease Manager Review', TRUE, FALSE, FALSE, 'lease_contract'),
					  (102, 101, 101, 103, 2, 'lease-finance', 'Lease Finance Review', FALSE, TRUE, TRUE, 'lease_contract'),
					  (104, 102, 101, 102, 1, 'lease-change-manager', 'Lease Change Manager Review', TRUE, FALSE, FALSE, 'lease_change'),
					  (105, 102, 101, 103, 2, 'lease-change-finance', 'Lease Change Finance Review', FALSE, TRUE, TRUE, 'lease_change'),
					  (103, 103, 102, 103, 1, 'invoice-finance', 'Invoice Finance Review', FALSE, TRUE, TRUE, 'invoice'),
					  (106, 104, 108, 102, 1, 'overtime-manager', 'Overtime Manager Review', TRUE, FALSE, FALSE, 'overtime_bill'),
					  (107, 104, 108, 103, 2, 'overtime-finance', 'Overtime Finance Review', FALSE, TRUE, FALSE, 'overtime_bill')
					ON DUPLICATE KEY UPDATE step_order = VALUES(step_order), name = VALUES(name), can_submit_to_manager = VALUES(can_submit_to_manager), validates_after_confirm = VALUES(validates_after_confirm), prints_after_confirm = VALUES(prints_after_confirm), process_class = VALUES(process_class)
				`)
			return err
		},
	}
}

func seedNumberingSequences() Seed {
	return Seed{
		name: "numbering_sequences",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
				INSERT INTO numbering_sequences (code, next_value, prefix)
				VALUES
				  ('contract', 101, 'CON'),
				  ('bill', 101, 'BIL'),
				  ('invoice', 101, 'INV'),
				  ('voucher', 101, 'VCH')
				ON DUPLICATE KEY UPDATE next_value = VALUES(next_value), prefix = VALUES(prefix)
			`)
			return err
		},
	}
}

func seedWorkflowTransitions() Seed {
	return Seed{
		name: "workflow_transitions",
		run: func(ctx context.Context, tx *sql.Tx) error {
			_, err := tx.ExecContext(ctx, `
					INSERT INTO workflow_transitions (id, workflow_definition_id, from_node_id, to_node_id, action)
					VALUES
					  (101, 101, NULL, 101, 'submit'),
					  (102, 101, 101, 102, 'approve'),
					  (104, 102, NULL, 104, 'submit'),
					  (105, 102, 104, 105, 'approve'),
					  (103, 103, NULL, 103, 'submit'),
					  (106, 104, NULL, 106, 'submit'),
					  (107, 104, 106, 107, 'approve')
					ON DUPLICATE KEY UPDATE to_node_id = VALUES(to_node_id), action = VALUES(action)
				`)
			return err
		},
	}
}
