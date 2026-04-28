ALTER TABLE lease_contracts
  ADD COLUMN subtype VARCHAR(32) NOT NULL DEFAULT 'standard' AFTER lease_no;

CREATE TABLE lease_contract_joint_operation_details (
  lease_contract_id BIGINT PRIMARY KEY,
  bill_cycle INT NOT NULL,
  rent_inc VARCHAR(255) NOT NULL,
  account_cycle INT NOT NULL,
  tax_rate DECIMAL(10,4) NOT NULL,
  tax_type INT NOT NULL,
  settlement_currency_type_id BIGINT NOT NULL,
  in_tax_rate DECIMAL(10,4) NOT NULL,
  out_tax_rate DECIMAL(10,4) NOT NULL,
  month_settle_days DECIMAL(10,2) NOT NULL DEFAULT 0,
  late_pay_interest_rate DECIMAL(10,4) NOT NULL DEFAULT 0,
  interest_grace_days INT NOT NULL DEFAULT 0,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_lease_joint_operation_contract FOREIGN KEY (lease_contract_id) REFERENCES lease_contracts(id) ON DELETE CASCADE,
  CONSTRAINT fk_lease_joint_operation_currency_type FOREIGN KEY (settlement_currency_type_id) REFERENCES currency_types(id)
);

CREATE TABLE lease_contract_ad_board_details (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  lease_contract_id BIGINT NOT NULL,
  ad_board_id BIGINT NOT NULL,
  description VARCHAR(255) NOT NULL DEFAULT '',
  status INT NOT NULL DEFAULT 0,
  start_date DATE NOT NULL,
  end_date DATE NOT NULL,
  rent_area DECIMAL(12,2) NOT NULL,
  airtime INT NOT NULL,
  frequency VARCHAR(8) NOT NULL,
  frequency_days INT NOT NULL DEFAULT 0,
  frequency_mon BOOLEAN NOT NULL DEFAULT FALSE,
  frequency_tue BOOLEAN NOT NULL DEFAULT FALSE,
  frequency_wed BOOLEAN NOT NULL DEFAULT FALSE,
  frequency_thu BOOLEAN NOT NULL DEFAULT FALSE,
  frequency_fri BOOLEAN NOT NULL DEFAULT FALSE,
  frequency_sat BOOLEAN NOT NULL DEFAULT FALSE,
  frequency_sun BOOLEAN NOT NULL DEFAULT FALSE,
  between_from INT NOT NULL DEFAULT 0,
  between_to INT NOT NULL DEFAULT 0,
  store_id BIGINT NULL,
  building_id BIGINT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  KEY idx_lease_ad_board_contract (lease_contract_id),
  CONSTRAINT fk_lease_ad_board_contract FOREIGN KEY (lease_contract_id) REFERENCES lease_contracts(id) ON DELETE CASCADE,
  CONSTRAINT fk_lease_ad_board_store FOREIGN KEY (store_id) REFERENCES stores(id),
  CONSTRAINT fk_lease_ad_board_building FOREIGN KEY (building_id) REFERENCES buildings(id)
);

CREATE TABLE lease_contract_area_ground_details (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  lease_contract_id BIGINT NOT NULL,
  code VARCHAR(64) NOT NULL,
  name VARCHAR(128) NOT NULL,
  type_id BIGINT NOT NULL,
  description VARCHAR(255) NOT NULL DEFAULT '',
  status INT NOT NULL DEFAULT 0,
  start_date DATE NOT NULL,
  end_date DATE NOT NULL,
  rent_area DECIMAL(12,2) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  KEY idx_lease_area_ground_contract (lease_contract_id),
  CONSTRAINT fk_lease_area_ground_contract FOREIGN KEY (lease_contract_id) REFERENCES lease_contracts(id) ON DELETE CASCADE
);
