DROP TABLE IF EXISTS ar_payment_entries;

ALTER TABLE ar_open_items
  DROP FOREIGN KEY fk_ar_open_items_document_line,
  DROP INDEX uq_ar_open_items_document_line,
  DROP INDEX idx_ar_open_items_document,
  DROP COLUMN billing_document_line_id,
  DROP COLUMN settled_at;
