ALTER TABLE floors
  ADD COLUMN floor_plan_image_url VARCHAR(512) NULL AFTER status;

CREATE TABLE unit_layout_positions (
  unit_id BIGINT PRIMARY KEY,
  pos_x INT NOT NULL DEFAULT 0,
  pos_y INT NOT NULL DEFAULT 0,
  CONSTRAINT fk_unit_layout_positions_unit FOREIGN KEY (unit_id) REFERENCES units(id)
);

ALTER TABLE shop_types
  ADD COLUMN color_hex CHAR(7) NULL AFTER name;
