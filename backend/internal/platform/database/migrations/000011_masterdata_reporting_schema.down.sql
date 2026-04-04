DROP INDEX idx_lease_contracts_management_type ON lease_contracts;
DROP INDEX idx_lease_contracts_trade ON lease_contracts;
DROP INDEX idx_lease_contracts_brand ON lease_contracts;
DROP INDEX idx_lease_contracts_customer ON lease_contracts;

ALTER TABLE lease_contracts
  DROP FOREIGN KEY fk_lease_contracts_management_type,
  DROP FOREIGN KEY fk_lease_contracts_trade,
  DROP FOREIGN KEY fk_lease_contracts_brand,
  DROP FOREIGN KEY fk_lease_contracts_customer,
  DROP COLUMN management_type_id,
  DROP COLUMN trade_id,
  DROP COLUMN brand_id,
  DROP COLUMN customer_id;

ALTER TABLE units
  DROP COLUMN name;

DROP TABLE IF EXISTS brands;
DROP TABLE IF EXISTS customers;
