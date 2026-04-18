import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import GeneralizeReportsView from './GeneralizeReportsView.vue'

vi.mock('../api/reports', () => ({
  exportReport: vi.fn(),
  queryReport: vi.fn(),
}))

import { queryReport } from '../api/reports'

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
    return () => h('section', { 'data-testid': 'generalize-filter-form-stub' }, props.title)
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props) {
    return () => h('div', { 'data-testid': 'generalize-error-alert' }, `${props.title} ${props.description}`.trim())
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

const ElInputStub = defineComponent({
  setup() {
    return () => h('input')
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
    return () => h('div', { 'data-testid': attrs['data-testid'] ?? 'generalize-table-stub' })
  },
})

const ElTableColumnStub = defineComponent({
  setup() {
    return () => null
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('GeneralizeReportsView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('mounts without error', async () => {
    vi.mocked(queryReport).mockResolvedValue({
      data: {
        report_id: 'r01',
        columns: [],
        rows: [],
        generated_at: '',
      },
    } as never)

    const wrapper = mount(GeneralizeReportsView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          FilterForm: FilterFormStub,
          ElAlert: ElAlertStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElFormItem: ElFormItemStub,
          ElInput: ElInputStub,
          ElOption: ElOptionStub,
          ElSelect: ElSelectStub,
          ElTag: ElTagStub,
          ElTable: ElTableStub,
          ElTableColumn: ElTableColumnStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="generalize-reports-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Generalize reports')
  })

  it('queries the default report on mount', async () => {
    vi.mocked(queryReport).mockResolvedValue({
      data: {
        report_id: 'r01',
        columns: [],
        rows: [],
        generated_at: '',
      },
    } as never)

    mount(GeneralizeReportsView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          FilterForm: FilterFormStub,
          ElAlert: ElAlertStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElFormItem: ElFormItemStub,
          ElInput: ElInputStub,
          ElOption: ElOptionStub,
          ElSelect: ElSelectStub,
          ElTag: ElTagStub,
          ElTable: ElTableStub,
          ElTableColumn: ElTableColumnStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(queryReport).toHaveBeenCalledTimes(1)
    expect(queryReport).toHaveBeenCalledWith('r01', {
      period: new Date().toISOString().slice(0, 7),
    })
  })

  it('shows an error alert when report loading fails', async () => {
    vi.mocked(queryReport).mockRejectedValue(new Error('Report request failed'))

    const wrapper = mount(GeneralizeReportsView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          FilterForm: FilterFormStub,
          ElAlert: ElAlertStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElFormItem: ElFormItemStub,
          ElInput: ElInputStub,
          ElOption: ElOptionStub,
          ElSelect: ElSelectStub,
          ElTag: ElTagStub,
          ElTable: ElTableStub,
          ElTableColumn: ElTableColumnStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="generalize-error-alert"]').text()).toContain('Generalize report request failed Report request failed')
  })
})
