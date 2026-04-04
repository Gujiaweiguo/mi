CREATE TABLE unit_rent_budgets (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  unit_id BIGINT NOT NULL,
  fiscal_year SMALLINT NOT NULL,
  budget_price DECIMAL(12,2) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_unit_rent_budgets_unit_year (unit_id, fiscal_year),
  CONSTRAINT fk_unit_rent_budgets_unit FOREIGN KEY (unit_id) REFERENCES units(id)
);

CREATE TABLE store_rent_budgets (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  store_id BIGINT NOT NULL,
  fiscal_year SMALLINT NOT NULL,
  fiscal_month TINYINT NOT NULL,
  monthly_budget DECIMAL(14,2) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_store_rent_budgets_store_period (store_id, fiscal_year, fiscal_month),
  CONSTRAINT fk_store_rent_budgets_store FOREIGN KEY (store_id) REFERENCES stores(id)
);

CREATE TABLE unit_prospects (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  unit_id BIGINT NOT NULL,
  fiscal_year SMALLINT NOT NULL,
  potential_customer_id BIGINT NULL,
  prospect_brand_id BIGINT NULL,
  prospect_trade_id BIGINT NULL,
  avg_transaction DECIMAL(12,2) NULL,
  prospect_rent_price DECIMAL(12,2) NULL,
  rent_increment VARCHAR(64) NULL,
  prospect_term_months INT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_unit_prospects_unit_year (unit_id, fiscal_year),
  CONSTRAINT fk_unit_prospects_unit FOREIGN KEY (unit_id) REFERENCES units(id),
  CONSTRAINT fk_unit_prospects_customer FOREIGN KEY (potential_customer_id) REFERENCES customers(id),
  CONSTRAINT fk_unit_prospects_brand FOREIGN KEY (prospect_brand_id) REFERENCES brands(id),
  CONSTRAINT fk_unit_prospects_trade FOREIGN KEY (prospect_trade_id) REFERENCES trade_definitions(id)
);
