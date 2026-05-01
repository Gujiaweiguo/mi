import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import SalesAdminView from './SalesAdminView.vue'

vi.mock('../api/sales', () => ({
  createCustomerTraffic: vi.fn(),
  createDailySale: vi.fn(),
  downloadCustomerTrafficTemplate: vi.fn(),
  downloadDailySalesTemplate: vi.fn(),
  importCustomerTrafficWorkbook: vi.fn(),
  importDailySalesWorkbook: vi.fn(),
  listCustomerTraffic: vi.fn(),
  listDailySales: vi.fn(),
}))

vi.mock('../api/structure', () => ({
  listStructureStores: vi.fn(),
  listStructureUnits: vi.fn(),
}))

vi.mock('../composables/useDownload', () => ({
  downloadBlob: vi.fn(),
}))

vi.mock('../composables/useErrorMessage', () => ({
  getErrorMessage: vi.fn((error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)),
}))

import {
  createCustomerTraffic,
  createDailySale,
  downloadCustomerTrafficTemplate,
  downloadDailySalesTemplate,
  importCustomerTrafficWorkbook,
  importDailySalesWorkbook,
  listCustomerTraffic,
  listDailySales,
} from '../api/sales'
import { listStructureStores, listStructureUnits } from '../api/structure'
import { downloadBlob } from '../composables/useDownload'

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () =>
      h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary), h('div', slots.actions?.())])
  },
})

const FilterFormStub = defineComponent({
  props: {
    title: { type: String, required: true },
    showActions: { type: Boolean, default: true },
  },
  setup(props, { slots }) {
    return () => h('section', [h('h2', props.title), slots.default?.()])
  },
})

const ElFormStub = defineComponent({
  name: 'ElForm',
  setup(_, { slots, expose }) {
    expose({ validate: () => Promise.resolve(true) })
    return () => h('form', slots.default?.())
  },
})

const ElFormItemStub = defineComponent({
  name: 'ElFormItem',
  setup(_, { slots }) {
    return () => h('div', slots.default?.())
  },
})

const ElSelectStub = defineComponent({
  name: 'ElSelect',
  inheritAttrs: false,
  props: {
    modelValue: { type: [String, Number], default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'select',
        {
          ...attrs,
          value: props.modelValue ?? '',
          onChange: (event: Event) => {
            const rawValue = (event.target as HTMLSelectElement).value
            const testId = String(attrs['data-testid'] ?? '')
            const nextValue = testId.includes('-filter-') || rawValue === '' ? rawValue : Number(rawValue)
            emit('update:modelValue', nextValue)
          },
        },
        [h('option', { value: '' }, ''), slots.default?.()],
      )
  },
})

