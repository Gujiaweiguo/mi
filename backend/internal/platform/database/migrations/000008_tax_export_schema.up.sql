CREATE TABLE tax_voucher_rule_sets (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  code VARCHAR(64) NOT NULL,
  name VARCHAR(128) NOT NULL,
  document_type VARCHAR(16) NOT NULL,
  status VARCHAR(32) NOT NULL,
  created_by BIGINT NOT NULL,
  updated_by BIGINT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_tax_voucher_rule_sets_code (code),
  CONSTRAINT fk_tax_voucher_rule_sets_created_by FOREIGN KEY (created_by) REFERENCES users(id),
  CONSTRAINT fk_tax_voucher_rule_sets_updated_by FOREIGN KEY (updated_by) REFERENCES users(id)
);

CREATE TABLE tax_voucher_rules (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  rule_set_id BIGINT NOT NULL,
  sequence_no INT NOT NULL,
  entry_side VARCHAR(16) NOT NULL,
  charge_type_filter VARCHAR(32) NOT NULL,
  account_number VARCHAR(64) NOT NULL,
  account_name VARCHAR(128) NOT NULL,
  explanation_template VARCHAR(255) NOT NULL,
  use_tenant_name BOOLEAN NOT NULL DEFAULT FALSE,
  is_balancing_entry BOOLEAN NOT NULL DEFAULT FALSE,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_tax_voucher_rules_set_sequence (rule_set_id, sequence_no),
  CONSTRAINT fk_tax_voucher_rules_rule_set FOREIGN KEY (rule_set_id) REFERENCES tax_voucher_rule_sets(id) ON DELETE CASCADE
);
