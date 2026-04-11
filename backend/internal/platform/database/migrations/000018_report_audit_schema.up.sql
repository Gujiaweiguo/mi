CREATE TABLE report_audit_logs (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  report_id VARCHAR(16) NOT NULL,
  action VARCHAR(16) NOT NULL,
  actor_user_id BIGINT NOT NULL,
  row_count INT NOT NULL DEFAULT 0,
  export_size_bytes INT NOT NULL DEFAULT 0,
  request_payload JSON NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_report_audit_actor FOREIGN KEY (actor_user_id) REFERENCES users(id),
  INDEX idx_report_audit_report_created (report_id, created_at),
  INDEX idx_report_audit_actor_created (actor_user_id, created_at)
);
