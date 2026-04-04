DROP INDEX idx_units_shop_type ON units;

ALTER TABLE units
  DROP FOREIGN KEY fk_units_shop_type,
  DROP COLUMN shop_type_id;
