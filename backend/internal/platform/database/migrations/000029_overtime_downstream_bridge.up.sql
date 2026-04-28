ALTER TABLE billing_charge_lines
  MODIFY lease_term_id BIGINT NULL,
  ADD COLUMN charge_source VARCHAR(32) NOT NULL DEFAULT 'standard' AFTER charge_type,
  ADD COLUMN overtime_bill_id BIGINT NULL AFTER charge_source,
  ADD COLUMN overtime_formula_id BIGINT NULL AFTER overtime_bill_id,
  ADD COLUMN overtime_charge_id BIGINT NULL AFTER overtime_formula_id,
  ADD UNIQUE KEY uq_billing_charge_lines_overtime_charge (overtime_charge_id),
  ADD CONSTRAINT fk_billing_charge_lines_overtime_bill FOREIGN KEY (overtime_bill_id) REFERENCES overtime_bills(id),
  ADD CONSTRAINT fk_billing_charge_lines_overtime_formula FOREIGN KEY (overtime_formula_id) REFERENCES overtime_bill_formulas(id),
  ADD CONSTRAINT fk_billing_charge_lines_overtime_charge FOREIGN KEY (overtime_charge_id) REFERENCES overtime_generated_charges(id);

ALTER TABLE billing_document_lines
  ADD COLUMN charge_source VARCHAR(32) NOT NULL DEFAULT 'standard' AFTER charge_type,
  ADD COLUMN overtime_bill_id BIGINT NULL AFTER charge_source,
  ADD COLUMN overtime_formula_id BIGINT NULL AFTER overtime_bill_id,
  ADD COLUMN overtime_charge_id BIGINT NULL AFTER overtime_formula_id,
  ADD CONSTRAINT fk_billing_document_lines_overtime_bill FOREIGN KEY (overtime_bill_id) REFERENCES overtime_bills(id),
  ADD CONSTRAINT fk_billing_document_lines_overtime_formula FOREIGN KEY (overtime_formula_id) REFERENCES overtime_bill_formulas(id),
  ADD CONSTRAINT fk_billing_document_lines_overtime_charge FOREIGN KEY (overtime_charge_id) REFERENCES overtime_generated_charges(id);

ALTER TABLE ar_open_items
  ADD COLUMN charge_source VARCHAR(32) NOT NULL DEFAULT 'standard' AFTER charge_type,
  ADD COLUMN overtime_bill_id BIGINT NULL AFTER charge_source,
  ADD COLUMN overtime_formula_id BIGINT NULL AFTER overtime_bill_id,
  ADD COLUMN overtime_charge_id BIGINT NULL AFTER overtime_formula_id,
  ADD CONSTRAINT fk_ar_open_items_overtime_bill FOREIGN KEY (overtime_bill_id) REFERENCES overtime_bills(id),
  ADD CONSTRAINT fk_ar_open_items_overtime_formula FOREIGN KEY (overtime_formula_id) REFERENCES overtime_bill_formulas(id),
  ADD CONSTRAINT fk_ar_open_items_overtime_charge FOREIGN KEY (overtime_charge_id) REFERENCES overtime_generated_charges(id);
