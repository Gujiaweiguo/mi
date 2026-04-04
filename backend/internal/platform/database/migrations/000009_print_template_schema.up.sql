CREATE TABLE print_templates (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  code VARCHAR(64) NOT NULL,
  name VARCHAR(128) NOT NULL,
  document_type VARCHAR(32) NOT NULL,
  output_mode VARCHAR(32) NOT NULL,
  status VARCHAR(32) NOT NULL,
  title VARCHAR(255) NOT NULL,
  subtitle VARCHAR(255) NULL,
  header_lines TEXT NOT NULL,
  footer_lines TEXT NOT NULL,
  created_by BIGINT NOT NULL,
  updated_by BIGINT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_print_templates_code (code),
  CONSTRAINT fk_print_templates_created_by FOREIGN KEY (created_by) REFERENCES users(id),
  CONSTRAINT fk_print_templates_updated_by FOREIGN KEY (updated_by) REFERENCES users(id)
);
