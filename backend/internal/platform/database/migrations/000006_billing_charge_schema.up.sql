CREATE TABLE billing_runs (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  period_start DATE NOT NULL,
  period_end DATE NOT NULL,
  status VARCHAR(32) NOT NULL,
  triggered_by BIGINT NOT NULL,
  generated_count INT NOT NULL DEFAULT 0,
  skipped_count INT NOT NULL DEFAULT 0,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  KEY idx_billing_runs_period (period_start, period_end),
  CONSTRAINT fk_billing_runs_triggered_by FOREIGN KEY (triggered_by) REFERENCES users(id)
);

CREATE TABLE billing_charge_lines (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  billing_run_id BIGINT NOT NULL,
  lease_contract_id BIGINT NOT NULL,
  lease_term_id BIGINT NOT NULL,
  charge_type VARCHAR(32) NOT NULL,
  period_start DATE NOT NULL,
  period_end DATE NOT NULL,
  quantity_days INT NOT NULL,
  unit_amount DECIMAL(12,2) NOT NULL,
  amount DECIMAL(12,2) NOT NULL,
  currency_type_id BIGINT NOT NULL,
  source_effective_version INT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_billing_charge_lines_contract_term_period (lease_contract_id, lease_term_id, period_start, period_end),
  KEY idx_billing_charge_lines_run (billing_run_id),
  KEY idx_billing_charge_lines_period (period_start, period_end),
  CONSTRAINT fk_billing_charge_lines_run FOREIGN KEY (billing_run_id) REFERENCES billing_runs(id),
  CONSTRAINT fk_billing_charge_lines_contract FOREIGN KEY (lease_contract_id) REFERENCES lease_contracts(id),
  CONSTRAINT fk_billing_charge_lines_term FOREIGN KEY (lease_term_id) REFERENCES lease_contract_terms(id),
  CONSTRAINT fk_billing_charge_lines_currency FOREIGN KEY (currency_type_id) REFERENCES currency_types(id)
);
