import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import BillingChargesView from './BillingChargesView.vue'

const push = vi.fn()

vi.mock('vue-router', () => ({
  useRouter: () => ({ push }),
}))

vi.mock('../api/billing', () => ({
  generateCharges: vi.fn(),
  listCharges: vi.fn(),
}))

vi.mock('../api/invoice', () => ({
  createInvoice: vi.fn(),
}))

import { listCharges, type ChargeLine } from '../api/billing'
import { createInvoice } from '../api/invoice'

const chargeLines: ChargeLine[] = [
  {
    id: 11,
    billing_run_id: 5,
    lease_contract_id: 7,
    lease_no: 'LEASE-001',
    tenant_name: 'Tenant A',
    lease_term_id: null,
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
    currency_type_id: 1,
    source_effective_version: 1,
    created_at: '2026-01-01T00:00:00Z',
  },
  {
    id: 12,
    billing_run_id: 5,
    lease_contract_id: 7,
    lease_no: 'LEASE-001',
    tenant_name: 'Tenant A',
    lease_term_id: null,
    charge_type: 'service',
    charge_source: 'overtime',
    overtime_bill_id: null,
    overtime_formula_id: null,
    overtime_charge_id: null,
    period_start: '2026-01-01',
    period_end: '2026-01-31',
    quantity_days: 31,
    unit_amount: 250,
    amount: 250,
    currency_type_id: 1,
    source_effective_version: 1,
    created_at: '2026-01-01T00:00:00Z',
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(BillingChargesView, {
    global: {
      plugins: [i18n, ElementPlus],
    },
  })

  await flushPromises()

  return wrapper
}

const emitSelectionChange = async (wrapper: ReturnType<typeof mount>, selection: ChargeLine[]) => {
  wrapper.getComponent({ name: 'ElTable' }).vm.$emit('selection-change', selection)
  await flushPromises()
}

describe('BillingChargesView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')

    vi.mocked(listCharges).mockResolvedValue({
      data: {
        items: chargeLines,
        total: chargeLines.length,
        page: 1,
        page_size: 20,
      },
    } as never)
  })

  it('tracks row selection and enables create document when rows are selected', async () => {
    const wrapper = await mountView()

    expect((wrapper.get('[data-testid="create-document-button"]').element as HTMLButtonElement).disabled).toBe(true)

    await emitSelectionChange(wrapper, [chargeLines[0]])

    expect((wrapper.get('[data-testid="create-document-button"]').element as HTMLButtonElement).disabled).toBe(false)
  })

  it('creates a billing document successfully and clears the selection', async () => {
    vi.mocked(createInvoice).mockResolvedValue({
      data: {
        document: {
          id: 88,
        },
      },
    } as never)

    const wrapper = await mountView()

    await emitSelectionChange(wrapper, chargeLines)
    await wrapper.get('[data-testid="create-document-button"]').trigger('click')
    await flushPromises()

    wrapper.getComponent({ name: 'ElRadioGroup' }).vm.$emit('update:modelValue', 'bill')
    await flushPromises()

    await wrapper.get('[data-testid="create-document-confirm-button"]').trigger('click')
    await flushPromises()

    expect(createInvoice).toHaveBeenCalledWith({
      document_type: 'bill',
      billing_charge_line_ids: [11, 12],
    })
    expect(vi.mocked(listCharges)).toHaveBeenCalledTimes(2)
    expect(wrapper.get('[data-testid="billing-charges-feedback-alert"]').text()).toContain('Document created successfully')
    expect(wrapper.get('[data-testid="billing-charges-feedback-alert"]').text()).toContain('Document #88 has been created')
    expect((wrapper.get('[data-testid="create-document-button"]').element as HTMLButtonElement).disabled).toBe(true)
    expect(wrapper.getComponent({ name: 'ElDialog' }).props('modelValue')).toBe(false)
  })

  it('shows an error feedback and keeps the dialog open when document creation fails', async () => {
    vi.mocked(createInvoice).mockRejectedValue(new Error('Create failed'))

    const wrapper = await mountView()

    await emitSelectionChange(wrapper, [chargeLines[0]])
    await wrapper.get('[data-testid="create-document-button"]').trigger('click')
    await flushPromises()
    await wrapper.get('[data-testid="create-document-confirm-button"]').trigger('click')
    await flushPromises()

    expect(createInvoice).toHaveBeenCalledWith({
      document_type: 'invoice',
      billing_charge_line_ids: [11],
    })
    expect(wrapper.get('[data-testid="billing-charges-feedback-alert"]').text()).toContain('Failed to create document')
    expect(wrapper.get('[data-testid="billing-charges-feedback-alert"]').text()).toContain('Create failed')
    expect(wrapper.getComponent({ name: 'ElDialog' }).props('modelValue')).toBe(true)
  })

  it('keeps the create document button disabled when no rows are selected', async () => {
    const wrapper = await mountView()

    expect((wrapper.get('[data-testid="create-document-button"]').element as HTMLButtonElement).disabled).toBe(true)
  })
})
