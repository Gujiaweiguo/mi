import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import BillingInvoicesView from './BillingInvoicesView.vue'

vi.mock('vue-router', () => ({
  useRouter: () => ({ push: vi.fn() }),
}))

vi.mock('../api/invoice', () => ({
  cancelInvoice: vi.fn(),
  listInvoices: vi.fn(),
  submitInvoice: vi.fn(),
}))

import { listInvoices } from '../api/invoice'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props) {
    return () => h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary)])
  },
})

const FilterFormStub = defineComponent({
  props: {
    title: { type: String, default: '' },
  },
  setup(props) {
    return () => h('section', { 'data-testid': 'billing-filter-form-stub' }, props.title)
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props, { attrs }) {
    return () =>
      h(
        'div',
        { 'data-testid': attrs['data-testid'] ?? 'billing-alert-stub' },
        `${props.title} ${props.description}`.trim(),
      )
  },
})

const ElButtonStub = defineComponent({
  setup(_, { slots }) {
    return () => h('button', slots.default?.())
  },
})

const ElFormItemStub = defineComponent({
  setup(_, { slots }) {
    return () => h('div', slots.default?.())
  },
})

const ElSelectStub = defineComponent({
  setup(_, { slots }) {
    return () => h('select', slots.default?.())
  },
})

const ElOptionStub = defineComponent({
  setup() {
    return () => h('option')
  },
})

const ElTagStub = defineComponent({
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const ElCardStub = defineComponent({
  setup(_, { slots }) {
    return () => h('section', slots.default?.())
  },
})

const ElTableStub = defineComponent({
  setup(_, { attrs }) {
    return () => h('div', { 'data-testid': attrs['data-testid'] ?? 'billing-table-stub' })
  },
})

const ElTableColumnStub = defineComponent({
  setup() {
    return () => null
  },
})

const ElPaginationStub = defineComponent({
  setup() {
    return () => h('div', { 'data-testid': 'billing-pagination-stub' })
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('BillingInvoicesView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('mounts without error', async () => {
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
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          FilterForm: FilterFormStub,
          ElAlert: ElAlertStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElFormItem: ElFormItemStub,
          ElOption: ElOptionStub,
          ElSelect: ElSelectStub,
          ElTag: ElTagStub,
          ElTable: ElTableStub,
          ElTableColumn: ElTableColumnStub,
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="billing-invoices-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Billing invoices')
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

    mount(BillingInvoicesView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          FilterForm: FilterFormStub,
          ElAlert: ElAlertStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElFormItem: ElFormItemStub,
          ElOption: ElOptionStub,
          ElSelect: ElSelectStub,
          ElTag: ElTagStub,
          ElTable: ElTableStub,
          ElTableColumn: ElTableColumnStub,
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(listInvoices).toHaveBeenCalledTimes(1)
    expect(listInvoices).toHaveBeenCalledWith({
      document_type: undefined,
      status: undefined,
      page: 1,
      page_size: 20,
    })
  })

  it('shows an error alert when invoice loading fails', async () => {
    vi.mocked(listInvoices).mockRejectedValue(new Error('Invoice load failed'))

    const wrapper = mount(BillingInvoicesView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          FilterForm: FilterFormStub,
          ElAlert: ElAlertStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElFormItem: ElFormItemStub,
          ElOption: ElOptionStub,
          ElSelect: ElSelectStub,
          ElTag: ElTagStub,
          ElTable: ElTableStub,
          ElTableColumn: ElTableColumnStub,
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="billing-invoices-error-alert"]').text()).toContain('Invoice records unavailable Invoice load failed')
  })
})
