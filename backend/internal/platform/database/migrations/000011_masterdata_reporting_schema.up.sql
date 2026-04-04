CREATE TABLE customers (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  code VARCHAR(64) NOT NULL UNIQUE,
  name VARCHAR(128) NOT NULL,
  trade_id BIGINT NULL,
  department_id BIGINT NULL,
  status VARCHAR(32) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_customers_trade FOREIGN KEY (trade_id) REFERENCES trade_definitions(id),
  CONSTRAINT fk_customers_department FOREIGN KEY (department_id) REFERENCES departments(id)
);

CREATE TABLE brands (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  code VARCHAR(64) NOT NULL UNIQUE,
  name VARCHAR(128) NOT NULL,
  status VARCHAR(32) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

ALTER TABLE units
  ADD COLUMN name VARCHAR(128) NOT NULL DEFAULT '' AFTER code;

ALTER TABLE lease_contracts
  ADD COLUMN customer_id BIGINT NULL AFTER building_id,
  ADD COLUMN brand_id BIGINT NULL AFTER customer_id,
  ADD COLUMN trade_id BIGINT NULL AFTER brand_id,
  ADD COLUMN management_type_id BIGINT NULL AFTER trade_id,
  ADD CONSTRAINT fk_lease_contracts_customer FOREIGN KEY (customer_id) REFERENCES customers(id),
  ADD CONSTRAINT fk_lease_contracts_brand FOREIGN KEY (brand_id) REFERENCES brands(id),
  ADD CONSTRAINT fk_lease_contracts_trade FOREIGN KEY (trade_id) REFERENCES trade_definitions(id),
  ADD CONSTRAINT fk_lease_contracts_management_type FOREIGN KEY (management_type_id) REFERENCES store_management_types(id);

CREATE INDEX idx_lease_contracts_customer ON lease_contracts(customer_id);
CREATE INDEX idx_lease_contracts_brand ON lease_contracts(brand_id);
CREATE INDEX idx_lease_contracts_trade ON lease_contracts(trade_id);
CREATE INDEX idx_lease_contracts_management_type ON lease_contracts(management_type_id);
