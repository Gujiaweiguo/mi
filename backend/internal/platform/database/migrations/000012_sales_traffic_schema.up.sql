CREATE TABLE daily_shop_sales (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  store_id BIGINT NOT NULL,
  unit_id BIGINT NOT NULL,
  sale_date DATE NOT NULL,
  sales_amount DECIMAL(15,2) NOT NULL DEFAULT 0,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uk_daily_sales (store_id, unit_id, sale_date),
  CONSTRAINT fk_daily_sales_store FOREIGN KEY (store_id) REFERENCES stores(id),
  CONSTRAINT fk_daily_sales_unit FOREIGN KEY (unit_id) REFERENCES units(id)
);

CREATE INDEX idx_daily_sales_date ON daily_shop_sales(sale_date);

CREATE TABLE customer_traffic (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  store_id BIGINT NOT NULL,
  traffic_date DATE NOT NULL,
  inbound_count INT NOT NULL DEFAULT 0,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uk_traffic (store_id, traffic_date),
  CONSTRAINT fk_traffic_store FOREIGN KEY (store_id) REFERENCES stores(id)
);

CREATE INDEX idx_traffic_date ON customer_traffic(traffic_date);
