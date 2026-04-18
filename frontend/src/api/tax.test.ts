import { beforeEach, describe, expect, it, vi } from 'vitest'

import { exportTaxVouchers, listTaxRuleSets, upsertTaxRuleSet } from './tax'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('tax api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listTaxRuleSets', () => {
    it('calls GET /tax/rule-sets with params and returns the response', async () => {
      const params = { page: 1, page_size: 20 }
      const response = { data: { items: [{ id: 1 }], total: 1, page: 1, page_size: 20 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listTaxRuleSets(params)

      expect(http.get).toHaveBeenCalledWith('/tax/rule-sets', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listTaxRuleSets({ page: 2 })).rejects.toThrow('fail')
    })
  })

  describe('upsertTaxRuleSet', () => {
    it('calls POST /tax/rule-sets and returns the response', async () => {
      const payload = {
        code: 'TRS-001',
        name: 'Voucher Export',
        document_type: 'voucher',
        rules: [
          {
            sequence_no: 1,
            entry_side: 'debit',
            charge_type_filter: 'rent',
            account_number: '4001',
            account_name: 'Tax Payable',
            explanation_template: 'Voucher export',
            use_tenant_name: true,
            is_balancing_entry: false,
          },
        ],
      }
      const response = { data: { tax_rule_set: { id: 1, code: 'TRS-001' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await upsertTaxRuleSet(payload)

      expect(http.post).toHaveBeenCalledWith('/tax/rule-sets', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        upsertTaxRuleSet({
          code: 'TRS-001',
          name: 'Voucher Export',
          document_type: 'voucher',
          rules: [],
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('exportTaxVouchers', () => {
    it('calls POST /tax/exports/vouchers with blob response type and returns the response', async () => {
      const payload = {
        rule_set_code: 'TRS-001',
        from_date: '2026-01-01',
        to_date: '2026-01-31',
      }
      const response = { data: new Blob(['voucher-export']) } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await exportTaxVouchers(payload)

      expect(http.post).toHaveBeenCalledWith('/tax/exports/vouchers', payload, { responseType: 'blob' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        exportTaxVouchers({
          rule_set_code: 'TRS-001',
          from_date: '2026-01-01',
          to_date: '2026-01-31',
        }),
      ).rejects.toThrow('fail')
    })
  })
})
