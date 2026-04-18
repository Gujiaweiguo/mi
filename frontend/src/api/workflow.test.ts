import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  approveWorkflow,
  getWorkflowAuditHistory,
  getWorkflowInstance,
  listWorkflowDefinitions,
  listWorkflowInstances,
  rejectWorkflow,
  resubmitWorkflow,
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
