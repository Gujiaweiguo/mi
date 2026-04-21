CREATE INDEX idx_lease_contracts_start_date ON lease_contracts (start_date);
CREATE INDEX idx_daily_sales_store_date ON daily_shop_sales (store_id, sale_date);
CREATE INDEX idx_traffic_store_date ON customer_traffic (store_id, traffic_date);
CREATE INDEX idx_units_status ON units (status);
CREATE INDEX idx_billing_documents_customer ON billing_documents (lease_contract_id, document_type);
