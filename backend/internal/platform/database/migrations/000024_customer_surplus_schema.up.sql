CREATE TABLE customer_surplus_balances (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  customer_id BIGINT NOT NULL,
  available_amount DECIMAL(12,2) NOT NULL,
  last_applied_at TIMESTAMP NULL,
  created_by BIGINT NOT NULL,
  updated_by BIGINT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_customer_surplus_balances_customer (customer_id),
  CONSTRAINT fk_customer_surplus_balances_customer FOREIGN KEY (customer_id) REFERENCES customers(id),
  CONSTRAINT fk_customer_surplus_balances_created_by FOREIGN KEY (created_by) REFERENCES users(id),
  CONSTRAINT fk_customer_surplus_balances_updated_by FOREIGN KEY (updated_by) REFERENCES users(id)
);

CREATE TABLE ar_surplus_entries (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  surplus_balance_id BIGINT NOT NULL,
  entry_type VARCHAR(32) NOT NULL,
  customer_id BIGINT NOT NULL,
  billing_document_id BIGINT NULL,
  ar_open_item_id BIGINT NULL,
  amount DECIMAL(12,2) NOT NULL,
  note VARCHAR(255) NULL,
  idempotency_key VARCHAR(128) NOT NULL,
  recorded_by BIGINT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_ar_surplus_entries_document_type_key (billing_document_id, entry_type, idempotency_key),
  KEY idx_ar_surplus_entries_balance_created (surplus_balance_id, created_at),
  KEY idx_ar_surplus_entries_customer_created (customer_id, created_at),
  CONSTRAINT fk_ar_surplus_entries_balance FOREIGN KEY (surplus_balance_id) REFERENCES customer_surplus_balances(id),
  CONSTRAINT fk_ar_surplus_entries_customer FOREIGN KEY (customer_id) REFERENCES customers(id),
  CONSTRAINT fk_ar_surplus_entries_document FOREIGN KEY (billing_document_id) REFERENCES billing_documents(id),
  CONSTRAINT fk_ar_surplus_entries_open_item FOREIGN KEY (ar_open_item_id) REFERENCES ar_open_items(id),
  CONSTRAINT fk_ar_surplus_entries_recorded_by FOREIGN KEY (recorded_by) REFERENCES users(id)
);
