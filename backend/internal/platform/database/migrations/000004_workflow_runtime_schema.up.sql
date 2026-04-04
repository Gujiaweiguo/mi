CREATE TABLE workflow_transitions (
  id BIGINT PRIMARY KEY,
  workflow_definition_id BIGINT NOT NULL,
  from_node_id BIGINT NULL,
  to_node_id BIGINT NOT NULL,
  action VARCHAR(32) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_workflow_transitions_definition_action_from (workflow_definition_id, from_node_id, action),
  CONSTRAINT fk_workflow_transitions_definition FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id),
  CONSTRAINT fk_workflow_transitions_from_node FOREIGN KEY (from_node_id) REFERENCES workflow_nodes(id),
  CONSTRAINT fk_workflow_transitions_to_node FOREIGN KEY (to_node_id) REFERENCES workflow_nodes(id)
);

CREATE TABLE workflow_instances (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  workflow_definition_id BIGINT NOT NULL,
  document_type VARCHAR(64) NOT NULL,
  document_id BIGINT NOT NULL,
  status VARCHAR(32) NOT NULL,
  current_node_id BIGINT NULL,
  current_step_order INT NULL,
  current_cycle INT NOT NULL DEFAULT 1,
  version INT NOT NULL DEFAULT 1,
  submitted_by BIGINT NOT NULL,
  submitted_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  completed_at TIMESTAMP NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_workflow_instances_definition FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id),
  CONSTRAINT fk_workflow_instances_current_node FOREIGN KEY (current_node_id) REFERENCES workflow_nodes(id),
  CONSTRAINT fk_workflow_instances_submitted_by FOREIGN KEY (submitted_by) REFERENCES users(id)
);

CREATE TABLE workflow_instance_steps (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  workflow_instance_id BIGINT NOT NULL,
  workflow_node_id BIGINT NOT NULL,
  step_order INT NOT NULL,
  cycle INT NOT NULL,
  assignee_role_id BIGINT NOT NULL,
  assignee_department_id BIGINT NOT NULL,
  assignee_user_id BIGINT NULL,
  status VARCHAR(32) NOT NULL,
  action_comment TEXT NULL,
  acted_by BIGINT NULL,
  acted_at TIMESTAMP NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_workflow_instance_steps_instance_node_cycle (workflow_instance_id, workflow_node_id, cycle),
  CONSTRAINT fk_workflow_instance_steps_instance FOREIGN KEY (workflow_instance_id) REFERENCES workflow_instances(id),
  CONSTRAINT fk_workflow_instance_steps_node FOREIGN KEY (workflow_node_id) REFERENCES workflow_nodes(id),
  CONSTRAINT fk_workflow_instance_steps_role FOREIGN KEY (assignee_role_id) REFERENCES roles(id),
  CONSTRAINT fk_workflow_instance_steps_department FOREIGN KEY (assignee_department_id) REFERENCES departments(id),
  CONSTRAINT fk_workflow_instance_steps_assignee FOREIGN KEY (assignee_user_id) REFERENCES users(id),
  CONSTRAINT fk_workflow_instance_steps_actor FOREIGN KEY (acted_by) REFERENCES users(id)
);

CREATE TABLE workflow_audit_history (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  workflow_instance_id BIGINT NOT NULL,
  action VARCHAR(32) NOT NULL,
  actor_user_id BIGINT NOT NULL,
  from_status VARCHAR(32) NOT NULL,
  to_status VARCHAR(32) NOT NULL,
  from_step_order INT NULL,
  to_step_order INT NULL,
  comment TEXT NULL,
  idempotency_key VARCHAR(128) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_workflow_audit_instance_idempotency (workflow_instance_id, idempotency_key),
  CONSTRAINT fk_workflow_audit_instance FOREIGN KEY (workflow_instance_id) REFERENCES workflow_instances(id),
  CONSTRAINT fk_workflow_audit_actor FOREIGN KEY (actor_user_id) REFERENCES users(id)
);

CREATE TABLE outbox_messages (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  aggregate_type VARCHAR(64) NOT NULL,
  aggregate_id BIGINT NOT NULL,
  event_type VARCHAR(64) NOT NULL,
  dedupe_key VARCHAR(128) NOT NULL,
  payload JSON NOT NULL,
  status VARCHAR(32) NOT NULL,
  attempt_count INT NOT NULL DEFAULT 0,
  available_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  processed_at TIMESTAMP NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY uq_outbox_messages_dedupe_key (dedupe_key)
);
