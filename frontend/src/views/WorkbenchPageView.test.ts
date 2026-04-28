import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { defineComponent, h, nextTick } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import { useAppStore } from '../stores/app'
import WorkbenchPageView from './WorkbenchPageView.vue'

vi.mock('../api/dashboard', () => ({
  getWorkbenchAggregate: vi.fn(),
  getEmptyWorkbenchAggregate: vi.fn(() => ({
    pendingApprovals: { count: null, routeTarget: '/workflow/admin', previewRows: [] },
    receivables: { count: null, routeTarget: '/billing/receivables', previewRows: [] },
    overdueReceivables: { count: null, routeTarget: '/billing/receivables', previewRows: [] },
    activeLeases: { count: null, routeTarget: '/lease/contracts', previewRows: [] },
  })),
}))

import { getWorkbenchAggregate } from '../api/dashboard'

const WorkbenchViewStub = defineComponent({
  props: {
    eyebrow: { type: String, required: true },
    title: { type: String, required: true },
    summary: { type: String, required: true },
    sections: { type: Array, required: true },
    isLoading: { type: Boolean, required: true },
    lastUpdatedAt: { type: String, required: true },
    testId: { type: String, required: true },
  },
  emits: ['refresh'],
  setup(props) {
    return () =>
      h('section', { 'data-testid': props.testId }, [
        h('span', props.eyebrow),
        h('h1', props.title),
        h('p', props.summary),
        h('strong', `sections:${props.sections.length}`),
        h('strong', `loading:${String(props.isLoading)}`),
        h('strong', `pending:${(props.sections[0] as { count: number | null }).count}`),
      ])
  },
})

const ElAlertStub = defineComponent({
  props: {
    title: { type: String, default: '' },
    description: { type: String, default: '' },
  },
  setup(props) {
    return () => h('div', { 'data-testid': 'workbench-error-alert' }, `${props.title} ${props.description}`.trim())
  },
})

const flushPromises = async () => {
  await Promise.resolve()
  await nextTick()
  await Promise.resolve()
  await nextTick()
}

describe('WorkbenchPageView', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    i18n.global.locale.value = 'en-US'
    useAppStore().setLocale('en-US')
  })

  it('loads workbench queue data on mount and passes four sections to the shared view', async () => {
    vi.mocked(getWorkbenchAggregate).mockResolvedValue({
      pendingApprovals: {
        count: 2,
        routeTarget: '/workflow/admin',
        previewRows: [{ id: 1, title: 'Lease LC-001', subtitle: 'Tenant A', status: 'pending_approval', meta: 'Today', routeTarget: '/lease/contracts' }],
      },
      receivables: { count: 4, routeTarget: '/billing/receivables', previewRows: [] },
      overdueReceivables: { count: 1, routeTarget: '/billing/receivables', previewRows: [] },
      activeLeases: { count: 5, routeTarget: '/lease/contracts', previewRows: [] },
    })

    const wrapper = mount(WorkbenchPageView, {
      global: {
        plugins: [i18n],
        stubs: {
          WorkbenchView: WorkbenchViewStub,
          ElAlert: ElAlertStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(getWorkbenchAggregate).toHaveBeenCalledTimes(1)
    expect(wrapper.get('[data-testid="workbench-view"]').text()).toContain('Workbench')
    expect(wrapper.get('[data-testid="workbench-view"]').text()).toContain('sections:4')
    expect(wrapper.get('[data-testid="workbench-view"]').text()).toContain('pending:2')
  })

  it('shows an error alert when the workbench request fails', async () => {
    vi.mocked(getWorkbenchAggregate).mockRejectedValue(new Error('Workbench unavailable'))

    const wrapper = mount(WorkbenchPageView, {
      global: {
        plugins: [i18n],
        stubs: {
          WorkbenchView: WorkbenchViewStub,
          ElAlert: ElAlertStub,
        },
        directives: {
          loading: {},
        },
      },
    })

    await flushPromises()

    expect(wrapper.get('[data-testid="workbench-error-alert"]').text()).toContain(
      'Workbench data unavailable Workbench unavailable',
    )
  })
})
