import { beforeEach, describe, expect, it, vi } from 'vitest'

import { generateCharges, listCharges } from './billing'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('billing api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('generateCharges', () => {
    it('calls POST /billing/charges/generate and returns the response', async () => {
      const payload = { period_start: '2026-01-01', period_end: '2026-01-31' }
      const response = { data: { run: { id: 1 }, lines: [], totals: { generated: 1, skipped: 0 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await generateCharges(payload)

      expect(http.post).toHaveBeenCalledWith('/billing/charges/generate', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(generateCharges({ period_start: '2026-01-01', period_end: '2026-01-31' })).rejects.toThrow(
        'fail',
      )
    })
  })

  describe('listCharges', () => {
    it('calls GET /billing/charges with params and returns the response', async () => {
      const params = { lease_contract_id: 5, period_start: '2026-01-01', page: 1, page_size: 20 }
      const response = { data: { items: [{ id: 1 }], total: 1 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listCharges(params)

      expect(http.get).toHaveBeenCalledWith('/billing/charges', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listCharges({ lease_contract_id: 5 })).rejects.toThrow('fail')
    })
  })
})
