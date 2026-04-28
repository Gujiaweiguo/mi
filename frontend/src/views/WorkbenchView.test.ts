import { mount } from '@vue/test-utils'
import { defineComponent, h } from 'vue'
import { beforeEach, describe, expect, it, vi } from 'vitest'

import { i18n } from '../i18n'
import WorkbenchView from './WorkbenchView.vue'

vi.mock('vue-router', () => ({
  useRouter: () => ({ push: vi.fn() }),
}))

const PageSectionStub = defineComponent({
  props: {
    title: { type: String, required: true },
    summary: { type: String, required: true },
    eyebrow: { type: String, default: '' },
  },
  setup(props, { slots }) {
    return () => h('section', [h('span', props.eyebrow), h('h1', props.title), h('p', props.summary), slots.actions?.()])
  },
})

const ElTagStub = defineComponent({
  setup(_, { slots }) {
    return () => h('span', slots.default?.())
  },
})

const ElButtonStub = defineComponent({
  emits: ['click'],
  setup(_, { emit, slots }) {
    return () => h('button', { onClick: () => emit('click') }, slots.default?.())
  },
})

const ElCardStub = defineComponent({
  setup(_, { slots, attrs }) {
    return () => h('section', attrs, slots.default?.())
  },
})

const ElStatisticStub = defineComponent({
  props: { value: { type: Number, default: null } },
  setup(props) {
    return () => h('span', String(props.value))
  },
})

describe('WorkbenchView', () => {
  beforeEach(() => {
    i18n.global.locale.value = 'en-US'
  })

  it('renders populated queue sections', () => {
    const wrapper = mount(WorkbenchView, {
      props: {
        eyebrow: 'Operations',
        title: 'Workbench',
        summary: 'Daily queues',
        isLoading: false,
        lastUpdatedAt: 'Apr 28, 2026',
        testId: 'workbench-view',
        sections: [
          {
            key: 'pending-approvals',
            title: 'Pending approvals',
            summary: 'Review queued approvals.',
            count: 2,
            routeTarget: '/workflow/admin',
            previewRows: [
              {
                id: 1,
                title: 'Lease LC-001',
                subtitle: 'Tenant A',
                status: 'pending_approval',
                meta: 'Today',
                routeTarget: '/lease/contracts',
              },
            ],
          },
        ],
      },
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElTag: ElTagStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElStatistic: ElStatisticStub,
        },
      },
    })

    expect(wrapper.text()).toContain('Pending approvals')
    expect(wrapper.text()).toContain('Lease LC-001')
    expect(wrapper.text()).toContain('Today')
  })

  it('renders empty queue messaging when a section has no preview rows', () => {
    const wrapper = mount(WorkbenchView, {
      props: {
        eyebrow: 'Operations',
        title: 'Workbench',
        summary: 'Daily queues',
        isLoading: false,
        lastUpdatedAt: 'Apr 28, 2026',
        testId: 'workbench-view',
        sections: [
          {
            key: 'receivables',
            title: 'Receivables',
            summary: 'Follow up collections.',
            count: 0,
            routeTarget: '/billing/receivables',
            previewRows: [],
          },
        ],
      },
      global: {
        plugins: [i18n],
        stubs: {
          PageSection: PageSectionStub,
          ElTag: ElTagStub,
          ElButton: ElButtonStub,
          ElCard: ElCardStub,
          ElStatistic: ElStatisticStub,
        },
      },
    })

    expect(wrapper.get('[data-testid="workbench-view-receivables-empty"]').text()).toContain('No items are waiting in this queue.')
  })
})
