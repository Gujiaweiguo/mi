ALTER TABLE ar_open_items
  ADD COLUMN billing_document_line_id BIGINT NULL AFTER billing_document_id,
  ADD COLUMN settled_at TIMESTAMP NULL DEFAULT NULL AFTER outstanding_amount,
  ADD UNIQUE KEY uq_ar_open_items_document_line (billing_document_line_id),
  ADD KEY idx_ar_open_items_document (billing_document_id, due_date),
  ADD CONSTRAINT fk_ar_open_items_document_line FOREIGN KEY (billing_document_line_id) REFERENCES billing_document_lines(id);

CREATE TABLE ar_payment_entries (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  billing_document_id BIGINT NOT NULL,
  lease_contract_id BIGINT NOT NULL,
  payment_date DATE NOT NULL,
  amount DECIMAL(12,2) NOT NULL,
  note VARCHAR(255) NULL,
  recorded_by BIGINT NOT NULL,
  idempotency_key VARCHAR(128) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_ar_payment_entries_document_key (billing_document_id, idempotency_key),
  KEY idx_ar_payment_entries_document_date (billing_document_id, payment_date),
  KEY idx_ar_payment_entries_lease_date (lease_contract_id, payment_date),
  CONSTRAINT fk_ar_payment_entries_document FOREIGN KEY (billing_document_id) REFERENCES billing_documents(id),
  CONSTRAINT fk_ar_payment_entries_lease FOREIGN KEY (lease_contract_id) REFERENCES lease_contracts(id)
);
