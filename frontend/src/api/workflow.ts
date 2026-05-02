import { http } from './http'
import type { IdempotencyRequest } from './types'

export interface WorkflowDefinition {
  ID: number
  Code: string
  Name: string
  ProcessClass: string
}

export interface WorkflowDefinitionTemplate {
  id: number
  business_group_id: number
  code: string
  name: string
  process_class: string
  status: string
  published_definition_id?: number
  published_version_number?: number
}

export interface WorkflowDefinitionAssignmentRule {
  id: number
  workflow_node_id: number
  strategy_type: string
  config_json: string
}

export interface WorkflowDefinitionNode {
  ID: number
  WorkflowDefinitionID: number
  FunctionID: number
  RoleID: number
  StepOrder: number
  Code: string
  Name: string
  CanSubmitToManager: boolean
  ValidatesAfterConfirm: boolean
  PrintsAfterConfirm: boolean
  ProcessClass: string
  AssignmentRules: WorkflowDefinitionAssignmentRule[]
}

export interface WorkflowDefinitionTransition {
  ID: number
  WorkflowDefinitionID: number
  FromNodeID: number | null
  ToNodeID: number
  Action: string
}

export interface WorkflowDefinitionDetail {
  ID: number
  BusinessGroupID: number
  WorkflowTemplateID: number
  Code: string
  Name: string
  VoucherType: string
  IsInitial: boolean
  Status: string
  ProcessClass: string
  VersionNumber: number
  LifecycleStatus: string
  PublishedAt?: string | null
  TransitionsEnabled: boolean
  Nodes: WorkflowDefinitionNode[]
  Transitions: WorkflowDefinitionTransition[]
}

export interface WorkflowDefinitionValidationIssue {
  code: string
  field?: string
  message: string
}

export interface WorkflowDefinitionValidationResult {
  valid: boolean
  issues: WorkflowDefinitionValidationIssue[]
}

export interface SaveWorkflowDefinitionDraftNodeRequest {
  function_id: number
  role_id: number
  step_order: number
  code: string
  name: string
  can_submit_to_manager: boolean
  validates_after_confirm: boolean
  prints_after_confirm: boolean
  process_class: string
  assignment_rules: Array<{
    strategy_type: string
    config_json: string
  }>
}

export interface SaveWorkflowDefinitionDraftTransitionRequest {
  from_node_code: string | null
  to_node_code: string
  action: string
}

export interface SaveWorkflowDefinitionDraftRequest {
  name: string
  voucher_type: string
  is_initial: boolean
  nodes: SaveWorkflowDefinitionDraftNodeRequest[]
  transitions: SaveWorkflowDefinitionDraftTransitionRequest[]
}

export interface RollbackWorkflowDefinitionTemplateRequest {
  definition_id: number
}

export interface WorkflowInstance {
  id: number
  workflow_definition_id: number
  document_type: string
  document_id: number
  status: string
  current_node_id: number | null
  current_step_order: number | null
  current_cycle: number
  version: number
  submitted_by: number
  submitted_at: string
  completed_at: string | null
}

export interface WorkflowInstanceListItem extends WorkflowInstance {
  current_node_code?: string | null
  created_at?: string
}

export interface ListWorkflowInstancesParams {
  status?: string
  document_type?: string
  document_id?: number
}

export interface AuditEntry {
  id: number
  workflow_instance_id: number
  action: string
  actor_user_id: number
  from_status: string
  to_status: string
  from_step_order: number
  to_step_order: number
  comment: string
  idempotency_key: string
  created_at: string
}

export const startWorkflow = (data: {
  definition_code: string
  document_type: string
  document_id: number
  actor_user_id: number
  department_id: number
  idempotency_key: string
}) => http.post<{ instance: WorkflowInstance }>('/workflow/instances', data)
export const listWorkflowDefinitions = () => http.get<{ definitions: WorkflowDefinition[] }>('/workflow/definitions')
export const listWorkflowDefinitionTemplates = () =>
  http.get<{ templates: WorkflowDefinitionTemplate[] }>('/workflow/admin/templates')
export const listWorkflowDefinitionVersions = (templateId: number) =>
  http.get<{ definitions: WorkflowDefinitionDetail[] }>(`/workflow/admin/templates/${templateId}/versions`)
export const getWorkflowDefinition = (definitionId: number) =>
  http.get<{ definition: WorkflowDefinitionDetail }>(`/workflow/admin/definitions/${definitionId}`)
export const saveWorkflowDefinitionDraft = (definitionId: number, data: SaveWorkflowDefinitionDraftRequest) =>
  http.put<{ definition: WorkflowDefinitionDetail }>(`/workflow/admin/definitions/${definitionId}`, data)
export const validateWorkflowDefinition = (definitionId: number) =>
  http.post<{ validation: WorkflowDefinitionValidationResult }>(`/workflow/admin/definitions/${definitionId}/validate`)
export const publishWorkflowDefinition = (definitionId: number) =>
  http.post<{ definition: WorkflowDefinitionDetail }>(`/workflow/admin/definitions/${definitionId}/publish`)
export const deactivateWorkflowDefinitionTemplate = (templateId: number) =>
  http.post<{ template: WorkflowDefinitionTemplate }>(`/workflow/admin/templates/${templateId}/deactivate`)
export const rollbackWorkflowDefinitionTemplate = (templateId: number, data: RollbackWorkflowDefinitionTemplateRequest) =>
  http.post<{ definition: WorkflowDefinitionDetail }>(`/workflow/admin/templates/${templateId}/rollback`, data)
export const listWorkflowInstances = (params?: ListWorkflowInstancesParams) =>
  http.get<{ instances: WorkflowInstanceListItem[] }>('/workflow/instances', { params })
export const getWorkflowInstance = (id: number) =>
  http.get<{ instance: WorkflowInstance }>(`/workflow/instances/${id}`)
export const getWorkflowAuditHistory = (id: number) =>
  http.get<{ history: AuditEntry[] }>(`/workflow/instances/${id}/audit`)
export interface ReminderEntry { [key: string]: unknown }

export const getReminderHistory = (instanceId: number) =>
  http.get<{ reminders: ReminderEntry[] }>(`/workflow/instances/${instanceId}/reminders`)
export const approveWorkflow = (id: number, data: IdempotencyRequest) =>
  http.post<{ instance: WorkflowInstance }>(`/workflow/instances/${id}/approve`, data)
export const rejectWorkflow = (id: number, data: IdempotencyRequest) =>
  http.post<{ instance: WorkflowInstance }>(`/workflow/instances/${id}/reject`, data)
export const resubmitWorkflow = (id: number, data: IdempotencyRequest) =>
  http.post<{ instance: WorkflowInstance }>(`/workflow/instances/${id}/resubmit`, data)
export const runReminders = () => http.post('/workflow/reminders/run')
