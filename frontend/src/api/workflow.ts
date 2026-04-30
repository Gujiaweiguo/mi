import { http } from './http'
import type { IdempotencyRequest } from './types'

export interface WorkflowDefinition {
  ID: number
  Code: string
  Name: string
  ProcessClass: string
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
