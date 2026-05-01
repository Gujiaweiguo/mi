import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import ExcelIOView from './ExcelIOView.vue'

vi.mock('../api/excel', () => ({
  downloadUnitTemplate: vi.fn(),
  importUnits: vi.fn(),
  exportOperational: vi.fn(),
}))

vi.mock('../composables/useDownload', () => ({
  downloadBlob: vi.fn(),
}))

vi.mock('../composables/useErrorMessage', () => ({
  getErrorMessage: vi.fn((error: unknown, fallback: string) => (error instanceof Error ? error.message : fallback)),
}))

import { downloadUnitTemplate, exportOperational, importUnits } from '../api/excel'
import { downloadBlob } from '../composables/useDownload'

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

const ElSelectStub = defineComponent({
  name: 'ElSelect',
  inheritAttrs: false,
  props: {
    modelValue: { type: String, default: '' },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, slots, emit }) {
    return () =>
      h(
        'select',
        {
          ...attrs,
          value: props.modelValue ?? '',
          onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLSelectElement).value),
        },
        [h('option', { value: '' }, ''), slots.default?.()],
      )
  },
})

const ElOptionStub = defineComponent({
  name: 'ElOption',
  props: {
    label: { type: String, default: '' },
    value: { type: String, default: '' },
  },
  setup(props) {
    return () => h('option', { value: props.value }, props.label)
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

const flushPromises = async () => {
  for (let index = 0; index < 5; index += 1) {
    await Promise.resolve()
    await nextTick()
  }
}

const mountView = async () => {
  const wrapper = mount(ExcelIOView, {
    global: {
      plugins: [ElementPlus, i18n, createPinia()],
      stubs: {
        PageSection: PageSectionStub,
        ElButton: ElButtonStub,
        ElUpload: ElUploadStub,
        ElAlert: ElAlertStub,
        ElTag: ElTagStub,
        ElCard: ElCardStub,
        ElSelect: ElSelectStub,
        ElOption: ElOptionStub,
        ElTable: ElTableStub,
        ElTableColumn: ElTableColumnStub,
      },
    },
  })

  await flushPromises()

  return wrapper
}

describe('ExcelIOView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'

    vi.mocked(downloadUnitTemplate).mockResolvedValue({ data: new Uint8Array([1, 2, 3]) } as never)
    vi.mocked(importUnits).mockResolvedValue({
      data: {
        imported_count: 2,
        diagnostics: [],
      },
    } as never)
    vi.mocked(exportOperational).mockResolvedValue({ data: new Uint8Array([4, 5, 6]) } as never)
  })

  it('renders the initial excel operations state', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="excel-io-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Excel import/export')
    expect(wrapper.text()).toContain('Template download')
    expect(wrapper.text()).toContain('Import data')
    expect(wrapper.text()).toContain('Export dataset')
    expect(wrapper.get('[data-testid="excel-import-button"]').attributes('disabled')).toBeDefined()
    expect(wrapper.get('[data-testid="excel-export-button"]').attributes('disabled')).toBeDefined()
  })

  it('downloads the unit template and shows success feedback', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="excel-download-template"]').trigger('click')
    await flushPromises()

    expect(downloadUnitTemplate).toHaveBeenCalledTimes(1)
    expect(downloadBlob).toHaveBeenCalledTimes(1)
    expect(vi.mocked(downloadBlob).mock.calls[0]?.[1]).toBe('unit-data-template.xlsx')
    expect(wrapper.text()).toContain('Template downloaded')
    expect(wrapper.text()).toContain('The unit data template has been downloaded.')
  })

  it('shows template download errors', async () => {
    vi.mocked(downloadUnitTemplate).mockRejectedValue(new Error('Template API offline'))

    const wrapper = await mountView()

    await wrapper.get('[data-testid="excel-download-template"]').trigger('click')
    await flushPromises()

    expect(downloadBlob).not.toHaveBeenCalled()
    expect(wrapper.text()).toContain('Template download failed')
    expect(wrapper.text()).toContain('Template API offline')
  })

  it('imports unit data and renders diagnostics feedback', async () => {
    vi.mocked(importUnits).mockResolvedValue({
      data: {
        imported_count: 1,
        diagnostics: [{ row: 3, field: 'code', message: 'Duplicate code' }],
      },
    } as never)

    const wrapper = await mountView()
    const file = new File(['unit'], 'unit-data.xlsx', { type: 'application/vnd.ms-excel' })
    const uploadInput = wrapper.get('[data-testid="excel-upload-input"]').element as HTMLInputElement

    Object.defineProperty(uploadInput, 'files', {
      value: [file],
      configurable: true,
    })

    await wrapper.get('[data-testid="excel-upload-input"]').trigger('change')
    await flushPromises()

    expect(wrapper.get('[data-testid="excel-import-button"]').attributes('disabled')).toBeUndefined()

    await wrapper.get('[data-testid="excel-import-button"]').trigger('click')
    await flushPromises()

    expect(importUnits).toHaveBeenCalledWith(file)
    expect(wrapper.text()).toContain('Import completed')
    expect(wrapper.text()).toContain('1 record(s) imported successfully.')
    expect(wrapper.text()).toContain('1 imported')
    expect(wrapper.text()).toContain('3 | code | Duplicate code')
  })

  it('shows import errors', async () => {
    vi.mocked(importUnits).mockRejectedValue(new Error('Workbook parsing failed'))

    const wrapper = await mountView()
    const file = new File(['bad'], 'broken.xlsx', { type: 'application/vnd.ms-excel' })
    const uploadInput = wrapper.get('[data-testid="excel-upload-input"]').element as HTMLInputElement

    Object.defineProperty(uploadInput, 'files', {
      value: [file],
      configurable: true,
    })

    await wrapper.get('[data-testid="excel-upload-input"]').trigger('change')
    await flushPromises()

    await wrapper.get('[data-testid="excel-import-button"]').trigger('click')
    await flushPromises()

    expect(importUnits).toHaveBeenCalledWith(file)
    expect(wrapper.text()).toContain('Import failed')
    expect(wrapper.text()).toContain('Workbook parsing failed')
  })

  it('exports the selected dataset and shows success feedback', async () => {
    const wrapper = await mountView()

    await wrapper.get('[data-testid="excel-export-dataset"]').setValue('billing_charges')
    await flushPromises()

    expect(wrapper.get('[data-testid="excel-export-button"]').attributes('disabled')).toBeUndefined()

    await wrapper.get('[data-testid="excel-export-button"]').trigger('click')
    await flushPromises()

    expect(exportOperational).toHaveBeenCalledWith('billing_charges')
    expect(vi.mocked(downloadBlob).mock.calls[0]?.[1]).toBe('billing_charges-export.xlsx')
    expect(wrapper.text()).toContain('Export completed')
    expect(wrapper.text()).toContain('Dataset billing_charges was exported successfully.')
  })

  it('shows export errors', async () => {
    vi.mocked(exportOperational).mockRejectedValue(new Error('Export timeout'))

    const wrapper = await mountView()

    await wrapper.get('[data-testid="excel-export-dataset"]').setValue('unit_data')
    await flushPromises()

    await wrapper.get('[data-testid="excel-export-button"]').trigger('click')
    await flushPromises()

    expect(exportOperational).toHaveBeenCalledWith('unit_data')
    expect(wrapper.text()).toContain('Export failed')
    expect(wrapper.text()).toContain('Export timeout')
  })
})
