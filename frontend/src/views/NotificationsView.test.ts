import ElementPlus from 'element-plus'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, inject, nextTick, provide } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import NotificationsView from './NotificationsView.vue'

vi.mock('../api/notification', () => ({
  listNotifications: vi.fn(),
}))

import { listNotifications } from '../api/notification'

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
            'data-testid': 'notifications-filter-submit',
            disabled: props.busy,
            onClick: () => emit('submit'),
          },
          'Submit',
        ),
        h(
          'button',
          {
            type: 'button',
            'data-testid': 'notifications-filter-reset',
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
          onChange: (event: Event) => emit('update:modelValue', (event.target as HTMLSelectElement).value),
        },
        slots.default?.(),
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
    modelValue: { type: Array as () => string[] | null, default: null },
  },
  emits: ['update:modelValue'],
  setup(props, { attrs, emit }) {
    return () =>
      h('input', {
        ...attrs,
        value: props.modelValue?.join(',') ?? '',
        onInput: (event: Event) => {
          const rawValue = (event.target as HTMLInputElement).value
          emit('update:modelValue', rawValue ? rawValue.split(',') : null)
        },
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
      h('div', { 'data-testid': 'notifications-pagination' }, [
        h('span', { 'data-testid': 'notifications-pagination-state' }, `${props.currentPage}:${props.pageSize}:${props.total}`),
        h(
          'button',
          {
            type: 'button',
            'data-testid': 'notifications-pagination-page-2',
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
            'data-testid': 'notifications-pagination-size-50',
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

const notificationRows = [
  {
    id: 101,
    event_type: 'lease.approved',
    aggregate_type: 'lease',
    aggregate_id: 9001,
    recipient_to: 'alpha@example.com',
    recipient_cc: '',
    subject: 'Lease approval sent',
    template_name: 'lease-approved',
    status: 'pending',
    attempt_count: 0,
    max_attempts: 5,
    sent_at: null,
    last_error: null,
    created_at: '2026-04-05T10:00:00Z',
    updated_at: '2026-04-05T10:00:00Z',
  },
  {
    id: 202,
    event_type: 'invoice.sent',
    aggregate_type: 'invoice',
    aggregate_id: 9002,
    recipient_to: 'beta@example.com',
    recipient_cc: 'billing@example.com',
    subject: 'Invoice dispatched',
    template_name: 'invoice-sent',
    status: 'sent',
    attempt_count: 1,
    max_attempts: 5,
    sent_at: '2026-04-10T12:30:00Z',
    last_error: null,
    created_at: '2026-04-10T09:15:00Z',
    updated_at: '2026-04-10T12:30:00Z',
  },
  {
    id: 303,
    event_type: 'invoice.sent',
    aggregate_type: 'invoice',
    aggregate_id: 9003,
    recipient_to: 'gamma@example.com',
    recipient_cc: '',
    subject: 'Invoice retry exhausted',
    template_name: 'invoice-sent',
    status: 'dead',
    attempt_count: 5,
    max_attempts: 5,
    sent_at: null,
    last_error: 'SMTP unavailable',
    created_at: '2026-05-01T08:00:00Z',
    updated_at: '2026-05-01T08:10:00Z',
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

  const wrapper = mount(NotificationsView, {
    global: {
      plugins: [ElementPlus, i18n, pinia],
      stubs: {
        PageSection: PageSectionStub,
        FilterForm: FilterFormStub,
        ElFormItem: ElFormItemStub,
        ElSelect: ElSelectStub,
        ElOption: ElOptionStub,
        ElDatePicker: ElDatePickerStub,
        ElAlert: ElAlertStub,
        ElTag: ElTagStub,
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

describe('NotificationsView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    i18n.global.locale.value = 'en-US'

    const pinia = createPinia()
    setActivePinia(pinia)
    useAppStore().setLocale('en-US')

    vi.mocked(listNotifications).mockResolvedValue({
      data: {
        items: notificationRows,
        total: notificationRows.length,
        page: 1,
        page_size: 20,
      },
    } as never)
  })

  it('loads notifications on mount and renders the table contents', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="notifications-view"]')).toBeTruthy()
    expect(listNotifications).toHaveBeenCalledTimes(1)
    expect(listNotifications).toHaveBeenCalledWith({
      event_type: undefined,
      status: undefined,
      page: 1,
      page_size: 20,
    })

    expect(wrapper.text()).toContain('Lease approval sent')
    expect(wrapper.text()).toContain('Invoice dispatched')
    expect(wrapper.text()).toContain('Invoice retry exhausted')
    expect(wrapper.text()).toContain('alpha@example.com')
    expect(wrapper.text()).toContain('beta@example.com')
    expect(wrapper.text()).toContain('Pending')
    expect(wrapper.text()).toContain('Sent')
    expect(wrapper.text()).toContain('Dead')
    expect(wrapper.text()).toContain('2026/04/05')
    expect(wrapper.text()).toContain('2026/04/10')
    expect(wrapper.text()).toContain('—')
  })

  it('submits filters, applies the local created-at range, and resets state', async () => {
    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="notifications-filter-reset"]').attributes('disabled')).toBeDefined()

    await wrapper.get('[data-testid="notifications-pagination-page-2"]').trigger('click')
    await flushPromises()

    vi.mocked(listNotifications).mockClear()

    await wrapper.get('[data-testid="notifications-event-type-filter"]').setValue('invoice.sent')
    await wrapper.get('[data-testid="notifications-status-filter"]').setValue('sent')
    await wrapper.get('[data-testid="notifications-created-at-range-filter"]').setValue('2026-04-01,2026-04-30')
    await flushPromises()

    expect(wrapper.get('[data-testid="notifications-filter-reset"]').attributes('disabled')).toBeUndefined()

    vi.mocked(listNotifications).mockResolvedValueOnce({
      data: {
        items: [notificationRows[1]],
        total: 1,
        page: 1,
        page_size: 20,
      },
    } as never)

    await wrapper.get('[data-testid="notifications-filter-submit"]').trigger('click')
    await flushPromises()

    expect(listNotifications).toHaveBeenCalledWith({
      event_type: 'invoice.sent',
      status: 'sent',
      page: 1,
      page_size: 20,
    })
    expect(wrapper.text()).toContain('Invoice dispatched')
    expect(wrapper.text()).not.toContain('Lease approval sent')
    expect(wrapper.text()).not.toContain('Invoice retry exhausted')

    vi.mocked(listNotifications).mockClear()

    await wrapper.get('[data-testid="notifications-filter-reset"]').trigger('click')
    await flushPromises()

    expect(listNotifications).toHaveBeenCalledWith({
      event_type: undefined,
      status: undefined,
      page: 1,
      page_size: 20,
    })
    expect((wrapper.get('[data-testid="notifications-event-type-filter"]').element as HTMLSelectElement).value).toBe('')
    expect((wrapper.get('[data-testid="notifications-status-filter"]').element as HTMLSelectElement).value).toBe('')
    expect((wrapper.get('[data-testid="notifications-created-at-range-filter"]').element as HTMLInputElement).value).toBe('')
    expect(wrapper.text()).toContain('Lease approval sent')
    expect(wrapper.text()).toContain('Invoice retry exhausted')
  })

  it('shows an error alert when loading notifications fails', async () => {
    vi.mocked(listNotifications).mockRejectedValue(new Error('Notification request failed'))

    const wrapper = await mountView()

    expect(wrapper.get('[data-testid="notifications-error-alert"]').text()).toContain('Notification history unavailable')
    expect(wrapper.get('[data-testid="notifications-error-alert"]').text()).toContain('Notification request failed')
    expect(wrapper.text()).not.toContain('Lease approval sent')
  })

  it('reloads data when pagination changes', async () => {
    const wrapper = await mountView()

    vi.mocked(listNotifications).mockClear()

    await wrapper.get('[data-testid="notifications-pagination-page-2"]').trigger('click')
    await flushPromises()

    expect(listNotifications).toHaveBeenCalledWith({
      event_type: undefined,
      status: undefined,
      page: 2,
      page_size: 20,
    })

    vi.mocked(listNotifications).mockClear()

    await wrapper.get('[data-testid="notifications-pagination-size-50"]').trigger('click')
    await flushPromises()

    expect(listNotifications).toHaveBeenCalledWith({
      event_type: undefined,
      status: undefined,
      page: 1,
      page_size: 50,
    })
  })
})
