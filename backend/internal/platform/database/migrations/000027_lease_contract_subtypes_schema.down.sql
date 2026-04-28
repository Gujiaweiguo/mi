DROP TABLE IF EXISTS lease_contract_area_ground_details;
DROP TABLE IF EXISTS lease_contract_ad_board_details;
DROP TABLE IF EXISTS lease_contract_joint_operation_details;

ALTER TABLE lease_contracts
  DROP COLUMN subtype;
