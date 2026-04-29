import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import InvoiceDetailView from './InvoiceDetailView.vue'

const push = vi.fn()
const route = {
  params: { id: '42' },
  query: {} as Record<string, string>,
}

vi.mock('vue-router', () => ({
  useRoute: () => route,
  useRouter: () => ({ push }),
}))

vi.mock('../api/invoice', () => ({
  adjustInvoice: vi.fn(),
  applyInvoiceDeposit: vi.fn(),
  applyInvoiceDiscount: vi.fn(),
  applyInvoiceSurplus: vi.fn(),
  generateInvoiceInterest: vi.fn(),
  getInvoiceReceivable: vi.fn(),
  getInvoice: vi.fn(),
  listReceivables: vi.fn(),
  recordInvoicePayment: vi.fn(),
  refundInvoiceDeposit: vi.fn(),
  submitInvoice: vi.fn(),
  cancelInvoice: vi.fn(),
}))

import { adjustInvoice, getInvoice, getInvoiceReceivable, listReceivables } from '../api/invoice'

const baseInvoice = {
  id: 42,
  document_type: 'invoice',
  document_no: 'INV-042',
  billing_run_id: 9,
  lease_contract_id: 7,
  tenant_name: 'Tenant A',
  period_start: '2026-01-01',
  period_end: '2026-01-31',
  total_amount: 1200,
  currency_type_id: 1,
  workflow_instance_id: 4,
  adjusted_from_id: null,
  submitted_at: '2026-01-02T00:00:00Z',
  approved_at: '2026-01-03T00:00:00Z',
  cancelled_at: null,
  created_by: 1,
  updated_by: 1,
  created_at: '2026-01-01T00:00:00Z',
  updated_at: '2026-01-03T00:00:00Z',
  lines: [
    {
      id: 101,
      billing_document_id: 42,
      billing_charge_line_id: 501,
      charge_type: 'rent',
      charge_source: 'standard',
      overtime_bill_id: null,
      overtime_formula_id: null,
      overtime_charge_id: null,
      period_start: '2026-01-01',
      period_end: '2026-01-31',
      quantity_days: 31,
      unit_amount: 1000,
      amount: 1000,
      created_at: '2026-01-01T00:00:00Z',
    },
    {
      id: 102,
      billing_document_id: 42,
      billing_charge_line_id: 502,
      charge_type: 'service',
      charge_source: 'standard',
      overtime_bill_id: null,
      overtime_formula_id: null,
      overtime_charge_id: null,
      period_start: '2026-01-01',
      period_end: '2026-01-31',
      quantity_days: 31,
      unit_amount: 200,
      amount: 200,
      created_at: '2026-01-01T00:00:00Z',
    },
  ],
}

const emptyReceivable = {
  billing_document_id: 42,
  document_no: 'INV-042',
  document_type: 'invoice',
  tenant_name: 'Tenant A',
  lease_contract_id: 7,
  outstanding_amount: 1200,
  customer_surplus_available: 0,
  settlement_status: 'outstanding',
  items: [
    {
      id: 201,
      lease_contract_id: 7,
      billing_document_id: 42,
      billing_document_line_id: 101,
      customer_id: 1,
      department_id: 1,
      trade_id: null,
      charge_type: 'rent',
      charge_source: 'standard',
      overtime_bill_id: null,
      overtime_formula_id: null,
      overtime_charge_id: null,
      due_date: '2026-01-31',
      outstanding_amount: 1000,
      settled_at: null,
      is_deposit: false,
      created_at: '2026-01-01T00:00:00Z',
      updated_at: '2026-01-01T00:00:00Z',
    },
  ],
  payment_history: [],
  discount_history: [],
  surplus_history: [],
  interest_history: [],
  deposit_application_history: [],
  deposit_refund_history: [],
}

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

describe('InvoiceDetailView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
    route.params.id = '42'
    route.query = {}

    vi.mocked(listReceivables).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 100,
      },
    } as never)
  })

  it('renders adjusted status and source lineage on replacement drafts', async () => {
    vi.mocked(getInvoice).mockResolvedValue({
      data: {
        document: {
          ...baseInvoice,
          id: 77,
          document_no: 'INV-077',
          status: 'draft',
          adjusted_from_id: 42,
        },
      },
    } as never)

    const wrapper = mount(InvoiceDetailView, {
      global: {
        plugins: [i18n, ElementPlus],
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="invoice-adjusted-from-button"]').text()).toContain('Document #42')
    expect(wrapper.get('[data-testid="invoice-adjustment-lineage"]')).toBeTruthy()

    await wrapper.get('[data-testid="invoice-adjustment-view-source-button"]').trigger('click')

    expect(push).toHaveBeenCalledWith({
      name: 'billing-invoice-detail',
      params: { id: '42' },
    })
  })

  it('submits an invoice adjustment and redirects to the replacement draft', async () => {
    vi.mocked(getInvoice).mockResolvedValue({
      data: {
        document: {
          ...baseInvoice,
          status: 'approved',
        },
      },
    } as never)
    vi.mocked(getInvoiceReceivable).mockResolvedValue({ data: { receivable: emptyReceivable } } as never)
    vi.mocked(adjustInvoice).mockResolvedValue({
      data: {
        document: {
          ...baseInvoice,
          id: 99,
          status: 'draft',
          adjusted_from_id: 42,
        },
      },
    } as never)

    const wrapper = mount(InvoiceDetailView, {
      global: {
        plugins: [i18n, ElementPlus],
      },
    })

    await flushPromises()

    const firstAmountInput = wrapper.get('[data-testid="invoice-adjustment-amount-input-101"] input')
    const secondAmountInput = wrapper.get('[data-testid="invoice-adjustment-amount-input-102"] input')

    expect(firstAmountInput.element).toBeTruthy()
    expect(secondAmountInput.element).toBeTruthy()

    await firstAmountInput.setValue('900')
    await firstAmountInput.trigger('input')
    await nextTick()
    await wrapper.get('[data-testid="invoice-adjustment-submit-button"]').trigger('click')
    await flushPromises()

    expect(adjustInvoice).toHaveBeenCalledWith(42, {
      lines: [
        { billing_charge_line_id: 501, amount: 900 },
        { billing_charge_line_id: 502, amount: 200 },
      ],
    })
    expect(push).toHaveBeenCalledWith({
      name: 'billing-invoice-detail',
      params: { id: '99' },
      query: { adjustment: 'created' },
    })
  })
})
