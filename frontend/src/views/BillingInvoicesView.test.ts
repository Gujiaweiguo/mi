import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import BillingInvoicesView from './BillingInvoicesView.vue'

const push = vi.fn()

vi.mock('vue-router', () => ({
  useRouter: () => ({ push }),
}))

vi.mock('../api/invoice', () => ({
  cancelInvoice: vi.fn(),
  listInvoices: vi.fn(),
  submitInvoice: vi.fn(),
}))

import { listInvoices } from '../api/invoice'

const flushPromises = async () => {
  await Promise.resolve()
  await Promise.resolve()
}

describe('BillingInvoicesView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('loads billing invoice rows on mount', async () => {
    vi.mocked(listInvoices).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 20,
      },
    } as never)

    const wrapper = mount(BillingInvoicesView, {
      global: {
        plugins: [i18n, ElementPlus],
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="billing-invoices-view"]')).toBeTruthy()
    expect(listInvoices).toHaveBeenCalledTimes(1)
    expect(listInvoices).toHaveBeenCalledWith({
      document_type: undefined,
      status: undefined,
      page: 1,
      page_size: 20,
    })
  })

  it('shows adjusted status, lineage, and adjustment entry in the list', async () => {
    vi.mocked(listInvoices).mockResolvedValue({
      data: {
        items: [
          {
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
            status: 'adjusted',
            workflow_instance_id: 4,
            adjusted_from_id: 41,
            submitted_at: '2026-01-02T00:00:00Z',
            approved_at: '2026-01-03T00:00:00Z',
            cancelled_at: null,
            created_by: 1,
            updated_by: 1,
            created_at: '2026-01-01T00:00:00Z',
            updated_at: '2026-01-03T00:00:00Z',
            lines: [],
          },
          {
            id: 43,
            document_type: 'invoice',
            document_no: 'INV-043',
            billing_run_id: 9,
            lease_contract_id: 7,
            tenant_name: 'Tenant A',
            period_start: '2026-02-01',
            period_end: '2026-02-28',
            total_amount: 1500,
            currency_type_id: 1,
            status: 'approved',
            workflow_instance_id: 5,
            adjusted_from_id: null,
            submitted_at: '2026-02-02T00:00:00Z',
            approved_at: '2026-02-03T00:00:00Z',
            cancelled_at: null,
            created_by: 1,
            updated_by: 1,
            created_at: '2026-02-01T00:00:00Z',
            updated_at: '2026-02-03T00:00:00Z',
            lines: [],
          },
        ],
        total: 2,
        page: 1,
        page_size: 20,
      },
    } as never)

    const wrapper = mount(BillingInvoicesView, {
      global: {
        plugins: [i18n, ElementPlus],
      },
    })

    await flushPromises()

    expect(wrapper.text()).toContain('Adjusted')
    expect(wrapper.text()).toContain('Document #41')

    await wrapper.get('[data-testid="invoice-row-adjust-button-43"]').trigger('click')

    expect(push).toHaveBeenCalledWith({
      name: 'billing-invoice-detail',
      params: { id: '43' },
    })
  })

  it('shows an error alert when invoice loading fails', async () => {
    vi.mocked(listInvoices).mockRejectedValue(new Error('Invoice load failed'))

    const wrapper = mount(BillingInvoicesView, {
      global: {
        plugins: [i18n, ElementPlus],
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="billing-invoices-error-alert"]').text()).toContain('Invoice load failed')
  })
})
