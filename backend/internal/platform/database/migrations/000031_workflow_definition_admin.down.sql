DROP TABLE IF EXISTS workflow_definition_audit;

DROP TABLE IF EXISTS workflow_assignment_rules;

ALTER TABLE workflow_templates
  DROP FOREIGN KEY fk_workflow_templates_published_definition,
  DROP COLUMN published_version_number,
  DROP COLUMN published_definition_id;
