CREATE TABLE workflow_definitions (
  id BIGINT PRIMARY KEY,
  business_group_id BIGINT NOT NULL,
  code VARCHAR(64) NOT NULL UNIQUE,
  name VARCHAR(128) NOT NULL,
  voucher_type VARCHAR(32) NOT NULL,
  is_initial BOOLEAN NOT NULL DEFAULT FALSE,
  status VARCHAR(32) NOT NULL,
  transitions_enabled BOOLEAN NOT NULL DEFAULT TRUE,
  process_class VARCHAR(64) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_workflow_definitions_business_group FOREIGN KEY (business_group_id) REFERENCES business_groups(id)
);

CREATE TABLE workflow_nodes (
  id BIGINT PRIMARY KEY,
  workflow_definition_id BIGINT NOT NULL,
  function_id BIGINT NOT NULL,
  role_id BIGINT NOT NULL,
  step_order INT NOT NULL,
  code VARCHAR(64) NOT NULL,
  name VARCHAR(128) NOT NULL,
  can_submit_to_manager BOOLEAN NOT NULL DEFAULT FALSE,
  validates_after_confirm BOOLEAN NOT NULL DEFAULT FALSE,
  prints_after_confirm BOOLEAN NOT NULL DEFAULT FALSE,
  process_class VARCHAR(64) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_workflow_nodes_definition_code (workflow_definition_id, code),
  CONSTRAINT fk_workflow_nodes_definition FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id),
  CONSTRAINT fk_workflow_nodes_function FOREIGN KEY (function_id) REFERENCES functions(id),
  CONSTRAINT fk_workflow_nodes_role FOREIGN KEY (role_id) REFERENCES roles(id)
);

CREATE TABLE numbering_sequences (
  code VARCHAR(64) PRIMARY KEY,
  next_value BIGINT NOT NULL,
  prefix VARCHAR(32) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
