import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  createCustomerTraffic,
  createDailySale,
  downloadCustomerTrafficTemplate,
  downloadDailySalesTemplate,
  importCustomerTrafficWorkbook,
  importDailySalesWorkbook,
  listCustomerTraffic,
  listDailySales,
} from './sales'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('sales api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('listDailySales', () => {
    it('calls GET /sales/daily with params and returns the response', async () => {
      const params = {
        store_id: 1,
        unit_id: 2,
        date_from: '2026-01-01',
        date_to: '2026-01-31',
      }
      const response = { data: { daily_sales: [{ id: 1, sales_amount: 5000 }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listDailySales(params)

      expect(http.get).toHaveBeenCalledWith('/sales/daily', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listDailySales({ store_id: 1 })).rejects.toThrow('fail')
    })
  })

  describe('createDailySale', () => {
    it('calls POST /sales/daily and returns the response', async () => {
      const payload = {
        store_id: 1,
        unit_id: 2,
        sale_date: '2026-01-01',
        sales_amount: 5000,
      }
      const response = { data: { daily_sale: { id: 1, store_id: 1 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createDailySale(payload)

      expect(http.post).toHaveBeenCalledWith('/sales/daily', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createDailySale({
          store_id: 1,
          unit_id: 2,
          sale_date: '2026-01-01',
          sales_amount: 5000,
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('downloadDailySalesTemplate', () => {
    it('calls GET /excel/templates/daily-sales with blob response type and returns the response', async () => {
      const response = { data: new Blob(['daily-sales-template']) } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await downloadDailySalesTemplate()

      expect(http.get).toHaveBeenCalledWith('/excel/templates/daily-sales', { responseType: 'blob' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(downloadDailySalesTemplate()).rejects.toThrow('fail')
    })
  })

  describe('importDailySalesWorkbook', () => {
    it('calls POST /excel/imports/daily-sales with form data and validateStatus config and returns the response', async () => {
      const file = new File(['daily-sales'], 'daily-sales.xlsx')
      const response = { data: { imported_count: 2, diagnostics: [] } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await importDailySalesWorkbook(file)

      expect(http.post).toHaveBeenCalledWith(
        '/excel/imports/daily-sales',
        expect.any(FormData),
        expect.objectContaining({ validateStatus: expect.any(Function) }),
      )
      expect(vi.mocked(http.post).mock.calls[0]?.[1]).toBeInstanceOf(FormData)

      const config = vi.mocked(http.post).mock.calls[0]?.[2]
      expect(config?.validateStatus?.(200)).toBe(true)
      expect(config?.validateStatus?.(400)).toBe(true)
      expect(config?.validateStatus?.(500)).toBe(false)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(importDailySalesWorkbook(new File(['daily-sales'], 'daily-sales.xlsx'))).rejects.toThrow('fail')
    })
  })

  describe('listCustomerTraffic', () => {
    it('calls GET /sales/traffic with params and returns the response', async () => {
      const params = {
        store_id: 1,
        date_from: '2026-01-01',
        date_to: '2026-01-31',
      }
      const response = { data: { customer_traffic: [{ id: 1, inbound_count: 300 }] } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listCustomerTraffic(params)

      expect(http.get).toHaveBeenCalledWith('/sales/traffic', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listCustomerTraffic({ store_id: 1 })).rejects.toThrow('fail')
    })
  })

  describe('createCustomerTraffic', () => {
    it('calls POST /sales/traffic and returns the response', async () => {
      const payload = {
        store_id: 1,
        traffic_date: '2026-01-01',
        inbound_count: 300,
      }
      const response = { data: { traffic: { id: 1, store_id: 1 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createCustomerTraffic(payload)

      expect(http.post).toHaveBeenCalledWith('/sales/traffic', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        createCustomerTraffic({
          store_id: 1,
          traffic_date: '2026-01-01',
          inbound_count: 300,
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('downloadCustomerTrafficTemplate', () => {
    it('calls GET /excel/templates/customer-traffic with blob response type and returns the response', async () => {
      const response = { data: new Blob(['customer-traffic-template']) } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await downloadCustomerTrafficTemplate()

      expect(http.get).toHaveBeenCalledWith('/excel/templates/customer-traffic', { responseType: 'blob' })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(downloadCustomerTrafficTemplate()).rejects.toThrow('fail')
    })
  })

  describe('importCustomerTrafficWorkbook', () => {
    it('calls POST /excel/imports/customer-traffic with form data and validateStatus config and returns the response', async () => {
      const file = new File(['customer-traffic'], 'customer-traffic.xlsx')
      const response = { data: { imported_count: 2, diagnostics: [] } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await importCustomerTrafficWorkbook(file)

      expect(http.post).toHaveBeenCalledWith(
        '/excel/imports/customer-traffic',
        expect.any(FormData),
        expect.objectContaining({ validateStatus: expect.any(Function) }),
      )
      expect(vi.mocked(http.post).mock.calls[0]?.[1]).toBeInstanceOf(FormData)

      const config = vi.mocked(http.post).mock.calls[0]?.[2]
      expect(config?.validateStatus?.(200)).toBe(true)
      expect(config?.validateStatus?.(400)).toBe(true)
      expect(config?.validateStatus?.(500)).toBe(false)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        importCustomerTrafficWorkbook(new File(['customer-traffic'], 'customer-traffic.xlsx')),
      ).rejects.toThrow('fail')
    })
  })
})
