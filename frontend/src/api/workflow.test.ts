import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  approveWorkflow,
  deactivateWorkflowDefinitionTemplate,
  getWorkflowDefinition,
  getWorkflowAuditHistory,
  getWorkflowInstance,
  listWorkflowDefinitionTemplates,
  listWorkflowDefinitionVersions,
  listWorkflowDefinitions,
  listWorkflowInstances,
  publishWorkflowDefinition,
  rejectWorkflow,
  rollbackWorkflowDefinitionTemplate,
  resubmitWorkflow,
  saveWorkflowDefinitionDraft,
  validateWorkflowDefinition,
} from './workflow'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('workflow api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listWorkflowDefinitions', () => {
    it('calls GET /workflow/definitions and returns the response', async () => {
      const response = { data: { definitions: [{ ID: 1, Code: 'LEASE', Name: 'Lease', ProcessClass: 'lease' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listWorkflowDefinitions()

      expect(http.get).toHaveBeenCalledWith('/workflow/definitions')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listWorkflowDefinitions()).rejects.toThrow('fail')
    })
  })

  describe('workflow definition admin', () => {
    it('listWorkflowDefinitionTemplates calls GET and returns the response', async () => {
      const response = { data: { templates: [{ id: 7, code: 'lease-approval', process_class: 'lease_contract' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listWorkflowDefinitionTemplates()

      expect(http.get).toHaveBeenCalledWith('/workflow/admin/templates')
      expect(result).toEqual(response)
    })

    it('listWorkflowDefinitionVersions calls GET and returns the response', async () => {
      const response = { data: { definitions: [{ ID: 11, WorkflowTemplateID: 7, VersionNumber: 2 }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listWorkflowDefinitionVersions(7)

      expect(http.get).toHaveBeenCalledWith('/workflow/admin/templates/7/versions')
      expect(result).toEqual(response)
    })

    it('getWorkflowDefinition calls GET and returns the response', async () => {
      const response = { data: { definition: { ID: 11, WorkflowTemplateID: 7, Nodes: [], Transitions: [] } } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getWorkflowDefinition(11)

      expect(http.get).toHaveBeenCalledWith('/workflow/admin/definitions/11')
      expect(result).toEqual(response)
    })

    it('saveWorkflowDefinitionDraft calls PUT and returns the response', async () => {
      const payload = {
        name: 'Lease Approval V2',
        voucher_type: 'LEASE',
        is_initial: false,
        nodes: [
          {
            function_id: 101,
            role_id: 201,
            step_order: 1,
            code: 'SUBMIT',
            name: 'Submit',
            can_submit_to_manager: false,
            validates_after_confirm: false,
            prints_after_confirm: false,
            process_class: 'lease_contract',
            assignment_rules: [{ strategy_type: 'fixed_role', config_json: '{"role_id":201}' }],
          },
        ],
        transitions: [{ from_node_code: null, to_node_code: 'SUBMIT', action: 'submit' }],
      }
      const response = { data: { definition: { ID: 11, Name: 'Lease Approval V2' } } } as never

      vi.mocked(http.put).mockResolvedValue(response)

      const result = await saveWorkflowDefinitionDraft(11, payload)

      expect(http.put).toHaveBeenCalledWith('/workflow/admin/definitions/11', payload)
      expect(result).toEqual(response)
    })

    it('validateWorkflowDefinition calls POST and returns the response', async () => {
      const response = { data: { validation: { valid: true, issues: [] } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await validateWorkflowDefinition(11)

      expect(http.post).toHaveBeenCalledWith('/workflow/admin/definitions/11/validate')
      expect(result).toEqual(response)
    })

    it('publishWorkflowDefinition calls POST and returns the response', async () => {
      const response = { data: { definition: { ID: 11, LifecycleStatus: 'published' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await publishWorkflowDefinition(11)

      expect(http.post).toHaveBeenCalledWith('/workflow/admin/definitions/11/publish')
      expect(result).toEqual(response)
    })

    it('deactivateWorkflowDefinitionTemplate calls POST and returns the response', async () => {
      const response = { data: { template: { id: 7, status: 'active' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await deactivateWorkflowDefinitionTemplate(7)

      expect(http.post).toHaveBeenCalledWith('/workflow/admin/templates/7/deactivate')
      expect(result).toEqual(response)
    })

    it('rollbackWorkflowDefinitionTemplate calls POST and returns the response', async () => {
      const payload = { definition_id: 11 }
      const response = { data: { definition: { ID: 11, LifecycleStatus: 'published' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await rollbackWorkflowDefinitionTemplate(7, payload)

      expect(http.post).toHaveBeenCalledWith('/workflow/admin/templates/7/rollback', payload)
      expect(result).toEqual(response)
    })

    it('propagates admin API errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)
      vi.mocked(http.put).mockRejectedValue(new Error('fail') as never)
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(listWorkflowDefinitionTemplates()).rejects.toThrow('fail')
      await expect(listWorkflowDefinitionVersions(7)).rejects.toThrow('fail')
      await expect(getWorkflowDefinition(11)).rejects.toThrow('fail')
      await expect(
        saveWorkflowDefinitionDraft(11, {
          name: 'Lease Approval V2',
          voucher_type: '',
          is_initial: false,
          nodes: [],
          transitions: [],
        }),
      ).rejects.toThrow('fail')
      await expect(validateWorkflowDefinition(11)).rejects.toThrow('fail')
      await expect(publishWorkflowDefinition(11)).rejects.toThrow('fail')
      await expect(deactivateWorkflowDefinitionTemplate(7)).rejects.toThrow('fail')
      await expect(rollbackWorkflowDefinitionTemplate(7, { definition_id: 11 })).rejects.toThrow('fail')
    })
  })

  describe('listWorkflowInstances', () => {
    it('calls GET /workflow/instances with params and returns the response', async () => {
      const params = { status: 'pending', document_type: 'lease', document_id: 42 }
      const response = { data: { instances: [{ id: 1, status: 'pending' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listWorkflowInstances(params)

      expect(http.get).toHaveBeenCalledWith('/workflow/instances', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listWorkflowInstances({ status: 'pending' })).rejects.toThrow('fail')
    })
  })

  describe('getWorkflowInstance', () => {
    it('calls GET /workflow/instances/:id and returns the response', async () => {
      const response = { data: { instance: { id: 42, status: 'pending' } } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getWorkflowInstance(42)

      expect(http.get).toHaveBeenCalledWith('/workflow/instances/42')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(getWorkflowInstance(42)).rejects.toThrow('fail')
    })
  })

  describe('getWorkflowAuditHistory', () => {
    it('calls GET /workflow/instances/:id/audit and returns the response', async () => {
      const response = { data: { history: [{ id: 1, action: 'submit' }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getWorkflowAuditHistory(42)

      expect(http.get).toHaveBeenCalledWith('/workflow/instances/42/audit')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(getWorkflowAuditHistory(42)).rejects.toThrow('fail')
    })
  })

  describe('approveWorkflow', () => {
    it('calls POST /workflow/instances/:id/approve and returns the response', async () => {
      const payload = { idempotency_key: 'approve-42' }
      const response = { data: { instance: { id: 42, status: 'approved' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await approveWorkflow(42, payload)

      expect(http.post).toHaveBeenCalledWith('/workflow/instances/42/approve', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(approveWorkflow(42, { idempotency_key: 'approve-42' })).rejects.toThrow('fail')
    })
  })

  describe('rejectWorkflow', () => {
    it('calls POST /workflow/instances/:id/reject and returns the response', async () => {
      const payload = { idempotency_key: 'reject-42' }
      const response = { data: { instance: { id: 42, status: 'rejected' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await rejectWorkflow(42, payload)

      expect(http.post).toHaveBeenCalledWith('/workflow/instances/42/reject', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(rejectWorkflow(42, { idempotency_key: 'reject-42' })).rejects.toThrow('fail')
    })
  })

  describe('resubmitWorkflow', () => {
    it('calls POST /workflow/instances/:id/resubmit and returns the response', async () => {
      const payload = { idempotency_key: 'resubmit-42' }
      const response = { data: { instance: { id: 42, status: 'pending' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await resubmitWorkflow(42, payload)

      expect(http.post).toHaveBeenCalledWith('/workflow/instances/42/resubmit', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(resubmitWorkflow(42, { idempotency_key: 'resubmit-42' })).rejects.toThrow('fail')
    })
  })
})