const ElOptionStub = defineComponent({
  name: 'ElOption',
  props: {
    label: { type: String, default: '' },
    value: { type: [String, Number], default: '' },
  },
  setup(props) {
    return () => h('option', { value: props.value }, props.label)
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
        value: props.modelValue,
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElInputNumberStub = defineComponent({
  name: 'ElInputNumber',
  inheritAttrs: false,
  props: {
    modelValue: { type: Number, default: undefined },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('div', { ...attrs }, [
        h('input', {
          type: 'number',
          value: props.modelValue ?? '',
          onInput: (event: Event) => {
            const rawValue = (event.target as HTMLInputElement).value
            emit('update:modelValue', rawValue === '' ? undefined : Number(rawValue))
          },
        }),
      ])
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
          disabled: props.disabled || props.loading,
          onClick: () => emit('click'),
        },
        slots.default?.(),
      )
  },
})

const ElUploadStub = defineComponent({
  name: 'ElUpload',
  inheritAttrs: false,
  props: {
    onChange: { type: Function, default: undefined },
  },
  setup(props, { attrs, slots }) {
    return () =>
      h('div', [
        slots.trigger?.(),
        h('input', {
          ...attrs,
          type: 'file',
          onChange: (event: Event) => {
            const file = (event.target as HTMLInputElement).files?.[0]
            props.onChange?.({ raw: file })
          },
        }),
      ])
  },
})

const ElAlertStub = defineComponent({
  name: 'ElAlert',
  inheritAttrs: false,
  props: {
    title: { type: String, default: '' },
    type: { type: String, default: 'info' },
    description: { type: String, default: '' },
  },
  setup(props, { attrs }) {
    return () => h('div', { ...attrs }, [h('strong', props.title), h('span', props.type), h('p', props.description)])
  },
})

const ElTagStub = defineComponent({
  name: 'ElTag',
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const ElCardStub = defineComponent({
  name: 'ElCard',
  setup(_, { slots }) {
    return () => h('section', [h('header', slots.header?.()), slots.default?.()])
  },
})

const ElTableStub = defineComponent({
  name: 'ElTable',
  inheritAttrs: false,
  props: {
    data: { type: Array, default: () => [] },
  },
  setup(props, { attrs, slots }) {
    return () =>
      h('div', { ...attrs }, [
        h(
          'div',
          { 'data-testid': `${String(attrs['data-testid'] ?? 'table')}-rows` },
          (props.data as Array<Record<string, unknown>>).map((row, index) => h('div', { key: index }, Object.values(row).join(' | '))),
        ),
        slots.default?.({ row: (props.data as Array<Record<string, unknown>>)[0] }),
      ])
  },
})

const ElTableColumnStub = defineComponent({
  name: 'ElTableColumn',
  setup(_, { slots }) {
    return () => h('div', slots.default?.({ row: {} }))
  },
})

const stores = [
  {
    id: 1,
    department_id: 10,
    store_type_id: 20,
    management_type_id: 30,
    code: 'STORE-001',
    name: 'North Plaza',
    short_name: 'North',
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
  {
    id: 2,
    department_id: 11,
    store_type_id: 21,
    management_type_id: 31,
    code: 'STORE-002',
    name: 'South Plaza',
    short_name: 'South',
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const units = [
  {
    id: 101,
    building_id: 1,
    floor_id: 1,
    location_id: 1,
    area_id: 1,
    unit_type_id: 1,
    shop_type_id: null,
    code: 'UNIT-101',
    floor_area: 120,
    use_area: 110,
    rent_area: 100,
    is_rentable: true,
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
  {
    id: 202,
    building_id: 2,
    floor_id: 2,
    location_id: 2,
    area_id: 2,
    unit_type_id: 1,
    shop_type_id: null,
    code: 'UNIT-202',
    floor_area: 220,
    use_area: 210,
    rent_area: 200,
    is_rentable: true,
    status: 'active',
    created_at: '2026-01-01T00:00:00Z',
    updated_at: '2026-01-01T00:00:00Z',
  },
]

const baseDailySales = [
  {
    id: 1,
    store_id: 1,
    unit_id: 101,
    sale_date: '2026-04-10',
    sales_amount: 8888.5,
    created_at: '2026-04-10T00:00:00Z',
    updated_at: '2026-04-10T00:00:00Z',
  },
]

const baseTraffic = [
  {
    id: 10,
    store_id: 1,
    traffic_date: '2026-04-10',
    inbound_count: 320,
    created_at: '2026-04-10T00:00:00Z',
    updated_at: '2026-04-10T00:00:00Z',
  },
]

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(SalesAdminView, {
    global: {
      plugins: [ElementPlus, i18n, createPinia()],
      stubs: {
        PageSection: PageSectionStub,
        FilterForm: FilterFormStub,
        ElForm: ElFormStub,
        ElFormItem: ElFormItemStub,
        ElSelect: ElSelectStub,
        ElOption: ElOptionStub,
        ElDatePicker: ElDatePickerStub,
        ElInputNumber: ElInputNumberStub,
        ElButton: ElButtonStub,
        ElUpload: ElUploadStub,
        ElAlert: ElAlertStub,
        ElTag: ElTagStub,
        ElCard: ElCardStub,
        ElTable: ElTableStub,
        ElTableColumn: ElTableColumnStub,
      },
    },
  })

  await flushPromises()

  return wrapper
}

describe('SalesAdminView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'

    vi.mocked(listStructureStores).mockResolvedValue({ data: { stores } } as never)
    vi.mocked(listStructureUnits).mockResolvedValue({ data: { units } } as never)
    vi.mocked(listDailySales).mockImplementation(async () => ({ data: { daily_sales: baseDailySales } }) as never)
    vi.mocked(listCustomerTraffic).mockImplementation(async () => ({ data: { customer_traffic: baseTraffic } }) as never)

    vi.mocked(createDailySale).mockResolvedValue({
      data: {
        daily_sale: {
          id: 2,
          store_id: 2,
          unit_id: 202,
          sale_date: '2026-04-11',
          sales_amount: 9999.99,
          created_at: '2026-04-11T00:00:00Z',
          updated_at: '2026-04-11T00:00:00Z',
        },
      },
    } as never)

    vi.mocked(createCustomerTraffic).mockResolvedValue({
      data: {
        traffic: {
          id: 11,
          store_id: 2,
          traffic_date: '2026-04-11',
          inbound_count: 456,
          created_at: '2026-04-11T00:00:00Z',
          updated_at: '2026-04-11T00:00:00Z',
        },
      },
    } as never)

    vi.mocked(downloadDailySalesTemplate).mockResolvedValue({ data: new Uint8Array([1, 2, 3]) } as never)
    vi.mocked(downloadCustomerTrafficTemplate).mockResolvedValue({ data: new Uint8Array([4, 5, 6]) } as never)
    vi.mocked(importDailySalesWorkbook).mockResolvedValue({
      status: 200,
      data: { imported_count: 2, diagnostics: [] },
    } as never)
    vi.mocked(importCustomerTrafficWorkbook).mockResolvedValue({
      status: 400,
      data: {
        imported_count: 1,
        diagnostics: [{ row: 3, field: 'inbound_count', message: 'Inbound count is invalid' }],
      },
    } as never)
  })

  it('renders initial data and loads all datasets on mount', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="sales-admin-view"]')).toBeTruthy()
    expect(listStructureStores).toHaveBeenCalledTimes(1)
    expect(listStructureUnits).toHaveBeenCalledTimes(1)
    expect(listDailySales).toHaveBeenCalledWith({
      store_id: undefined,
      unit_id: undefined,
      date_from: undefined,
      date_to: undefined,
    })
    expect(listCustomerTraffic).toHaveBeenCalledWith({
      store_id: undefined,
      date_from: undefined,
      date_to: undefined,
    })
    expect(wrapper.text()).toContain('North Plaza')
    expect(wrapper.text()).toContain('UNIT-101')
    expect(wrapper.text()).toContain('8888.5')
    expect(wrapper.text()).toContain('320')
  })

  it('submits and resets daily sales filters', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="sales-daily-filter-reset"]').attributes('disabled')).toBeDefined()

    vi.mocked(listDailySales).mockClear()

    await wrapper.get('[data-testid="sales-daily-filter-store"]').setValue('2')
    await wrapper.get('[data-testid="sales-daily-filter-unit"]').setValue('202')
    await wrapper.get('[data-testid="sales-daily-filter-date-from"] input').setValue('2026-04-01')
    await wrapper.get('[data-testid="sales-daily-filter-date-to"] input').setValue('2026-04-30')
    await flushPromises()

    expect(wrapper.get('[data-testid="sales-daily-filter-reset"]').attributes('disabled')).toBeUndefined()

    await wrapper.get('[data-testid="sales-daily-filter-submit"]').trigger('click')
    await flushPromises()

    expect(listDailySales).toHaveBeenLastCalledWith({
      store_id: 2,
      unit_id: 202,
      date_from: '2026-04-01',
      date_to: '2026-04-30',
    })

    await wrapper.get('[data-testid="sales-daily-filter-reset"]').trigger('click')
    await flushPromises()

    expect(listDailySales).toHaveBeenLastCalledWith({
      store_id: undefined,
      unit_id: undefined,
      date_from: undefined,
      date_to: undefined,
    })
  })

  it('validates and creates daily sales records', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="sales-daily-create-button"]').trigger('click')
    await flushPromises()

    expect(createDailySale).not.toHaveBeenCalled()
    expect(wrapper.get('[data-testid="sales-daily-feedback"]')).toBeTruthy()

    await wrapper.get('[data-testid="sales-daily-store-select"]').setValue('2')
    await wrapper.get('[data-testid="sales-daily-unit-select"]').setValue('202')
    await wrapper.get('[data-testid="sales-daily-date-input"] input').setValue('2026-04-11')
    await wrapper.get('[data-testid="sales-daily-amount-input"] input').setValue('9999.99')
    await flushPromises()

    await wrapper.get('[data-testid="sales-daily-create-button"]').trigger('click')
    await flushPromises()

    expect(createDailySale).toHaveBeenCalledWith({
      store_id: 2,
      unit_id: 202,
      sale_date: '2026-04-11',
      sales_amount: 9999.99,
    })
    expect(wrapper.text()).toContain('9999.99')
    expect((wrapper.get('[data-testid="sales-daily-store-select"]').element as HTMLSelectElement).value).toBe('')
    expect((wrapper.get('[data-testid="sales-daily-amount-input"] input').element as HTMLInputElement).value).toBe('')
  })

  it('downloads and imports daily sales workbooks', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="sales-daily-download-template"]').trigger('click')
    await flushPromises()

    expect(downloadDailySalesTemplate).toHaveBeenCalledTimes(1)
    expect(downloadBlob).toHaveBeenCalledTimes(1)
    expect(vi.mocked(downloadBlob).mock.calls[0]?.[1]).toBe('daily-sales-template.xlsx')

    const file = new File(['daily'], 'daily-sales.xlsx', { type: 'application/vnd.ms-excel' })
    const dailyUploadInput = wrapper.get('[data-testid="sales-daily-upload-input"]').element as HTMLInputElement
    Object.defineProperty(dailyUploadInput, 'files', {
      value: [file],
      configurable: true,
    })
    await wrapper.get('[data-testid="sales-daily-upload-input"]').trigger('change')
    await flushPromises()

    expect(wrapper.text()).toContain('daily-sales.xlsx')

    vi.mocked(listDailySales).mockClear()
    await wrapper.get('[data-testid="sales-daily-import-button"]').trigger('click')
    await flushPromises()

    expect(importDailySalesWorkbook).toHaveBeenCalledWith(file)
    expect(listDailySales).toHaveBeenCalledTimes(1)
    expect(wrapper.get('[data-testid="sales-daily-import-result"]')).toBeTruthy()
    expect(wrapper.text()).toContain('2')
  })

  it('submits traffic filters and creates traffic records', async () => {
    const wrapper = await mountView()

    vi.mocked(listCustomerTraffic).mockClear()

    await wrapper.get('[data-testid="sales-traffic-filter-store"]').setValue('2')
    await wrapper.get('[data-testid="sales-traffic-filter-date-from"] input').setValue('2026-04-01')
    await wrapper.get('[data-testid="sales-traffic-filter-date-to"] input').setValue('2026-04-30')
    await flushPromises()

    await wrapper.get('[data-testid="sales-traffic-filter-submit"]').trigger('click')
    await flushPromises()

    expect(listCustomerTraffic).toHaveBeenLastCalledWith({
      store_id: 2,
      date_from: '2026-04-01',
      date_to: '2026-04-30',
    })

    await wrapper.get('[data-testid="sales-traffic-create-button"]').trigger('click')
    await flushPromises()

    expect(createCustomerTraffic).not.toHaveBeenCalled()
    expect(wrapper.get('[data-testid="sales-traffic-feedback"]')).toBeTruthy()

    await wrapper.get('[data-testid="sales-traffic-store-select"]').setValue('2')
    await wrapper.get('[data-testid="sales-traffic-date-input"] input').setValue('2026-04-11')
    await wrapper.get('[data-testid="sales-traffic-count-input"] input').setValue('456')
    await flushPromises()

    await wrapper.get('[data-testid="sales-traffic-create-button"]').trigger('click')
    await flushPromises()

    expect(createCustomerTraffic).toHaveBeenCalledWith({
      store_id: 2,
      traffic_date: '2026-04-11',
      inbound_count: 456,
    })
    expect(wrapper.text()).toContain('456')
  })

  it('downloads and imports traffic workbooks with diagnostics feedback', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="sales-traffic-download-template"]').trigger('click')
    await flushPromises()

    expect(downloadCustomerTrafficTemplate).toHaveBeenCalledTimes(1)
    expect(vi.mocked(downloadBlob).mock.calls[0]?.[1]).toBe('customer-traffic-template.xlsx')

    const file = new File(['traffic'], 'traffic.xlsx', { type: 'application/vnd.ms-excel' })
    const trafficUploadInput = wrapper.get('[data-testid="sales-traffic-upload-input"]').element as HTMLInputElement
    Object.defineProperty(trafficUploadInput, 'files', {
      value: [file],
      configurable: true,
    })
    await wrapper.get('[data-testid="sales-traffic-upload-input"]').trigger('change')
    await flushPromises()

    vi.mocked(listCustomerTraffic).mockClear()
    await wrapper.get('[data-testid="sales-traffic-import-button"]').trigger('click')
    await flushPromises()

    expect(importCustomerTrafficWorkbook).toHaveBeenCalledWith(file)
    expect(listCustomerTraffic).not.toHaveBeenCalled()
    expect(wrapper.get('[data-testid="sales-traffic-import-result"]')).toBeTruthy()
    expect(wrapper.get('[data-testid="sales-traffic-diagnostics-table"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Inbound count is invalid')
  })
})
