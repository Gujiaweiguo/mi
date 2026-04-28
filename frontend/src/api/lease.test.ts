import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  amendLease,
  cancelOvertimeBill,
  createLease,
  createOvertimeBill,
  generateOvertimeBillCharges,
  getLease,
  getOvertimeBill,
  listLeases,
  listOvertimeBills,
  stopOvertimeBill,
  submitLease,
  submitOvertimeBill,
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
        subtype: 'joint_operation' as const,
        department_id: 1,
        store_id: 2,
        tenant_name: 'Tenant A',
        start_date: '2026-01-01',
        end_date: '2026-12-31',
        joint_operation: {
          bill_cycle: 30,
          rent_inc: '5% yearly',
          account_cycle: 30,
          tax_rate: 0.09,
          tax_type: 1,
          settlement_currency_type_id: 1,
          in_tax_rate: 0.03,
          out_tax_rate: 0.06,
          month_settle_days: 25,
          late_pay_interest_rate: 0.01,
          interest_grace_days: 5,
        },
        units: [{ unit_id: 10, rent_area: 99 }],
        terms: [
          {
            term_type: 'rent',
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
          subtype: 'ad_board',
          department_id: 1,
          store_id: 2,
          tenant_name: 'Tenant A',
          start_date: '2026-01-01',
          end_date: '2026-12-31',
          ad_boards: [
            {
              ad_board_id: 900,
              description: 'Atrium screen',
              status: 1,
              start_date: '2026-01-01',
              end_date: '2026-01-31',
              rent_area: 12,
              airtime: 10,
              frequency: 'D',
              frequency_days: 3,
              frequency_mon: false,
              frequency_tue: false,
              frequency_wed: false,
              frequency_thu: false,
              frequency_fri: false,
              frequency_sat: false,
              frequency_sun: false,
              between_from: 1,
              between_to: 5,
              store_id: 2,
              building_id: null,
            },
          ],
          units: [{ unit_id: 10, rent_area: 99 }],
          terms: [
            {
              term_type: 'rent',
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
        subtype: 'area_ground' as const,
        department_id: 1,
        store_id: 2,
        tenant_name: 'Tenant B',
        start_date: '2026-02-01',
        end_date: '2026-12-31',
        area_grounds: [
          {
            code: 'AREA-A',
            name: 'Festival Plaza',
            type_id: 1,
            description: 'Main event area',
            status: 1,
            start_date: '2026-02-01',
            end_date: '2026-12-31',
            rent_area: 120,
          },
        ],
        units: [{ unit_id: 11, rent_area: 120 }],
        terms: [
          {
            term_type: 'rent',
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
          subtype: 'standard',
          department_id: 1,
          store_id: 2,
          tenant_name: 'Tenant B',
          start_date: '2026-02-01',
          end_date: '2026-12-31',
          units: [{ unit_id: 11, rent_area: 120 }],
          terms: [
            {
              term_type: 'rent',
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

  describe('overtime bill api', () => {
    const createPayload = {
      lease_contract_id: 42,
      period_start: '2026-04-01',
      period_end: '2026-04-30',
      note: 'April overtime',
      formulas: [
        {
          charge_type: 'overtime_rent',
          formula_type: 'fixed' as const,
          rate_type: 'daily' as const,
          effective_from: '2026-04-01',
          effective_to: '2026-04-30',
          currency_type_id: 1,
          total_area: 10,
          unit_price: 2,
          base_amount: 0,
          fixed_rental: 0,
          percentage_option: '',
          minimum_option: '',
          percentage_tiers: [],
          minimum_tiers: [],
        },
      ],
    }

    it('calls POST /overtime/bills and returns the response', async () => {
      const response = { data: { bill: { id: 11, status: 'draft' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createOvertimeBill(createPayload)

      expect(http.post).toHaveBeenCalledWith('/overtime/bills', createPayload)
      expect(result).toEqual(response)
    })

    it('calls GET /overtime/bills with params and returns the response', async () => {
      const params = { lease_contract_id: 42, status: 'approved', page: 1, page_size: 20 } as const
      const response = { data: { items: [{ id: 11 }], total: 1 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listOvertimeBills(params)

      expect(http.get).toHaveBeenCalledWith('/overtime/bills', { params })
      expect(result).toEqual(response)
    })

    it('calls GET /overtime/bills/:id and returns the response', async () => {
      const response = { data: { bill: { id: 11, status: 'approved' } } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getOvertimeBill(11)

      expect(http.get).toHaveBeenCalledWith('/overtime/bills/11')
      expect(result).toEqual(response)
    })

    it('calls POST /overtime/bills/:id/submit and returns the response', async () => {
      const payload = { idempotency_key: 'submit-ot-11', comment: 'submit overtime' }
      const response = { data: { bill: { id: 11, status: 'pending_approval' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await submitOvertimeBill(11, payload)

      expect(http.post).toHaveBeenCalledWith('/overtime/bills/11/submit', payload)
      expect(result).toEqual(response)
    })

    it('calls POST /overtime/bills/:id/cancel and returns the response', async () => {
      const response = { data: { bill: { id: 11, status: 'cancelled' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await cancelOvertimeBill(11)

      expect(http.post).toHaveBeenCalledWith('/overtime/bills/11/cancel')
      expect(result).toEqual(response)
    })

    it('calls POST /overtime/bills/:id/stop and returns the response', async () => {
      const response = { data: { bill: { id: 11, status: 'stopped' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await stopOvertimeBill(11)

      expect(http.post).toHaveBeenCalledWith('/overtime/bills/11/stop')
      expect(result).toEqual(response)
    })

    it('calls POST /overtime/bills/:id/generate and returns the response', async () => {
      const response = { data: { charges: [], skipped: [], totals: { generated: 0, skipped: 0 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await generateOvertimeBillCharges(11)

      expect(http.post).toHaveBeenCalledWith('/overtime/bills/11/generate', {})
      expect(result).toEqual(response)
    })
  })
})
