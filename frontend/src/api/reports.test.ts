import { beforeEach, describe, expect, it, vi } from 'vitest'

import { exportReport, queryReport } from './reports'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('reports api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('queryReport', () => {
    it('calls POST /reports/:id/query and returns the response', async () => {
      const payload = { period: '2026-01', store_id: 3 }
      const response = {
        data: {
          report_id: 'r01',
          columns: [{ key: 'tenant_name', label: 'Tenant Name' }],
          rows: [{ tenant_name: 'Tenant A' }],
          generated_at: '2026-01-31T00:00:00Z',
        },
      } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await queryReport('r01', payload)

      expect(http.post).toHaveBeenCalledWith('/reports/r01/query', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(queryReport('r01', { period: '2026-01' })).rejects.toThrow('fail')
    })
  })

  describe('exportReport', () => {
    it('calls POST /reports/:id/export with blob response type and returns the response', async () => {
      const payload = { period: '2026-01', store_id: 3 }
      const response = { data: new Blob(['report']) } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await exportReport('r19', payload)

      expect(http.post).toHaveBeenCalledWith('/reports/r19/export', payload, { responseType: 'blob' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(exportReport('r19', { period: '2026-01' })).rejects.toThrow('fail')
    })
  })
})
