import { beforeEach, describe, expect, it, vi } from 'vitest'

import {
  applyInvoiceDeposit,
  applyInvoiceSurplus,
  applyInvoiceDiscount,
  adjustInvoice,
  cancelInvoice,
  createInvoice,
  generateInvoiceInterest,
  getInvoice,
  getInvoiceReceivable,
  listInvoices,
  listReceivables,
  recordInvoicePayment,
  refundInvoiceDeposit,
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

  describe('applyInvoiceDiscount', () => {
    it('calls POST /invoices/:id/discounts and returns the response', async () => {
      const payload = {
        billing_document_line_id: 7,
        amount: 250,
        reason: 'launch support',
        idempotency_key: 'discount-1',
      }
      const response = { data: { receivable: { billing_document_id: 42, outstanding_amount: 500 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await applyInvoiceDiscount(42, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/discounts', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        applyInvoiceDiscount(42, {
          billing_document_line_id: 7,
          amount: 250,
          reason: 'launch support',
          idempotency_key: 'discount-1',
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('applyInvoiceSurplus', () => {
    it('calls POST /invoices/:id/surplus-applications and returns the response', async () => {
      const payload = {
        billing_document_line_id: 7,
        amount: 250,
        note: 'use surplus',
        idempotency_key: 'surplus-1',
      }
      const response = { data: { receivable: { billing_document_id: 42, outstanding_amount: 250 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await applyInvoiceSurplus(42, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/surplus-applications', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        applyInvoiceSurplus(42, {
          billing_document_line_id: 7,
          amount: 250,
          note: 'use surplus',
          idempotency_key: 'surplus-1',
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('generateInvoiceInterest', () => {
    it('calls POST /invoices/:id/interest and returns the response', async () => {
      const payload = {
        billing_document_line_id: 7,
        as_of_date: '2026-05-10',
        idempotency_key: 'interest-1',
      }
      const response = { data: { receivable: { billing_document_id: 42, outstanding_amount: 500 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await generateInvoiceInterest(42, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/42/interest', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        generateInvoiceInterest(42, {
          billing_document_line_id: 7,
          as_of_date: '2026-05-10',
          idempotency_key: 'interest-1',
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('applyInvoiceDeposit', () => {
    it('calls POST /invoices/:id/deposit-applications and returns the response', async () => {
      const payload = {
        billing_document_line_id: 17,
        target_document_id: 42,
        target_billing_document_line_id: 7,
        amount: 300,
        note: 'apply deposit',
        idempotency_key: 'deposit-1',
      }
      const response = { data: { receivable: { billing_document_id: 42, outstanding_amount: 700 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await applyInvoiceDeposit(99, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/99/deposit-applications', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        applyInvoiceDeposit(99, {
          billing_document_line_id: 17,
          target_document_id: 42,
          target_billing_document_line_id: 7,
          amount: 300,
          note: 'apply deposit',
          idempotency_key: 'deposit-1',
        }),
      ).rejects.toThrow('fail')
    })
  })

  describe('refundInvoiceDeposit', () => {
    it('calls POST /invoices/:id/deposit-refunds and returns the response', async () => {
      const payload = {
        billing_document_line_id: 17,
        amount: 500,
        reason: 'lease ended',
        idempotency_key: 'deposit-refund-1',
      }
      const response = { data: { receivable: { billing_document_id: 99, outstanding_amount: 0 } } } as never

      vi.mocked(http.post).mockResolvedValue(response)

      const result = await refundInvoiceDeposit(99, payload)

      expect(http.post).toHaveBeenCalledWith('/invoices/99/deposit-refunds', payload)
      expect(result).toEqual(response)
    })

    it('propagates errors', async () => {
      vi.mocked(http.post).mockRejectedValue(new Error('fail') as never)

      await expect(
        refundInvoiceDeposit(99, {
          billing_document_line_id: 17,
          amount: 500,
          reason: 'lease ended',
          idempotency_key: 'deposit-refund-1',
        }),
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
