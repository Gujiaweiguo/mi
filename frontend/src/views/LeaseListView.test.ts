import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import LeaseListView from './LeaseListView.vue'

vi.mock('vue-router', () => ({
  useRouter: () => ({ push: vi.fn() }),
}))

vi.mock('../api/lease', () => ({
  listLeases: vi.fn(),
}))

vi.mock('../api/org', () => ({
  listDepartments: vi.fn(),
  listStores: vi.fn(),
}))

import { listLeases } from '../api/lease'
import { listDepartments, listStores } from '../api/org'

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
  setup(props, { slots }) {
    return () => h('section', { 'data-testid': 'lease-filter-form-stub' }, [props.title, slots.default?.()])
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props) {
    return () => h('div', { 'data-testid': 'lease-error-alert' }, `${props.title} ${props.description}`.trim())
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
  props: { modelValue: { type: String, default: '' } },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    return () =>
      h('input', {
        value: props.modelValue,
        onInput: (event: Event) => emit('update:modelValue', (event.target as HTMLInputElement).value),
      })
  },
})

const ElSelectStub = defineComponent({
  props: { modelValue: { type: [String, Number], default: '' } },
  emits: ['update:modelValue'],
  setup(props, { slots, emit }) {
    return () =>
      h('select', {
        value: props.modelValue,
        onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLSelectElement).value),
      }, slots.default?.())
  },
})

const ElOptionStub = defineComponent({
  props: {
    label: { type: String, default: '' },
    value: { type: [String, Number], default: '' },
  },
  setup(props) {
    return () => h('option', { value: props.value }, props.label)
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
    return () => h('div', { 'data-testid': attrs['data-testid'] ?? 'lease-table-stub' })
  },
})

const ElTableColumnStub = defineComponent({
  setup() {
    return () => null
  },
})

const ElPaginationStub = defineComponent({
  setup() {
    return () => h('div', { 'data-testid': 'lease-pagination-stub' })
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('LeaseListView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    vi.mocked(listDepartments).mockResolvedValue({ data: { departments: [{ id: 9, name: 'Leasing Ops' }] } } as never)
    vi.mocked(listStores).mockResolvedValue({ data: { stores: [{ id: 3, name: 'Atrium Store' }] } } as never)
  })

  it('mounts without error', async () => {
    vi.mocked(listLeases).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 20,
      },
    } as never)

    const wrapper = mount(LeaseListView, {
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
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="lease-list-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Lease contracts')
  })

  it('loads lease rows on mount', async () => {
    vi.mocked(listLeases).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 20,
      },
    } as never)

    mount(LeaseListView, {
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
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(listLeases).toHaveBeenCalledTimes(1)
    expect(listLeases).toHaveBeenCalledWith({
      lease_no: undefined,
      status: undefined,
      subtype: undefined,
      department_id: undefined,
      page: 1,
      page_size: 20,
    })
  })

  it('passes subtype and department filters when submitted', async () => {
    vi.mocked(listLeases).mockResolvedValue({
      data: {
        items: [],
        total: 0,
        page: 1,
        page_size: 20,
      },
    } as never)

    const wrapper = mount(LeaseListView, {
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
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()
    vi.mocked(listLeases).mockClear()

    const selects = wrapper.findAll('select')
    await selects[1].setValue('joint_operation')
    await selects[2].setValue('9')
    await flushPromises()

    wrapper.getComponent(FilterFormStub).vm.$emit('submit')
    await flushPromises()

    expect(listLeases).toHaveBeenCalledWith({
      lease_no: undefined,
      status: undefined,
      subtype: 'joint_operation',
      department_id: 9,
      page: 1,
      page_size: 20,
    })
  })

  it('shows an error alert when lease loading fails', async () => {
    vi.mocked(listLeases).mockRejectedValue(new Error('Lease load failed'))

    const wrapper = mount(LeaseListView, {
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
          ElPagination: ElPaginationStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="lease-error-alert"]').text()).toContain('Lease contract records unavailable Lease load failed')
  })
})
