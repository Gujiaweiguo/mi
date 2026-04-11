-- Partial unique index: at most one non-completed instance per (definition, document_type, document_id).
-- MySQL 8 does not support WHERE clauses in indexes, so we use a generated column trick:
-- we only index rows where completed_at IS NULL (active instances).
ALTER TABLE workflow_instances
  ADD COLUMN active_uk_stub BIT(1) GENERATED ALWAYS AS (IF(completed_at IS NULL, 1, NULL)) STORED,
  ADD UNIQUE INDEX uq_workflow_instances_active_document (workflow_definition_id, document_type, document_id, active_uk_stub);
