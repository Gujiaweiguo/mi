ALTER TABLE workflow_instances
  DROP INDEX uq_workflow_instances_active_document,
  DROP COLUMN active_uk_stub;
