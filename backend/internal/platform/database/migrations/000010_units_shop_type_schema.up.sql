ALTER TABLE units
  ADD COLUMN shop_type_id BIGINT NULL AFTER unit_type_id,
  ADD CONSTRAINT fk_units_shop_type FOREIGN KEY (shop_type_id) REFERENCES shop_types(id);

CREATE INDEX idx_units_shop_type ON units(shop_type_id);
