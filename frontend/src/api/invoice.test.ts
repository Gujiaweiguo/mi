import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  adjustInvoice,
  cancelInvoice,
  createInvoice,
  getInvoice,
  getInvoiceReceivable,
  listInvoices,
  listReceivables,
  recordInvoicePayment,
  submitInvoice,
} from './invoice'

vi.mock('./http', () => ({
  http: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}))

import { http } from './http'

describe('invoice api', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('createInvoice', () => {
    it('calls POST /invoices and returns the response', async () => {
      const payload = { document_type: 'invoice' as const, billing_charge_line_ids: [1, 2] }
      const response = { data: { document: { id: 1, document_no: 'INV-001' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await createInvoice(payload)

      expect(http.post).toHaveBeenCalledWith('/invoices', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(createInvoice({ document_type: 'bill', billing_charge_line_ids: [1] })).rejects.toThrow(
        'fail',
      )
    })
  })

  describe('listInvoices', () => {
    it('calls GET /invoices with params and returns the response', async () => {
      const params = { status: 'draft', lease_contract_id: 12, page: 1, page_size: 20 }
      const response = { data: { items: [{ id: 1 }], total: 1 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listInvoices(params)

      expect(http.get).toHaveBeenCalledWith('/invoices', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listInvoices({ status: 'submitted' })).rejects.toThrow('fail')
    })
  })

  describe('getInvoice', () => {
    it('calls GET /invoices/:id and returns the response', async () => {
      const response = { data: { document: { id: 42, document_no: 'INV-042' } } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getInvoice(42)

      expect(http.get).toHaveBeenCalledWith('/invoices/42')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(getInvoice(42)).rejects.toThrow('fail')
    })
  })

  describe('submitInvoice', () => {
    it('calls POST /invoices/:id/submit and returns the response', async () => {
      const payload = { idempotency_key: 'submit-42' }
      const response = { data: { document: { id: 42, status: 'submitted' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await submitInvoice(42, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/submit', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(submitInvoice(42, { idempotency_key: 'submit-42' })).rejects.toThrow('fail')
    })
  })

  describe('cancelInvoice', () => {
    it('calls POST /invoices/:id/cancel and returns the response', async () => {
      const response = { data: { document: { id: 42, status: 'cancelled' } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await cancelInvoice(42)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/cancel')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(cancelInvoice(42)).rejects.toThrow('fail')
    })
  })

  describe('adjustInvoice', () => {
    it('calls POST /invoices/:id/adjust and returns the response', async () => {
      const payload = { lines: [{ billing_charge_line_id: 10, amount: 199 }] }
      const response = { data: { document: { id: 42, adjusted_from_id: 41 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await adjustInvoice(42, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/adjust', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(adjustInvoice(42, { lines: [{ billing_charge_line_id: 10, amount: 199 }] })).rejects.toThrow(
        'fail',
      )
    })
  })

  describe('getInvoiceReceivable', () => {
    it('calls GET /invoices/:id/receivable and returns the response', async () => {
      const response = { data: { receivable: { billing_document_id: 42, outstanding_amount: 500 } } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await getInvoiceReceivable(42)

      expect(http.get).toHaveBeenCalledWith('/invoices/42/receivable')
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(getInvoiceReceivable(42)).rejects.toThrow('fail')
    })
  })

  describe('recordInvoicePayment', () => {
    it('calls POST /invoices/:id/payments and returns the response', async () => {
      const payload = { idempotency_key: 'payment-42', amount: 500, payment_date: '2026-03-01', note: 'paid' }
      const response = { data: { receivable: { billing_document_id: 42, outstanding_amount: 0 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await recordInvoicePayment(42, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/payments', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        recordInvoicePayment(42, { idempotency_key: 'payment-42', amount: 500 }),
      ).rejects.toThrow('fail')
    })
  })

  describe('listReceivables', () => {
    it('calls GET /receivables with params and returns the response', async () => {
      const params = { customer_id: 9, department_id: 3, page: 1, page_size: 20 }
      const response = { data: { items: [{ billing_document_id: 42 }], total: 1 } } as never

      vi.mocked(http.get).mockResolvedValue(response)

      const result = await listReceivables(params)

      expect(http.get).toHaveBeenCalledWith('/receivables', { params })
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.get).mockRejectedValue(new Error('fail') as never)

      await expect(listReceivables({ customer_id: 9 })).rejects.toThrow('fail')
    })
  })
})
