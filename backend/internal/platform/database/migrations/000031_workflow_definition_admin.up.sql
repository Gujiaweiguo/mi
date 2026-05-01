ALTER TABLE workflow_templates
  ADD COLUMN published_definition_id BIGINT NULL AFTER status,
  ADD COLUMN published_version_number INT NULL AFTER published_definition_id,
  ADD CONSTRAINT fk_workflow_templates_published_definition FOREIGN KEY (published_definition_id) REFERENCES workflow_definitions(id);

UPDATE workflow_templates wt
LEFT JOIN workflow_definitions wd
  ON wd.workflow_template_id = wt.id
 AND wd.status = 'active'
 AND wd.lifecycle_status = 'published'
SET wt.published_definition_id = wd.id,
    wt.published_version_number = wd.version_number
WHERE wt.published_definition_id IS NULL;

CREATE TABLE workflow_assignment_rules (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  workflow_node_id BIGINT NOT NULL,
  strategy_type VARCHAR(32) NOT NULL,
  config_json JSON NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  INDEX idx_workflow_assignment_rules_node (workflow_node_id),
  CONSTRAINT fk_workflow_assignment_rules_node FOREIGN KEY (workflow_node_id) REFERENCES workflow_nodes(id)
);

CREATE TABLE workflow_definition_audit (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  workflow_template_id BIGINT NOT NULL,
  workflow_definition_id BIGINT NULL,
  action VARCHAR(32) NOT NULL,
  actor_user_id BIGINT NOT NULL,
  details JSON NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_workflow_definition_audit_template (workflow_template_id),
  INDEX idx_workflow_definition_audit_definition (workflow_definition_id),
  CONSTRAINT fk_workflow_definition_audit_template FOREIGN KEY (workflow_template_id) REFERENCES workflow_templates(id),
  CONSTRAINT fk_workflow_definition_audit_definition FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id),
  CONSTRAINT fk_workflow_definition_audit_actor FOREIGN KEY (actor_user_id) REFERENCES users(id)
);
