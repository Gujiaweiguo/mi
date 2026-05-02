CREATE TABLE workflow_templates (
  id BIGINT PRIMARY KEY,
  business_group_id BIGINT NOT NULL,
  code VARCHAR(64) NOT NULL UNIQUE,
  name VARCHAR(128) NOT NULL,
  process_class VARCHAR(64) NOT NULL,
  status VARCHAR(32) NOT NULL DEFAULT 'active',
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_workflow_templates_business_group FOREIGN KEY (business_group_id) REFERENCES business_groups(id)
);

ALTER TABLE workflow_definitions
  ADD COLUMN workflow_template_id BIGINT NULL AFTER business_group_id,
  ADD COLUMN version_number INT NOT NULL DEFAULT 1 AFTER code,
  ADD COLUMN lifecycle_status VARCHAR(32) NOT NULL DEFAULT 'published' AFTER status,
  ADD COLUMN published_at TIMESTAMP NULL AFTER lifecycle_status;

INSERT INTO workflow_templates (id, business_group_id, code, name, process_class, status)
SELECT id, business_group_id, code, name, process_class, 'active'
FROM workflow_definitions
ON DUPLICATE KEY UPDATE
  business_group_id = VALUES(business_group_id),
  name = VALUES(name),
  process_class = VALUES(process_class),
  status = VALUES(status);

UPDATE workflow_definitions
SET workflow_template_id = id,
    version_number = 1,
    lifecycle_status = CASE WHEN status = 'active' THEN 'published' ELSE 'deactivated' END,
    published_at = COALESCE(published_at, created_at)
WHERE workflow_template_id IS NULL;

ALTER TABLE workflow_definitions
  MODIFY COLUMN workflow_template_id BIGINT NOT NULL,
  ADD CONSTRAINT fk_workflow_definitions_template FOREIGN KEY (workflow_template_id) REFERENCES workflow_templates(id),
  DROP INDEX code,
  ADD UNIQUE INDEX uq_workflow_definitions_template_version (workflow_template_id, version_number),
  ADD INDEX idx_workflow_definitions_code_lifecycle_version (code, lifecycle_status, version_number);

ALTER TABLE workflow_instances
  ADD COLUMN workflow_template_id BIGINT NULL AFTER workflow_definition_id,
  ADD COLUMN workflow_definition_version INT NOT NULL DEFAULT 1 AFTER workflow_template_id;

UPDATE workflow_instances wi
INNER JOIN workflow_definitions wd ON wd.id = wi.workflow_definition_id
SET wi.workflow_template_id = wd.workflow_template_id,
    wi.workflow_definition_version = wd.version_number
WHERE wi.workflow_template_id IS NULL;

ALTER TABLE workflow_instances
  MODIFY COLUMN workflow_template_id BIGINT NOT NULL,
  ADD CONSTRAINT fk_workflow_instances_template FOREIGN KEY (workflow_template_id) REFERENCES workflow_templates(id),
  ADD INDEX idx_workflow_instances_definition_id (workflow_definition_id),
  DROP INDEX uq_workflow_instances_active_document,
  DROP COLUMN active_uk_stub,
  ADD COLUMN active_uk_stub BIT(1) GENERATED ALWAYS AS (IF(completed_at IS NULL, 1, NULL)) STORED,
  ADD UNIQUE INDEX uq_workflow_instances_active_document (workflow_template_id, document_type, document_id, active_uk_stub);
