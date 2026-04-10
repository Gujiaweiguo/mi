CREATE TABLE workflow_reminder_audit (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  workflow_instance_id BIGINT NOT NULL,
  reminder_type VARCHAR(64) NOT NULL DEFAULT 'standard',
  reminder_key VARCHAR(255) NOT NULL,
  reminder_window_start TIMESTAMP NOT NULL,
  outcome VARCHAR(32) NOT NULL,
  reason_code VARCHAR(64) NOT NULL DEFAULT '',
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_workflow_reminder_audit_result (workflow_instance_id, reminder_key, outcome, reason_code),
  KEY idx_workflow_reminder_audit_key (reminder_key),
  KEY idx_workflow_reminder_audit_instance (workflow_instance_id),
  CONSTRAINT fk_workflow_reminder_audit_instance FOREIGN KEY (workflow_instance_id) REFERENCES workflow_instances(id)
);
