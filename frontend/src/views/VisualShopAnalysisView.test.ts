import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi, afterEach } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import VisualShopAnalysisView from './VisualShopAnalysisView.vue'

vi.mock('../api/reports', () => ({
  exportReport: vi.fn(),
  queryReport: vi.fn(),
}))

vi.mock('../composables/useDownload', () => ({
  downloadBlob: vi.fn(),
}))

import { exportReport, queryReport } from '../api/reports'
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
    title: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () => h('section', { 'data-testid': 'visual-shop-filter-form-stub' }, [h('h2', props.title), slots.default?.()])
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props) {
    return () => h('div', { 'data-testid': 'visual-shop-error-alert' }, `${props.title} ${props.description}`.trim())
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

const ElCardStub = defineComponent({
  name: 'ElCard',
  setup(_, { slots }) {
    return () => h('section', [h('header', slots.header?.()), slots.default?.()])
  },
})

const ElTagStub = defineComponent({
  name: 'ElTag',
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const baseReport = {
  report_id: 'r19',
  columns: [],
  rows: [],
  generated_at: '2026-01-03T12:34:00Z',
  visual: {
    floor: {
      id: 12,
      name: 'Level 2',
      floor_plan_image_url: 'https://example.com/floor-l2.png',
    },
    units: [
      {
        unit_id: 101,
        unit_code: 'A-01',
        unit_name: 'Alpha Boutique',
        floor_area: 120.5,
        rent_area: 100,
        rent_status: 'Leased',
        brand_name: 'Alpha',
        customer_name: 'ACME Retail',
        shop_type_name: 'Fashion',
        pos_x: 10,
        pos_y: 20,
        color_hex: '#123456',
      },
      {
        unit_id: 102,
        unit_code: 'B-02',
        unit_name: 'Beta Corner',
        floor_area: null,
        rent_area: null,
        rent_status: 'Vacant',
        brand_name: null,
        customer_name: null,
        shop_type_name: null,
        pos_x: 30,
        pos_y: 60,
        color_hex: null,
      },
    ],
    legend: [],
  },
} as const

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const pinia = createPinia()
  setActivePinia(pinia)
  const appStore = useAppStore()
  appStore.setLocale('en-US')

  const wrapper = mount(VisualShopAnalysisView, {
    global: {
      plugins: [pinia, i18n, ElementPlus],
      stubs: {
        PageSection: PageSectionStub,
        FilterForm: FilterFormStub,
        ElAlert: ElAlertStub,
        ElButton: ElButtonStub,
        ElFormItem: ElFormItemStub,
        ElInput: ElInputStub,
        ElCard: ElCardStub,
        ElTag: ElTagStub,
      },
    },
  })

  await flushPromises()

  return wrapper
}

describe('VisualShopAnalysisView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    i18n.global.locale.value = 'en-US'
  })

  afterEach(() => {
    vi.useRealTimers()
  })

  it('renders the default empty state without auto-loading report data', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="visual-shop-analysis-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Visual shop analysis')
    expect(queryReport).not.toHaveBeenCalled()
    expect(wrapper.findAll('[data-testid="visual-unit-marker"]').length).toBe(0)
    expect(wrapper.get('[data-testid="visual-shop-selected-unit-code"]').text()).toBe('—')
    expect(wrapper.text()).toContain('Legend will appear once the report returns visual items.')
  })

  it('queries report data with sanitized filters and renders markers, legend, and details', async () => {
    vi.mocked(queryReport).mockResolvedValue({ data: baseReport } as never)

    const wrapper = await mountView()

    await wrapper.get('[data-testid="visual-shop-store-input"]').setValue(' 7 ')
    await wrapper.get('[data-testid="visual-shop-floor-input"]').setValue('0')
    await wrapper.get('[data-testid="visual-shop-area-input"]').setValue('12.5')
    await flushPromises()

    await wrapper.get('[data-testid="visual-shop-query-button"]').trigger('click')
    await flushPromises()

    expect(queryReport).toHaveBeenCalledWith('r19', { store_id: 7 })
    expect(wrapper.findAll('[data-testid="visual-unit-marker"]').length).toBe(2)
    expect(wrapper.get('[data-testid="visual-shop-selected-unit-code"]').text()).toBe('A-01')
    expect(wrapper.get('[data-testid="visual-shop-canvas"]').attributes('style')).toContain(
      'background-image: url("https://example.com/floor-l2.png")',
    )
    expect(wrapper.text()).toContain('Alpha Boutique')
    expect(wrapper.text()).toContain('120.5 ㎡')
    expect(wrapper.text()).toContain('100 ㎡')
    expect(wrapper.get('[data-testid="visual-shop-legend"]').text()).toContain('Leased')
    expect(wrapper.get('[data-testid="visual-shop-legend"]').text()).toContain('Vacant')
  })

  it('switches selected markers and falls back for invalid color and empty detail values', async () => {
    vi.mocked(queryReport).mockResolvedValue({
      data: {
        ...baseReport,
        visual: {
          ...baseReport.visual,
          floor: {
            id: 22,
            name: 'Single Point Floor',
            floor_plan_image_url: '',
          },
          units: [
            {
              ...baseReport.visual.units[0],
              unit_id: 201,
              unit_code: 'C-03',
              pos_x: 50,
              pos_y: 50,
              color_hex: 'invalid-color',
            },
            {
              ...baseReport.visual.units[1],
              unit_id: 202,
              pos_x: 50,
              pos_y: 50,
            },
          ],
        },
      },
    } as never)

    const wrapper = await mountView()

    await wrapper.get('[data-testid="visual-shop-query-button"]').trigger('click')
    await flushPromises()

    const markers = wrapper.findAll('[data-testid="visual-unit-marker"]')
    expect(markers[0].attributes('style')).toContain('left: 50%')
    expect(markers[0].attributes('style')).toContain('top: 50%')
    expect(markers[0].attributes('style')).toContain('--visual-unit-color: var(--mi-color-primary)')

    await markers[1].trigger('click')
    await flushPromises()

    expect(wrapper.get('[data-testid="visual-shop-selected-unit-code"]').text()).toBe('B-02')
    expect(markers[1].classes()).toContain('visual-shop-analysis-view__unit-marker--active')
    expect(wrapper.text()).toContain('—')
  })

  it('resets entered filters and clears loaded visual state', async () => {
    vi.mocked(queryReport).mockResolvedValue({ data: baseReport } as never)

    const wrapper = await mountView()

    await wrapper.get('[data-testid="visual-shop-store-input"]').setValue('5')
    await wrapper.get('[data-testid="visual-shop-floor-input"]').setValue('8')
    await wrapper.get('[data-testid="visual-shop-area-input"]').setValue('13')
    await wrapper.get('[data-testid="visual-shop-query-button"]').trigger('click')
    await flushPromises()

    const resetButton = wrapper.findAll('button').find((button) => button.text().includes('Reset'))
    expect(resetButton).toBeTruthy()

    await resetButton!.trigger('click')
    await flushPromises()

    expect((wrapper.get('[data-testid="visual-shop-store-input"]').element as HTMLInputElement).value).toBe('')
    expect((wrapper.get('[data-testid="visual-shop-floor-input"]').element as HTMLInputElement).value).toBe('')
    expect((wrapper.get('[data-testid="visual-shop-area-input"]').element as HTMLInputElement).value).toBe('')
    expect(wrapper.findAll('[data-testid="visual-unit-marker"]').length).toBe(0)
    expect(wrapper.get('[data-testid="visual-shop-selected-unit-code"]').text()).toBe('—')
  })

  it('exports the current filter slice and infers the download file extension', async () => {
    vi.useFakeTimers()
    vi.setSystemTime(new Date('2026-05-01T08:00:00Z'))

    const blob = new Blob(['shop-analysis'], { type: 'text/csv' })
    vi.mocked(exportReport).mockResolvedValue({
      data: blob,
      headers: {
        'content-type': 'text/csv',
      },
    } as never)

    const wrapper = await mountView()

    await wrapper.get('[data-testid="visual-shop-store-input"]').setValue('9')
    await wrapper.get('[data-testid="visual-shop-floor-input"]').setValue('4')
    await wrapper.get('[data-testid="visual-shop-area-input"]').setValue('2')
    await flushPromises()

    await wrapper.get('[data-testid="visual-shop-export-button"]').trigger('click')
    await flushPromises()

    expect(exportReport).toHaveBeenCalledWith('r19', { store_id: 9, floor_id: 4, area_id: 2 })
    expect(downloadBlob).toHaveBeenCalledWith(blob, 'visual-shop-r19-9-4-2-2026-05-01.csv')
  })

  it('shows translated error feedback when query or export fails', async () => {
    vi.mocked(queryReport).mockRejectedValueOnce(new Error('Query failed hard'))
    vi.mocked(exportReport).mockRejectedValueOnce(new Error('Export failed hard'))

    const wrapper = await mountView()

    await wrapper.get('[data-testid="visual-shop-query-button"]').trigger('click')
    await flushPromises()

    expect(wrapper.get('[data-testid="visual-shop-error-alert"]').text()).toContain(
      'Visual shop analysis request failed Query failed hard',
    )

    await wrapper.get('[data-testid="visual-shop-export-button"]').trigger('click')
    await flushPromises()

    expect(wrapper.get('[data-testid="visual-shop-error-alert"]').text()).toContain(
      'Visual shop analysis request failed Export failed hard',
    )
  })
})
