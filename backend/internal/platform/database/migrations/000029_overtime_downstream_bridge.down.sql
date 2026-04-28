ALTER TABLE ar_open_items
  DROP FOREIGN KEY fk_ar_open_items_overtime_charge,
  DROP FOREIGN KEY fk_ar_open_items_overtime_formula,
  DROP FOREIGN KEY fk_ar_open_items_overtime_bill,
  DROP COLUMN overtime_charge_id,
  DROP COLUMN overtime_formula_id,
  DROP COLUMN overtime_bill_id,
  DROP COLUMN charge_source;

ALTER TABLE billing_document_lines
  DROP FOREIGN KEY fk_billing_document_lines_overtime_charge,
  DROP FOREIGN KEY fk_billing_document_lines_overtime_formula,
  DROP FOREIGN KEY fk_billing_document_lines_overtime_bill,
  DROP COLUMN overtime_charge_id,
  DROP COLUMN overtime_formula_id,
  DROP COLUMN overtime_bill_id,
  DROP COLUMN charge_source;

ALTER TABLE billing_charge_lines
  DROP FOREIGN KEY fk_billing_charge_lines_overtime_charge,
  DROP FOREIGN KEY fk_billing_charge_lines_overtime_formula,
  DROP FOREIGN KEY fk_billing_charge_lines_overtime_bill,
  DROP INDEX uq_billing_charge_lines_overtime_charge,
  DROP COLUMN overtime_charge_id,
  DROP COLUMN overtime_formula_id,
  DROP COLUMN overtime_bill_id,
  DROP COLUMN charge_source,
  MODIFY lease_term_id BIGINT NOT NULL;
