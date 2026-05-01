import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, inject, nextTick, provide } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import ReceivablesView from './ReceivablesView.vue'

const push = vi.fn()

vi.mock('vue-router', () => ({
  useRouter: () => ({ push }),
}))

vi.mock('../api/invoice', () => ({
  listReceivables: vi.fn(),
}))

import { listReceivables } from '../api/invoice'

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
    title: { type: String, required: true },
    busy: { type: Boolean, default: false },
    resetDisabled: { type: Boolean, default: false },
  },
  emits: ['reset', 'submit'],
  setup(props, { slots, emit }) {
    return () =>
      h('section', { 'data-testid': 'filter-form-stub' }, [
        h('h2', props.title),
        slots.default?.(),
        h(
          'button',
          {
            type: 'button',
            'data-testid': 'receivables-filter-submit',
            disabled: props.busy,
            onClick: () => emit('submit'),
          },
          'Submit',
        ),
        h(
          'button',
          {
            type: 'button',
            'data-testid': 'receivables-filter-reset',
            disabled: props.resetDisabled,
            onClick: () => emit('reset'),
          },
          'Reset',
        ),
      ])
  },
})

const ElFormItemStub = defineComponent({
  name: 'ElFormItem',
  setup(_, { slots }) {
    return () => h('div', slots.default?.())
  },
})

