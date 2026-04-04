SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'departments';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'users';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'stores';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'units';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'workflow_definitions';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'numbering_sequences';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'workflow_instances';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'workflow_instance_steps';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'workflow_audit_history';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'outbox_messages';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'lease_contracts';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'lease_contract_units';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'lease_contract_terms';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'billing_runs';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'billing_charge_lines';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'billing_documents';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'billing_document_lines';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'tax_voucher_rule_sets';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'tax_voucher_rules';

SELECT IF(COUNT(*) = 1, 1, 0)
FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'print_templates';
