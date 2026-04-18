import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import DashboardView from './DashboardView.vue'

vi.mock('vue-router', () => ({
  useRouter: () => ({ push: vi.fn() }),
}))

vi.mock('../api/dashboard', () => ({
  getDashboardSummary: vi.fn(),
  getEmptyDashboardSummary: vi.fn(() => ({
    activeLeases: null,
    pendingLeaseApprovals: null,
    pendingInvoiceApprovals: null,
    openReceivables: null,
    overdueReceivables: null,
    pendingWorkflows: null,
  })),
}))

import { getDashboardSummary } from '../api/dashboard'

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

const ElTagStub = defineComponent({
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const ElButtonStub = defineComponent({
  props: {
    disabled: { type: Boolean, default: false },
  },
  emits: ['click'],
  setup(props, { emit, slots }) {
    return () =>
      h(
        'button',
        {
          disabled: props.disabled,
          onClick: (event: MouseEvent) => emit('click', event),
        },
        slots.default?.(),
      )
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props) {
    return () => h('div', { 'data-testid': 'dashboard-error-alert' }, `${props.title} ${props.description}`.trim())
  },
})

const ElCardStub = defineComponent({
  setup(_, { slots }) {
    return () => h('section', [h('div', slots.header?.()), h('div', slots.default?.())])
  },
})

const ElStatisticStub = defineComponent({
  props: {
    value: { type: Number, default: null },
  },
  setup(props) {
    return () => h('span', { 'data-testid': 'dashboard-statistic' }, String(props.value))
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('DashboardView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('mounts without error', async () => {
    vi.mocked(getDashboardSummary).mockResolvedValue({
      activeLeases: 1,
      pendingLeaseApprovals: 2,
      pendingInvoiceApprovals: 3,
      openReceivables: 4,
      overdueReceivables: 5,
      pendingWorkflows: 6,
    })

    const wrapper = mount(DashboardView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElTag: ElTagStub,
          ElButton: ElButtonStub,
          ElAlert: ElAlertStub,
          ElCard: ElCardStub,
          ElStatistic: ElStatisticStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="dashboard-view"]')).toBeTruthy()
    expect(wrapper.text()).toContain('Dashboard')
  })

  it('loads dashboard summary on mount and displays returned counts', async () => {
    vi.mocked(getDashboardSummary).mockResolvedValue({
      activeLeases: 12,
      pendingLeaseApprovals: 8,
      pendingInvoiceApprovals: 5,
      openReceivables: 21,
      overdueReceivables: 3,
      pendingWorkflows: 9,
    })

    const wrapper = mount(DashboardView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElTag: ElTagStub,
          ElButton: ElButtonStub,
          ElAlert: ElAlertStub,
          ElCard: ElCardStub,
          ElStatistic: ElStatisticStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(getDashboardSummary).toHaveBeenCalledTimes(1)
    expect(wrapper.text()).toContain('Active leases')
    expect(wrapper.text()).toContain('12')
    expect(wrapper.text()).toContain('Open receivables')
    expect(wrapper.text()).toContain('21')
  })

  it('shows an error alert when the summary request fails', async () => {
    vi.mocked(getDashboardSummary).mockRejectedValue(new Error('Dashboard unavailable'))

    const wrapper = mount(DashboardView, {
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElTag: ElTagStub,
          ElButton: ElButtonStub,
          ElAlert: ElAlertStub,
          ElCard: ElCardStub,
          ElStatistic: ElStatisticStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="dashboard-error-alert"]').text()).toContain('Dashboard data unavailable Dashboard unavailable')
  })
})