const ElInputStub = defineComponent({
  name: 'ElInput',
  inheritAttrs: false,
  props: {
    modelValue: { type: [String, Number], default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        value: props.modelValue ?? '',
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElDatePickerStub = defineComponent({
  name: 'ElDatePicker',
  inheritAttrs: false,
  props: {
    modelValue: { type: String, default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        type: 'date',
        value: props.modelValue ?? '',
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElAlertStub = defineComponent({
  name: 'ElAlert',
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props, { attrs }) {
    return () => h('div', { ...attrs }, [h('strong', props.title), h('span', props.description)])
  },
})

const ElTagStub = defineComponent({
  name: 'ElTag',
  setup(_, { slots, attrs }) {
    return () => h('span', attrs, slots.default?.())
  },
})

const ElButtonStub = defineComponent({
  name: 'ElButton',
  inheritAttrs: false,
  props: {
    disabled: { type: Boolean, default: false },
    loading: { type: Boolean, default: false },
  },
  emits: ['click'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'button',
        {
          ...attrs,
          type: 'button',
          disabled: props.disabled || props.loading,
          onClick: (event: MouseEvent) => emit('click', event),
        },
        slots.default?.(),
      )
  },
})

const ElCardStub = defineComponent({
  name: 'ElCard',
  setup(_, { slots, attrs }) {
    return () => h('section', attrs, [h('header', slots.header?.()), slots.default?.()])
  },
})

const tableRowsKey = Symbol('table-rows')

const ElTableStub = defineComponent({
  name: 'ElTable',
  props: {
    data: { type: Array, default: () => [] },
  },
  setup(props, { slots, attrs }) {
    provide(tableRowsKey, props)
    return () => h('section', attrs, slots.default?.())
  },
})

const ElTableColumnStub = defineComponent({
  name: 'ElTableColumn',
  props: {
    prop: { type: String, default: '' },
    label: { type: String, default: '' },
  },
  setup(props, { slots }) {
    const table = inject<{ data: Array<Record<string, unknown>> }>(tableRowsKey)

    return () =>
      h(
        'div',
        { 'data-testid': `table-column-${props.prop || props.label}` },
        (table?.data ?? []).map((row, index) =>
          h(
            'div',
            { 'data-testid': `table-cell-${props.prop || props.label}-${index}` },
            slots.default ? slots.default({ row, $index: index }) : String(props.prop ? row[props.prop] ?? '' : ''),
          ),
        ),
      )
  },
})

const ElPaginationStub = defineComponent({
  name: 'ElPagination',
  props: {
    currentPage: { type: Number, default: 1 },
    pageSize: { type: Number, default: 20 },
    total: { type: Number, default: 0 },
  },
  emits: ['update:currentPage', 'update:pageSize', 'current-change', 'size-change'],
  setup(props, { emit }) {
    return () =>
      h('div', { 'data-testid': 'receivables-pagination' }, [
        h('span', { 'data-testid': 'receivables-pagination-state' }, `${props.currentPage}:${props.pageSize}:${props.total}`),
        h(
          'button',
          {
            type: 'button',
            'data-testid': 'receivables-pagination-page-2',
            onClick: () => {
              emit('update:currentPage', 2)
              emit('current-change', 2)
            },
          },
          'Page 2',
        ),
        h(
          'button',
          {
            type: 'button',
            'data-testid': 'receivables-pagination-size-50',
            onClick: () => {
              emit('update:pageSize', 50)
              emit('size-change', 50)
            },
          },
          'Size 50',
        ),
      ])
  },
})

const receivableRows = [
  {
    billing_document_id: 101,
    document_type: 'invoice',
    document_no: 'INV-101',
    tenant_name: 'Alpha Retail',
    document_status: 'approved',
    lease_contract_id: 31,
    customer_id: 12,
    department_id: 7,
    trade_id: null,
    earliest_due_date: '2026-04-01',
    latest_due_date: '2026-04-15',
    outstanding_amount: 1234.5,
    settlement_status: 'outstanding' as const,
  },
  {
    billing_document_id: 202,
    document_type: 'bill',
    document_no: 'BILL-202',
    tenant_name: 'Beta Foods',
    document_status: 'pending_approval',
    lease_contract_id: 32,
    customer_id: 15,
    department_id: 9,
    trade_id: 2,
    earliest_due_date: '2026-05-01',
    latest_due_date: '2026-05-31',
    outstanding_amount: 200,
    settlement_status: 'settled' as const,
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 4; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const pinia = createPinia()
  setActivePinia(pinia)

  const wrapper = mount(ReceivablesView, {
    global: {
      plugins: [ElementPlus, i18n, pinia],
      stubs: {
        PageSection: PageSectionStub,
        FilterForm: FilterFormStub,
        ElFormItem: ElFormItemStub,
        ElInput: ElInputStub,
        ElDatePicker: ElDatePickerStub,
        ElAlert: ElAlertStub,
        ElTag: ElTagStub,
        ElButton: ElButtonStub,
        ElCard: ElCardStub,
        ElTable: ElTableStub,
        ElTableColumn: ElTableColumnStub,
        ElPagination: ElPaginationStub,
      },
    },
  })

  await flushPromises()
  return wrapper
}

describe('ReceivablesView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    i18n.global.locale.value = 'en-US'

    const pinia = createPinia()
    setActivePinia(pinia)
    useAppStore().setLocale('en-US')

    vi.mocked(listReceivables).mockResolvedValue({
      data: {
        items: receivableRows,
        total: receivableRows.length,
        page: 1,
        page_size: 20,
      },
    } as never)
  })

  it('loads receivables on mount and renders the table contents', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="receivables-view"]')).toBeTruthy()
    expect(listReceivables).toHaveBeenCalledTimes(1)
    expect(listReceivables).toHaveBeenCalledWith({
      customer_id: undefined,
      department_id: undefined,
      due_date_start: undefined,
      due_date_end: undefined,
      page: 1,
      page_size: 20,
    })

    expect(wrapper.text()).toContain('INV-101')
    expect(wrapper.text()).toContain('BILL-202')
    expect(wrapper.text()).toContain('Alpha Retail')
    expect(wrapper.text()).toContain('Invoice')
    expect(wrapper.text()).toContain('Bill')
    expect(wrapper.text()).toContain('Approved')
    expect(wrapper.text()).toContain('Pending approval')
    expect(wrapper.text()).toContain('Outstanding')
    expect(wrapper.text()).toContain('Settled')
    expect(wrapper.text()).toContain('1,234.50')
    expect(wrapper.text()).toContain('2026-04-01 → 2026-04-15')
  })

  it('submits parsed filters, resets them, and blocks invalid due date ranges', async () => {
    const wrapper = await mountView()

    const resetButton = wrapper.get('[data-testid="receivables-filter-reset"]')
    expect(resetButton.attributes('disabled')).toBeDefined()

    vi.mocked(listReceivables).mockClear()

    await wrapper.get('[data-testid="receivables-customer-id-filter"]').setValue('12')
    await wrapper.get('[data-testid="receivables-department-id-filter"]').setValue('7')
    await wrapper.get('[data-testid="receivables-due-date-start-filter"]').setValue('2026-04-01')
    await wrapper.get('[data-testid="receivables-due-date-end-filter"]').setValue('2026-04-30')
    await flushPromises()

    expect(wrapper.get('[data-testid="receivables-filter-reset"]').attributes('disabled')).toBeUndefined()

    await wrapper.get('[data-testid="receivables-filter-submit"]').trigger('click')
    await flushPromises()

    expect(listReceivables).toHaveBeenCalledWith({
      customer_id: 12,
      department_id: 7,
      due_date_start: '2026-04-01',
      due_date_end: '2026-04-30',
      page: 1,
      page_size: 20,
    })

    vi.mocked(listReceivables).mockClear()

    await wrapper.get('[data-testid="receivables-filter-reset"]').trigger('click')
    await flushPromises()

    expect(listReceivables).toHaveBeenCalledWith({
      customer_id: undefined,
      department_id: undefined,
      due_date_start: undefined,
      due_date_end: undefined,
      page: 1,
      page_size: 20,
    })
    expect((wrapper.get('[data-testid="receivables-customer-id-filter"]').element as HTMLInputElement).value).toBe('')
    expect((wrapper.get('[data-testid="receivables-department-id-filter"]').element as HTMLInputElement).value).toBe('')

    vi.mocked(listReceivables).mockClear()

    await wrapper.get('[data-testid="receivables-due-date-start-filter"]').setValue('2026-05-31')
    await wrapper.get('[data-testid="receivables-due-date-end-filter"]').setValue('2026-05-01')
    await flushPromises()

    await wrapper.get('[data-testid="receivables-filter-submit"]').trigger('click')
    await flushPromises()

    expect(listReceivables).not.toHaveBeenCalled()
    expect(wrapper.get('[data-testid="receivables-error-alert"]').text()).toContain(
      'The due date start must be earlier than or equal to the due date end.',
    )
  })

  it('navigates to the invoice detail page from a row action', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="receivable-row-view-button-101"]').trigger('click')

    expect(push).toHaveBeenCalledWith({
      name: 'billing-invoice-detail',
      params: { id: '101' },
    })
  })

  it('reloads data when pagination changes', async () => {
    const wrapper = await mountView()

    vi.mocked(listReceivables).mockClear()

    await wrapper.get('[data-testid="receivables-pagination-page-2"]').trigger('click')
    await flushPromises()

    expect(listReceivables).toHaveBeenCalledWith({
      customer_id: undefined,
      department_id: undefined,
      due_date_start: undefined,
      due_date_end: undefined,
      page: 2,
      page_size: 20,
    })

    vi.mocked(listReceivables).mockClear()

    await wrapper.get('[data-testid="receivables-pagination-size-50"]').trigger('click')
    await flushPromises()

    expect(listReceivables).toHaveBeenCalledWith({
      customer_id: undefined,
      department_id: undefined,
      due_date_start: undefined,
      due_date_end: undefined,
      page: 1,
      page_size: 50,
    })
  })
})
