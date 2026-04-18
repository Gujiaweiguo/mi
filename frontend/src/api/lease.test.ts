import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  amendLease,
  createLease,
  getLease,
  listLeases,
  submitLease,
  terminateLease,
} from './lease'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('lease api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('createLease', () => {
    it('calls POST /leases and returns the response', async () => {
      const payload = {
        lease_no: 'L-001',
        department_id: 1,
        store_id: 2,
        tenant_name: 'Tenant A',
        start_date: '2026-01-01',
        end_date: '2026-12-31',
        units: [{ unit_id: 10, rent_area: 99 }],
        terms: [
          {
            term_type: 'base_rent',
            billing_cycle: 'monthly',
            currency_type_id: 1,
            amount: 1000,
            effective_from: '2026-01-01',
            effective_to: '2026-12-31',
          },
        ],
      }
      const response = { data: { lease: { id: 1, lease_no: 'L-001' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createLease(payload)

      expect(http.post).toHaveBeenCalledWith('/leases', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createLease({
          lease_no: 'L-001',
          department_id: 1,
          store_id: 2,
          tenant_name: 'Tenant A',
          start_date: '2026-01-01',
          end_date: '2026-12-31',
          units: [{ unit_id: 10, rent_area: 99 }],
          terms: [
            {
              term_type: 'base_rent',
              billing_cycle: 'monthly',
              currency_type_id: 1,
              amount: 1000,
              effective_from: '2026-01-01',
              effective_to: '2026-12-31',
            },
          ],
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('listLeases', () => {
    it('calls GET /leases with params and returns the response', async () => {
      const params = { lease_no: 'L-001', status: 'draft', page: 1, page_size: 20 }
      const response = { data: { items: [{ id: 1 }], total: 1 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listLeases(params)

      expect(http.get).toHaveBeenCalledWith('/leases', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listLeases({ status: 'active' })).rejects.toThrow('fail')
    })
  })

  describe('getLease', () => {
    it('calls GET /leases/:id and returns the response', async () => {
      const response = { data: { lease: { id: 42, lease_no: 'L-042' } } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getLease(42)

      expect(http.get).toHaveBeenCalledWith('/leases/42')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(getLease(42)).rejects.toThrow('fail')
    })
  })

  describe('submitLease', () => {
    it('calls POST /leases/:id/submit and returns the response', async () => {
      const payload = { idempotency_key: 'submit-42' }
      const response = { data: { lease: { id: 42, status: 'submitted' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await submitLease(42, payload)

      expect(http.post).toHaveBeenCalledWith('/leases/42/submit', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(submitLease(42, { idempotency_key: 'submit-42' })).rejects.toThrow('fail')
    })
  })

  describe('amendLease', () => {
    it('calls POST /leases/:id/amend and returns the response', async () => {
      const payload = {
        lease_no: 'L-042A',
        department_id: 1,
        store_id: 2,
        tenant_name: 'Tenant B',
        start_date: '2026-02-01',
        end_date: '2026-12-31',
        units: [{ unit_id: 11, rent_area: 120 }],
        terms: [
          {
            term_type: 'base_rent',
            billing_cycle: 'monthly',
            currency_type_id: 1,
            amount: 1200,
            effective_from: '2026-02-01',
            effective_to: '2026-12-31',
          },
        ],
      }
      const response = { data: { lease: { id: 42, effective_version: 2 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await amendLease(42, payload)

      expect(http.post).toHaveBeenCalledWith('/leases/42/amend', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        amendLease(42, {
          lease_no: 'L-042A',
          department_id: 1,
          store_id: 2,
          tenant_name: 'Tenant B',
          start_date: '2026-02-01',
          end_date: '2026-12-31',
          units: [{ unit_id: 11, rent_area: 120 }],
          terms: [
            {
              term_type: 'base_rent',
              billing_cycle: 'monthly',
              currency_type_id: 1,
              amount: 1200,
              effective_from: '2026-02-01',
              effective_to: '2026-12-31',
            },
          ],
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('terminateLease', () => {
    it('calls POST /leases/:id/terminate and returns the response', async () => {
      const payload = { terminated_at: '2026-12-31' }
      const response = { data: { lease: { id: 42, status: 'terminated' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await terminateLease(42, payload)

      expect(http.post).toHaveBeenCalledWith('/leases/42/terminate', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(terminateLease(42, { terminated_at: '2026-12-31' })).rejects.toThrow('fail')
    })
  })
})
